namespace FtpProxy.Entity
{
    public static class ProcessingClientCommand
    {
        /// <summary>
        /// Переход в пассивный режим
        /// </summary>
        public const string User = "USER";
        /// <summary>
        /// Переход в пассивный режим
        /// </summary>
        public const string Pass = "PASS";
        /// <summary>
        /// Переход в пассивный режим
        /// </summary>
        public const string Pasv = "PASV";
        /// <summary>
        /// Переход в расширенный пассивный режим
        /// </summary>
        public const string Epsv = "EPSV";
        /// <summary>
        /// Переход в активный режим
        /// </summary>
        public const string Port = "PORT";
        /// <summary>
        /// Переход в расширенный активный режим
        /// </summary>
        public const string Eprt = "EPRT";
        /// <summary>
        /// Организация защищенного канала (TLS)
        /// </summary>
        public const string Auth = "AUTH";
        /// <summary>
        /// Установка уровня защиты каналов передачи
        /// </summary>
        public const string Prot = "PROT";
        /// <summary>
        /// Размер буфера защиты.
        /// </summary>
        public const string Pbsz = "PBSZ";
        /// <summary>
        /// Завершение работы
        /// </summary>
        public const string Quit = "QUIT";
    }
}