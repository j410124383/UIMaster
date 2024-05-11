using UnityEngine;



public class UIM_GoldFingerPanel : MonoBehaviour
{
    private bool isOpen;

    private UIMControls inputActions;

    private void Awake()
    {
        inputActions = new UIMControls();
        inputActions.Enable();
    }

    private void OnEnable()
    {
      
    }

    void Update()
    {

        if (inputActions.UI.CallGold.triggered)
        {
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
