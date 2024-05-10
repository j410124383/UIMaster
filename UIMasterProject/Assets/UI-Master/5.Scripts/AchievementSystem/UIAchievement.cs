using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// Defines the logic behind a single achievement on the UI
/// </summary>
public class UIAchievement : MonoBehaviour
{
    public TMP_Text Title, Description, Percent;
    public Image Icon, OverlayIcon, ProgressBar;
    public GameObject SpoilerOverlay;
    public TMP_Text SpoilerText;
    [NonSerialized] public AchievenmentStack AS;

    /// <summary>
    /// Destroy object after a certain amount of time
    /// </summary>
    public void StartDeathTimer ()
    {
        StartCoroutine(Wait());
    }

    /// <summary>
    /// Add information  about an Achievement to the UI elements显示弹窗属性
    /// </summary>
    public void SetDisplay (AchievementInfromation Information)
    {
        //print(Information.DisplayName);
        var State = Information.State;
        var AM = UIM_AchievenManager.instance;

        if (Information.Spoiler && !Information.State.Achieved)
        {
            SpoilerOverlay.SetActive(true);
            SpoilerText.text = AM.SpoilerAchievementMessage;
        }
        else
        {
            //print(Title.name);
            Title.text = Information.DisplayName;
            Description.text = Information.Description;

            if (Information.LockOverlay && !Information.State.Achieved)
            {
                OverlayIcon.gameObject.SetActive(true);
                OverlayIcon.sprite = Information.LockedIcon;
                Icon.sprite = Information.AchievedIcon;
            }
            else
            {
                Icon.sprite = State.Achieved ? Information.AchievedIcon : Information.LockedIcon;
            }

            if (Information.Progression)
            {
                float CurrentProgress = AM.ShowExactProgress ? State.Progress : (State.LastProgressUpdate * Information.NotificationFrequency);
                float DisplayProgress = State.Achieved ? Information.ProgressGoal : CurrentProgress;

                var str1 = Information.ProgressGoal + Information.ProgressSuffix;
                var str2 = Information.ProgressGoal + Information.ProgressSuffix;
                var str3 = State.Achieved ? "(Achieved)" : null;
                Percent.text = string.Format("{0}/{1} {2}", str1, str2, str3);

                ProgressBar.fillAmount = DisplayProgress / Information.ProgressGoal;
            }
            else //Single Time
            {
                ProgressBar.fillAmount = State.Achieved ? 1 : 0;
                Percent.text = State.Achieved ? "(Achieved)" : "(Locked)";
            }
        }
    }

    private IEnumerator Wait ()
    {
        yield return new WaitForSeconds(UIM_AchievenManager.instance.DisplayTime);
        GetComponent<Animator>().SetBool("ISSWITCH",true);
        yield return new WaitForSeconds(0.1f);
        AS.CheckBackLog();
        Destroy(gameObject);
    }
}
