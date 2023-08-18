using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TargetBar : MonoBehaviour
{
    [SerializeField]
    Image targetBar;

    [SerializeField]
    Map map;


    [SerializeField]
    TextMeshProUGUI target_txt,metre_txt;

    [SerializeField]
    LevelManager lvl;

    [SerializeField]
    Transform tren;

    float _kalanMesafe;

    GameManager _gm;

    private void Start()
    {
        _gm = GameObject.FindObjectOfType<GameManager>();
    }

    public void TargetBilginisiAl()
    {
        target_txt.text = map.GetCityName();

    }

    public string GetCityName()
    {
        return map.GetCityName();
    }

    private void Update()
    {

        if (_gm._isEndGame == true)
        {
            targetBar.fillAmount = 1f;

            metre_txt.text = "0 Mt.";
           
            return;
        }
        
        
        targetBar.fillAmount =((1f*tren.position.z) / lvl._finishPoint.z);

        _kalanMesafe = (lvl._finishPoint.z - tren.position.z);


        metre_txt.text = _kalanMesafe.ToString("0") + " Mt.";
    }

}
