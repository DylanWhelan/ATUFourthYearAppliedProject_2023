using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeInfo
{
    // All of the variables are set as properties to be accessed more easily
    public string SlimeName { get; }
    public float SlimeScale { get; }
    public float SlimeSpeed { get; }
    public int SlimeGeneration { get; }
    // The children has a publit setter, as it is the only one that should be incrementable after the class has been instantiated
    public int SlimeChildren { get; set; }
    public SlimeInfo ParentSlime { get; }

    // Constructor for first generation slime, whom has no parent
    public SlimeInfo(string slimeName, float slimeSize, float slimeSpeed)
    {
        SlimeName = slimeName;
        SlimeScale = slimeSize;
        SlimeSpeed = slimeSpeed;
        SlimeGeneration = 0;
        SlimeChildren = 0;
    }

    // Constructor for slime with parent, takes the generation and parentSlime as additional paramters
    public SlimeInfo(string slimeName, float slimeSize, float slimeSpeed, int slimeGeneration, SlimeInfo parentSlime)
    {
        SlimeName = slimeName;
        SlimeScale = slimeSize;
        SlimeSpeed = slimeSpeed;
        SlimeGeneration = slimeGeneration;
        // The SlimeInfo parentSlime is a way to set the connect the parentSlimeInfo to this slimeInfo approximating a linked list
        ParentSlime = parentSlime;
        SlimeChildren = 0;
    }
}
