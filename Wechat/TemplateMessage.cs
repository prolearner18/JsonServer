using System;

namespace Wechat
{
    public class TemplateMessage
    {
        public string touser { get; set; }

        public string template_id { get; set; }

        public string url { get; set; }

        public object data { get; set; }

        public static void SendPendingWorkNotify(string toUser, string url, string title, string number, string summary, DateTime time, string remark)
        {
            var message = new TemplateMessage()
            {
                touser = toUser,
                url = url,
                template_id = "FSRsuS7dg7JJubGHbyLWMJYhtprfiS6MQcbodJ7Vs1A",
                data = new
                {
                    first = new
                    {
                        value = title,
                        color = "#173177"
                    },
                    keyword1 = new
                    {
                        value = number,
                        color = "#173177"
                    },
                    keyword2 = new
                    {
                        value = summary,
                        color = "#173177"
                    },
                    keyword3 = new
                    {
                        value = time.ToString("yyyy-MM-dd HH:mm"),
                        color = "#173177"
                    },
                    remark = new
                    {
                        value = remark,
                        color = "#173177"
                    }
                }
            };

            var messageBody = Newtonsoft.Json.JsonConvert.SerializeObject(message);
            var accessToken = WebSDK.ACCESS_TOKEN;
            var gateway = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + accessToken;
            try
            {
                HttpService.Post(messageBody, gateway);
            }
            catch { }
        }
    }

    public class TemplateMessageResult : BaseResult
    {
        public string msgid { get; set; }
    }
}
