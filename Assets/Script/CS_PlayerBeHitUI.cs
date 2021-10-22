using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CS_PlayerBeHitUI : MonoBehaviour
{
    // Start is called before the first frame update

    public Image img;
    public float time;

    public Color flashColor;

    private Color defualtColor;



    void Start()
    {
        defualtColor=img.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitFlashScreen(){
        StartCoroutine(hitFlash());

    }

    IEnumerator hitFlash(){
        img.color=flashColor;
        yield return new WaitForSecondsRealtime(time);
        img.color=defualtColor;
    }
}
