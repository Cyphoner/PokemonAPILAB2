using MongoDB.Bson.Serialization.Attributes;

namespace PokemonAPILAB2.Models
{
    public class Pokemon
    {
        [BsonId]
        public string Id { get; set; }
        public string name { get; set; }
        public string type { get; set; }

    }
}
