using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class InventoryController
{
    [Header("Owner Details")]
    public BaseCharacterController Owner;
    [SerializeField] private bool _IsPlayerOwned = false;

    [Header("Inventory Settings")]
    [SerializeField] private List<BaseItem> _Items = new List<BaseItem>();
    public List<BaseItem> Items { get => _Items; }
    [SerializeField] private float _MaxInventoryWeight;
    [SerializeField] private float _CurrentInventoryWeight;
    [SerializeField] private Transform _InventoryObject;                // Reference to where we store weapons after they've been picked up

    [Header("Equipped Items")]
    private WeaponController _EquippedWeapon_1;
    public WeaponController EquippedWeapon1 { get => _EquippedWeapon_1; }
    private WeaponController _EquippedWeapon_2;
    public WeaponController EquippedWeapon2 { get => _EquippedWeapon_2; }
    private WeaponController _EquippedMelee;

    [Header("Attachment Settings")]
    [SerializeField] private Transform _AR_Holster_1;
    [SerializeField] private Transform _AR_Holster_2;
    [SerializeField] private Transform _Pistol_Holster;
    [SerializeField] private Transform _KnifeHolster;
    [SerializeField] private Transform _InHand;

    private bool _UsingEquippedWeapon_1 = true;
    public bool UsingEquippedWeapon_1 { get => _UsingEquippedWeapon_1; }

    [Header("Cameras")]
    [SerializeField] protected GameObject _InventoryCamera;
    [HideInInspector] public Camera _GameCamera;

    [Header("UI")]
    public ItemView _ItemView;
    [SerializeField] private InventoryViewController _InventoryView;

    [Header("Spawn Settings")]
    [SerializeField] private List<string> _SpawnWithItems = new List<string>();
    public List<string> SpawnWithItems => _SpawnWithItems;

    /// <summary>
    /// Adds a new item to the inventory
    /// </summary>
    /// <param name="item">Item to add</param>
    public void AddItem(BaseItem item)
    {
        if(item)
        {
            _Items.Add(item);
            _CurrentInventoryWeight += item.ItemWeight;
            if(_CurrentInventoryWeight > _MaxInventoryWeight)
            {
                Owner.IsOverWeightLimit = true;
            } else 
            {
                Owner.IsOverWeightLimit = false;
            }

            if(item is WeaponController weapon)
                weapon.Owner = Owner;


            if(_InventoryObject)
            {
                item.transform.SetParent(_InventoryObject);
                item.gameObject.SetActive(false);
            }
                
        }
    }

    /// <summary>
    /// Removes the specified items from the inventory
    /// </summary>
    /// <param name="item">Item to remove</param>
    public void RemoveItem(BaseItem item)
    {
        if(item && _Items.Contains(item))
        {
            _Items.Remove(item);
            _CurrentInventoryWeight -= item.ItemWeight;
            if(_CurrentInventoryWeight > _MaxInventoryWeight)
            {
                Owner.IsOverWeightLimit = true;
            } else 
            {
                Owner.IsOverWeightLimit = false;
            }

            if(item is WeaponController weapon)
                weapon.Owner = null;
        }
    }

    /// <summary>
    /// Gets the equipped weapon
    /// </summary>
    /// <returns>When that is in hand</returns>
    public WeaponController GetInHandWeapon()
    {
        if(_EquippedWeapon_1 != null && _EquippedWeapon_1.IsInHand)
            return _EquippedWeapon_1;
        else if(_EquippedWeapon_2 != null && _EquippedWeapon_2.IsInHand)
            return _EquippedWeapon_2;

        return null;
    }

    /// <summary>
    /// Equips the item into slot 1
    /// </summary>
    /// <param name="weapon"></param>
    public void EquipItem_1(WeaponController weapon)
    {
        // Check that the weapon is valid and that it is in the inventory
        if(weapon && _Items.Contains(weapon))
        {
            bool holstered = true;              // Predefine if the weapon is holstered

            // Validate weapon 1 and whether weapon 1 is currently holstered or not
            if(_EquippedWeapon_1 != null)
            {
                if(_EquippedWeapon_1.IsEquipped)
                {
                    holstered = _EquippedWeapon_1.IsHolstered;              // Set whether we will be holstering the item or not        
                    _EquippedWeapon_1.SetEquipped(false);                   // Set the current weapon 1 to unequipped
                }
            }

            weapon.IsHolstered = holstered;                     // Set if we are holstering this item
            weapon.IsEquipped = !holstered;                     // Set if we are equipping this item
            _EquippedWeapon_1 = weapon;                         // Set the new weapon 1
            weapon.SetInHand();                                 // Set the weapon into the world

            // If holstered than determine the parent of the item
            if(holstered)
            {
                switch(weapon.WeaponType)
                {
                    case EWeaponType.RIFLE:
                    case EWeaponType.SMG:
                        weapon.transform.SetParent(_AR_Holster_1);
                        break;
                    case EWeaponType.PISTOL:
                        weapon.transform.SetParent(_Pistol_Holster);
                        break;
                    case EWeaponType.MELEE:
                        if(weapon is MeleeController melee)
                        {
                            switch(melee.MeleeType)
                            {
                                case EMeleeType.KNIFE:
                                    weapon.transform.SetParent(_KnifeHolster);
                                    break;
                            }
                        }
                        break;
                }

                weapon.IsInHand = false;

                // Notify the owner that the weapon is not in hand (is holstered)
                if(Owner)
                    Owner.WeaponInHand = false;
            } else 
            {
                weapon.transform.SetParent(_InHand);
                weapon.IsInHand = true;
                if(Owner)
                    Owner.WeaponInHand = true;
            }

            if(_IsPlayerOwned)
                if(Owner is PlayerController player)
                    player.HudController.SetSlotOne(weapon);
        }
    }

    /// <summary>
    /// Equips the item into slot 2
    /// </summary>
    /// <param name="weapon"></param>
    public void EquipWeapon_2(WeaponController weapon)
    {
        if(weapon && _Items.Contains(weapon))
        {
            bool holstered = true;
            if(_EquippedWeapon_2 != null)
            {
                if(_EquippedWeapon_2.IsEquipped)
                {
                    holstered = _EquippedWeapon_2.IsHolstered;
                    _EquippedWeapon_2.SetEquipped(false);
                }
            }

            weapon.IsHolstered = holstered;
            weapon.IsEquipped = !holstered;
            _EquippedWeapon_2 = weapon;
            weapon.SetInHand();
            if(holstered)
            {
                switch(weapon.WeaponType)
                {
                    case EWeaponType.RIFLE:
                    case EWeaponType.SMG:
                        weapon.transform.SetParent(_AR_Holster_2);
                        break;
                    case EWeaponType.PISTOL:
                        weapon.transform.SetParent(_Pistol_Holster);
                        break;
                    case EWeaponType.MELEE:
                        if(weapon is MeleeController melee)
                        {
                            switch(melee.MeleeType)
                            {
                                case EMeleeType.KNIFE:
                                    weapon.transform.SetParent(_KnifeHolster);
                                    break;
                            }
                        }
                        break;
                    
                }

                weapon.IsInHand = false;

                if(Owner)
                    Owner.WeaponInHand = false;
            } else 
            {
                weapon.transform.SetParent(_InHand);
                weapon.IsInHand = true;
                if(Owner)
                    Owner.WeaponInHand = true;
            }

            if(_IsPlayerOwned)
                if(Owner is PlayerController player)
                    player.HudController.SetSlotTwo(weapon);
        }
    }

    public void HolsterEquipped(bool holster, int index)
    {
        if(holster)
        {
            if(index == 1)
            {
                _EquippedWeapon_1.IsHolstered = true;
                _EquippedWeapon_1.IsInHand = false;
                switch(_EquippedWeapon_1.WeaponType)
                {
                    case EWeaponType.RIFLE:
                    case EWeaponType.SMG:
                        _EquippedWeapon_1.transform.SetParent(_AR_Holster_1);
                        break;
                    case EWeaponType.PISTOL:
                        _EquippedWeapon_1.transform.SetParent(_Pistol_Holster);
                        break;
                    case EWeaponType.MELEE:
                        if(_EquippedWeapon_1 is MeleeController melee)
                        {
                            switch(melee.MeleeType)
                            {
                                case EMeleeType.KNIFE:
                                    _EquippedWeapon_1.transform.SetParent(_KnifeHolster);
                                    break;
                            }
                        }
                        break;
                    
                    
                }
                _EquippedWeapon_1.SetInHand();
            } else 
            {
                _EquippedWeapon_2.IsHolstered = true;
                _EquippedWeapon_2.IsInHand = false;
                switch(_EquippedWeapon_2.WeaponType)
                {
                    case EWeaponType.RIFLE:
                    case EWeaponType.SMG:
                        _EquippedWeapon_2.transform.SetParent(_AR_Holster_2);
                        break;
                    case EWeaponType.PISTOL:
                        _EquippedWeapon_2.transform.SetParent(_Pistol_Holster);
                        break;
                    case EWeaponType.MELEE:
                        if(_EquippedWeapon_1 is MeleeController melee)
                        {
                            switch(melee.MeleeType)
                            {
                                case EMeleeType.KNIFE:
                                    _EquippedWeapon_1.transform.SetParent(_KnifeHolster);
                                    break;
                            }
                        }
                        break;
                    
                }
                _EquippedWeapon_2.SetInHand();
            }
        } else 
        {
            if(index == 1)
            {
                if(_EquippedWeapon_2 != null)
                {
                    if(!_EquippedWeapon_2.IsHolstered)
                    {
                        _EquippedWeapon_2.IsHolstered = true;
                        _EquippedWeapon_2.IsInHand = false;
                        switch(_EquippedWeapon_2.WeaponType)
                        {
                            case EWeaponType.RIFLE:
                            case EWeaponType.SMG:
                                _EquippedWeapon_2.transform.SetParent(_AR_Holster_2);
                                break;
                            case EWeaponType.PISTOL:
                                _EquippedWeapon_2.transform.SetParent(_Pistol_Holster);
                                break;
                            
                        }
                        _EquippedWeapon_2.SetInHand();
                    }
                }

                _EquippedWeapon_1.IsHolstered = false;
                _EquippedWeapon_1.IsInHand = true;
                _EquippedWeapon_1.transform.SetParent(_InHand);
                _EquippedWeapon_1.SetInHand();
            } else 
            {
                if(_EquippedWeapon_1 != null)
                {
                    if(!_EquippedWeapon_1.IsHolstered)
                    {
                        _EquippedWeapon_1.IsHolstered = true;
                        _EquippedWeapon_1.IsInHand = false;
                        switch(_EquippedWeapon_1.WeaponType)
                        {
                            case EWeaponType.RIFLE:
                            case EWeaponType.SMG:
                                _EquippedWeapon_1.transform.SetParent(_AR_Holster_1);
                                break;
                            case EWeaponType.PISTOL:
                                _EquippedWeapon_1.transform.SetParent(_Pistol_Holster);
                                break;
                            
                        }
                        _EquippedWeapon_1.SetInHand();
                    }
                }

                _EquippedWeapon_2.IsHolstered = false;
                _EquippedWeapon_2.IsInHand = true;
                _EquippedWeapon_2.transform.SetParent(_InHand);
                _EquippedWeapon_2.SetInHand();
            }
        }
    }

    public void EquipMelee(WeaponController melee)
    {
        if(melee && _Items.Contains(melee))
        {
            bool holstered = true;
            if(_EquippedMelee.IsEquipped)
            {
                holstered = _EquippedMelee.IsHolstered;
                _EquippedMelee.SetEquipped(false);
            }

            _EquippedMelee = melee;
            melee.IsEquipped = !holstered;
            melee.IsHolstered = holstered;
            melee.SetInHand();

        }
    }

    public void Open()
    {
        _GameCamera.enabled = false;
        _InventoryCamera.SetActive(true);
        _ItemView._Inventory = this;

        if(_InventoryView)
            _InventoryView.Setup(this);
    }

    public void Close()
    {
        _GameCamera.enabled = true;
        _InventoryCamera.SetActive(false);

    }

    public int GetAmmoCount(EAmmoType type)
    {
        int count = 0;
        foreach(var item in _Items)
        {
            if(item is Ammo ammo)
            {
                if(ammo == type)
                {
                    count += ammo.AmmoCount;
                }
            }
        }

        return count;
    }

    public void ReduceAmmo(int amount, EAmmoType type) 
    {
        foreach(var item in _Items)
        {
            if(item is Ammo ammo)
            {
                if(ammo == type)
                {
                    ammo.AmmoCount -= amount;
                    if(ammo.AmmoCount <= 0)
                        RemoveItem(ammo);

                    break;
                }
            }
        }
    }



    public void ToggleEquippedWeapon() => _UsingEquippedWeapon_1 = !_UsingEquippedWeapon_1;
}