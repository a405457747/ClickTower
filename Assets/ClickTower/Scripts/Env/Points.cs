using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class Points : MonoSingleton<Points>
{
    [SerializeField]
    public List<Transform> PointsList;

    private void Awake()
    {
        PointsList = new List<Transform>();
        foreach (Transform pointTrans in this.transform)
        {
            PointsList.Add(pointTrans);
        }
    }
}
