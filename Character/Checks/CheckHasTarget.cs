using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckHasTarget : TreeNode
{
    public CheckHasTarget(BehaviourTree tree) : base(tree)
    {
        TaskName = "Check Has Target";
    }

    public CheckHasTarget(BehaviourTree tree, List<TreeNode> children) : base(tree, children)
    {
        TaskName = "Check Has Target";
    }

    public override ETreeNodeState Run()
    {
        if(_Tree.Debug)
            UpdateDebugger();
            
        Blackboard bb = _Tree.Owner.GetBlackboard();
        if(bb != null)
        {
            GameObject target = bb.GetValueAsGameObject("Target");
            if(target)
            {
                return ETreeNodeState.SUCCESS;
            }
        }

        return ETreeNodeState.FAILURE;
    }
}