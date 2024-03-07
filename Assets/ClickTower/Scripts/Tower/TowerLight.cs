using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLight : MonoBehaviour
{
    private SpriteRenderer lightSr;
    private Tower tower;

    private void Awake()
    {
        lightSr = transform.Find("TowerLight").GetComponent<SpriteRenderer>();
        tower = GetComponent<Tower>();
    }

    private void Update()
    {
        if (tower.PackingNum == tower.MaxPackingNum)
        {
            lightSr.enabled = true;
        }
        else
        {
            lightSr.enabled = false;
        }
    }
}
