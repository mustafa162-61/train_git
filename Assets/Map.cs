using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Map : MonoBehaviour
{
    [SerializeField]
    GameObject[] allCity;

    [SerializeField]
    Transform mapCam,mapCamPoint,mapKirmiziIcon;

    int _level;

    [SerializeField]
    TextAsset allCityTEXT;

    string _currentCity;

    [SerializeField]
    GameObject MapOBJ;

    [SerializeField]
    GameObject _canvas;

    [SerializeField]
    GameObject closeBTN;

    [SerializeField]
    Material renkliHaritaMateryali, duzHaritaMateryali;

    [SerializeField]
    TextMeshProUGUI toplamKM_txt;

   


    private void Start()
    {

        SehirleriRenksizYap();
        
        
        OpenMap();
    }


    void SehirleriRenksizYap()
    {


        for(int i = 0; i < allCity.Length; i++)
        {
            allCity[i].GetComponent<MeshRenderer>().material = duzHaritaMateryali;
        }


    }



    public void OpenMap()
    {
        _canvas.SetActive(false);
        MapOBJ.SetActive(true);
        
        closeBTN.SetActive(true);

        _level= PlayerPrefs.GetInt("levelNo");


        float oldKM;

        if (PlayerPrefs.HasKey("ToplamKM") == true)
        {
            oldKM = PlayerPrefs.GetFloat("ToplamKM");
        }
        else
        {
            oldKM = 0f;
            PlayerPrefs.SetFloat("ToplamKM",oldKM);
        }


        toplamKM_txt.text = oldKM.ToString("0.0") + " KM";


        for (int i = 0; i < allCity.Length; i++)
        {

            if (i <= _level)
            {
                allCity[i].GetComponent<MeshRenderer>().material = renkliHaritaMateryali;

                if (i == _level)
                {
                    allCity[i].GetComponent<MeshRenderer>().material.DOColor(Color.green, 0.5f).SetLoops(-1).SetEase(Ease.Linear);
                    break;
                }

            }
            else
            {
                if (i == allCity.Length - 1)  // en son sehire gelinmisse o levelde kaliyoruz
                {

                    allCity[i].GetComponent<MeshRenderer>().material.DOColor(Color.white, 0.5f).SetLoops(-1).SetEase(Ease.Linear);

                }
            }
           
        }


        int indexNo = _level;

        if (_level > (allCity.Length - 1))  // eðer level elimizdeki sehir sayisindan fazlaysa en son sehir sayisini baz aliyoruz
        {
            indexNo = (allCity.Length - 1);
        }

       
    

      //  mapCamPoint.transform.localPosition = new Vector3(allCity[indexNo].transform.localPosition.x, 0f, allCity[indexNo].transform.localPosition.z);

     //   mapCam.DOLookAt(allCity[indexNo].transform.position, 0f);

       
        /*
        mapKirmiziIcon.SetParent(allCity[indexNo].transform);
        mapKirmiziIcon.localPosition = new Vector3(0, 7f, 0f);
      */
        
        // mapKirmiziIcon.localScale = Vector3.one * 200f;

        // string[] allCityText = allCityTEXT.ToString().Trim().Split('\n');
        // _currentCity = allCityText[indexNo].Trim();

        string[] allCityText = allCity[indexNo].name.Trim().Split('_');
        _currentCity = allCityText[1].Trim();

    }


    public string GetCityName()
    {
        _level = PlayerPrefs.GetInt("levelNo");


        int indexNo = _level;

        if (_level > (allCity.Length - 1))  // eðer level elimizdeki sehir sayisindan fazlaysa en son sehir sayisini baz aliyoruz
        {
            indexNo = (allCity.Length - 1);
        }



        // string[] allCityText = allCityTEXT.ToString().Trim().Split('\n');
        // string[] kes= allCityText[indexNo].Trim().Split(',');

        string[] allCityText = allCity[indexNo].name.Trim().Split('_');
        _currentCity = allCityText[1].Trim();


        return _currentCity;
    }

  

    public void CloseMapBTN()
    {

        SesControl.instance.ZoomSesi();
        
        int indexNo = _level;

        if (_level > (allCity.Length - 1))  // eðer level elimizdeki sehir sayisindan fazlaysa en son sehir sayisini baz aliyoruz
        {
            indexNo = (allCity.Length - 1);
        }

        mapCam.DOLookAt(allCity[indexNo].transform.position, 10f);

        //mapCam.DOLookAt(allCity[0].transform.position, 0f);
        mapCam.DOMove(allCity[indexNo].transform.position, 0.75f).SetEase(Ease.Linear).OnComplete(()=>CanvasSetActive());

        GameObject.FindObjectOfType<TargetBar>().TargetBilginisiAl();

        closeBTN.SetActive(false);
    }


    void CanvasSetActive()
    {
        _canvas.SetActive(true);
        MapOBJ.SetActive(false);
    }

}
