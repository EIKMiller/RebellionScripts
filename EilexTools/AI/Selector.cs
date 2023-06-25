using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : TreeNode
{
    public Selector(BehaviourTree owner) : base(owner)
    {
    }

    public Selector(BehaviourTree owner, List<TreeNode> children) : base(owner, children) 
    {

    }

    public override ETreeNodeState Run()
    {
        foreach(var node in _Children)
        {
            switch(node.Run())
            {
                case ETreeNodeState.SUCCESS:
                    return ETreeNodeState.SUCCESS;
                case ETreeNodeState.RUNNING:
                    return ETreeNodeState.RUNNING;
                case ETreeNodeState.FAILURE:
                    continue;
            }
        }

        return ETreeNodeState.FAILURE;
    }
}
