using System;

namespace Wechat
{
    [Serializable]
    public class AccessTokenResult : BaseResult
    {
        /// <summary>
        /// 网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
        /// </summary>
        public string Access_token { get; set; }

        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public int Expires_in { get; set; }

        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string Refresh_token { get; set; }

        /// <summary>
        /// 用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
        /// </summary>
        public string Openid { get; set; }

        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        public string Scope { get; set; }
    }
}
