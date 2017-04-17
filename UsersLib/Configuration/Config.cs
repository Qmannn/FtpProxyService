using System.Configuration;

namespace UsersLib.Configuration
{
    public static class Config
    {
        /// <summary>
        /// Домен LDAP 
        /// </summary>
        public static string LdapDomain => ConfigurationManager.AppSettings["LdapDomain"];
        /// <summary>
        /// Сервисный аккаунт LDAP
        /// </summary>
        public static string LdapServiceAccount => ConfigurationManager.AppSettings["LdapServiceAccount"];
        /// <summary>
        /// Пароль к сервисному аккаунту LDAP
        /// </summary>
        public static string LdapServicePassword => ConfigurationManager.AppSettings["LdapServicePassword"];
    }
}