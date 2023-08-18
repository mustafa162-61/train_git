using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class NewTrenManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] allTren;
    
    [SerializeField]
    TextMeshProUGUI yuzde_txt;

    [SerializeField]
    GameObject newTrenPNL;

    [SerializeField]
    Image resimYeri,resimYeriSiyah;

    [SerializeField]
    Sprite[] allTrenResimleri;


    int _trenNo;

    float _resimFillDegeri;

    private void Start()
    {
        if (PlayerPrefs.HasKey("trenNo"))
        {
            _trenNo = PlayerPrefs.GetInt("trenNo");
            _resimFillDegeri = PlayerPrefs.GetFloat("resimFill");
        }
        else
        {
            _trenNo = 0;
            PlayerPrefs.SetInt("trenNo", _trenNo);

            _resimFillDegeri = 0f;
            PlayerPrefs.SetFloat("resimFill",_resimFillDegeri);
        }

        TrenGorunurlugu();
    }


    void TrenGorunurlugu()
    {
        for (int i = 0; i < allTren.Length; i++)
        {
            allTren[i].SetActive(false);
        }

        allTren[_trenNo].SetActive(true);
    }






    public void SetTren()
    {

        if (_trenNo >= allTrenResimleri.Length)  // acilacak tren kalmamissa calismayacak
        {

           GameObject.FindObjectOfType<UpgradeManager>().WinPNLGoster();
                
           return;
        }
        
        
        newTrenPNL.SetActive(true);

        resimYeri.sprite = allTrenResimleri[_trenNo];
        resimYeriSiyah.sprite = allTrenResimleri[_trenNo];

        resimYeri.fillAmount = _resimFillDegeri;

        yuzde_txt.text = "%" + (_resimFillDegeri * 100).ToString("0");


        StartCoroutine(ZamanlaDegeriArtir());

    }

    IEnumerator ZamanlaDegeriArtir()
    {
        yield return new WaitForSeconds(1.5f);

        float target = (_resimFillDegeri + 0.2f);

  

        for (int i = 0; i < 20; i++)
        {
            _resimFillDegeri += 0.01f;

            resimYeri.fillAmount = _resimFillDegeri;
            yuzde_txt.text = "%" + (_resimFillDegeri * 100).ToString("0");

            yield return new WaitForSeconds(0.02f);

        }


        if (target >= 1.0f)
        {
            _trenNo++;
            _resimFillDegeri = 0.0f;
            target = 0.0f;
        }

        PlayerPrefs.SetInt("trenNo", _trenNo);
        PlayerPrefs.SetFloat("resimFill", target);
    }

}
