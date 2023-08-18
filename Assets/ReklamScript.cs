using UnityEngine;
using System.Collections;
using System;
using GoogleMobileAds.Api;
using System.Collections.Generic;


public class ReklamScript : MonoBehaviour
{
    private static ReklamScript instance = null;

    [Header("Kimlikler_IOS")]
    public string bannerKimligi_ios = "";
    public string interstitialKimligi_ios = "";
    public string rewardedVideoKimligi_ios = "";

    [Header("Kimlikler_ANDROID")]
    public string bannerKimligi_and = "";
    public string interstitialKimligi_and = "";
    public string rewardedVideoKimligi_and = "";


    string bannerKimligi;
    string interstitialKimligi;
    string rewardedVideoKimligi;

    [Header("Test Modu")]
    public bool testModu = false;
    public string testDeviceID = "";
 

    [Header("Diğer Ayarlar")]
    public bool cocuklaraYonelikReklamGoster = false;
    public AdPosition bannerPozisyonu = AdPosition.Top;

    private BannerView bannerReklam;
    private InterstitialAd interstitialReklam;
    private RewardedAd rewardedVideoReklam;

    private float interstitialIstekTimeoutZamani;
    private float rewardedVideoIstekTimeoutZamani;

    private float bannerOtomatikYeniIstekZamani = float.PositiveInfinity;
    private float interstitialOtomatikYeniIstekZamani = float.PositiveInfinity;
    private float rewardedVideoOtomatikYeniIstekZamani = float.PositiveInfinity;

    private IEnumerator interstitialGosterCoroutine;
    private IEnumerator rewardedVideoGosterCoroutine;

    public delegate void RewardedVideoOdul(Reward odul);
    private RewardedVideoOdul odulDelegate;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);


#if UNITY_ANDROID
            bannerKimligi = bannerKimligi_and.Trim();
            interstitialKimligi = interstitialKimligi_and.Trim();
            rewardedVideoKimligi = rewardedVideoKimligi_and.Trim();
#else
            bannerKimligi = bannerKimligi_ios.Trim();
            interstitialKimligi = interstitialKimligi_ios.Trim();
            rewardedVideoKimligi = rewardedVideoKimligi_ios.Trim();
