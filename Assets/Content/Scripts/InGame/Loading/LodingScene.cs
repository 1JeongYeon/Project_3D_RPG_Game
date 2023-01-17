using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LodingScene : MonoBehaviour
{
    private static string nextScene;
    [SerializeField]
    private Image bar;
    private float loadingPercent;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene(nextScene);
    }
    private void Start()
    {
        loadingPercent = 0f;
        StartCoroutine(LoadScene());
    }
    private IEnumerator LoadScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("GameStartScene");
        op.allowSceneActivation = false; // 장면이 준비된 즉시 장면이 활성화된것 false

        float timer = 0f;
        while (!op.isDone) // isDone = 해당 동작이 완료되었는지를 나타냄
        {
            yield return null;

            if (op.progress < loadingPercent) // progress = 작업의 진행상태를 나타냄
            {
                bar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                bar.fillAmount = Mathf.Lerp(loadingPercent, 1f, timer);
                if(bar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
