using System;
using System.Collections.Generic;
using UnityEngine;

public enum EMeleeType
{
    KNIFE
}

public class MeleeController : WeaponController
{

    [SerializeField] private EMeleeType _MeleeType;
    public EMeleeType MeleeType => _MeleeType;
    
    public override int GetActionPointsUsed()
    {
        return _ActionPointsRequired;
    }

    public override void UseItem()
    {
        
    }
}