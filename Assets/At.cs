using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class At : MonoBehaviour
{


    float _toplamSaniye=4f;



    [SerializeField]
    Animator _anim;

    [SerializeField]
    Transform _at;


    [SerializeField]
    Transform[] allTeker;

    UpgradeManager _upManager;

    [SerializeField]
    Rigidbody[] allParca;

    TrainMove _trenMove;

    int _durumNo;  //  bu degere gore atin hizini zamanini ayarliyoruz
   
    void Start()
    {

        _durumNo = Random.Range(0, 3);  // 0 yavas, carpisma, hizli git
        
        transform.position = new Vector3(transform.position.x, -0.8f, transform.position.z);

        _upManager = GameObject.FindObjectOfType<UpgradeManager>();

        int currentLevel = PlayerPrefs.GetInt("levelNo");


        _trenMove = GameObject.FindObjectOfType<TrainMove>();


         _at.localPosition = new Vector3(-40f, _at.localPosition.y, _at.localPosition.z);
       
       // _at.localPosition = new Vector3(Random.Range(-50f,-10f), _at.localPosition.y, _at.localPosition.z);


        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.left*100, out hit, 7f))
        {
            if (hit.collider.gameObject.tag == "prop")
            {
            
                Destroy(hit.collider.gameObject);

            }

        }

        StartCoroutine(MesafeKontrol());

    }



    public void Carpisma()
    {


        for (int i = 0; i < allParca.Length; i++)
        {
            allParca[i].isKinematic = false;
            allParca[i].transform.SetParent(null);
           
        }
    }


    bool _kontrol = true;
    IEnumerator MesafeKontrol()
    {
        float ara;

        float yaklasmaMesafesi=0f;

        switch (_durumNo)
        {
            case 0: yaklasmaMesafesi = 50f;break;
            case 1: yaklasmaMesafesi = 70f;break;
            case 2: yaklasmaMesafesi = 90;break;
        }


        while (_kontrol)
        {

            ara = _at.position.z - _trenMove.transform.position.z;

            if (ara <= yaklasmaMesafesi)  
            {

               // SesControl.instance.AtKosmaSesi();
                
                _kontrol = false;

                _anim.enabled = true;

                _toplamSaniye = ((3.8f * 20) / _trenMove.GetMaxSpeed());  // trenin max hizina gore kendine toplam varis suresi belirliyor

                _at.DOLocalMoveX(20f, _toplamSaniye).SetEase(Ease.Linear);

                // Destroy(_at, 20f);

                StartCoroutine(PasifYap(_at.gameObject));

                for(int i = 0; i < allTeker.Length; i++)
                {
                    allTeker[i].DOLocalRotate(new Vector3(359f, 0f, 0f), 1f,RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
                }

               
                Tutorial tuto = GameObject.FindObjectOfType<Tutorial>();

                if (tuto != null)
                {
                    GameObject.FindObjectOfType<Tutorial>().ParmakCekGoster(_trenMove.transform, transform);
                }
            }


            yield return new WaitForSeconds(0.5f);
        }




    }


    private void Update()
    {
        if (_at.position.z < _trenMove.transform.position.z)
        {

            if (_at.position.x >= _trenMove.transform.position.x)
            {
                _at.gameObject.SetActive(false);
                this.enabled = false;
            }


        }
    }

    IEnumerator PasifYap(GameObject obj)
    {
        yield return new WaitForSeconds(_toplamSaniye);

        Tutorial tuto = GameObject.FindObjectOfType<Tutorial>();

        if (tuto != null)
        {
            GameObject.FindObjectOfType<Tutorial>().HesaplamayiPasifYap();
        }

        obj.SetActive(false);
    }



}


