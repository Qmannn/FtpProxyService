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
        /// Группа LDAP 
        /// </summary>
        public static string LdapGroupName => ConfigurationManager.AppSettings[ "LdapGroupName" ];
        /// <summary>
        /// Сервисный аккаунт LDAP
        /// </summary>
        public static string LdapServiceAccount => ConfigurationManager.AppSettings["LdapServiceAccount"];
        /// <summary>
        /// Пароль к сервисному аккаунту LDAP
        /// </summary>
        public static string LdapServicePassword => ConfigurationManager.AppSettings["LdapServicePassword"];
        /// <summary>
        /// URL для обращения к хранилищу паролей PASSWORK
        /// </summary>
        public static string PassworkApiUrl => ConfigurationManager.AppSettings[ "PassworkApiUrl" ];
        /// <summary>
        /// Серкетное слово пользователя PASSWORK
        /// </summary>
        public static string PassworkMasterWord => ConfigurationManager.AppSettings[ "PassworkMasterWord" ];
        /// <summary>
        /// Серкетное слово пользователя PASSWORK
        /// </summary>
        public static string PassworkLogin => ConfigurationManager.AppSettings[ "PassworkLogin" ];
        /// <summary>
        /// Серкетное слово пользователя PASSWORK
        /// </summary>
        public static string PassworkPassword => ConfigurationManager.AppSettings[ "PassworkPassword" ];
        /// <summary>
        /// Серкетное слово пользователя PASSWORK
        /// </summary>
        public static string PassworkGroupName => ConfigurationManager.AppSettings[ "PassworkGroupName" ];

    }
}