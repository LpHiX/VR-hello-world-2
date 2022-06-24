using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridClone : MonoBehaviour
{
    public GameObject ClonedObject;
    public int X = 2;
    public int Y = 2;
    public int Z = 2;
    public float Separation = 0.3f;

    void Start()
    {
        for (int x = 0; x < X; x++)
        {
            for (int y = 0; y < Y; y++)
            {
                for (int z = 0; z < Z; z++)
                {
                    UnityEngine.Object clone = Instantiate(ClonedObject, transform.position + new Vector3(x, y, z) * Separation, Quaternion.identity);
                }
            }
        }
    }
}