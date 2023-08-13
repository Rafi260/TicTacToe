using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public AdmobAdsScript admobAdsScript;

    private void Start()
    {
        admobAdsScript.LoadBannerAd();
    }

    public void _Interstitial()
    {
        admobAdsScript.LoadInterstitialAd();
    }
}
