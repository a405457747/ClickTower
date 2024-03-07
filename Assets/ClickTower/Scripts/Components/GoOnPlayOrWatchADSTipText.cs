using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using xmaolol.com;


public class GoOnPlayOrWatchADSTipText : MonoBehaviour
{
    //是重玩是下一关的提示
    public bool IsRetryPlayMessage;

    private Text text;
    private MySaveManager tempMySaveManager;

    void Start()
    {
        tempMySaveManager = MySaveManager.Instance;
        text = GetComponent<Text>();
    }


    void Update()
    {
        UpdateText();
    }

    void UpdateText()
    {
        if (IsRetryPlayMessage)
        {
            if (tempMySaveManager.CanPlayGame())
            {
                text.text = "重玩";
            }
            else
            {
                text.text = "重玩(补充体力)";
            }
        }
        else
        {
            if (tempMySaveManager.CanPlayGame())
            {
                text.text = "下一关";
            }
            else
            {
                text.text = "下一关(补充体力)";
            }
        }
    }
}
