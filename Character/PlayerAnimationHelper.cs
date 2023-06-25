using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHelper : MonoBehaviour
{
    [SerializeField] private PlayerController _Player;
    [SerializeField] private Animator _Anim;
    public int SwitchingTo = 0;
    public int SwitchingSlotIndex = 0;
    public int CurrentSlotIndex = 0;

    private void Awake()
    {
        if(!_Player)
            _Player = this.GetComponent<PlayerController>();
        
        if(!_Anim)
            _Anim = this.GetComponent<Animator>();
    }

    public void HolsterWeapon()
    {
        _Player.Inventory.HolsterEquipped(true, CurrentSlotIndex);

        if(SwitchingSlotIndex > 0)
        {
            _Anim.SetInteger("WeaponSlotIndex", SwitchingSlotIndex);
            _Anim.SetInteger("EquippedWeapon", SwitchingTo);
            _Anim.SetTrigger("Equip");
            CurrentSlotIndex = SwitchingSlotIndex;
        } else
        {
            _Anim.SetInteger("EquippedWeapon", 0);
        }
    }

    public void AttachWeapon()
    {
        _Player.Inventory.HolsterEquipped(false, CurrentSlotIndex);
    }

    public void TriggerGetWeapon(int weaponSlot, int weaponType)
    {

        if(!_Anim)
        {
            Debug.LogError("No Animator Attached");
            return;
        }
            

        _Anim.SetInteger("EquippedWeapon", weaponType);
        _Anim.SetInteger("WeaponSlotIndex", weaponSlot);
        _Anim.SetTrigger("Equip");
        _Anim.SetTrigger("SwitchWeapon");
        CurrentSlotIndex = weaponSlot;
    }

    public void TriggerHolster(int weaponSlot, int switchingTo, int nextSlotIndex = 0)
    {
        SwitchingTo = switchingTo;
        if(_Anim != null)
        {
            _Anim.SetInteger("WeaponSlotIndex", weaponSlot);
            _Anim.SetTrigger("SwitchWeapon");
            SwitchingSlotIndex = nextSlotIndex;
            CurrentSlotIndex = weaponSlot;
        }
    }
}
