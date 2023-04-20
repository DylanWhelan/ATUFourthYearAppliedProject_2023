using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeInfo
{
    public string SlimeName { get; }
    public float SlimeScale { get; }
    public float SlimeSpeed { get; }
    public int SlimeGeneration { get; }
    public int SlimeChildren { get; set; }
    public SlimeInfo ParentSlime { get; }

    public SlimeInfo(string slimeName, float slimeSize, float slimeSpeed)
    {
        SlimeName = slimeName;
        SlimeScale = slimeSize;
        SlimeSpeed = slimeSpeed;
        SlimeGeneration = 0;
        SlimeChildren = 0;
    }

    public SlimeInfo(string slimeName, float slimeSize, float slimeSpeed, int slimeGeneration, SlimeInfo parentSlime)
    {
        SlimeName = slimeName;
        SlimeScale = slimeSize;
        SlimeSpeed = slimeSpeed;
        SlimeGeneration = slimeGeneration;
        ParentSlime = parentSlime;
        SlimeChildren = 0;
    }
}
