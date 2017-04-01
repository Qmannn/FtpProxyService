namespace FtpProxyService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.FtpServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.FtpServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // FtpServiceProcessInstaller
            // 
            this.FtpServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.FtpServiceProcessInstaller.Password = null;
            this.FtpServiceProcessInstaller.Username = null;
            // 
            // FtpServiceInstaller
            // 
            this.FtpServiceInstaller.ServiceName = "FtpProxyService";
            this.FtpServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.FtpServiceProcessInstaller,
            this.FtpServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller FtpServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller FtpServiceInstaller;
    }
}