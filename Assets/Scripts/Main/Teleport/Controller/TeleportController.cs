using System.Runtime.CompilerServices;
using UnityEngine;

public class TeleportController : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static TeleportController Instance;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Game Object References")]
	[SerializeField]
	private GameObject transitionProps;
	[SerializeField]
	private GameObject marker;
	
	[Header("Box Collider References")]
	[SerializeField]
	private BoxCollider teleportCollider;

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
        CheckIfPlayerIsStandingOnTeleport(other);
    }

	#endregion

	#region CUSTOM METHODS

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize()
	{
		transitionProps.SetActive(false);
		marker.SetActive(false);

		applicationManager = FindObjectOfType<ApplicationManager>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PrepareTeleport()
	{
		if (applicationManager.teleportState == 1)
			ActivateTeleport();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ActivateTeleport()
	{
		marker.SetActive(true);
		transitionProps.SetActive(true);
		teleportCollider.enabled = true;

		arrowAnimationController.Initialize();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void DeactivateTeleport()
	{
		marker.SetActive(false);
		transitionProps.SetActive(false);
		teleportCollider.enabled = false;

		arrowAnimationController.Terminate();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckIfPlayerIsStandingOnTeleport(Collider other)
	{
        if (other.CompareTag("Player"))
        {
			DeactivateTeleport();
			
			ScenesManager.Instance.ChangeSceneManual();
        }
    }
	
	#endregion

}
