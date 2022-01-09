using System.Runtime.CompilerServices;
using UnityEngine;

public class InstructionsManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static InstructionsManager Instance;

	#endregion

	#region PUBLIC FIELDS

	[HideInInspector]
	public int instructionID;
	[HideInInspector]
	public bool isShowingInstructions;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Game Object References")]
	[SerializeField]
	private GameObject instructionsPC;
	[SerializeField]
	private GameObject instructionsMobile;

	[Header("Rect Transform References")]
	[SerializeField]
	private RectTransform joystickInstructions;
	[SerializeField]
	private RectTransform touchCameraInstructions;
	[SerializeField]
	private RectTransform cameraToggleInstructions;
	[SerializeField]
	private RectTransform valueIconInstructions;
	[SerializeField]
	private RectTransform behaviourQuizInstructions;
	[SerializeField]
	private RectTransform touchCameraScreenField;

	[Header("Canvas References")]
	[SerializeField]
	private Canvas joystickCanvas;
	[SerializeField]
	private Canvas touchCameraCanvas;
	[SerializeField]
	private Canvas cameraToggleCanvas;
	[SerializeField]
	private Canvas instructionsCanvas;

	[Header("Game Object Array References")]
	[SerializeField]
	private GameObject[] mobileInstructionsList;
	[SerializeField]
	private GameObject[] pcInstructionsList;

	[Header("Script Array References")]
	[SerializeField]
	private UIAnimationController[] mobileInstructionAnimationControllers;
	[SerializeField]
	private UIAnimationController[] pcInstructionAnimationControllers;

	#endregion

	#region PRIVATE VARIABLES

	private float instructionsHeight;

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
		isShowingInstructions = false;

		applicationManager = FindObjectOfType<ApplicationManager>();

		instructionsHeight = UICanvasScalerManager.Instance.canvasHeight;

		CheckForInstructionsInScene();
		CheckPlatformToShowInstructions();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckForInstructionsInScene()
	{
		joystickInstructions.sizeDelta = new Vector2(0f, instructionsHeight);
		touchCameraInstructions.sizeDelta = new Vector2(0f, instructionsHeight);
		cameraToggleInstructions.sizeDelta = new Vector2(0f, instructionsHeight);
		valueIconInstructions.sizeDelta = new Vector2(0f, instructionsHeight);
		behaviourQuizInstructions.sizeDelta = new Vector2(0f, instructionsHeight);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckPlatformToShowInstructions()
	{
		if (applicationManager.gameInstructionsViewed == 0)
		{
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WindowsEditor)
			{
				instructionsPC.SetActive(false);
				instructionsMobile.SetActive(true);
			}
			else
			{
				instructionsPC.SetActive(true);
				instructionsMobile.SetActive(false);
			}

			ToggleInstructionsOnOff();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ToggleInstructionsOnOff()
	{
		isShowingInstructions = !isShowingInstructions;

		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WindowsEditor)
		{
			mobileInstructionsList[instructionID].SetActive(isShowingInstructions);

			if (isShowingInstructions)
				mobileInstructionAnimationControllers[instructionID].Initialize();
			else
				mobileInstructionAnimationControllers[instructionID].Terminate();

			ReorderSceneCanvases();
		}
		else
		{
			pcInstructionsList[instructionID].SetActive(isShowingInstructions);

			if (isShowingInstructions)
				pcInstructionAnimationControllers[instructionID].Initialize();
			else
				pcInstructionAnimationControllers[instructionID].Terminate();
		}

		if (isShowingInstructions)
			touchCameraScreenField.offsetMax = new Vector2(-300, 0);
		else
			touchCameraScreenField.offsetMax = new Vector2(0, 0);

		SaveAndResetInstructions();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ShowNextInstruction()
	{
		if (isShowingInstructions)
		{
			instructionID++;
			
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WindowsEditor)
			{
				mobileInstructionsList[instructionID].SetActive(true);
				mobileInstructionsList[instructionID - 1].SetActive(false);

				mobileInstructionAnimationControllers[instructionID].Initialize();
				mobileInstructionAnimationControllers[instructionID - 1].Terminate();

				ReorderSceneCanvases();
			}
			else
			{
				pcInstructionsList[instructionID].SetActive(true);
				pcInstructionsList[instructionID - 1].SetActive(false);

				pcInstructionAnimationControllers[instructionID].Initialize();
				pcInstructionAnimationControllers[instructionID - 1].Terminate();
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SaveAndResetInstructions()
	{
		if (instructionID == (mobileInstructionsList.Length - 1))
		{
			applicationManager.gameInstructionsViewed = 1;
			PlayerPrefs.SetInt("GameInstructionsViewed", applicationManager.gameInstructionsViewed);
		}

		instructionID = 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ReorderSceneCanvases()
	{
		if (instructionID == 0)
		{
			joystickCanvas.sortingOrder = instructionsCanvas.sortingOrder + 1;
		}
		else if (instructionID == 1)
		{
			joystickCanvas.sortingOrder = 3;
			touchCameraCanvas.sortingOrder = instructionsCanvas.sortingOrder + 1;
		}
		else if (instructionID == 2)
		{
			touchCameraCanvas.sortingOrder = 0;
			cameraToggleCanvas.sortingOrder = instructionsCanvas.sortingOrder + 1;
		}
		else if (instructionID == 3)
		{
			cameraToggleCanvas.sortingOrder = 2;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SkipInstructions()
	{
		isShowingInstructions = false;

		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WindowsEditor)
		{
			touchCameraCanvas.sortingOrder = 0;
			cameraToggleCanvas.sortingOrder = 2;
			joystickCanvas.sortingOrder = 3;
			instructionsCanvas.sortingOrder = 4;

			mobileInstructionsList[instructionID].SetActive(isShowingInstructions);
		}
		else
		{
			pcInstructionsList[instructionID].SetActive(isShowingInstructions);
		}
		
		instructionID = 0;

		touchCameraScreenField.offsetMax = new Vector2(0, 0);
	}

	#endregion

}
