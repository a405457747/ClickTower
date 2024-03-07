using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using xmaolol.com;
using QFramework;
using Lean.Pool;
using UnityEngine.SceneManagement;
public class Bullet : MonoBehaviour
{
    public bool isScopeBullet;
    public float boomRange = 0.5f;

    private GameObject hitEffect;
    private Enemy centerEnemy;
    float delayDetect = 0.15f;
    //非中心伤害比
    float NoncentralInjuryRatio = 0.5f;
    float flySpeed;
    bool isRun;
    float towerDemageFixed;
    int demage;
    float critRate;
    bool isCrit;
    bool isReduceBullet;
    bool canShowCircle;
    bool IsHpToMax;
    //是否双倍爆炸范围
    bool isDoubleBoomRange;
    //增加的爆率
    float AddExplodeRate;
    bool needAddExplodeRate;
    bool isDoubleCoin;
    bool IsPercentageDemageT1;
    bool IsCombustionState;
    //敌人可以直接死亡吗
    bool enemyCandie;
    protected BoxCollider2D boxCollider2D;
    protected CircleCollider2D circleCollider2D;
    int sceneLevel;

    public bool NeedAddExplodeRate
    {
        get => needAddExplodeRate; set
        {
            needAddExplodeRate = value;
            if (value == true)
            {
                AddExplodeRate = Consts.AddExplodeRate;
            }
            else
            {
                AddExplodeRate = 0f;
            }
        }
    }

