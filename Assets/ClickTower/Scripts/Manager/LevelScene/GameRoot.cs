using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using xmaolol.com;
using UnityEngine.SceneManagement;

public class GameRoot : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += LoadSceneCallback;
        IsGivenUserPPValue();
        MySaveManager.Instance.BecameVip();
    }

    private void LoadSceneCallback(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name == "Main")
        {
            SceneManager.sceneLoaded -= LoadSceneCallback;
            GameObject.Find("ComLogoPanel").SetActive(false);
            Destroy(this.gameObject);
        }
    }



    //要给用户体力值吗
    private void IsGivenUserPPValue()
    {
        if (MySaveManager.Instance.SaveMapping.TimestampMinutes == 0)
        {
            MySaveManager.Instance.SaveMapping.TimestampMinutes = MyTool.GetUnixStartToNowTimeTotalMinutes();
            MySaveManager.Instance.AddPhysicalPower(MySaveManager.Instance.playEveryDayaddPPValue);
            MySaveManager.Instance.Save();
        }
        else
        {
            //一天1440分钟呢
            /*if (MyTool.GetUnixStartToNowTimeTotalMinutes() - MySaveManager.Instance.SaveMapping.TimestampMinutes >= 1440)
            {
                MySaveManager.Instance.PlayAdVideo(()=>{});
                MySaveManager.Instance.AddPhysicalPower(MySaveManager.Instance.playEveryDayaddPPValue);
                MySaveManager.Instance.SaveMapping.TimestampMinutes = MyTool.GetUnixStartToNowTimeTotalMinutes();
                MySaveManager.Instance.Save();
            }*/
        }
    }
}
