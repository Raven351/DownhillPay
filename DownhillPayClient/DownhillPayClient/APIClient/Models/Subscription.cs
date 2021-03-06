﻿using APIClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillPayClient.APIClient.Models
{
    public class Subscription : DatabaseModel
    {
        public Subscription()
        {
        }

        public Subscription(int price, TimeSpan period, int posNumber, string name, string description, DateTime startTime, DateTime endTime)
        {
            Price = price;
            Period = period;
            PosNumber = posNumber;
            Name = name;
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
        }

        [JsonProperty ("price")]
        public int Price { get; set; }
        [JsonProperty ("period")]
        public TimeSpan Period { get; set; }
        [JsonProperty ("pos_number")]
        public int PosNumber { get; set; }
        [JsonProperty ("name")]
        public string Name { get; set; }
        [JsonProperty ("description")]
        public string Description { get; set; }
        [JsonProperty ("start_time")]
        public DateTime StartTime { get; set; }
        [JsonProperty ("end_time")]
        public DateTime EndTime { get; set; }
    }
}