#endif


            testDeviceID = testDeviceID.Trim();

            MobileAds.Initialize(reklamDurumu => { });

            RequestConfiguration.Builder reklamKonfigurasyonu = new RequestConfiguration.Builder();

            if (testModu && !string.IsNullOrEmpty(testDeviceID))
                reklamKonfigurasyonu.SetTestDeviceIds(new List<string>() { testDeviceID });

            if (cocuklaraYonelikReklamGoster)
                reklamKonfigurasyonu.SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.True);

            MobileAds.SetRequestConfiguration(reklamKonfigurasyonu.build());

          
           // if (PlayerPrefs.HasKey("noADS") == false)
           // {
                BannerReklamYukle();
           // }
           

            InterstitialReklamYukle();
            RewardedReklamYukle();
        }
        else if (this != instance)
            Destroy(this);
    }

    private void Update()
    {
        float zaman = Time.realtimeSinceStartup;

        if (zaman >= bannerOtomatikYeniIstekZamani)
        {
            bannerOtomatikYeniIstekZamani = float.PositiveInfinity;
            BannerReklamYukle();
        }

        if (zaman >= interstitialOtomatikYeniIstekZamani)
        {
            interstitialOtomatikYeniIstekZamani = float.PositiveInfinity;
            InterstitialReklamYukle();
        }

        if (zaman >= rewardedVideoOtomatikYeniIstekZamani)
        {
            rewardedVideoOtomatikYeniIstekZamani = float.PositiveInfinity;
            RewardedReklamYukle();
        }
    }

    private void BannerReklamYukle()
    {
     
        
        if (!testModu && string.IsNullOrEmpty(bannerKimligi))
            return;

        if (bannerReklam != null)
            bannerReklam.Destroy();

        if (testModu && (string.IsNullOrEmpty(testDeviceID) || string.IsNullOrEmpty(bannerKimligi)))
        {

           

#if UNITY_ANDROID
            bannerReklam = new BannerView("ca-app-pub-3940256099942544/6300978111", AdSize.Banner, bannerPozisyonu);
#else
            bannerReklam = new BannerView( "ca-app-pub-3940256099942544/2934735716", AdSize.Banner, bannerPozisyonu );
#endif
        }
        else
            bannerReklam = new BannerView(bannerKimligi, AdSize.Banner, bannerPozisyonu);

        bannerReklam.OnAdFailedToLoad += BannerYuklenemedi;
        bannerReklam.LoadAd(ReklamIstegiOlustur());
      //  bannerReklam.Hide();
    }

    private void InterstitialReklamYukle()
    {
        if (!testModu && string.IsNullOrEmpty(interstitialKimligi))
            return;

        if (interstitialReklam != null && interstitialReklam.IsLoaded())
            return;

        if (interstitialReklam != null)
            interstitialReklam.Destroy();

        if (testModu && (string.IsNullOrEmpty(testDeviceID) || string.IsNullOrEmpty(interstitialKimligi)))
        {
#if UNITY_ANDROID
            interstitialReklam = new InterstitialAd("ca-app-pub-3940256099942544/1033173712");
#else
            interstitialReklam = new InterstitialAd( "ca-app-pub-3940256099942544/4411468910" );
#endif
        }
        else
            interstitialReklam = new InterstitialAd(interstitialKimligi);

        interstitialReklam.OnAdClosed += InterstitialDelegate;
        interstitialReklam.OnAdFailedToLoad += InterstitialYuklenemedi;
        interstitialReklam.LoadAd(ReklamIstegiOlustur());

        interstitialIstekTimeoutZamani = Time.realtimeSinceStartup + 10f;
    }

    private void RewardedReklamYukle()
    {
        if (!testModu && string.IsNullOrEmpty(rewardedVideoKimligi))
            return;

        if (rewardedVideoReklam != null && rewardedVideoReklam.IsLoaded())
            return;

        if (rewardedVideoReklam != null)
            rewardedVideoReklam.Destroy();

        if (testModu && (string.IsNullOrEmpty(testDeviceID) || string.IsNullOrEmpty(rewardedVideoKimligi)))
        {
#if UNITY_ANDROID
            rewardedVideoReklam = new RewardedAd("ca-app-pub-3940256099942544/5224354917");
#else
            rewardedVideoReklam = new RewardedAd( "ca-app-pub-3940256099942544/1712485313" );
#endif
        }
        else
            rewardedVideoReklam = new RewardedAd(rewardedVideoKimligi);

        rewardedVideoReklam.OnAdClosed += RewardedVideoDelegate;
        rewardedVideoReklam.OnAdFailedToLoad += RewardedVideoYuklenemedi;
        rewardedVideoReklam.OnUserEarnedReward += RewardedVideoOdullendir;
        rewardedVideoReklam.LoadAd(ReklamIstegiOlustur());

        rewardedVideoIstekTimeoutZamani = Time.realtimeSinceStartup + 30f;
    }



    private AdRequest ReklamIstegiOlustur()
    {
        return new AdRequest.Builder().Build();
    }

    private void InterstitialDelegate(object sender, EventArgs args)
    {
        InterstitialReklamYukle();
    }

    private void RewardedVideoDelegate(object sender, EventArgs args)
    {
        RewardedReklamYukle();
    }

    private void BannerYuklenemedi(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log(args.LoadAdError.ToString());
        bannerOtomatikYeniIstekZamani = Time.realtimeSinceStartup + 30f;

        if (bannerReklam != null)
        {
            bannerReklam.Destroy();
            bannerReklam = null;
        }
    }

    private void InterstitialYuklenemedi(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log(args.LoadAdError.ToString());
        interstitialOtomatikYeniIstekZamani = Time.realtimeSinceStartup + 30f;

        if (interstitialReklam != null)
        {
            interstitialReklam.Destroy();
            interstitialReklam = null;
        }
    }

    private void RewardedVideoYuklenemedi(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log(args.LoadAdError.ToString());
        rewardedVideoOtomatikYeniIstekZamani = Time.realtimeSinceStartup + 30f;

        if (rewardedVideoReklam != null)
        {
            rewardedVideoReklam.Destroy();
            rewardedVideoReklam = null;
        }
    }



    public static void BannerReklamAl()
    {
        if (instance == null)
            return;

        instance.BannerReklamYukle();
    }

    public static void BannerGoster()
    {
        if (instance == null)
            return;

        if (instance.bannerReklam == null)
        {
            instance.BannerReklamYukle();
            if (instance.bannerReklam == null)
                return;
        }

        instance.bannerReklam.Show();
    }

    public static void BannerGizle()
    {
        if (instance == null)
            return;

        if (instance.bannerReklam == null)
            return;

        instance.bannerReklam.Hide();
    }









    public static bool InterstitialHazirMi()
    {
        if (instance == null)
            return false;

        if (instance.interstitialReklam == null)
            return false;

        return instance.interstitialReklam.IsLoaded();
    }

    public static void InterstitialReklamAl()
    {
        if (instance == null)
            return;

        instance.InterstitialReklamYukle();
    }

    public static void InsterstitialGoster()
    {
        if (instance == null)
            return;


        /*
        if (PlayerPrefs.HasKey("noADS")==true)  // Reklam kapatma satin alinmissa
        {
            
            return;
        }
        */
       

        if (instance.interstitialReklam == null)
        {
            instance.InterstitialReklamYukle();
            if (instance.interstitialReklam == null)
                return;
        }

        if (instance.interstitialGosterCoroutine != null)
        {
            instance.StopCoroutine(instance.interstitialGosterCoroutine);
            instance.interstitialGosterCoroutine = null;
        }

        if (instance.interstitialReklam.IsLoaded())
            instance.interstitialReklam.Show();
        else
        {
            if (Time.realtimeSinceStartup >= instance.interstitialIstekTimeoutZamani)
                instance.InterstitialReklamYukle();

            instance.interstitialGosterCoroutine = instance.InsterstitialGosterCoroutine();
            instance.StartCoroutine(instance.interstitialGosterCoroutine);
        }
    }



    public static bool RewardedReklamHazirMi()
    {
        if (instance == null)
            return false;

        if (instance.rewardedVideoReklam == null)
            return false;

        return instance.rewardedVideoReklam.IsLoaded();
    }

    public static void RewardedReklamAl()
    {
        if (instance == null)
            return;

        instance.RewardedReklamYukle();
    }

    public static void RewardedReklamGoster(RewardedVideoOdul odulFonksiyonu)
    {
        if (instance == null)
            return;

        if (instance.rewardedVideoReklam == null)
        {
            instance.RewardedReklamYukle();
            if (instance.rewardedVideoReklam == null)
                return;
        }

        if (instance.rewardedVideoGosterCoroutine != null)
        {
            instance.StopCoroutine(instance.rewardedVideoGosterCoroutine);
            instance.rewardedVideoGosterCoroutine = null;
        }

        instance.odulDelegate = odulFonksiyonu;

        if (instance.rewardedVideoReklam.IsLoaded())
            instance.rewardedVideoReklam.Show();
        else
        {
            if (Time.realtimeSinceStartup >= instance.rewardedVideoIstekTimeoutZamani)
                instance.RewardedReklamYukle();

            instance.rewardedVideoGosterCoroutine = instance.RewardedVideoGosterCoroutine();
            instance.StartCoroutine(instance.rewardedVideoGosterCoroutine);
        }
    }


 


    private IEnumerator InsterstitialGosterCoroutine()
    {
        float istekTimeoutAni = Time.realtimeSinceStartup + 2.5f;
        while (!interstitialReklam.IsLoaded())
        {
            if (Time.realtimeSinceStartup > istekTimeoutAni)
                yield break;

            yield return null;

            if (interstitialReklam == null)
                yield break;
        }

        interstitialReklam.Show();
    }

    private IEnumerator RewardedVideoGosterCoroutine()
    {
        float istekTimeoutAni = Time.realtimeSinceStartup + 10f;
        while (!rewardedVideoReklam.IsLoaded())
        {
            if (Time.realtimeSinceStartup > istekTimeoutAni)
                yield break;

            yield return null;

            if (rewardedVideoReklam == null)
                yield break;
        }

        rewardedVideoReklam.Show();
    }

    private void RewardedVideoOdullendir(object sender, Reward odul)
    {
        if (odulDelegate != null)
        {
            odulDelegate(odul);
            odulDelegate = null;
        }
    }
}