using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeHandler : MonoBehaviour
{

    [Range(0.2f, 2)]
    public float diskRadius;
    public int selectedIndex;
    public List<Shape> shapes = new List<Shape>();
    public List<Vertices> totalPoints = new List<Vertices>();
}

[System.Serializable]
public class Shape
{
    public List<Vertices> nodes = new List<Vertices>();
    public List<Vector3> points = new List<Vector3>();
    public int shapeId;
}


[System.Serializable]
public class Vertices
{
    public Vector3 pos;
    public List<Vertices> neighbours;
}




