using Newtonsoft.Json;
using System;

namespace APIClient.Models
{
    
    public class DatabaseModel
    {
        public DatabaseModel()
        {
        }

        public DatabaseModel(Guid id)
        {
            Id = id;
        }

        //[JsonIgnore]
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}