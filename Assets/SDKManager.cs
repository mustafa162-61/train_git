using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDKManager : MonoBehaviour
{

    GameManager gm;


    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
    }


    public void FullStaminaReklamBTN()
    {
        if (ReklamScript.RewardedReklamHazirMi() == true)
        {
            ReklamScript.RewardedReklamGoster(StaminaFulle);
        }
    }


    public void GecisReklamiGoster()
    {
        if (ReklamScript.InterstitialHazirMi()==true)
        {
            ReklamScript.InsterstitialGoster();
        }
    }


    public void ParaOdulluReklamGoster()  // 5x para artiriyoruz
    {
        if (ReklamScript.RewardedReklamHazirMi() == true)
        {
            ReklamScript.RewardedReklamGoster(KatlamaOdullu);
        }
        
    }


    public void FailDevamEtmeReklamiGoster()
    {

        if (ReklamScript.RewardedReklamHazirMi() == true)
        {
           
            ReklamScript.RewardedReklamGoster(DevamOdulluReklam);
        }

    }

    void DevamOdulluReklam(GoogleMobileAds.Api.Reward odul)
    {
        gm.TekrarOynamaHakkiAl();
    }

    void StaminaFulle(GoogleMobileAds.Api.Reward odul)
    {
        GameObject.FindObjectOfType<StaminaManager>().StaminaFulle();
    }

    void KatlamaOdullu(GoogleMobileAds.Api.Reward odul)
    {
        GameObject.FindObjectOfType<UpgradeManager>().BolumSonuParaArtir();
    }
}
