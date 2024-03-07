using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using xmaolol.com;
using QFramework;
using Lean.Pool;


public class Hp_Num_Canvas : MonoBehaviour
{
    public Text numText;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private GameObject laserRedShot;
    private float offsetUp = 0.86f;
    private float wantUpDis;
    private float initPosY;
    private float cd = 0.45f;
    private float cd2 = 0.45f;
    //ÊÇ·ñÊÇ±©»÷
    private bool isCrit = false;
    private int originalSize;
    private int CritSize;

    public bool IsCrit
    {
        get => isCrit; set
        {
            isCrit = value;
            if (value == true)
            {
                numText.fontSize = CritSize;
                numText.color = Color.red;
                laserRedShot.SetActive(true);
            }
            else
            {
                numText.fontSize = originalSize;
                numText.color = new Color32(255, 102, 51, 255);
                if (laserRedShot.activeInHierarchy == true)
                {
                    laserRedShot.SetActive(false);
                }
            }
        }
    }

    private void Awake()
    {
        laserRedShot = transform.Find("Text/laserRedShot").gameObject;
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        originalSize = numText.fontSize;
        CritSize = (int)(originalSize * 1.2f);
    }
    private void OnSpawn()
    {
        initPosY = rectTransform.anchoredPosition.y;
        wantUpDis = initPosY + offsetUp;
        var s = DOTween.Sequence();
        s.Append(rectTransform.DOAnchorPos3DY(initPosY, Consts.AnimationResetTime)).SetEase(Ease.Linear);
        s.Insert(0, canvasGroup.DOFade(1, Consts.AnimationResetTime).SetEase(Ease.Linear));
    }

    public void InitData(int demageNum, bool isCrit)
    {
        IsCrit = isCrit;
        numText.text = demageNum.ToString();
        var s = DOTween.Sequence();
        s.Append(rectTransform.DOAnchorPos3DY(wantUpDis, cd + cd2)).SetEase(Ease.Linear);
        s.Insert(cd, canvasGroup.DOFade(0, cd2).SetEase(Ease.Linear));
        s.InsertCallback((cd + cd2), () => { DestorySelf(); });
    }

    public void DestorySelf()
    {
        LeanPool.Despawn(this.gameObject);
    }
}
