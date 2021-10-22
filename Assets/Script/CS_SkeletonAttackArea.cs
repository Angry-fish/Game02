using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SkeletonAttackArea : MonoBehaviour
{
    public float damage;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other){      
        if(other.CompareTag("Player")&& other.GetComponent<CS_Player>()){
            other.GetComponent<CS_Player>().HitPlayer(this.transform, damage);
        }

    }
}
