using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using xmaolol.com;
using QFramework;
using System;

public class Level_Add_Panel : MonoSingleton<Level_Add_Panel>
{
    public Button[] prevAndAfterBtn;
    public Button[] btnAdds;
    public Button[] btnReduces;

    public Text NeedMoneyText1;
    public Text CurrentTowerName;
    public Text TextMaxLevelMessage;
    public RectTransform rectTransform;
    public Text levelText;
    public Text FunctionText1;
    public Text FunctionText2;
    public Text FunctionText3;
    public Text FunctionText4;
    public Text FunctionText5;
    public Text FunctionText6;
    public Sprite[] sprites;
    public Image towerImage;
    public GameObject[] towerList;
    public Text TextSkillPoint;
    public List<Tower> towerCSList = new List<Tower>();
    public bool buffSwitch = false;

    //已经加点了吗
    public bool hasAddSkill;
    //每一级加多少暴击
    private float perLevelAddFunction1;
    //攻击倍率
    private float perLevelAddFunction2;
    //减少cd
    private float perLevelAddFunction3;
    //减速率
    private float perLevelAddFunction4;
    //塔侦察范围
    private float perLevelAddFunction5;
    //子弹加多少飞行速度
    private float perLevelAddFunction6;

    //技能点的初始点数
    // private int InitSkillPoint = 0;
    private int maxTowerIndex;
    int skillPoint;
    //  int maxSkillPoint;
    int currentSelect;
    [SerializeField]
    private Tower currentSelectTower;

    public Tower CurrentSelectTower { get { return towerCSList[CurrentSelect]; } set => currentSelectTower = value; }
    public int SkillPoint
    {
        get => skillPoint; set
        {
            skillPoint = value;
            TextSkillPoint.text = $"剩余技能点数:{value}";
            UpdateCurrentTowerMessage();
            UpdateWantShowMaxLevelSkillMessage(CurrentSelect);
            UPdateBtnState();
            UpdatePrevAndAfterBtnState();
        }
    }
    //  public int MaxSkillPoint { get => maxSkillPoint; set => maxSkillPoint = value; }
    public int CurrentSelect
    {
        get => currentSelect; set
        {
            if (value > maxTowerIndex)
            {
                value = 0;
            }
            if (value < 0)
            {
                value = maxTowerIndex;
            }
            currentSelect = value;
            towerImage.sprite = sprites[value];
            UpdateCurrentTowerMessage();
            UPdateBtnState();
        }
    }
    public float PerLevelAddFunction1 { get => MyConfigManager.Instance.towerConfigParent.perLevelAddCrit; set => perLevelAddFunction1 = value; }
    //攻击力
    public float PerLevelAddFunction2 { get => MyConfigManager.Instance.towerConfigParent.perLevelAttackRate; set => perLevelAddFunction2 = value; }
    public float PerLevelAddFunction3 { get => MyConfigManager.Instance.towerConfigParent.perLevelReduceCD; set => perLevelAddFunction3 = value; }
    public float PerLevelAddFunction4 { get => MyConfigManager.Instance.towerConfigParent.perLevelReduceSpeedRate; set => perLevelAddFunction4 = value; }
    public float PerLevelAddFunction5 { get => MyConfigManager.Instance.towerConfigParent.perLevelAddMonitoringScope; set => perLevelAddFunction5 = value; }
    public float PerLevelAddFunction6 { get => MyConfigManager.Instance.towerConfigParent.perLevelAddBulletSpeed; set => perLevelAddFunction6 = value; }
    //是否显示技能解锁
    public bool IsShowTextMaxLevelMessage
    {
        get
        {
            return isShowTextMaxLevelMessage;
        }
        set
        {
            isShowTextMaxLevelMessage = value;
            if (value)
            {
                string str = "";
                string towerName = "";
                switch (CurrentSelect)
                {
                    case 0:
                        str = $"{Consts.TowerMaxLevel}级开启被动技能,子弹变成每发伤害敌人最大生命的{Consts.percentageDemageT1 * 100}%";
                        towerName = "待升级塔T1";
                        break;
                    case 1:
                        str = $"{Consts.TowerMaxLevel}级开启被动技能,子弹有{Consts.DurationOfBurningBrithRate * 100}%几率让敌人灼烧,每次造成敌人当前生命的{Consts.DurationOfBurningDemagePercentage * 100}%";
                        towerName = "待升级塔T2";
                        break;
                    case 2:
                        str = $"{Consts.TowerMaxLevel}级开启被动技能,子弹爆炸范围增大一倍";
                        towerName = "待升级塔F1";
                        break;
                    case 3:
                        str = $"{Consts.TowerMaxLevel}级开启被动技能,塔不会受伤";
                        towerName = "待升级塔T3";
                        break;
                    case 4:
                        str = $"{Consts.TowerMaxLevel}级开启被动技能,增加{Consts.AddExplodeRate * 100}%的装备爆率";
                        towerName = "待升级塔F2";
                        break;
                    case 5:
                        str = $"{Consts.TowerMaxLevel}级开启被动技能,子弹{Consts.fatalityRate * 100}%的几率让敌人直接死亡";
                        towerName = "待升级塔T4";
                        break;
                    case 6:
                        str = $"{Consts.TowerMaxLevel}级开启被动技能,该塔消灭的敌人出双倍金币";
                        towerName = "待升级塔F3";
                        break;
                }
                //更新信息
                UpdateTextMaxLevelMessage(str);
                CurrentTowerName.text = towerName;
            }
            else
            {
                TextMaxLevelMessage.text = "";
            }
        }
    }



