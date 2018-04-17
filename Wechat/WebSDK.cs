using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
namespace Wechat
{
    public class WebSDK
    {
        #region 属性 及 构造函数

        private const string cache_key_access_token = "weixin_access_token";
        private const string cache_key_access_ticket = "weixin_jsapi_ticket";
        private static string _local_api_getway;
        private static string _weixin_sdk_mode;

        private static string APPID { get; set; }

        private static string APPSECRET { get; set; }

        public static string ACCESS_TOKEN
        {
            get
            {
                var cache = HttpRuntime.Cache.Get(cache_key_access_token);
                var token = cache == null ? "" : cache.ToString();
                if (token.Length == 0)
                {
                    token = Get_access_token();
                }
                return token;
            }
        }

        public static string JSAPI_TICKET
        {
            get
            {
                var cache = HttpRuntime.Cache.Get(cache_key_access_ticket);
                var ticket = cache == null ? "" : cache.ToString();
                if (ticket.Length == 0)
                {
                    ticket = Get_jsapi_ticket();
                }
                return ticket;
            }
        }

        /// <summary>
        /// 网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
        /// </summary>
        public AccessTokenResult AccessToken { get; set; }

        static WebSDK()
        {
            APPID = Config.APPID;
            APPSECRET = Config.APPSECRET;
            _weixin_sdk_mode = ConfigurationManager.AppSettings["weixin.sdk.mode"];
            _local_api_getway = ConfigurationManager.AppSettings["ApiGetway"];
        }

        #endregion

        #region  网页账号

        public void GetOpenidAndAccessToken(HttpContext context, string redirectUrl, string state = "STATE", string scope = "snsapi_base")
        {
            if (!string.IsNullOrEmpty(context.Request.QueryString["code"]))
            {
                //获取code码，以获取openid和网页授权access_token
                string code = context.Request.QueryString["code"];
                GetOpenidAndAccessTokenFromCode(code);
            }
            else
            {
                //构造网页授权获取code的URL
                //string host = context.Request.Url.Host;
                //string path = context.Request.Path;
                //string redirect_uri = HttpUtility.UrlEncode("http://" + host + path);
                if (String.IsNullOrEmpty(redirectUrl))
                {
                    redirectUrl = context.Request.Url.AbsoluteUri.ToString();
                }
                string redirect_uri = HttpUtility.UrlEncode(redirectUrl);
                var data = new ParamsData();
                data.SetValue("appid", APPID);
                data.SetValue("redirect_uri", redirect_uri);
                data.SetValue("response_type", "code");
                data.SetValue("scope", scope);
                data.SetValue("state", state + "#wechat_redirect");
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + data.ToUrl();

                context.Response.Redirect(url);
            }
        }

        private void GetOpenidAndAccessTokenFromCode(string code)
        {
            var json = HttpGet<AccessTokenResult>("https://api.weixin.qq.com/sns/oauth2/access_token", PrepareParameters(new Dictionary<string, object>() {
                { "appid", APPID },
                { "secret", APPSECRET },
                { "code", code },
                { "grant_type", "authorization_code" }
            }));

            AccessToken = json;
        }

        #endregion

        #region  小程序码
        public static byte[] GetAcode(string token, string scene, string page, string width, string auto_color, string line_color)
        {
            var Param = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(scene))
                Param.Add("scene", scene);
            if (!string.IsNullOrEmpty(page))
                Param.Add("page", page);
            if (!string.IsNullOrEmpty(width))
                Param.Add("width", width);
            if (!string.IsNullOrEmpty(auto_color))
                Param.Add("auto_color", auto_color);
            if (!string.IsNullOrEmpty(line_color))
                Param.Add("line_color", line_color);
            var json = HttpPost("https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + token, PrepareParameters(Param));

            return json;
        }
        #endregion
        #region  小程序码流
        public static Stream GetAcodeStream(string token, string scene, string page, string width, string auto_color, string line_color)
        {
            var Param = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(scene))
                Param.Add("scene", scene);
            if (!string.IsNullOrEmpty(page))
                Param.Add("page", page);
            if (!string.IsNullOrEmpty(width))
                Param.Add("width", width);
            if (!string.IsNullOrEmpty(auto_color))
                Param.Add("auto_color", auto_color);
            if (!string.IsNullOrEmpty(line_color))
                Param.Add("line_color", line_color);
            var json = HttpPostStream("https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + token, PrepareParameters(Param));

            return json;
        }
        #endregion
        #region  获取头像
        public static byte[] GetAvatar(string path)
        {
            var json = HttpGetImg(path);
            return json; 
        }
        #endregion
        #region  获取头像流
        public static Stream GetAvatarStream(string path)
        {
            var json = HttpGetImgStream(path);
            return json; 
        }
        #endregion
        #region 辅助方法（内部使用）

        #region 微信小程序sessionKey获取
        public static SessionResult JsCode2SessionByCache(string openID)
        {
            return (SessionResult)HttpRuntime.Cache.Get(openID);
        }
        public static SessionResult JsCode2SessionByCGI(string code)
        {
            //从微信接口获取
            var json = HttpGet<SessionResult>("https://api.weixin.qq.com/sns/jscode2session", PrepareParameters(new Dictionary<string, object>() {
                    { "appid", APPID },
                    { "secret", APPSECRET },
                    { "js_code", code },
                    { "grant_type", "authorization_code" }
                }));
            if (!String.IsNullOrEmpty(json.session_key))
            {
                HttpRuntime.Cache.Remove(json.openid);
                HttpRuntime.Cache.Insert(json.openid, json, null, DateTime.UtcNow.AddSeconds(json.expires_in - 300), Cache.NoSlidingExpiration);
            }
            return json;
        } 
        #endregion

