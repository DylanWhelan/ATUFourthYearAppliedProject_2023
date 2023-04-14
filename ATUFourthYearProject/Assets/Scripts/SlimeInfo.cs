using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeInfo
{
    private string slimeName { get; set; }
    private float slimeSize { get; set; }
    private SlimeInfo parentSlime { get; }

    public SlimeInfo(string slimeName, float slimeSize)
    {
        this.slimeName = slimeName;
        this.slimeSize = slimeSize;
    }

    public SlimeInfo(string slimeName, float slimeSize, SlimeInfo parentSlime)
    {
        this.slimeName = slimeName;
        this.slimeSize = slimeSize;
        this.parentSlime = parentSlime;
    }
}
