using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerBT : BehaviourTree
{
    protected override TreeNode CreateTree()
    {
        return new Selector(this, new List<TreeNode> 
        {
            new Sequence(this, new List<TreeNode>
            {
                new CheckHasMoveToLocation(this),
                new MoveToLocation(this)
            }),
            new Sequence(this, new List<TreeNode>
            {
                new CheckHasTarget(this),
                new CheckCanFire(this),
                new CheckHasAP(this),
                new CheckWithinRange(this),
                new FireAtTarget(this)
            })
        });
    }
}