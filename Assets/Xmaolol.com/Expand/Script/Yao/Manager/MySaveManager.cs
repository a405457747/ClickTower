
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;
using QFramework;
using QFramework.Example;

namespace xmaolol.com
{
    //数据保存和加载配置表的类
    public class MySaveManager : MonoSingleton<MySaveManager>
    {
        [HideInInspector]
        public SaveMapping SaveMapping;
        private Dictionary<string, Dictionary<string, string>> languageConfigDic;
        private int watchADSaddPPValue = 3;
        //每天登陆加多少体力值呢 数据没对接改了要改的
        [HideInInspector]
        public int playEveryDayaddPPValue = 5;
        //最大体力值
        private int maxPPValue = 20;
        private Action watchOverCallback=null;

        protected void Awake()
        {
            Load();
            // print(SaveMapping.CurrentGameLevel);
            //  LoadConfiguration();
            // print(SaveMapping.GoodsList[0]);
        }

        //加载配置表
        public void LoadConfiguration()
        {
            MyExcelData.LoadExcelFormCSV("languageCfg", out languageConfigDic);
        }

        //加载数据 
        public void Load()
        {
            string path = Application.persistentDataPath + "/saveData.json";
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    SaveMapping = JsonUtility.FromJson<SaveMapping>(reader.ReadToEnd());
                }
            }
            else
            {
                TextAsset txt = Resources.Load<TextAsset>("Text/saveData");
                SaveMapping = JsonUtility.FromJson<SaveMapping>(txt.text);
                using (var s = File.Create(path))
                {
                }
                Save();
            }
        }

        //储存数据呢
        public void Save()
        {
            string json = JsonUtility.ToJson(SaveMapping);
            string savePath = Application.persistentDataPath + "/saveData.json";
            File.WriteAllText(savePath, json, Encoding.UTF8);
        }

        public bool CanPlayGame()
        {
            if (SaveMapping.IsVip)
            {
                return true;
            }
            else
            {
                if (SaveMapping.PhysicalPower > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //成为vip的方法
        public void BecomeVipAndSave()
        {
            SaveMapping.IsVip = true;
            Save();
        }

        public void RecordNowTimestampAndSave()
        {
            SaveMapping.TimestampMinutes = MyTool.GetUnixStartToNowTimeTotalMinutes();
            Save();
        }

        public void AddPhysicalPower(int howManyPhysicalPower)
        {
            SaveMapping.PhysicalPower += howManyPhysicalPower;
            if (SaveMapping.PhysicalPower >= maxPPValue)
            {
                SaveMapping.PhysicalPower = maxPPValue;
            }
        }

        public void ReducePhysicalPowerAndSave()
        {
            if (SaveMapping.IsVip == false)
            {
                SaveMapping.PhysicalPower -= 1;
                Save();
            }
        }

        public int GetGoodsListOneNum(int detailIndex)
        {
            for (int i = 0; i < SaveMapping.GoodsList.Count; i++)
            {
                if (detailIndex == i)
                {
                    string item = SaveMapping.GoodsList[i];
                    return int.Parse(item[item.Length - 1].ToString());

                }
            }
            return -1;
        }
        public int GetGoodsListRandomValue(int DetailIndex)
        {
            for (int i = 0; i < SaveMapping.GoodsList.Count; i++)
            {
                if (DetailIndex == i)
                {
                    return int.Parse(SaveMapping.GoodsList[i][2].ToString());

                }
            }
            return -1;
        }

        public bool HavePropDemageFixed(out int maxRandomValue, char goodsIndex)
        {
            bool Res = false;

            maxRandomValue = 0;
            foreach (string item in SaveMapping.GoodsList)
            {
                if (item[0] == '1' && item[item.Length - 1] == goodsIndex)
                {
                    int k = int.Parse(item[2].ToString());
                    maxRandomValue = Mathf.Max(k, maxRandomValue);
                    Res = true;
                }
            }
            return Res;
        }

        public string ReturnLanguageMessage(int id)
        {
            string message = "";
            switch (SaveMapping.LanguageKey)
            {
                case 0:
                    message = languageConfigDic["ChineseText"][id.ToString()];
                    break;
                case 1:
                    message = languageConfigDic["EnglishText"][id.ToString()];
                    break;
                case 2:
                    message = languageConfigDic["TradChinese"][id.ToString()];
                    break;
            }
            return message;
        }

        public void GetPhsicPower()
        {
            #region 这也是回调函数
            //增加体力保存
            AddPhysicalPower(watchADSaddPPValue);
            Save();
            if (watchOverCallback != null) watchOverCallback();
            #endregion
        }

        //真正的播放广告
        public void PlayAdVideo(Action watchOverCallback)
        {
            this.watchOverCallback = null;
            this.watchOverCallback = watchOverCallback;
            Debug.Log("播放广告");
            MyYomobManager.Instance.PlayAwardVedioAD();
        }

        public void  BecameVip()
        {
            if (SaveMapping.CurrentGameLevel == Consts.MaxGameLevel)
            {
                BecomeVipAndSave();
            }
        }
    }
}
