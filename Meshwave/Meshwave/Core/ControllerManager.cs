using Enums;
using Models;

namespace Core;

public class ControllerManager
{
    public ControllerManager(){}
    public void HandleRequest(RequestCode requestCode, ActionCode actionCode, ContractValidationRequest output, Node node)
    {
        if (requestCode is RequestCode.Validation) new  BlockchainExecutor(node).ExtractAsync(output);
    }
}