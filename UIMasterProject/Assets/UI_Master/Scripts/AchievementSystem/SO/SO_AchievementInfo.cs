using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New AchievementInfo",menuName ="Achievement/Info")]
public class SO_AchievementInfo : ScriptableObject
{
    [Tooltip("��Ҫ�洢������״̬")]
    [SerializeField] public SO_AchieveStates so_AchieveStates;
    [Tooltip("����ʵ�йصĵ�ǰ״̬")]
    [SerializeField] public List<AchievementInfromation> AchievementList = new List<AchievementInfromation>();


    /// <summary>
    /// ��ǰ״̬������״̬
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
    /// ����״̬����ǰ״̬
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
