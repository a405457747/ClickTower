using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using xmaolol.com;
using QFramework;

public class TowerF2 : Tower
{
    public Transform BulletParentLeft;
    public Transform BulletParentRight;

    public override bool IsPacking
    {
        get => isPacking; set
        {
            isPacking = value;
            if (IsPacking == true)
            {
                GameObject bullet = GameObject.Instantiate(BulletRockBoy, transform.position, Quaternion.identity);
                GameObject bullet2 = GameObject.Instantiate(BulletRockBoy, transform.position, Quaternion.identity);
                SetParentTrans(bullet.transform, BulletParentLeft);
                SetParentTrans(bullet2.transform, BulletParentRight);
                willBullet.Add(bullet.GetComponent<Bullet>());
                willBullet.Add(bullet2.GetComponent<Bullet>());
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
            ShootFire();
            this.Delay(0.1f, () => { ShootFire(); });
        }
    }
}
