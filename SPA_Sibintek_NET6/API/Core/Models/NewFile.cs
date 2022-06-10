namespace SPA_Sibintek_NET6.API.Core.Models
{
    public class NewFile
    {
        public FormFile File { get; set; }
        public string ContentType { get; set; }
        public string Md5 { get; set; }
    }
}
