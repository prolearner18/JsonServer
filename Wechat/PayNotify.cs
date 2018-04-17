using System.Web;

namespace Wechat
{
    /// <summary>
    /// 支付结果通知回调处理类
    /// 负责接收微信支付后台发送的支付结果并对订单有效性进行验证，将验证结果反馈给微信支付后台
    /// </summary>
    public class PayNotify : Notify
    {
        private bool _paySuccess = false;

        public bool PaySuccess
        {
            get { return _paySuccess; }
        }

        public ParamsData OrderData { get; set; }

        public PayNotify(HttpContext context)
            : base(context)
        { }

        public override bool ProcessNotify()
        {
            //检查支付结果中transaction_id是否存在
            if (!NotifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                ResponseData.SetValue("return_code", "FAIL");
                ResponseData.SetValue("return_msg", "支付结果中微信订单号不存在");

                Log.Error(this.GetType().ToString(), "The Pay result is error : " + ResponseData.ToXml());

                return false;
            }

            //查询订单，判断订单真实性
            string transaction_id = NotifyData.GetValue("transaction_id").ToString();
            if (!QueryOrder(transaction_id))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                ResponseData.SetValue("return_code", "FAIL");
                ResponseData.SetValue("return_msg", "订单查询失败");

                Log.Error(this.GetType().ToString(), "Order query failure : " + ResponseData.ToXml());

                return false;
            }
            //查询订单成功
            else
            {
                ResponseData.SetValue("return_code", "SUCCESS");
                ResponseData.SetValue("return_msg", "OK");

                Log.Info(this.GetType().ToString(), "order query success : " + ResponseData.ToXml());

                return true;
            }
        }

        //查询订单
        private bool QueryOrder(string transaction_id)
        {
            var req = new ParamsData();
            req.SetValue("transaction_id", transaction_id);

            var res = PayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                _paySuccess = true;

                OrderData = res;

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
