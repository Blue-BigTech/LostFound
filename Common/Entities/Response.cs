
using System.Net;

namespace Common.Abstract.Entities
{
    public class Response
    {
        public int StatusCode { get; set; } = (int)HttpStatusCode.OK;
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
        public dynamic Data { get; set; }
        public long Total { get; set; }
        public bool IsValidated { get { return StatusCode == (int)HttpStatusCode.OK; } }



    }
}
