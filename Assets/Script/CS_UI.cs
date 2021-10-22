using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CS_UI : MonoBehaviour
{
    public static CS_UI instance;

    public static int count;

    public Text textCount;

    void Update(){
        textCount.text = count.ToString();

    }
}