    public bool CanShowCircle
    {
        get => canShowCircle; set
        {
            canShowCircle = value;
            if (value == false)
            {
                circleCollider2D.enabled = false;
                boxCollider2D.enabled = true;
                SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer item in spriteRenderers)
                {
                    item.enabled = true;
                }
            }
            else
            {
                circleCollider2D.enabled = true;
                boxCollider2D.enabled = false;
                SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer item in spriteRenderers)
                {
                    item.enabled = false;
                }
            }
        }
    }

    public virtual bool IsRun
    {
        get => isRun; set
        {
            isRun = value;
            if (value == true)
            {
                transform.parent = null;
                boxCollider2D.enabled = true;
            }
            else
            {
                boxCollider2D.enabled = false;
            }
        }
    }

    public bool IsDoubleBoomRange
    {
        get => isDoubleBoomRange; set
        {
            isDoubleBoomRange = value;
            if (value == true)
            {
                circleCollider2D.radius = boomRange * 2f;
            }
            else
            {
                circleCollider2D.radius = boomRange;
            }
        }
    }

    public virtual void NeedData(float flySpeed, int demage, float critRate, float towerDemageFixed, float reduceEnemySpeedRate, float bulletSpeed, bool isDoubleRange, bool needAddExplodeRate, bool isDoubleCoin, bool enemyCandie, bool IsPercentageDemageT1, bool IsHpToMax, bool IsCombustionState)
    {
        this.flySpeed = flySpeed;
        this.critRate = critRate;
        this.flySpeed = bulletSpeed;
        this.towerDemageFixed = towerDemageFixed;
        this.isDoubleCoin = isDoubleCoin;
        this.IsHpToMax = IsHpToMax;
        this.IsPercentageDemageT1 = IsPercentageDemageT1;
        this.IsCombustionState = IsCombustionState;
        IsDoubleBoomRange = isDoubleRange;
        NeedAddExplodeRate = needAddExplodeRate;
        this.enemyCandie = enemyCandie;

        if (MyTool.CanBirth(reduceEnemySpeedRate))
        {
            isReduceBullet = true;
        }
        else
        {
            isReduceBullet = false;
        }
        if (MyTool.CanBirth(this.critRate))
        {
            isCrit = true;
        }
        else
        {
            isCrit = false;
        }
        int fixedDamage = 0;
        DemageManager.Instance.GetFixedDamage(demage, isCrit, out fixedDamage, towerDemageFixed);
        this.demage = fixedDamage;
    }

    protected void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        CanShowCircle = false;
        if (hitEffect == null)
        {
            hitEffect = Resources.Load<GameObject>("HitEffect") as GameObject;
        }
        sceneLevel = MyTool.GetNumberByString(SceneManager.GetActiveScene().name);
    }

    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        IsRun = false;
    }

    protected virtual void Update()
    {
        if (IsRun)
        {
            transform.Translate(Vector3.up * Time.deltaTime * flySpeed, Space.Self);
        }
    }

    //控制是否发射
    public virtual void OpenSwitch()
    {
        IsRun = true;
    }

    private void HurtEffect(Vector2 position, int demage)
    {
        var hitEffectObj = LeanPool.Spawn(hitEffect, position, Quaternion.identity);
        float demageFixed = 0f;
        if (demage >= 0 && demage <= 100)
        {
            demageFixed = 0.05f;
        }
        else

                 if (demage > 100 && demage <= 1000)
        {
            demageFixed = 0.1f;

        }
        else

                 if (demage > 1000 && demage <= 10000)
        {
            demageFixed = 0.15f;

        }
        else
                 if (demage > 10000)
        {
            demageFixed = 0.3f;

        }
        HitEffect hitEffectCS = hitEffectObj.GetComponent<HitEffect>();
        hitEffectCS.SetSize(sceneLevel * 0.0175f + demageFixed);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            DestroySelf();
        }

        if (CanShowCircle == false)
        {
            if (collision.CompareTag("Enemy"))
            {
                //让坦克销毁吧
                Enemy enemyCS = collision.gameObject.GetComponent<Enemy>();
                centerEnemy = enemyCS;
                enemyCS.enemyCandie = enemyCandie;
                enemyCS.IsPercentageDemageT1 = IsPercentageDemageT1;
                enemyCS.BeInjered(demage, isCrit);
                HitEnemyEffect();
                HurtEffect(enemyCS.transform.position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0), demage);
                enemyCS.IsCombustionState = IsCombustionState;
                if (isReduceBullet)
                {
                    enemyCS.IsReduceSpeedSpeed = true;
                }
                enemyCS.AddExplodeRate = AddExplodeRate;
                enemyCS.IsDoubleCoin = isDoubleCoin;
                //销毁自己
                DestroySelf();
            }
        }
        else
        {
            if (isScopeBullet == false)
            {
                //不用写
            }
            else
            {
                if (collision.CompareTag("Enemy"))
                {
                    //让坦克销毁吧
                    Enemy enemyCS = collision.gameObject.GetComponent<Enemy>();
                    enemyCS.enemyCandie = enemyCandie;
                    enemyCS.IsPercentageDemageT1 = IsPercentageDemageT1;
                    float demageRatio = Mathf.Abs((circleCollider2D.radius - Vector2.Distance(enemyCS.transform.position, transform.position)));
                    if (enemyCS != centerEnemy)
                        enemyCS.BeInjered((int)(demage * demageRatio) + (int)(demage * 0.5f), isCrit);
                    HurtEffect(enemyCS.transform.position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0), demage);
                    enemyCS.IsCombustionState = IsCombustionState;
                    if (isReduceBullet)
                    {
                        enemyCS.IsReduceSpeedSpeed = true;
                    }
                    enemyCS.AddExplodeRate = AddExplodeRate;
                    enemyCS.IsDoubleCoin = isDoubleCoin;
                }
            }
        }
    }

    private void HitEnemyEffect()
    {
        if (this.gameObject.name.Length == 15)
        {
            MyAudioManager.GetInstance().PlaySound(Consts.bulletHit);
        }
        else
        {
            MyAudioManager.GetInstance().PlaySound(Consts.BoomHit);
        }
    }

    public virtual void DestroySelf()
    {
        CanShowCircle = true;
        this.Delay(delayDetect, DestroyData);
    }

    private void DestroyData()
    {
        centerEnemy = null;
        Destroy(this.gameObject);
    }

}
