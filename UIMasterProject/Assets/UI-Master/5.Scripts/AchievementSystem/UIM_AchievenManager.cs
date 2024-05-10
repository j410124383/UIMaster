using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Controls interactions with the Achievement System
/// </summary>
[System.Serializable]
public class UIM_AchievenManager : MonoBehaviour
{
    [Tooltip("The number of seconds an achievement will stay on the screen after being unlocked or progress is made.")]
    public float DisplayTime = 3;
    [Tooltip("The total number of achievements which can be on the screen at any one time.")]
    public int NumberOnScreen = 3;
    [Tooltip("If true, progress notifications will display their exact progress. If false it will show the closest bracket.")]
    public bool ShowExactProgress = false;
    [Tooltip("If true, achievement unlocks/progress update notifications will be displayed on the player's screen.")]
    public bool DisplayAchievements;

    [Tooltip("If true, the state of all achievements will be saved without any call to the manual save function (Recommended = true)")]
    public bool AutoSave;
    [Tooltip("The message which will be displayed on the UI if an achievement is marked as a spoiler.")]
    public string SpoilerAchievementMessage = "Hidden";

    private AudioSource AudioSource;

    //List of achievement states (achieved, progress and last notification)
    //[SerializeField] public List<AchievementState> States = new List<AchievementState>();

    [Tooltip("创建成就系统的列表，改为so引用")]
    public SO_AchievementInfo so_AchievementInfo;
    [NonSerialized]public List<AchievementInfromation> AchievementList ;  

    [Tooltip("If true, one achievement will be automatically unlocked once all others have been completed")]
    public bool UseFinalAchievement = false;
    [Tooltip("The key of the final achievement")]
    public string FinalAchievementKey;

    public static UIM_AchievenManager instance = null; //Singleton Instance
    private AchievenmentStack Stack;

    void Awake()
    {
       if (instance == null)
       {
            instance = this;
       }
       else if (instance != this)
       {
            Destroy(gameObject);
       }
        DontDestroyOnLoad(gameObject);
        AudioSource = gameObject.GetComponent<AudioSource>();
       
        LoadAchievementState();
    }

    private void Start()
    {
        Stack = AchievenmentStack.instance;
        AchievementList = so_AchievementInfo.AchievementList;
    }

    # region Miscellaneous
    /// <summary>
    /// Does an achievement exist in the list
    /// </summary>
    /// <param name="Key">The Key of the achievement to test</param>
    /// <returns>true : if exists. false : does not exist</returns>
    public bool AchievementExists(string Key)
    {
        return AchievementExists(AchievementList.FindIndex(x => x.Key.Equals(Key)));
    }
    /// <summary>
    /// Does an achievement exist in the list
    /// </summary>
    /// <param name="Index">The index of the achievement to test</param>
    /// <returns>true : if exists. false : does not exist</returns>
    public bool AchievementExists(int Index)
    {
        return Index <= AchievementList.Count && Index >= 0;
    }
    /// <summary>
    /// Returns the total number of achievements which have been unlocked.
    /// </summary>
    public int GetAchievedCount()
    {
        int Count = (from AchievementState i in AchievementList
                     where i.Achieved == true
                    select i).Count();
        return Count;
    }

    /// <summary>
    /// Returns the current percentage of unlocked achievements.
    /// </summary>
    public float GetAchievedPercentage()
    {
        if(AchievementList.Count == 0)
        {
            return 0;
        }
        return (float)GetAchievedCount() / AchievementList.Count * 100;
    }
    #endregion

