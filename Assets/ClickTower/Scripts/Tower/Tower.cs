using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework.Example;
using QFramework;
using xmaolol.com;
using UnityEngine.UI;
using System;
using Lean.Pool;
//using QFramework.UIExample;

public class Tower : MonoBehaviour
{
    #region 属性
    protected CircleCollider2D circleCollider2D;
    //准备发射的子弹
    [HideInInspector]
    protected List<Bullet> willBullet = new List<Bullet>();
    //是否装填好子弹了
    [HideInInspector]
    protected bool isPacking = false;

    private float shootHowmanyTime = 1f;
    int maxHp;
    //[HideInInspector]
    int hp;
    private TowerMonitor towerMonitor;
    //装填引用计数和IsPacking属性有很大联
    [HideInInspector]
    private int packingNum;
    int demage;
    int sellPrice;
    //子弹的速度
    [HideInInspector]
    public float bulletSpeed;
    //炮塔的等级
    int currentLevel = 1;

    public int price;
    public bool IsBuff;
    public int TowerIndex;
    [HideInInspector]
    public Gun gun;
    [HideInInspector]
    public List<Enemy> EnemyRange;
    //这个名字不改了，要注意不一定是boy;
    public GameObject BulletRockBoy;
    public Transform bulletParent;
    public Transform GunTrans;
    //暴击几率
    [HideInInspector]
    public float critRate;
    //塔的侦察范围
    [HideInInspector]
    public float towerMonitorDetectRange;
    [HideInInspector]
    public float shootCD;
    //塔的伤害修正
    [HideInInspector]
    public float towerDemageFixed;
    public int MaxPackingNum = 1;
    //减速率
    [HideInInspector]
    public float reduceEnemySpeedRate;

    //当前等级
    public int CurrentLevel
    {
        get
        {
            if (IsBuff)
            {
                return currentLevel;
            }
            else
            {
                Level_Add_Panel.Instance.GetTowerIndexMessage(TowerIndex, out float tempRate, out float tempTowerDemageFixed, out float shootCd, out float reduceSpeedRate, out float detectRange, out float bulletSpeed, out int level, out int price22);
                return level;
            }
        }
        set => currentLevel = value;
    }
    public int Price
    {
        get
        {
            if (IsBuff)
            {
                int randomValue = 0;
                if (MySaveManager.Instance.HavePropDemageFixed(out randomValue, '4'))
                {

                    float discount = randomValue / 10f;
                    int discountPrice = (int)(price * discount);
                    return (int)((price - discountPrice));
                }
                else
                {
                    return price;
                }
            }
            else
            {
                Level_Add_Panel.Instance.GetTowerIndexMessage(TowerIndex, out float tempRate, out float tempTowerDemageFixed, out float shootCd, out float reduceSpeedRate, out float detectRange, out float bulletSpeed, out int level, out int price22);
                return price22;
            }
        }
        set => price = value;
    }
    public float CritRate
    {
        get
        {
            if (IsBuff)
            {
                if (MySaveManager.Instance.HavePropDemageFixed(out int randomValue, '5'))
                {
                    float rate = randomValue / 10f;
                    return rate + critRate;
                }
                else
                {
                    return critRate;
                }
            }
            else
            {
                Level_Add_Panel.Instance.GetTowerIndexMessage(TowerIndex, out float tempRate, out float tempTowerDemageFixed, out float shootCd, out float reduceSpeedRate, out float detectRange, out float bulletSpeed, out int level, out int price22);
                return tempRate;
            }
        }
        set => critRate = value;
    }
    public float ShootCD
    {
        get
        {
            if (IsBuff)
            {
                if (MySaveManager.Instance.HavePropDemageFixed(out int randomValue, '6'))
                {
                    return shootCD - (randomValue / 10f);
                }
                else
                {
                    return shootCD;
                }
            }
            else
            {
                Level_Add_Panel.Instance.GetTowerIndexMessage(TowerIndex, out float tempRate, out float tempTowerDemageFixed, out float shootCd, out float reduceSpeedRate, out float detectRange, out float bulletSpeed, out int level, out int price22);
                return shootCd;
            }
        }
        set => shootCD = value;
    }
    [HideInInspector]
    public int PackingNum
    {
        get => packingNum; set
        {
            packingNum = value;
            if (value == 0)
            {
                IsPacking = false;
            }
            else if (value == MaxPackingNum)
            {
                IsPacking = true;
            }
        }
    }
    [HideInInspector]
    public virtual bool IsPacking
    {
        get => isPacking; set
        {
            isPacking = value;
            if (IsPacking == true)
            {
                GameObject bullet = GameObject.Instantiate(BulletRockBoy, transform.position, Quaternion.identity);
                SetParentTrans(bullet.transform, bulletParent);
                willBullet.Add(bullet.GetComponent<Bullet>());
            }
            else
            {
                this.Delay(ShootCD, PackingBullet);
            }
        }
    }
    public float BulletSpeed
    {
        get
        {
            if (IsBuff)
            {
                return bulletSpeed;
            }
            else
            {
                Level_Add_Panel.Instance.GetTowerIndexMessage(TowerIndex, out float tempRate, out float tempTowerDemageFixed, out float shootCd, out float reduceSpeedRate, out float detectRange, out float bulletSpeed, out int level, out int price22);
                return bulletSpeed;
            }

        }
        set => bulletSpeed = value;
    }
    public int SellPrice { get => Price / 2; set => sellPrice = value; }
    public int Demage { get => demage; set => demage = value; }

