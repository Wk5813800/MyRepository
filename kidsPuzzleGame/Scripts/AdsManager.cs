using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;



public class AdsManager : MonoBehaviour
{
    # if UNITY_ANDROID
    
    public static AdsManager Instance;
    private  BannerView _bannerView;
    private RewardedInterstitialAd _rewardedInterstitialAd;
    private InterstitialAd _interstitialAd;
    // Create a 320x50 banner views at coordinate (0,50) on screen.
        //  BannerView _bannerView;
     public void Start()
    {
        // // Initialize the Google Mobile Ads SDK.
        // MobileAds.Initialize((InitializationStatus initStatus) =>
        // {
        //     // This callback is called once the MobileAds SDK is initialized.
        //     LoadAd();
        //     LoadRewardedInterstitialAd();
        //     // ShowRewardedInterstitialAd();
        // });
    }
    private void Awake() 
    {
        Instance = this;
                // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            LoadAd();
            LoadRewardedInterstitialAd();
            // ShowRewardedInterstitialAd();
        });
    }
     // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
  private string _adUnitId = "ca-app-pub-5995381836831404~4780824124";
    private string _adUnitIdbnner = "ca-app-pub-5995381836831404~4780824124";

#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
  private string _adUnitId = "unused";
#endif



  /// <summary>
  /// Creates a 320x50 banner view at top of the screen.
  /// </summary>
  public void CreateBannerView()
  {
      Debug.Log("Creating banner view");

      // If we already have a banner, destroy the old one.
      if (_bannerView != null)
      {
          DestroyAd();
      }

      // Create a 320x50 banner at top of the screen
      _bannerView = new BannerView(_adUnitIdbnner, AdSize.Banner, AdPosition.Top);
      
  }
  
/// <summary>
/// Creates the banner view and loads a banner ad.
/// </summary>
public void LoadAd()
{
    // create an instance of a banner view first.
    if(_bannerView == null)
    {
        CreateBannerView();
    }

    // create our request used to load the ad.
    var adRequest = new AdRequest();

    // send the request to load the ad.
    Debug.Log("Loading banner ad.");
    _bannerView.LoadAd(adRequest);
}
/// <summary>
/// listen to events the banner view may raise.
/// </summary>
private void ListenToAdEvents()
{
    // Raised when an ad is loaded into the banner view.
    _bannerView.OnBannerAdLoaded += () =>
    {
        Debug.Log("Banner view loaded an ad with response : "
            + _bannerView.GetResponseInfo());
    };
    // Raised when an ad fails to load into the banner view.
    _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
    {
        Debug.LogError("Banner view failed to load an ad with error : "
            + error);
    };
    // Raised when the ad is estimated to have earned money.
    _bannerView.OnAdPaid += (AdValue adValue) =>
    {
        Debug.Log(string.Format("Banner view paid {0} {1}.",
            adValue.Value,
            adValue.CurrencyCode));
    };
    // Raised when an impression is recorded for an ad.
    _bannerView.OnAdImpressionRecorded += () =>
    {
        Debug.Log("Banner view recorded an impression.");
    };
    // Raised when a click is recorded for an ad.
    _bannerView.OnAdClicked += () =>
    {
        Debug.Log("Banner view was clicked.");
    };
    // Raised when an ad opened full screen content.
    _bannerView.OnAdFullScreenContentOpened += () =>
    {
        Debug.Log("Banner view full screen content opened.");
    };
    // Raised when the ad closed full screen content.
    _bannerView.OnAdFullScreenContentClosed += () =>
    {
        Debug.Log("Banner view full screen content closed.");
    };
}
public void DestroyAd()
{
    if (_bannerView != null)
    {
        Debug.Log("Destroying banner view.");
        _bannerView.Destroy();
        _bannerView = null;
    }
}

