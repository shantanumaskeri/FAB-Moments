using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DoorController : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static DoorController Instance;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[SerializeField]
	private GameObject doorProps;
	[SerializeField]
	private GameObject marker;

	[Header("Transform References")]
	[SerializeField]
    private Transform doorFrameLeft;
    [SerializeField]
    private Transform doorFrameRight;

	[Header("Box Collider References")]
	[SerializeField]
	private BoxCollider doorCollider;

	[Header("Script References")]
	[SerializeField]
	private UIAnimationController arrowAnimationController;

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

	private void OnTriggerEnter(Collider other)
	{
		CheckIfPlayerIsNearDoor(other);
	}

	#endregion

	#region CUSTOM METHODS

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize()
	{
		doorProps.SetActive(false);
		marker.SetActive(false);

		applicationManager = FindObjectOfType<ApplicationManager>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckIfPlayerIsNearDoor(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			OpenDoor();
			DisableDoor();

			AudioManager.Instance.PlayAudio("DoorOpen");

			applicationManager.doorState = 1;
			
			HUDManager.Instance.UpdateHUDLevelMessage("Select the correct sentences\nassociated to FAB\nCustomer First Behaviours");
			HUDManager.Instance.UpdateHUDBanner();

			ValuesAPIManager.Instance.Values("POST");
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PrepareDoor()
	{
		if (applicationManager.doorState == 1)
		{
			doorFrameLeft.position = new Vector3(0.0f, doorFrameLeft.position.y, doorFrameLeft.position.z);
			doorFrameRight.position = new Vector3(-7.0f, doorFrameRight.position.y, doorFrameRight.position.z);
		}
		else
		{
			if (applicationManager.valueLevelCompleted == 1)
				EnableDoor();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void EnableDoor()
	{
		doorProps.SetActive(true);
		marker.SetActive(true);
		
		doorCollider.enabled = true;

		arrowAnimationController.Initialize();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void DisableDoor()
	{
		arrowAnimationController.Terminate();

		doorCollider.enabled = false;

		marker.SetActive(false);
		doorProps.SetActive(false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void OpenDoor()
	{
		doorFrameLeft.DOMoveX(0.0f, 4.0f).SetEase(Ease.OutBack);
		doorFrameRight.DOMoveX(-7.0f, 4.0f).SetEase(Ease.OutBack);
	}

	#endregion

}
