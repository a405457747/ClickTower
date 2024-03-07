using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using QFramework;
using xmaolol.com;

public class Image_item : MonoBehaviour
{

    public Text detail;
    public Text head;

    private int nameIndex;
    private int currentGameLevel;
    private Button start;

    public int NameIndex
    {
        get => nameIndex; set
        {
            nameIndex = value;
            if (value <= MySaveManager.Instance.SaveMapping.CurrentGameLevel && MySaveManager.Instance.CanPlayGame())
            {
                start.interactable = true;
            }
            else
            {
                start.interactable = false;
            }
        }
    }

    private void Awake()
    {
        start = transform.Find("Text_start").GetComponent<Button>();
        detail = transform.Find("Text_detail").GetComponent<Text>();
        head = transform.Find("Text_head").GetComponent<Text>();
        start.onClick.AddListener(ClickButton);
    }

    private void Start()
    {
        int num = MyTool.GetNumberByString(this.gameObject.name) + 1;
        string detailStr = $"这是关卡{num}";
        string headStr = $"关卡{num}";
        UpdateUI(detailStr, headStr);
    }

    public void UpdateUI(string detailStr, string headStr)
    {
        detail.text = detailStr;
        head.text = headStr;
    }

    public void ClickButton()
    {
        int num = MyTool.GetNumberByString(this.gameObject.name);
        string sceneName = $"Level{num}";
        MyTool.OpenLoadSceneHelper();
        MySceneManager.Instance.ChangeBlackAnimation(sceneName, () => { });
        MyAudioManager.GetInstance().PlaySound(Consts.enterEffect);

        //消耗体力值
        MySaveManager.Instance.ReducePhysicalPowerAndSave();
    }

    private void OnDestroy()
    {
        start.onClick.RemoveListener(ClickButton);
    }
}
