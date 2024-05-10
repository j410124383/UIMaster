using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the display of achievements on the screen
/// </summary>
public class AchievenmentStack : MonoBehaviour
{
    public static AchievenmentStack instance;
    public RectTransform StackPanels;
    public List<UIAchievement> BackLog = new List<UIAchievement>();

    public GameObject AchievementTemplate;
    private UIM_AchievenManager AM;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
      
        AM = UIM_AchievenManager.instance;
    }

    /// <summary>
    /// Add an achievement to screen if it fits, otherwise, add to the backlog list
    /// </summary>
    /// <param name="Index">Index of achievement to add</param>
    public void ScheduleAchievementDisplay (int Index)
    {
        var Spawned = Instantiate(AchievementTemplate).GetComponent<UIAchievement>();
        Spawned.AS = this;
        Spawned.Set(AM.AchievementList[Index]);
        
        //If there is room on the screen
        if (GetCurrentStack().childCount < AM.NumberOnScreen)
        {
            Spawned.transform.SetParent(GetCurrentStack(), false);
            Spawned.StartDeathTimer();
        }
        else
        {
            Spawned.gameObject.SetActive(false);
            BackLog.Add(Spawned);
        }
    }

    /// <summary>
    /// Find the box where achievements should be spawned
    /// </summary>
    public Transform GetCurrentStack () => StackPanels.transform;

    /// <summary>
    /// Add one achievement from the backlog to the screen
    /// </summary>
    public void CheckBackLog ()
    {
        if(BackLog.Count > 0)
        {
            BackLog[0].transform.SetParent(GetCurrentStack(), false);
            BackLog[0].gameObject.SetActive(true);
            BackLog[0].StartDeathTimer();
            BackLog.RemoveAt(0);
        }
    }
}
