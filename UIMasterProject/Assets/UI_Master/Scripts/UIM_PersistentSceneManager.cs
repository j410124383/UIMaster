using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIM_PersistentSceneManager : MonoBehaviour
{
    private string scenceName = "========PersistentScene========";

    // 在游戏开始时创建永久场景并加载
    void Awake()
    {
        // 检查永久场景是否已经存在
        Scene persistentScene = SceneManager.GetSceneByName(scenceName);
        if (!persistentScene.isLoaded)
        {
            // 创建永久场景并加载
            persistentScene = SceneManager.CreateScene(scenceName);
            SceneManager.LoadScene(scenceName, LoadSceneMode.Additive);
        }

        // 将当前对象移到永久场景中
        SceneManager.MoveGameObjectToScene(gameObject, persistentScene);
    }



}
