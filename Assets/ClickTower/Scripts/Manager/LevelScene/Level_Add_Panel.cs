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

    //�Ѿ��ӵ�����
    public bool hasAddSkill;
    //ÿһ���Ӷ��ٱ���
    private float perLevelAddFunction1;
    //��������
    private float perLevelAddFunction2;
    //����cd
    private float perLevelAddFunction3;
    //������
    private float perLevelAddFunction4;
    //����췶Χ
    private float perLevelAddFunction5;
    //�ӵ��Ӷ��ٷ����ٶ�
    private float perLevelAddFunction6;

    //���ܵ�ĳ�ʼ����
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
            TextSkillPoint.text = $"ʣ�༼�ܵ���:{value}";
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
    //������
    public float PerLevelAddFunction2 { get => MyConfigManager.Instance.towerConfigParent.perLevelAttackRate; set => perLevelAddFunction2 = value; }
    public float PerLevelAddFunction3 { get => MyConfigManager.Instance.towerConfigParent.perLevelReduceCD; set => perLevelAddFunction3 = value; }
    public float PerLevelAddFunction4 { get => MyConfigManager.Instance.towerConfigParent.perLevelReduceSpeedRate; set => perLevelAddFunction4 = value; }
    public float PerLevelAddFunction5 { get => MyConfigManager.Instance.towerConfigParent.perLevelAddMonitoringScope; set => perLevelAddFunction5 = value; }
    public float PerLevelAddFunction6 { get => MyConfigManager.Instance.towerConfigParent.perLevelAddBulletSpeed; set => perLevelAddFunction6 = value; }
    //�Ƿ���ʾ���ܽ���
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
                        str = $"{Consts.TowerMaxLevel}��������������,�ӵ����ÿ���˺��������������{Consts.percentageDemageT1 * 100}%";
                        towerName = "��������T1";
                        break;
                    case 1:
                        str = $"{Consts.TowerMaxLevel}��������������,�ӵ���{Consts.DurationOfBurningBrithRate * 100}%�����õ�������,ÿ����ɵ��˵�ǰ������{Consts.DurationOfBurningDemagePercentage * 100}%";
                        towerName = "��������T2";
                        break;
                    case 2:
                        str = $"{Consts.TowerMaxLevel}��������������,�ӵ���ը��Χ����һ��";
                        towerName = "��������F1";
                        break;
                    case 3:
                        str = $"{Consts.TowerMaxLevel}��������������,����������";
                        towerName = "��������T3";
                        break;
                    case 4:
                        str = $"{Consts.TowerMaxLevel}��������������,����{Consts.AddExplodeRate * 100}%��װ������";
                        towerName = "��������F2";
                        break;
                    case 5:
                        str = $"{Consts.TowerMaxLevel}��������������,�ӵ�{Consts.fatalityRate * 100}%�ļ����õ���ֱ������";
                        towerName = "��������T4";
                        break;
                    case 6:
                        str = $"{Consts.TowerMaxLevel}��������������,��������ĵ��˳�˫�����";
                        towerName = "��������F3";
                        break;
                }
                //������Ϣ
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
        FunctionText1.text = $"��ǰ��������{FixedTo100(temp1) }%";

    }

    public void BtnReduceSkillPoint()
    {

        UpdateReduce();
        CurrentSelectTower.critRate -= PerLevelAddFunction1;
        int temp1 = (int)(CurrentSelectTower.CritRate * 100);
        FunctionText1.text = $"��ǰ��������{FixedTo100(temp1)}%";

    }

    public void BtnAddSkillPoint2()
    {

        UpdateAdd();
        CurrentSelectTower.towerDemageFixed += PerLevelAddFunction2;
        FunctionText2.text = $"��ǰ��������{CurrentSelectTower.TowerDemageFixed:f1}";

    }

    public void BtnReduceSkillPoint2()
    {

        UpdateReduce();
        CurrentSelectTower.towerDemageFixed -= PerLevelAddFunction2;
        FunctionText2.text = $"��ǰ��������{CurrentSelectTower.TowerDemageFixed:f1}";

    }

    public void BtnAddSkillPoint3()
    {

        UpdateAdd();
        CurrentSelectTower.shootCD -= PerLevelAddFunction3;
        FunctionText3.text = $"��ǰ������ȴ{CurrentSelectTower.ShootCD:f1}";

    }

    public void BtnReduceSkillPoint3()
    {

        UpdateReduce();
        CurrentSelectTower.shootCD += PerLevelAddFunction3;
        FunctionText3.text = $"��ǰ������ȴ{CurrentSelectTower.ShootCD:f1}";

    }

    public void BtnAddSkillPoint4()
    {

        UpdateAdd();
        CurrentSelectTower.reduceEnemySpeedRate += PerLevelAddFunction4;
        int tempReduceSpeedRate = (int)(CurrentSelectTower.ReduceEnemySpeedRate * 100);
        FunctionText4.text = $"�������ٶ���{FixedTo100(tempReduceSpeedRate)}%";

    }

    public void BtnReduceSkillPoint4()
    {

        UpdateReduce();
        CurrentSelectTower.reduceEnemySpeedRate -= PerLevelAddFunction4;
        int tempReduceSpeedRate = (int)(CurrentSelectTower.ReduceEnemySpeedRate * 100);
        FunctionText4.text = $"�������ٶ���{FixedTo100(tempReduceSpeedRate)}%";

    }

    public void BtnAddSkillPoint5()
    {

        UpdateAdd();
        CurrentSelectTower.towerMonitorDetectRange += PerLevelAddFunction5;
        FunctionText5.text = $"����췶Χ{CurrentSelectTower.TowerMonitorDetectRange:f1}";

    }

    public void BtnReduceSkillPoint5()
    {

        UpdateReduce();
        CurrentSelectTower.towerMonitorDetectRange -= PerLevelAddFunction5;
        FunctionText5.text = $"����췶Χ{CurrentSelectTower.TowerMonitorDetectRange:f1}";

    }

    public void BtnAddSkillPoint6()
    {

        UpdateAdd();
        CurrentSelectTower.bulletSpeed += PerLevelAddFunction6;
        FunctionText6.text = $"�ӵ������ٶ�{CurrentSelectTower.BulletSpeed:f1}";

    }

    public void BtnReduceSkillPoint6()
    {

        UpdateReduce();
        CurrentSelectTower.bulletSpeed -= PerLevelAddFunction6;
        FunctionText6.text = $"�ӵ������ٶ�{CurrentSelectTower.BulletSpeed:f1}";

    }

    [SerializeField]
    private int tempMoney = 0;

    //Ӧ��������
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

        levelText.text = $"�ȼ���{ CurrentSelectTower.CurrentLevel}";
        MyAudioManager.GetInstance().PlaySound(Consts.backEffect);
        UpdateNeedMoneyText1();
    }

    private void UpdateAdd()
    {
        hasAddSkill = true;
        SkillPoint--;

        UIMain.Instance.InitialMoney -= towerCSList[CurrentSelect].Price * towerCSList[CurrentSelect].CurrentLevel;

        CurrentSelectTower.CurrentLevel += 1;

        levelText.text = $"�ȼ���{CurrentSelectTower.CurrentLevel}";
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
            //����add��״̬
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
        FunctionText1.text = $"��ǰ��������{FixedTo100(temp1)}%";
        FunctionText2.text = $"��ǰ��������{CurrentSelectTower.TowerDemageFixed:f1}";
        FunctionText3.text = $"��ǰ������ȴ{CurrentSelectTower.ShootCD:f1}";
        int tempReduceSpeedRate = (int)(CurrentSelectTower.ReduceEnemySpeedRate * 100);
        FunctionText4.text = $"�������ٶ���{FixedTo100(tempReduceSpeedRate)}%";
        FunctionText5.text = $"����췶Χ{CurrentSelectTower.TowerMonitorDetectRange:f1}";
        FunctionText6.text = $"�ӵ������ٶ�{CurrentSelectTower.BulletSpeed:f1}";
        levelText.text = $"�ȼ���{CurrentSelectTower.CurrentLevel}";
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
