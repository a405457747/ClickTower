using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  DG.Tweening;
using QFramework;
using xmaolol.com;

//后面和QF集成啊
public class MyComLogoPanel : MonoBehaviour
{

    private CanvasGroup canvasGroup;
    
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        this.Delay(Consts.ComLogoSaveTime, () =>
            {
                canvasGroup.DOFade(0, Consts.ComLogoFadeTime).OnComplete(() =>
                {
                    this.gameObject.SetActive(false);
                    MyAudioManager.GetInstance().PlayMusic("DST-TowerDefenseTheme_1");

                });
            });
    }
}
