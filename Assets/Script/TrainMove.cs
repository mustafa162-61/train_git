using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TrainMove : MonoBehaviour
{

    bool _isMove;

    float _currentSpeed;
    float _maxSpeed;

    float _maxTrenSesPitch = 1.5f;
    float _minTrenSesPitch = 0.5f;

    float _currentIsciSpeed = 1.0f;
    float _maxIsciHizi = 2.5f;
    float _minIsciHizi = 1.0f;

    Transform _tr;

    [SerializeField]
    CameraControl _camControl;

    [SerializeField]
    GameObject _tekerKivilcimEfekti;

    [SerializeField]
    Animator[] allIsciAnim;

    [SerializeField]
    UpgradeManager _upManager;

    [SerializeField]
    StaminaManager staminaManager;

    GameManager _gm;

    bool _paraOlustur;

    [SerializeField]
    RectTransform _paraSpawnPos;

    [SerializeField]
    GameObject _paraUiPrefab;

    [SerializeField]
    Transform _canvas, _paraVerisYeri;

    [SerializeField]
    GameObject _paraTextAnim;

    int _kazanilacakParaDegeri;

    [SerializeField]
    LayerMask engelLayer;

    RaycastHit hit;

    [SerializeField]
    ParticleSystem patlamaEfekti;

    float _rayAcilmaZamani;



    void Start()
    {
        _tr = transform;

        _gm = GameObject.FindObjectOfType<GameManager>();



        _rayAcilmaZamani = (0.5f / _upManager._worker_level);

        TreninMaxHiziniAyarla();
    }

 


    public void TreninMaxHiziniAyarla()  // Kayitli veriye gore hizini ayarliyoruz, en fazla 50 oluyor, 50'den sonra staminayi yukseltiyor
    {

            _maxSpeed = 15+(_upManager._worker_level * 5f);

            if (_maxSpeed >= 50f)
            {
                _maxSpeed = 50f;

            }
    }

    public float GetMaxSpeed()
    {
        return _maxSpeed;
    }

    public bool TrenMaxHizdaMi()
    {
        if (_maxSpeed < 50f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void MoveChange(bool value)  // Trenin ve kameranin hareketini kontrol ediyor
    {
        StartCoroutine(IsciAnimAktifYap(value));

        _paraOlustur = value;
        _isMove = value;
        _tekerKivilcimEfekti.SetActive(value);

      //  Debug.Log(_paraOlustur);
      
        if (value == true)
        {
            _camControl.CameraPosChange(1);
        }
        else
        {
            _camControl.CameraPosChange(0);
        }


    }



    IEnumerator ParaOlustur(int miktar)  //  para kazanirken calisiyor
    {

        int dongu = miktar;

        if (dongu > 10)
        {
            dongu = 10;
        }

        float beklemeAraligi = (10 * 0.1f) / dongu;

        int kalanMiktar = miktar;

            while (_paraOlustur)
            {

            yield return new WaitForSeconds(beklemeAraligi);

            if (_paraOlustur == false)
            {
                break;
            }

            List<Transform> allPara = new List<Transform>();

            for (int i = 0; i < dongu; i++)
            {

                Vector3 newPos = new Vector3(Random.Range(_paraSpawnPos.position.x - 40f, _paraSpawnPos.position.x + 40f), Random.Range(_paraSpawnPos.position.y - 40f, _paraSpawnPos.position.y + 40f), 0);

                GameObject newPara = Instantiate(_paraUiPrefab, newPos, Quaternion.identity, _canvas);

                allPara.Add(newPara.transform);

                newPara.transform.DOScale(0.75f, 0.5f);

                yield return new WaitForSeconds(beklemeAraligi);

            }


            for (int i = 0; i < allPara.Count; i++)
            {

                allPara[i].DOMove(_paraVerisYeri.position, 0.75f);


                if (i == (allPara.Count - 1))  // son para kalincaya kadar -1 azaltiyoruz, en son parada geriye kalani ekliyoruz
                {
                    StartCoroutine(ParaVarisYeriHareketi(0.75f, kalanMiktar, allPara[i].gameObject));

                    kalanMiktar = miktar;
                }
                else
                {

                    StartCoroutine(ParaVarisYeriHareketi(0.75f, 1, allPara[i].gameObject));

                    kalanMiktar--;
                }


                yield return new WaitForSeconds(beklemeAraligi / 2);


            }



        }


    }



    public void UpgradeParaHarca(int miktar, Transform target)
    {
        StartCoroutine(ParaOlustur2(miktar, target));
    }


    public IEnumerator ParaOlustur2(int miktar, Transform target)  //  para kaybederken, upgrade edilirken calisiyor
    {
        int count = miktar;


        if (count > 10)  // asiri para nesnesi olusturmamak icin sinirliyoruz
        {
            count = 10;
        }


        Vector3 baslangicPos = _paraVerisYeri.position;

        _upManager.ParaAzalt(miktar);

        for (int i = 0; i < count; i++)
        {
            GameObject newPara = Instantiate(_paraUiPrefab, baslangicPos, Quaternion.identity, _canvas);

            newPara.transform.DOScale(1f, 0f);

            newPara.transform.DOMove(target.position, 0.75f);

            Destroy(newPara, 0.75f);

            yield return new WaitForSeconds(0.05f);



        }

    }


    IEnumerator TextAnimGoster()
    {
        yield return new WaitForSeconds(0.75f);


        _kazanilacakParaDegeri = _upManager._income_level;


        _paraTextAnim.SetActive(true);
        _paraTextAnim.GetComponentInChildren<TextMeshProUGUI>().text = "+" + _kazanilacakParaDegeri;


        yield return new WaitForSeconds(0.46f);  // animasyon bitince


        _paraTextAnim.SetActive(false);

    }


    IEnumerator ParaVarisYeriHareketi(float time, int miktar, GameObject paraOBJ)
    {
        yield return new WaitForSeconds(time);


        _upManager.ParaArtir(miktar);

        paraOBJ.SetActive(false);

    }


    public void MoveBTN(bool isMove)
    {
        if (_gm._isEndGame == true)
        {
            _paraOlustur = false;
            return;
        }


        if (isMove == true)
        {
            // if (_isMove == false)
            // {

            if (_gm._isGameStart == false)  // Play butonuna tiklanmis gibi yapiyoruz
            {
                //  _tekerKivilcimEfekti.SetActive(true);

                SesControl.instance.TrenSes(true);

                _gm.GameStart();
            }


            SesControl.instance.DudukSes();

             _paraOlustur = true;

            MoveChange(true);

            int sayi = _upManager._income_level;

            StartCoroutine(ParaOlustur(sayi));

           

            _gm.UpgradeBTN_DOWN();

        }
        else
        {
            MoveChange(false);

        }

    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ray")
        {
            other.transform.DOScale(1f, _rayAcilmaZamani);
        }

    }





    void Update()
    {

        if (_gm._isEndGame == true)
        {
            TrenSesiniYavaslat();

            MoveChange(false);

            _tr.position += new Vector3(0, 0, _currentSpeed * Time.deltaTime);

            return;
        }


        if (Physics.Raycast(transform.position, Vector3.forward, out hit, 7f, engelLayer))
        {
            if (hit.collider.gameObject.tag == "engel")
            {
                hit.collider.GetComponentInParent<At>().Carpisma();

                patlamaEfekti.Play();
                SesControl.instance.PatlamaSesi();
                SesControl.instance.TrenSes(false);

                // _gm.Fail();
                _gm.FailPNLGoster();
               // _paraOlustur = false;
                MoveChange(false);


                //hit.collider.GetComponent<Engel>().YokEt();






            }
            else if (hit.collider.gameObject.tag == "finishPoint")
            {

                _gm.Finish();
              //  _paraOlustur = false;
                MoveChange(false);
            }

        }


        if (_isMove)
        {

            if (staminaManager._isBos == false)
            {

                _currentSpeed += ((_upManager._worker_level * 11f) * Time.deltaTime);

                if (_currentSpeed >= _maxSpeed)
                {
                    _currentSpeed = _maxSpeed;
                }


                // tren sesini hizlandiriyoruz
                SesControl.instance.trenSes.pitch += 0.01f;

                if (SesControl.instance.trenSes.pitch >= _maxTrenSesPitch)
                {
                    SesControl.instance.trenSes.pitch = _maxTrenSesPitch;
                }


                // iscilerin animasyon hizini artiriyoruz
                _currentIsciSpeed += 0.01f;

                if (_currentIsciSpeed >= _maxIsciHizi)
                {
                    _currentIsciSpeed = _maxIsciHizi;
                }

                for (int i = 0; i < allIsciAnim.Length; i++)
                {
                    allIsciAnim[i].SetFloat("animSpeed", _currentIsciSpeed);
                }

            }
            else
            {
               // _paraOlustur = false;
                MoveChange(false);
            }

        }
        else
        {


            // tren sesini yavaslatiyoruz
            TrenSesiniYavaslat();

            // iscilerin animasyon hizini yavaslatiyoruz
            _currentIsciSpeed -= 0.01f;

            if (_currentIsciSpeed <= _minIsciHizi)
            {
                _currentIsciSpeed = _minIsciHizi;
            }

            for (int i = 0; i < allIsciAnim.Length; i++)
            {
                allIsciAnim[i].SetFloat("animSpeed", _currentIsciSpeed);
            }

        }


        //  _tr.Translate(_tr.forward * _currentSpeed * Time.deltaTime, Space.World);

        _tr.position += new Vector3(0, 0, _currentSpeed * Time.deltaTime);

    }


    void TrenSesiniYavaslat()
    {

        _currentSpeed -= ((_upManager._worker_level * 15) * Time.deltaTime);

        if (_currentSpeed <= 0f)
        {
            _currentSpeed = 0f;

        }


        SesControl.instance.trenSes.pitch -= 0.01f;

        if (SesControl.instance.trenSes.pitch <= _minTrenSesPitch)
        {
            SesControl.instance.trenSes.pitch = _minTrenSesPitch;
        }
    }







    IEnumerator IsciAnimAktifYap(bool durum)
    {
        for (int i = 0; i < allIsciAnim.Length; i++)
        {
            // allIsciAnim[i].enabled = true;

            allIsciAnim[i].SetBool("work", durum);

            yield return new WaitForSeconds(0.2f);
        }
    }
}
