using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public enum EEquippedWeaponType
{
    UNARMED = 0,
    RIFLE = 1,
    SHOTGUN = 2,
    PISTOL = 3
}

public class BaseCharacterController : MonoBehaviour
{
    [Header("Inventory Settings")]
    [SerializeField] protected InventoryController _Inventory;
    public InventoryController Inventory { get => _Inventory; }
    public bool IsOverWeightLimit = false;

    [Header("Movement Settings")]
    [SerializeField] protected float _WalkSpeed;
    [SerializeField] protected float _RunSpeed;
    public bool _OverInventoryWeight = false;
    protected bool IsMoving = false;

    [Header("Animation")]
    [SerializeField] protected Animator _Anim;
    [SerializeField] protected EEquippedWeaponType _EquippedWeaponType = EEquippedWeaponType.UNARMED;
    
    [Header("AI")]
    [SerializeField] protected NavMeshAgent _Agent;
    public NavMeshAgent Agent => _Agent;
    [SerializeField] protected BehaviourTree _BTree;
    protected Blackboard _Blackboard;

    [Header("Attributes")]
    [SerializeField] private CharacterAttributes _Attributes;
    public CharacterAttributes Attributes { get => _Attributes; }
    [SerializeField] private float _MaxHealth;
    [SerializeField] private float _CurrentHealth;

    [SerializeField] private List<BodyPart> _BodyParts = new List<BodyPart>();

    public float CurrentHealth { get => _CurrentHealth; }

    [Header("Action Points")]
    [SerializeField] protected int _MaxActionPoints = 10;
    public int MaxActionPoints { get => _MaxActionPoints; }
    [SerializeField] protected int _CurrentActionPoints;
    public int CurrentActionPoints { get => _CurrentActionPoints; }
    [Space(5)]
    public UnityEvent ApEvents;

    [Header("Combat Settings")]
    [SerializeField] private float _FireWaitTime;
    public bool WeaponInHand = false;
    [SerializeField] private ParticleSystem _BloodParticle;


    [Header("Targeting")]
    private List<BaseCharacterController> _TargetedBy = new List<BaseCharacterController>();

    [Header("Line Of Sight Locations")]
    [SerializeField] protected Transform _SightPoint;
    public Transform SightPoint { get => _SightPoint; }
    [SerializeField] protected Transform _HeadPoint;
    public Transform HeadPoint { get => _HeadPoint; }
    [SerializeField] protected Transform _TorsoPoint;
    public Transform TorsoPoint { get => _TorsoPoint; }
    [SerializeField] protected Transform _FeetPoint;
    public Transform FeetPoint { get => _FeetPoint; }

    [Header("Debugging")]
    public BehaviorTreeDebugger _BTDebugger;

    protected virtual void Awake()
    {
        if(!_Agent)
            _Agent = GetComponent<NavMeshAgent>();

        if(!_BTree)
            _BTree = GetComponent<BehaviourTree>();

        if(!_Anim)
            _Anim = GetComponent<Animator>();

        _Inventory.Owner = this;
    }

    protected virtual void Start()
    {
        _CurrentActionPoints = _MaxActionPoints;
        SetupBlackboard();
        _CurrentHealth = _MaxHealth;
        SetupBodyParts();
        SpawnItemsInInventory();
    }

    private void SpawnItemsInInventory()
    {
        foreach(var item in _Inventory.SpawnWithItems)
        {
            BaseItem spawningItem = ItemDatabase.Instance?.GetItem(item);
            if(spawningItem)
            {
                GameObject go = GameObject.Instantiate(spawningItem.gameObject);
                if(go != null)
                {
                    _Inventory.AddItem(go.GetComponent<BaseItem>());
                }
            }
        }
    }
    
    /// <summary>
    /// Sets the default values for the blackboard
    /// </summary>
    protected virtual void SetupBlackboard()
    {
        _Blackboard = new Blackboard(this);
        _Blackboard.SetValueAsBool("HasMoveToLocation", false);
        _Blackboard.SetValueAsVector("MoveToLocation", Vector3.zero);
        _Blackboard.SetValueAsGameObject("Target", null);
        _Blackboard.SetValueAsBool("CanFire", true);
        _Blackboard.SetValueAsInt("ActionPoints", _CurrentActionPoints);
    }

    /// <summary>
    /// Creates the default body parts for the character
    /// </summary>
    private void SetupBodyParts()
    {
        BodyPart head = new BodyPart();
        head.Setup("Head", 0.2f, 2.3f);
        _BodyParts.Add(head);

        BodyPart torso = new BodyPart();
        torso.Setup("Torso", 0.5f, 1.4f);
        _BodyParts.Add(torso);

        BodyPart arms = new BodyPart();
        arms.Setup("Arms", 0.3f, 1.2f);
        _BodyParts.Add(arms);

        BodyPart legs = new BodyPart();
        legs.Setup("Legs", 0.3f, 1.2f);
        _BodyParts.Add(legs);

    }

