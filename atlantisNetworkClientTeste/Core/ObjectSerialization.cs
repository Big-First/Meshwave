using System.Formats.Cbor;
using System.Text.Json;
using Enums;
using MessagePack;
using Models;

namespace Core;

public class ObjectSerialization
{
    public ObjectSerialization(){}
    public static byte[] Serialize(ContractValidationRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        using (var stream = new MemoryStream())
        {
            var writer = new CborWriter();
            writer.WriteStartMap(6); // Agora são 6 campos: userId, contract, requestCode, actionCode, previousHash, block
            writer.WriteTextString("userId");
            writer.WriteTextString(request.userId);
            writer.WriteTextString("contract");
            writer.WriteByteString(JsonSerializer.SerializeToUtf8Bytes(request.contract));
            writer.WriteTextString("requestCode");
            writer.WriteInt32((int)request.requestCode);
            writer.WriteTextString("actionCode");
            writer.WriteInt32((int)request.actionCode);
            writer.WriteTextString("previousHash");
            writer.WriteByteString(request.previousHash);
            writer.WriteTextString("block");
            writer.WriteByteString(JsonSerializer.SerializeToUtf8Bytes(request.block));
            writer.WriteEndMap();
            stream.Write(writer.Encode());
            return stream.ToArray();
        }
    }

    public static void Deserialize(byte[] data, Action<RequestCode, ActionCode, ContractValidationRequest> processDataCallback)
    {
        if (data == null || data.Length == 0)
            throw new ArgumentException("Os dados não podem ser nulos ou vazios.", nameof(data));

        var reader = new CborReader(data);
        reader.ReadStartMap();
        string userId = null;
        SmartContract contract = null;
        RequestCode requestCode = RequestCode.None;
        ActionCode actionCode = ActionCode.None;
        byte[] previousHash = null;
        Block block = null;
        for (int i = 0; i < 6; i++) // Lendo os seis campos
        {
            string key = reader.ReadTextString();
            switch (key)
            {
                case "userId":
                    userId = reader.ReadTextString();
                    break;
                case "contract":
                    contract = JsonSerializer.Deserialize<SmartContract>(reader.ReadByteString());
                    break;
                case "requestCode":
                    requestCode = (RequestCode)reader.ReadInt32();
                    break;
                case "actionCode":
                    actionCode = (ActionCode)reader.ReadInt32();
                    break;
                case "previousHash":
                    previousHash = reader.ReadByteString();
                    break;
                case "block":
                    block = JsonSerializer.Deserialize<Block>(reader.ReadByteString());
                    break;
            }
        }
        reader.ReadEndMap();
        var obj = new ContractValidationRequest(userId, contract, requestCode, actionCode, previousHash, block);
        processDataCallback(requestCode, actionCode, obj);
    }
}