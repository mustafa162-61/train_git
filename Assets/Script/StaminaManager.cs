using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StaminaManager : MonoBehaviour
{
    [SerializeField]
    Image _stamiaBar;

    GameManager _gm;

    [SerializeField]
    UpgradeManager _upManager;

    [HideInInspector]
    public bool _isBos;

    [SerializeField]
    Animator barAnim;

    float _barAmount;

    [SerializeField]
    Transform staminaFullReklamBTN;

    bool staminaDegeriAlindiMi = false;

   

    [SerializeField]
    GameObject notStaminaInfo;

  
    void Start()
    {
        _stamiaBar.fillAmount = 1.0f;

        _gm = GameObject.FindObjectOfType<GameManager>();

        
    }

    // Update is called once per frame

    bool staminaFulleniyor = false;
   
      private void Update()
      {
         if (_gm._isGameStart == true)
         {
            if (Input.GetMouseButton(0))
            {
                if (staminaFulleniyor == true)
                {
                    return;
                }

                if (staminaDegeriAlindiMi == false)
                {
                    staminaDegeriAlindiMi = true;
                    _barAmount = _upManager._staminaCurrent;

                }
                
                _barAmount -= (0.1f * Time.deltaTime);


               _stamiaBar.fillAmount = (_barAmount / _upManager._staminaCurrent);



                if (_stamiaBar.fillAmount <= 0.5f && _stamiaBar.fillAmount > 0.2f)
                {

                   // BarRenkDegisimi(Color.yellow);

                    if (_stamiaBar.fillAmount <= .3f)
                    {
                 
                        staminaFullReklamBTN.DOScale(1f, 1f).SetEase(Ease.OutBack);

                    }
                   

                }else if (_stamiaBar.fillAmount <= 0.2f)
                {
                   // BarRenkDegisimi(Color.red);

                }

            }

            if (_stamiaBar.fillAmount > 0f)
            {
                _isBos = false;
            }
            else
            {
                _isBos = true;

                staminaFullReklamBTN.DOScale(0f, 0f).SetEase(Ease.OutBack);

                notStaminaInfo.SetActive(true);

                _gm.FailPNLGoster();
            }

         }
      }



    public void StaminaFulle()  // fail alininca tekrar baslarken calisiyor, reklam izlendiyse
    {

        staminaFulleniyor = true;

        _barAmount = _upManager._staminaCurrent;
        _stamiaBar.fillAmount = (_barAmount / _upManager._staminaCurrent);

        notStaminaInfo.SetActive(false);

        _stamiaBar.color = Color.green;

        staminaFullReklamBTN.DOScale(0f, 0f);

        staminaFulleniyor = false;

    }



    void BarRenkDegisimi(Color clr)
    {
        if (_stamiaBar.color != clr)
        {
            _stamiaBar.DOColor(clr, 1.5f);
        }
       
    }



}
