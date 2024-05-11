using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;


public class UIM_GoldFingerPanel : MonoBehaviour
{
    private bool isOpen;
    //public KeyCode callKey;
    private UIMControls inputActions;

    private void Awake()
    {
        inputActions = new UIMControls();

    }


    void Update()
    {
        //if (Input.GetKeyDown(callKey))
        //{

        //    isOpen = !isOpen;
        //    Switch();
        //}

        if (inputActions.UI.CallGold.IsPressed())
        {
            isOpen = !isOpen;
            Switch();
            print("1");
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
