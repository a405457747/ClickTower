using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using xmaolol.com;
//道具的静态属性
public class DemageManager : MonoSingleton<DemageManager>
{

    public float CritRate { get => critRate; set => critRate = value; }
    public float WhiteRate { get => whiteRate; set => whiteRate = value; }
    public float RedRate { get => redRate; set => redRate = value; }
    public float Yellow { get => yellow; set => yellow = value; }
    public float Blue { get => blue; set => blue = value; }
    public float PropRate
    {
        get
        {
            if (havePropFixed)
            {
                MySaveManager.Instance.HavePropDemageFixed(out int res, '0');
                return res;
            }
            else
            {
                return propRate;
            }
        }
        set => propRate = value;
    }

    public float MoraleRate
    {
        get
        {
            switch (SliderParent.Instance.CurrentMorale)
            {
                case Morale.White:
                    return WhiteRate;
                case Morale.Red:
                    return RedRate;
                case Morale.yellow:
                    return Yellow;
                case Morale.Blue:
                    return Blue;
            }
            return MoraleRate;
        }
        set => moraleRate = value;
    }

    //暴击修正倍率
    private float critRate = 2.5f;
    float whiteRate = 0.5f;
    float redRate = 1.1f;
    float yellow = 1.5f;
    float blue = 2.5f;
    //道具伤害修正
    private float propRate = 1f;
    //是否有伤害修正啊
    bool havePropFixed;
    //士气伤害修正
    private float moraleRate = 1f;

    private void Start()
    {
        UpdateHaveBoolFixed();
    }

    public void UpdateHaveBoolFixed()
    {
        int temp = 0;
        havePropFixed = MySaveManager.Instance.HavePropDemageFixed(out temp, '0');
    }

    public void GetFixedDamage(int originalDemage, bool isCrit, out int fixedDamage, float towerDemageRate = 1)
    {
        if (isCrit)
        {
            fixedDamage = (int)(originalDemage * towerDemageRate * PropRate * MoraleRate * CritRate);
        }
        else
        {
            fixedDamage = (int)(originalDemage * towerDemageRate * PropRate * MoraleRate);
        }
    }
}
