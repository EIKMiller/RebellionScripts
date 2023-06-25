using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    private List<BaseItem> _Items = new List<BaseItem>();

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        } else 
        {
            Destroy(this.gameObject);
        }
    }

    public BaseItem GetItem(string itemName)
    {
        foreach(var item in _Items)
        {
            if(item.ItemName == itemName)
                return item;
        }

        return null;

    }
}