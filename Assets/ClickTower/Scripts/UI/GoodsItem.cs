using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using xmaolol.com;
using System;

public class GoodsItem : MonoBehaviour
{
    public int RandomValue;
    public string Detail;
    public Button btn;
    public Button btn_unload;
    public string name;
    public Image logoImage;
    public Button imageBtn;
    public Sprite logoSprite;
    public Text nameText;
    //它自己的下标啊
    public int NumIndex;
    //它自己的编号呢
    public int Num;
    public Text txt_equip;
    public delegate void select();
    public event select SelectStart;

    //是否装备
    [SerializeField]
    private bool isEquip;
    private bool imageIsSelect;

    public bool IsEquip
    {
        get => isEquip; set
        {
            isEquip = value;
            UpdateEquipBtn();
        }
    }
    public bool ImageIsSelect
    {
        get => NumIndex == GoodsItemParent.Instance.DetailIndex; set
        {
            imageIsSelect = value;
            if (value == true)
            {
                logoImage.LocalScale(Vector3.one * 1.2f);
            }
            else
            {
                logoImage.LocalScale(Vector3.one * 1f);
            }
        }
    }

    private void Awake()
    {
        txt_equip = transform.Find("txt_equip").GetComponent<Text>();
        nameText = transform.Find("Text").GetComponent<Text>();
        logoImage = transform.Find("Image").GetComponent<Image>();
        imageBtn = transform.Find("Image").GetComponent<Button>();
        btn = transform.Find("Button").GetComponent<Button>();
        btn_unload = transform.Find("btn_unload").GetComponent<Button>();
        btn.onClick.AddListener(ClickBtn);
        imageBtn.onClick.AddListener(ClickImageBtn);
        btn_unload.onClick.AddListener(UnloadEquip);
    }

    private void UnloadEquip()
    {
        if (IsEquip == true)
        {
            GoodsItemParent goodsItemParent = GoodsItemParent.Instance;
            if (goodsItemParent.CurrentEquipGoods > 0)
            {
                goodsItemParent.CurrentEquipGoods--;
                IsEquip = false;
            }
            goodsItemParent.WriteData();
        }
        ClickImageBtn();
        MyAudioManager.GetInstance().PlaySound(Consts.backEffect);
    }

    private void ClickImageBtn()
    {
        int index = MyTool.GetNumberByString(this.gameObject.name);
        GoodsItemParent.Instance.DetailIndex = NumIndex;
        ImageIsSelect = true;
        GoodsItemParent.Instance.UpdateAllLogoChangeBig(index);
    }

    public void Init(string detail, string name, Sprite logo, bool isEquip, int num, int randomValue, int numIndex)
    {
        this.Detail = detail;
        this.name = name;
        this.logoSprite = logo;
        this.IsEquip = isEquip;
        this.Num = num;
        this.RandomValue = randomValue;
        this.NumIndex = numIndex;
        UpdateUI();
    }

    public void UpdateUI()
    {
        logoImage.sprite = logoSprite;
        nameText.text = name;
    }

    public void UpdateEquipBtn()
    {
        if (IsEquip)
        {
            btn.gameObject.SetActive(false);
            txt_equip.gameObject.SetActive(true);
            btn_unload.gameObject.SetActive(true);
        }
        else
        {
            btn.gameObject.SetActive(true);
            txt_equip.gameObject.SetActive(false);
            btn_unload.gameObject.SetActive(false);
        }
    }

    public void ClickBtn()
    {
        if (IsEquip == false)
        {
            GoodsItemParent goodsItemParent = GoodsItemParent.Instance;
            if (goodsItemParent.CurrentEquipGoods < goodsItemParent.MaxEquipGoods)
            {
                goodsItemParent.CurrentEquipGoods++;
                IsEquip = true;
            }
            goodsItemParent.WriteData();
        }
        ClickImageBtn();
        MyAudioManager.GetInstance().PlaySound(Consts.enterEffect);
    }

    private void OnDestroy()
    {
        btn.onClick.RemoveListener(ClickBtn);
        imageBtn.onClick.RemoveListener(ClickImageBtn);
        btn_unload.onClick.RemoveListener(UnloadEquip);
    }
}
