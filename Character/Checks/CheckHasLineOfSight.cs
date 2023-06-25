using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckHasLineOfSight : TreeNode
{
    public CheckHasLineOfSight(BehaviourTree tree) : base(tree)
    {
    }

    public override ETreeNodeState Run()
    {
        if(_Tree.Owner == null)
            return ETreeNodeState.FAILURE;

        Blackboard bb = _Tree.Owner.GetBlackboard();
        if(bb != null)
        {
            GameObject target = bb.GetValueAsGameObject("Target");
            if(target != null)
            {
                AIController aiCharacter = _Tree.Owner as AIController;
                BaseCharacterController targetController = target.GetComponent<BaseCharacterController>();
                if(aiCharacter && targetController)
                {
                    bool hasLine = aiCharacter.HasLineOfSight(targetController);
                    if(hasLine)
                        return ETreeNodeState.SUCCESS;
                }
            }
        }

        return ETreeNodeState.FAILURE;
    }
}