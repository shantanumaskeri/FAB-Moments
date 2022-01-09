using System.Runtime.CompilerServices;
using UnityEngine;

public class CustomerFirstInstructionsManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static CustomerFirstInstructionsManager Instance;

	#endregion

	#region PUBLIC FIELDS

	[HideInInspector]
	public int instructionID;
	[HideInInspector]
	public bool isShowingInstructions;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Rect Transform References")]
	[SerializeField]
	private RectTransform videoThumbnailInstructions;
	[SerializeField]
	private RectTransform playNowInstructions;
	
	[Header("Canvas References")]
	[SerializeField]
	private Canvas videoThumbnailCanvas;
	[SerializeField]
	private Canvas playNowCanvas;
	[SerializeField]
	private Canvas instructionsCanvas;

	[Header("Game Object Array References")]
	[SerializeField]
	private GameObject[] instructionsList;
	
	[Header("Script Array References")]
	[SerializeField]
	private UIAnimationController[] instructionAnimationControllers;
	
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
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckForInstructionsInScene()
	{
		videoThumbnailInstructions.sizeDelta = new Vector2(0f, instructionsHeight);
		playNowInstructions.sizeDelta = new Vector2(0f, instructionsHeight);

		if (applicationManager.cfInstructionsViewed == 0)
			ToggleInstructionsOnOff();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ToggleInstructionsOnOff()
	{
		isShowingInstructions = !isShowingInstructions;

		instructionsList[instructionID].SetActive(isShowingInstructions);

		if (isShowingInstructions)
			instructionAnimationControllers[instructionID].Initialize();
		else
			instructionAnimationControllers[instructionID].Terminate();

		ReorderSceneCanvases();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ShowNextInstruction()
	{
		if (isShowingInstructions)
		{
			instructionID++;

			instructionsList[instructionID].SetActive(true);
			instructionsList[instructionID - 1].SetActive(false);

			instructionAnimationControllers[instructionID].Initialize();
			instructionAnimationControllers[instructionID - 1].Terminate();

			ReorderSceneCanvases();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SaveAndResetInstructions()
	{
		if (instructionID == (instructionsList.Length - 1))
		{
			applicationManager.cfInstructionsViewed = 1;
			PlayerPrefs.SetInt("CFInstructionsViewed", applicationManager.cfInstructionsViewed);
		}

		instructionID = 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ReorderSceneCanvases()
	{
		if (instructionID == 0)
		{
			videoThumbnailCanvas.sortingOrder = 4;
			instructionsCanvas.sortingOrder = 3;
		}
		else if (instructionID == 1)
		{
			videoThumbnailCanvas.sortingOrder = 3;
			playNowCanvas.sortingOrder = 4;
			instructionsCanvas.sortingOrder = 2;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SkipInstructions()
	{
		isShowingInstructions = false;

		playNowCanvas.sortingOrder = 2;
		videoThumbnailCanvas.sortingOrder = 3;
		instructionsCanvas.sortingOrder = 4;

		instructionsList[instructionID].SetActive(isShowingInstructions);

		SaveAndResetInstructions();
	}

	#endregion

}
