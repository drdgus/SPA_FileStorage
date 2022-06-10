using System;

namespace Angular_SPA_Sibintek.API.Core.DAL.Entities
{
    public class UserFileEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public DateTimeOffset UploadDateTime { get; set; }
        public byte[] Data { get; set; }
        public string UserToken { get; set; }
    }
}
