using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using Passwork.Core.Models;
using System.IO;
using Passwork.Core.Exceptions;

namespace Passwork.Core.Services
{
    class TransportService
    {
        private string url;
        private string database;
        private Tools.IJsonParser parser;
        private string sessionCode = null;
        private string email;
        private string password;
        private string hash = null;

        public string PasswordHash
        {
            get
            {
                return hash;
            }
        }

        public TransportService(string email, string password, string url, string database, Tools.IJsonParser parser)
        {
            this.url = url;
            if (!this.url.EndsWith("/"))
                this.url += "/";

            this.parser = parser;
            this.email = email;
            this.password = password;
            this.database = database;
        }

        private string MakeUrl(string method)
        {
            return string.Format(@"{0}api2/{1}", this.url, method);
        }

        private string Post(string command, Dictionary<string, string> post)
        {
            using (WebClient wc = new WebClient())
            {
                NameValueCollection reqparm = new NameValueCollection();
                foreach (var item in post)
                {
                    reqparm.Add(item.Key, item.Value);
                }

                byte[] responsebytes = wc.UploadValues(MakeUrl(command), "POST", reqparm);
                string responsebody = Encoding.UTF8.GetString(responsebytes);

                return responsebody.Trim();
            }
        }

        private ReplyModel<T> PostAndParse<T>(string command, Dictionary<string, string> post)
        {
            var result = Post(command, post);
            return parser.Parse<ReplyModel<T>>(result);
        }

        private ReplyModel<T> Request<T>(string command, Dictionary<string, string> data, bool iter = false)
        {
            if (command == "openSession")
            {
                try
                {
                    return PostAndParse<T>(command, data);
                }
                catch
                {
                    throw new PasswordCoreException("Wrong creds", PasswordCoreException.ErrorTypeEnum.BadPassword.ToString());
                }
            }

            if (string.IsNullOrEmpty(this.sessionCode))
            {
                OpenSession();
            }

            if (string.IsNullOrEmpty(this.sessionCode))
                throw new Exceptions.PasswordCoreException("PWTransport can not open session");

            data["session"] = sessionCode;

            var result = PostAndParse<T>(command, data);
            if (result.IsSuccess())
                return result;

            if (result.errorCode == "expired")
            {
                if (iter)
                    throw new Exceptions.PasswordCoreException("PWTransport can not open session");

                OpenSession();

                return Request<T>(command, data, true);

            }

            return result;
        }

        private T ProcessReply<T>(ReplyModel<T> reply)
        {
            if (reply.IsSuccess())
                return reply.response;

            throw new Exceptions.PasswordCoreException(reply.errorMessage, reply.errorCode);
        }

        private bool OpenSession()
        {
            var data = new Dictionary<string, string>();
            data["email"] = this.email;
            data["password"] = this.password;

            var session = Request<AuthorizeReplyModel>("openSession", data);
            if (session != null && session.response != null && !string.IsNullOrEmpty(session.response.code))
            {
                this.sessionCode = session.response.code;
                this.hash = session.response.hash;
            }

            return string.IsNullOrEmpty(this.sessionCode);

        }

        public TreeReplyModel GetData()
        {
            var result = Request<TreeReplyModel>("getData", new Dictionary<string, string>());
            return ProcessReply<TreeReplyModel>(result);
        }

        public TreeReplyModel GetDataFromLocalStorage()
        {
            try
            {
                return this.parser.Parse<TreeReplyModel>(File.ReadAllText(this.database));
            }
            catch
            {
                return null;
            }

        }

        public void SaveDataToLocalStorage(TreeReplyModel tree, Func<string, string> processor)
        {
            var data = processor(this.parser.Serialize(tree));
            try
            {
                File.WriteAllText(this.database, data);
            }
            catch
            {
                
            }
        }

        public bool Login()
        {
            return OpenSession();
        }

    }
}
