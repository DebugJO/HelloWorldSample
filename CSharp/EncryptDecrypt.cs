 private string Encrypt(string source, string key)
 {
     using TripleDESCryptoServiceProvider tripleDESCryptoService = new();
     using MD5CryptoServiceProvider hashMD5Provider = new();
     byte[] byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
     tripleDESCryptoService.Key = byteHash;
     tripleDESCryptoService.Mode = CipherMode.ECB;
     byte[] data = Encoding.UTF8.GetBytes(source);
     return Convert.ToBase64String(tripleDESCryptoService.CreateEncryptor().TransformFinalBlock(data, 0, data.Length));
 }

 private string Decrypt(string encrypt, string key)
 {
     using TripleDESCryptoServiceProvider tripleDESCryptoService = new();
     using MD5CryptoServiceProvider hashMD5Provider = new();
     byte[] byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
     tripleDESCryptoService.Key = byteHash;
     tripleDESCryptoService.Mode = CipherMode.ECB;
     byte[] data = Convert.FromBase64String(encrypt);
     return Encoding.UTF8.GetString(tripleDESCryptoService.CreateDecryptor().TransformFinalBlock(data, 0, data.Length));
 }
