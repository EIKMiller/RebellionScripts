using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : TreeNode
{
    public FollowTarget(BehaviourTree tree) : base(tree)
    {
        TaskName = "Follow Target";
    }

    public override ETreeNodeState Run()
    {
        if(!_Tree.Owner)
            return ETreeNodeState.FAILURE;

        Blackboard bb = _Tree.Owner.GetBlackboard();
        if(bb != null)
        {
            float followDistance = bb.GetValueAsFloat("FollowTargetDistance");
            GameObject target = bb.GetValueAsGameObject("Target");
            if(target)
            {
                Vector3 targetPos = target.transform.position;
                Vector3 characterPos = _Tree.Owner.transform.position;
                if(Vector3.Distance(targetPos, characterPos) > followDistance)
                {
                    _Tree.Owner.SetMoveToLocation(targetPos);
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