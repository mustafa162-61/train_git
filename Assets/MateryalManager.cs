using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MateryalManager : MonoBehaviour
{
    GameManager _gm;
    UpgradeManager _up;


  
    float currentSaniye = 0f;

    void Start()
    {
        _gm = GameObject.FindObjectOfType<GameManager>();
        _up = GameObject.FindObjectOfType<UpgradeManager>();

    }

    private void Update()
    {
        if (_gm._isGameStart == true)
        {
            if (Input.GetMouseButton(0))
            {
                if (Time.time >= currentSaniye)
                {

                    currentSaniye = (Time.time + 1.0f);
                   
                    _up.MateryaliAzalt();

                }
               
            }

        }
    }

}
