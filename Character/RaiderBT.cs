using System;
using System.Collections.Generic;
using UnityEngine;

public class RaiderBT : BehaviourTree
{
    protected override TreeNode CreateTree()
    {
        return new Selector(this, new List<TreeNode>
        {
            new Sequence(this, new List<TreeNode>
            {
                new CheckHasTarget(this),
                new FollowTarget(this)
            }),
            new Sequence(this, new List<TreeNode>
            {
                new CheckHasMoveToLocation(this),
                new MoveToLocation(this)
            }),
            new Sequence(this, new List<TreeNode> 
            {
                new HasEquippedWeapon(this),
                new CheckHasTarget(this),
                new CheckAttackDistance(this),
                new CheckCanFire(this),
                new CheckHasLineOfSight(this),
                new FireAtTarget(this)
            }),
            new Sequence(this, new List<TreeNode> 
            {
                new CheckHasPath(this),
                new GetNodePathPoint(this),
                new MoveToPathPoint(this),
                new Wait(this),
                new UpdatePathIndex(this)
            })
        });
    }
}