using ReadSpeaker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event Action OnPreSceneUnload;
    public event Action OnPostSceneLoad;

    [SerializeField]
    private float transitionTime = 0.5f;
    [SerializeField]
    private CanvasGroup fadeCanvasGroup;
    [SerializeField]
    private ProgressBar progressBar;

    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    private bool isFading = false;


    public enum SceneName
    {
        PERSISTENT,
        MAIN_MENU,
        GAMEPLAY,
    }

    private void Awake()
    {
        instance = this;

        // Persistent should be loaded already
        // No transition on game startup
        //StartCoroutine(LoadScene(SceneName.MAIN_MENU, true));

        #if !UNITY_EDITOR
            SceneManager.LoadScene((int)SceneName.MAIN_MENU, LoadSceneMode.Additive);
        #endif
    }

    private void Start()
    {
        StartCoroutine(LoadTTS());
        //SoundManager.instance.PlayTrack("track01");
    }

    private IEnumerator LoadTTS()
    {
        //TTS.Init();

        while (true)
        {
            List<TTSEngine> engines = TTS.GetInstalledEngines();
            if (engines == null || engines.Count == 0)
            {
                Debug.Log("Was null or 0");
            }
            else
            {

                Debug.Log($"Was not null or 0 {engines}");
                break;
            }
            yield return null;
        }
        yield return null;
    }

    // Start game
    public void LoadGame()
    {
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneName.GAMEPLAY, LoadSceneMode.Additive));
        StartCoroutine(GetLoadingProgress());
    }

    // Quit game
    public void QuitGame(SceneName currentScene)
    {
        FadeAndLoadScene(SceneName.MAIN_MENU, currentScene);
    }


    /// <summary>
    /// Loads scene with transitions.
    /// </summary>
    /// <param name="sceneName">The enum representing the scene name and order.</param>
    public void FadeAndLoadScene(SceneName newScene, SceneName oldScene)
    {
        if (!isFading)
            StartCoroutine(FadeToScene((int)newScene, (int)oldScene));
    }

    /// <summary>
    /// Load scene with fade transitions over several frames.
    /// </summary>
    /// <param name="sceneName">The enum representing the scene name and order.</param>
    /// <returns>Coroutine.</returns>
    private IEnumerator FadeToScene(int sceneIndex, int oldSceneIndex)
    {
        // Start fade
        yield return StartCoroutine(FadeScreen(1f));

        // Call any event actions
        if (OnPreSceneUnload != null)
            OnPreSceneUnload();

        // Unload current scene over multiple frames
        Debug.Log($"unloading scene with build index: {oldSceneIndex}");
        yield return SceneManager.UnloadSceneAsync(oldSceneIndex);

        // Load next scene over multiple frames
        yield return StartCoroutine(LoadScene(sceneIndex, true));

        // Call any event actions
        if (OnPostSceneLoad != null)
            OnPostSceneLoad();

        // Start reverse fade
        yield return StartCoroutine(FadeScreen(0f));
    }

    /// <summary>
    /// Load a scene  and activate it.
    /// </summary>
    /// <param name="sceneName">The enum representing the scene name and order.</param>
    /// <param name="additive">True for additive or false for single mode.</param>
    /// <returns>Coroutine.</returns>
    private IEnumerator LoadScene(int sceneIndex, bool additive)
    {
        if (additive)
        {
            yield return SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        }
        else
        {
            yield return SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        }

        Debug.Log($"loaded scene: {sceneIndex}");
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
    }

    /// <summary>
    /// Use canvas group's current alpha value to fade the screen towards target value.
    /// </summary>
    /// <param name="targetAlpha">The value to reach.</param>
    /// <returns>Coroutine.</returns>
    private IEnumerator FadeScreen(float targetAlpha)
    {
        isFading = true;

        float speed = Mathf.Abs(targetAlpha - fadeCanvasGroup.alpha) / transitionTime;
        float timer = transitionTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);

            //Debug.Log($"alpha: {fadeCanvasGroup.alpha}");

            yield return null;
        }

        isFading = false;
    }

    // Track loading progress for one or more scenes being loaded
    public IEnumerator GetLoadingProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                float loadingProgress = 0;
                foreach (AsyncOperation op in scenesLoading)
                {
                    loadingProgress += op.progress;
                }

                loadingProgress = (loadingProgress / scenesLoading.Count) * 100f;
                progressBar.current = Mathf.RoundToInt(loadingProgress);

                yield return new WaitForSeconds(0.5f);
                Debug.Log($"Progress is: {loadingProgress}");

                yield return null;
            }
        }
    }
}
