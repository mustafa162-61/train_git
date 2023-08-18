using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    GameObject tapToPlayTutorial, releasePlayTutorial;

    bool _tiklaniyormu;

    int _level;

    float _mesafe;

    bool mesafeyiHesapla = false;

    Transform tren, engel;

    private void Start()
    {
        _level= PlayerPrefs.GetInt("levelNo");

        if (_level != 0)
        {
            TaptoPlay_Release_Close();
            Destroy(this);
        }
    }

    public void TapToPlatTutorialGoster()
    {
      
            releasePlayTutorial.SetActive(false);
            tapToPlayTutorial.SetActive(true);
        

    }

    public void ReleaseTutorialGoster()
    {
   
                releasePlayTutorial.SetActive(true);
                tapToPlayTutorial.SetActive(false);
       
    }

    public void ParmakCekGoster(Transform tren,Transform engel)
    {
       
            mesafeyiHesapla = true;
            this.tren = tren;
            this.engel = engel;
       
      
    }


    public void TaptoPlay_Release_Close()  
    {
       
            releasePlayTutorial.SetActive(false);
            tapToPlayTutorial.SetActive(false);
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TaptoPlay_Release_Close();

        }
       
        if (Input.GetMouseButtonUp(0))
        {
            TapToPlatTutorialGoster();
        }


       
            if (mesafeyiHesapla == true)
            {
                _mesafe = engel.position.z - tren.position.z;

       

                if (_mesafe <= 50f)
                {
                if (_mesafe > 0)
                {
                    ReleaseTutorialGoster();
                }
                else
                {
                    mesafeyiHesapla = false;
                    TaptoPlay_Release_Close();
                }
                   

                }

            }


    }


    public void HesaplamayiPasifYap()
    {
        mesafeyiHesapla = false;
        TaptoPlay_Release_Close();
    }
}
