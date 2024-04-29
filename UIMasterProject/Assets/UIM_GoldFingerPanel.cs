using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIM_GoldFingerPanel : MonoBehaviour
{
    private bool isOpen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)){

            isOpen = !isOpen;
            Switch();
        }

    }


    void Switch()
    {
        var c = GetComponent<CanvasGroup>();
        if (isOpen)
        {
            c.alpha = 1;
            c.interactable = true;
        }
        else
        {
            c.alpha = 0;
            c.interactable = false;
        }
    }
}
