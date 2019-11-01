using APIClient.Models;
using DownhillPayClient.APIClient.Serializers;
using DownhillPayClient.Properties;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillPayClient.APIClient.Requests
{
    public abstract class Request : RestClient
    {
        /// <summary>
        /// Creates object with URI of API set from application settings.
        /// </summary>
        public Request() => base.BaseUrl = new Uri(Settings.Default.API_URI);
        public string EndpointUri { get; set; }
        public string EndpointUriId { get; set; }
        public string Post(DatabaseModel databaseModel)
        {
            var request = new RestRequest(EndpointUri, Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddHeader("Content-Type", "application/json");
            request.JsonSerializer = new JsonNETSerializer();
            request.AddJsonBody(databaseModel);
            //request.AddParameter(JsonConvert.SerializeObject(client), ParameterType.RequestBody);
            var response = this.Post(request);
            var data = response.Content;
            return Convert.ToString(data);
        }
    }
}
