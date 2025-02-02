using System.Text;
using Enums;
using MessagePack;
using Models;

namespace Core;

public class Message
{
    public Message()
    {
    }

    private byte[] data = new byte[1024];

    /// <summary>
    /// Concatenates a RequestCode, ActionCode, and input string into a single byte array.
    /// </summary>
    /// <param name="requestCode">The RequestCode enum indicating a request type.</param>
    /// <param name="actionCode">The ActionCode enum indicating an action type.</param>
    /// <param name="input">The input string to include in the concatenated byte array.</param>
    /// <returns>A byte array containing the combined data from the RequestCode, ActionCode, and input string.</returns>
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

    /// <summary>
    /// Extracts data from a byte array, processes it to identify the request code, action code,
    /// and associated string, and invokes the specified callback with these extracted values.
    /// </summary>
    /// <param name="data">The byte array containing the serialized data to be extracted.</param>
    /// <param name="processDataCallback">
    /// The callback that processes the extracted information. It takes the request code,
    /// action code, and the extracted string as its parameters.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the provided byte array is too small to extract the request code and action code.
    /// </exception>
    public void ExtractByteArrays(byte[] data, Action<RequestCode, ActionCode, string> processDataCallback)
    {
        int requestCodeSize = sizeof(int); // 4 bytes para RequestCode
        int actionCodeSize = sizeof(int); // 4 bytes para ActionCode

        if (data.Length < requestCodeSize + actionCodeSize)
        {
            throw new ArgumentException("O array de bytes é muito pequeno para conter todos os dados.");
        }

        // Extrai os bytes
        RequestCode requestCode = (RequestCode)BitConverter.ToInt32(data, 0);
        ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, requestCodeSize);

        // O resto é a string
        int stringSize = data.Length - (requestCodeSize + actionCodeSize);
        string extractedString = Encoding.UTF8.GetString(data, requestCodeSize + actionCodeSize, stringSize);
        processDataCallback(requestCode, actionCode, extractedString);
    }
}