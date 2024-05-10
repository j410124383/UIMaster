using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIM_GoldFingerPanel : MonoBehaviour
{
    private bool isOpen;
    public KeyCode callKey;

    void Update()
    {
        if (Input.GetKeyDown(callKey)){

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
