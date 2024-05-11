using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New AchievementInfo",menuName ="Achievement/Info")]
public class SO_AchievementInfo : ScriptableObject
{
    [Tooltip("需要存储的数据状态")]
    [SerializeField] public SO_AchieveStates so_AchieveStates;
    [Tooltip("与现实有关的当前状态")]
    [SerializeField] public List<AchievementInfromation> AchievementList = new List<AchievementInfromation>();


    /// <summary>
    /// 当前状态→数据状态
    /// </summary>
    public void PushData()
    {
        so_AchieveStates.achieveStateList= new List<AchieveState>();
        var x = AchievementList;
        for (int i = 0; i < x.Count; i++)
        {
            so_AchieveStates.achieveStateList.Add(x[i].State);
        }

    }

    /// <summary>
    /// 数据状态→当前状态
    /// </summary>
    public  void PullData()
    {
        var x = so_AchieveStates.achieveStateList;
        for (int i = 0; i < x.Count; i++)
        {
            AchievementList[i].State.CopyState(x[i]);

        }
    }



}
