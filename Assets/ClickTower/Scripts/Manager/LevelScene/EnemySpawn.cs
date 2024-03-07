using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System.Linq;
using xmaolol.com;
using Lean.Pool;


public class EnemySpawn : MonoSingleton<EnemySpawn>
{
    public GameObject BrithEffect;
    //坦克1其实就是敌人E
    public GameObject EnemyTank1;
    public GameObject EnemyF;
    public GameObject EnemyG;
    public GameObject EnemyH;
    public GameObject EnemyD;
    public GameObject EnemyC;
    public GameObject EnemyB;
    public GameObject EnemyA;
    public Vector3 birthPoint;
    public List<Enemy> waveEnemyList = new List<Enemy>();
    public int maxLevelCount;
    public List<table> levelList;

    [SerializeField]
    private int currentEnemyIndex = 0;
    private float timer = 0;
    private int currentEnemyDieCount = 0;
    //当前是第几波
    private int currentWave = 1;
    private int layerLevel = 0;

    //  public bool IsShowAddLevelPanel = false;
    public int CurrentWave
    {
        get => currentWave; set
        {
            if (currentWave != value)
            {
                IsShowAdd_SkillPanl();
            }
            //else
            //{
            //    IsShowAddLevelPanel = false;
            //}

            currentWave = value;
            UIMain.Instance.UpdateWaveCountText(value, maxLevelCount);
        }
    }
    public int CurrentEnemyDieCount { get => currentEnemyDieCount; set => currentEnemyDieCount = value; }

    void Start()
    {
        birthPoint = MainManager.Instance.enemyBrithPoint;
        levelList = MyConfigManager.Instance.myLevelData.levelList;
        maxLevelCount = levelList[levelList.Count - 1].wave;
        CurrentWave = 1;
    }

    private void Update()
    {
        if (MainManager.Instance.GameState == GameState.GameStart)
        {
            if (currentEnemyIndex > (levelList.Count - 1))
            {
                return;
            }

            if (!CanAddCurrentEnemyIndex())
            {
                return;
            }

            timer += Time.deltaTime;

            if (timer >= levelList[currentEnemyIndex].wait)
            {
                table item = levelList[currentEnemyIndex];
                GameObject enemy = CreateEnemy(item.enemyType);
                LeanPool.Spawn(BrithEffect, birthPoint, Quaternion.identity);
                Enemy enemyCS = enemy.GetComponent<Enemy>();
                enemyCS.SetLayerLevel(layerLevel++);
                enemyCS.InitData(item.hp, item.loseMoney, item.moveSpeed, item.dropRate, item.wave, item.dropProp, item.Demage, item.shapeFactor);
                int waveNum = item.wave;
                CurrentWave = waveNum;
                waveEnemyList.Add(enemyCS);
                AddCurrentEnemyIndex();
                timer = 0;
            }
        }
    }

    public void AddCurrentEnemyIndex()
    {
        currentEnemyIndex++;
    }

    public void IsShowAdd_SkillPanl()
    {
        if (waveEnemyList.Count == 0 && maxLevelCount != CurrentWave)
        {
            UIMain.Instance.OpenSkillPointPanel();
        }
    }

    //可以增加他的Index吗
    public bool CanAddCurrentEnemyIndex()
    {
        if (waveEnemyList.Count == 0) return true;
        return waveEnemyList.All(item => item.waveCount == levelList[currentEnemyIndex].wave);
    }

    private GameObject CreateEnemy(string type)
    {
        switch (type)
        {
            case "A":
                return GameObject.Instantiate(EnemyA, birthPoint, Quaternion.identity);
            case "B":
                return GameObject.Instantiate(EnemyB, birthPoint, Quaternion.identity);
            case "C":
                return GameObject.Instantiate(EnemyC, birthPoint, Quaternion.identity);
            case "D":
                return GameObject.Instantiate(EnemyD, birthPoint, Quaternion.identity);
            case "E":
                return GameObject.Instantiate(EnemyTank1, birthPoint, Quaternion.identity);
            case "F":
                return GameObject.Instantiate(EnemyF, birthPoint, Quaternion.identity);
            case "G":
                return GameObject.Instantiate(EnemyG, birthPoint, Quaternion.identity);
            case "H":
                return GameObject.Instantiate(EnemyH, birthPoint, Quaternion.identity);
        }
        return null;
    }

}
