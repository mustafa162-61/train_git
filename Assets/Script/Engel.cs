using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class Engel : MonoBehaviour
{

    [SerializeField]
    EngelParca[] allEngel;

    int _rndEngel;


    [SerializeField]
    Transform canvasTransform;
    
    [SerializeField]
    Image _sanieBar;


    [SerializeField]
    TextMeshProUGUI _saniye_txt;


    float _toplamSaniye;

    float _kalanSaniye;

    [SerializeField]
    Animator[] _allAnim;

    Transform train;

    Coroutine _saniyeFonk=null;

    void Start()
    {
        train = GameObject.FindGameObjectWithTag("train").transform;
        
        int currentLevel = PlayerPrefs.GetInt("levelNo");

        _toplamSaniye = 3;

        _kalanSaniye = _toplamSaniye;


        _rndEngel = Random.Range(0, allEngel.Length);

        
        for(int i = 0; i < allEngel.Length; i++)
        {

            if (i == _rndEngel)
            {
                allEngel[i].engelOBJ.SetActive(true);
            }
            else
            {
                allEngel[i].engelOBJ.SetActive(false);
            }

        }
        
        
       

        StartCoroutine(MesafeKontrol());
    }

 

    IEnumerator MesafeKontrol()
    {
        bool dongu = true;

        float mesafe;



        while (dongu)
        {
            mesafe = transform.position.z-train.position.z;

         

            if (mesafe <= 50f)
            {
                dongu = false;

                StartCoroutine(IsciKontrol(true));
                _saniyeFonk=StartCoroutine(SaniyeyiBaslat());



                Tutorial tuto = GameObject.FindObjectOfType<Tutorial>();

                if (tuto != null)
                {
                    GameObject.FindObjectOfType<Tutorial>().ParmakCekGoster(train, transform);
                }



              //  GameObject.FindObjectOfType<Tutorial>().ParmakCekGoster(train, transform);
               // GameObject.FindObjectOfType<Tutorial>().ReleaseTutorialGoster();
            }
            
            yield return new WaitForSeconds(0.3f);
        }
    }


   
  

    IEnumerator IsciKontrol(bool durum)
    {
        for(int i = 0; i < _allAnim.Length; i++)
        {
            _allAnim[i].SetFloat("animSpeed", 1f);
            _allAnim[i].SetBool("work", durum);

            yield return new WaitForSeconds(0.5f);
        }
    }





    IEnumerator SaniyeyiBaslat()
    {
       
        for(float i = _kalanSaniye; i >= 0f; i-=1.0f)
        {
          
            _saniye_txt.text = i.ToString();

           // _saniye_txt.transform.DOScale(1.5f, 0.15f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Linear);

           // _sanieBar.fillAmount = i/_toplamSaniye;

            _sanieBar.DOFillAmount((i / _toplamSaniye), 1f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(1f);

            if (i == 0)
            {
                YokEt();
            }


        }
    }


    public void YokEt()
    {
       
        Tutorial tuto = GameObject.FindObjectOfType<Tutorial>();

        if (tuto != null)
        {
            GameObject.FindObjectOfType<Tutorial>().HesaplamayiPasifYap();
        }


        if (_saniyeFonk != null)
        {
            StopCoroutine(_saniyeFonk);
        }
        
        switch (allEngel[_rndEngel].tip)
        {
            case "tahta": SesControl.instance.TahtaParcalamaSesi(); break;
            case "tas": SesControl.instance.TasParcalamaSesi(); break;
        }

        gameObject.tag = "Untagged";

        canvasTransform.DOScale(0f, 0.5f).SetEase(Ease.InBack);

        StartCoroutine(IsciKontrol(false));

        StartCoroutine(IscileriAcKapat());

        for (int j = 0; j < allEngel[_rndEngel].allEngelParca.Length; j++)
        {
            allEngel[_rndEngel].allEngelParca[j].isKinematic = false;
            // Destroy(allEngelParca[j].gameObject, 3f);
        }


       
    }


    IEnumerator IscileriAcKapat()
    {
        for(int j = 0; j < 15; j++)
        {
            for (int i = 0; i < _allAnim.Length; i++)
            {
                _allAnim[i].gameObject.SetActive(!_allAnim[i].gameObject.activeInHierarchy);
            }

            yield return new WaitForSeconds(0.15f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "prop")
        {
            Destroy(other.gameObject);
        }
    }

}

[System.Serializable]
public class EngelParca
{
    public GameObject engelOBJ;
    public string tip;
    public Rigidbody[] allEngelParca;
}
