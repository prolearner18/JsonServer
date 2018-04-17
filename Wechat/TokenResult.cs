using System;

namespace Wechat
{
    [Serializable]
    public class TokenResult : BaseResult
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }
    }
}
