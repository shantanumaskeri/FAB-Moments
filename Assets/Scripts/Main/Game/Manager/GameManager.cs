using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static GameManager Instance;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Script Array References")]
	[SerializeField]
	private FABValueIconController[] fabValueIcons;
	[SerializeField]
	private NonValueIconController[] nonValueIcons;
	[SerializeField]
	private BehaviourIconController[] behaviourIcons;
	[SerializeField]
	private SwitchController[] switches;

	#endregion

	#region PRIVATE VARIABLES

	private int currentSceneIndex;

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
		currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		
		applicationManager = FindObjectOfType<ApplicationManager>();

		InvokeRepeating(nameof(CheckConnectionAndCallLevelAPI), 0.1f, 0.1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckConnectionAndCallLevelAPI()
	{
		if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			if (currentSceneIndex == 5 || currentSceneIndex == 8)
			{
				ValuesAPIManager.Instance.Values("GET");
				BehavioursAPIManager.Instance.Behaviours("GET");
			}

			if (currentSceneIndex == 8)
				ScenariosAPIManager.Instance.Scenarios("GET");

			CancelInvoke(nameof(CheckConnectionAndCallLevelAPI));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LoadGameStatus()
	{
		HUDManager.Instance.PrepareHUD();
		ProfileManager.Instance.SetProfileDetails();
		ScoreManager.Instance.ConfigureScore();
		
		if (currentSceneIndex == 5)
		{
			DoorController.Instance.PrepareDoor();
			TeleportController.Instance.PrepareTeleport();

			for (int i = 0; i < fabValueIcons.Length; i++)
			{
				fabValueIcons[i].PrepareIcons();
			}
			for (int i = 0; i < nonValueIcons.Length; i++)
			{
				nonValueIcons[i].PrepareIcons();
			}
			for (int i = 0; i < behaviourIcons.Length; i++)
			{
				behaviourIcons[i].PrepareIcons();
			}
		}

		if (currentSceneIndex == 8)
			for (int i = 0; i < switches.Length; i++)
			{
				switches[i].PrepareSwitch();
			}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SaveGameStatus()
	{
		if (applicationManager.valueIconsCollected == 5)
			applicationManager.valueLevelCompleted = 1;

		if (applicationManager.behaviourQuizAnswers == 5)
			applicationManager.behaviourLevelCompleted = 1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void CheckPlatformAndSceneToShowInstructions(int instructionID)
	{
		if (currentSceneIndex == 5)
			if (InstructionsManager.Instance.instructionID == instructionID)
				InstructionsManager.Instance.ShowNextInstruction();
	}

	#endregion

}
