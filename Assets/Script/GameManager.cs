using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    //[HideInInspector]
    public bool _isGameStart,_isEndGame;

    [SerializeField]
    Transform[] _allUpgradeBTN;


    [SerializeField]
    GameObject failPNL;

    [SerializeField]
    UpgradeManager _upManager;

    float _upgradeBTNPos_Y;

    [SerializeField]
    Image _siyahlik;


    private void Start()
    {
        Application.targetFrameRate = 60;
        
        _upgradeBTNPos_Y = _allUpgradeBTN[0].position.y;

       
    }


    public void GameStart()
    {

        _isGameStart = true;

     

    }

 

    public void Finish()
    {
        if (_isEndGame == false)
        {

            SesControl.instance.WinSesi();

            _isEndGame = true;

            GameObject.FindObjectOfType<NewTrenManager>().SetTren();  // finishpaneli inspectordan dotwen animasyon bitince aciyoruz
            
         // finishPNL.SetActive(true);

            _isGameStart = false;

            _upManager.LevelUp();

            ToplamKMkontrol();

            HighScoreControl();
        }
      

    }


    void ToplamKMkontrol()
    {
        float toplamKm= GameObject.FindGameObjectWithTag("train").transform.position.z;

        if (PlayerPrefs.HasKey("ToplamKM"))
        {
            float oldKM = PlayerPrefs.GetFloat("ToplamKM");

            if (toplamKm > oldKM)
            {
                PlayerPrefs.SetFloat("ToplamKM", toplamKm);
            }
        }

    }


    void HighScoreControl()
    {
        float Z = GameObject.FindGameObjectWithTag("train").transform.position.z;


        if (PlayerPrefs.HasKey("HighScore"))
        {
            float oldScore = PlayerPrefs.GetFloat("HighScore");

            if (Z > oldScore)
            {
                PlayerPrefs.SetFloat("HighScore", Z);
            }
        }
        else
        {
            PlayerPrefs.SetFloat("HighScore", Z);
        }
    }

    void Fail()
    {

        HighScoreControl();

        GameObject.FindObjectOfType<Vibration_Controller>().Medium_vib();

      //  StartCoroutine(GecikmeliPanelGoster(2f, failPNL));

    }



    public void FailPNLGoster()
    {

        _isGameStart = false;
        _isEndGame = true;

        StartCoroutine(GecikmeliPanelGoster(3f, failPNL));
    }



    public void TekrarOynamaHakkiAl()  // faildeki odullu reklam izlenirse calisiyor
    {
        
        SesControl.instance.TrenSes(true);

        _isGameStart = true;
        _isEndGame = false;

        failPNL.SetActive(false);

      //  GameObject.Find("FailPNL").SetActive(false);

     
        
    }




    IEnumerator GecikmeliTitret()
    {
       
        for(int i = 0; i < 10;i++)
        {
            GameObject.FindObjectOfType<Vibration_Controller>().Medium_vib();

            yield return new WaitForSeconds(0.01f);
        }
    }



    IEnumerator GecikmeliPanelGoster(float time,GameObject obj)
    {
        yield return new WaitForSeconds(time);

        obj.SetActive(true);
    }

    public void UpgradeBTN_UP()
    {
        
        for (int i = 0; i < _allUpgradeBTN.Length; i++)
        {
            _allUpgradeBTN[i].DOMoveY(_upgradeBTNPos_Y, 0.5f);
        }
        

    }

    public void UpgradeBTN_DOWN()
    {
        for (int i = 0; i < _allUpgradeBTN.Length; i++)
        {
            _allUpgradeBTN[i].DOMoveY(-1000f, 0.5f);
        }
    }


    public void SahneyiTekrarAc()
    {
        int currentSceneNo = SceneManager.GetActiveScene().buildIndex;

        _siyahlik.DOFade(1f, 0.3f).OnComplete(() => SceneManager.LoadScene(currentSceneNo));
    }


    public void OdulluFullStaminaReklamBTN()
    {
        GameObject.FindObjectOfType<SDKManager>().FullStaminaReklamBTN();
    }


    public void GecisReklamiBTN()
    {
        GameObject.FindObjectOfType<SDKManager>().GecisReklamiGoster();
    }


    public void Odullu5XReklamBTN()
    {
        GameObject.FindObjectOfType<SDKManager>().ParaOdulluReklamGoster();
    }


    public void OdulluContinueBTN()
    {
        GameObject.FindObjectOfType<SDKManager>().FailDevamEtmeReklamiGoster();
    }



}
