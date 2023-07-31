using FThingSoftware.InFunityScript;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public class Main : Scenario
{
    public override async UniTask Content()
    {
        await sys_label("start");

        // await text("", "");

        await sys_call<RunMelos>();

        //await sys_call<Chapter1_1>();

        //await sys_call<Chapter1_2>();

        //await sys_call<Chapter1_3>();

        await sys_jump<Main>("start");

        return;
    }
}