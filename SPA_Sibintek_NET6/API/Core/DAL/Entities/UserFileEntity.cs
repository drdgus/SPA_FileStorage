using System.ComponentModel.DataAnnotations.Schema;

namespace SPA_Sibintek_NET6.API.Core.DAL.Entities
{
    public class UserFileEntity
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(300)")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(32)")]
        public string Hash { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string ContentType { get; set; }
        public DateTimeOffset UploadDateTime { get; set; }
        public byte[] Data { get; set; }
        public string UserToken { get; set; }
    }
}
