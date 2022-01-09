using System.Runtime.CompilerServices;
using UnityEngine;

public class ScenariosManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static ScenariosManager Instance;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Game Object Array References")]
	[SerializeField]
	private GameObject[] allDirectionHints;

	[Header("Script Array References")]
	[SerializeField]
	private SwitchController[] allSwitchControllers;

	#endregion

	#region PRIVATE VARIABLES

	private int scenario1SubLevelsTotal;
	private int scenario2SubLevelsTotal;
	private int scenario3SubLevelsTotal;
	private int scenario4SubLevelsTotal;

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

		AssignScenarioTotalLevels();
		HideAllDirectionHints();
		DeactivateAllSwitches();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void AssignScenarioTotalLevels()
	{
		scenario1SubLevelsTotal = scenario3SubLevelsTotal = scenario4SubLevelsTotal = 1;
		scenario2SubLevelsTotal = 2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void HideAllDirectionHints()
	{
		for (int i = 0; i < allDirectionHints.Length; i++)
		{
			allDirectionHints[i].SetActive(false);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void DeactivateAllSwitches()
	{
		for (int i = 0; i < allSwitchControllers.Length; i++)
		{
			allSwitchControllers[i].DeactivateSwitch();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void StartScenarioLevel()
	{
		ScenariosDialogueManager.Instance.HideAllComponents();

		DeactivateScenario();
		LoadScenarioBasedOnDepartment();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void LoadScenarioBasedOnDepartment()
	{
		switch (applicationManager.selectedDepartment)
		{
			case "Corporate & Investment Banking Group":
				if (applicationManager.cibScenarioCompleted == 1)
					applicationManager.scenario1SubLevelsCompleted = 0;

				if (applicationManager.scenario1SubLevelsCompleted == 0)
					EnableCurrentScenarioHints(0, "Go to the Manager's cabin,\non first floor extreme right corner");
				break;

			case "Personal Banking Group":
				if (applicationManager.pbgScenarioCompleted == 1)
					applicationManager.scenario2SubLevelsCompleted = 0;
				
				if (applicationManager.scenario2SubLevelsCompleted == 0)
					EnableCurrentScenarioHints(1, "Walk up to Ahmed\nsitting on the ground floor");
				else if (applicationManager.scenario2SubLevelsCompleted == 1)
					EnableCurrentScenarioHints(2, "Walk up to Nada\nsitting on the first floor");
				break;

			case "Control Functions":
				if (applicationManager.cfScenarioCompleted == 1)
					applicationManager.scenario3SubLevelsCompleted = 0;

				if (applicationManager.scenario3SubLevelsCompleted == 0)
					EnableCurrentScenarioHints(3, "Walk up to Rony\nsitting on the ground floor");
				break;

			case "Enablement Functions":
				if (applicationManager.enabScenarioCompleted == 1)
					applicationManager.scenario4SubLevelsCompleted = 0;

				if (applicationManager.scenario4SubLevelsCompleted == 0)
					EnableCurrentScenarioHints(4, "Walk up to Saba\nsitting on the first floor");
				break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void EnableCurrentScenarioHints(int id, string message)
	{
		allDirectionHints[id].SetActive(true);
		allSwitchControllers[id].ActivateSwitch();

		UIAnimationController[] animationControllers = allDirectionHints[id].GetComponentsInChildren<UIAnimationController>();
		for (int i = 0; i < animationControllers.Length; i++)
		{
			animationControllers[i].Initialize();
		}

		HUDManager.Instance.UpdateHUDLevelMessage(message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void DisableCurrentScenarioHints(int id)
	{
		UIAnimationController[] animationControllers = allDirectionHints[id].GetComponentsInChildren<UIAnimationController>();
		for (int i = 0; i < animationControllers.Length; i++)
		{
			animationControllers[i].Terminate();
		}

		allSwitchControllers[id].DeactivateSwitch();
		allDirectionHints[id].SetActive(false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ActivateScenario(GameObject switchInstance)
	{
		if (switchInstance.GetComponent<SwitchController>().switchID > -1)
		{
			ScenariosVideoManager.Instance.ConfigureVideoForScenario(switchInstance);
			DisableCurrentScenarioHints(switchInstance.GetComponent<SwitchController>().switchID);
		}
		
		CameraManager.Instance.ConfigureCameraForScenario();
		ControlsManager.Instance.ConfigureControlsForScenario();
		PlayerController.Instance.ConfigurePlayerForScenario(switchInstance);

		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WindowsEditor)
			Joystick.Instance.ResetAnchorPosition();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DeactivateScenario()
	{
		PlayerController.Instance.ConfigurePlayerForInteraction();
		CameraManager.Instance.ConfigureCameraForInteraction();
		ControlsManager.Instance.ConfigureControlsForInteraction();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void CheckScenarioStatus()
	{
		ScenariosVideoManager.Instance.StopScenarioVideo();
		HUDManager.Instance.ToggleHUDOnOff(true);
		ControlsManager.Instance.ToggleControlsOnOff(true);

		UpdateScenarioBasedOnDepartment();
		CheckScenarioLevelCompletion();
		
		ScenariosAPIManager.Instance.Scenarios("POST");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void UpdateScenarioBasedOnDepartment()
	{
		if (applicationManager.selectedDepartment == "Corporate & Investment Banking Group")
		{
			applicationManager.cibSwitchState = 1;
			applicationManager.scenario1SubLevelsCompleted++;

			ScenariosDialogueManager.Instance.ShowDialogues("player", "Thank you so much for taking out time to tell me about FAB's awesome customer service! This has been a great experience.", "afterQuiz", 0);

		}
		else if (applicationManager.selectedDepartment == "Personal Banking Group")
		{
			applicationManager.scenario2SubLevelsCompleted++;

			if (applicationManager.scenario2SubLevelsCompleted == scenario2SubLevelsTotal)
			{
				applicationManager.pbg2SwitchState = 1;

				ScenariosDialogueManager.Instance.ShowDialogues("player", "Thank you so much for taking out time to tell me about FAB's awesome customer service! This has been a great experience.", "afterQuiz", 2);
			}
			else
			{
				applicationManager.pbg1SwitchState = 1;

				ScenariosDialogueManager.Instance.ShowDialogues("player", "Wow! This seems to be heading towards a FAB moment. Thank you for your time.", "afterQuiz", 1);

				EnableCurrentScenarioHints(2, "Walk up to Nada\nsitting on the first floor");
			}
		}
		else if (applicationManager.selectedDepartment == "Control Functions")
		{
			applicationManager.cfSwitchState = 1;
			applicationManager.scenario3SubLevelsCompleted++;

			ScenariosDialogueManager.Instance.ShowDialogues("player", "Thank you so much for taking out time to tell me about FAB's awesome customer service! This has been a great experience.", "afterQuiz", 0);

		}
		else if (applicationManager.selectedDepartment == "Enablement Functions")
		{
			applicationManager.enabSwitchState = 1;
			applicationManager.scenario4SubLevelsCompleted++;

			ScenariosDialogueManager.Instance.ShowDialogues("player", "Thank you so much for taking out time to tell me about FAB's awesome customer service! This has been a great experience.", "afterQuiz", 0);

		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckScenarioLevelCompletion()
	{
		if (applicationManager.scenario1SubLevelsCompleted == scenario1SubLevelsTotal)
			applicationManager.cibScenarioCompleted = applicationManager.pbgScenarioUnlocked = applicationManager.cfScenarioUnlocked = applicationManager.enabScenarioUnlocked = 1;

		if (applicationManager.scenario2SubLevelsCompleted == scenario2SubLevelsTotal)
			applicationManager.pbgScenarioCompleted = applicationManager.cfScenarioUnlocked = applicationManager.enabScenarioUnlocked = applicationManager.cibScenarioUnlocked = 1;

		if (applicationManager.scenario3SubLevelsCompleted == scenario3SubLevelsTotal)
			applicationManager.cfScenarioCompleted = applicationManager.enabScenarioUnlocked = applicationManager.cibScenarioUnlocked = applicationManager.pbgScenarioUnlocked = 1;

		if (applicationManager.scenario4SubLevelsCompleted == scenario4SubLevelsTotal)
			applicationManager.enabScenarioCompleted = applicationManager.cibScenarioUnlocked = applicationManager.pbgScenarioUnlocked = applicationManager.cfScenarioUnlocked = 1;
	}

	#endregion

}
