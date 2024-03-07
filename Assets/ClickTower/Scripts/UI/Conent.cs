using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using xmaolol.com;

public class Conent : MonoBehaviour
{
    private int levelNum;

    public GameObject Image_item;
    public int LevelNum { get => Consts.MaxGameLevel; set => levelNum = value; }
    public Text AddphysicalPowerText;
    public Text physicalPowerText;
    public Button RefuelButton;

    private void Start()
    {
        for (int i = 0; i <= LevelNum; i++)
        {
            GameObject imageLevl = GameObject.Instantiate(Image_item);
            imageLevl.name = $"ImageItem{i}";
            imageLevl.transform.SetParent(this.transform, false);
            // imageLevl.GetComponent<Image_item>().NameIndex = i;
        }

        UpdateSomeState();
    }

    private void UpdateSomeState()
    {
        UpdateButtonState();
        UptatePhysicalPower();
        if (MySaveManager.Instance.CanPlayGame())
        {
            AddphysicalPowerText.gameObject.SetActive(false);
        }
        else
        {
            AddphysicalPowerText.gameObject.SetActive(true);
        }
    }

    private void UpdateButtonState()
    {
        for (int i = 0; i <= transform.childCount - 1; i++)
        {
            Image_item imageItem = transform.GetChild(i).GetComponent<Image_item>();
            imageItem.GetComponent<Image_item>().NameIndex = i;
        }
    }

    private void UptatePhysicalPower()
    {
        physicalPowerText.text = $"体力值:{MySaveManager.Instance.SaveMapping.PhysicalPower}";
    }

    public void PlayVedio()
    {
        MySaveManager.Instance.PlayAdVideo(() =>
        {
            //更新状态啊
            UpdateSomeState();
        });
    }
}
