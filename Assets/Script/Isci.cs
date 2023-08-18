using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Isci : MonoBehaviour
{
    [SerializeField]
    ParticleSystem hammerEffect;

   
    public void HammerEffect()  // animasyonla calistiriyoruz
    {
        hammerEffect.Play();


        if (hammerEffect.gameObject.activeInHierarchy == true)
        {
            SesControl.instance.HammerSesi();
        }
        
    }


   
}
