using System.Text;
using Enums;

namespace Core;

public class Message
{
    public Message(){}
    private byte[] data = new byte[1024];
    public byte[] ConcatenateByteArrays(RequestCode requestCode, ActionCode actionCode, string input)
    {
        // Converte os enums e a string em byte[]
        byte[] requestBytes = BitConverter.GetBytes((int)requestCode);
        byte[] actionBytes = BitConverter.GetBytes((int)actionCode);
        byte[] stringBytes = System.Text.Encoding.UTF8.GetBytes(input);

        // Cria um array de bytes com o tamanho total
        byte[] result = new byte[requestBytes.Length + actionBytes.Length + stringBytes.Length];

        // Copia os bytes para o array resultante
        Buffer.BlockCopy(requestBytes, 0, result, 0, requestBytes.Length);
        Buffer.BlockCopy(actionBytes, 0, result, requestBytes.Length, actionBytes.Length);
        Buffer.BlockCopy(stringBytes, 0, result, requestBytes.Length + actionBytes.Length, stringBytes.Length);

        return result;
    }
    public void ExtractByteArrays(byte[] data, Action<RequestCode, ActionCode, string> processDataCallback)
    {
        // O tamanho de cada parte
        int requestCodeSize = sizeof(int); // 4 bytes para RequestCode
        int actionCodeSize = sizeof(int);  // 4 bytes para ActionCode

        if (data.Length < requestCodeSize + actionCodeSize)
        {
            throw new ArgumentException("O array de bytes é muito pequeno para conter todos os dados.");
        }

        // Extrai os bytes
        RequestCode requestCode = (RequestCode)BitConverter.ToInt32(data, 0);
        ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, requestCodeSize);
    
        // O resto é a string
        int stringSize = data.Length - (requestCodeSize + actionCodeSize);
        string extractedString = System.Text.Encoding.UTF8.GetString(data, requestCodeSize + actionCodeSize, stringSize);

        processDataCallback(requestCode, actionCode, extractedString);
    }
    public byte[] PackData(ActionCode actionCode, string data)
    {
        byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        int DataAmount = actionCodeBytes.Length + dataBytes.Length;
        byte[] DataAmountBytes = BitConverter.GetBytes(DataAmount);
        return DataAmountBytes.Concat(actionCodeBytes).ToArray().Concat(dataBytes).ToArray();
    }
    public byte[] PackData(RequestCode requestData, ActionCode actionCode, string data)
    {
        byte[] requestCodeBytes = BitConverter.GetBytes((int)requestData);
        byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        int DataAmount = requestCodeBytes.Length + dataBytes.Length + actionCodeBytes.Length;
        byte[] DataAmountBytes = BitConverter.GetBytes(DataAmount);
        byte[] newBytes = DataAmountBytes.Concat(requestCodeBytes).ToArray().Concat(actionCodeBytes).ToArray();
        return newBytes.Concat(dataBytes).ToArray();
    }
}