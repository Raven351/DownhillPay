﻿using APIClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillPayClient.APIClient.Models
{
    public class RfidCard : DatabaseModel
    {
        public RfidCard() : base()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        public RfidCard(string uid, string uid2, string cardNumber, int pointsBalance, DateTime createdDate, DateTime modifiedDate) : base()
        {
            Uid = uid;
            Uid2 = uid2;
            CardNumber = cardNumber;
            PointsBalance = pointsBalance;
            CreatedDate = createdDate;
            ModifiedDate = modifiedDate;
        }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("uid2")]
        public string Uid2 { get; set; }

        [JsonProperty("card_number")]
        public string CardNumber { get; set; }

        [JsonProperty("points_balance")]
        public int PointsBalance { get; set; }

        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("modified_date")]
        public DateTime ModifiedDate { get; set; }

        [JsonProperty("timeout")]
        public DateTime Timeout { get; set; }
        [JsonProperty("id_client")]
        public Guid? ClientId { get; set; }
    }
}
