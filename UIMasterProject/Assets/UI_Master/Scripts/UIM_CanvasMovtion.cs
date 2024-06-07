using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIM_CanvasMovtion : MonoBehaviour
{
    public static UIM_CanvasMovtion instance;
    private Animator an;

    private void Awake()
    {
        instance = this;
        an = GetComponent<Animator>();

    }

    public  void OnCanvasActive()
    {
        an.SetTrigger("ISACTIVE");

    }
    

}
