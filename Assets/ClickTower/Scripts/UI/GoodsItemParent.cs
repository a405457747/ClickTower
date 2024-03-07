using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using xmaolol.com;
using UnityEngine.UI;
using System;
public class GoodsItemParent : MonoSingleton<GoodsItemParent>
{
    public GameObject GoodsItem;
    public Sprite[] icons;
    public string[] names;
    public List<string> GoodsList;
    public List<GoodsItem> GoodsItemList;
    public List<string> DetailStrList;
    public Text Text_Num_detail;
    public Text Text_Detail;

    //Ĭ��detailIndex��0��
    private int detailIndex;
    private int detailIndexRandomValue;
    private int maxEquipGoods;
    private int currentEquipGoods = 0;

    public int MaxEquipGoods { get => maxEquipGoods; set => maxEquipGoods = value; }
    public int CurrentEquipGoods
    {
        get => currentEquipGoods; set
        {
            currentEquipGoods = value;
            UpdateText_Num_detail(value, MaxEquipGoods);
        }
    }
    public int DetailIndexRandomValue { get { return MySaveManager.Instance.GetGoodsListRandomValue(DetailIndex); } set => detailIndexRandomValue = value; }
    public int DetailIndex
    {
        get => detailIndex; set
        {
            detailIndex = value;
            UpdateDetailText();
        }
    }

    public void UpdateText_Num_detail(int current, int max)
    {
        Text_Num_detail.text = $@"�Ѿ�װ������Ŀ:{current}                ���װ����:{max}";
    }

    //�������е�logo
    public void UpdateAllLogoChangeBig(int currentIndex)
    {
        foreach (GoodsItem goodsItem in GoodsItemList)
        {
            goodsItem.ImageIsSelect = false;
        }
        GoodsItemList[currentIndex].ImageIsSelect = true;
    }

    public void DestoryAllChildren()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        GoodsItemList.Clear();
        currentEquipGoods = 0;
    }

    //���˵�ʱ����һ���Ѿ�װ���ı��û��װ����
    public void ChangeEquipment(int index)
    {
        GoodsItemList[index].IsEquip = true;
    }

    //д��װ��������
    public void WriteData()
    {
        List<string> list = new List<string>();
        foreach (GoodsItem goodsItem in GoodsItemList)
        {
            string k;
            if (goodsItem.IsEquip == true)
            {
                k = $"{1}g{goodsItem.RandomValue}{goodsItem.Num}";
            }
            else
            {
                k = $"{0}g{goodsItem.RandomValue}{goodsItem.Num}";
            }
            list.Add(k);
        }
        MySaveManager.Instance.SaveMapping.GoodsList = list;
        MySaveManager.Instance.Save();
    }

    public void UpdateDetailText()
    {
        Text_Detail.text = GetDetailTextStr(DetailIndex);
    }

    public string GetDetailTextStr(int DetailIndex)
    {
        int num = MySaveManager.Instance.GetGoodsListOneNum(DetailIndex);
        switch (num)
        {
            case 0:
                return $"����{DetailIndexRandomValue}���˺�";
            case 1:
                return $"��סһ��������ʱ�����{DetailIndexRandomValue}��";
            case 2:
                return $"����{DetailIndexRandomValue}��ʿ��������";
            case 3:
                return $"����{DetailIndexRandomValue}���������ֵ";
            case 4:
                return $"�������ü���{DetailIndexRandomValue}0%";
            case 5:
                return $"����{DetailIndexRandomValue}0%��������";
            case 6:
                return $"������{DetailIndexRandomValue}0%����ʱ����";
        }
        return null;
    }

    private void Awake()
    {
        maxEquipGoods = (MySaveManager.Instance.SaveMapping.CurrentGameLevel / 5) + 1;
    }

    public void CreateObj()
    {
        GoodsList = MySaveManager.Instance.SaveMapping.GoodsList;
        GetComponent<RectTransform>().offsetMin = new Vector2(GetComponent<RectTransform>().offsetMin.x, -100 * GoodsList.Count);
        for (int i = 0; i < GoodsList.Count; i++)
        {
            string k = GoodsList[i];
            char boolchar = k[0];
            char Num = k[k.Length - 1];
            int RandomValue = int.Parse(k[2].ToString());
            bool isEquip = false;
            if (boolchar == '0')
            {
                isEquip = false;
            }
            else
            {
                isEquip = true;
                currentEquipGoods++;
            }
            int num = Convert.ToInt32(Num.ToString());
            GameObject obj = GameObject.Instantiate(GoodsItem, transform.position, Quaternion.identity);
            obj.name = $"GoodsItem{i}";
            obj.GetComponent<RectTransform>().SetSizeWidth(779.6f);
            obj.transform.SetParent(this.transform, false);
            GoodsItem goodsItem = obj.GetComponent<GoodsItem>();
            goodsItem.Init(DetailStrList[num], names[num], icons[num], isEquip, num, RandomValue, i);
            GoodsItemList.Add(goodsItem);
        }
        GoodsItem first = GoodsItemList[0];
        DetailIndex = 0;
        UpdateAllLogoChangeBig(0);
        UpdateText_Num_detail(CurrentEquipGoods, MaxEquipGoods);
    }
}
