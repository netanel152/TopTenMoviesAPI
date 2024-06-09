﻿using System.Net;

namespace BacsoftLWFWebApi.Models
{
    public class APIResponse<T>
    {
        public APIResponse()
        {
            ErrorMessages = new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public T Result { get; set; }
    }
}