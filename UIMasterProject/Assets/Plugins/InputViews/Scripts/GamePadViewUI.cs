using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class GamePadViewUI : MonoBehaviour
{

    public static GamePadViewUI instance;



    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {

        CheckGamepad();

    }


    public void CheckKeyboard()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            print("空格键");
        }
        if (Keyboard.current.dKey.wasReleasedThisFrame)
        {
            print("d键抬起");
        }
        if (Keyboard.current.spaceKey.isPressed)
        {
            print("空格按下");
        }
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
           
        }

    }

    public void CheckMouse()
    {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                print("鼠标右键按下");
            }
            if (Mouse.current.middleButton.wasPressedThisFrame)
            {
                print("鼠标中建按下");
            }
            if (Mouse.current.forwardButton.wasPressedThisFrame)
            {
                print("鼠标前键按下");
            }
            if (Mouse.current.backButton.wasPressedThisFrame)
            {
                print("鼠标后键按下");
            }
            //屏幕坐标(左下角为（0,0)
            print(Mouse.current.position.ReadValue());
            //两帧之间的偏移
            print(Mouse.current.delta.ReadValue());
            //滚轮
            print(Mouse.current.scroll.ReadValue());
        

    }

    public void CheckTouchScreen()
    {
        
        Touchscreen ts = Touchscreen.current;
        if (ts == null)
        {
            return;
        }
        else
        {
            var tc = ts.touches[0];
            if (tc.press.wasPressedThisFrame)
            {
                print("按下");
            }
            if (tc.press.wasReleasedThisFrame)
            {
                print("抬起");
            }
            if (tc.press.isPressed)
            {
                print("按住");
            }
            if (tc.tap.isPressed)
            {

            }
            //点击次数 
            print(tc.tapCount);
            //点击位置
            print(tc.position.ReadValue());
            //第一次接触位置
            print(tc.startPosition.ReadValue());
            //得到的范围
            print(tc.radius.ReadValue());
            //偏移位置
            print(tc.delta.ReadValue());
            //返回TouchPhase: None,Began,Moved,Ended,Canceled,Stationary
            print(tc.phase.ReadValue());

            //判断状态
            UnityEngine.InputSystem.TouchPhase tp = tc.phase.ReadValue();
            switch (tp)
            {
                //无
                case UnityEngine.InputSystem.TouchPhase.None:
                    break;
                //开始接触
                case UnityEngine.InputSystem.TouchPhase.Began:
                    break;
                //移动
                case UnityEngine.InputSystem.TouchPhase.Moved:
                    break;
                //结束
                case UnityEngine.InputSystem.TouchPhase.Ended:
                    break;
                //取消
                case UnityEngine.InputSystem.TouchPhase.Canceled:
                    break;
                //静止
                case UnityEngine.InputSystem.TouchPhase.Stationary:
                    break;
            }
        }


    }

    private string gamepadDesc;
    public void CheckGamepad()
    {
        
        Gamepad gamepad = Gamepad.current;
        if (gamepad == null)
        {
            return;
        }

        gamepadDesc = "\n<color=yellow>Gamepad</color>\n\n<color=yellow>DisplayName</color>\n"+gamepad.displayName;
        gamepadDesc += "\n\n<color=yellow>Device</color>\n" +gamepad.device;

        Vector2 leftDir = gamepad.leftStick.ReadValue();
        Vector2 rightDir = gamepad.rightStick.ReadValue();


        #region 会用到的方法
        //左摇杆按下抬起
        if (Gamepad.current.leftStickButton.wasPressedThisFrame)
        {

        }
        if (Gamepad.current.leftStickButton.wasReleasedThisFrame)
        {

        }
        if (Gamepad.current.leftStickButton.isPressed)
        {

        }
        //右摇杆按下抬起
        if (Gamepad.current.rightStickButton.wasPressedThisFrame)
        {

        }
        if (Gamepad.current.rightStickButton.wasReleasedThisFrame)
        {

        }
        if (Gamepad.current.rightStickButton.isPressed)
        {

        }

        if (Gamepad.current.dpad.left.wasPressedThisFrame)
        {

        }
        if (Gamepad.current.dpad.left.wasReleasedThisFrame)
        {

        }
        if (Gamepad.current.dpad.left.isPressed)
        {

        }
        //右侧三角方块/XYAB按键
        //Gamepad.current.buttonEast;
        //Gamepad.current.buttonWest;
        //Gamepad.current.buttonSouth;
        //Gamepad.current.buttonEast;
        if (Gamepad.current.buttonNorth.wasPressedThisFrame)
        {

        }
        if (Gamepad.current.buttonNorth.wasReleasedThisFrame)
        {

        }
        if (Gamepad.current.buttonNorth.isPressed)
        {

        }
        //手柄中央键
        if (Gamepad.current.startButton.wasPressedThisFrame)
        {

        }
        if (Gamepad.current.selectButton.wasPressedThisFrame)
        {

        }
        //肩键
        if (Gamepad.current.leftShoulder.wasPressedThisFrame)
        {

        }
        if (Gamepad.current.rightShoulder.wasPressedThisFrame)
        {

        }
        if (Gamepad.current.leftTrigger.wasPressedThisFrame)
        {

        }
        if (Gamepad.current.rightTrigger.wasPressedThisFrame)
        {

        }

        #endregion

        but_Gamepad_RightStrick.color= gamepad.rightStick.IsPressed()? PressColor:NormalColor;
        but_Gamepad_LeftStrick.color = gamepad.leftStick.IsPressed() ? PressColor : NormalColor;
        but_Gamepad_dpad.color = gamepad.dpad.IsPressed() ? PressColor : NormalColor;
        but_Gamepad_ButNorth.color = gamepad.buttonNorth.IsPressed() ? PressColor : NormalColor;
        but_Gamepad_ButSouth.color = gamepad.buttonSouth.IsPressed() ? PressColor : NormalColor;
        but_Gamepad_ButWest.color = gamepad.buttonWest.IsPressed() ? PressColor : NormalColor;
        but_Gamepad_ButEast.color = gamepad.buttonEast.IsPressed() ? PressColor : NormalColor;
        but_Gamepad_LeftShoulder.color = gamepad.leftShoulder.IsPressed() ? PressColor : NormalColor;
        but_Gamepad_RightShoulder.color = gamepad.rightShoulder.IsPressed() ? PressColor : NormalColor;
        but_Gamepad_LeftTrigger.color = gamepad.leftTrigger.IsPressed() ? PressColor : NormalColor;
        but_Gamepad_RightTrigger.color = gamepad.rightTrigger.IsPressed() ? PressColor : NormalColor;
        but_Gamepad_Start.color = gamepad.startButton.IsPressed() ? PressColor : NormalColor;
        but_Gamepad_Select.color = gamepad.selectButton.IsPressed() ? PressColor : NormalColor;

        //按下后变小
        var v = new Vector3(StrickPressSize, StrickPressSize, StrickPressSize);
        but_Gamepad_RightStrick.transform.parent.localScale = gamepad.rightStickButton.IsPressed() ? v : Vector3.one;
        but_Gamepad_LeftStrick.transform.parent.localScale = gamepad.leftStickButton.IsPressed() ? v : Vector3.one;

        but_Gamepad_RightStrick.transform.localPosition = gamepad.rightStick.ReadValue() * Strickdistacne;
        but_Gamepad_LeftStrick.transform.localPosition = gamepad.leftStick.ReadValue() * Strickdistacne;

        var dpadimage = but_Gamepad_dpad.GetComponent<Image>();
        if (gamepad.dpad.IsPressed())
        {
            
            var pv2 = gamepad.dpad.ReadValue();
            //print(pv2);

            if (pv2.x >0) {
                dpadimage.sprite = dpadIcon[4];
            }
            if (pv2.x < 0)
            {
                dpadimage.sprite = dpadIcon[3];
            }
            if (pv2.y<0)
            {
                dpadimage.sprite = dpadIcon[2];

            }
            if (pv2.y>0)
            {
                dpadimage.sprite = dpadIcon[1];
            }

        }
        else
        {
            dpadimage.sprite = dpadIcon[0];
        }





        textGamepad.text=gamepadDesc;
    }


 
    public Image but_Gamepad_RightStrick, but_Gamepad_LeftStrick, 
        but_Gamepad_dpad,
        but_Gamepad_ButNorth,
        but_Gamepad_ButSouth,
        but_Gamepad_ButWest,
        but_Gamepad_ButEast,
        but_Gamepad_LeftShoulder,
        but_Gamepad_RightShoulder,
        but_Gamepad_LeftTrigger,
        but_Gamepad_RightTrigger,
        but_Gamepad_Start,
        but_Gamepad_Select,
        but_Gamepad_LeftStrickBut,
        but_Gamepad_RightStrickBut

        ;

    public TMP_Text textGamepad;
    public float Strickdistacne = 30f;
    public float StrickPressSize = 0.7f;
    public List<Sprite> dpadIcon;

    public Color NormalColor, PressColor;



}
