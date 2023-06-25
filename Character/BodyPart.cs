using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BodyPart
{
    [Header("PartDetails")]
    public string PartName;

    [Header("Weights")]
    [SerializeField] private float _Weight = 1.0f;
    public float Weight { get => _Weight; }

    [Header("Modifiers")]
    [SerializeField] private float _DamageMultiplier = 1.0f;
    public float DamageMod { get => _DamageMultiplier; }
    public float ArmourMod = 1.0f;

    public void Setup(string name, float weight, float dmg = 1.0f, float arm = 1.0f)
    {
        PartName = name;
        _Weight = weight;
        _DamageMultiplier = dmg;
        ArmourMod  = arm;
    }
}