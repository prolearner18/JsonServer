using System;

namespace Wechat
{
    public class JsApiPay
    {
        public enum TRADE_TYPE
        {
            /// <summary>
            /// 公众号支付
            /// </summary>
            JSAPI,
            /// <summary>
            /// 原生扫码支付
            /// </summary>
            NATIVE,
            /// <summary>
            /// app支付
            /// </summary>
            APP
        }

        /// <summary>
        /// trade_type=JSAPI时（即公众号支付），此参数必传，此参数为微信用户在商户对应appid下的唯一标识。
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 订单总金额，单位为分
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 商户系统内部订单号，要求32个字符内、且在同一个商户号下唯一。
        /// </summary>
        public string out_trade_no { get; set; }

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

        public TRADE_TYPE trade_type { get; set; }

        /// <summary>
        /// trade_type=NATIVE时（即扫码支付），此参数必传。此参数为二维码中包含的商品ID，商户自行定义。
        /// </summary>
        public string product_id { get; set; }

        public string notify_url { get; set; }

        /// <summary>
        /// 统一下单接口返回结果
        /// </summary>
        public ParamsData unifiedOrderResult { get; set; }


        /**
         * 调用统一下单，获得下单结果
         * @return 统一下单结果
         * @失败时抛异常WxPayException
         */
        public ParamsData GetUnifiedOrderResult()
        {
            if (String.IsNullOrEmpty(notify_url))
            {
                notify_url = Config.NOTIFY_URL;
            }

            //统一下单
            var data = new ParamsData();
            data.SetValue("device_info", "WEB");
            data.SetValue("body", "文化广场会员订单");
            data.SetValue("attach", "");    //附加数据，在查询API和支付通知中原样返回，可作为自定义参数使用。
            data.SetValue("out_trade_no", out_trade_no);    //商户系统内部订单号，要求32个字符内、且在同一个商户号下唯一。
            data.SetValue("total_fee", total_fee);  //订单总金额，单位为分
            if (!String.IsNullOrEmpty(time_expire))
            {
                data.SetValue("time_expire", time_expire);
            }
            else if (expire_minutes > 5)
            {
                data.SetValue("time_expire", DateTime.Now.AddMinutes(expire_minutes).ToString("yyyyMMddHHmmss"));
            }
            data.SetValue("trade_type", trade_type.ToString());
            data.SetValue("openid", openid);
            data.SetValue("product_id", product_id == null ? "" : product_id);
            data.SetValue("notify_url", notify_url);

            var result = PayApi.UnifiedOrder(data);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new Exception("UnifiedOrder response error!");
            }

            unifiedOrderResult = result;
            return result;
        }

        /**
        *  
        * 从统一下单成功返回的数据中获取微信浏览器调起jsapi支付所需的参数，
        * 微信浏览器调起JSAPI时的输入参数格式如下：
        * {
        *   "appId" : "wx2421b1c4370ec43b",     //公众号名称，由商户传入     
        *   "timeStamp":" 1395712654",         //时间戳，自1970年以来的秒数     
        *   "nonceStr" : "e61463f8efa94090b1f366cccfbbb444", //随机串     
        *   "package" : "prepay_id=u802345jgfjsdfgsdg888",     
        *   "signType" : "MD5",         //微信签名方式:    
        *   "paySign" : "70EA570631E4BB79628FBCA90534C63FF7FADD89" //微信签名 
        * }
        * @return string 微信浏览器调起JSAPI时的输入参数，json格式可以直接做参数用
        * 更详细的说明请参考网页端调起支付API：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7
        * 
        */
        public string GetJsApiParameters()
        {
            Log.Debug(this.GetType().ToString(), "JsApiPay::GetJsApiParam is processing...");

            var jsApiParam = new ParamsData();
            jsApiParam.SetValue("appId", unifiedOrderResult.GetValue("appid"));
            jsApiParam.SetValue("timeStamp", PayApi.GenerateTimeStamp());
            jsApiParam.SetValue("nonceStr", PayApi.GenerateNonceStr());
            jsApiParam.SetValue("package", "prepay_id=" + unifiedOrderResult.GetValue("prepay_id"));
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign());

            string parameters = jsApiParam.ToJson();

            Log.Debug(this.GetType().ToString(), "Get jsApiParam : " + parameters);
            return parameters;
        }
    }
}
