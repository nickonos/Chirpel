using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirpel.Logic
{
    public class Response
    {
        public bool Succes { get; set; }
        public string Message { get; set; }

        public Response(bool _succes, string _message)
        {
            Succes = _succes;
            Message = _message;
        }
    }
}
