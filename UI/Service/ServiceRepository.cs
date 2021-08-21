using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace UI.Service
{
    public class ServiceRepository
    {
        public HttpClient Client { get; set; }

        public ServiceRepository()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ServiceUrl"].ToString());
        }
        public HttpResponseMessage GetResponse(String url)
        {
            return Client.GetAsync(url).Result;
        }
        public HttpResponseMessage GetResponse2(String url,object model)
        {
            return Client.GetAsync(url).Result;
        }

        public HttpResponseMessage PutResponse(String url, object model)
        {
            return Client.PutAsJsonAsync(url, model).Result;
        }

        public HttpResponseMessage PostResponse(String url, object model)
        {
            return Client.PostAsJsonAsync(url, model).Result;
        }

        public HttpResponseMessage DeleteResponse(String url)
        {
            return Client.DeleteAsync(url).Result;
        }

    }
}