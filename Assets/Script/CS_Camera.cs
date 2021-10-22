using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Camera : MonoBehaviour
{
    private static CS_Camera instance;
    public static CS_Camera Instance{
        get
        {
            if(instance ==null){
                instance = Transform.FindObjectOfType<CS_Camera>();
            }
            return instance;
        }
    }
    private bool isShake;
    Transform player;
    public float followSpeed;

    public void Shake(float shakeTime,float shakeStrength){
        if(!isShake){
            StartCoroutine(shake(shakeTime,shakeStrength));
        }
    }

    IEnumerator shake(float shakeTime,float shakeStrength){
        isShake=true;
        Transform camera=Camera.main.transform;
        Vector3 startPoint = camera.position;
        while(shakeTime>0){
            camera.position=Random.insideUnitSphere*shakeStrength+startPoint;
            shakeTime-=Time.deltaTime;
            yield return null;
        }
        camera.position=startPoint;
        isShake=false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player==null)
        {
            player=GameObject.FindGameObjectWithTag("Player").transform;
        }
        else
        {
            if(isShake)return;
             Vector3 target=player.transform.position;
             target.z=transform.position.z;
             transform.position=Vector3.MoveTowards(transform.position,target,followSpeed);
        }
    }
}
