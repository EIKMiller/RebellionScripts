using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETreeNodeState
{
    SUCCESS = 0,
    RUNNING = 1,
    FAILURE = 2
}

public abstract class TreeNode
{
    public string TaskName;
    public TreeNode Parent;                 // Reference to the parent node
    protected List<TreeNode> _Children = new List<TreeNode>();          // Reference to all the children nodes that we need to run
    private BehaviorTreeDebugger _Debugger;

    // Reference to the owing BT
    protected BehaviourTree _Tree;
    public BehaviourTree Tree { get => _Tree; }

    public TreeNode(BehaviourTree tree)
    {
        _Tree = tree;
        _Debugger = _Tree.Owner._BTDebugger;
    }

    public TreeNode(BehaviourTree tree, List<TreeNode> children)
    {
        _Tree = tree;
        AddChildren(children);
        _Debugger = _Tree.Owner._BTDebugger;
    }

    /// <summary>
    /// Adds a new child to the tree node and sets it parent
    /// </summary>
    /// <param name="child"></param>
    public void AddChild(TreeNode child)
    {
        _Children.Add(child);
        child.Parent = this;
    }

    public void AddChildren(List<TreeNode> children)
    {
        foreach(var child in children)
            AddChild(child);
    }

    public abstract ETreeNodeState Run();

    protected void UpdateDebugger()
    {
        _Debugger.CurrentTaskName = TaskName;
    }
}
