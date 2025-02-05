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
    public  byte[] Serialize(ContractValidationRequest request)
    {
        //Stopwatch sw = Stopwatch.StartNew();
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(request);
        //sw.Stop();
        //Console.WriteLine($"Tempo de serialização: {sw.ElapsedMilliseconds}ms");
        return bytes;
    }

    public T Deserialize<T>(byte[] data)
    {
        T objDeserializado = default(T);
        objDeserializado = JsonSerializer.Deserialize<T>(data);
        //sw.Stop();
        //Console.WriteLine($"Tempo de desserialização: {sw.ElapsedMilliseconds}ms");
        return objDeserializado;
    }
}