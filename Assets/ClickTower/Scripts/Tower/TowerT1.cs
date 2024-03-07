using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using xmaolol.com;
using QFramework;

public class TowerT1 : Tower
{
    private bool WantChangePos = false;

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

                    if (WantChangePos == false)
                    {
                        SetParentTrans(bullet.transform, bulletParent);
                    }
                    else
                    {
                        bullet.transform.SetParent(bulletParent);
                        bullet.transform.localPosition = Vector3.zero + new Vector3(0.13f, 0, 0);
                        bullet.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    }
                    WantChangePos = !WantChangePos;

                    willBullet.Add(bullet.GetComponent<Bullet>());
                }
            }
            else
            {
                this.Delay(ShootCD, PackingBullet);
            }
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

}
