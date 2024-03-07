using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using xmaolol.com;
using QFramework;

public class TowerT3 : Tower
{

    public override void InputKeyDown()
    {
        if (MainManager.Instance.CanDemolition == true)
        {
            DelTower();
        }
        else if (isPacking && PackingNum == MaxPackingNum)
        {
            StartCoroutine("ShootIenumerator");
        }
    }

    public override bool IsPacking
    {
        get => isPacking; set
        {
            isPacking = value;
            if (IsPacking == true)
            {
                for (int i = 0; i < MaxPackingNum; i++)
                {
                    GameObject bullet = GameObject.Instantiate(BulletRockBoy, transform.position, Quaternion.identity);
                    SetParentTrans(bullet.transform, bulletParent);
                    willBullet.Add(bullet.GetComponent<Bullet>());
                }
            }
            else
            {
                this.Delay(ShootCD, PackingBullet);
            }
        }
    }
}
