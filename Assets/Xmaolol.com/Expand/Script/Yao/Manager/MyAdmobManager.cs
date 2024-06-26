/*
using System;
using UnityEngine;
using QFramework;
using GoogleMobileAds.Api;

namespace xmaolol.com
{
    // Example script showing how to invoke the Google Mobile Ads Unity plugin.
    public class MyAdmobManager : MonoSingleton<MyAdmobManager>
    {
        private BannerView bannerView;
        private InterstitialAd interstitial;
        private RewardBasedVideoAd rewardBasedVideo;
        bool isVip = false;

        public bool IsVip
        {
            get
            {
                return isVip;
            }

            set
            {
                isVip = value;
            }
        }

        public void Start()
        {

#if UNITY_ANDROID
        string appId = "ca-app-pub-8118100316";
#elif UNITY_IPHONE
        string appId = "ca-app-pub99942544~1458002511";
#else
            string appId = "unexpectatform";
#endif

            MobileAds.SetiOSAppPauseOnBackground(true);

            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(appId);

            // Get singleton reward based video ad reference.
            this.rewardBasedVideo = RewardBasedVideoAd.Instance;

            // RewardBasedVideoAd is a singleton, so handlers should only be registered once.
            this.rewardBasedVideo.OnAdLoaded += this.HandleRewardBasedVideoLoaded;
            this.rewardBasedVideo.OnAdFailedToLoad += this.HandleRewardBasedVideoFailedToLoad;
            this.rewardBasedVideo.OnAdOpening += this.HandleRewardBasedVideoOpened;
            this.rewardBasedVideo.OnAdStarted += this.HandleRewardBasedVideoStarted;
            this.rewardBasedVideo.OnAdRewarded += this.HandleRewardBasedVideoRewarded;
            this.rewardBasedVideo.OnAdClosed += this.HandleRewardBasedVideoClosed;
            this.rewardBasedVideo.OnAdLeavingApplication += this.HandleRewardBasedVideoLeftApplication;

            if (IsVip == false)
            {
                LoadAllAdvertisement();
            }
        }

        private void LoadAllAdvertisement()
        {
            RequestBanner();
            RequestInterstitial();
            RequestRewardBasedVideo();
        }

        // Returns an ad request with custom ad targeting.
        private AdRequest CreateAdRequest()
        {
            return new AdRequest.Builder().Build();
        }

        public void DestroyBanner()
        {
            if (this.bannerView != null)
            {
                this.bannerView.Destroy();
            }
        }

        public void RequestBanner()
        {
            // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
            string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-8118569577100316/2724473751";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif

            // Clean up banner ad before creating a new one.
            if (this.bannerView != null)
            {
                this.bannerView.Destroy();
            }

            // Create a 320x50 banner at the top of the screen.
            this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

            // Register for ad events.
            this.bannerView.OnAdLoaded += this.HandleAdLoaded;
            this.bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
            this.bannerView.OnAdOpening += this.HandleAdOpened;
            this.bannerView.OnAdClosed += this.HandleAdClosed;
            this.bannerView.OnAdLeavingApplication += this.HandleAdLeftApplication;

            // Load a banner ad.
            this.bannerView.LoadAd(this.CreateAdRequest());
        }

        private void RequestInterstitial()
        {
            // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
            string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-8118569577100316/4973687983";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

            // Clean up interstitial ad before creating a new one.
            if (this.interstitial != null)
            {
                this.interstitial.Destroy();
            }

            // Create an interstitial.
            this.interstitial = new InterstitialAd(adUnitId);

            // Register for ad events.
            this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
            this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
            this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
            this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
            this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;

            // Load an interstitial ad.
            this.interstitial.LoadAd(this.CreateAdRequest());
        }

        private void RequestRewardBasedVideo()
        {
#if UNITY_EDITOR
            string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-8118569577100316/9842871286";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        string adUnitId = "unexpected_platform";
#endif

            this.rewardBasedVideo.LoadAd(this.CreateAdRequest(), adUnitId);
        }

        public void ShowInterstitial()
        {
            if (this.interstitial.IsLoaded())
            {
                this.interstitial.Show();
            }
            else
            {
                MonoBehaviour.print("Interstitial is not ready yet");
            }
        }

        public void ShowRewardBasedVideo()
        {
            if (this.rewardBasedVideo.IsLoaded())
            {
                this.rewardBasedVideo.Show();
            }
            else
            {
                //   this.ShowInterstitial();
                MonoBehaviour.print("Reward based video ad is not ready yet");
            }
        }

        #region Banner callback handlers

        public void HandleAdLoaded(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleAdLoaded event received");
        }

        public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            //   this.RequestBanner();
            MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
        }

        public void HandleAdOpened(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleAdOpened event received");
        }

        public void HandleAdClosed(object sender, EventArgs args)
        {
            // this.RequestBanner();
            MonoBehaviour.print("HandleAdClosed event received");
        }

        public void HandleAdLeftApplication(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleAdLeftApplication event received");
        }

        #endregion

        #region Interstitial callback handlers

        public void HandleInterstitialLoaded(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleInterstitialLoaded event received");
        }

        public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            //   RequestInterstitial();
            MonoBehaviour.print(
                "HandleInterstitialFailedToLoad event received with message: " + args.Message);
        }

        public void HandleInterstitialOpened(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleInterstitialOpened event received");
        }

        public void HandleInterstitialClosed(object sender, EventArgs args)
        {
            //  RequestInterstitial();
            MonoBehaviour.print("HandleInterstitialClosed event received");
        }

        public void HandleInterstitialLeftApplication(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleInterstitialLeftApplication event received");
        }

        #endregion

        #region RewardBasedVideo callback handlers

        public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
        }

        public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            //    this.RequestRewardBasedVideo();
            MonoBehaviour.print(
                "HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
        }

        public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
        }

        public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
        }

        public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
        {
            // this.RequestRewardBasedVideo();
            MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
        }

        public void HandleRewardBasedVideoRewarded(object sender, Reward args)
        {
            string type = args.Type;
            double amount = args.Amount;
            MonoBehaviour.print(
                "HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
        }

        public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
        }

        #endregion
    }
}
*/