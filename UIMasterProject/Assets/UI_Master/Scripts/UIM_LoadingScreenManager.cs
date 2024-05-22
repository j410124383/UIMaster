using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UIM_LoadingScreenManager : MonoBehaviour
{
    public string targetSceneName; // Ŀ�곡������
    public Slider progressBar; // UI������

    void Start()
    {
        StartCoroutine(LoadTargetSceneAsync());
    }

    IEnumerator LoadTargetSceneAsync()
    {
        // ��ʼ�첽����Ŀ�곡��
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(targetSceneName);

        // �����Զ������л�
        asyncOperation.allowSceneActivation = false;

        // ����UI������
        while (!asyncOperation.isDone)
        {
            // ��������ֵ [0, 1]
            progressBar.value = asyncOperation.progress;

            // ����Ƿ�������
            if (asyncOperation.progress >= 0.9f)
            {
                // ������ɣ��������л�
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
