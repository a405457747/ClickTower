using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System.Xml;
using xmaolol.com;
using System.Text;
using System.IO;
using System.Runtime;
using UnityEngine.SceneManagement;

public struct table
{
    public int wave;
    public string enemyType;
    public int hp;
    public float wait;
    public int loseMoney;
    public float moveSpeed;
    public float dropRate;
    public int dropProp;
    public int Demage;
    public float shapeFactor;
}

public struct TowerConfigTable
{
    public int demage;
    public float bulletSpeed;
    public float critRate;
    public float towerMonitorDetectRange;
    public float shootCD;
    public float towerDemageFixed;
    public float reduceEnemySpeedRate;
}

public struct TowerConfigParent
{
    public List<TowerConfigTable> TowerConfigTables;
    public float perLevelAddCrit;
    public float perLevelAttackRate;
    public float perLevelReduceCD;
    public float perLevelReduceSpeedRate;
    public float perLevelAddMonitoringScope;
    public float perLevelAddBulletSpeed;
}

public struct level
{
    public List<table> levelList;
    public int initialMoney;
    public int maxTowerIndex;
}

public class MyConfigManager : MonoSingleton<MyConfigManager>
{
    private string path = @"EnemyWaveConfig";

    public level myLevelData;
    public int LevelIndex;
    public TowerConfigParent towerConfigParent = new TowerConfigParent();
    //满级了才增加HpLevelRate倍率
    private float HpLevelRate = 32.75f;
    private float AttackLevelRate = 1.25f;
    private void Awake()
    {
        towerConfigParent.TowerConfigTables = new List<TowerConfigTable>();
        LevelIndex = MyTool.GetNumberByString(SceneManager.GetActiveScene().name);
        LoadByXML();
    }

    public TowerConfigTable GetTowerConfigByIndex(int index)
    {
        TowerConfigTable towerConfigTable = new TowerConfigTable();
        towerConfigTable.demage = towerConfigParent.TowerConfigTables[index].demage;
        towerConfigTable.bulletSpeed = towerConfigParent.TowerConfigTables[index].bulletSpeed;
        towerConfigTable.critRate = towerConfigParent.TowerConfigTables[index].critRate;
        towerConfigTable.towerMonitorDetectRange = towerConfigParent.TowerConfigTables[index].towerMonitorDetectRange;
        towerConfigTable.shootCD = towerConfigParent.TowerConfigTables[index].shootCD;
        towerConfigTable.towerDemageFixed = towerConfigParent.TowerConfigTables[index].towerDemageFixed;
        towerConfigTable.reduceEnemySpeedRate = towerConfigParent.TowerConfigTables[index].reduceEnemySpeedRate;
        return towerConfigTable;
    }

    private void LoadByXML()
    {
        XmlDocument xmlDocument = new XmlDocument();
        TextAsset tw = Resources.Load(path) as TextAsset;
        xmlDocument.LoadXml(tw.text);
        XmlNodeList contentXmls = xmlDocument.GetElementsByTagName("level");
        XmlNode contentXml = contentXmls[LevelIndex];
        myLevelData.levelList = new List<table>();
        myLevelData.maxTowerIndex = int.Parse(contentXml.Attributes["maxTowerIndex"].Value);
        myLevelData.initialMoney = int.Parse(contentXml.Attributes["initialMoney"].Value);
        foreach (XmlNode item in contentXml)
        {
            table tableTemp = new table()
            {
            };
            tableTemp.wave = int.Parse(item.Attributes["wave"].Value);
            tableTemp.enemyType = item.Attributes["enemyType"].Value;
            int temHp = int.Parse(item.Attributes["hp"].Value);
            tableTemp.hp = temHp + (int)((temHp * LevelIndex / Consts.MaxGameLevel) * HpLevelRate) +100000;
            tableTemp.wait = float.Parse(item.Attributes["wait"].Value) + Random.Range(0, 0.67f);
            tableTemp.loseMoney = int.Parse(item.Attributes["loseMoney"].Value);
            tableTemp.moveSpeed = float.Parse(item.Attributes["moveSpeed"].Value);
            tableTemp.dropRate = float.Parse(item.Attributes["dropRate"].Value);
            tableTemp.dropProp = int.Parse(item.Attributes["dropProp"].Value);
            tableTemp.Demage = int.Parse(item.Attributes["demage"].Value);
            tableTemp.shapeFactor = float.Parse(item.Attributes["shapeFactor"].Value);
            myLevelData.levelList.Add(tableTemp);
        }

        /*验证数据啊
         * table tableTest = myLevelData.levelList[0];
           print($"{tableTest.wave} {tableTest.enemyType} {tableTest.hp} {tableTest.wait} {tableTest.loseMoney} {tableTest.moveSpeed} {tableTest.dropRate}");
         */

        XmlNodeList towers = xmlDocument.GetElementsByTagName("TowerConfig");
        XmlNode towersParent = towers[0];

        towerConfigParent.perLevelAddCrit = float.Parse(towersParent.Attributes["perLevelAddCrit"].Value);
        towerConfigParent.perLevelAttackRate = float.Parse(towersParent.Attributes["perLevelAttackRate"].Value);
        towerConfigParent.perLevelReduceCD = float.Parse(towersParent.Attributes["perLevelReduceCD"].Value);
        towerConfigParent.perLevelReduceSpeedRate = float.Parse(towersParent.Attributes["perLevelReduceSpeedRate"].Value);
        towerConfigParent.perLevelAddMonitoringScope = float.Parse(towersParent.Attributes["perLevelAddMonitoringScope"].Value);
        towerConfigParent.perLevelAddBulletSpeed = float.Parse(towersParent.Attributes["perLevelAddBulletSpeed"].Value);

        foreach (XmlNode item in towersParent)
        {
            TowerConfigTable tableTemp = new TowerConfigTable()
            {
            };
            int tempDemage = int.Parse(item.Attributes["demage"].Value);
            tableTemp.demage = tempDemage + (int)((tempDemage * LevelIndex / Consts.MaxGameLevel) * AttackLevelRate);
            tableTemp.bulletSpeed = float.Parse(item.Attributes["bulletSpeed"].Value);
            tableTemp.critRate = float.Parse(item.Attributes["critRate"].Value);
            tableTemp.towerMonitorDetectRange = float.Parse(item.Attributes["towerMonitorDetectRange"].Value);
            tableTemp.shootCD = float.Parse(item.Attributes["shootCD"].Value);
            tableTemp.towerDemageFixed = float.Parse(item.Attributes["towerDemageFixed"].Value);
            tableTemp.reduceEnemySpeedRate = float.Parse(item.Attributes["reduceEnemySpeedRate"].Value);
            towerConfigParent.TowerConfigTables.Add(tableTemp);
        }

    }
}
