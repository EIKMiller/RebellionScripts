using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPathPoint : TreeNode
{
    private const float STOPPING_DISTANCE = 1.0f;
    public MoveToPathPoint(BehaviourTree tree) : base(tree)
    {
        TaskName = "Move To Path Point";
    }

    public override ETreeNodeState Run()
    {
        if(_Tree.Debug)
            this.UpdateDebugger();
        
        Debug.Log("Moving To Path point");

        if(_Tree.Owner == null)
            return ETreeNodeState.FAILURE;

        Blackboard bb = _Tree.Owner.GetBlackboard();

        if(bb != null)
        {
            Vector3 moveToLocation = bb.GetValueAsVector("MoveToLocation");
            Vector3 characterPos = _Tree.Owner.transform.position;

            if(moveToLocation != Vector3.zero)
            {
                if(Vector3.Distance(moveToLocation, characterPos) > STOPPING_DISTANCE)
                {
                    _Tree.Owner.SetMoveToLocation(moveToLocation);
                    return ETreeNodeState.RUNNING;
                } else 
                {
                    return ETreeNodeState.SUCCESS;
                }
            }
        }

        return ETreeNodeState.FAILURE;
    }
}