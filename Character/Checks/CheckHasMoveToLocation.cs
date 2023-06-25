using UnityEngine;
using System.Collections.Generic;
using System;

public class CheckHasMoveToLocation : TreeNode
{
    public CheckHasMoveToLocation(BehaviourTree owner) : base(owner)
    {
        TaskName = "Check Has Move To Location";
    }

    public CheckHasMoveToLocation(BehaviourTree owner, List<TreeNode> children) : base(owner, children)
    {
        TaskName = "Check Has Move To Location";
    }

    public override ETreeNodeState Run()
    {
        if(_Tree.Debug)
            UpdateDebugger();
            
        Blackboard bb = _Tree.Owner.GetBlackboard();
        if(bb != null)
        {
            bool hasMoveToLocation = bb.GetValueAsBool("HasMoveToLocation");
            if(hasMoveToLocation)
            {
                
                return ETreeNodeState.SUCCESS;
            } else 
            {
                return ETreeNodeState.FAILURE;
            }
        }

        return ETreeNodeState.FAILURE;
    }
}