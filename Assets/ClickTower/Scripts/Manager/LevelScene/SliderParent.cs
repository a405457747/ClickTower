using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using xmaolol.com;
using System.Linq;
using System;
using HedgehogTeam.EasyTouch;

public class SliderParent : MonoSingleton<SliderParent>
{
    List<Slider> sliders = new List<Slider>();
    //最大士气槽level
    private int maxMoraleLevel = 2;
    private float pressSecond = 1.35f;
    //每秒减少多少士气值
    private float perMoraleReduce = 0.015f;
    [SerializeField]
    private float currentMoraleNum;

    public float PerMoraleReduce { get => perMoraleReduce; set => perMoraleReduce = value; }
    public Morale CurrentMorale;

    public float PressSecond
    {
        get
        {
            int randomValue = 0;
            bool haveFixed = MySaveManager.Instance.HavePropDemageFixed(out randomValue, '1');
            if (haveFixed)
            {
                return pressSecond / randomValue;
            }
            else
            {
                return pressSecond;
            }
        }
        set => pressSecond = value;
    }
    public int MaxMoraleLevel
    {
        get
        {
            int randomValue = 0;
            bool haveFixed = MySaveManager.Instance.HavePropDemageFixed(out randomValue, '2');
            if (haveFixed)
            {
                return maxMoraleLevel + randomValue;
            }
            else
            {
                return maxMoraleLevel;
            }
        }
        set => maxMoraleLevel = value;
    }
    public float CurrentMoraleNum
    {
        get
        {
            if (currentMoraleNum <= 0)
            {
                return 0f;
            }
            else
            {
                return currentMoraleNum;
            }
        }
        set
        {
            currentMoraleNum = value;
            if (value <= 0)
            {
                value = 0;
            }
            if (value >= MaxMoraleLevel)
            {
                value = MaxMoraleLevel;
            }
            UpdateSlidersValue();
            int moraleRangeIndex = GetMoraleSection(value);
            switch (moraleRangeIndex)
            {
                case 1:
                    CurrentMorale = Morale.White;
                    break;
                case 2:
                    CurrentMorale = Morale.Red;
                    break;
                case 3:
                    CurrentMorale = Morale.yellow;
                    break;
                case 4:
                    CurrentMorale = Morale.Blue;
                    break;
            }
        }
    }

    private void Awake()
    {
        sliders = this.GetComponentsInChildren<Slider>().ToList();
    }

    private void Start()
    {
        CurrentMoraleNum = MaxMoraleLevel;
        //显示有多少个slider要显示
        ShowSlider();
        if (!IsInvoking("ReduceTime"))
        {
            InvokeRepeating("ReduceTime", 0f, 1f);
        }
    }

    private void ShowSlider()
    {
        for (int i = 0; i < sliders.Count; i++)
        {
            if (i <= (MaxMoraleLevel - 1))
            {
                sliders[i].gameObject.SetActive(true);
            }
            else
            {
                sliders[i].gameObject.SetActive(false);
            }
        }
    }

    private void ReduceTime()
    {
        CurrentMoraleNum -= PerMoraleReduce;
    }

    public void ResumeMorale()
    {
        CurrentMoraleNum = MaxMoraleLevel;
    }

    private void UpdateSlidersValue()
    {
        for (int i = 0; i < sliders.Count; i++)
        {
            Slider slider = sliders[i];
            //表示有几个要更新其他为0；
            int num = GetMoraleSection(CurrentMoraleNum);
            if (i + 1 < num)
            {
                slider.value = 1f;
            }
            else if (i + 1 == num)
            {
                slider.value = CurrentMoraleNum - i;
            }
            else
            {
                slider.value = 0f;
            }
        }
    }

    private int GetMoraleSection(float moraleValue)
    {
        if (moraleValue >= 0 && moraleValue <= 1)
        {
            return 1;
        }
        if (moraleValue > 1 && moraleValue <= 2)
        {
            return 2;
        }
        if (moraleValue > 2 && moraleValue <= 3)
        {
            return 3;
        }
        if (moraleValue > 3 && moraleValue <= 4)
        {
            return 4;
        }
        return -1;
    }

    public void TimePressed(Gesture gesture)
    {
        if (gesture.actionTime > PressSecond)
        {
            ResumeMorale();
        }
    }
}
