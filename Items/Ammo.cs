using System;
using System.Collections.Generic;
using UnityEngine;

public enum EAmmoType
{
    NINE_MM,
    FIVE_FIVE_SIX,
    
}


public class Ammo : BaseItem
{
    private EAmmoType _AmmoType;
    public EAmmoType AmmoType { get => _AmmoType; }

    public int AmmoCount;

    public override void UseItem()
    {
        
    }

    public static bool operator ==(Ammo a, EAmmoType b)
    {
        if(a.AmmoType == b)
            return true;

        return false;
    }

    public static bool operator !=(Ammo a, EAmmoType b)
    {
        if(a == b)
            return false;

        return true;
    }
}