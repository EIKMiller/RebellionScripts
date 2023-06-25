using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckHasPath : TreeNode
{
    public CheckHasPath(BehaviourTree tree) : base(tree)
    {
        TaskName = "Check Has Path";
    }

    public override ETreeNodeState Run()
    {
        if(_Tree.Owner == null)
            return ETreeNodeState.FAILURE;

        if(_Tree.Owner is AIController controller)
            if(controller.HasPath())
                return ETreeNodeState.SUCCESS;


        return ETreeNodeState.FAILURE;
        
    }
}