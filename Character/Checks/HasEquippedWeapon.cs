using System;
using System.Collections.Generic;
using UnityEngine;

public class HasEquippedWeapon : TreeNode
{
    public HasEquippedWeapon(BehaviourTree tree) : base(tree)
    {
        TaskName = "Has Equipped Weapon";
    }

    public override ETreeNodeState Run()
    {
        if(!_Tree.Owner)
            return ETreeNodeState.FAILURE;

        InventoryController inv = _Tree.Owner.Inventory;
        if(inv != null)
        {
            if(inv.EquippedWeapon1 == null)
            {
                return ETreeNodeState.FAILURE;
            } else if(inv.EquippedWeapon2 == null)
            {
                return ETreeNodeState.FAILURE;
            }
        }

        return ETreeNodeState.SUCCESS;
    }
}