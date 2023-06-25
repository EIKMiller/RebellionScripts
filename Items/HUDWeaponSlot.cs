using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDWeaponSlot : MonoBehaviour
{
    [Header("Owner Details")]
    [SerializeField] private PlayerController _Player;
    
    [Header("Button Settings")]
    [SerializeField] private int _Slot;

    [Header("Elements")]
    [SerializeField] private Image _ItemImage;

    private BaseItem _OwningItem;

    private void Awake()
    {
        if(!_Player)
            _Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void SetItem(BaseItem item)
    {
        _OwningItem = item;
        if(_ItemImage)
            _ItemImage.sprite = _OwningItem.ItemIcon;
    }

    public void OnClick()
    {
        if(!_Player || _Slot == 0)
            return; 

        switch(_Slot)
        {
            case 1:
                _Player.ToggleSlotOne();
                break;
            case 2:
                _Player.ToggleSlotTwo();
                break;
        }
    }
}
