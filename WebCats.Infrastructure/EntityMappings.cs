using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using WebCats.Model;

namespace WebCats.Infrastructure
{
    public class EntityMappings
    {
        public static void Map()
        {
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
            
            BsonClassMap.RegisterClassMap<Image>(x =>
            {
                x.AutoMap();
                x.SetIgnoreExtraElements(true);
                x.MapIdMember(y => y.Id);
            });
            
            BsonClassMap.RegisterClassMap<User>(x =>
            {
                x.AutoMap();
                x.SetIgnoreExtraElements(true);
            });
        }
    }
}