using atlantisNetworkClientTeste;
using Enums;
using Models;

namespace Core;

public class ControllerManager
{
    public ControllerManager(){}
    
    public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string output, BlockchainClient client)
    {
        if (requestCode is RequestCode.Validation) new SmartContractExecutor(client).ExtractAsync(output);
        if (requestCode is RequestCode.User) new UserController(client).ExtractAsync(output);
    }
}