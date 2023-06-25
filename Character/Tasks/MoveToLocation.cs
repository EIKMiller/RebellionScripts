using UnityEngine;
using System;
using System.Collections.Generic;

public class MoveToLocation : TreeNode
{
    private const float STOPPING_DISTANCE = 2.0f;
    public MoveToLocation(BehaviourTree tree) : base(tree)
    {
        TaskName = "Move To Location";
    }

    public MoveToLocation(BehaviourTree tree, List<TreeNode> children) : base(tree, children)
    {
        TaskName = "Move To Location";
    }

    public override ETreeNodeState Run()
    {
        if(_Tree.Debug)
            UpdateDebugger();
            
        Blackboard bb = _Tree.Owner.GetBlackboard();
        if(bb != null)
        {
            Vector3 moveToLocation = bb.GetValueAsVector("MoveToLocation");
            Vector3 playerLocation = _Tree.Owner.transform.position;
            if(moveToLocation != Vector3.zero)
            {
                if(Vector3.Distance(moveToLocation, playerLocation) > STOPPING_DISTANCE)
                {
                    _Tree.Owner.SetMoveToLocation(moveToLocation);

                    return ETreeNodeState.RUNNING;
                } else 
                {
                    _Tree.Owner.StopMoving();
                    return ETreeNodeState.SUCCESS;
                }
            } else 
            {
                _Tree.Owner.StopMoving();
            }
        }

        return ETreeNodeState.SUCCESS;
    }
}