using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    [SerializeField]
    GameObject _rayPrefab;

    [SerializeField]
    GameObject[] _allFinishZone;

    [SerializeField]
    GameObject[] _allProb;

    [SerializeField]
    GameObject atEngel;

    [HideInInspector]
    public Vector3 _finishPoint;


    List<GameManager> secilenProb = new List<GameManager>();

    bool birKopruEklendiMi;



    [SerializeField]
    GameObject[] allRay;

    [SerializeField]
    Transform highBayrak;




    void NesneleriEkle(int levelNo)
    {
        //  RaylariSakla();

        int sonRayNo = (levelNo + 1) * 14;

        if (sonRayNo >= (allRay.Length - 5))
        {
            sonRayNo = (allRay.Length - 5);
        }

        List<int> allPropNo = new List<int>();
        int propDongu = sonRayNo / 3;


        List<int> allEngelNo = new List<int>();

        int engelDongu = sonRayNo / 20;


            for (int i = 0; i < engelDongu; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int rnd = Random.Range(10, (sonRayNo-3));


                    if (allEngelNo.Contains(rnd) == false)
                    {
                        allEngelNo.Add(rnd);
                        EngelAdd(allRay[rnd].transform.position);
                        break;
                    }
                    else
                    {
                        j--;
                    }
                }

            }
        



        for (int i = 0; i < propDongu; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                int rnd = Random.Range(10, sonRayNo);

                if (allPropNo.Contains(rnd) == false && allEngelNo.Contains(rnd) == false)
                {
                    allPropNo.Add(rnd);
                    PropAdd(allRay[rnd].transform.position);
                    break;
                }
                else
                {
                    j--;
                }
            }


        }





        _finishPoint = allRay[sonRayNo].transform.position;
        FinishZoneAdd(levelNo);


        HighBayrakKonumlandir();

        /*
       Vector3 rayPos = new Vector3(0, 0, 30);

       int rayCount =  18 + (levelNo*10);   


       for (int i = 0; i <rayCount ; i++)
       {
           GameObject newRay = Instantiate(_rayPrefab, rayPos, Quaternion.identity);

           rayPos = new Vector3(0, 0, rayPos.z + 10);


           if (levelNo <3)//ilk levelse engeli koymak mecburi oluyor
           {

               if (i == (rayCount / 2))  // yolun yarisina ekliyoruz
               {

                   GameObject newProp = Instantiate(engel, rayPos, Quaternion.identity);

               }
               else
               {
                   if (Random.Range(0, 10) >= 2)  // rast gele prob ekliyoruz
                   {
                       PropAdd(rayPos);
                   }

               }

           }
           else
           {

               if (Random.Range(0, 10) >= 2)  // rast gele prob ekliyoruz
               {
                   PropAdd(rayPos);
               }
               else
               {

                   EngelAdd(rayPos);
               }

           }


           if (i == (rayCount - 1))
           {
               RaylariSakla();

               _finishPoint = rayPos;
               FinishZoneAdd(levelNo);
           }

       }

       */
    }

    public void RaylariEkle(int levelNo)  // level*15  miktarinca ray ekliyoruz
    {

        float zPos = 0f;
        
        for(int i = 0; i < allRay.Length; i++)
        {
            allRay[i].transform.position = new Vector3(0, 0, zPos);

            zPos += 10f;

            if (i == (allRay.Length - 1))
            {
                NesneleriEkle(levelNo);
            }

        }

    }



    void HighBayrakKonumlandir()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            float Z = PlayerPrefs.GetFloat("HighScore");

            highBayrak.position = new Vector3(highBayrak.position.x, highBayrak.position.y, Z);
        }
    }


    void RaylariSakla()  // Raylar eklendikten sonra gorunmez yapiyoruz
    {

        for(int i = 0; i < allRay.Length; i++)
        {
            allRay[i].transform.localScale = Vector3.zero;
        }
    }




    void FinishZoneAdd(int levelNo)  // Finish bolgesini ekliyoruz
    {
      

        int zoneNo;

        if(levelNo < (_allFinishZone.Length - 1))
        {
            zoneNo = levelNo;
        }
        else
        {
            zoneNo = Random.Range(0, _allFinishZone.Length);
        }

     

        GameObject newFinishZone = Instantiate(_allFinishZone[zoneNo], _finishPoint, Quaternion.identity);


        newFinishZone.GetComponentInChildren<TextMeshProUGUI>().text = GameObject.FindObjectOfType<TargetBar>().GetCityName();

    }

    
    void PropAdd(Vector3 pos)
    {

       GameObject newProp = Instantiate(_allProb[0], pos, Quaternion.identity);
        
    }


    void EngelAdd(Vector3 pos)
    {


       GameObject newEngelAt = Instantiate(atEngel, pos, Quaternion.identity);
        
       


    }


   

}
