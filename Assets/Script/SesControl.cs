using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SesControl : MonoBehaviour
{
    public static SesControl instance = null;
    
    public AudioSource trenSes;

    [SerializeField]
    AudioSource digerSes;

    [SerializeField]
    AudioClip dudukSesi, trenSesi, hammerSesi,tasParcalamaSesi,tahtaParcalamaSesi,satinalmaSesi,paraKazanmaSesi,zoomSesi,patlamaEfektiSesi,atKosmaSesi,winSesi;



    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    public void SesSeviyesi(float seviye)
    {
        trenSes.volume = seviye;
        digerSes.volume = seviye;
    }


    public void DudukSes()
    {
        digerSes.PlayOneShot(dudukSesi);
    }



    public void TrenSes(bool durum)
    {
        if (durum == true)
        {
            if (trenSes.isPlaying == false)
            {
                trenSes.Play();
            }
        }
        else
        {
            if (trenSes.isPlaying == true)
            {
                trenSes.Stop();
            }
        }
     
    }

    public void HammerSesi()
    {
        digerSes.PlayOneShot(hammerSesi);
    }

    public void TasParcalamaSesi()
    {
        digerSes.PlayOneShot(tasParcalamaSesi);
    }

    public void TahtaParcalamaSesi()
    {
        digerSes.PlayOneShot(tahtaParcalamaSesi);
    }


    public void ZoomSesi()
    {
        digerSes.PlayOneShot(zoomSesi);
    }

    public void SatinAlmaSesi()
    {
        digerSes.PlayOneShot(satinalmaSesi);
    }


    public void PatlamaSesi()
    {
        digerSes.PlayOneShot(patlamaEfektiSesi);
    }

    /*
    public void ParaKazanmaSesi()
    {
        digerSes.PlayOneShot(paraKazanmaSesi);
    }
  

    public void AtKosmaSesi()
    {
        digerSes.PlayOneShot(atKosmaSesi);
    }

      */
    public void WinSesi()
    {
        digerSes.PlayOneShot(winSesi);
    }

}
