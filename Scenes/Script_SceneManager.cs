using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_SceneManager : MonoBehaviour
{
    public static Script_SceneManager SM;
    public static readonly string TitleScene = "Title";
    public static readonly string GameScene = "Game";
    [SerializeField] private Transform LoadingScreen;
    void Awake()
    {
        if (SM == null)
        {
            DontDestroyOnLoad(this.gameObject);
            SM = this;
        }
        else if (this != SM)    
        {
            Destroy(this.gameObject);
        }
    }

    public static void ToTitleScene()
    {
        Script_Start.startState = Script_Start.StartStates.Start;
        SceneManager.LoadScene(TitleScene);
    }

    public static void ToGameOver(Script_GameOverController.DeathTypes deathType)
    {
        Script_Start.startState     = Script_Start.StartStates.GameOver;
        Script_Start.deathType      = deathType;
        SceneManager.LoadScene(TitleScene);
    }

    public static void ToGameScene()
    {
        SM.StartCoroutine(LoadSceneAsync(GameScene));
    }

    private static IEnumerator LoadSceneAsync(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        SM.LoadingScreen.gameObject.SetActive(true);

        while (asyncLoad.progress < 1)
        {
            Debug.Log($"load progress: {asyncLoad.progress}");
            yield return null;
        }

        SM.LoadingScreen.gameObject.SetActive(false);
    }
}