using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public class HttpClientHelper
    {
        private HttpClient _httpClient;

        public HttpClientHelper(string baseUrl)
        {

            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip,
            };
            HttpClient client = new HttpClient(handler);

            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(baseUrl);

        }


        public string Get(Dictionary<string, string> parameters, string requestUri)
        {
            if (parameters == null)
            {
                requestUri = ConcatURL(requestUri);
            }
            else
            {
                var starParam = string.Join("&", parameters.Select(o => o.Key + "=" + o.Value));
                requestUri = string.Concat(ConcatURL(requestUri), '?', starParam);
            }
            var result = _httpClient.GetStringAsync(requestUri);
            return result.Result;
        }

        public string Get(Dictionary<string, string> parameters, HttpHeaders headers, string requestUri)
        {
            if (parameters == null)
            {
                requestUri = ConcatURL(requestUri);
            }
            else
            {
                var starParam = string.Join("&", parameters.Select(o => o.Key + "=" + o.Value));
                requestUri = string.Concat(ConcatURL(requestUri), '?', starParam);
            }

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }

            var result = _httpClient.GetStringAsync(requestUri);

            return result.Result;
        }

        public string Post(string requestUri, object entity)
        {
            string request = string.Empty;
            if (entity != null)
                request = entity.ToJson();

            HttpContent httpContent = new StringContent(request);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return Post(requestUri, httpContent);
        }

        public string Post(string requestUri, object entity, HttpHeaders headers)
        {
            string request = string.Empty;
            if (entity != null)
                request = entity.ToJson();

            HttpContent httpContent = new StringContent(request);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
            return Post(requestUri, httpContent);
        }

        private string Post(string requestUrl, HttpContent content)
        {
            var result = _httpClient.PostAsync(ConcatURL(requestUrl), content);
            return result.Result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// 把请求的URL相对路径组合成绝对路径 
        /// </summary>
        private string ConcatURL(string requestUrl)
        {
            return new Uri(_httpClient.BaseAddress, requestUrl).OriginalString;
        }
    }
}
