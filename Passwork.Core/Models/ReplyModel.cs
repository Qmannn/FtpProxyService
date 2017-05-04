using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Passwork.Core.Models
{
    public class ReplyModel<T>
    {
        public T response { get; set; }
        public string errorCode { get; set; }
        public string errorMessage { get; set; }

        public bool IsSuccess()
        {
            return response != null;
        }
    }
}
