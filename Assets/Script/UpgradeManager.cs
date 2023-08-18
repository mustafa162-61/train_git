using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UpgradeManager : MonoBehaviour
{

    [SerializeField]
    DefaultValue baslangicDegerleri;

    [SerializeField]
    ButonHalleri[] butonHalleri;


    [HideInInspector] public float _staminaCurrent;

    [HideInInspector] public int _levelNo;  // bolge no kullaniyoruz. Teksas gibi vb.

    [HideInInspector] public int _worker_level;
    [HideInInspector] public int _stamina_level;
    [HideInInspector] public int _income_level;
    [HideInInspector] public int _materyal_level;

    int _worker_price;
    int _stamina_price;
    int _income_price;
    int _materyal_price;


    [SerializeField]
    WORKER[] allWorkerParent;

    [HideInInspector]
    public int _coin;

    [SerializeField]
    TextMeshProUGUI _coin_txt, _staminaPrice_txt, _workerPrice_txt, _incomePrice_txt, _materyalPrice_txt, _staminaLevel_txt, _workerLevel_txt,_incomeLevel_txt, _materyalLevel_txt;

    [SerializeField]
    GameObject notMoneyInfo;

    [SerializeField]
    LevelManager lvlManager;

    [SerializeField]
    TrainMove trainManager;

    [SerializeField]
    GameObject winPNL;

    [SerializeField]
    TextMeshProUGUI winPNL_para_txt,level_txt;

    int _bolumSonuOdul;

    float _staminaArtmaDegeri = 0.1f;

    [SerializeField]
    GameObject notMateryalInfo;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("baslangicDegerleri"))
        {
            _levelNo = PlayerPrefs.GetInt("levelNo");
            
            _staminaCurrent = PlayerPrefs.GetFloat("stamina");

            _worker_level = PlayerPrefs.GetInt("worker_level");
            _stamina_level = PlayerPrefs.GetInt("stamina_level");
            _income_level = PlayerPrefs.GetInt("income_level");
           

            _worker_price = PlayerPrefs.GetInt("worker_price");
            _stamina_price = PlayerPrefs.GetInt("stamina_price");
            _income_price = PlayerPrefs.GetInt("income_price");
           


            if (PlayerPrefs.HasKey("materyal_level") == false)
            {
                _materyal_level = 1;
                _materyal_price = 2;

                Kaydet();

            }
            else
            {
                _materyal_level = PlayerPrefs.GetInt("materyal_level");
                _materyal_price = PlayerPrefs.GetInt("materyal_price");

            }

            _coin = PlayerPrefs.GetInt("coin");

        }
        else
        {
            _levelNo = 0;

            _staminaCurrent = baslangicDegerleri._staminaDegeri;

            _worker_level = 1;
            _stamina_level = 1;
            _income_level = 1;
            _materyal_level = 1;


            _worker_price = 50;
            _stamina_price = 1;
            _income_price = 5;
            _materyal_price = 2;

            _coin = 0;

            Kaydet();

            PlayerPrefs.SetString("baslangicDegerleri", "true");

        }




       // PlayerPrefs.SetInt("levelNo",690);
       // _levelNo = PlayerPrefs.GetInt("levelNo");

        lvlManager.RaylariEkle(_levelNo);
        /*
        if (_levelNo > 30)
        {
            lvlManager.RaylariEkle(30);
        }
        else
        {
            lvlManager.RaylariEkle(_levelNo);
        }
        */


        DegerGoster();
        
        WorkerGorunurlugunuAyarla();

        WokerMateryalGorunurlugunuAyarla(true);
    }

    public void LevelUp()
    {
        _levelNo++;

        Kaydet();
    }


    void WokerMateryalGorunurlugunuAyarla(bool diziyiTersineCevir=false)  // varsayin degerleri yukleniyor materyalde
    {

        int seviye = (25 + _materyal_level);

        if (seviye > allWorkerParent[0].allYukMaterial.Length)
        {
            seviye = allWorkerParent[0].allYukMaterial.Length;
        }



        for (int i = 0; i < allWorkerParent.Length; i++)
        {
            if (diziyiTersineCevir == true)
            {
                System.Array.Reverse(allWorkerParent[i].allYukMaterial);
            }

            if (allWorkerParent[i].bone.activeInHierarchy == true)
            {
                allWorkerParent[i].upgradeEfekti.Play();
            }
           

            for (int j = 0; j < allWorkerParent[i].allYukMaterial.Length; j++)
            {
                if (j < seviye)
                {
                    allWorkerParent[i].allYukMaterial[j].SetActive(true);
                }
                else
                {
                    allWorkerParent[i].allYukMaterial[j].SetActive(false);
                }
            }
        }

    }



    public void MateryaliAzalt()  
    {

        for (int i = 0; i < allWorkerParent.Length; i++)
        {

            for (int j = (allWorkerParent[i].allYukMaterial.Length-1); j >=0 ; j--)
            {
                if (allWorkerParent[i].allYukMaterial[j].activeInHierarchy==true)
                {
                    allWorkerParent[i].allYukMaterial[j].SetActive(false);

                    if (j == 0)  // en son materyal de bitmisse
                    {
                        notMateryalInfo.SetActive(true);

                        GameObject.FindObjectOfType<GameManager>().FailPNLGoster();
                    }

                    break;
                }
            }
        }

    }


    public void MateryaliFulle() // rekla izlenirse calisiyor
    {

        WokerMateryalGorunurlugunuAyarla();


    }

   


    void WorkerGorunurlugunuAyarla()
    {

        for (int i = 0; i < allWorkerParent.Length; i++)
        {

            for (int j = (allWorkerParent[i].allWorker.Length - 1); j >= 0; j--)
            {
                    allWorkerParent[i].bone.SetActive(false);
                    allWorkerParent[i].allWorker[j].worker.SetActive(false);
            }

        }


        for (int i=0;i<allWorkerParent.Length; i++)
        {

            for(int j = (allWorkerParent[i].allWorker.Length-1); j >=0; j--)
            {

                if(allWorkerParent[i].allWorker[j].levelId <= _worker_level)
                {
                    allWorkerParent[i].bone.SetActive(true);
                    allWorkerParent[i].allWorker[j].worker.SetActive(true);

                    break;
                }


            }

        }

    }


    public void MateryalBTN()
    {

        if (_coin < _materyal_price)
        {
            ParaYeterliDegilPaneliniGoster();
            return;
        }

        SesControl.instance.SatinAlmaSesi();
        trainManager.UpgradeParaHarca(_materyal_price, _materyalPrice_txt.transform);

        MateryalUpgrade();
    }



    public void MateryalUpgrade()
    {

        _materyal_level++;
       
        _materyal_price = ((_materyal_level * (_materyal_level / 2)) * 2) + 2;



        WokerMateryalGorunurlugunuAyarla();


        Kaydet();

        DegerGoster();


    }




    void ParaYeterliDegilPaneliniGoster()
    {
        if (notMoneyInfo.activeInHierarchy == false)
        {
            notMoneyInfo.SetActive(true);

            StartCoroutine(NesneleriPasifYap(notMoneyInfo, 1f));
        }
    }


    public void StaminaBTN()
    {
        if (_coin < _stamina_price)
        {
            ParaYeterliDegilPaneliniGoster();
            return;
        }


        SesControl.instance.SatinAlmaSesi();

        trainManager.UpgradeParaHarca(_stamina_price, _staminaPrice_txt.transform);

        StaminaUpgrade();

    }


    public void StaminaUpgrade()
    {
        
        _stamina_level++;
        _stamina_price = ((_stamina_level * (_stamina_level / 3)) * 2) + 1;

        _staminaCurrent += _staminaArtmaDegeri;

        Kaydet();

        DegerGoster();
    }


    void WorkerFazlasiStaminaArtir()  // tren  en hizli hale gelmiþse her worker upgrade edilisinde staminayi artiriyoruz
    {
        _staminaCurrent -= (_staminaArtmaDegeri*5f);

        Kaydet();

        DegerGoster();
    }


    public void WorkerBTN()
    {
        if (_coin < _worker_price)
        {
            ParaYeterliDegilPaneliniGoster();
            return;
        }

        SesControl.instance.SatinAlmaSesi();

        trainManager.UpgradeParaHarca(_worker_price, _workerPrice_txt.transform);

        WorkerUpgrade();
    }


    public void WorkerUpgrade()
    {

        _worker_level++;
        // _worker_price = (_worker_level * _worker_level * _worker_level) * 50;
        _worker_price = (_worker_level * 50);


        if (trainManager.TrenMaxHizdaMi() == false)
        {
            trainManager.TreninMaxHiziniAyarla();
        }
        else
        {
            WorkerFazlasiStaminaArtir();
        }

        WorkerGorunurlugunuAyarla();

        Kaydet();

        DegerGoster();
    }


    public void IncomeBTN()
    {
        if (_coin < _income_price)
        {
            ParaYeterliDegilPaneliniGoster();
            return;
        }

        SesControl.instance.SatinAlmaSesi();

        trainManager.UpgradeParaHarca(_income_price, _incomePrice_txt.transform);

        IncomeUpgrade();

    }



    public void IncomeUpgrade()
    {
        _income_level++;
        _income_price = ((_income_level * _income_level) * 3) + 2;

        Kaydet();

        DegerGoster();
    }


    IEnumerator NesneleriPasifYap(GameObject obj, float sure)
    {
        yield return new WaitForSeconds(sure);

        obj.SetActive(false);
    }

    void DegerGoster()
    {
        level_txt.text = "Level " + (_levelNo + 1);

        _coin_txt.text = _coin.ToString();

        _staminaPrice_txt.text = _stamina_price.ToString();
        _staminaLevel_txt.text = "LEVEL" + _stamina_level.ToString();

        _workerPrice_txt.text = _worker_price.ToString();
        _workerLevel_txt.text = "LEVEL" + _worker_level.ToString();

        _incomePrice_txt.text = _income_price.ToString();
        _incomeLevel_txt.text = "LEVEL" + _income_level.ToString();

        _materyalPrice_txt.text = _materyal_price.ToString();
        _materyalLevel_txt.text = "LEVEL" + _materyal_level.ToString();

        //  ButonGrafikDegisimiKontrolu();
    }



    void ButonGrafikDegisimiKontrolu()  // reklam butonlarýný görünür yapiyoruz
    {
        if (_coin >= _stamina_price)
        {
            for(int i = 0; i < butonHalleri.Length; i++)
            {
                if (butonHalleri[i].name == "stamina")
                {
                    butonHalleri[i].normal.SetActive(true);
                    butonHalleri[i].reklamli.SetActive(false);
                    break;
                }
            }
        }
        if (_coin >= _worker_price)
        {
            for (int i = 0; i < butonHalleri.Length; i++)
            {
                if (butonHalleri[i].name == "worker")
                {
                    butonHalleri[i].normal.SetActive(true);
                    butonHalleri[i].reklamli.SetActive(false);
                    break;
                }
            }
        }
        if (_coin >= _income_price)
        {
            for (int i = 0; i < butonHalleri.Length; i++)
            {
                if (butonHalleri[i].name == "income")
                {
                    butonHalleri[i].normal.SetActive(true);
                    butonHalleri[i].reklamli.SetActive(false);
                    break;
                }
            }
        }


        if (_coin < _stamina_price)
        {
            for (int i = 0; i < butonHalleri.Length; i++)
            {
                if (butonHalleri[i].name == "stamina")
                {
                    butonHalleri[i].normal.SetActive(false);
                    butonHalleri[i].reklamli.SetActive(true);
                    break;
                }
            }
        }
        if (_coin < _worker_price)
        {
            for (int i = 0; i < butonHalleri.Length; i++)
            {
                if (butonHalleri[i].name == "worker")
                {
                    butonHalleri[i].normal.SetActive(false);
                    butonHalleri[i].reklamli.SetActive(true);
                    break;
                }
            }
        }
        if (_coin < _income_price)
        {
            for (int i = 0; i < butonHalleri.Length; i++)
            {
                if (butonHalleri[i].name == "income")
                {
                    butonHalleri[i].normal.SetActive(false);
                    butonHalleri[i].reklamli.SetActive(true);
                    break;
                }
            }
        }

    }


    void Kaydet()
    {
        PlayerPrefs.SetInt("levelNo",_levelNo);

        PlayerPrefs.SetFloat("stamina", _staminaCurrent);

        PlayerPrefs.SetInt("worker_price", _worker_price);
        PlayerPrefs.SetInt("stamina_price", _stamina_price);
        PlayerPrefs.SetInt("income_price", _income_price);
        PlayerPrefs.SetInt("materyal_price", _materyal_price);

        PlayerPrefs.SetInt("worker_level", _worker_level);
        PlayerPrefs.SetInt("stamina_level", _stamina_level);
        PlayerPrefs.SetInt("income_level", _income_level);
        PlayerPrefs.SetInt("materyal_level", _materyal_level);

        PlayerPrefs.SetInt("coin", _coin);
    }


    public void ParaArtir(int deger)
    {
        _coin += deger;

       // SesControl.instance.ParaKazanmaSesi();

        Kaydet();

        DegerGoster();
    }

    public void ParaAzalt(int deger)
    {
        _coin -= deger;

        Kaydet();

        DegerGoster();
    }


    public void WinPNLGoster()  // paneli acip alacagi odul miktarini gosteriyoruz
    {
        winPNL.SetActive(true);

        _bolumSonuOdul= (_levelNo * 25);

        winPNL_para_txt.text = "+" + _bolumSonuOdul;

    }


    public void BolumSonuParaArtir(GoogleMobileAds.Api.Reward odul)  
    {
        int para = _bolumSonuOdul * 5;

        ParaArtir(para);
    }

}


[System.Serializable]
public class WORKER
{
    public GameObject bone;
    public GameObject[] allYukMaterial;
    public HumanWorker[] allWorker;
    public ParticleSystem upgradeEfekti;

}


[System.Serializable]
public class HumanWorker
{
    public int levelId;
    public GameObject worker;

}


[System.Serializable]
public class ButonHalleri
{
    public string name;
    public GameObject normal, reklamli;
}
