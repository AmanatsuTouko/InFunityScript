using FThingSoftware.InFunityScript;
using Cysharp.Threading.Tasks;
using System.Reflection.Emit;
using System.Diagnostics;

public class SelectButtonTest : Scenario
{
    public enum Label{
        Today,
        Tomorrow,
        NextWeek,
        Confluence
    }

    public override async UniTask Content()
    {
        // ここからシナリオ命令を記述
        await chara_show<maho>(maho.normal, reverse:true);
        await chara_face<maho>(maho.angry);

        await text("真帆", "ね！駅前にできたクレープ屋さん、一緒に行かない？");

        // 選択肢表示、分岐開始
        // await select_button_start();
        await select_button_show<SelectButtonTest>("今日行こう！", Label.Today.ToString());
        await select_button_show<SelectButtonTest>("明日行こう！", Label.Tomorrow.ToString());
        await select_button_show<SelectButtonTest>("来週行こう！", Label.NextWeek.ToString());
        await select_button_waitclick();
        
        await sys_label(Label.Today.ToString());
        await text("真帆", "やった！");
        await sys_jump<SelectButtonTest>(Label.Confluence.ToString());

        await sys_label(Label.Tomorrow.ToString());
        await text("真帆", "今日がいいなぁ……？");
        await sys_jump<SelectButtonTest>(Label.Confluence.ToString());

        await sys_label(Label.NextWeek.ToString());
        await text("真帆", "流石に来週は遅くない？");
        await sys_jump<SelectButtonTest>(Label.Confluence.ToString());

        // 分岐終わり
        await sys_label(Label.Confluence.ToString());

        await sys_return();
        return;
    }
}