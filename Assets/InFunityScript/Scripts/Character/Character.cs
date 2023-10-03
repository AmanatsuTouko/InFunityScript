using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Character : MonoBehaviour
{
    public string Name;
    public string SpritePath = "";
    public string[] Images;

    public RectTransform RectTransform;
    public Vector2Int Pos = new Vector2Int(0, 0);
    public Vector2 Scale = new Vector2(1, 1);
    public int Width = 0;
    public int Height = 0;

    public GameObject CharaImagePrefab;

    // ScenarioCommandCharacter.csから呼び出す。継承したクラスでパラメータを再度定義する
    public virtual void setParameter()
    {
    }

    // facetypeからspriteを読み込んで返す
    public List<Sprite> GetSpritesFromFacetype(string[] facetype)
    {
        List<Sprite> sprites = new List<Sprite>();

        for (int i = facetype.Length - 1; i >= 0; i--)
        {
            string spriteName = facetype[i];
            Sprite sprite = Resources.Load<Sprite>(SpritePath + spriteName);
            sprites.Add(sprite);
        }

        return sprites;
    }

    // chara_showで画面描画された時の初期設定
    public void SetDefault()
    {
        // サイズと位置の調整を行う
        this.transform.localPosition = new Vector3(Pos.x, Pos.y);
        // 子オブジェクトのサイズ調整
        this.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(Width, Height);
        this.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(Width, Height);
    }
}