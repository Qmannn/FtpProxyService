using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Passwork.Core.Models
{
    public class CoreOptionModel
    {
        public string Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string MasterPassword { get; set; }
        
        /// <summary>
        /// Reserved for the future
        /// </summary>
        public string Database { get; set; }

        public CoreOptionModel()
        {
            Url = @"http://passwork.me/";
        }

    }
}
