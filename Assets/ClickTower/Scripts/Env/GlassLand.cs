using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Lean.Pool;
using xmaolol.com;

public class GlassLand : MonoBehaviour
{
    [SerializeField]
    private Tower tower;
    MainManager mainManager;
    BoxCollider2D boxCollider2D;
    private GameObject DustEffect;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (DustEffect == null)
        {
            DustEffect = Resources.Load<GameObject>("VortexBurst2");
        }
    }

    public Tower Tower
    {
        get => tower; set
        {
            tower = value;
            if (value != null)
            {
                boxCollider2D.enabled = false;
            }
            else
            {
                boxCollider2D.enabled = true;
            }
        }
    }

    private void Start()
    {
        LetTowerNull();
        mainManager = MainManager.Instance;
        spriteRenderer.sprite = MainManager.Instance.GetGlassSprite();
    }

    public void LetTowerNull()
    {
        Tower = null;
    }

    public void ClickDown()
    {
        if (mainManager.GameState == xmaolol.com.GameState.GameStart)
        {
            int PriceMargin = 0;

            int towerId = mainManager.SelectTower.towerCS.TowerIndex;
            Level_Add_Panel.Instance.GetTowerIndexMessage(towerId, out float c, out float t,
                out float s, out float r, out float d, out float b, out int level, out int price2);
           // Debug.Log(level);
            if (mainManager.SelectTower != null && mainManager.SelectTower.towerCS != null)
            {
                PriceMargin = (UIMain.Instance.InitialMoney - mainManager.SelectTower.towerCS.Price*level);
            }

            if (mainManager.SelectTower != null && mainManager.SelectTower.towerCS != null && (PriceMargin >= 0))
            {
                if (Tower == null)
                {
                    //实例化效果来
                    LeanPool.Spawn(DustEffect, transform.position, Quaternion.identity);
                    MyAudioManager.Instance.PlaySound(Consts.BuildTower);

                    //实例化
                    GameObject towerObj = GameObject.Instantiate(mainManager.SelectTower.towerCS.gameObject, this.transform.position, Quaternion.identity);
                    towerObj.transform.SetParent(this.transform);
                    Tower = towerObj.GetComponent<Tower>();
                    //减少钱
       

                    UIMain.Instance.InitialMoney -= Tower.Price*level;
                }
            }
            else
            {
                UIMain.Instance.DoShakeMoneyText();
            }
        }
    }
}