    bool isShowTextMaxLevelMessage;

    private void Awake()
    {
        for (int i = 0; i < towerList.Length; i++)
        {
            GameObject towerObj = GameObject.Instantiate(towerList[i], transform.position, Quaternion.identity);
            towerCSList.Add(towerObj.GetComponent<Tower>());
            towerObj.transform.SetParent(transform);
        }
    }

    private void Start()
    {
        CurrentSelect = 0;
        maxTowerIndex = MyConfigManager.Instance.myLevelData.maxTowerIndex;
    }

    private void UpdateTextMaxLevelMessage(string str)
    {
        TextMaxLevelMessage.text = str;
    }

    //public void SetInitPoint()
    //{
    //    InitSkillPoint = SkillPoint;
    //}

    public void BtnUp()
    {
        CurrentSelect++;
        MyAudioManager.GetInstance().PlaySound(Consts.enterEffect);
        UpdateNeedMoneyText1();

    }

    public void BtnDown()
    {
        CurrentSelect--;
        MyAudioManager.GetInstance().PlaySound(Consts.backEffect);
        UpdateNeedMoneyText1();
    }

    public void BtnAddSkillPoint()
    {

        UpdateAdd();
        CurrentSelectTower.critRate += PerLevelAddFunction1;
        int temp1 = (int)(CurrentSelectTower.CritRate * 100);
        FunctionText1.text = $"当前暴击几率{FixedTo100(temp1) }%";

    }

    public void BtnReduceSkillPoint()
    {

        UpdateReduce();
        CurrentSelectTower.critRate -= PerLevelAddFunction1;
        int temp1 = (int)(CurrentSelectTower.CritRate * 100);
        FunctionText1.text = $"当前暴击几率{FixedTo100(temp1)}%";

    }

    public void BtnAddSkillPoint2()
    {

        UpdateAdd();
        CurrentSelectTower.towerDemageFixed += PerLevelAddFunction2;
        FunctionText2.text = $"当前攻击倍率{CurrentSelectTower.TowerDemageFixed:f1}";

    }

    public void BtnReduceSkillPoint2()
    {

        UpdateReduce();
        CurrentSelectTower.towerDemageFixed -= PerLevelAddFunction2;
        FunctionText2.text = $"当前攻击倍率{CurrentSelectTower.TowerDemageFixed:f1}";

    }

