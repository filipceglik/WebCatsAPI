using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using WebCats.Model;

namespace WebCats.Infrastructure
{
    public class ImageRepository
    {
        private readonly DatabaseContext _databaseContext;

        public ImageRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        
        
        public async Task<Image> GetImage(int responseCode) => await _databaseContext
            .GetCollection<Image>()
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Filename == (responseCode+".jpeg"));

        public async Task CreateImage(Image image)
        {
            await _databaseContext
                .GetCollection<Image>()
                .InsertOneAsync(image);
        }
        
    }
}