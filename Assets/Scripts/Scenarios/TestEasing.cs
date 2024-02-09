using FThingSoftware.InFunityScript;
using Cysharp.Threading.Tasks;

public class TestEasing : Scenario
{
    public override async UniTask Content()
    {
        // ここからシナリオ命令を記述
        await text("イージング関数を使った移動のテスト");

        await chara_show<maho>(maho.normal, posx: -500);

        await text("Linear");

        await chara_move<maho>(posx:1000, easing:Easing.Ease.Linear);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.Linear);

        await text("InSine, OutSine, InOutSine");

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InSine);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InSine);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.OutSine);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.OutSine);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InOutSine);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InOutSine);

        await text("InQuad, OutQuad, InOutQuad");

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InQuad);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InQuad);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.OutQuad);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.OutQuad);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InOutQuad);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InOutQuad);

        await text("InCubic, OutCubic, InOutCubic");

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InCubic);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InCubic);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.OutCubic);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.OutCubic);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InOutCubic);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InOutCubic);

        await text("InQuart, OutQuart, InOutQuart");

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InQuart);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InQuart);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.OutQuart);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.OutQuart);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InOutQuart);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InOutQuart);

        await text("InQuint, OutQuint, InOutQuint");

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InQuint);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InQuint);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.OutQuint);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.OutQuint);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InOutQuint);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InOutQuint);

        await text("InExpo, OutExpo, InOutExpo");

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InExpo);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InExpo);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.OutExpo);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.OutExpo);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InOutExpo);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InOutExpo);

        await text("InCirc, OutCirc, InOutCirc");

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InCirc);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InCirc);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.OutCirc);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.OutCirc);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InOutCirc);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InOutCirc);

        await text("InBack, OutBack, InOutBack");

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InBack);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InBack);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.OutBack);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.OutBack);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InOutBack);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InOutBack);

        await text("InElastic, OutElastic, InOutElastic");

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InElastic);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InElastic);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.OutElastic);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.OutElastic);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InOutElastic);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InOutElastic);

        await text("InBounce, OutBounce, InOutBounce");

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InBounce);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InBounce);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.OutBounce);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.OutBounce);

        await chara_move<maho>(posx:1000, easing:Easing.Ease.InOutBounce);
        await chara_move<maho>(posx:-1000, easing:Easing.Ease.InOutBounce);

        await sys_return();
        return;
    }
}