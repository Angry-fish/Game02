using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Shadow : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer mySprite;
    private SpriteRenderer playerSprite;
    private Color color;

    [Header("时间")]
    public float activeTime;
    public float activeStart;
    [Header("不透明度")]
    private float alpha;
    public float alphaSet;
    public float alphaMultiplier;
    
    private void OnEnable(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSprite =player.GetComponent<SpriteRenderer>();
        mySprite=GetComponent<SpriteRenderer>();
        alpha = alphaSet;
        
        mySprite.sprite=playerSprite.sprite;
        transform.position=player.position;
        transform.rotation=player.rotation;
        transform.localScale=player.localScale;

        activeStart=Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        alpha*=alphaMultiplier;
        color =new Color(0.5f,1,1,alpha);
        mySprite.color=color;

        if(Time.time>(activeStart+activeTime)){
            CS_shadowPool.instance.RetrunPool(this.gameObject);
        }
    }
}
