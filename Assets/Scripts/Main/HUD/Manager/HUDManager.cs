using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static HUDManager Instance;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Game Object References")]
	[SerializeField]
	private GameObject hudValues;
	[SerializeField]
	private GameObject hudBehaviours;
	[SerializeField]
	private GameObject information;
	[SerializeField]
	private GameObject hudCanvas;

	[Header("Text References")]
	[SerializeField]
	private Text playerNameHUD;
	[SerializeField]
	private Text levelText;

	[Header("Game Object Array References")]
	[SerializeField]
	private GameObject[] hudIcons;

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
		
		CheckPlatformToShowInstructionsUI();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckPlatformToShowInstructionsUI()
	{
		if (currentSceneIndex == 8)
			information.SetActive(false);
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PrepareHUD()
	{
		CheckHUDState();
		SetHUDPlayerName();
		UpdateHUDBanner();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetHUDPlayerName()
	{
		playerNameHUD.text = applicationManager.playerFirstName + " " + applicationManager.playerLastName;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckHUDState()
	{
		if (currentSceneIndex == 5)
		{
			if (applicationManager.valueLevelCompleted == 0 && applicationManager.behaviourLevelCompleted == 0)
				UpdateHUDLevelMessage("Find the 5 FAB values");

			if (applicationManager.valueLevelCompleted == 1 && applicationManager.behaviourLevelCompleted == 0)
			{
				if (applicationManager.doorState == 1)
					UpdateHUDLevelMessage("Select the correct sentences\nassociated to FAB\nCustomer First Behaviours");
				else
					UpdateHUDLevelMessage("Walk towards the Door");
			}

			if (applicationManager.valueLevelCompleted == 1 && applicationManager.behaviourLevelCompleted == 1)
				UpdateHUDLevelMessage("Stand on the Teleport\nto go to the next stage");
		}
		
		if (currentSceneIndex == 8)
			if (applicationManager.valueLevelCompleted == 1 && applicationManager.behaviourLevelCompleted == 1)
				UpdateHUDLevelMessage("Stand on the Teleport\nin front of you");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ShowHUDIcons(Image icon)
	{
		icon.DOFade(1.0f, 2.0f).SetEase(Ease.OutBack);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void UpdateHUDIcons(Image icon)
	{
		icon.color = new Color(1f, 1f, 1f, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void UpdateHUDLevelMessage(string value)
	{
		levelText.text = value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void UpdateHUDBanner()
	{
		if (applicationManager.valueLevelCompleted == 0 && applicationManager.behaviourLevelCompleted == 0)
			ShowValueBanner();

		if (applicationManager.valueLevelCompleted == 1 && applicationManager.behaviourLevelCompleted == 0)
		{
			if (applicationManager.doorState == 1)
				ShowOtherBanner();
			else
				ShowValueBanner();
		}

		if (applicationManager.valueLevelCompleted == 1 && applicationManager.behaviourLevelCompleted == 1)
			ShowOtherBanner();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ShowValueBanner()
	{
		hudValues.SetActive(true);
		hudBehaviours.SetActive(false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ShowOtherBanner()
	{
		hudValues.SetActive(false);
		hudBehaviours.SetActive(true);

		for (int i = 0; i < hudIcons.Length; i++)
		{
			hudIcons[i].SetActive(false);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ToggleHUDOnOff(bool value)
	{
		hudCanvas.SetActive(value);
	}

	#endregion

}
