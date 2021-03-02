using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirpel.Common.Models
{
    public class HttpResponse
    {
        public bool Succes { get; set; }
        public string Message { get; set; }

        public HttpResponse(bool _succes, string _message)
        {
            Succes = _succes;
            Message = _message;
        }
    }
}
