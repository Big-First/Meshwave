using System.Buffers;
using System.Diagnostics;
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
        //Stopwatch sw = Stopwatch.StartNew();
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(request);
        //sw.Stop();
        //Console.WriteLine($"Tempo de serialização: {sw.ElapsedMilliseconds}ms");
        return bytes;
    }

    public static void Deserialize(byte[] data, Action<RequestCode, ActionCode, ContractValidationRequest> processDataCallback)
    {
        //Stopwatch sw = Stopwatch.StartNew();
        ContractValidationRequest objDeserializado = JsonSerializer.Deserialize<ContractValidationRequest>(data);
        //sw.Stop();
        //Console.WriteLine($"Tempo de desserialização: {sw.ElapsedMilliseconds}ms");
        if(objDeserializado != null) processDataCallback(objDeserializado.requestCode, objDeserializado.actionCode, objDeserializado);
    }
}