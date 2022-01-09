using System.Runtime.CompilerServices;
using UnityEngine;

public class SwitchController : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static SwitchController Instance;

	#endregion

	#region PUBLIC FIELDS

	[Header("Integer Values")]
	public int switchID;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Game Object References")]
	[SerializeField]
	private GameObject transitionProps;
	[SerializeField]
	private GameObject marker;

	[Header("Box Collider References")]
	[SerializeField]
	private BoxCollider switchCollider;

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
		CheckIfPlayerIsStandingOnSwitch(other);
	}

	#endregion

	#region CUSTOM METHODS

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize()
	{
		applicationManager = FindObjectOfType<ApplicationManager>();

		DeactivateSwitch();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PrepareSwitch()
	{
		string switchName = gameObject.name;
		switch (switchName)
		{
			case "-1 BO Switch Receptionist":
				ActivateSwitch();
				break;

			case "0 BO Switch CIB":
				if (applicationManager.cibSwitchState == 1)
					DeactivateSwitch();
				break;

			case "1 BO Switch PBG 1":
				if (applicationManager.pbg1SwitchState == 1)
					DeactivateSwitch();
				break;

			case "2 BO Switch PBG 2":
				if (applicationManager.pbg2SwitchState == 1)
					DeactivateSwitch();
				break;

			case "3 BO Switch CF":
				if (applicationManager.cfSwitchState == 1)
					DeactivateSwitch();
				break;

			case "4 BO Switch ENAB":
				if (applicationManager.enabSwitchState == 1)
					DeactivateSwitch();
				break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ActivateSwitch()
	{
		marker.SetActive(true);
		transitionProps.SetActive(true);
		switchCollider.enabled = true;

		arrowAnimationController.Initialize();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DeactivateSwitch()
	{
		marker.SetActive(false);
		transitionProps.SetActive(false);
		switchCollider.enabled = false;

		arrowAnimationController.Terminate();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckIfPlayerIsStandingOnSwitch(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			ScenariosManager.Instance.ActivateScenario(gameObject);

			DeactivateSwitch();
		}
	}

	#endregion

}
