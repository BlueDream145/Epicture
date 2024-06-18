using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;

namespace epicture.Network
{
    public class ClientNetwork
    {

        #region "Variables"

        public ClientGetter Getter { get; set; }

        public Engine Engine { get; set; }

        public HttpResponseMessage Response { get; set; }

        #endregion

        #region "Builder"

        public ClientNetwork(Engine _engine)
        {
            Engine = _engine;
            Getter = new ClientGetter(this);
        }

        #endregion

        #region "Methods"

        public string ConcatUri(Dictionary<string, string> values, string url)
        {
            try
            {
                var postData = "";
                int index = 0;

                foreach (KeyValuePair<string, string> pair in values)
                {
                    if (index != 0)
                        postData += "&";
                    postData += pair.Key + "=" + Uri.EscapeDataString(pair.Value);
                    index++;
                }
                if (index != 0)
                    url += "?" + postData;
                return (url);
            } catch { return (url); }
        }

        public async void MakePostRequest(StringContent values, string url)
        {
            HttpClient client = new HttpClient();
            Response = await client.PostAsync(url, values);
        }

        public string MakeGetRequest(Dictionary<string, string> values, string url, bool concat = false)
        {
            HttpWebResponse response;
            HttpWebRequest request;
            string res;

            try
            {
                if (concat)
                    url = ConcatUri(values, url);
                request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "GET";
                request.Headers.Add(HttpRequestHeader.CacheControl, "no-cache, no-store");
                foreach(KeyValuePair<string, string> v in values)
                    request.Headers.Add(v.Key, v.Value);
                response = (HttpWebResponse)request.GetResponse();
                res = new StreamReader(response.GetResponseStream()).ReadToEnd();
                response.Close();
                return (res);
            } catch { return (""); }
        }

        public void Dispose()
        {
            try
            {
                if (Getter != null)
                {
                    Getter.Dispose();
                    Getter = null;
                }
            } catch { }
        }

        #endregion
    }
}
