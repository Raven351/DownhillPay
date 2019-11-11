using DownhillPayClient.APIClient.Models;
using DownhillPayClient.Properties;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillPayClient.APIClient.Requests
{
    public class SubscriptionRequest : Request
    {
        private readonly string pOSNumberUri;
        public SubscriptionRequest() : base()
        {
            EndpointUri = base.BaseUrl + APIEndpoints.Default.Subscription;
            pOSNumberUri = EndpointUri + APIEndpoints.Default.SubscriptionPOS;
        }
        /// <summary>
        /// Returns subscription object based on given number of pos_number column in database
        /// </summary>
        /// <param name="PosNumber">Number of button in subscriptions menu</param>
        /// <returns></returns>
        public Subscription GetByPosNumber(int PosNumber)
        {
            var request = new RestRequest(pOSNumberUri + PosNumber);
            var response = this.Get<List<Subscription>>(request);
            var subscription = response.Data;
            if (subscription.Count == 1) return subscription[0];
            else return null;
        }

        public List<Subscription>GetByPosNumber(bool notNull)
        {
            RestRequest request = new RestRequest();
            if (notNull == true) request.Resource = EndpointUri + "pos_number=not.is.null";
            else request.Resource = EndpointUri + "pos_number=is.null";
            var reponse = this.Get<List<Subscription>>(request);
            var subscriptionCollection = reponse.Data;
            if (subscriptionCollection.Count > 0) return subscriptionCollection;
            else return null;
        }
    }
}
