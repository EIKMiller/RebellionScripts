using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckWithinRange : TreeNode
{
    public CheckWithinRange(BehaviourTree tree) : base(tree)
    {
        TaskName = "Check Within Range";
    }

    public CheckWithinRange(BehaviourTree tree, List<TreeNode> children) : base(tree, children)
    {
        TaskName = "Check Within Range";
    }

    public override ETreeNodeState Run()
    {
        if(_Tree.Debug)
            UpdateDebugger();

        BaseCharacterController owner = _Tree.Owner;
        if(owner != null)
        {
            WeaponController equippedWeapon = owner.Inventory.GetInHandWeapon();
            if(equippedWeapon != null)
            {
                if(!equippedWeapon.HasAttackDistance)
                {
                    return ETreeNodeState.SUCCESS;
                } else 
                {
                    Blackboard bb = _Tree.GetBlackboard();
                    if(bb != null)
                    {
                        GameObject target = bb.GetValueAsGameObject("Target");
                        if(target != null)
                        {
                            Vector3 targetPos = target.transform.position;
                            Vector3 ownerPos = owner.transform.position;
                            if(Vector3.Distance(targetPos, ownerPos) <  equippedWeapon.MaxAttackDistance)
                            {
                                return ETreeNodeState.SUCCESS;
                            }
                        }
                    }
                }
            }
        }

        return ETreeNodeState.FAILURE;
    }
}