        private static string Get_access_token()
        {
            if (_weixin_sdk_mode != null && _weixin_sdk_mode.Equals("local", StringComparison.OrdinalIgnoreCase))
            {
                //本地模式
                var getway = String.Format("{0}/api/wechat/get_access_token", _local_api_getway);
                using (var wc = new WebClient())
                {
                    return wc.DownloadString(getway);
                }
            }
            else
            {
                //从微信接口获取
                var json = HttpGet<TokenResult>("https://api.weixin.qq.com/cgi-bin/token", PrepareParameters(new Dictionary<string, object>() {
                    { "appid", APPID },
                    { "secret", APPSECRET },
                    { "grant_type", "client_credential" }
                }));
                if (!String.IsNullOrEmpty(json.access_token))
                {
                    HttpRuntime.Cache.Insert(cache_key_access_token, json.access_token, null, DateTime.UtcNow.AddSeconds(json.expires_in - 300), Cache.NoSlidingExpiration);
                }
                return json.access_token;
            }
        }

        private static string Get_jsapi_ticket()
        {
            if (_weixin_sdk_mode != null && _weixin_sdk_mode.Equals("local", StringComparison.OrdinalIgnoreCase))
            {
                //本地模式
                var getway = String.Format("{0}/api/wechat/get_jsapi_ticket", _local_api_getway);
                using (var wc = new WebClient())
                {
                    return wc.DownloadString(getway);
                }
            }
            else
            {
                //从微信API获取
                var json = HttpGet<TicketResult>("https://api.weixin.qq.com/cgi-bin/ticket/getticket", PrepareParameters(new Dictionary<string, object>() {
                    { "access_token", ACCESS_TOKEN },
                    { "type", "jsapi" }
                }));
                if (json.errcode == 0)
                {
                    HttpRuntime.Cache.Insert(cache_key_access_ticket, json.ticket, null, DateTime.UtcNow.AddSeconds(json.expires_in - 300), Cache.NoSlidingExpiration);
                }
                return json.ticket;
            }
        }

        private static Dictionary<string, object> PrepareParameters(Dictionary<string, object> parameters)
        {
            var internalParameters = parameters;
            if (internalParameters == null)
                internalParameters = new Dictionary<string, object>();
            return internalParameters;
        }

        private static T HttpGet<T>(string url, Dictionary<string, object> args)
        {
            var s = from item in args
                    select String.Format("{0}={1}", item.Key, HttpUtility.UrlEncode(item.Value.ToString()));
            if (args.Count > 0)
            {
                url = url + "?" + String.Join("&", s.ToArray());
            }
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            using (var sr = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                var str = sr.ReadToEnd();
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                return serializer.Deserialize<T>(str);
            }
        }
        private static Stream HttpPostStream(string url, Dictionary<string, object> args)
        {
            var s = from item in args
                    select String.Format("\"{0}\":\"{1}\"", item.Key, item.Value.ToString());
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            Stream stream = null;//用于传参数的流  

            string parameters = "{" + String.Join(",", s.ToArray()) + "}";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(parameters);
            request.ContentLength = data.Length;
            stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            Stream returnStream;
            returnStream = response.GetResponseStream();
            return returnStream;
        }
        private static Stream HttpGetImgStream(string url)
        {
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "image/jpeg";
            return request.GetResponse().GetResponseStream();
        }
        private static byte[] HttpPost(string url, Dictionary<string, object> args)
        {
            var s = from item in args
                    select String.Format("\"{0}\":\"{1}\"", item.Key, HttpUtility.UrlEncode(item.Value.ToString()));
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            Stream stream = null;//用于传参数的流  

            string parameters = "{" + String.Join(",", s.ToArray()) + "}";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(parameters);
            request.ContentLength = data.Length;
            stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            using (var sr = request.GetResponse().GetResponseStream())
            {
                var ms = StreamToMemoryStream(sr);
                ms.Seek(0, SeekOrigin.Begin); int buffsize = (int)ms.Length; //rs.Length 此流不支持查找,先转为MemoryStream
                byte[] bytes = new byte[buffsize];
                ms.Read(bytes, 0, buffsize);
                sr.Dispose();
                ms.Dispose();
                return bytes;
            }
        }
        private static byte[] HttpGetImg(string url)
        {
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "image/jpeg";
            using (var sr = request.GetResponse().GetResponseStream())
            {
                var ms = StreamToMemoryStream(sr);
                ms.Seek(0, SeekOrigin.Begin); int buffsize = (int)ms.Length; //rs.Length 此流不支持查找,先转为MemoryStream
                byte[] bytes = new byte[buffsize];
                ms.Read(bytes, 0, buffsize);
                sr.Dispose();
                ms.Dispose();
                return bytes;
            }
        }
        private static MemoryStream StreamToMemoryStream(Stream instream)
        {
            MemoryStream outstream = new MemoryStream();
            const int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int count = 0;
            while ((count = instream.Read(buffer, 0, bufferLen)) > 0)
            {
                outstream.Write(buffer, 0, count);
            }
            return outstream;
        }
        #endregion
    }
}
