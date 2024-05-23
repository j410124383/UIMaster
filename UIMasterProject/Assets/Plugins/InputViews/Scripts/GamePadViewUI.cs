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
            print("�ո��");
        }
        if (Keyboard.current.dKey.wasReleasedThisFrame)
        {
            print("d��̧��");
        }
        if (Keyboard.current.spaceKey.isPressed)
        {
            print("�ո���");
        }
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
           
        }

    }

    public void CheckMouse()
    {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                print("����Ҽ�����");
            }
            if (Mouse.current.middleButton.wasPressedThisFrame)
            {
                print("����н�����");
            }
            if (Mouse.current.forwardButton.wasPressedThisFrame)
            {
                print("���ǰ������");
            }
            if (Mouse.current.backButton.wasPressedThisFrame)
            {
                print("���������");
            }
            //��Ļ����(���½�Ϊ��0,0)
            print(Mouse.current.position.ReadValue());
            //��֮֡���ƫ��
            print(Mouse.current.delta.ReadValue());
            //����
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
                print("����");
            }
            if (tc.press.wasReleasedThisFrame)
            {
                print("̧��");
            }
            if (tc.press.isPressed)
            {
                print("��ס");
            }
            if (tc.tap.isPressed)
            {

            }
            //������� 
            print(tc.tapCount);
            //���λ��
            print(tc.position.ReadValue());
            //��һ�νӴ�λ��
            print(tc.startPosition.ReadValue());
            //�õ��ķ�Χ
            print(tc.radius.ReadValue());
            //ƫ��λ��
            print(tc.delta.ReadValue());
            //����TouchPhase: None,Began,Moved,Ended,Canceled,Stationary
            print(tc.phase.ReadValue());

            //�ж�״̬
            UnityEngine.InputSystem.TouchPhase tp = tc.phase.ReadValue();
            switch (tp)
            {
                //��
                case UnityEngine.InputSystem.TouchPhase.None:
                    break;
                //��ʼ�Ӵ�
                case UnityEngine.InputSystem.TouchPhase.Began:
                    break;
                //�ƶ�
                case UnityEngine.InputSystem.TouchPhase.Moved:
                    break;
                //����
                case UnityEngine.InputSystem.TouchPhase.Ended:
                    break;
                //ȡ��
                case UnityEngine.InputSystem.TouchPhase.Canceled:
                    break;
                //��ֹ
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


        #region ���õ��ķ���
        //��ҡ�˰���̧��
        if (Gamepad.current.leftStickButton.wasPressedThisFrame)
        {

        }
        if (Gamepad.current.leftStickButton.wasReleasedThisFrame)
        {

        }
        if (Gamepad.current.leftStickButton.isPressed)
        {

        }
        //��ҡ�˰���̧��
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
        //�Ҳ����Ƿ���/XYAB����
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
        //�ֱ������
        if (Gamepad.current.startButton.wasPressedThisFrame)
        {

        }
        if (Gamepad.current.selectButton.wasPressedThisFrame)
        {

        }
        //���
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

        //���º��С
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
