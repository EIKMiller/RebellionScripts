using UnityEngine;
using System;
using System.Collections.Generic;

public class BlackboardDebugger : MonoBehaviour
{
    public bool HasMoveToLocation;
    public Vector3 MoveToLocation;
    public GameObject Target;
    public bool CanFire;

    private Blackboard _Blackboard;

    private void Start()
    {
        _Blackboard = this.GetComponent<BaseCharacterController>().GetBlackboard();
    }

    private void Update()
    {
        if(_Blackboard != null)
        {
            HasMoveToLocation = _Blackboard.GetValueAsBool("HasMoveToLocation");
            MoveToLocation = _Blackboard.GetValueAsVector("MoveToLocation");
            Target = _Blackboard.GetValueAsGameObject("Target");
            CanFire = _Blackboard.GetValueAsBool("CanFire");
        } else 
        {
            _Blackboard = this.GetComponent<BaseCharacterController>().GetBlackboard();
        }
    }
}