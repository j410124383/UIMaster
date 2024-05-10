using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Example Code Only
/// </summary>
public class ExampleScript : MonoBehaviour
{
    public Button UnlockGoalExample;
    public Button Progress_1, Progress_5, Progress_10, Progress_100;
    public Button Overlay1Example;
    public Button Overlay2_1, Overlay2_5, Overlay2_10, Overlay2_100;
    public Button UnlockSpoilerExample;
    public Button ResetAC;

    private UIM_AchievenManager AM;

    private void Start()
    {
        AM = UIM_AchievenManager.instance;
        UnlockGoalExample.onClick.AddListener(delegate { AM.Unlock("GoalExample"); });
        Progress_1.onClick.AddListener(delegate { AM.AddAchievementProgress("ProgressionExample", 1); });
        Progress_5.onClick.AddListener(delegate { AM.AddAchievementProgress("ProgressionExample", 5); });
        Progress_10.onClick.AddListener(delegate { AM.AddAchievementProgress("ProgressionExample", 10); });
        Progress_100.onClick.AddListener(delegate { AM.AddAchievementProgress("ProgressionExample", 100); });

        Overlay1Example.onClick.AddListener(delegate { AM.Unlock("OverlayExample1"); }) ;

        Overlay2_1.onClick.AddListener(delegate { AM.AddAchievementProgress("OverlayExample2", 1); });
        Overlay2_5.onClick.AddListener(delegate { AM.AddAchievementProgress("OverlayExample2", 5); });
        Overlay2_10.onClick.AddListener(delegate { AM.AddAchievementProgress("OverlayExample2", 10); });
        Overlay2_100.onClick.AddListener(delegate { AM.AddAchievementProgress("OverlayExample2", 100); });

        UnlockSpoilerExample.onClick.AddListener(delegate { AM.Unlock("SpoilerExample"); });

        ResetAC.onClick.AddListener(AM.ResetAchievementState);
    }


}
