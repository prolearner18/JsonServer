using System;
using System.Web;

namespace Wechat
{
    /// <summary>
    /// 扫码支付模式一回调处理类
    /// 接收微信支付后台发送的扫码结果，调用统一下单接口并将下单结果返回给微信支付后台
    /// </summary>
    public class NativeNotify : Notify
    {
        public string openId { get; set; }

        /// <summary>
        /// trade_type=NATIVE时（即扫码支付），此参数必传。此参数为二维码中包含的商品ID，商户自行定义。
        /// </summary>
        public string product_id { get; set; }

        /// <summary>
        /// 订单总金额，单位为分
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 支付失效时间（分钟）
        /// </summary>
        public double expire_minutes { get; set; }

        /// <summary>
        /// 订单生成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010。
        /// </summary>
        public string time_start { get; set; }

        /// <summary>
        /// 订单失效时间，格式为yyyyMMddHHmmss，如2009年12月27日9点10分10秒表示为20091227091010，注意：最短失效时间间隔必须大于5分钟
        /// </summary>
        public string time_expire { get; set; }

        public string notify_url { get; set; }

        public NativeNotify(HttpContext context)
            : base(context)
        { }

        public override bool ProcessNotify()
        {
            var notifyData = NotifyData;

            //检查openid和product_id是否返回
            if (!notifyData.IsSet("openid") || !notifyData.IsSet("product_id"))
            {
                ResponseData.SetValue("return_code", "FAIL");
                ResponseData.SetValue("return_msg", "回调数据异常");

                Log.Info(this.GetType().ToString(), "The data WeChat post is error : " + ResponseData.ToXml());

                return false;
            }

            product_id = notifyData.GetValue("product_id").ToString();
            openId = notifyData.GetValue("openid").ToString();

            return true;
        }

        public ParamsData UnifiedOrder()
        {
            if (String.IsNullOrEmpty(notify_url))
            {
                notify_url = Config.NOTIFY_URL;
            }

            //统一下单
            var req = new ParamsData();
            req.SetValue("body", "E717平台订单");
            req.SetValue("attach", "");
            req.SetValue("out_trade_no", product_id);
            req.SetValue("total_fee", total_fee);
            if (!String.IsNullOrEmpty(time_expire))
            {
                req.SetValue("time_expire", time_expire);
            }
            else if (expire_minutes > 5)
            {
                req.SetValue("time_expire", DateTime.Now.AddMinutes(expire_minutes).ToString("yyyyMMddHHmmss"));
            }
            req.SetValue("goods_tag", "");
            req.SetValue("trade_type", "NATIVE");
            req.SetValue("openid", openId);
            req.SetValue("product_id", product_id);
            req.SetValue("notify_url", notify_url);

            return PayApi.UnifiedOrder(req);
        }
    }
}
