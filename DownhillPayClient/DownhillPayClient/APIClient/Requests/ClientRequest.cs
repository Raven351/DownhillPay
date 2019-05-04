using System;
using System.Collections.Generic;
using System.Text;
using APIClient.Models;
using DownhillPayClient.Properties;
using DownhillPayClient.APIClient.Serializers;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers;
using Newtonsoft.Json;

namespace APIClient.Requests
{
    public class ClientRequest : RestClient
    {
        readonly string endpointUri;
        readonly string endpointIdUri;
        public ClientRequest()
        {
            base.BaseUrl = new Uri(Settings.Default.API_URI);
            endpointUri = base.BaseUrl + APIEndpoints.Default.Client;
            endpointIdUri = base.BaseUrl + APIEndpoints.Default.ClientID;
        }
        public ClientRequest(Uri baseUrl) : base(baseUrl)
        {
        }
        public ClientRequest(string baseUrl) : base(baseUrl)
        {
        }

        public List<Client> Get()
        {
            var request = new RestRequest(endpointUri);
            var response = this.Get<List<Client>>(request);
            return response.Data;
        }

        public Client Get(int id)
        {
            var request = new RestRequest(endpointIdUri + id);
            var response = this.Get<List<Client>>(request);
            var client = response.Data;
            if (client.Count == 1) return client[0];
            else return null;
        }

        public string Post(Client client)
        {
            var request = new RestRequest(endpointUri, Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddHeader("Content-Type", "application/json");
            request.JsonSerializer = new JsonNETSerializer();
            request.AddJsonBody(client);
            //request.AddParameter(JsonConvert.SerializeObject(client), ParameterType.RequestBody);
            var response = this.Post(request);
            var data = response.StatusCode;
            return Convert.ToString(data);
        }

    }
}
