using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class Script_SceneManager : MonoBehaviour
{
    public static Script_SceneManager SM;
    public const string TitleScene = "Title";
    public const string GameScene = "Game";
    [SerializeField] private Transform LoadingScreen;

    /// <summary>
    /// Singleton needs to be in Awake because there are multiple 
    /// of this, don't want to have to set up by both Start and Game
    /// </summary>
    public void Awake()
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
    
    /// <summary>
    /// Will erase all current Run data and start at the beginning of the Run
    /// </summary>
    public static void RestartGame()
    {
        SceneManager.LoadScene(GameScene);
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

    public void Setup()
    {
        
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_SceneManager))]
public class Script_SceneManagerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_SceneManager t = (Script_SceneManager)target;
        if (GUILayout.Button("Restart Game"))
        {
            Script_SceneManager.RestartGame();
        }
    }
}
#endif