    public float TowerDemageFixed
    {
        get
        {
            if (IsBuff)
            {
                if (MySaveManager.Instance.HavePropDemageFixed(out int randomValue, '0'))
                {
                    float rate = randomValue / 1f;
                    return rate + towerDemageFixed;
                }
                else
                {
                    return towerDemageFixed;
                }
            }
            else
            {
                Level_Add_Panel.Instance.GetTowerIndexMessage(TowerIndex, out float tempRate, out float tempTowerDemageFixed, out float shootCd, out float reduceSpeedRate, out float detectRange, out float bulletSpeed, out int level, out int price22);
                return tempTowerDemageFixed;
            }
        }
        set => towerDemageFixed = value;
    }
    public float TowerMonitorDetectRange
    {
        get
        {
            if (IsBuff)
            {
                return towerMonitorDetectRange;
            }
            else
            {
                Level_Add_Panel.Instance.GetTowerIndexMessage(TowerIndex, out float tempRate, out float tempTowerDemageFixed, out float shootCd, out float reduceSpeedRate, out float detectRange, out float bulletSpeed, out int level, out int price22);
                return detectRange;
            }
        }
        set => towerMonitorDetectRange = value;
    }
    public float ReduceEnemySpeedRate
    {
        get
        {
            if (IsBuff)
            {
                return reduceEnemySpeedRate;
            }
            else
            {
                Level_Add_Panel.Instance.GetTowerIndexMessage(TowerIndex, out float tempRate, out float tempTowerDemageFixed, out float shootCd, out float reduceSpeedRate, out float detectRange, out float bulletSpeed, out int level, out int price22);
                return reduceSpeedRate;
            }
        }
        set => reduceEnemySpeedRate = value;
    }
    //当前血量
    public int Hp
    {
        get => hp; set
        {
            hp = value;
            if (value <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
    #endregion
    private GameObject DustEffect;

    private Vector3 JutiTargetpos;
    #region Mono函数
    protected virtual void Awake()
    {
        gun = transform.Find("Gun").gameObject.AddComponent<Gun>();
        towerMonitor = transform.Find("TowerMonitor").GetComponent<TowerMonitor>();
    }

    ////开始设置不然可能获取不到
    protected virtual void Start()
    {
        Hp = 100;
        EnemyRange = towerMonitor.EnemyRange;
        PackingNum = MaxPackingNum;
        maxHp = Hp;
        HpToMax();
        JutiTargetpos = MainManager.Instance.enemyBrithPoint - Vector3.up;

        critRate = MyConfigManager.Instance.GetTowerConfigByIndex(TowerIndex).critRate;
        demage = MyConfigManager.Instance.GetTowerConfigByIndex(TowerIndex).demage;
        bulletSpeed = MyConfigManager.Instance.GetTowerConfigByIndex(TowerIndex).bulletSpeed;
        towerMonitorDetectRange = MyConfigManager.Instance.GetTowerConfigByIndex(TowerIndex).towerMonitorDetectRange;
        shootCD = MyConfigManager.Instance.GetTowerConfigByIndex(TowerIndex).shootCD;
        towerDemageFixed = MyConfigManager.Instance.GetTowerConfigByIndex(TowerIndex).towerDemageFixed;
        reduceEnemySpeedRate = MyConfigManager.Instance.GetTowerConfigByIndex(TowerIndex).reduceEnemySpeedRate;
    }

    //血量回满
    internal void HpToMax()
    {
        Hp = maxHp;
    }

    internal void BeInjured(int demage)
    {
        Hp -= demage;
    }

    public virtual void InputKeyDown()
    {
        if (MainManager.Instance.CanDemolition == true)
        {
            DelTower();
        }
        else if (isPacking)
        {
            ShootFire();
        }
    }

    protected void SetParentTrans(Transform child, Transform parent)
    {
        child.transform.SetParent(parent);
        child.transform.localPosition = Vector3.zero;
        child.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    protected virtual void Update()
    {
        LookAtEnemy();
    }

    protected virtual void LookAtEnemy()
    {
        if (EnemyRange.Count > 0 && EnemyRange[0] != null)
        {
            Transform enemyTrans = EnemyRange[0].transform;
            gun.LookAtEnemy(enemyTrans.position);
        }
    }

    #endregion
    protected virtual void ShootFire()
    {
        if (willBullet.Count > 0)
        {
            Bullet bullet = willBullet[willBullet.Count - 1];
            bool isToMax = TowerIndex == 3 && CurrentLevel == Consts.TowerMaxLevel;
            if (isToMax)
            {
                HpToMax();
            }
            //  print("cirtrate:" + CritRate + "towerDemageFixed:" + TowerDemageFixed + "shootcd:" + ShootCD + "reduceSpeedRate:" + ReduceEnemySpeedRate + "detectRange:" + TowerMonitorDetectRange  +"bullSpped:" +BulletSpeed);
            bullet.NeedData(BulletSpeed, Demage, CritRate, TowerDemageFixed, ReduceEnemySpeedRate, BulletSpeed, TowerIndex == 2 && CurrentLevel == Consts.TowerMaxLevel,
                TowerIndex == 4 && CurrentLevel == Consts.TowerMaxLevel,
                TowerIndex == 6 && CurrentLevel == Consts.TowerMaxLevel,
                TowerIndex == 5 && CurrentLevel == Consts.TowerMaxLevel && MyTool.CanBirth(Consts.fatalityRate),
                                TowerIndex == 0 && CurrentLevel == Consts.TowerMaxLevel,
                              false,
                              TowerIndex == 1 && CurrentLevel == Consts.TowerMaxLevel && MyTool.CanBirth(Consts.DurationOfBurningBrithRate)
                );
            bullet.OpenSwitch();
            willBullet.Remove(bullet);
            PackingNum -= 1;
        }
    }

    private IEnumerator ShootIenumerator()
    {
        for (int i = 0; i < MaxPackingNum; i++)
        {
            yield return new WaitForSeconds(shootHowmanyTime / MaxPackingNum);
            ShootFire();
        }
    }

    //用完了开始装填
    protected virtual void PackingBullet()
    {
        PackingNum = MaxPackingNum;
    }

    #region 公共函数
    //删除塔的方法
    public virtual void DelTower()
    {
        DestroySelf();
        UIMain.Instance.InitialMoney += SellPrice;
        MyAudioManager.GetInstance().PlaySound(Consts.SellTower);
        GlassLand glassLand = transform.parent.GetComponent<GlassLand>();
        glassLand.LetTowerNull();
    }

    public virtual void DestroySelf()
    {
        Destroy(this.gameObject);
    }
    #endregion
}
