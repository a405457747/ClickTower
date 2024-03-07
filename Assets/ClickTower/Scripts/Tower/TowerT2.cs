using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using xmaolol.com;
using QFramework;

public class TowerT2 : Tower
{
    int randomWantValue = 0;

    public int RandomWantValue
    {
        get => randomWantValue; set
        {
            if (value > 3)
            {
                value = 0;
            }
            randomWantValue = value;
        }
    }

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

                    if (RandomWantValue == 0)
                    {
                        SetParentTrans(bullet.transform, bulletParent);
                    }
                    else if (RandomWantValue == 1)
                    {
                        bullet.transform.SetParent(bulletParent);
                        bullet.transform.localPosition = Vector3.zero + new Vector3(0.05f, 0, 0);
                        bullet.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    }
                    else if (RandomWantValue == 2)
                    {
                        bullet.transform.SetParent(bulletParent);
                        bullet.transform.localPosition = Vector3.zero + new Vector3(0.1f, 0, 0);
                        bullet.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    }
                    else if (randomWantValue == 3)
                    {
                        bullet.transform.SetParent(bulletParent);
                        bullet.transform.localPosition = Vector3.zero + new Vector3(0.15f, 0, 0);
                        bullet.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    }
                    RandomWantValue++;
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
