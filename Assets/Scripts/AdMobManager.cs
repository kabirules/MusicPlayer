using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobManager : MonoBehaviour {

	private BannerView bannerView;
	private InterstitialAd interstitial;

	// Use this for initialization
	void Start () {
        #if UNITY_ANDROID
            // string appId = "ca-app-pub-3940256099942544~3347511713"; // TEST
			string appId = "ca-app-pub-2228911308495304~3604820553"; // LIVE
			
        #elif UNITY_IPHONE
            string appId = "ca-app-pub-3940256099942544~1458002511"; // TEST
        #else
            string appId = "unexpected_platform";
        #endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

		this.RequestBanner();
		this.RequestInterstitial();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void RequestBanner()
    {
        #if UNITY_ANDROID
            // string adUnitId = "ca-app-pub-3940256099942544/6300978111"; // TEST
			string adUnitId = "ca-app-pub-2228911308495304/3436465445"; // LIVE
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716"; // TEST
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Create a 320x50 banner at the Bottom of the screen.
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        bannerView.LoadAd(request);		
    }

	private void RequestInterstitial()
	{
		#if UNITY_ANDROID
			// string adUnitId = "ca-app-pub-3940256099942544/1033173712"; // TEST
			string adUnitId = "ca-app-pub-2228911308495304/6203063735"; // LIVE
		#elif UNITY_IPHONE
			string adUnitId = "ca-app-pub-3940256099942544/4411468910"; // TEST
		#else
			string adUnitId = "unexpected_platform";
		#endif

		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd(adUnitId);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		interstitial.LoadAd(request);	
	}

	public void ShowInterstitial()
	{
		if (interstitial.IsLoaded()) {
			interstitial.Show();
		}
	}	
}
