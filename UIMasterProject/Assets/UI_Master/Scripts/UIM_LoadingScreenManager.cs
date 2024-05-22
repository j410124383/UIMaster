using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UIM_LoadingScreenManager : MonoBehaviour
{
    public string targetSceneName; // 目标场景名称
    public Slider progressBar; // UI进度条

    void Start()
    {
        StartCoroutine(LoadTargetSceneAsync());
    }

    IEnumerator LoadTargetSceneAsync()
    {
        // 开始异步加载目标场景
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(targetSceneName);

        // 禁用自动场景切换
        asyncOperation.allowSceneActivation = false;

        // 更新UI进度条
        while (!asyncOperation.isDone)
        {
            // 进度条的值 [0, 1]
            progressBar.value = asyncOperation.progress;

            // 检查是否加载完成
            if (asyncOperation.progress >= 0.9f)
            {
                // 加载完成，允许场景切换
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
