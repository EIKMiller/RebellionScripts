using System;
using System.Collections.Generic;
using UnityEngine;

public class GetNodePathPoint : TreeNode
{
    public GetNodePathPoint(BehaviourTree tree) : base(tree)
    {
    }

    public override ETreeNodeState Run()
    {
        if(!_Tree.Owner)
            return ETreeNodeState.FAILURE;

        Blackboard bb = _Tree.Owner.GetBlackboard();

        if(bb != null)
        {
            if(_Tree.Owner is AIController ai)
            {
                if(ai.HasPath())
                {
                    int pathIndex = bb.GetValueAsInt("PathIndex");
                    PathController path = ai.ConnectedPath;
                    if(path != null)
                    {
                        Vector3 moveToLocation = path.GetPointPosition(pathIndex);
                        if(moveToLocation != Vector3.zero)
                        {
                            bb.SetValueAsVector("MoveToLocation", moveToLocation);
                            return ETreeNodeState.SUCCESS;
                        }
                    }
                }
            }
        }

        return ETreeNodeState.FAILURE;
    }
}