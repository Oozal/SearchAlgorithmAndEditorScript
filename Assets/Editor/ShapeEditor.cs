using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeHandler))]
public class ShapeEditor : Editor
{
    ShapeHandler shapeHandler;
    Event guiEvent;
    Shape currentShape;
    SelectionManager selectionManager;
    Vector3 initialPos;
    bool posRecorded;
    private void OnEnable()
    {
        shapeHandler = (ShapeHandler)target;
        selectionManager = new SelectionManager();
        
    }

    private void OnSceneGUI()
    {
        guiEvent = Event.current;
       if(guiEvent.type == EventType.MouseDown && guiEvent.button==0 && guiEvent.modifiers==EventModifiers.None)
        {
            Undo.RecordObject(shapeHandler, "Addpoint");
            if(shapeHandler.shapes.Count==0)
            {
                Debug.Log("ShapeCreated");

                CreateShape();
                
            }

            Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mouseRay, out hit))
            {
                if (!CheckPointClicked(hit.point))
                {
                    CreatePoint(hit.point);
                }

            }
            else
                selectionManager.pointSelectedIndex = -1;
        }

       else if(guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.Shift)
        {
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mouseRay, out hit))
            {
                CreateShape();
                CreatePoint(hit.point);
                

            }
            else
                selectionManager.pointSelectedIndex = -1;

        }

        else if(guiEvent.type == EventType.MouseDrag && guiEvent.button==0 && selectionManager.pointSelectedIndex!=-1 && guiEvent.modifiers==EventModifiers.None)
        {
            MovePoint();
        }
        
       if(guiEvent.type ==EventType.MouseUp && guiEvent.button ==0 && selectionManager.pointSelectedIndex!=-1)
        {
            Vector3 tempPos = currentShape.nodes[selectionManager.pointSelectedIndex].pos;
            currentShape.nodes[selectionManager.pointSelectedIndex].pos = initialPos;
            Undo.RecordObject(shapeHandler, "PointMoved");
            currentShape.nodes[selectionManager.pointSelectedIndex].pos = tempPos;
            posRecorded = false;



            selectionManager.pointSelectedIndex = -1;
        }

       if(guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.Control)
        {
            DeletePoint();
        }

        if (guiEvent.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }

       
        DrawPoints();
    }

    public void DeletePoint()
    {
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit))
        {
            if (CheckPointClicked(hit.point))
            {
                Undo.RecordObject(shapeHandler, "Point Deleted");
                currentShape.nodes.RemoveAt(selectionManager.pointSelectedIndex);
                selectionManager.pointSelectedIndex = -1;
            }

        }
    }
    public void CreatePoint( Vector3 pos)
    {
        currentShape = shapeHandler.shapes[selectionManager.shapeSelectedIndex];
        float minDist = 100000;
        int pointIndex = 0;
        int numPoints = currentShape.nodes.Count;
        Vertices newVertices = new Vertices();
        if (numPoints > 2)

        {
            for (int j = 0; j < numPoints; j++)
            {
                int lineToIndex = (j + 1) % numPoints;
                float lineDist = HandleUtility.DistancePointLine(pos.GetXZ(), currentShape.nodes[j].pos.GetXZ(),
                    currentShape.nodes[lineToIndex].pos.GetXZ());

                if (minDist > lineDist)
                {
                    minDist = lineDist;
                    pointIndex = j + 1;
                }


            }
            
            newVertices.pos = pos;
            currentShape.nodes.Insert(pointIndex, newVertices);
            currentShape.points.Insert(pointIndex, pos);
            selectionManager.pointSelectedIndex = pointIndex;
        }
        else
        {
            newVertices.pos = pos;
            currentShape.nodes.Add(newVertices);
            currentShape.points.Add(pos);
            selectionManager.pointSelectedIndex = currentShape.points.Count - 1;
        }

        
        HandleUtility.Repaint();
    }


    
    void DrawPoints()
    {
        if (currentShape != null)
        {
            Color col;
            int numShape = shapeHandler.shapes.Count;
            for (int j = 0; j < numShape; j++)
            {
                if (j == selectionManager.shapeSelectedIndex)
                {
                    col = Color.white;
                }
                else
                    col = Color.gray;
                Shape s = shapeHandler.shapes[j];
                int n = s.nodes.Count;
                if (n > 0)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (i == selectionManager.pointSelectedIndex &&j== selectionManager.shapeSelectedIndex)
                            Handles.color = Color.red * col;
                        else
                            Handles.color = Color.white * col;
                        Handles.DrawSolidDisc(s.nodes[i].pos, Vector3.up, shapeHandler.diskRadius);
                        if (n >= 2)
                        {
                            int joinToIndex = (i + 1) % n;
                            Handles.color = Color.cyan * col;

                            Handles.DrawDottedLine(s.nodes[i].pos, s.nodes[joinToIndex].pos, 4);
                        }
                    }
                }
            }
        }

    }

    public void CreateShape()
    {
       
        
        currentShape = new Shape();
        shapeHandler.shapes.Add(currentShape);
        selectionManager.shapeSelectedIndex = shapeHandler.shapes.Count - 1;
        currentShape.points = new List<Vector3>();

    }

    public bool CheckPointClicked(Vector3 position)
    {
        int numShape = shapeHandler.shapes.Count;
        float radius = shapeHandler.diskRadius;
        for (int i = 0; i < numShape; i++)
        {
            Shape s = shapeHandler.shapes[i];
            int numPoints = s.nodes.Count;
            for (int j = 0; j < numPoints; j++)
            {
                if (Vector3.Distance(s.nodes[j].pos, position)<=radius*2)
                {
                    selectionManager.pointSelectedIndex = j;
                    selectionManager.shapeSelectedIndex = i;
                    currentShape = shapeHandler.shapes[selectionManager.shapeSelectedIndex];
                    shapeHandler.selectedIndex = i;
                    return true;
                }
            }
        }
        return false;
    }
   

    void MovePoint()
    {
        
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        RaycastHit hit;
        if(!posRecorded)
        {
            initialPos = currentShape.nodes[selectionManager.pointSelectedIndex].pos;
            posRecorded = true;
        }
       
        if (Physics.Raycast(mouseRay, out hit))
        {
            
            Vector3 pos = hit.point;
            currentShape.nodes[selectionManager.pointSelectedIndex].pos = pos;
            currentShape.points[selectionManager.pointSelectedIndex] = pos;
        }
    }
}


public class SelectionManager
{
    public int pointSelectedIndex = -1;
    public int shapeSelectedIndex = -1;
    

}

public static class Extension
{
    public static Vector2 GetXZ(this Vector3 vec)
    {
        Vector2 xz = new Vector2(vec.x, vec.z);
        return xz;
    }
}