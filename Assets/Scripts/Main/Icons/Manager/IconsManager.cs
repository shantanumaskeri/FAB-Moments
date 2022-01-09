using System.Runtime.CompilerServices;
using UnityEngine;

public class IconsManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static IconsManager Instance;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Box Collider References")]
	[SerializeField]
	private BoxCollider[] iconColliders;

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

	private void Update()
	{
		MouseInput();	
	}

	#endregion

	#region CUSTOM METHODS

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize()
	{
		applicationManager = FindObjectOfType<ApplicationManager>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void MouseInput()
	{
		if (Input.GetMouseButtonDown(0))
			if (!InstructionsManager.Instance.isShowingInstructions && !PanelsManager.Instance.isPanelOpen)
				ApplyRaycastOnIcons();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ApplyRaycastOnIcons()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
		{
			if (hit.collider.CompareTag("FAB Value Icons"))
			{
				applicationManager.consecutiveCorrectIconsFound++;
				
				hit.collider.enabled = false;

				hit.collider.gameObject.GetComponent<FABValueIconController>().PlayAnimation(hit.collider.gameObject.transform);
				hit.collider.gameObject.GetComponent<FABValueIconController>().ShowHighlight(hit.collider.gameObject.GetComponent<FABValueIconController>().valueName, hit.collider.gameObject.GetComponent<FABValueIconController>().shadow);
				hit.collider.gameObject.GetComponent<FABValueIconController>().SaveValueIcon(hit.collider.gameObject.name);

				AudioManager.Instance.PlayAudio("CorrectAnswer");
				
				ScoreManager.Instance.IncrementScore(10 * applicationManager.consecutiveCorrectIconsFound);

				applicationManager.valueIconsCollected++;
				
				if (applicationManager.valueIconsCollected == 5)
				{
					DoorController.Instance.EnableDoor();

					HUDManager.Instance.UpdateHUDLevelMessage("Walk towards the Door");

					DisableNonValueIconColliders();
				}

				HUDManager.Instance.ShowHUDIcons(hit.collider.gameObject.GetComponent<FABValueIconController>().iconHUD);
			}
			else if (hit.collider.CompareTag("Non Value Icons"))
			{
				applicationManager.consecutiveCorrectIconsFound = 0;
				
				hit.collider.enabled = false;

				hit.collider.gameObject.GetComponent<NonValueIconController>().ShowHighlight(hit.collider.gameObject.GetComponent<SpriteRenderer>());
				hit.collider.gameObject.GetComponent<NonValueIconController>().SaveNonValueIcon(hit.collider.gameObject.name);

				AudioManager.Instance.PlayAudio("IncorrectAnswer");
				
				ScoreManager.Instance.DecrementScore(5);
			}
			else if (hit.collider.CompareTag("Behaviour Icons"))
			{
				/*if (!PanelsManager.Instance.isPanelOpen)
				{
					
				}*/

				string iconName = hit.collider.gameObject.name;
				string iconDetail = iconName.Substring(iconName.Length - 1);
				int iconID = int.Parse(iconDetail);

				PanelsManager.Instance.Expand(PanelsManager.Instance.quizPanel);

				BehavioursQuizXMLManager.Instance.ConfigureXML(iconID);

				if (InstructionsManager.Instance.instructionID == 4)
					InstructionsManager.Instance.ToggleInstructionsOnOff();
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void DisableNonValueIconColliders()
	{
		for (int i = 0; i < iconColliders.Length; i++)
		{
			if (iconColliders[i].enabled)
				iconColliders[i].enabled = false;
		}
	}

	#endregion

}
