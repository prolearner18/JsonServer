using System;
using System.Collections.Generic;

namespace Wechat
{
    public class NativePay
    {
        /**
        * 生成扫描支付模式一URL
        * @param productId 商品ID或订单号
        * @return 模式一URL
        */
        public string GetPrePayUrl(string productId)
        {
            Log.Info(this.GetType().ToString(), "Native pay mode 1 url is producing...");

            var data = new ParamsData();
            data.SetValue("appid", Config.APPID);//公众帐号id
            data.SetValue("mch_id", Config.MCHID);//商户号
            data.SetValue("time_stamp", PayApi.GenerateTimeStamp());//时间戳
            data.SetValue("nonce_str", PayApi.GenerateNonceStr());//随机字符串
            data.SetValue("product_id", productId);//商品ID
            data.SetValue("sign", data.MakeSign());//签名
            string str = ToUrlParams(data.GetValues());//转换为URL串
            string url = "weixin://wxpay/bizpayurl?" + str;

            Log.Info(this.GetType().ToString(), "Get native pay mode 1 url : " + url);
            return url;
        }

        /**
        * 生成直接支付url，支付url有效期为2小时,模式二
        * @param productId 商品ID
        * @return 模式二URL
        */
        public string GetPayUrl(string out_trade_no)
        {
            Log.Info(this.GetType().ToString(), "Native pay mode 2 url is producing...");

            var data = new ParamsData();
            data.SetValue("body", "E717平台订单");//商品描述
            data.SetValue("attach", "");//附加数据
            data.SetValue("out_trade_no", out_trade_no);//随机字符串
            data.SetValue("total_fee", 1);//总金额
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
            data.SetValue("goods_tag", "");//商品标记
            data.SetValue("trade_type", "NATIVE");//交易类型
            data.SetValue("product_id", out_trade_no);//商品ID

            var result = PayApi.UnifiedOrder(data);//调用统一下单接口
            string url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接

            Log.Info(this.GetType().ToString(), "Get native pay mode 2 url : " + url);
            return url;
        }

        /**
        * 参数数组转换为url格式
        * @param map 参数名与参数值的映射表
        * @return URL字符串
        */
        private string ToUrlParams(SortedDictionary<string, object> map)
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in map)
            {
                buff += pair.Key + "=" + pair.Value + "&";
            }
            buff = buff.Trim('&');
            return buff;
        }
    }
}