    #region Unlock and Progress
    /// <summary>
    /// Fully unlocks a progression or goal achievement.
    /// </summary>
    /// <param name="Key">The Key of the achievement to be unlocked</param>
    public void Unlock(string Key)
    {
        Unlock(FindAchievementIndex(Key));
    }
    /// <summary>
    /// Fully unlocks a progression or goal achievement.
    /// </summary>
    /// <param name="Index">The index of the achievement to be unlocked</param>
    public void Unlock(int Index)
    {
        if (!AchievementList[Index].State.Achieved)
        {
            AchievementList[Index].State.Progress = AchievementList[Index].ProgressGoal;
            AchievementList[Index].State.Achieved = true;
            DisplayUnlock(Index);
            AutoSaveStates();

            if(UseFinalAchievement)
            {
                int Find = AchievementList.FindIndex(x => !x.State.Achieved);
                bool CompletedAll = (Find == -1 || AchievementList[Find].Key.Equals(FinalAchievementKey));
                if (CompletedAll)
                {
                    Unlock(FinalAchievementKey);
                }
            }
        }
    }
    /// <summary>
    /// Set the progress of an achievement to a specific value. 
    /// </summary>
    /// <param name="Key">The Key of the achievement</param>
    /// <param name="Progress">Set progress to this value</param>
    public void SetAchievementProgress(string Key, float Progress)
    {
        SetAchievementProgress(FindAchievementIndex(Key), Progress);
    }
    /// <summary>
    /// Set the progress of an achievement to a specific value. 
    /// </summary>
    /// <param name="Index">The index of the achievement</param>
    /// <param name="Progress">Set progress to this value</param>
    public void SetAchievementProgress(int Index, float Progress)
    {
        if(AchievementList[Index].Progression)
        {
            if (AchievementList[Index].State.Progress >= AchievementList[Index].ProgressGoal)
            {
                Unlock(Index);
            }
            else
            {
                AchievementList[Index].State.Progress = Progress;
                DisplayUnlock(Index);
                AutoSaveStates();                
            }
        }
    }
    /// <summary>
    /// Adds the input amount of progress to an achievement. Clamps achievement progress to its max value.
    /// </summary>
    /// <param name="Key">The Key of the achievement</param>
    /// <param name="Progress">Add this number to progress</param>
    public void AddAchievementProgress(string Key, float Progress)
    {
        AddAchievementProgress(FindAchievementIndex(Key), Progress);
    }
    /// <summary>
    /// Adds the input amount of progress to an achievement. Clamps achievement progress to its max value.
    /// </summary>
    /// <param name="Index">The index of the achievement</param>
    /// <param name="Progress">Add this number to progress</param>
    public void AddAchievementProgress(int Index, float Progress)
    {
        if (AchievementList[Index].Progression)
        {
            if (AchievementList[Index].State.Progress + Progress >= AchievementList[Index].ProgressGoal)
            {
                Unlock(Index);
            }
            else
            {
                AchievementList[Index].State.Progress += Progress;
                DisplayUnlock(Index);
                AutoSaveStates();
            }
        }
    }
    #endregion

    #region Saving and Loading
    /// <summary>
    /// 存储，后面会拜托使用playerprefs的方法
    /// </summary>
    public void SaveAchievementState()
    {
        for (int i = 0; i < AchievementList.Count; i++)
        {
            PlayerPrefs.SetString("AchievementState_" + i, JsonUtility.ToJson(AchievementList[i]));
        }
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Loads all progress and achievement states from player prefs. This function is automatically called if the Auto Load setting is set to true.
    /// </summary>
    public void LoadAchievementState()
    {
        AchievementState NewState;

        //for (int i = 0; i < AchievementList.Count; i++)
        //{
        //    //Ensure that new project get default values
        //    if (PlayerPrefs.HasKey("AchievementState_" + i))
        //    {
        //        NewState = JsonUtility.FromJson<AchievementState>(PlayerPrefs.GetString("AchievementState_" + i));
        //        AchievementList[i].State.Add(NewState);
        //    }
        //    else { AchievementList[i].State.Add(new AchievementState()); }

        //}
    }
    /// <summary>
    /// Clears all saved progress and achieved states.
    /// </summary>
    public void ResetAchievementState()
    {
        //初始化各个成就的状态

        //States.Clear();
        for (int i = 0; i < AchievementList.Count; i++)
        {
            PlayerPrefs.DeleteKey("AchievementState_" + i);
            AchievementList[i].State.Reset();
        }
        SaveAchievementState();
    }
    #endregion

    /// <summary>
    /// Find the index of an achievement with a cetain key
    /// </summary>
    /// <param name="Key">Key of achievevment</param>
    private int FindAchievementIndex(string Key)
    {
        return AchievementList.FindIndex(x => x.Key.Equals(Key));
    }
    /// <summary>
    /// Test if AutoSave is valid. If true, save list
    /// </summary>
    private void AutoSaveStates()
    {
        if (AutoSave)
        {
            SaveAchievementState();
        }
    }
    /// <summary>
    /// Display achievements progress to screen  
    /// </summary>
    /// <param name="Index">Index of achievement to display</param>
    private void DisplayUnlock(int Index)
    {
        var state = AchievementList[Index].State;
        if (DisplayAchievements && !AchievementList[Index].Spoiler || state.Achieved)
        {
            //If not achieved
            if (AchievementList[Index].Progression && state.Progress < AchievementList[Index].ProgressGoal)
            {
                int Steps = (int)AchievementList[Index].ProgressGoal / (int)AchievementList[Index].NotificationFrequency;

                //Loop through all notification point backwards from last possible option
                for (int i = Steps; i > state.LastProgressUpdate; i--)
                {
                   //When it finds the largest valid notification point
                   if (state.Progress >= AchievementList[Index].NotificationFrequency * i)
                   {
                        UIM_SoundManager.Play_SF("id_ACprogress");
                        state.LastProgressUpdate = i;
                        //print(Index);
                        Stack.ScheduleAchievementDisplay(Index);
                        
                        return;
                   }
                }
            }
            else
            {
                UIM_SoundManager.Play_SF("id_ACunlock");
                //print(Index);
                Stack.ScheduleAchievementDisplay(Index);
                
            }
        }
    }
}