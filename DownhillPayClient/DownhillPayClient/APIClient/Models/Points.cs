using APIClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillPayClient.APIClient.Models
{
    public class Points : DatabaseModel
    {
        [JsonProperty ("amount")]
        public int Amount { get; set; }
        [JsonProperty ("price")]
        public int Price { get; set; }
        [JsonProperty ("pos_number")]
        public int PosNumber { get; set; }
    }
}
