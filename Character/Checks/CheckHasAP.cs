using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckHasAP : TreeNode
{
    public CheckHasAP(BehaviourTree tree) : base(tree)
    {
        TaskName = "Check Has AP";
    }

    public CheckHasAP(BehaviourTree tree, List<TreeNode> children) : base(tree, children)
    {
        TaskName = "Check Has AP";
    }

    public override ETreeNodeState Run()
    {
        if(_Tree.Debug)
            UpdateDebugger();

        Blackboard bb = _Tree.Owner.GetBlackboard();
        if(bb != null)
        {
            int actionPoints = bb.GetValueAsInt("ActionPoints");
            if(actionPoints > 0)
            {
                WeaponController equippedWeapon = _Tree.Owner.Inventory.GetInHandWeapon();
                if(equippedWeapon != null)
                {
                    int requiredPoints = equippedWeapon.ActionPointsRequired;
                    if(actionPoints >= requiredPoints)
                        return ETreeNodeState.SUCCESS;
                }
            }
                
        }

        return ETreeNodeState.FAILURE;
    }
}