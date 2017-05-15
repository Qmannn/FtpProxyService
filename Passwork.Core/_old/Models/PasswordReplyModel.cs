using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Passwork.Core.Models
{
    public class PasswordReplyModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string login { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public string groupId { get; set; }
        public string categoryId { get; set; }
        public string cryptedPassword { get; set; }
        public List<CustomPasswordReplyModel> custom { get; set; }

        public PasswordReplyModel Clone()
        {
            var clone = new PasswordReplyModel();
            clone.id = this.id;
            clone.name = this.name;
            clone.login = this.login;
            clone.url = this.url;
            clone.description = this.description;
            clone.groupId = this.groupId;
            clone.categoryId = this.categoryId;
            clone.cryptedPassword = this.cryptedPassword;
            clone.custom = new List<CustomPasswordReplyModel>();
            if(this.custom != null)
                foreach(var item in this.custom)
                    clone.custom.Add(item);

            return clone;
        }
    }
}
