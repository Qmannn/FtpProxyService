using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UsersLib.DbEntity;

namespace UsersLib.Entity
{
    public class SecureSiteData
    {
        [Key]
        [ForeignKey("Site")]
        public virtual int SiteId { get; set; }
        public virtual string Url { get; set; }
        public virtual int Port { get; set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        
        // Ignored
        /// <summary>
        /// Флаг, указывающий на необходимость зашифровать логин и пароль
        /// при пересохранении повторно шифровать не нужно
        /// </summary>
        public bool NeedToEncrypt { get; set; } = false;
        
        // 
        public virtual Site Site { get; set; }
    }
}