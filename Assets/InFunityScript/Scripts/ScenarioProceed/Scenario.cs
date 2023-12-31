﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace FThingSoftware.InFunityScript
{
    public class Scenario : MonoBehaviour
    {
        // 呼び出し元のシナリオ進行を管理するクラス
        private ScenarioManager sm;
        // シナリオ命令を全て記述してあるクラス
        private ScenarioCommands sc;

        // UniTaskのキャンセル用のトークン
        public CancellationTokenSource tokenSource;
        public CancellationToken token;
        
        public void SetScenarioManagerAndCommands(ScenarioManager sm, ScenarioCommands sc)
        {
            this.sm = sm;
            this.sc = sc;
        }

        // Sceneクラスを継承したファイルでContentの内容をOverrideして、
        // ScenarioManagerから共通してこのUniTaskを実行する
        public virtual async UniTask Content()
        {
            return;
        }

        // ================================================================================
        // このクラスを継承したシナリオファイルから、呼び出せるようにシナリオ命令を定義する
        // ================================================================================

        // クリック待ち関数をラッパーする
        // UniTaskのキャンセルの時の処理もこの関数で吸収する
        private async UniTask waitclick(UniTask task)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            await sm.WaitClick(task);
        }
        private async UniTask nowait(UniTask task)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            await sm.NoWaitClick(task);
        }

        // ====================================
        // シナリオ進行に用いるシステム的な関数
        // ====================================

        // call関数をラッパーする。waitを内包する
        public async UniTask sys_call<Scenario>()
        {
            if (!sm.isDoTask()) return;
            string scenarioName = typeof(Scenario).ToString();
            await nowait(sm.Call(scenarioName));
        }
        // return関数をラッパーする。waitを内包する
        public async UniTask sys_return()
        {
            if (!sm.isDoTask()) return;
            await nowait(sm.CallReturn());
        }

        // 任意のC#コードを実行する
        // await code(() => { int x = 2; int y = 3; Debug.Log(x + y); });
        // await code(() => { SaveDataHolder.I.developerVariables.Each[0].love_girl += 1; });
        // のように匿名関数として実行を渡せば、フラグ変数の更新などに使える
        public async UniTask sys_code(Action action)
        {
            if (!sm.isDoTask()) return;
            action();
        }

        // ラベルの探索
        public async UniTask sys_jump<Scenario>(string labelName)
        {
            if (!sm.isDoTask()) return;
            string scenarioName = typeof(Scenario).ToString();
            await nowait(sm.FindLabelAndJumpScenario(scenarioName, labelName));
        }
        // ラベルの設定
        public async UniTask sys_label(string labelName)
        {
            // ラベルについてラベル探索中も実行できるようにする
            if (!sm.isDoTaskOfSysLabel()) return;
            await nowait(sm.FinishFindLabel(labelName));

            // ラベル探索終了以降の命令を実行しないようにする
            // sys_labelの後にnowaitの命令があった場合に、実行されてしまうのを防ぐ
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
        
        // ==================================================================
        // シナリオコマンド一覧
        // ==================================================================
        // if (!sm.isDoTask()) return; を必ずシナリオ関数の実行前に入れること
        // 通常時(戻り値：false)シナリオ関数の進行カウンタの増加
        // LOAD時(戻り値：true) シナリオ関数を指定カウンタまで実行しない
        // ==================================================================

        // nameを更新する クリック待ちは入れない
        public async UniTask name(string inputName)
        {
            if (!sm.isDoTask()) return;
            await nowait(sc.UpdateTalker(inputName));
        }
        // テキストウィンドウに出力する
        public async UniTask text(string text)
        {
            if (!sm.isDoTask()) return;
            await waitclick(sc.UpdateText(text));
        }
        public async UniTask text(string charaName, string text)
        {            
            await this.name(charaName);
            await this.text(text);
        }

        // characterの表示
        public async UniTask chara_show<Character>(string[] facetype, float time = 1.0f, float posx = 0, float posy = 0, bool reverse = false)
        {
            if (!sm.isDoTask()) return;
            string charaName = typeof(Character).ToString();
            await sc.CharaShow(charaName, facetype, time, posx, posy, reverse);
        }
    }
}
