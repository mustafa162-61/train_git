using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CameraControl : MonoBehaviour
{
    [SerializeField]
    Transform[] allCamPos;

    float _time=5f;


    public void CameraPosChange(int indexNo)
    {

       

        DOTween.Kill("camMove");

       

        if (indexNo == 0)
        {
            transform.DOLocalMove(allCamPos[indexNo].localPosition, _time/2f).SetId("camMove");
        }
        else
        {
            transform.DOLocalMove(allCamPos[indexNo].localPosition, _time).SetId("camMove");
        }


    }





}
