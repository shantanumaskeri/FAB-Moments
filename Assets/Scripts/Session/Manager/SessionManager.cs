using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour
{

	#region PRIVATE VARIABLES

	private int currentSceneIndex;
	private int totalScenes;

	private ApplicationManager applicationManager;

	#endregion

	#region UNITY MONOBEHAVIOURS

	private void Start()
	{
		Initialize();
	}

	#endregion

	#region CUSTOM METHODS

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize()
	{
		currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		totalScenes = SceneManager.sceneCountInBuildSettings;

		applicationManager = FindObjectOfType<ApplicationManager>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ConfigureExit()
	{
		CheckSceneAndResetGameData();
		StartCoroutine(ExitApplication());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckSceneAndResetGameData()
	{
		if (currentSceneIndex == (totalScenes - 1))
		{
			applicationManager.Reset();

			//ValuesAPIManager.Instance.Values("DELETE");
			//BehavioursAPIManager.Instance.Behaviours("DELETE");
			//ScenariosAPIManager.Instance.Scenarios("DELETE");
			//CustomerFirstAPIManager.Instance.CustomerFirst("DELETE");
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerator ExitApplication()
    {
		yield return new WaitForSeconds(1f);

		applicationManager.SetTimeOfApplicationTermination();
		
		Application.Quit();
    }

	#endregion

}
