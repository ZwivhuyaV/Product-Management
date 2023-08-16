using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DAL.DTOs
{
    public class ResponseMessage
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }

        public ResponseMessage(bool isSuccessful, string message = null)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }
    }
}
