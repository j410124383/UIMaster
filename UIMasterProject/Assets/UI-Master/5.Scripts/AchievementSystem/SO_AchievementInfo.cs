using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New AchievementInfo",menuName ="AchievementInfo")]
public class SO_AchievementInfo : ScriptableObject
{

    [SerializeField] public List<AchievementInfromation> AchievementList = new List<AchievementInfromation>();

}
