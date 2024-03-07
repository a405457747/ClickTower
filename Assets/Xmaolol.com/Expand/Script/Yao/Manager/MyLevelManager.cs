
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using QFramework;

namespace xmaolol.com
{
    public class MyLevelManager : MonoSingleton<MyLevelManager>
    {

        public Dictionary<string, Dictionary<string, string>> levelConfigDic;

        //当前关卡的 ID
        int currentLevelID;
        //进度关卡的ID
        int levelProgress;

        public int CurrentLevelID
        {
            get
            {
                return currentLevelID;
            }

            set
            {
                if (value > Consts.MaxGameLevel)
                {
                    value = Consts.MaxGameLevel;
                }
                currentLevelID = value;
                LoadNext();
            }
        }

        public int LevelProgress
        {
            get
            {
                levelProgress = MySaveManager.Instance.SaveMapping.CurrentGameLevel;
                return levelProgress;
            }

            set
            {
                if (value > Consts.MaxGameLevel)
                {
                    value = Consts.MaxGameLevel + 1;
                }
                levelProgress = value;
            }
        }

        void Awake()
        {
            LoadConfiguration();
        }

        public void LoadConfiguration()
        {
            MyExcelData.LoadExcelFormCSV("LevelCfg", out levelConfigDic);
        }

        public void LoadNext()
        {
            string levelName = levelConfigDic["LevelSceneName"][CurrentLevelID.ToString()];
            SceneManager.LoadScene(levelName);
        }
    }
}
