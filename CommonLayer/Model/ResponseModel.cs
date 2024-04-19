using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;
using CommonLayer.Model.ResponseDTO;

namespace ModelLayer.Models
{
    public class ResponseModel<T>
    {
        [JsonIgnore]
        public int StatusCode { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}