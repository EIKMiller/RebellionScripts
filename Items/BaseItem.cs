using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    [Header("Item Details")]
    [SerializeField] protected string _ItemName;
    public string ItemName { get => _ItemName; }
    [SerializeField] protected string _ItemDescription;
    public string ItemDescription { get => _ItemDescription; }
    [SerializeField] protected Sprite _ItemIcon;
    public Sprite ItemIcon { get => _ItemIcon; }
    [SerializeField] protected int _Value;
    public int Value { get => _Value; }
    

    [Header("Inventory Settings")]
    [SerializeField] private float _ItemWeight;
    public float ItemWeight { get => _ItemWeight; }

    public void SetEquipped(bool equipped = false)
    {
        if(equipped)
        {
            this.gameObject.SetActive(true);
        } else 
        {
            this.gameObject.SetActive(false);
        }
    }

    public abstract void UseItem();

}
