using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirpel.Models
{
    public class ApiResponse
    {
        public bool Succes { get; set; }
        public string Message { get; set; }

        public ApiResponse(bool _succes, string _message)
        {
            Succes = _succes;
            Message = _message;
        }
    }
}
