using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IheapItem<Node>
{
    public int gCost, hCost;
    public Vector3 myPos;
    public int x_index, y_index;
    int _heapIndex;
    public bool isWalkable;

    public Node parentNode;
    public  int Fcost { get
        {
            return gCost + hCost;
        }
    }

    public int heapIndex { 
        get
        {
            return _heapIndex;
        }
        set
        {
            _heapIndex = heapIndex;
        }

        }

    public  Node(Vector3 pos,int _xindex,int _yindex, bool _isWalkable)
    {
        myPos = pos;
        isWalkable = _isWalkable;
        x_index = _xindex;
        y_index = _yindex;
    }

    public int CompareTo(Node nodeToCompare)
    {
        if (Fcost == nodeToCompare.Fcost)
        {
            if (hCost < nodeToCompare.hCost)
            {
                return 1;
            }
            else return -1;
        }

        else if (Fcost < nodeToCompare.Fcost)
            return 1;
        else
            return 0;
       
    }
}
