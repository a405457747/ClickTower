
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using Together;
using UnityEngine.UI;
using xmaolol.com;

public class MyYomobManager : MonoSingleton<MyYomobManager>
{
    private readonly string AppID = "9115";
    //看广告恢复体力
    private readonly string AwardVedioSceneID = "pasX0IM";
    private readonly string TableSceneID = "LNzfPI1E";
    private readonly string CanCloseVedioSceneID = "y1wodIzin";
    private readonly string BannerSceneID = "ZEPta";


    public Text logField;
    void Awake()
    {
        TGSDK.SetDebugModel(false);
        TGSDK.SDKInitFinishedCallback = (string msg) =>
        {
            TGSDK.TagPayingUser(TGPayingUser.TGMediumPaymentUser, "CNY", 0, 0);
            //Log("TGSDK finished : " + msg);
            Debug.Log("TGSDK GetUserGDPRConsentStatus = " + TGSDK.GetUserGDPRConsentStatus());
            TGSDK.SetUserGDPRConsentStatus("yes");
            Debug.Log("TGSDK GetIsAgeRestrictedUser = " + TGSDK.GetIsAgeRestrictedUser());
            TGSDK.SetIsAgeRestrictedUser("no");
            float bannerHeight = (float)(Screen.height) * 0.123f;
            TGSDK.SetBannerConfig(BannerSceneID, "TGBannerNormal", 0, Display.main.systemHeight - bannerHeight, Display.main.systemWidth, bannerHeight, 8);
            PreloadAd();
        };

#if !UNITY_EDITOR && UNITY_ANDROID
        TGSDK.Initialize(AppID);
#endif
        //初始化
    }

    public void Log(string message)
    {
        Debug.Log("[TGSDK-Unity]  " + message);
        if (logField != null)
        {
            logField.text = message;
        }
    }

    public void PreloadAd()
    {
        TGSDK.PreloadAdSuccessCallback = (string msg) =>
        {
            // Log("PreloadAdSuccessCallback : " + msg);
        };
        TGSDK.PreloadAdFailedCallback = (string msg) =>
        {
            Log("网络似乎开了小差，请确保开启了网络链接或者权限用于加载视频广告");
        };
        TGSDK.InterstitialLoadedCallback = (string msg) =>
        {
              //Log("InterstitialLoadedCallback : " + msg);
        };
        TGSDK.InterstitialVideoLoadedCallback = (string msg) =>
        {
            //  Log("InterstitialVideoLoadedCallback : " + msg);
        };
        TGSDK.AwardVideoLoadedCallback = (string msg) =>
        {
            //Log("AwardVideoLoadedCallback : " + msg);
        };
        TGSDK.AdShowSuccessCallback = (string scene, string msg) =>
        {
            //  Log("AdShow : " + scene + " SuccessCallback : " + msg);
        };
        TGSDK.AdShowFailedCallback = (string scene, string msg, string err) =>
        {
               // Log("AdShow : " + scene + " FailedCallback : " + msg + ", " + err);
        };
        TGSDK.AdCloseCallback = (string scene, string msg, bool award) =>
        {
            //  Log("AdClose : " + scene + " Callback : " + msg + " Award : " + award);
            if (award)
            {
                MySaveManager.Instance.GetPhsicPower();
            }
        };
        TGSDK.AdClickCallback = (string scene, string msg) =>
        {
            //  Log("AdClick : " + scene + " Callback : " + msg);
        };
        TGSDK.PreloadAd();
    }//预先加载

    //播放场景1的广告
    public void PlayAwardVedioAD()
    {
        if (TGSDK.CouldShowAd(AwardVedioSceneID))
        {
            TGSDK.ShowAd(AwardVedioSceneID);

        }
        else
        {
            //  Log("Scene " + AwardVedioSceneID + " could not to show");
        }
    }

    private bool isTableAd;

    public void PlayBannerAD()
    {

        if (TGSDK.CouldShowAd(BannerSceneID))
        {
            TGSDK.ShowAd(BannerSceneID);
        }
        else
        {
           // Log("BannerScene " + BannerSceneID + " could not to show");
        }
    }

    public void CloseBannerAD()
    {
        TGSDK.CloseBanner(BannerSceneID);
    }


    public void PlayTableADOrCanCloseVedioAD()
    {
        if (MyTool.CanBirth(0.5f))
        {
            isTableAd = true;
        }
        else
        {
            isTableAd = false;
        }

        if (isTableAd)
        {
            if (TGSDK.CouldShowAd(TableSceneID))
            {
                TGSDK.ShowAd(TableSceneID);
            }
            else
            {
            }
        }
        else
        {
            if (TGSDK.CouldShowAd(CanCloseVedioSceneID))
            {
                TGSDK.ShowAd(CanCloseVedioSceneID);
            }
            else
            {
            }
        }
    }
}
