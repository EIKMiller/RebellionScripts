using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : WeaponController
{

    [Header("Fire Settings")]
    [SerializeField] protected bool _SingleFire;            // Flag if the weapon is single fire
    [SerializeField] protected bool _BurstFire;             // Flag if the weapon is burst fire
    [SerializeField] protected float _Range;                // How far the weapon is accuarate
    public float Range { get => _Range; }

    [Header("SFX")]
    [SerializeField] private AudioController _Audio;                // Reference to the audio controller
    

    [Header("Ammo Settings")]
    [SerializeField] protected int _AmmoInMag;
    [SerializeField] protected int _MaxAmmoInMag;
    [SerializeField] protected EAmmoType _AmmoType;
    public EAmmoType AmmoType { get => _AmmoType; }

    [Header("Reload Settings")]
    [SerializeField] private bool _CanReload = true;
    [SerializeField] private bool _IsReloading = false;
    public bool IsReloading { get => _IsReloading; }
    [SerializeField] private float _ReloadTime = 2.0f;

    [Header("Action Points")]
    [SerializeField] private int _SingleFireAP;
    public int SingleFireAP { get => _SingleFireAP; }
    [SerializeField] private int _BurstFireAP;
    public int BurstFireAP { get => _BurstFireAP; }

    [Header("Attachments")]
    [SerializeField] private Attachments _SightAttachment;
    public Attachments SightAttachment { get => _SightAttachment; }
    [SerializeField] private Attachments _BarrelAttachment;
    public Attachments BarrelAttachment { get => _BarrelAttachment; }
    [SerializeField] private Attachments _MuzzleAttachment;
    public Attachments MuzzleAttachment { get => _MuzzleAttachment; }

    [Header("Attachment Points")]
    public bool HasSightAttachment =  false;
    [SerializeField] private Transform _SightAttachmentPoint;
    [SerializeField] private bool _CanHaveSightAttachment = true;
    public bool CanHaveSightAttachment { get => _CanHaveSightAttachment; }
    public bool HasBarrelAttachment;
    [SerializeField] private Transform _UnderBarrelAttachmentPoint;
    [SerializeField] private bool _CanHaveUnderBarrel = true;
    public bool CanHaveBarrelAttachment { get => _CanHaveUnderBarrel;}
    public bool HasMuzzleAttachment;
    [SerializeField] private Transform _MuzzleAttachmentPoint;
    [SerializeField] private bool _CanHaveMuzzleAttachment = true;
    public bool CanHaveMuzzleAttachment { get => _CanHaveMuzzleAttachment; }
    

    [Header("VFX")]
    [SerializeField] private GameObject _MuzzleFlash;
    [SerializeField] private float _MuzzleFlashTime;

    private void Awake()
    {
        if(!_Audio)
            _Audio = this.GetComponent<AudioController>();
    }

    public void FireWeapon()
    {
        // Check that this weapon is ready to fire
        if(!_CanAttack || _IsReloading)
            return;
        
        // Check that this weapon has ammo, if not Queue reload
        if(_AmmoInMag <= 0)
        {
            _CanAttack = false;
            StartReload();
            return;
        }

        _Audio.PlayAudioClip("FireWeaponSingle");
        _AmmoInMag -= 1;
        _CanAttack = false;
        if(Owner)
            Owner.GetBlackboard()?.SetValueAsBool("CanFire", false);
        StartCoroutine(ResetAttack());
        StartCoroutine(TriggerMuzzleFlash());
    }

    private IEnumerator TriggerMuzzleFlash()
    {
        if(_MuzzleFlash)
            _MuzzleFlash.SetActive(true);
        yield return new WaitForSeconds(_MuzzleFlashTime);
        if(_MuzzleFlash)
            _MuzzleFlash.SetActive(false);
        
    }


    public void StartReload()
    {
        _IsReloading = true;
        StartCoroutine(Reload());
        if(Owner)
            Owner.TriggerReload();
    }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(_ReloadTime);

        int ammoRequired = _MaxAmmoInMag - _AmmoInMag;
        int totalAmmo = Owner.Inventory.GetAmmoCount(_AmmoType);
        if(totalAmmo > ammoRequired)
        {
            _AmmoInMag = _MaxAmmoInMag;
            Owner.Inventory.ReduceAmmo(_AmmoInMag, _AmmoType);
            _CanAttack = true;
            // TODO: Reduce action points
        } else 
        {
            if(totalAmmo > 0)
            {
                _AmmoInMag = totalAmmo;
                Owner.Inventory.ReduceAmmo(totalAmmo, _AmmoType);
                _CanAttack = true;
                // TODO: Reduce action points
            }
        }

        if(Owner)
            Owner.FinishReload();

        _IsReloading = false;
    }

    public string GetFireTypeAsString()
    {
        if(_SingleFire)
        {
            return "Single";
        } else 
        {
            return "Burst";
        }
    }

    public override void UseItem()
    {
        FireWeapon();
    }



    public override int GetActionPointsUsed()
    {
        return _BurstFire ? _BurstFireAP : _SingleFireAP;
    }
}
