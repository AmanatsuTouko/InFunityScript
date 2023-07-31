using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    // シナリオ進行に用いる真偽値の変数をまとめて管理する
    public class ScenarioBooleans : MonoBehaviour
    {
        // 各入力をしているかどうか
        public bool isWaitClick;
        public bool isSkipping;
        public bool isAutoPlaying;

        // テキストウィンドウへの文字追加と、中断するための変数
        public bool isAddingText;
        public bool isStopAddingTextAndDisplayAllText;

        // クリック待ち中にSkip/Autoボタンを押した時、次のページに自動的に進むようにする
        public bool isNextPageForce_WhenPressSkipOrAuto_WhileWaitClick;

        // ラベル探索処理中か
        public bool isGettingLabel;

        // バックログを表示しているか
        public bool isDisplayBacklog;
        // 設定画面を表示しているか
        public bool isDisplaySettings;
        // Save/Load画面を表示しているか
        public bool isDisplaySaveLoadPanel;

        // ボタンにマウスオーバーしているか
        public bool isMouseOverOnSystemButtons;
    }
}
