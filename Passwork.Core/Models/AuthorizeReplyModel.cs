using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Passwork.Core.Models
{
    public class AuthorizeReplyModel
    {
        public string code { get; set; }
        public string hash { get; set; }
    }
}
