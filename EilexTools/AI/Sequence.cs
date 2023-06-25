using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : TreeNode
{
    public Sequence(BehaviourTree owner) : base(owner)
    {
    }

    public Sequence(BehaviourTree tree, List<TreeNode> children) : base(tree, children)
    {

    }

    public override ETreeNodeState Run()
    {
        foreach(var node in _Children)
        {
            switch(node.Run())
            {
                case ETreeNodeState.SUCCESS:
                    continue;
                case ETreeNodeState.RUNNING:
                    return ETreeNodeState.RUNNING;
                case ETreeNodeState.FAILURE:
                    return ETreeNodeState.FAILURE;
            }
        }

        return ETreeNodeState.RUNNING;
    }
}
