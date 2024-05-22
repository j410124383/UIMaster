using UnityEngine;



public class UIM_GoldFingerManager : MonoBehaviour
{
    private bool isOpen;

    private UIMControls inputActions;
    public GameObject GFPanel;

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
            GFPanel.SetActive(true);
        }
        else
        {
            GFPanel.SetActive(false);
        }
    }
}
