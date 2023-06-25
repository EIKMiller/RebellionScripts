using System;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeDebugger : MonoBehaviour
{
    public BehaviourTree Tree;
    public string CurrentTaskName;
    public string LogAtTask;
    public string DebugMessage;

    public void Update()
    {
        if(!Tree || !Tree.Debug)
            return;


        if(CurrentTaskName == LogAtTask)
        {
            if(DebugMessage == "")
            {
                Debug.Log(CurrentTaskName);
            } else 
            {
                Debug.Log(DebugMessage);
            }
        }
    }
}