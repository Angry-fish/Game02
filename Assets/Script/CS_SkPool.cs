using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SkPool : MonoBehaviour
{
    public static CS_SkPool instance;
    public GameObject shadowPrefab; 
    public int shadowCount;
    private Queue<GameObject> Qgameobject=new Queue<GameObject>();
    void Awake(){
        instance =this;
        FillPool();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void FillPool(){
        for(int i=0;i<shadowCount;i++){
            var newShadow =Instantiate(shadowPrefab);
            newShadow.transform.SetParent(transform);
            RetrunPool(newShadow);
        }
    }

    public void RetrunPool(GameObject g){
        g.SetActive(false);
        Qgameobject.Enqueue(g);

    }

    public GameObject GetFromPool(Transform startPoint){
        if(Qgameobject.Count==0){
            FillPool();
        }
        var outShadow = Qgameobject.Dequeue();
        outShadow.SetActive(true);
        outShadow.transform.position=startPoint.position;
        return outShadow;
    }

  
    
}
