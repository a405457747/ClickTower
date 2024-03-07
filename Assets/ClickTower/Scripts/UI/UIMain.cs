using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using xmaolol.com;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIMain : MonoSingleton<UIMain>
{
    public LevelAddProp[] levelAddProps;
    public Text LevelName;
    public Text moneyText2;
    public GameObject SkillPointPanel;
    public GameObject[] towerToggles;
    public GameObject GameOverPanel;
    public GameObject GameWinPanel;
    public GameObject GamePausePanel;
    public Text TowerSellPriceTextF2;
    public Text TowerSellPriceTextF3;
    public Text TowerSellPriceTextF1;
    public Text TowerSellPriceTextT1;
    public Text TowerSellPriceTextT2;
    public Text TowerSellPriceTextT3;
    public Text TowerSellPriceTextT4;
    public Text moneyText;
    public Text heartCountText;
    public Text WaveCountText;

    private int initialMoney;
    private int heart;
    private Sequence mScoreSequence;
    int F1 { get { return Level_Add_Panel.Instance.GetTowerIndexPrice(2); } }
    int F2 { get { return Level_Add_Panel.Instance.GetTowerIndexPrice(4); } }
    int F3 { get { return Level_Add_Panel.Instance.GetTowerIndexPrice(6); } }
    int T1 { get { return Level_Add_Panel.Instance.GetTowerIndexPrice(0); } }
    int T2 { get { return Level_Add_Panel.Instance.GetTowerIndexPrice(1); } }
    int T3 { get { return Level_Add_Panel.Instance.GetTowerIndexPrice(3); } }
    int T4 { get { return Level_Add_Panel.Instance.GetTowerIndexPrice(5); } }

    public int InitialMoney
    {
        get => initialMoney; set
        {
            UpdateMoneyText(initialMoney, value);
            initialMoney = value;
            MyAudioManager.GetInstance().PlaySound(Consts.coinEffect);
            UpdatelevelAddProps(value - Level_Add_Panel.Instance.towerCSList[Level_Add_Panel.Instance.CurrentSelect].Price >= 0);
        }
    }

    public int Heart
    {
        get
        {
            return heart;
        }
        set
        {
            heart = value;

            UpdaeHeartCount(value);
            House.Instance.BeInjured();
            MyAudioManager.GetInstance().PlaySound(Consts.heartEffect);

            if ((value == 0) && MainManager.Instance.GameState != GameState.Win)
            {
                MainManager.Instance.GameState = GameState.GameOver;
                GameOver();
            }
        }
    }

    private void Awake()
    {
        int randomValue = 0;
        heart = 4;
        if (MySaveManager.Instance.HavePropDemageFixed(out randomValue, '3'))
        {
            heart += randomValue;
        }
        UpdaeHeartCount(Heart);
        mScoreSequence = DOTween.Sequence();
        mScoreSequence.SetAutoKill(false);
    }

    private void Start()
    {
        Level_Add_Panel level_Add_Panel = Level_Add_Panel.Instance;
        UpdateTowerSellPriceText(F2, F3, F1, T1, T2, T3, T4);
        MyConfigManager myConfigManager = MyConfigManager.Instance;
        UpdateTowerToggles(myConfigManager.myLevelData.maxTowerIndex, myConfigManager.myLevelData.initialMoney);
    }

    private void UpdatelevelAddProps(bool isShow)
    {
        foreach (var item in levelAddProps)
        {
            item.UpdateAddBtn(isShow);
        }
    }

    private void UpdateTowerToggles(int towerRange, int initialMoney)
    {
        InitialMoney = initialMoney;
        for (int i = 0; i < towerToggles.Length; i++)
        {
            if (i <= towerRange)
            {
                towerToggles[i].SetActive(true);
            }
            else
            {
                towerToggles[i].SetActive(false);
            }
        }
    }

    public void DoShakeMoneyText()
    {
        moneyText.transform.DOPunchPosition(new Vector3(2, 0, 0), 1.5f, 10, 0.5f);
    }

    public void UpdateMoneyText(int mOldScore, int newScore)
    {
        mScoreSequence.Append(DOTween.To(delegate (float value)
        {
            //向下取整
            var temp = Mathf.Floor(value);
            //向Text组件赋值
            moneyText.text = temp + "";
            moneyText2.text = temp + "";
        }, mOldScore, newScore, 0.13f));
    }

    public void UpdaeHeartCount(int num)
    {
        heartCountText.text = num.ToString();
    }

    public void HeartBeInjured(int num)
    {
        Heart -= num;
        SC_shakeCamera.shakeCamera();
    }

    public void GameOver()
    {
        this.Delay(Consts.PanelDelayTime, () =>
        {
            GameOverPanel.SetActive(true);
            MySaveManager.Instance.RecordNowTimestampAndSave();
        });
    }


    public void GameWin()
    {
        this.Delay(Consts.PanelDelayTime, () =>
        {
            GameWinPanel.SetActive(true);
            //胜利了保存更新进度
            if (MySaveManager.Instance.SaveMapping.CurrentGameLevel == MyTool.GetNumberByString(SceneManager.GetActiveScene().name))
            {
                MySaveManager.Instance.SaveMapping.CurrentGameLevel++;
            }
            MySaveManager.Instance.RecordNowTimestampAndSave();
        });
    }

    public void Pause()
    {
        MyYomobManager.Instance.PlayTableADOrCanCloseVedioAD();
        MyAudioManager.GetInstance().PauseMusic();
        MyAudioManager.GetInstance().PlaySound(Consts.enterEffect);
        int nameIndex = MyTool.GetNumberByString(SceneManager.GetActiveScene().name);
        LevelName.text = $"第{nameIndex + 1}关";
        MainManager.Instance.GameState = GameState.Pause;
        GamePausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumePause()
    {
        Time.timeScale = 1;
        GamePausePanel.SetActive(false);
        MyAudioManager.GetInstance().PlaySound(Consts.backEffect);
        MyAudioManager.GetInstance().ResumeMusic();
        this.Delay(0.15f, () => { MainManager.Instance.GameState = GameState.GameStart; });
    }

    public void RetryPlay()
    {
        if (MySaveManager.Instance.CanPlayGame())
        {
            Time.timeScale = 1;
            string sceneName = SceneManager.GetActiveScene().name;
            MyTool.OpenLoadSceneHelper();
            MySceneManager.Instance.ChangeBlackAnimation(sceneName, () => { });
            MyAudioManager.GetInstance().PlaySound(Consts.enterEffect);

            //消耗体力值
            MySaveManager.Instance.ReducePhysicalPowerAndSave();
        }
        else
        {
            MySaveManager.Instance.PlayAdVideo(() => { });
        }
    }

    public void LoadMain()
    {
        Time.timeScale = 1;
        MyTool.OpenLoadSceneHelper();
        MySceneManager.Instance.ChangeBlackAnimation("Main", () => { });
        MyAudioManager.GetInstance().PlaySound(Consts.enterEffect);
    }

    public void LoadNext()
    {
        if (MySaveManager.Instance.CanPlayGame())
        {
            int currentIndex = MyTool.GetNumberByString(SceneManager.GetActiveScene().name);
            currentIndex += 1;
            if (currentIndex >= Consts.MaxGameLevel)
            {
                currentIndex = Consts.MaxGameLevel;
            }
            MyTool.OpenLoadSceneHelper();
            string nextSceneStr = $"Level{currentIndex}";
            MySceneManager.Instance.ChangeBlackAnimation(nextSceneStr, () => { });
            MyAudioManager.GetInstance().PlaySound(Consts.enterEffect);

            //消耗体力值
            MySaveManager.Instance.ReducePhysicalPowerAndSave();
        }
        else
        {
            MySaveManager.Instance.PlayAdVideo(() => { });
        }
    }

    public void OpenSkillPointPanel()
    {
        if (MainManager.Instance.GameState == GameState.GameStart)
        {
            MainManager.Instance.GameState = GameState.LevelAdd;
            this.Delay(Consts.PanelDelayTime, () =>
            {
                Level_Add_Panel.Instance.OpenPanel();
                Level_Add_Panel.Instance.SkillPoint = 1;
                Level_Add_Panel.Instance.hasAddSkill = false;
                Level_Add_Panel.Instance.UpdatePrevAndAfterBtnState();
                //Level_Add_Panel.Instance.SetInitPoint();
                // Level_Add_Panel.Instance.MaxSkillPoint = Level_Add_Panel.Instance.SkillPoint;
                Level_Add_Panel.Instance.buffSwitch = true;
            });
        }
    }

    //分配技能点确定
    public void SkillPointConfim()
    {
        // EnemySpawn.Instance.AddCurrentEnemyIndex();
        Level_Add_Panel.Instance.ClosePanel();
        this.Delay(0.2f, () =>
        {
            MainManager.Instance.GameState = GameState.GameStart;
        });
        //更新左侧栏的UI了
        int fF2 = GetRealPrice(4,1200);
        int fF3 = GetRealPrice(6, 1600);
        int fF1 = GetRealPrice(2, 1000);
        int fT1 = GetRealPrice(0, 600);
        int fT2 = GetRealPrice(1, 900);
        int fT3 = GetRealPrice(3, 1050);
        int fT4 = GetRealPrice(5, 1250);

        UpdateTowerSellPriceText(fF2, fF3, fF1, fT1, fT2, fT3, fT4);
        //Level_Add_Panel.Instance.CalculateMoney();
    }

    public int GetRealPrice(int id, int Price)
    {
        Level_Add_Panel.Instance.GetTowerIndexMessage(id, out float c, out float t,
            out float s, out float r, out float d, out float b, out int level, out int price2);

        return Price * level;
    }

    public void UpdateWaveCountText(int currentLevel, int maxLevel)
    {
        WaveCountText.text = $"波数：{currentLevel} /{maxLevel}";
    }

    public void UpdateTowerSellPriceText(int f2, int f3, int f1, int t1, int t2, int t3, int t4)
    {
        if (TowerSellPriceTextF2.IsActive())
        {
            TowerSellPriceTextF2.text = $"{f2.ToString()}";
        }
        if (TowerSellPriceTextF3.IsActive())
        {
            TowerSellPriceTextF3.text = $"{f3.ToString()}";
        }
        if (TowerSellPriceTextF1.IsActive())
        {
            TowerSellPriceTextF1.text = $"{f1.ToString()}";
        }
        if (TowerSellPriceTextT1.IsActive())
        {
            TowerSellPriceTextT1.text = $"{t1.ToString()}";
        }
        if (TowerSellPriceTextT2.IsActive())
        {
            TowerSellPriceTextT2.text = $"{t2.ToString()}";
        }
        if (TowerSellPriceTextT3.IsActive())
        {
            TowerSellPriceTextT3.text = $"{t3.ToString()}";
        }
        if (TowerSellPriceTextT4.IsActive())
        {
            TowerSellPriceTextT4.text = $"{t4.ToString()}";
        }
    }
}

