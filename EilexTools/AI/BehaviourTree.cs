using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{
    private TreeNode _RootNode;
    public BaseCharacterController Owner;
    
    [SerializeField] private bool _Debug = false;
    public bool Debug { get => _Debug; }

    protected void Awake()
    {
        _RootNode = CreateTree();
    }

    protected void Update()
    {
        if(_RootNode != null)
            _RootNode.Run();
    }

    protected virtual TreeNode CreateTree()
    {
        return null;
    }

    public Blackboard GetBlackboard()
    {
        if(Owner)
        {
            return Owner.GetBlackboard();
        }

        return null;
    }
}
