using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbControl : MonoBehaviour
{
    [SerializeField]
    GameObject[] allProb;


    void Start()
    {
        int rnd = Random.Range(0, allProb.Length);

        for (int i = 0; i < allProb.Length; i++)
        {
            if (i == rnd)
            {
                allProb[i].SetActive(true);
            }
            else
            {
                allProb[i].SetActive(false);
            }

        }
    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "prop")
        {
           other.gameObject.SetActive(false); 
        }
        else if (other.gameObject.tag == "zone")  // finish bolumundeki yerlesim yeriyse bu nesneyi kapatiyoruz
        {
            gameObject.SetActive(false);
        }
    }
}
