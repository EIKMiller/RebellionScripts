using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckCanFire : TreeNode
{
    public CheckCanFire(BehaviourTree tree) : base(tree)
    {
        TaskName = "Check Can Fire";
    }

    public CheckCanFire(BehaviourTree tree, List<TreeNode> children) : base(tree, children)
    {
        TaskName = "Check Can Fire";
    }

    public override ETreeNodeState Run()
    {
        if(_Tree.Debug)
            UpdateDebugger();

        Blackboard bb = _Tree.Owner.GetBlackboard();
        if(bb != null)
        {
            if(bb.GetValueAsBool("CanFire"))
                return ETreeNodeState.SUCCESS;
        }

        return ETreeNodeState.FAILURE;
    }
}