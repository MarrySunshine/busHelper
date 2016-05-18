using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EasyNavigator.Libs
{
    public class Request
    {
        private HttpClient client;
        public HttpClient Client { get { return client; } }
        private HttpRequestHeaders headers;
        public HttpRequestHeaders Headers { get { return headers; } }
        public delegate void RequestSuccessAction(HttpResponseHeaders responseHeaders, string body);
        public delegate void RequestErrorAction(HttpResponseHeaders responseHeaders, HttpStatusCode statusCode);
        
        public Request()
        {
            var handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.UseCookies = false;

            client = new HttpClient(handler);
            headers = client.DefaultRequestHeaders;
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            headers.UserAgent.TryParseAdd("ie Mozilla/5.0 (Windows NT 6.2; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0");

        }
        public async Task getAsync(string uri, List<KeyValuePair<string, string>> paramsList = null, RequestSuccessAction successAction = null, RequestErrorAction errorAction = null)
        {
            StringBuilder uriWithParams = new StringBuilder(uri);
            if (paramsList != null && paramsList.Count != 0)
            {
                uriWithParams.Append('?');
                foreach (var item in paramsList)
                {
                    uriWithParams.Append($"{item.Key}={item.Value}&");
                }
                uriWithParams.Remove(uriWithParams.Length - 1, 1);
            }

            handleResponse(await client.GetAsync(uriWithParams.ToString()), successAction, errorAction);
        }
        public async Task postAsync(string uri, List<KeyValuePair<string, string>> body, RequestSuccessAction successAction = null, RequestErrorAction errorAction = null)
        {
            handleResponse(await client.PostAsync(uri, new FormUrlEncodedContent(body)), successAction, errorAction);
        }

        private async void handleResponse(HttpResponseMessage response, RequestSuccessAction successAction, RequestErrorAction errorAction)
        {
            if (response.EnsureSuccessStatusCode().IsSuccessStatusCode && successAction != null)
            {
                successAction(response.Headers, Encoding.UTF8.GetString(await response.Content.ReadAsByteArrayAsync()));
            }
            else
            {
                if (errorAction != null)
                {
                    errorAction(response.Headers, response.StatusCode);
                }
            }
        }
    }
}
