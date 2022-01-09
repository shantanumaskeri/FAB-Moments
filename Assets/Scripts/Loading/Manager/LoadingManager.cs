using DG.Tweening;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{

    #region SINGLETON INSTANCE

    public static LoadingManager Instance;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

    [Header("Game Object References")]
	[SerializeField]
    private GameObject loadingScreen;

    [Header("Slider References")]
    [SerializeField]
    private Slider slider;

    [Header("Text References")]
    [SerializeField]
    private Text progressText;

    [Header("Image References")]
    [SerializeField]
    private Image blackFade;

	#endregion

	#region PRIVATE VARIABLES

	private int levelId;
    private int currentSceneIndex;
    private int totalScenes;

	#endregion

	#region UNITY MONOBEHAVIOURS

	private void Start()
	{
        Instance = this;

        Initialize();
    }

	#endregion

	#region CUSTOM METHODS

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Initialize()
	{
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        totalScenes = SceneManager.sceneCountInBuildSettings;

        blackFade.DOFade(0.0f, 1.0f).SetEase(Ease.Linear);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void FadeToLevel(int id)
    {
        levelId = id;

        blackFade.DOFade(1.0f, 1.0f).SetEase(Ease.Linear);

        Invoke(nameof(OnFadeComplete), 1.0f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnFadeComplete()
    {
        LoadLevel(currentSceneIndex + levelId);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex)); 
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            
            if (progress < 1f)
                progressText.text = progress * 100f + "%";
            else
            {
                progressText.text = "Loading. Please Wait...";

                operation.allowSceneActivation = true;
            }
            
            yield return null;
        }
    }

	#endregion

}
