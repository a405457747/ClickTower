using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using DG.Tweening;

namespace xmaolol.com
{
    //游戏和经常使用的DoTween动画管理和时间管理类
    public class MyGameManager : MonoSingleton<MyGameManager>
    {
        #region StartTime()函数相关
        public int TotalTime;
        public Text TimeText;
        private int mumite;
        private int second;
        #endregion

        #region Mono函数
        #endregion

        #region 私有函数
        #endregion

        #region 游戏状态
        public void GameStart()
        {

        }

        public void Pause()
        {

        }

        public void Resume()
        {

        }

        public void GameOver()
        {

        }
        #endregion

        #region 公共函数
        public void OpenScalePanel(RectTransform rect)
        {
            rect.gameObject.SetActive(true);
            rect.DOScale(1, Consts.PanelProcessTime).OnComplete(() =>
            {
                /*
                switch (rect.gameObject.name)
                {
                    case "Panel_Pause":
                        break;
                }
                */
            }).SetDelay(Consts.PanelDelayTime);
        }

        public void CloseScalePanel(RectTransform rect)
        {
            rect.DOScale(0, Consts.PanelProcessTime).OnComplete(() =>
            {
                rect.gameObject.SetActive(false);
            }).SetDelay(Consts.PanelDelayTime);
        }

        public IEnumerator StartTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);//由于开始倒计时，需要经过一秒才开始减去1秒，
                TotalTime++;
                TimeText.text = "Time:" + TotalTime;
                mumite = TotalTime / 60; //输出显示分
                second = TotalTime % 60; //输出显示秒
                if (second >= 10)
                {
                    TimeText.text = "0" + mumite + ":" + second;
                }     //如果秒大于10的时候，就输出格式为 00：00
                else
                    TimeText.text = "0" + mumite + ":0" + second;      //如果秒小于10的时候，就输出格式为 00：00
            }
        }
        #endregion
    }
}
