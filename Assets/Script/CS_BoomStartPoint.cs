using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_BoomStartPoint : MonoBehaviour
{
     public Transform[] point;
    public bool[] isOpen; 

    public int maxBoom;
     public float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IE_BurstSK());
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator IE_BurstSK(){
        while(true){
            
            for(int index=0;index<point.Length;index++){
                if(GameObject.FindObjectsOfType<CS_Boom>().Length< maxBoom && isOpen[index]){
                 CS_BoomPool.instance.GetFromPool(point[index].transform);
                }
            }
            yield return new WaitForSecondsRealtime(waitTime);
        }
    }
}
