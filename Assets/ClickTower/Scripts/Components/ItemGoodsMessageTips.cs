using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.UI;


public class ItemGoodsMessageTips : MonoSingleton<ItemGoodsMessageTips>
{
    private Text message;
    private string contentStr;
    private string ContentStr { get => contentStr;
        set
        {
            contentStr = value;
            UpdateMessage();
            
        }
    }

    void Awake()
    {
        message = GetComponent<Text>();
    }

    void UpdateMessage()
    {
        message.text = contentStr;
        this.Delay(2.1f, () => { message.text = string.Empty; });
    }

    public void SetContentStrValue(string tipMessage)
    {
        ContentStr = tipMessage;
    }
}
