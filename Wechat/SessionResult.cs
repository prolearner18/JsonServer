using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wechat
{
    public class SessionResult : BaseResult
    {
        /// <summary>
        /// 例子json+1:{"session_key":"3pEs7+45ZBNA7AKa7ON9wg==","expires_in":7200,"openid":"oUgYS1cOV14ZqErd49rGln4bLPU8"}
        /// </summary>
        public string session_key{get;set;}
        public int expires_in{get;set;}
        public string openid{get;set;}
    }
}
