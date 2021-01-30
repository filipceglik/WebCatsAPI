using System;

namespace WebCats.Model
{
    public class Image
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public string StatusCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public string FilePath { get; set; }

        public Image(Guid id, string filename, string statusCode, DateTime createdOn, string filepath)
        {
            Id = id;
            Filename = filename;
            StatusCode = statusCode;
            CreatedOn = createdOn;
            FilePath = filepath;
        }
        
    }
}