/// <summary>
/// //////////////////////////////
/// </summary>///
  public void LoadRewardedInterstitialAd()
  {
      // Clean up the old ad before loading a new one.
      if (_rewardedInterstitialAd != null)
      {
            _rewardedInterstitialAd.Destroy();
            _rewardedInterstitialAd = null;
      }

      Debug.Log("Loading the rewarded interstitial ad.");

      // create our request used to load the ad.
      var adRequest = new AdRequest();
      adRequest.Keywords.Add("unity-admob-sample");

      // send the request to load the ad.
      RewardedInterstitialAd.Load(_adUnitId, adRequest,
          (RewardedInterstitialAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("rewarded interstitial ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.Log("Rewarded interstitial ad loaded with response : "
                        + ad.GetResponseInfo());

              _rewardedInterstitialAd = ad;
          });
  }

  public void ShowRewardedInterstitialAd()
{
    const string rewardMsg =
        "Rewarded interstitial ad rewarded the user. Type: {0}, amount: {1}.";

    if (_rewardedInterstitialAd != null && _rewardedInterstitialAd.CanShowAd())
    {
        _rewardedInterstitialAd.Show((Reward reward) =>
        {
            // TODO: Reward the user.
            Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
        });
    }
    LoadRewardedInterstitialAd();
}
private void RegisterEventHandlers(RewardedInterstitialAd ad)
{
    // Raised when the ad is estimated to have earned money.
    ad.OnAdPaid += (AdValue adValue) =>
    {
        Debug.Log(string.Format("Rewarded interstitial ad paid {0} {1}.",
            adValue.Value,
            adValue.CurrencyCode));
    };
    // Raised when an impression is recorded for an ad.
    ad.OnAdImpressionRecorded += () =>
    {
        Debug.Log("Rewarded interstitial ad recorded an impression.");
    };
    // Raised when a click is recorded for an ad.
    ad.OnAdClicked += () =>
    {
        Debug.Log("Rewarded interstitial ad was clicked.");
    };
    // Raised when an ad opened full screen content.
    ad.OnAdFullScreenContentOpened += () =>
    {
        Debug.Log("Rewarded interstitial ad full screen content opened.");
    };
    // Raised when the ad closed full screen content.
    ad.OnAdFullScreenContentClosed += () =>
    {
        Debug.Log("Rewarded interstitial ad full screen content closed.");
    };
    // Raised when the ad failed to open full screen content.
    ad.OnAdFullScreenContentFailed += (AdError error) =>
    {
        Debug.LogError("Rewarded interstitial ad failed to open " +
                       "full screen content with error : " + error);
    };
}
private void RegisterReloadHandler(RewardedInterstitialAd ad)
{
    // Raised when the ad closed full screen content.
    ad.OnAdFullScreenContentClosed += ()=>
    {
        Debug.Log("Rewarded interstitial ad full screen content closed.");

        // Reload the ad so that we can show another as soon as possible.
        LoadRewardedInterstitialAd();
    };
    // Raised when the ad failed to open full screen content.
    ad.OnAdFullScreenContentFailed += (AdError error) =>
    {
        Debug.LogError("Rewarded interstitial ad failed to open " +
                       "full screen content with error : " + error);

        // Reload the ad so that we can show another as soon as possible.
        LoadRewardedInterstitialAd();
    };
}/////////////////////////////////////////////////////////

/// Load intestetial Add
      public void LoadInterstitialAd()
  {
      // Clean up the old ad before loading a new one.
      if (_interstitialAd != null)
      {
            _interstitialAd.Destroy();
            _interstitialAd = null;
      }

      Debug.Log("Loading the interstitial ad.");

      // create our request used to load the ad.
      var adRequest = new AdRequest();

      // send the request to load the ad.
      InterstitialAd.Load(_adUnitId, adRequest,
          (InterstitialAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("interstitial ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.Log("Interstitial ad loaded with response : "
                        + ad.GetResponseInfo());

              _interstitialAd = ad;
          });
  }
  ////// Show intestetial Add
    public void ShowInterstitialAd()
{
    if (_interstitialAd != null && _interstitialAd.CanShowAd())
    {
        Debug.Log("Showing interstitial ad.");
        _interstitialAd.Show();
    }
    else
    {
        Debug.LogError("Interstitial ad is not ready yet.");
    }
    LoadInterstitialAd();
}
  /////////////////// Intestetial Add Events...
    private void RegisterEventHandlers(InterstitialAd interstitialAd)
{
    // Raised when the ad is estimated to have earned money.
    interstitialAd.OnAdPaid += (AdValue adValue) =>
    {
        Debug.Log(string.Format("Interstitial ad paid {0} {1}.",
            adValue.Value,
            adValue.CurrencyCode));
    };
    // Raised when an impression is recorded for an ad.
    interstitialAd.OnAdImpressionRecorded += () =>
    {
        Debug.Log("Interstitial ad recorded an impression.");
    };
    // Raised when a click is recorded for an ad.
    interstitialAd.OnAdClicked += () =>
    {
        Debug.Log("Interstitial ad was clicked.");
    };
    // Raised when an ad opened full screen content.
    interstitialAd.OnAdFullScreenContentOpened += () =>
    {
        Debug.Log("Interstitial ad full screen content opened.");
    };
    // Raised when the ad closed full screen content.
    interstitialAd.OnAdFullScreenContentClosed += () =>
    {
        Debug.Log("Interstitial ad full screen content closed.");
    };
    // Raised when the ad failed to open full screen content.
    interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
    {
        Debug.LogError("Interstitial ad failed to open full screen content " +
                       "with error : " + error);
    };
}
/////////////////// PreLoad Adds
    private void RegisterReloadHandler(InterstitialAd interstitialAd)
{
    // Raised when the ad closed full screen content.
    interstitialAd.OnAdFullScreenContentClosed += () =>
    {
        Debug.Log("Interstitial Ad full screen content closed.");

        // Reload the ad so that we can show another as soon as possible.
        LoadInterstitialAd();
    };
    // Raised when the ad failed to open full screen content.
    interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
    {
        Debug.LogError("Interstitial ad failed to open full screen content " +
                       "with error : " + error);

        // Reload the ad so that we can show another as soon as possible.
        LoadInterstitialAd();
    };
}
#endif
}

