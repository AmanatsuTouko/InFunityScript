using FThingSoftware.InFunityScript;
using Cysharp.Threading.Tasks;

public class Chapter1_2 : Scenario
{
    public override async UniTask Content()
    {
        await text("", "Chapter1_2");
        // await sys_call<Chapter1_3>();

        await sys_return();
        return;
    }
}