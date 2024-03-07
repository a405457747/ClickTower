using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using xmaolol.com;


public class GoOnPlayOrWatchADSTipText : MonoBehaviour
{
    //����������һ�ص���ʾ
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
                text.text = "����";
            }
            else
            {
                text.text = "����(��������)";
            }
        }
        else
        {
            if (tempMySaveManager.CanPlayGame())
            {
                text.text = "��һ��";
            }
            else
            {
                text.text = "��һ��(��������)";
            }
        }
    }
}
