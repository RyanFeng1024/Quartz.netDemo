using System;
using System.IO;
using System.Net;
using System.Text;

namespace Quartz.Net.Demo.Codes
{
    public class HttpUtils
    {

        public static string HttpPost(string url, string data = "", string contentType = "application/x-www-form-urlencoded")
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };    // 忽略证书错误
            byte[] buffer = Encoding.Default.GetBytes(data);
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "post";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = contentType;
                request.Proxy = null;
                request.KeepAlive = false;
                request.Timeout = 30000;
                request.ContentLength = buffer.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Log.Error("POST error:" + ex.Message, ex);
                return "";
            }
        }

        public static string HttpGet(string url)
        {
            return HttpGet(url, Encoding.UTF8);
        }

        public static string HttpGet(string url, Encoding encoding)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Timeout = 30000;
                var response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Log.Error("HttpGet error:" + ex.Message, ex);
                return "";
            }
        }

    }
}