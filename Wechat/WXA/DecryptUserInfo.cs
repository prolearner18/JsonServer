using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Wechat.WXA
{
    public class DecryptUserInfo
    {
        #region 微信小程序用户数据验证
        /// <summary>
        /// 微信小程序用户数据验证
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="rawData">用户信息json</param>
        /// <param name="signature">校验串</param>
        /// <returns></returns>
        public static bool DataCheck(string sessionKey, string rawData, string signature)
        {
            if ((string.IsNullOrWhiteSpace(sessionKey) && string.IsNullOrWhiteSpace(rawData)) || string.IsNullOrWhiteSpace(signature))
                return false;
            return signature == SHA1_Hash(rawData + sessionKey);
        }
        #endregion

        #region 微信小程序用户数据解密
        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="inputdata">输入的数据encryptedData</param>  
        /// <param name="AesKey">sessionKey</param>  
        /// <param name="AesIV">iv向量128</param>  
        /// <returns name="result">解密后的字符串</returns>  
        public static UserInfo AESDecrypt(string inputdata, string aesKey, string aesIV)
        {
            try
            {
                aesIV = aesIV.Replace(" ", "+");
                aesKey = aesKey.Replace(" ", "+");
                inputdata = inputdata.Replace(" ", "+");
                byte[] encryptedData = Convert.FromBase64String(inputdata);
                RijndaelManaged rijndaelCipher = new RijndaelManaged()
                {
                   Key = Convert.FromBase64String(aesKey), // Encoding.UTF8.GetBytes(AesKey);  
                IV = Convert.FromBase64String(aesIV),// Encoding.UTF8.GetBytes(AesIV);  
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };
             
                ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
                byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                string result = Encoding.UTF8.GetString(plainText);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfo>(result);
            }
            catch (Exception ex)
            {
                Log.Error("DecryptUserInfo.AESDecrypt", ex.ToString());
                return null;
            }
        }
        #endregion

        public class UserInfo
        {
            public string OpenId { get; set; }
            public string NickName { get; set; }
            public int Gender { get; set; }
            public string City { get; set; }
            public string Province { get; set; }
            public string Country { get; set; }
            public string AvatarUrl { get; set; }
            public string UnionId { get; set; }
            public Watermark WaterMark { get; set; }
        }
        public class Watermark
        {
            public string Appid { get; set; }
            public long TimeStamp { get; set; }
        }

        #region 验证串获取函数(不含-)
        //MD5
        static public string MD5_Hash(string str_md5_in)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            //byte[] bytes_md5_in = UTF8Encoding.Default.GetBytes(str_md5_in);
            byte[] bytes_md5_in = Encoding.UTF8.GetBytes(str_md5_in);
            byte[] bytes_md5_out = md5.ComputeHash(bytes_md5_in);
            string str_md5_out = BitConverter.ToString(bytes_md5_out);
            str_md5_out = str_md5_out.Replace("-", "").ToLower();
            return str_md5_out;
        }
        //SHA1
        static public string SHA1_Hash(string str_sha1_in)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            //byte[] bytes_sha1_in = UTF8Encoding.Default.GetBytes(str_sha1_in);
            byte[] bytes_sha1_in = Encoding.UTF8.GetBytes(str_sha1_in);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
            str_sha1_out = str_sha1_out.Replace("-", "").ToLower();
            return str_sha1_out;


            ////创建SHA1签名类
            //SHA1 sha1 = new SHA1CryptoServiceProvider();
            ////编码用于SHA1验证的源数据
            //byte[] source = Encoding.UTF8.GetBytes(rawData + sessionKey);
            ////生成签名
            //byte[] target = sha1.ComputeHash(source);
            ////转化为string类型，注意此处转化后是中间带短横杠的大写字母，需要剔除横杠转小写字母
            //string result = BitConverter.ToString(target).Replace("-", "").ToLower();
            ////比对，输出验证结果
            //return signature == result;
        }
        #endregion
    }
}
