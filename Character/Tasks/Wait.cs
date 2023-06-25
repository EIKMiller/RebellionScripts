using System;
using System.Collections.Generic;
using UnityEngine;

public class Wait : TreeNode
{
    private float _CurrentTime = 0.0f;

    public Wait(BehaviourTree tree) : base(tree)
    {
        TaskName = "Wait";
    }

    public override ETreeNodeState Run()
    {
        if(_Tree.Debug)
            UpdateDebugger();

        if(!_Tree.Owner)
            return ETreeNodeState.FAILURE;
        
        Blackboard bb = _Tree.Owner.GetBlackboard();
        if(bb != null)
        {
            int waitAtIndex = bb.GetValueAsInt("WaitAtPathIndex");
            if(waitAtIndex == -1)
            {
                return ETreeNodeState.SUCCESS;
            } else 
            {
                int currentPathIndex = bb.GetValueAsInt("PathIndex");
                if(currentPathIndex == waitAtIndex)
                {
                    _CurrentTime += 1 * Time.deltaTime;
                    if(_CurrentTime > bb.GetValueAsFloat("WaitTime"))
                        return ETreeNodeState.SUCCESS;

                    
                    _Tree.Owner.StopMoving();
                    return ETreeNodeState.RUNNING;
                } else 
                {
                    return ETreeNodeState.SUCCESS;
                }
            }
        }

        return ETreeNodeState.FAILURE;
    }
}