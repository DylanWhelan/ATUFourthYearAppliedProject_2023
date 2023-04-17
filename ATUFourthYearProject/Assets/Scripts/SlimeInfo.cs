using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeInfo
{
    public string SlimeName { get; }
    public float SlimeSize { get; }
    public float SlimeSpeed { get; }
    public int SlimeGeneration { get; }
    private SlimeInfo ParentSlime { get; }

    public SlimeInfo(string slimeName, float slimeSize, float slimeSpeed)
    {
        SlimeName = slimeName;
        SlimeSize = slimeSize;
        SlimeSpeed = slimeSpeed;
        SlimeGeneration = 0;
    }

    public SlimeInfo(string slimeName, float slimeSize, float slimeSpeed, int slimeGeneration, SlimeInfo parentSlime)
    {
        SlimeName = slimeName;
        SlimeSize = slimeSize;
        SlimeSpeed = slimeSpeed;
        SlimeGeneration = slimeGeneration;
        ParentSlime = parentSlime;
    }
}
