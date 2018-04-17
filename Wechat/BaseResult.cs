using System;

namespace Wechat
{
    [Serializable]
    public abstract class BaseResult
    {
        public int errcode { get; set; }

        public string errmsg { get; set; }
    }
}
