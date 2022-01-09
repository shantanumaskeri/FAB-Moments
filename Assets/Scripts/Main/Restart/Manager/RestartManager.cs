using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class RestartManager : MonoBehaviour
{

    #region SINGLETON INSTANCE

    public static RestartManager Instance;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Button References")]
	[SerializeField]
	private Button restartGameButton;

	#endregion

	#region PRIVATE VARIABLES

	private ApplicationManager applicationManager;

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
		applicationManager = FindObjectOfType<ApplicationManager>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void RestartGame()
    {
		restartGameButton.interactable = false;

		LeaderboardManager.Instance.ToggleLoadingSpinnerOnOff(true);

		//UpdateBackendAPIs();

		InvokeRepeating(nameof(CheckAllAPIDeletedToChangeScene), 0.1f, 0.1f);
	}

	/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void UpdateBackendAPIs()
	{
		//ValuesAPIManager.Instance.Values("DELETE");
		//BehavioursAPIManager.Instance.Behaviours("DELETE");
		//ScenariosAPIManager.Instance.Scenarios("DELETE");
		//CustomerFirstAPIManager.Instance.CustomerFirst("DELETE");
	}*/

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckAllAPIDeletedToChangeScene()
	{
		if (applicationManager.apisDeleted == 0)
		{
			CancelInvoke(nameof(CheckAllAPIDeletedToChangeScene));

			applicationManager.Reset();

			LeaderboardManager.Instance.ToggleLoadingSpinnerOnOff(false);

			ScenesManager.Instance.ChangeSceneManual();
		}
	}

	#endregion

}
