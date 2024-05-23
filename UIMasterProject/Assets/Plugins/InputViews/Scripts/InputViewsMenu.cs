using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InputViewsMenu : MonoBehaviour
{
    public PlayerInput playerInput;
    public TMP_Text inputText;

    public List<GameObject> gamePadList;

    private void Awake()
    {
        playerInput.onControlsChanged += CheckControllerType;
    }

    private void Update()
    {
      
      
        var str = "";
        str += "\n\n<color=yellow>ActionMap</color>\n" + playerInput.currentActionMap;
        str += "\n\n<color=yellow>ControlScheme</color>\n" + playerInput.currentControlScheme;
        str += "\n\n<color=yellow>GamepadCount</color>\n" + Gamepad.all.Count;

        inputText.text = str;
    }


    void CheckControllerType(PlayerInput action)
    {
        string name =Gamepad.current.displayName;

        if (IsXboxController(name))
        {
            Debug.Log("This is an Xbox controller.");
            switchGameUI(0);
        }
        else if (IsPS4Controller(name))
        {
            Debug.Log("This is a PS4 controller.");
            switchGameUI(1);
        }
        else
        {
            Debug.Log("This is an unknown or unsupported controller.");

        }
    }

    bool IsXboxController(string controllerName)
    {
        return controllerName.ToLower().Contains("xbox");
    }

    bool IsPS4Controller(string controllerName)
    {
        return controllerName.ToLower().Contains("wireless controller");
    }

    void switchGameUI(int num)
    {

        for (int i = 0; i < gamePadList.Count; i++)
        {
            gamePadList[i].SetActive(i == num?true:false);

        }


    }

}
