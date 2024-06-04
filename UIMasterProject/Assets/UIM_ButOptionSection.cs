using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIM_ButOptionSection : MonoBehaviour
{
    private UIM_ButTwoHead but;

    private void Awake()
    {
        but = GetComponentInChildren<UIM_ButTwoHead>();
        GetComponent<Button>().onClick.AddListener(delegate { but.ChangeValue(1,true); });
    }


}
