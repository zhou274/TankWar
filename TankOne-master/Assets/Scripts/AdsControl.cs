using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Events;
public class AdsControl : MonoBehaviour
{

    private bool isTest = false;

    private string TEST_APPID = "ca-app-pub-3940256099942544~3347511713";
    private string TEST_INTERS_ID = "";
    private string TEST_REWARD_ID = "ca-app-pub-3940256099942544/5224354917";
    private string APP_ID = "REAL APP ID";
    private string INTERST_ID = "REAL INTERESTITIAL ID";
    

    

    private UnityAction rewardedAction;

    protected AdsControl()
    {
    }

    private static AdsControl _instance;
    private InterstitialAd interstitial;
    private BannerView bannerView;

    public static AdsControl Instance;
    

    void Awake()
    {
        Instance = this;


        DontDestroyOnLoad(gameObject); //Already done by CBManager
    }

    private void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(isTest ? TEST_APPID : APP_ID);

        
        RequestInterstitial();

        

    }
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            //.AddTestDevice(AdRequest.TestDeviceSimulator)
            //.AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
            //.AddExtra("color_bg", "9B30FF")
            .Build();
    }

   

    void RequestInterstitial()
    {
        
        // Clean up interstitial ad before creating a new one.
        if (interstitial != null)
        {
            interstitial.Destroy();

        
        }

        
        interstitial = new InterstitialAd(isTest ? TEST_INTERS_ID : INTERST_ID);

        interstitial.OnAdClosed += HandleInterstialAdClosed;
        interstitial.OnAdOpening += InterstitialAdOpening;
        interstitial.OnAdLoaded += InterstitialAdLoaded;
        interstitial.OnAdFailedToLoad += Interstitial_OnAdFailedToLoad;
        

        AdRequest request = new AdRequest.Builder()
            //.AddTestDevice("2077ef9a63d2b398840261c8221a0c9b")
            .Build();
        interstitial.LoadAd(request);

        
    }

    private void Interstitial_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        
    }

    private void InterstitialAdLoaded(object sender, EventArgs e)
    {
        
    }

    private void InterstitialAdOpening(object sender, EventArgs e)
    {
        
    }

    

    private void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs e)
    {
        
    }

    

    public void showAds(UnityAction action)
    {
        
        if (rewardedAction != null)
        {
            rewardedAction = null;
        }
        this.rewardedAction = action;

        if (interstitial.IsLoaded())
        {
            
            interstitial.Show();
        }
        
        
    }

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        HideBannerAds();
    }

    public void HandleInterstialAdClosed(object sender, EventArgs args)
    {
        if (rewardedAction != null)
            rewardedAction.Invoke();

        RequestInterstitial();


    }

    public void HideBannerAds()
    {
        bannerView.Hide();
    }

    public void ShowBannerAds()
    {
        bannerView.Show();
    }
    //public void ShowRewardedAd(UnityAction action)
    //{
    //    if (rewardedAd.IsLoaded())
    //        rewardedAd.Show();
    //    if (action != null)
    //        action = null;
    //    this.rewardedAction = action;

    //}
}