    public void BtnAddSkillPoint3()
    {

        UpdateAdd();
        CurrentSelectTower.shootCD -= PerLevelAddFunction3;
        FunctionText3.text = $"当前攻击冷却{CurrentSelectTower.ShootCD:f1}";

    }

    public void BtnReduceSkillPoint3()
    {

        UpdateReduce();
        CurrentSelectTower.shootCD += PerLevelAddFunction3;
        FunctionText3.text = $"当前攻击冷却{CurrentSelectTower.ShootCD:f1}";

    }

    public void BtnAddSkillPoint4()
    {

        UpdateAdd();
        CurrentSelectTower.reduceEnemySpeedRate += PerLevelAddFunction4;
        int tempReduceSpeedRate = (int)(CurrentSelectTower.ReduceEnemySpeedRate * 100);
        FunctionText4.text = $"减敌人速度率{FixedTo100(tempReduceSpeedRate)}%";

    }

    public void BtnReduceSkillPoint4()
    {

        UpdateReduce();
        CurrentSelectTower.reduceEnemySpeedRate -= PerLevelAddFunction4;
        int tempReduceSpeedRate = (int)(CurrentSelectTower.ReduceEnemySpeedRate * 100);
        FunctionText4.text = $"减敌人速度率{FixedTo100(tempReduceSpeedRate)}%";

    }

    public void BtnAddSkillPoint5()
    {

        UpdateAdd();
        CurrentSelectTower.towerMonitorDetectRange += PerLevelAddFunction5;
        FunctionText5.text = $"塔侦察范围{CurrentSelectTower.TowerMonitorDetectRange:f1}";

    }

    public void BtnReduceSkillPoint5()
    {

        UpdateReduce();
        CurrentSelectTower.towerMonitorDetectRange -= PerLevelAddFunction5;
        FunctionText5.text = $"塔侦察范围{CurrentSelectTower.TowerMonitorDetectRange:f1}";

    }

    public void BtnAddSkillPoint6()
    {

        UpdateAdd();
        CurrentSelectTower.bulletSpeed += PerLevelAddFunction6;
        FunctionText6.text = $"子弹飞行速度{CurrentSelectTower.BulletSpeed:f1}";

    }

    public void BtnReduceSkillPoint6()
    {

        UpdateReduce();
        CurrentSelectTower.bulletSpeed -= PerLevelAddFunction6;
        FunctionText6.text = $"子弹飞行速度{CurrentSelectTower.BulletSpeed:f1}";

    }

    [SerializeField]
    private int tempMoney = 0;

    //应该有问题
    public int TempMoney
    {
        get
        {
            return -towerCSList[CurrentSelect].Price * towerCSList[CurrentSelect].CurrentLevel;

        }
        set => tempMoney = value;
    }

    private void UpdateReduce()
    {


        hasAddSkill = false;
        SkillPoint++;
     
        CurrentSelectTower.CurrentLevel -= 1;
        UIMain.Instance.InitialMoney += towerCSList[CurrentSelect].Price * towerCSList[CurrentSelect].CurrentLevel;

        levelText.text = $"等级：{ CurrentSelectTower.CurrentLevel}";
        MyAudioManager.GetInstance().PlaySound(Consts.backEffect);
        UpdateNeedMoneyText1();
    }

    private void UpdateAdd()
    {
        hasAddSkill = true;
        SkillPoint--;

        UIMain.Instance.InitialMoney -= towerCSList[CurrentSelect].Price * towerCSList[CurrentSelect].CurrentLevel;

        CurrentSelectTower.CurrentLevel += 1;

        levelText.text = $"等级：{CurrentSelectTower.CurrentLevel}";
        MyAudioManager.GetInstance().PlaySound(Consts.enterEffect);
        UpdateNeedMoneyText1();
    }

    public void UpdatePrevAndAfterBtnState()
    {
        if (hasAddSkill)
        {
            foreach (Button btn in prevAndAfterBtn)
            {
                btn.interactable = false;
            }
        }
        else
        {
            foreach (Button btn in prevAndAfterBtn)
            {
                btn.interactable = true;
            }
        }
    }

    //public void CalculateMoney()
    //{

