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
            ReklamScript.RewardedReklamGoster(GameObject.FindObjectOfType<StaminaManager>().StaminaFulle);
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
            ReklamScript.RewardedReklamGoster(GameObject.FindObjectOfType<UpgradeManager>().BolumSonuParaArtir);
        }
        
    }


    public void FailDevamEtmeReklamiGoster()
    {

        if (ReklamScript.RewardedReklamHazirMi() == true)
        {
            gm.TekrarOynamaHakkiAl();
            ReklamScript.RewardedReklamGoster(BosOdulluReklam);
        }

    }

    void BosOdulluReklam(GoogleMobileAds.Api.Reward odul)
    {

    }
}
