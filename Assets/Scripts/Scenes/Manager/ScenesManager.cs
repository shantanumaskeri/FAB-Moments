using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{

    #region SINGLETON INSTANCE

    public static ScenesManager Instance;

    #endregion

    #region PUBLIC FIELDS

    [HideInInspector]
    public int currentSceneIndex;
    [HideInInspector]
    public int totalScenes;

    #endregion

    #region EDITOR ASSIGNED VARIABLES

    [Header("String References")]
    [SerializeField]
    private string sceneChangeMode;

    [Header("Floating Point Values")]
    [SerializeField]
    private float sceneChangeDelay;

	#endregion

	#region PRIVATE VARIABLES

	private GameObject applicationManager;

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

        CheckSceneLoadedIndex();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CheckSceneLoadedIndex()
	{
        switch (currentSceneIndex)
		{
            case 0:
                applicationManager = GameObject.Find("Application");
                DontDestroyOnLoad(applicationManager);

                SceneManager.LoadScene(currentSceneIndex + 1);
                break;

            default:
                CheckSceneChangeMode();
                break;
		}
	}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CheckSceneChangeMode()
	{
        switch (sceneChangeMode)
		{
            case "Automatic":
                StartCoroutine(ChangeSceneAuto());
                break;
		}
	}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IEnumerator ChangeSceneAuto()
	{
        yield return new WaitForSeconds(sceneChangeDelay);

        LoadingManager.Instance.FadeToLevel(1);
	}

    public void ChangeSceneManual()
	{
        switch (currentSceneIndex)
		{
            case 2:
                LaunchManager.Instance.CheckStatusAndLoadNextScene();
                break;

            case 3:
                LoginManager.Instance.CheckStatusAndLoadNextScene();
                break;

            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
                LoadingManager.Instance.FadeToLevel(1);
                break;

            case 9:
                LoadingManager.Instance.FadeToLevel(-2);
                break;
        }
    }

	#endregion

}
