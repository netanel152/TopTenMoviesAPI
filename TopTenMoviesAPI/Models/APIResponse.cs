using System.Net;

namespace BacsoftLWFWebApi.Models
{
    public class APIResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }
}
