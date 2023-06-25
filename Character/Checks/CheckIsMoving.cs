using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckIsMoving : TreeNode
{
    public CheckIsMoving(BehaviourTree tree) : base(tree)
    {
        TaskName = "Check Is Moving";
    }

    public CheckIsMoving(BehaviourTree tree, List<TreeNode> children) : base(tree, children)
    {
        TaskName = "Check Is Moving";
    }

    public override ETreeNodeState Run()
    {
        if(_Tree.Debug)
            UpdateDebugger();
            
        Blackboard bb = _Tree.Owner.GetBlackboard();
        if(bb != null)
        {
            if(bb.GetValueAsBool("HasMoveToLocation"))
                return ETreeNodeState.FAILURE;
        }

        return ETreeNodeState.SUCCESS;
    }
}