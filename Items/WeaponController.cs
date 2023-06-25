using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWeaponType
{
    RIFLE = 1,
    SMG = 2,
    PISTOL = 3,
    MELEE = 4
}

public abstract class WeaponController : BaseItem
{
    [Header("Item Details")]
    [SerializeField] protected EWeaponType _WeaponType;
    public EWeaponType WeaponType { get => _WeaponType; }
    public BaseCharacterController Owner;

    [Header("Damage Settings")]
    [SerializeField] protected float _Damage;
    public float Damage { get => _Damage; }

    [Header("Attack Settings")]
    [SerializeField] private bool _HasAttackDistance = false;
    public bool HasAttackDistance { get => _HasAttackDistance; }
    [SerializeField] private float _MaxAttackDistance;
    public float MaxAttackDistance { get => _MaxAttackDistance; }
    [SerializeField] protected bool _CanAttack = true;
    public bool CanAttack { get => _CanAttack; }
    [SerializeField] private float _AttackCooldown;
    
    [Header("Equipped Settings")]
    public bool IsEquipped = false;
    public bool IsHolstered = true;
    public bool IsInHand = false;

    [Header("Positioning")]
    [SerializeField] private Vector3 _InhandPosition;
    [SerializeField] private Vector3 _InHandRotation;
    [SerializeField] private Vector3 _InHandScale;
    [SerializeField] private Vector3 _InHolsterPostion;
    [SerializeField] private Vector3 _InHolsterRotation;
    [SerializeField] private Vector3 _InHolsterScale;
    [SerializeField] private bool _UseScale;                // If we need to change the scale

    [Header("Action Points")]
    [SerializeField] protected int _ActionPointsRequired;
    public int ActionPointsRequired { get => _ActionPointsRequired; }

    

    public void SetInHand()
    {
        if(IsHolstered)
        {
            StartCoroutine(SetHolstered());
        } else 
        {
            StartCoroutine(SetEquipped());
        }
    }

    public IEnumerator SetHolstered()
    {
        yield return new WaitForSeconds(0.2f);
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = _InHolsterRotation;
        this.transform.SetLocalPositionAndRotation(_InHolsterPostion, rotation);
        if(_UseScale)
            this.transform.localScale = _InHolsterScale;
        this.IsEquipped = false;
    }

    public IEnumerator SetEquipped()
    {
        yield return new WaitForSeconds(0.2f);
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = _InHandRotation;
        this.transform.SetLocalPositionAndRotation(_InhandPosition, rotation);
        if(_UseScale)
            this.transform.localScale = _InHandScale;
        this.IsEquipped = true;
    }

    protected IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(_AttackCooldown);
        _CanAttack = true;

        if(Owner != null)
            Owner.GetBlackboard()?.SetValueAsBool("CanFire", true);
    }

    public abstract int GetActionPointsUsed();

    public virtual int GetAnimState()
    {
        if(_WeaponType == EWeaponType.SMG)
            return 1;

        if(_WeaponType == EWeaponType.RIFLE)
            return 1;

        if(_WeaponType == EWeaponType.PISTOL)
            return 2;

        if(_WeaponType == EWeaponType.MELEE)
            return 3;


        return 0;
    }

    public float CalculateDamage(BaseCharacterController target, BodyPart part)
    {
        if(!target)
            return 0f;

        InventoryController targetInventory = target.Inventory;
        if(targetInventory == null)
            return 0f;

        float damageModifier = 1.0f;
        damageModifier += part.DamageMod;
        damageModifier -= part.ArmourMod;

        return this.Damage * damageModifier;
    }

}
