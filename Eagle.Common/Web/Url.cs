using Eagle.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Common.Web
{
    public static class Url
    {
        /// <summary>
        /// 构建完整的Url带query string编码
        /// </summary>
        public static string DataToUrl(string url, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            bool first = true;
            var urlBuilder = new StringBuilder(url);

            foreach (var item in parameters)
            {
                if (first)
                {
                    urlBuilder.Append('?');
                    first = false;
                }
                else
                {
                    urlBuilder.Append('&');
                }
                urlBuilder.Append(Uri.EscapeDataString(item.Key) + "=" + Uri.EscapeDataString(item.Value));
            }
            return urlBuilder.ToString();
        }

        /// <summary>
        /// 解码Url带query string
        /// </summary>
        public static Tuple<string, IEnumerable<KeyValuePair<string, string>>> UrlToData(string url)
        {
            if (!url.HasValue())
            {
                throw new ArgumentNullException("url");
            }

            url = url.Trim();

            try
            {
                var splittedUrl = url.Split(new[] { '?', '&' }, StringSplitOptions.RemoveEmptyEntries);

                if (splittedUrl.Length == 1)
                {
                    return new Tuple<string, IEnumerable<KeyValuePair<string, string>>>(url, null);
                }

                //获取前面的URL地址
                var host = splittedUrl[0];

                var pairs = splittedUrl.Skip(1).Select(s =>
                {
                    //没有用String.Split防止某些少见Query String中出现多个=，要把后面的无法处理的=全部显示出来
                    var idx = s.IndexOf('=');
                    return new KeyValuePair<string, string>(Uri.UnescapeDataString(s.Substring(0, idx)), Uri.UnescapeDataString(s.Substring(idx + 1)));

                }).ToList();

                return new Tuple<string, IEnumerable<KeyValuePair<string, string>>>(host, pairs);
            }
            catch (Exception ex)
            {
                throw new FormatException("URL格式错误", ex);
            }
        }

    }
}