    /// <summary>
    /// Applies damage to the character and checks if they are still alive
    /// </summary>
    /// <param name="dmgPoints">Damage Points to apply</param>
    public virtual void TakeDamage(float dmgPoints)
    {
        _CurrentHealth -= dmgPoints;
        _BloodParticle.Play();
        if(_CurrentHealth < 0)
        {
            ClearAllTargets();
            _Blackboard = null;
            _BTree = null;
        }
    }

    /// <summary>
    /// Add a character to who ever is targeting this character
    /// </summary>
    /// <param name="targetBy">New character targeting</param>
    public void BeingTargetedBy(BaseCharacterController targetBy) => _TargetedBy.Add(targetBy);

    /// <summary>
    /// Removes a character that is targeting this character
    /// </summary>
    /// <param name="targetBy"></param>
    private void RemoveTargetedBy(BaseCharacterController targetBy) => _TargetedBy.Remove(targetBy);

    /// <summary>
    /// Clears anyone targeting this character
    /// </summary>
    private void ClearAllTargets()
    {
        foreach(var target in _TargetedBy)
        {
            if(target != null)
                target.SetTarget(null);
        }
    }

    public void UseWeapon()
    {
        if(Inventory == null)
            return;

        WeaponController inUseWeapon = null;            // Predefine the weapon in use

        // Determine which weapon is in use
        if(Inventory.EquippedWeapon1.IsInHand)
            inUseWeapon = Inventory.EquippedWeapon1;
        else if(Inventory.EquippedWeapon2.IsInHand)
            inUseWeapon = Inventory.EquippedWeapon2;

        if(inUseWeapon == null)
            return;

        inUseWeapon.UseItem();
        
    }

    /// <summary>
    /// Updates the current target to remove this character, then assigns the new target (if it's not null)
    /// </summary>
    /// <param name="target">Character to target</param>
    protected void SetTarget(GameObject target)
    {
        if(_Blackboard != null)
        {
            BaseCharacterController prevTarget = _Blackboard.GetValueAsGameObject("Target")?.GetComponent<BaseCharacterController>();
            if(prevTarget)
            {
                prevTarget.RemoveTargetedBy(this);
            }
            _Blackboard.SetValueAsGameObject("Target", target);
            if(target)
                target.GetComponent<BaseCharacterController>()?.BeingTargetedBy(this);      

        }

        if(_Anim)
        {
            if(target)
            {
                _Anim.SetBool("HasTarget", true);
            } else 
            {
                _Anim.SetBool("HasTarget", false);
                
            }
        }
    }


    private IEnumerator ResetCanFire()
    {
        yield return new WaitForSeconds(_FireWaitTime);
    }

    public virtual void SetMoveToLocation(Vector3 moveToLocation)
    {
        _Agent.isStopped = false;
        _Agent.SetDestination(moveToLocation);
        if(_Anim)
            _Anim.SetBool("IsMoving", true);
    }

    public virtual void StopMoving()
    {
        _Agent.isStopped = true;
        _Blackboard.SetValueAsBool("HasMoveToLocation", false);
        _Blackboard.SetValueAsVector("MoveToLocation", Vector3.zero);

        if(_Anim)
            _Anim.SetBool("IsMoving", false);
    }

    public void TriggerReload()
    {
        if(!_Anim)
            return;

        _Anim.SetTrigger("Reload");
        _Blackboard.SetValueAsBool("CanFire", false);
    }

    public void FinishReload()
    {
        _Blackboard.SetValueAsBool("CanFire", true);
    }

    public void SwitchWeaponType(EEquippedWeaponType weapon)
    {
        _EquippedWeaponType = weapon;
        _Anim.SetInteger("EquippedWeapon", (int)_EquippedWeaponType);
    }

    public void ReduceAP(int amount)
    {
        _CurrentActionPoints -= amount;
        _Blackboard.SetValueAsInt("ActionPoints", _CurrentActionPoints);
        ApEvents?.Invoke();
    }

    public void IncreaseAP(int amount)
    {
        _CurrentActionPoints += amount;
        if(_CurrentActionPoints > _MaxActionPoints)
            _CurrentActionPoints = _MaxActionPoints;

        ApEvents?.Invoke();
    }

    public BodyPart GetRandomPart()
    {
        float totalWeight = 0f;

        foreach(var part in _BodyParts)
            totalWeight += part.Weight;

        float randValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        foreach(var part in _BodyParts)
        {
            cumulativeWeight += part.Weight;
            if(randValue < cumulativeWeight)
            {
                return part;
            }
        }

        return null;
    }

    public Blackboard GetBlackboard() => _Blackboard;
    public bool IsAlive() => _CurrentHealth > _MaxHealth;
}