using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using xmaolol.com;
using QFramework;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;

public class MySceneManager : MonoSingleton<MySceneManager>
{
    //开关待做呢
    AsyncOperation async;
    public Slider Slider;
    private int barProgress;
    Action Del;
    bool isLoadingMode;

    Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        IsLoadingMode = false;
        ChangeWhiteAnimation();
    }
    private void ChangeWhiteAnimation()
    {
        image.DOFade(0, Consts.MaskPanelSaveTime).OnComplete(() => {
            gameObject.SetActive(false);
            //if (MainManager.Instance != null)
            //{
            //    MainManager.Instance.GameState = GameState.GameStart;
            //}
        });
    }

    public bool IsLoadingMode
    {
        get => isLoadingMode; set
        {
            isLoadingMode = value;

            if (value == true)
            {
                transform.Find("LoadingContent").gameObject.SetActive(true);
            }else
            {
                transform.Find("LoadingContent").gameObject.SetActive(false);
            }
        }
    }

    public void ChangeBlackAnimation(string sceneName, Action del)
    {
        image.DOFade(1, Consts.MaskPanelSaveTime).OnComplete(() => {
            LoadScene(sceneName, del);
        });
    }

    public void LoadScene(string sceneName, Action del)
    {


        #region load部分
        IsLoadingMode = true;
        barProgress = 0;
        this.Del = del;
        StartCoroutine("LoadScece", sceneName);
        #endregion
    }

    IEnumerator LoadScece(string sceneName)
    {
        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        yield return async;
    }

    private void Update()
    {
        if (async == null)
        {
            return;
        }
        else
        {
            int theProgress = 0;

            if (async.progress < 0.9f)
            {
                theProgress = (int)(async.progress * 100);
            }
            else
            {
                theProgress = 100;
            }
            if (barProgress < theProgress)
            {
                barProgress++;
            }
            Slider.value = barProgress / 100f;
            if (barProgress == 100)
            {
                async.allowSceneActivation = true;
            }
            if (async.isDone)
            {
                Del?.Invoke();
                //清理
                async = null;
                MyTool.ClearMemory();
                IsLoadingMode = false;
                this.gameObject.SetActive(false);
                //然场景显示
            }
        }
    }
}
