using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BehaviourIconController : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static BehaviourIconController Instance;

	#endregion

	#region PUBLIC FIELDS

	[HideInInspector]
	public int maximumWrongAttempts;

	[Header("Image References")]
	public Image fadedPatch;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Box Collider References")]
	[SerializeField]
	private BoxCollider boxCollider;

	#endregion

	#region PRIVATE VARIABLES

	private string iconName;

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
		iconName = gameObject.name;

		applicationManager = FindObjectOfType<ApplicationManager>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PrepareIcons()
	{
		switch (iconName)
		{
			case "FabBehaviour1":
				LoadBehaviourIcon(applicationManager.fabBehaviour1);
				break;

			case "FabBehaviour2":
				LoadBehaviourIcon(applicationManager.fabBehaviour2);
				break;

			case "FabBehaviour3":
				LoadBehaviourIcon(applicationManager.fabBehaviour3);
				break;

			case "FabBehaviour4":
				LoadBehaviourIcon(applicationManager.fabBehaviour4);
				break;

			case "FabBehaviour5":
				LoadBehaviourIcon(applicationManager.fabBehaviour5);
				break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void LoadBehaviourIcon(int behaviour)
	{
		if (behaviour == 1)
		{
			boxCollider.enabled = false;
			fadedPatch.color = new Color(0f, 0f, 0f, 0.7f);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SaveBehaviourIcon(string behaviour, string question, string answer)
	{
		switch (behaviour)
		{
			case "FabBehaviour1":
				applicationManager.fabBehaviour1 = 1;
				applicationManager.fabBehaviour1Question = question;
				applicationManager.fabBehaviour1Answer = answer;
				break;

			case "FabBehaviour2":
				applicationManager.fabBehaviour2 = 1;
				applicationManager.fabBehaviour2Question = question;
				applicationManager.fabBehaviour2Answer = answer;
				break;

			case "FabBehaviour3":
				applicationManager.fabBehaviour3 = 1;
				applicationManager.fabBehaviour3Question = question;
				applicationManager.fabBehaviour3Answer = answer;
				break;

			case "FabBehaviour4":
				applicationManager.fabBehaviour4 = 1;
				applicationManager.fabBehaviour4Question = question;
				applicationManager.fabBehaviour4Answer = answer;
				break;

			case "FabBehaviour5":
				applicationManager.fabBehaviour5 = 1;
				applicationManager.fabBehaviour5Question = question;
				applicationManager.fabBehaviour5Answer = answer;
				break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ShowHighlight(Image patch)
	{
		patch.DOFade(0.7f, 1.0f).SetEase(Ease.OutBack);
	}

	#endregion

}
