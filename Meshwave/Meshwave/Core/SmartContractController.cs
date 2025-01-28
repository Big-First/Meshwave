using Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Core;

public class SmartContractController
{
    Wavechain _wavechain { get; set; }
    MeshwavePersistence _meshwavePersistence { get; set; }
    Server _server { get; set; }

    public SmartContractController(Server server)
        => _server = server;
}