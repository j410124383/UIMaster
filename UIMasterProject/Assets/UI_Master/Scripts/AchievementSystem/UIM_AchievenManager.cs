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

    [Tooltip("The message which will be displayed on the UI if an achievement is marked as a spoiler.")]
    public string SpoilerAchievementMessage = "Hidden";


    [Tooltip("创建成就系统的列表，改为so引用")]
    public SO_AchievementInfo so_AchievementInfo;
    [NonSerialized]public List<AchievementInfromation> AchievementList ;
    [NonSerialized]public List<AchieveState> AchieveStateList;

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

        Load();
    }

    private void Start()
    {
        Stack = AchievenmentStack.instance;
        AchievementList = so_AchievementInfo.AchievementList;
        AchieveStateList = so_AchievementInfo.so_AchieveStates.achieveStateList;
    }

    #region Svae and Load
    void Save()
    {
        //先将状态转变为so文件
        so_AchievementInfo.PushData();
        //再将so文件存储为json
        UIM_SaveLoad.SaveData(so_AchievementInfo.so_AchieveStates, "AchievementData");
    }

    void Load()
    {
        so_AchievementInfo.PullData();
        UIM_SaveLoad.LoadData(so_AchievementInfo, "AchievementData");
    }

    #endregion


    #region Miscellaneous
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
        int Count = (from AchieveState i in AchievementList
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
            Save();

            if (UseFinalAchievement)
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
                Save();
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
                Save();
            }
        }
    }
    #endregion


    /// <summary>
    /// Clears all saved progress and achieved states.
    /// </summary>
    public void ResetAchievementState()
    {
        //初始化各个成就的状态
        for (int i = 0; i < AchievementList.Count; i++)
        {
            AchievementList[i].State.Reset();
        }
        Save();
       
    }
  

    /// <summary>
    /// Find the index of an achievement with a cetain key
    /// </summary>
    /// <param name="Key">Key of achievevment</param>
    private int FindAchievementIndex(string Key)
    {
        return AchievementList.FindIndex(x => x.Key.Equals(Key));
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