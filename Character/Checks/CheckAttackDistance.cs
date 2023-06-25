using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckAttackDistance : TreeNode
{
    public CheckAttackDistance(BehaviourTree tree) : base(tree)
    {
        TaskName = "Check Attack Distance";
    }

    public CheckAttackDistance(BehaviourTree tree, List<TreeNode> children) : base(tree, children)
    {
        TaskName = "Check Attack Distance";
    }

    public override ETreeNodeState Run()
    {
        if(_Tree.Owner == null)
            return ETreeNodeState.FAILURE;

        Blackboard bb = _Tree.Owner.GetBlackboard();

        if(bb != null)
        {
            // Determine the weapon that is equipped
            WeaponController weapon = null;
            if(_Tree.Owner.Inventory.EquippedWeapon1.IsInHand)
            {
                weapon = _Tree.Owner.Inventory.EquippedWeapon1;
            } else if(_Tree.Owner.Inventory.EquippedWeapon2.IsInHand)
            {
                weapon = _Tree.Owner.Inventory.EquippedWeapon2;
            }

            if(weapon != null)
            {
                float weaponDistance = weapon.MaxAttackDistance;
                GameObject target = bb.GetValueAsGameObject("Target");
                if(target != null)
                {
                    Vector3 targetPos = target.transform.position;
                    Vector3 ownerPos = _Tree.Owner.transform.position;

                    if(Vector3.Distance(targetPos, ownerPos) < weaponDistance)
                        return ETreeNodeState.SUCCESS;
                }
            }
        }

        return ETreeNodeState.FAILURE;
    }
}