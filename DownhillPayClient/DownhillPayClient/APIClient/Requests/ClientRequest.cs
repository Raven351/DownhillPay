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
using DownhillPayClient.APIClient.Requests;

namespace APIClient.Requests
{
    public class ClientRequest : Request
    {
        public ClientRequest() : base()
        {
            EndpointUri = base.BaseUrl + APIEndpoints.Default.Client;
            EndpointUriId = EndpointUri + APIEndpoints.Default.ClientID;
        }

        public List<Client> Get()
        {
            var request = new RestRequest(EndpointUri);
            var response = this.Get<List<Client>>(request);
            return response.Data;
        }

        public Client Get(int id)
        {
            var request = new RestRequest(EndpointUriId + id);
            var response = this.Get<List<Client>>(request);
            var client = response.Data;
            if (client.Count == 1) return client[0];
            else return null;
        }

        public Client Get(string firstName, string lastName, string phoneNumber, DateTime birthDate)
        {
            var request = new RestRequest(EndpointUri + 
                APIEndpoints.Default.ClientFirstName + firstName + "&" +
                APIEndpoints.Default.ClientLastName + lastName + "&" + 
                APIEndpoints.Default.ClientPhoneNumber + phoneNumber + "&" + 
                APIEndpoints.Default.ClientBirthDate + birthDate.ToString("yyyy/MM/dd"));
            var response = this.Get< List < Client >>(request);
            var client = response.Data;
            if (client.Count == 1) return client[0];
            else return null;
        }

        //public string Post(Client client)
        //{
        //    var request = new RestRequest(EndpointUri, Method.POST)
        //    {
        //        RequestFormat = DataFormat.Json
        //    };
        //    request.AddHeader("Content-Type", "application/json");
        //    request.JsonSerializer = new JsonNETSerializer();
        //    request.AddJsonBody(client);
        //    //request.AddParameter(JsonConvert.SerializeObject(client), ParameterType.RequestBody);
        //    var response = this.Post(request);
        //    var data = response.StatusCode;
        //    return Convert.ToString(data);
        //}

    }
}
