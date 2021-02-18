using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GridCreator : MonoBehaviour
{
    public float gridRadius;
    public Vector2 gridSize;
    [SerializeField]
   
    public Node[,] allNodes;
    public float gridDiameter;
    Vector3 startingPos, maximumPos;
    public int numX, numY;
    public int positionCount, boolCount;
    public bool gridInitialized;
    Camera cam;
    public List<Node> path;
    public bool isSettingWalkable;
    public bool drawGizmos;
    private void Awake()
    {
        Initialize();
        cam = Camera.main;
    }

  
    public float GetYpos
    {
        get
        {
            return transform.position.z;
        }

    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isSettingWalkable)
        {
            Ray r = cam.ScreenPointToRay(Input.mousePosition);
            float rayDist = (GetYpos - r.origin.y) / r.direction.y;
            Vector3 pos = r.GetPoint(rayDist);
            Vector2Int index = GetIndexFromPos(pos);
            allNodes[index.x, index.y].isWalkable = false;
        }
    }
    public void Initialize()
    {
        Ray r = Camera.main.ScreenPointToRay(new Vector3(200, 200, 0));
        
        gridDiameter = gridRadius * 2;
        numX = Mathf.RoundToInt(gridSize.x / gridDiameter);
        numY = Mathf.RoundToInt(gridSize.y / gridDiameter);

      
        allNodes = new Node[numX, numY];
        startingPos = transform.position - new Vector3((gridSize.x / 2)-gridRadius, 0, (gridSize.y / 2)-gridRadius);
        maximumPos = startingPos + new Vector3(numX * gridDiameter, 0, numY * gridDiameter);
        for(int i=0;i<numX;i++)
        {
            for(int j = 0;j<numY;j++)
            {
              
                Vector3 currentPos = new Vector3(startingPos.x + i * gridDiameter, transform.position.y,
                    startingPos.z + j * gridDiameter);
                allNodes[i, j] = new Node(currentPos,i,j, true);
               
            }
        }

       
    }

    public int GetSize()
    {
        
        return numX * numY;
    }
    public Vector2Int GetIndexFromPos(Vector3 pos)
    {
        float percentX = (pos.x - startingPos.x) / (maximumPos.x - startingPos.x);
        percentX = Mathf.Clamp01(percentX);
        float percentY = (pos.z - startingPos.z) / (maximumPos.z - startingPos.z);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt(numX  * percentX);
        int y = Mathf.RoundToInt(numY  * percentY);
       
        return (new Vector2Int(x, y));
    }

    public List<Node> FindNeighbourNode(int x_index, int y_index)
    {
        List<Node> neighbourNode = new List<Node>();
        for(int i=-1;i<=1;i++)
        {
            for(int j=-1;j<=1;j++)
            {
                if(i==0 && j ==0)
                    continue;
                int x = x_index + i;
                int y = y_index + j;
                Debug.Log("AllneighbourIndex: " + x + ", " + y);
                if (x >= 0 && x < numX && y>=0 && y< numY)
                {
                    Debug.Log("Adding neighbour");
                    Debug.Log("neighbourIndex: " + x + ", " + y);
                    neighbourNode.Add(allNodes[x, y]);
                   
                }
            }
        }
        return neighbourNode;
    }
    public void SetNonWalkable(Vector3 pos)
    {
        Vector2Int index = GetIndexFromPos(pos);

        allNodes[index.x, index.y].isWalkable = false;
    }

    private void OnDrawGizmos()
    {
      if(drawGizmos)
            foreach (Node n in allNodes)
            {
                if (n.isWalkable)
                {
                    Gizmos.color = Color.white;
                }
                else
                    Gizmos.color = Color.red;
                if (path != null && path.Contains(n))
                    Gizmos.color = Color.black;

                Gizmos.DrawCube(n.myPos, Vector3.one * gridDiameter * 0.8f);
            }
        
    }


}
