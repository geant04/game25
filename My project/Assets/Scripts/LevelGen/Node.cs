using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class Node {
    private List<Node> childrenNodeList;

    public List<Node> ChildrenNodeList { get => childrenNodeList; }

    public bool Visited { get; set; }

    public Vector2Int BottomLeftAreaCorner { get; set; }
    public Vector2Int BottomRightAreaCorner { get; set; }
    public Vector2Int TopLeftAreaCorner { get; set; }
    public Vector2Int TopRightAreaCorner { get; set; }
    
    public Node Parent { get; set; }

    public int TreeLayerIndex {get; set;}

    public Node(Node parentNode) {
        childrenNodeList = new List<Node>();
        this.Parent = parentNode;
        if (parentNode != null) {
            parentNode.AddChild(this);
        }
    }

    private void AddChild(Node child) {
        childrenNodeList.Add(child);
    }

    private void RemoveChild(Node child) {
        childrenNodeList.Remove(child);
    }
}