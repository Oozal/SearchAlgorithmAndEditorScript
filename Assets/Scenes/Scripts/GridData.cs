using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "data",menuName ="GridData")]
public class GridData : ScriptableObject
{
    public Vector3[,] pos;
}
