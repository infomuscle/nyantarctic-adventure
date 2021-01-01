using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobManager : MonoBehaviour {
    public static AdmobManager instance;

    private BannerView bannerView;

    private RewardedAd rewardedAd;

    private string adUnitId;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Debug.LogWarning("Multiple AdmobManagers on Scene");
            Destroy(gameObject);
        }
    }

    void Start() {
        MobileAds.Initialize(initStatus => { });

        RequestBanner();
    }

    private void RequestBanner() {
        #if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/6300978111";
        #elif UNITY_IPHONE
        adUnitId = "ca-app-pub-3940256099942544/2934735716";
        #else
        adUnitId = "unexpected_platform";
        #endif

        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }

    public void RequestReward() {
        #if UNITY_ANDROID
            adUnitId = "ca-app-pub-3940256099942544/5224354917";
        #elif UNITY_IPHONE
        adUnitId = "ca-app-pub-3940256099942544/1712485313";
        #else
            adUnitId = "unexpected_platform";
        #endif

        Debug.Log("RequestReward!");

        rewardedAd = new RewardedAd(adUnitId);
        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);
        rewardedAd.Show();
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args) {
        print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args) {
        print(
            "HandleRewardedAdFailedToLoad event received with message: "
            + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args) {
        print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args) {
        string type = args.Type;
        double amount = args.Amount;
        print(
            "HandleRewardedAdRewarded event received for "
            + amount.ToString() + " " + type);
    }
}