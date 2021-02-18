using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActualPathFinding : MonoBehaviour
{
    public Heap<Node> openSets;
    public HashSet<Node> closedSets;
    public GridCreator gridCreator;
    public Transform startPoint, target;
    Node targetNode, startNode;


  
    private void Update()
    {
        StartPathFinding();
    }
    public void StartPathFinding()
    {
        Vector2Int targetIndex = gridCreator.GetIndexFromPos(target.transform.position);
        targetNode = gridCreator.allNodes[targetIndex.x,targetIndex.y];

        Vector2Int startIndex = gridCreator.GetIndexFromPos(startPoint.transform.position);
        startNode = gridCreator.allNodes[startIndex.x, startIndex.y];

        openSets = new Heap<Node>(gridCreator.GetSize());
        closedSets = new HashSet<Node>();
        openSets.Add(startNode);
        Node currentNode;
        while(openSets.Count()>0)
        {

            currentNode = openSets.GetFirst();
           
            closedSets.Add(currentNode);


            if (currentNode ==targetNode)
            {
                
                gridCreator.path = TracePath(startNode, targetNode);
                return;
            }
            
            foreach(Node n in gridCreator.FindNeighbourNode(currentNode.x_index,currentNode.y_index))
            {
               
                if(n.isWalkable && !closedSets.Contains(n))
                {
                    
                    int newgCost = currentNode.gCost + FindDistanceToNode(currentNode, n);
                    if(n.gCost>newgCost || !openSets.Contains(n))
                    {
                        Debug.Log("updating neighbour");
                        n.gCost = newgCost;
                        n.hCost = FindDistanceToNode(n, targetNode);
                        n.parentNode = currentNode;
                        openSets.UpdateHeap(n);
                    }
                    if(!openSets.Contains(n))
                    {
                        Debug.Log("Adding in open set");
                        openSets.Add(n);
                    }
                }
            }
          
            
        }
    }


    public List<Node> TracePath(Node startNode,Node endNode)
    {
        Node currentNode = endNode;
        List<Node> path = new List<Node>();
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
            Debug.Log(currentNode.x_index + ", " + currentNode.y_index);

        }
        path.Reverse();
        return path;
    }
    public int FindDistanceToNode(Node firstNode, Node secondNode)
    {
        int x = Mathf.Abs(firstNode.x_index - secondNode.x_index);
        int y = Mathf.Abs(firstNode.y_index - secondNode.y_index);

        int dist = Mathf.Min(x, y) * 14 + Mathf.Abs(x - y) * 10;

        return dist;
    }
}
