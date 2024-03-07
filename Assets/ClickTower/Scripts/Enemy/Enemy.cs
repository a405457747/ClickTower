using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using xmaolol.com;
using UnityEngine.UI;
using QFramework;
using DG.Tweening;
using Lean.Pool;

public class Enemy : MonoBehaviour
{
    public GameObject BloodMapEffect;
    public GameObject HitEffect;

    //属于的波数
    internal bool IsDoubleCoin;
    public int waveCount;
    //这个是显示血效果了现在
    public GameObject CollectedPSHP;
    public GameObject CoinPrefab;
    public delegate void EnemyDie(Enemy enemy);
    public event EnemyDie DieHandler;

    EnemyDir enemyDir;
    //坦克出生方向
    Vector3 tankBrithPos;
    private MainManager mainManager;
    bool isReduceSpeedSpeed = false;
    float moveSpeed;
    int maxHp;
    [SerializeField]
    int hp;
    //掉落多少钱呢
    int loseMoney;
    //掉落率
    float dropRate;
    Slider hpSlider;
    List<Transform> PointsList;
    float reduceSpeedTimeOfDuration = 1.3f;
    #region 泛白效果
    private SpriteRenderer spriteRenderer;
    private float whiteCoolTime = 0.015f;
    float whiteCoolTimer;
    #endregion

    //路径点的下标
    protected int RoadIndex;
    //掉落物品的类型
    protected int dropProp;

    public int Hp
    {
        get => hp; set
        {

            hp = value;
            float hpFloat = (float)value / MaxHp;
            hpSlider.value = hpFloat;
            if (hp <= 0)
            {
                DestroySelf();
            }
            PauseFrameOne();
            //StartCoroutine("PauseFrameOne");
        }
    }
    public int LoseMoney { get => loseMoney; set => loseMoney = value; }
    public EnemyDir EnemyDir
    {
        get => enemyDir; set
        {
            enemyDir = value;
            switch (value)
            {
                case EnemyDir.Right:
                    transform.rotation = Quaternion.Euler(Vector3.zero);
                    transform.localScale = Vector3.one * ShapeFactor;
                    break;
                case EnemyDir.Left:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    transform.localScale = Vector3.one * ShapeFactor;
                    break;
                case EnemyDir.Up:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    transform.localScale = Vector3.one * ShapeFactor;
                    break;
                case EnemyDir.Down:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                    transform.localScale = Vector3.one * ShapeFactor;
                    break;
            }
        }
    }
    public float MoveSpeed
    {
        get
        {
            if (IsHitReduceSpeed)
            {
                return 0;
            }

            if (IsReduceSpeedSpeed)
            {
                return moveSpeed / 2f;
            }
            else
            {
                return moveSpeed;
            }
        }
        set => moveSpeed = value;
    }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public float DropRate { get => dropRate; set => dropRate = value; }
    public bool IsReduceSpeedSpeed
    {
        get => isReduceSpeedSpeed; set
        {
            isReduceSpeedSpeed = value;
            if (value == true)
            {
                this.Delay(ReduceSpeedTimeOfDuration, RecoverySpeed);
            }
        }
    }
    public float ReduceSpeedTimeOfDuration { get => reduceSpeedTimeOfDuration; set => reduceSpeedTimeOfDuration = value; }

    //增加的爆率
    internal float AddExplodeRate = 0f;
    internal bool enemyCandie;
    //是否是T1百分比状态
    internal bool IsPercentageDemageT1;

    //是否是击中状态速度为0
    bool isHitReduceSpeed = false;
    //是否是灼烧状态
    bool isCombustionState;
    float isCombusitonTimer = 0f;
    float shapeFactor;
    public bool IsCombustionState
    {
        get => isCombustionState; set
        {
            isCombustionState = value;
            if (value == true)
            {
                this.Delay(Consts.DurationOfBurningSaveTime, () => { IsCombustionState = false; });
            }
            else
            {
                isCombusitonTimer = 0f;
            }
        }
    }

    protected virtual void DieEffect()
    {
        if (this.gameObject.name.Length == 13)
        {
            MyAudioManager.GetInstance().PlaySound(Consts.humanDie);

        }
        else
        {
            MyAudioManager.GetInstance().PlaySound(Consts.robotDie);

        }
    }

    public bool IsHitReduceSpeed
    {
        get => isHitReduceSpeed; set
        {
            isHitReduceSpeed = value;
            if (value == true)
            {
                this.Delay(Random.value * 0.1f, () => { isHitReduceSpeed = false; });
            }
        }
    }

    public float ShapeFactor { get => shapeFactor; set => shapeFactor = value; }

    public int Demage;

    //暂停几毫秒哦帧率
    private void PauseFrameOne()
    {
        this.Delay(0.02f, () => { IsHitReduceSpeed = true; });
    }

    private void Awake()
    {
        hpSlider = transform.Find("Slider_Canvas/Slider").GetComponent<Slider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        PointsList = Points.Instance.PointsList;
        mainManager = MainManager.Instance;
        Init();
    }

    private void Update()
    {
        if (mainManager.GameState == GameState.GameStart)
        {
            Move();

            #region 泛白效果
            //被攻击状态冷却，设置被攻击参数为0
            if (whiteCoolTimer <= 0)
                spriteRenderer.material.SetInt("_BeAttack", 0);
            else
                whiteCoolTimer -= Time.deltaTime;
            #endregion

            if (IsCombustionState)
            {
                isCombusitonTimer += Time.deltaTime;
                if (isCombusitonTimer >= 1)
                {
                    BeInjered((int)(Hp * Consts.DurationOfBurningDemagePercentage), false);
                    isCombusitonTimer = 0f;
                }
            }
        }

    }

