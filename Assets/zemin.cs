using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zemin : MonoBehaviour
{
    bool _renkAyarlandi;

    [SerializeField]
    MeshRenderer[] allMesh;
 
    public void ZeminRenginiAyarla(Material mat)
    {
        if (_renkAyarlandi == false)
        {
            _renkAyarlandi = true;
           
            for(int i = 0; i < allMesh.Length; i++)
            {
                allMesh[i].material = mat;
            }

        }
       
    }



   
}
