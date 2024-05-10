using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Add list of achievements to screen
/// </summary>
public class AchievenmentListIngame : MonoBehaviour
{
    public GameObject scrollContent;
    public GameObject prefab;

    public Text CountText;
    public Text CompleteText;
    public Scrollbar Scrollbar;


    private void OnEnable()
    {
        AddAchievements();
    }

    /// <summary>
    /// Adds all achievements to the UI based on a filter
    /// </summary>
    /// <param name="Filter">Filter to use (All, Achieved or Unachieved)</param>
    private void AddAchievements()
    {  
        foreach (Transform child in scrollContent.transform)
        {
            Destroy(child.gameObject);
        }
        UIM_AchievenManager AM = UIM_AchievenManager.instance;

       // CountText.text = "" + AM.AchievementList.Count + " / " + AM.AchievementList.Count;
        //CompleteText.text = "Complete (" + AM.GetAchievedPercentage() + "%)";

        for (int i = 0; i < AM.AchievementList.Count; i ++)
        {
            var state = AM.AchievementList[i].State;
            AddAchievementToUI(AM.AchievementList[i]);
        }
        Scrollbar.value = 1;
    }

    public void AddAchievementToUI(AchievementInfromation Achievement)
    {
        UIAchievement UIAchievement = Instantiate(prefab).GetComponent<UIAchievement>();
        UIAchievement.Set(Achievement);
        UIAchievement.transform.SetParent(scrollContent.transform);
        UIAchievement.transform.localScale = Vector3.one;
    }




}