    private void BeAttack()
    {
        spriteRenderer.material.SetInt("_BeAttack", 1);
        whiteCoolTimer = whiteCoolTime;
    }

    private void RecoverySpeed()
    {
        IsReduceSpeedSpeed = false;
    }

    public void InitData(int hp, int loseMoney, float moveSpeed, float dropRate, int waveCount, int dropProp, int Demage, float shapeFactor)
    {
        this.hp = hp;
        this.maxHp = hp;
        this.loseMoney = loseMoney;
        this.moveSpeed = moveSpeed;
        this.dropRate = dropRate;
        this.waveCount = waveCount;
        this.dropProp = dropProp;
        this.Demage = Demage;
        this.shapeFactor = shapeFactor;
    }

    //初始化的方法
    public virtual void Init()
    {
        tankBrithPos = mainManager.enemyBrithPoint;
        EnemyDir = GetDir(tankBrithPos, PointsList[RoadIndex].position);
        RoadIndex = 0;
    }

    protected EnemyDir GetDir(Vector3 point1, Vector3 point2)
    {
        Vector3 v = (point2 - point1).normalized;
        if (v == new Vector3(1, 0, 0))
        {
            return EnemyDir.Right;
        }
        else if (v == new Vector3(-1, 0, 0))
        {
            return EnemyDir.Left;
        }
        else if (v == new Vector3(0, 1, 0))
        {
            return EnemyDir.Up;
        }
        else
        {
            return EnemyDir.Down;
        }
    }

    protected void Move()
    {
        if (RoadIndex < PointsList.Count)
        {
            transform.Translate((PointsList[RoadIndex].position - transform.position).normalized * Time.deltaTime * MoveSpeed, Space.World);
            if (Vector3.Distance(PointsList[RoadIndex].position, transform.position) < 0.1f)
            {
                RoadIndex++;
                if (RoadIndex < PointsList.Count)
                {
                    EnemyDir = GetDir(PointsList[RoadIndex - 1].position, PointsList[RoadIndex].position);
                }
                if (RoadIndex == PointsList.Count)
                {
                    TakeDemageToPlayer();
                }
            }
        }
    }

    protected void TakeDemageToPlayer()
    {
        UIMain.Instance.HeartBeInjured(1);
        Destroy(this.gameObject, 0.5f);
        RecyclingData();
    }

    protected void DestroySelf()
    {
        DieEffect();
        Destroy(this.gameObject);
        Coin coint = LeanPool.Spawn(CoinPrefab, transform.position, Quaternion.identity).GetComponent<Coin>();
        LeanPool.Spawn(BloodMapEffect, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));

        if (IsDoubleCoin)
        {
            coint.Init(LoseMoney * 2);
        }
        else
        {
            coint.Init(LoseMoney);
        }
        //除了生存金币外，还要生存道具
        if (MyTool.CanBirth(this.DropRate + AddExplodeRate))
        {
            string goodsStr;
            int RandomValue = 0;
            switch (this.dropProp)
            {
                case 0:
                    RandomValue = Random.Range(2, 3);
                    break;
                case 1:
                    RandomValue = Random.Range(2, 3);
                    break;
                case 2:
                    RandomValue = Random.Range(1, 2);
                    break;
                case 3:
                    RandomValue = Random.Range(1, 2);
                    break;
                case 4:
                    RandomValue = Random.Range(1, 3);
                    break;
                case 5:
                    RandomValue = Random.Range(1, 2);
                    break;
                case 6:
                    RandomValue = Random.Range(1, 2);
                    break;
            }
            goodsStr = $"0g{RandomValue}{this.dropProp}";
            MySaveManager.Instance.SaveMapping.GoodsList.Add(goodsStr);
            ItemGoodsMessageTips.Instance.SetContentStrValue("有物品掉落胜利后请查收");
        }
        RecyclingData();
    }

    public void BeInjered(int demage, bool isCrit)
    {
        if (enemyCandie)
        {
            demage = Consts.maxDemage;
        }
        if (IsPercentageDemageT1)
        {
            demage = (int)(Consts.percentageDemageT1 * MaxHp);
        }

        if (MainManager.Instance.GameState ==GameState.GameStart)
        {
            Hp -= demage;
        }

        BeAttack();
        //实例化hp效果
        GameObject hp_Num_CanvasObj = LeanPool.Spawn(CollectedPSHP, transform.position + new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(0f, 0.4f), 0), Quaternion.identity);
        Hp_Num_Canvas hp_Num_Canvas = hp_Num_CanvasObj.GetComponent<Hp_Num_Canvas>();
        hp_Num_Canvas.InitData(demage, isCrit);
    }

    public void SetLayerLevel(int layerValue)
    {
        spriteRenderer.sortingOrder = layerValue;
    }


    //private int myWave;
    public void RecyclingData()
    {
        EnemySpawn enemySpawn = EnemySpawn.Instance;
        enemySpawn.waveEnemyList.Remove(this);

        enemySpawn.CurrentEnemyDieCount++;
        if (enemySpawn.CurrentEnemyDieCount == enemySpawn.levelList.Count)
        {
            if (mainManager.GameState != GameState.GameOver)
                mainManager.GameState = GameState.Win;
        }

        DieHandler?.Invoke(this);

    }
}
