using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework.Example;
using QFramework;
using xmaolol.com;
using UnityEngine.UI;
//using QFramework.UIExample;

public class MainManager : MonoSingleton<MainManager>
{
    //public  int 
    public SpriteRenderer sr;
    public Sprite[] BGSprites;
    public Sprite[] FloorSprites;
    public Sprite[] glassSprites;
    #region 公共字段
    public TowerData TowerF1;
    public TowerData TowerF2;
    public TowerData TowerF3;
    public TowerData TowerT1;
    public TowerData TowerT2;
    public TowerData TowerT3;
    public TowerData TowerT4;
    public TowerData SelectTower;
    public Vector3 enemyBrithPoint;
    #endregion

    #region 私有字段
    [SerializeField]
    private GameState gameState;
    //可以拆除吗
    [SerializeField]
    private bool canDemolition = false;

    public GameState GameState
    {
        get => gameState; set
        {
            gameState = value;
            if (value == GameState.Win)
            {
                UIMain.Instance.GameWin();
                MyAudioManager.GetInstance().PlaySound(Consts.WinEffect);
            }else if (value == GameState.GameOver)
            {
                MyAudioManager.GetInstance().PlaySound(Consts.LoseEffect);
            }
        }
    }
    public bool CanDemolition
    {
        get => canDemolition; set
        {
            canDemolition = value;
            if (value == true)
            {
                //光标要在x上面，然后手上的塔数据为null;
                SelectTower = null;
            }
        }
    }
    #endregion

    #region Mono函数
    private void Awake()
    {
        GameState = GameState.Null;
        enemyBrithPoint = GameObject.FindWithTag("StartPoint").transform.position;
        this.Delay(1.35f, () => { GameState = GameState.GameStart; });
    }
    private void Start()
    {
        if (MyAudioManager.Instance != null)
        {
            MyAudioManager.Instance.PlayMusic("Zander Noriega - Fight Them Until We Cant");
        }
        sr.sprite = BGSprites[Random.Range(0, BGSprites.Length)];
    }
    #endregion
    #region 公共函数
    public void TowerF1Select(bool isSelect)
    {
        if (!isSelect)
        {
            SelectTower = TowerF1;
            CanDemolition = false;
            MyAudioManager.Instance.PlaySound(Consts.selectTower);
        }
    }

    public void TowerF2Select(bool isSelect)
    {
        if (!isSelect)
        {
            SelectTower = TowerF2;
            CanDemolition = false;
            MyAudioManager.Instance.PlaySound(Consts.selectTower);
        }
    }

    public void TowerF3Select(bool isSelect)
    {
        if (!isSelect)
        {
            SelectTower = TowerF3;
            CanDemolition = false;
            MyAudioManager.Instance.PlaySound(Consts.selectTower);
        }
    }

    public void TowerT1Select(bool isSelect)
    {
        if (!isSelect)
        {
            SelectTower = TowerT1;
            CanDemolition = false;
            MyAudioManager.Instance.PlaySound(Consts.selectTower);
        }
    }

    public void TowerT2Select(bool isSelect)
    {
        if (!isSelect)
        {
            SelectTower = TowerT2;
            CanDemolition = false;
            MyAudioManager.Instance.PlaySound(Consts.selectTower);
        }
    }

    public Sprite GetGlassSprite()
    {
        int random = Random.Range(0, glassSprites.Length);
        return glassSprites[random];
    }

    public Sprite GetFloorSprite()
    {
        int random = Random.Range(0, FloorSprites.Length);
        return FloorSprites[random];
    }


    public void TowerT3Select(bool isSelect)
    {
        if (!isSelect)
        {
            SelectTower = TowerT3;
            CanDemolition = false;
            MyAudioManager.Instance.PlaySound(Consts.selectTower);
        }
    }

    public void TowerT4Select(bool isSelect)
    {
        if (!isSelect)
        {
            SelectTower = TowerT4;
            CanDemolition = false;
            MyAudioManager.Instance.PlaySound(Consts.selectTower);
        }
    }

    public void DestoryTower(bool isSelect)
    {
        if (isSelect)
        {
            CanDemolition = true;
        }
    }
    #endregion
}
