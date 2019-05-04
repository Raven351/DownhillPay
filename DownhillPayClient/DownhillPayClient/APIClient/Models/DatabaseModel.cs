using Newtonsoft.Json;

namespace APIClient.Models
{
    
    public class DatabaseModel
    {
        public DatabaseModel()
        {
        }

        public DatabaseModel(int id)
        {
            Id = id;
        }

        [JsonIgnore]
        public int Id { get; set; }
    }
}