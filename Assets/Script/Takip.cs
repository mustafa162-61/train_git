using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Takip : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    Camera cam;

    Transform tr;


    private void Start()
    {
        tr = transform;
    }



    void LateUpdate()
    {

        tr.position = cam.WorldToScreenPoint(target.position);



    }
}
