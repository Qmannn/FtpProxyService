::Остановка службы, если она запущена
sc stop FtpProxyService
:: Удаление, если она существует
sc delete FtpProxyService

::Установка
C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil.exe "%~dp0\FtpProxyService.exe"
pause