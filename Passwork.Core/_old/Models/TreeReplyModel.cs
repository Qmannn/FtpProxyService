using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Passwork.Core.Models
{
    public class TreeReplyModel
    {
        public string user { get; set; }
        public List<GroupReplyModel> groups { get; set; }
    }
}
