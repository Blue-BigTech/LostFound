using Common.Abstract.Extension;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Dto
{
    public class UserModelDto
    {
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public string Id { get; set; }
        [NotMapped]
        public string Code { get { return Id.Encode(); } }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string PoliceStation { get; set; }
        public int Total { get; set; }

    }
}
