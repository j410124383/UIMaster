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

    void Update()
    {
        if (UIM_SettingManager.instance.curSetData.num_AllowedGF != 0) return;

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
