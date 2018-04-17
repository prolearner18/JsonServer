using System;
using System.Text;
using System.Web;

namespace Wechat
{
    /// <summary>
    /// 回调处理基类
    /// 主要负责接收微信支付后台发送过来的数据，对数据进行签名验证
    /// 子类在此类基础上进行派生并重写自己的回调处理过程
    /// </summary>
    public abstract class Notify
    {
        private HttpContext _context;
        private ParamsData _notifyData;
        private ParamsData _responseData;

        public ParamsData NotifyData
        {
            get { return _notifyData; }
        }

        public ParamsData ResponseData
        {
            get { return _responseData; }
        }

        public Notify(HttpContext context)
        {
            _context = context;
            _responseData = new ParamsData();
            _notifyData = GetNotifyData();
        }

        /// <summary>
        /// 接收从微信支付后台发送过来的数据并验证签名
        /// </summary>
        /// <returns>微信支付后台返回的数据</returns>
        private ParamsData GetNotifyData()
        {
            //接收从微信后台POST过来的数据
            System.IO.Stream s = _context.Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            Log.Info(this.GetType().ToString(), "Receive data from WeChat : " + builder.ToString());

            //转换数据格式并验证签名
            var data = new ParamsData();
            try
            {
                data.FromXml(builder.ToString());

                Log.Info(this.GetType().ToString(), "Check sign success");
            }
            catch (Exception ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                _responseData.SetValue("return_code", "FAIL");
                _responseData.SetValue("return_msg", ex.Message);

                Log.Error(this.GetType().ToString(), "Sign check error : " + _responseData.ToXml());
            }

            return data;
        }

        //派生类需要重写这个方法，进行不同的回调处理
        public abstract bool ProcessNotify();
    }
}
