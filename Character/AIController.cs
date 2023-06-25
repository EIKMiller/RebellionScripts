using System;
using System.Collections.Generic;
using UnityEngine;

public enum EPathFollowDirection
{
    FORWARDS,
    BACKWARDS
}

public class AIController : BaseCharacterController
{
    [Header("Path Settings")]
    [SerializeField] private PathController _ConnectedPath;                  // Path that the AI is currently following
    public PathController ConnectedPath { get => _ConnectedPath; }
    public bool CirclePath = false;
    public EPathFollowDirection FollowDirection = EPathFollowDirection.FORWARDS;


    protected override void Start()
    {
        base.Start();
        
    }
    
    protected override void SetupBlackboard()
    {
        base.SetupBlackboard();
        _Blackboard.SetValueAsBool("HasPathPoint", false);
        _Blackboard.SetValueAsInt("PathIndex", 0);
        _Blackboard.SetValueAsInt("WaitAtPathIndex", -1);               // If -1 don't wait at any
        _Blackboard.SetValueAsFloat("WaitTime", 2.0f);
        _Blackboard.SetValueAsFloat("FollowTargetDistance", 2.0f);

        if(_ConnectedPath)
        {
            _Blackboard.SetValueAsInt("WaitAtPathIndex", _ConnectedPath.WaitAtIndex);
            _Blackboard.SetValueAsBool("HasPathPoint", true);
        }
            
    }

    private void Update()
    {

    }

    public bool HasLineOfSight(BaseCharacterController target)
    {
        return false;
    }

    public override void SetMoveToLocation(Vector3 moveToLocation)
    {
        _Agent.isStopped = false;
        _Agent.SetDestination(moveToLocation);
        if(_Anim)
            _Anim.SetBool("IsMoving", true);

    }

    public override void StopMoving()
    {
        _Agent.isStopped = true;
        _Blackboard.SetValueAsBool("HasMoveToLocation", false);

        if(_Anim)
            _Anim.SetBool("IsMoving", false);
    }

    public bool HasPath()
    {
        if(_ConnectedPath)
            return true;

        return false;
    }
}