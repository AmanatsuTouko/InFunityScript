using FThingSoftware.InFunityScript;
using Cysharp.Threading.Tasks;

public class Main : Scenario
{
    public override async UniTask Content()
    {
        await sys_label("start");

        await sys_call<SelectButtonTest>();

        await sys_call<RunMelos_01>();
        await sys_call<RunMelos_02>();
        await sys_call<RunMelos_03>();

        await sys_jump<Main>("start");
        
        return;
    }
}