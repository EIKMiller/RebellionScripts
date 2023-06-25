using System;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePathIndex : TreeNode
{
    public UpdatePathIndex(BehaviourTree tree) : base(tree)
    {
        TaskName = "Update Path Index";
    }

    public override ETreeNodeState Run()
    {
        if(_Tree.Owner == null)
            return ETreeNodeState.FAILURE;

        Blackboard bb = _Tree.Owner.GetBlackboard();

        if(bb != null)
        {
            if(_Tree.Owner is AIController ai)
            {
                int currentPathIndex = bb.GetValueAsInt("PathIndex");
                bool circle = ai.CirclePath;
                int totalPathPoints = ai.ConnectedPath.TotalPathPoints;
                

                if(circle)
                {
                    EPathFollowDirection direction = ai.FollowDirection;
                    if(direction == EPathFollowDirection.FORWARDS)
                    {
                        currentPathIndex++;
                        if(currentPathIndex == totalPathPoints)
                        {
                            ai.FollowDirection = EPathFollowDirection.BACKWARDS;
                            currentPathIndex -= 2;
                        }
                    } else 
                    {
                        currentPathIndex--;
                        if(currentPathIndex < 0)
                        {
                            currentPathIndex += 2;
                            ai.FollowDirection = EPathFollowDirection.FORWARDS;
                        }
                    }
                } else 
                {
                    currentPathIndex++;
                    if(currentPathIndex == totalPathPoints)
                        currentPathIndex = 0;
                }

                bb.SetValueAsInt("PathIndex", currentPathIndex);
                return ETreeNodeState.SUCCESS;

            }
        }


        return ETreeNodeState.FAILURE;
    }
}