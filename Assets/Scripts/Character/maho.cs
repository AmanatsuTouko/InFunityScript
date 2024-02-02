using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FThingSoftware.InFunityScript;

public class maho : Character
{
    public override void setParameter()
    {
        Name = "maho";
        SpritePath = "Character/maho/";
        Pos = new Vector2Int(0, -400);
        Scale = new Vector2(1, 1);
        Width = 906;
        Height = 2048;
    }

    public static string[] normal = new string[] { "face_normal", "body_normal" };

    public static string[] angry = new string[] { "face_angry", "body_normal" };
}
