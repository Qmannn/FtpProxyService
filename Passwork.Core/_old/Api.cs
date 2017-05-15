using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Passwork.Core.Models;
using Passwork.Core.Services;

namespace Passwork.Core
{

    /// <summary>
    /// Passwork API 
    /// </summary>
    public class Api
    {
        private CoreOptionModel options;
        private TransportService transportService;
        private CryptService cryptService;
        private bool inited = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">You need to specify only URL</param>
        public Api(CoreOptionModel options)
        {
            this.options = options;
        }

        /// <summary>
        /// Authorization
        /// </summary>
        /// <param name="login">Login (email)</param>
        /// <param name="password">Auth password</param>
        /// <returns></returns>
        public bool Login(string login, string password)
        {
            this.options.Login = login;
            this.options.Password = password;
            transportService = new TransportService(this.options.Login, this.options.Password, this.options.Url, this.options.Database, new Tools.JsonParser());
            return transportService.Login();
        }

        /// <summary>
        /// Checks if the secret word is correct
        /// </summary>
        /// <param name="password">Secret word</param>
        /// <returns></returns>
        public bool CheckMasterPassword(string password)
        {
            if (transportService == null)
                return false;

            if (string.IsNullOrEmpty(transportService.PasswordHash))
                return false;

            return CryptService.Md5Hash(password) == transportService.PasswordHash;

        }


        /// <summary>
        /// Saves secret word
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool SetMasterPassword(string password)
        {
            this.options.MasterPassword = password;
            cryptService = new CryptService(this.options.MasterPassword);

            return true;
        }

        /// <summary>
        /// Decrypt password
        /// </summary>
        /// <param name="cryptedPassword">Encrypted password</param>
        /// <param name="cryptedGroupPassword">Encrypted group's password</param>
        /// <returns></returns>
        public PasswordReplyModel GetDecryptedPassword(PasswordReplyModel cryptedPassword, string cryptedGroupPassword)
        {
            var groupPassword = cryptService.Decrypt(cryptedGroupPassword);
            var cloned = cryptedPassword.Clone();

            cloned.cryptedPassword = cryptService.Decrypt(cloned.cryptedPassword, groupPassword);
            foreach (var item in cloned.custom)
            {
                item.name = cryptService.Decrypt(item.name, groupPassword);
                item.value = cryptService.Decrypt(item.value, groupPassword);
            }

            return cloned;

        }


        /// <summary>
        /// Gets all data (semi-encrypted)
        /// </summary>
        /// <returns></returns>
        public TreeReplyModel GetTree()
        {
            return transportService.GetData();
        }

    }
}
