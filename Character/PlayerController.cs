using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseCharacterController
{

    [Header("Animations")]
    [SerializeField] private PlayerAnimationHelper _AnimHelper;         // Reference to the animation helper

    private Camera _Cam;                    // Reference to the main game camera

    private bool _InventoryOpen = false;            // Flag if the inventory is open or not

    [Header("Raycast Layers")]
    [SerializeField] private LayerMask _MoveToLayer;
    [SerializeField] private LayerMask _EnemyLayer;


    [Header("Cursors")]
    [SerializeField] private Texture2D _MoveToCursor;
    [SerializeField] private Texture2D _ShootCursor;

    [Header("Debug")]
    public WeaponController _DebugItem1;
    public WeaponController _DebugItem2;

    public InventoryController Inventory { get => _Inventory; }

    [Header("UI")]
    [SerializeField] private GameObject _UIObject;
    [SerializeField] private HUDController _HudController;
    public HUDController HudController { get => _HudController; }
    

    protected override void Awake()
    {
        base.Awake();
        // Get reference to the animation helper if we haven't already
        if(!_AnimHelper)
            _AnimHelper = this.GetComponent<PlayerAnimationHelper>();

        // Get reference to the HUD controller
        if(!_HudController)
            if(_UIObject)
                _HudController = _UIObject.GetComponent<HUDController>();
    }

    protected override void Start()
    {
        base.Start();
        _Cam = Camera.main;             // Get reference to the main camera

        _Inventory._GameCamera = Camera.main;           // Set the camera in the inventory

        // === DEBUBG === //
        if(_DebugItem1 != null)
            _Inventory.AddItem(_DebugItem1);

        if(_DebugItem2 != null)
            _Inventory.AddItem(_DebugItem2);
    }


    // Update is called once per frame
    void Update()
    {
        RaycastMovement();
        RaycastCursor();
        RaycastSetTarget();
        

        // Trigger running in the animator
        if(_Anim)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                _Anim.SetBool("IsRunning", true);
                _Agent.speed = _RunSpeed;
            } else 
            {
                _Anim.SetBool("IsRunning", false);
                _Agent.speed = _WalkSpeed;
            }
        }


        // === Trigger Holstering and Equip of the equiped item 1
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleSlotOne();
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            ToggleSlotTwo();
        }

        if(Input.GetKeyDown(KeyCode.R))
            StartReload();

        ToggleInventory();
    }

    public void ToggleSlotOne()
    {
        // Validate the equipped item
        if(_Inventory.EquippedWeapon1 != null)
        {
            // Check if it is holstered
            // If it is than start equipping
            // otherwise holster the item
            if(_Inventory.EquippedWeapon1.IsHolstered)
            {
                // Check if weapon 2 is equipped if it is than holster it and equip
                // Otherwise just equip the item
                if(_Inventory.EquippedWeapon2 && _Inventory.EquippedWeapon2.IsEquipped)
                {
                    _AnimHelper.TriggerHolster(2, _Inventory.EquippedWeapon1.GetAnimState(), 1);
                } else 
                {
                    _AnimHelper.TriggerGetWeapon(1, _Inventory.EquippedWeapon1.GetAnimState());
                }
                
            } else 
            {
                _AnimHelper.TriggerHolster(1, 0, 0);            // Holster the item
            }
        }
    }

    public void ToggleSlotTwo()
    {
        if(_Inventory.EquippedWeapon2 != null)
        {
            if(_Inventory.EquippedWeapon2.IsHolstered)
            {
                if(_Inventory.EquippedWeapon1 && _Inventory.EquippedWeapon1.IsEquipped)
                {
                    _AnimHelper.TriggerHolster(1, _Inventory.EquippedWeapon2.GetAnimState(), 2);
                } else 
                {
                    _AnimHelper.TriggerGetWeapon(2, _Inventory.EquippedWeapon2.GetAnimState());
                }
            } else 
            {
                _AnimHelper.TriggerHolster(2, 0, 0);
            }
        }
    }

    private void RaycastMovement()
    {
        if(!_Cam || _InventoryOpen)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = _Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, _MoveToLayer))
            {
                if(hit.collider.CompareTag("Ground"))
                {
                    _Blackboard.SetValueAsVector("MoveToLocation", hit.point);
                    _Blackboard.SetValueAsBool("HasMoveToLocation", true);
                }
            }
        }
    }

    private void RaycastSetTarget()
    {
        if(!_Cam || _InventoryOpen)
            return;

        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = _Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, _EnemyLayer))
            {
                if(hit.collider)
                {
                    if(_Blackboard.GetValueAsGameObject("Target") == hit.collider.gameObject)
                    {
                        SetTarget(null);
                    } else 
                    {
                        SetTarget(hit.collider.gameObject);
                    }
                }
            }
        }
    }

    private void RaycastCursor()
    {
        if(!_Cam)
            return;

        Ray ray = _Cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.CompareTag("Ground"))
            {
                Cursor.SetCursor(_MoveToCursor, Vector2.zero, CursorMode.Auto);
            } else if(hit.collider.CompareTag("Enemy"))
            {
                Cursor.SetCursor(_ShootCursor, Vector2.zero, CursorMode.Auto);
            } 
            else 
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }
    }

    private void StartReload()
    {
        GunController weapon = null;                // Pre define the gun we want

        // Validate inventory
        if(_Inventory != null)
        {
            // Determine which can to reload if any
            if(_Inventory.EquippedWeapon1.IsInHand)
            {
                if(_Inventory.EquippedWeapon1 is GunController gun)
                    weapon = gun;
            } else if(_Inventory.EquippedWeapon2.IsInHand)
            {
                if(_Inventory.EquippedWeapon2 is GunController gun)
                    weapon = gun;
            }
        }

        // Ensure we have a gun then start reloading
        if(weapon != null)
        {
            weapon.StartReload();
        }
    }

    private void ToggleInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            _InventoryOpen = !_InventoryOpen;
            if(_InventoryOpen)
            {
                _Inventory.Open();
            } else 
            {
                _Inventory.Close();
            }
        }
    }
}
