using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FThingSoftware.InFunityScript
{
    [Serializable]
    public class RebuildDatas
    {
        public List<RebuildData> Each = new List<RebuildData>() { };
    }

    [Serializable]
    public class RebuildData
    {
        public int Num = 0;
        public string ScenarioName = "";
        public int ScenarioPage = 0;
        public string Date = "";
        public string ScreenShotFile = "";
        public string TextTalker = "";
        public string TextMain = "";
        public string Chapter = "";
        public string BackGroudImage = "";

        public string BGM = "";
        public int VolumePlayingBGM = 100;

        public List<DisplayChara> DisplayCharas = new List<DisplayChara>() { };
        public List<DisplayImage> DisplayImages = new List<DisplayImage>() { };
        public List<DisplaySelectButton> DisplaySelectButtons = new List<DisplaySelectButton>() { };
        public DisplayCamera DisplayCamera = new DisplayCamera();

        public RebuildData DeepCopy()
        {
            RebuildData rd = new RebuildData();

            rd.ScenarioName = ScenarioName;
            rd.ScenarioPage = ScenarioPage;
            rd.Date = Date;
            rd.ScreenShotFile = ScreenShotFile;
            rd.TextTalker = TextTalker;
            rd.TextMain = TextMain;
            rd.Chapter = Chapter;
            rd.BackGroudImage = BackGroudImage;
            rd.BGM = BGM;
            rd.VolumePlayingBGM = VolumePlayingBGM;

            if (DisplayCharas.Count > 0)
            {
                foreach (var charas in DisplayCharas)
                {
                    rd.DisplayCharas.Add(charas.DeepCopy());
                }
            }
            if (DisplayImages.Count > 0)
            {
                foreach (var images in DisplayImages)
                {
                    rd.DisplayImages.Add(images.DeepCopy());
                }
            }
            if (DisplaySelectButtons.Count > 0)
            {
                foreach (var selectButtons in DisplaySelectButtons)
                {
                    rd.DisplaySelectButtons.Add(selectButtons.DeepCopy());
                }
            }
            rd.DisplayCamera = DisplayCamera.DeepCopy();

            return rd;
        }
    }

    [Serializable]
    public class DisplayChara
    {
        public string Name = "";
        public string[] Images;
        public Vector3 Pos = new Vector3(0, 0, 0);
        public Vector3 Scale = new Vector3(1, 1, 1);
        public bool Reverse = false;

        public DisplayChara DeepCopy()
        {
            DisplayChara dc = new DisplayChara();
            dc.Name = Name;
            dc.Images = new string[Images.Length];
            for (int i = 0; i < Images.Length; i++)
            {
                dc.Images[i] = Images[i];
            }
            dc.Pos = new Vector3(Pos.x, Pos.y, Pos.z);
            dc.Scale = new Vector3(Scale.x, Scale.y, Scale.z);
            dc.Reverse = Reverse;
            return dc;
        }
    }

    [Serializable]
    public class DisplayImage
    {
        public string Name = "";
        public Vector3 Pos = new Vector3(0, 0, 0);
        public Vector3 Scale = new Vector3(1, 1, 1);

        public DisplayImage DeepCopy()
        {
            DisplayImage di = new DisplayImage();
            di.Name = Name;
            di.Pos = new Vector3(Pos.x, Pos.y, Pos.z);
            di.Scale = new Vector3(Scale.x, Scale.y, Scale.z);
            return di;
        }
    }

    [Serializable]
    public class DisplaySelectButton
    {
        public string Text = "";
        public string Scene = "";
        public string Label = "";

        public DisplaySelectButton DeepCopy()
        {
            DisplaySelectButton dsb = new DisplaySelectButton();
            dsb.Text = Text;
            dsb.Scene = Scene;
            dsb.Label = Label;
            return dsb;
        }
    }

    [Serializable]
    public class DisplayCamera
    {
        public Vector3 Pos = new Vector3(0, 0, 0);
        public float Scale = 1;
        public float Rotate = 0;

        public DisplayCamera DeepCopy()
        {
            DisplayCamera dc = new DisplayCamera();
            dc.Pos = new Vector3(Pos.x, Pos.y, Pos.z);
            dc.Scale = Scale;
            dc.Rotate = Rotate;
            return dc;
        }
    }
}