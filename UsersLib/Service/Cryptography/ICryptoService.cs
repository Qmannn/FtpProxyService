namespace UsersLib.Service.Cryptography
{
    internal interface ICryptoService
    {
        string DecryptData(string encryptedData);
        string EncryptString(string data);
    }
}