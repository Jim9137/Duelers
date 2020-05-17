using System;
using System.Net.Http;

namespace Duelers.Local.Controller
{
    public class RemoteAssetManagementService : IAssetManagementService
    {
        private readonly HttpClient m_HttpClient = new HttpClient();
        private readonly Uri m_BaseUri = new Uri("https://mechaz.org/");
        
        
        public string GetPlist(string path)
        {
            var res = m_HttpClient.GetAsync(m_BaseUri + path).Result;
            return res.Content.ReadAsStringAsync().Result;
        }
    }
}