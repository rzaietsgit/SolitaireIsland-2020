using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ITSoft {
    public class AdsManager : MonoBehaviour
    {
        public static Action OnCompleteRewardVideo;
        public static Action OnCompleteInterVideo;
        public static System.Action OnRewardedAdSkiped;
        public static System.Action OnRewardedAdFailed;

        private static bool removeAds = false;
        
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            removeAds = bool.Parse(PlayerPrefs.GetString("addfreechk", "false"));

            InitIronSDK();
            LoadInterstitial();
        }

        private void OnEnable()
        {
            IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += ErrorShowingReward;
            IronSourceEvents.onRewardedVideoAdClosedEvent += CloseShowingReward;
            IronSourceEvents.onInterstitialAdClosedEvent += LoadInterstitial;
            IronSourceEvents.onInterstitialAdClosedEvent += InterVideoAdRewardedEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent += LoadInterstitial;
        }

        private void OnDisable()
        {
            IronSourceEvents.onRewardedVideoAdRewardedEvent -= RewardedVideoAdRewardedEvent;
            IronSourceEvents.onRewardedVideoAdShowFailedEvent -= ErrorShowingReward;
            IronSourceEvents.onRewardedVideoAdClosedEvent += CloseShowingReward;
            IronSourceEvents.onInterstitialAdClosedEvent -= LoadInterstitial;
            IronSourceEvents.onInterstitialAdClosedEvent -= InterVideoAdRewardedEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent -= LoadInterstitial;
        }
        
        private void CloseShowingReward()
        {
            OnRewardedAdFailed?.Invoke();
        }

        private void InitIronSDK()
        {
#if UNITY_ANDROID
            // string appKey = "85460dcd";
            string appKey = "10b87950d";
#elif UNITY_IPHONE
        string appKey = "8545d445";
#else
        string appKey = "unexpected_platform";
#endif
            IronSource.Agent.validateIntegration();
            IronSource.Agent.init(appKey);
        }

        void RewardedVideoAdRewardedEvent(IronSourcePlacement ssp)
        {
            Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent, amount = " + ssp.getRewardAmount() + " name = " + ssp.getRewardName());
            OnCompleteRewardVideo?.Invoke();
        }

        void InterVideoAdRewardedEvent()
        {
            OnCompleteInterVideo?.Invoke();
            OnCompleteInterVideo = null;
        }

        public static void RemoveAds()
        {
            removeAds = true;
        }
        
        public static bool RewardIsReady()
        {
            Debug.Log("Show reward video. Reward video is" + (IronSource.Agent.isRewardedVideoAvailable() ? "" : " not") + " ready");
#if UNITY_EDITOR
            return true;
#endif
            return IronSource.Agent.isRewardedVideoAvailable();
        }

        public static bool InterIsReady() => IronSource.Agent.isInterstitialReady();
        
        public static void ShowRewarded()
        {
            if (removeAds)
            {
                OnCompleteRewardVideo?.Invoke();
                return;
            }
            if (RewardIsReady())
            {
#if UNITY_EDITOR
                OnCompleteRewardVideo?.Invoke();
#endif
                IronSource.Agent.showRewardedVideo();
            }
            else
            {
                Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - False");
                #if UNITY_EDITOR
                OnCompleteRewardVideo?.Invoke();
                #endif
            }
        }

        private void ErrorShowingReward(IronSourceError error)
        {
            Debug.Log("Error to show reward! " + error.ToString());
        }
        
        public static void LoadInterstitial()
        {
            // if (BizzyBeeGames.IAPManager.Instance.IsProductPurchased("removeads"))
            //     return;
            Debug.Log("unity-script: IronSource.Agent.loadInterstitial - True");
            IronSource.Agent.loadInterstitial();
        }    
        
        public static void LoadInterstitial(IronSourceError error)
        {
            Debug.Log("unity-script: IronSource.Agent.loadInterstitial - " + error.getDescription());
            IronSource.Agent.loadInterstitial();
        }

        public static void ShowInterstitial(System.Action ViewComplete = null)
        {
            // if (BizzyBeeGames.IAPManager.Instance.IsProductPurchased("removeads"))
            //     return;
            if (removeAds)
            {
                ViewComplete?.Invoke();
                return;
            }
            if (InterIsReady())
            {
                Debug.Log("unity-script: IronSource.Agent.isInterstitialReady - True");
                OnCompleteInterVideo = ViewComplete;
                IronSource.Agent.showInterstitial();
            }
            else
            {
                Debug.Log("unity-script: IronSource.Agent.isInterstitialReady - False");
                LoadInterstitial();
                ViewComplete?.Invoke();
            }
        }

        public static void ShowInterstitial(string placementName)
        {
            // if (BizzyBeeGames.IAPManager.Instance.IsProductPurchased("removeads"))
            //     return;

            ShowInterstitial();
            return;

            IronSource.Agent.showInterstitial(placementName);
        }

        public static void ShowBanner(IronSourceBannerPosition bannerPosition = IronSourceBannerPosition.BOTTOM)
        {
            // if (BizzyBeeGames.IAPManager.Instance.IsProductPurchased("removeads"))
            //     return;
            if (removeAds)
            {
                return;
            }
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, bannerPosition);
        }

        public static void HideBanner()
        {
            IronSource.Agent.destroyBanner();
        }
    }
}
