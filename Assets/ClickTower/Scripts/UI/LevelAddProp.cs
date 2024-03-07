using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelAddProp : MonoBehaviour
{
    Button addBtn;

    private void Awake()
    {
        addBtn = transform.Find("PropItem/Button_add").GetComponent<Button>();
    }

    public void UpdateAddBtn(bool isShow)
    {
        if (isShow)
        {
            addBtn.enabled = true;
        }else
        {
            addBtn.enabled = false;
        }
    }
}
