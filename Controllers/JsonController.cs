using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonMongoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class JsonController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Application runs.");
        }

        [HttpPost]
        public IActionResult Post(object json)
        {
            var connectionString = "mongodb://localhost";

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("Jsony");
            var collection = database.GetCollection<BsonDocument>("Jsony");          

            var jsonString = json.ToString();

            if(jsonString.First().Equals('['))
            {
                var array = JsonConvert.DeserializeObject<JArray>(jsonString);

                foreach (JObject item in array)
                {
                    var doc = BsonSerializer.Deserialize<BsonDocument>(item.ToString());
                    collection.InsertOne(doc);
                }

                return Ok();
            }
            else
            {
                var document = BsonSerializer.Deserialize<BsonDocument>(json.ToString());
                collection.InsertOne(document);
                return Ok();
            }          
        }
    }
}
