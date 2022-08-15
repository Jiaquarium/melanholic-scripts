using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class Script_SceneManager : MonoBehaviour
{
    public static Script_SceneManager SM;
    public const string TitleScene = "Title";
    public const string GameScene = "Game";

    [Range(0, 1)] [SerializeField] private float loadingProgressBound0 = 0.15f;
    [Range(0, 1)] [SerializeField] private float loadingProgressBound1 = 0.40f;
    [Range(0, 1)] [SerializeField] private float loadingProgressBound2 = 0.65f;
    [SerializeField] private List<GameObject> loadingDuckMasks;

    [SerializeField] private float duckMaskRevealTime0;
    [SerializeField] private float duckMaskRevealTime1;

    [Header("Loading Screen Settings")]
    [SerializeField] private Script_CanvasGroupController LoadingScreen;
    [SerializeField] private Script_CanvasGroupController ducksCanvasFocused;
    [SerializeField] private Script_CanvasGroupController completeTextCanvasGroup;
    [SerializeField] private Script_CanvasGroupController loadingTextCanvasGroup;

    private Coroutine forceDucksActiveCo;

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
    /// 
    /// NOTE: TO BE CALLED BY GAME
    /// </summary>
    public static void RestartGame()
    {
        SceneManager.LoadScene(GameScene);
    }
    
    /// <summary>
    /// Go from Game to Title Scene.
    /// Change start state so Intro will not be played.
    /// </summary>
    public static void ToTitleScene()
    {
        Script_Start.startState = Script_Start.StartStates.BackToMainMenu;
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
        // Note: Must clean this up, in case Game Scene loads before this
        // coroutine can complete.
        SM.forceDucksActiveCo = SM.StartCoroutine(FakeDucksLoading());
        SM.StartCoroutine(LoadingScreenToGameAsync(GameScene));

        /// <summary>
        /// Arbitrarily show Duck Masks 0 and 1, to give some indication that progress is working.
        /// Nothing will happen if they are already shown, reflecting true progress.
        /// </summary>
        IEnumerator FakeDucksLoading()
        {
            yield return new WaitForSecondsRealtime(SM.duckMaskRevealTime0);

            SM.loadingDuckMasks[0].SetActive(true);

            yield return new WaitForSecondsRealtime(SM.duckMaskRevealTime1);

            SM.loadingDuckMasks[1].SetActive(true);
        }
    }

    private static IEnumerator LoadingScreenToGameAsync(string scene)
    {
        SM.LoadingScreen.Open();
        EventSystem.current.sendNavigationEvents = false;

        // Note: In Unity Editor, the loading screen will not show the same
        // frame as when running async load, so wait a frame
        yield return null;
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        // Don't let scene activate until instructed to
        asyncLoad.allowSceneActivation = false;
        bool isWaitingForSceneActivation = true;

        // isDone will be maintained as false as long as allowSceneActivation = false
        // https://docs.unity3d.com/ScriptReference/AsyncOperation-allowSceneActivation.html 
        while (!asyncLoad.isDone)
        {
            Debug.Log($"load progress: {asyncLoad.progress}");
            SM.HandleDuckMasksProgress(asyncLoad.progress);

            // Progress will be maintained at .9f as well until allowSceneActivation is set to true
            if (asyncLoad.progress >= 0.9f && isWaitingForSceneActivation)
            {
                isWaitingForSceneActivation = false;

                // Show loading complete state
                SM.loadingTextCanvasGroup.Close();
                SM.completeTextCanvasGroup.Open();

                SM.ducksCanvasFocused.FadeIn(FadeSpeeds.XFast.GetFadeTime(), () => {
                    SM.CleanUpLoadingCoroutines();
                    
                    // Activate the new scene
                    asyncLoad.allowSceneActivation = true;
                });
            }

            yield return null;
        }

        // Called after new Scene is loaded
        Debug.Log("End of LoadSceneAsync, Closing loading screen");
        SM.LoadingScreen.Close();
    }

    private void HandleDuckMasksProgress(float progress)
    {
        if (progress >= loadingProgressBound2)
            loadingDuckMasks.SetAllUntilIndex(2, true);
        else if (progress >= loadingProgressBound1)
            loadingDuckMasks.SetAllUntilIndex(1, true);
        else if (progress >= loadingProgressBound0)
            loadingDuckMasks.SetAllUntilIndex(0, true);
    }

    private void CleanUpLoadingCoroutines()
    {
        Debug.Log("Attempting to clean up forceDucksActiveCo");
        
        if (SM.forceDucksActiveCo != null)
        {
            Debug.Log("Cleaned up forceDucksActiveCo");
            
            StopCoroutine(SM.forceDucksActiveCo);
            SM.forceDucksActiveCo = null;
        }
    }

    public void InitialState()
    {
        loadingDuckMasks.ForEach(mask => mask.SetActive(false));
        LoadingScreen.Close();
        ducksCanvasFocused.Close();
        completeTextCanvasGroup.Close();
        loadingTextCanvasGroup.Open();
    }

    public void Setup()
    {
        InitialState();
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

            if (GUILayout.Button("Handle Ducks Progress 50%"))
            {
                t.HandleDuckMasksProgress(.5f);
            }
        }
    }
    #endif

}
