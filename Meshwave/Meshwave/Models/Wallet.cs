using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;

namespace Models;

public class Wallet
{
    public string publicKey { get; private set; }
    public string privateKey { get; private set; }
    public Dictionary<Guid, Coin> balances { get; }
    public DateTime timeSpam { get; }
    public byte[] data { get; }

    public Wallet()
    {
        GenerateKeys();
        timeSpam = DateTime.Now;
        this.balances = new Dictionary<Guid, Coin>();
        data = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes($"{(privateKey).Replace("-", "")}{((privateKey)).Replace("-", "")}{(publicKey)}{timeSpam}"));
    }
    
    private void GenerateKeys()
    {
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            rsa.PersistKeyInCsp = false;
            privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
            publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
        }
    }
    
    public bool VerifySignature(string data, string signature, string publicKey)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.PersistKeyInCsp = false;
            rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var signatureBytes = Convert.FromBase64String(signature);
            return rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}