    //    UIMain.Instance.InitialMoney += TempMoney;
    //}

    public void UPdateBtnState()
    {
            //更新add的状态
            if ((UIMain.Instance.InitialMoney - towerCSList[CurrentSelect].Price * towerCSList[CurrentSelect].CurrentLevel) >=
                0 && SkillPoint > 0)
            {
                foreach (Button btn in btnAdds)
                {
                    btn.interactable = true;
                }
               // Debug.Log("true");
            }
            else
            {
                foreach (Button btn in btnAdds)
                {
                    btn.interactable = false;
                }
               // Debug.Log("false");
            }


            if (SkillPoint == 0)
            {
                foreach (Button btn in btnReduces)
                {
                    btn.interactable = true;
                }
            }
            else
            {
                foreach (Button btn in btnReduces)
                {
                    btn.interactable = false;
                }
            }
    }

    public void UpdateCurrentTowerMessage()
    {
        int temp1 = (int)(CurrentSelectTower.CritRate * 100);
        FunctionText1.text = $"当前暴击几率{FixedTo100(temp1)}%";
        FunctionText2.text = $"当前攻击倍率{CurrentSelectTower.TowerDemageFixed:f1}";
        FunctionText3.text = $"当前攻击冷却{CurrentSelectTower.ShootCD:f1}";
        int tempReduceSpeedRate = (int)(CurrentSelectTower.ReduceEnemySpeedRate * 100);
        FunctionText4.text = $"减敌人速度率{FixedTo100(tempReduceSpeedRate)}%";
        FunctionText5.text = $"塔侦察范围{CurrentSelectTower.TowerMonitorDetectRange:f1}";
        FunctionText6.text = $"子弹飞行速度{CurrentSelectTower.BulletSpeed:f1}";
        levelText.text = $"等级：{CurrentSelectTower.CurrentLevel}";
        UpdateWantShowMaxLevelSkillMessage(CurrentSelect);
        UpdateNeedMoneyText1();

        UPdateBtnState();
        UpdatePrevAndAfterBtnState();
    }

    private int FixedTo100(int value)
    {
        if (value >= 100)
        {
            value = 100;
        }

        return value;
    }

    public void UpdateWantShowMaxLevelSkillMessage(int index)
    {
        if (towerCSList[index].CurrentLevel >= Consts.TowerMaxLevel)
        {
            IsShowTextMaxLevelMessage = true;
        }
        else
        {
            IsShowTextMaxLevelMessage = true;
        }
    }

    public void GetTowerIndexMessage(int TowerIndex, out float critRate, out float TowerDemageFixed, out float shootCd, out float reduceSpeedRate, out float detectRange, out float bulletFlySpeed, out int level, out int Price22)
    {
        critRate = towerCSList[TowerIndex].CritRate;
        TowerDemageFixed = towerCSList[TowerIndex].TowerDemageFixed;
        shootCd = towerCSList[TowerIndex].ShootCD;
        reduceSpeedRate = towerCSList[TowerIndex].ReduceEnemySpeedRate;
        detectRange = towerCSList[TowerIndex].TowerMonitorDetectRange;
        bulletFlySpeed = towerCSList[TowerIndex].BulletSpeed;
        level = towerCSList[TowerIndex].CurrentLevel;
        Price22 = towerCSList[TowerIndex].Price;
    }

    public int GetTowerIndexPrice(int TowerIndex)
    {
        return towerCSList[TowerIndex].Price;
    }

    private void UpdateNeedMoneyText1()
    {
        NeedMoneyText1.text = TempMoney.ToString();
    }

    internal void OpenPanel()
    {
        rectTransform.offsetMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(0, 0);
        MyAudioManager.GetInstance().PlaySound(Consts.enterEffect);
    }

    internal void ClosePanel()
    {
        rectTransform.offsetMin = new Vector2(-1800, 0);
        rectTransform.offsetMax = new Vector2(-1800, 0);
        MyAudioManager.GetInstance().PlaySound(Consts.backEffect);
    }
}
