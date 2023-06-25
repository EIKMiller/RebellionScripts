using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemView : MonoBehaviour
{
    public InventoryController _Inventory;
    private BaseItem _ViewingItem = null;
    [Header("Item Icon Settings")]
    [SerializeField] private Image _ItemImage;

    [Header("Text Items")]
    [SerializeField] private TextMeshProUGUI _ItemName;
    [SerializeField] private TextMeshProUGUI _Property_1;
    [SerializeField] private TextMeshProUGUI _Property_2;
    [SerializeField] private TextMeshProUGUI _Weight;
    [SerializeField] private TextMeshProUGUI _Property_3;
    [SerializeField] private TextMeshProUGUI _Value;
    [SerializeField] private TextMeshProUGUI _Equip;

    [Header("Action Buttons")]
    [SerializeField] private Button _Slot_1;
    [SerializeField] private Button _Slot_2;

    private void Start()
    {
        Clear();
    }

    public void SetItem(BaseItem item)
    {
        _ViewingItem = item;
        _ItemName.text = item.ItemName;
        _ItemImage.sprite = item.ItemIcon;
        _ItemImage.color = new Color(255, 255, 255, 255);
        _Weight.text = "Weight: " + item.ItemWeight.ToString();
        _Value.text = "Value: " + item.Value.ToString();

        if(_ViewingItem is GunController gun)
        {
            ViewGun(gun);
        }

        if(_ViewingItem == _Inventory.EquippedWeapon1)
        {
            _Slot_1.interactable = false;
            _Slot_2.interactable = true;
        } else if(_ViewingItem == _Inventory.EquippedWeapon2)
        {
            _Slot_1.interactable = true;
            _Slot_2.interactable = false;
        }

    }

    private void ViewGun(GunController gun)
    {
        _Property_1.text = "Damage: " + gun.Damage.ToString();
        _Property_2.text = "Fire Type: " + gun.GetFireTypeAsString();
        _Property_3.text = "Range: " + gun.Range.ToString();
    }

    public void Clear()
    {
        _ViewingItem = null;
        _ItemName.text = "";
        _ItemImage.sprite = null;
        _Weight.text = "";
        _Value.text = "";
        _Property_1.text = "";
        _Property_2.text = "";
        _Property_3.text = "";

        _ItemImage.color = new Color(0, 0, 0, 0);
    }

    public void OnSlotOnePressed()
    {
        if(_ViewingItem is WeaponController weapon)
        {
            if(_Inventory.EquippedWeapon1 != weapon)
            {
                _Inventory.EquipItem_1(weapon);
                _Slot_1.interactable = false;
                _Slot_2.interactable = true;
                if(_Inventory.EquippedWeapon2 == weapon)
                    _Inventory.EquipWeapon_2(null);
            }
        }
    }

    public void OnSlotTwoPressed()
    {
        if(_ViewingItem is WeaponController weapon)
        {
            if(_Inventory.EquippedWeapon2 != weapon)
            {
                _Inventory.EquipWeapon_2(weapon);
                _Slot_1.interactable = true;
                _Slot_2.interactable = false;
                if(_Inventory.EquippedWeapon1 == weapon)
                {
                    _Inventory.EquipItem_1(null);
                }
            }
        }
    }
}
