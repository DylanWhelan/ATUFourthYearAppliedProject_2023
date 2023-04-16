using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeInfo
{
    public string slimeName { get; }
    public float slimeSize { get; }
    public float slimeSpeed { get; }
    public int slimeGeneration { get; }
    private SlimeInfo parentSlime { get; }

    public SlimeInfo(string slimeName, float slimeSize, float slimeSpeed)
    {
        this.slimeName = slimeName;
        this.slimeSize = slimeSize;
        this.slimeSpeed = slimeSpeed;
        this.slimeGeneration = 0;
        this.parentSlime = null;
    }

    public SlimeInfo(string slimeName, float slimeSize, float slimeSpeed, int slimeGeneration, SlimeInfo parentSlime)
    {
        this.slimeName = slimeName;
        this.slimeSize = slimeSize;
        this.parentSlime = parentSlime;
        this.slimeGeneration = slimeGeneration;
        this.parentSlime = parentSlime;
    }
}
