using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{

    [SerializeField]
    GameObject[] allBitki;

    [SerializeField]
    bool _isBitki,_isTas;

    int _level;

    [SerializeField]
    Material kirac, yesillik;
    
    void Start()
    {
       
        _level = PlayerPrefs.GetInt("levelNo");

        if (_isBitki || _isTas)
        {

            if (_isBitki)
            {
                for (int i = 0; i < allBitki.Length; i++)
                {
                    int rnd = Random.Range(0, 10);

                    if (rnd >= 2)
                    {
                        allBitki[i].SetActive(true);

                        if (_isBitki)
                        {
                            allBitki[i].transform.localPosition += new Vector3(Random.Range(-25, 0), 0, 0);
                        }

                    }
                    else
                    {
                        allBitki[i].SetActive(false);
                    }

                }

            }
            else
            {
                for (int i = 0; i < allBitki.Length; i++)
                {
                    int rnd = Random.Range(0, 10);

                    if (rnd >= 7)
                    {
                        allBitki[i].SetActive(true);
                        
                        MateryalRenkAyari(allBitki[i]);

                        if (_isBitki)
                        {
                            allBitki[i].transform.localPosition += new Vector3(Random.Range(-25, 0), 0, 0);
                        }

                    }
                    else
                    {
                        allBitki[i].SetActive(false);
                    }

                }
            }

        }
        else  // gecitse
        {
            int rnd = Random.Range(0, allBitki.Length);

            for (int i = 0; i < allBitki.Length; i++)
            {
                if (i == rnd)
                {
                    int rnd2 = Random.Range(0, 10);

                    if (rnd2 >= 9)
                    {
                        allBitki[i].SetActive(true);
                        MateryalRenkAyari(allBitki[i]);
                    }
                    else
                    {
                        allBitki[i].SetActive(false);
                    }
                   
                }
                else
                {
                    allBitki[i].SetActive(false);
                }

            }
        }
        
        
       
        
    }



    // levele gore proplari boyuyoruz
    void MateryalRenkAyari(GameObject obj)
    {

        Material mat;

        if (_level == 0)
        {

            obj.GetComponent<MeshRenderer>().material = kirac;
            mat = kirac;

        }
        else if (_level == 1)
        {
            obj.GetComponent<MeshRenderer>().material = yesillik;
            mat = yesillik;
        }
        else if (_level > 1 && _level <= 5)
        {

            obj.GetComponent<MeshRenderer>().material = kirac;
            mat = kirac;

        }
        else if (_level>5  && _level<=8)
        {
            obj.GetComponent<MeshRenderer>().material = yesillik;
            mat = yesillik;
        }
        else
        {
            int rnd = Random.Range(0, 10);

            if (rnd > 5)
            {
                obj.GetComponent<MeshRenderer>().material = kirac;
                mat = kirac;
            }
            else
            {
                obj.GetComponent<MeshRenderer>().material = yesillik;
                mat = yesillik;
            }
        }



        GameObject.FindObjectOfType<zemin>().ZeminRenginiAyarla(mat);

    }


}
