using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NonValueIconController : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static NonValueIconController Instance;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Box Collider References")]
	[SerializeField]
	private BoxCollider boxCollider;

	[Header("Sprite Renderer References")]
	[SerializeField]
	private SpriteRenderer sprite;

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
		CheckIconState();
		CheckValueLevelState();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckIconState()
	{
		switch (iconName)
		{
			case "NonValue1":
				LoadNonValueIcon(applicationManager.nonValue1);
				break;

			case "NonValue2":
				LoadNonValueIcon(applicationManager.nonValue2);
				break;

			case "NonValue3":
				LoadNonValueIcon(applicationManager.nonValue3);
				break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckValueLevelState()
	{
		if (applicationManager.valueIconsCollected == 5)
			boxCollider.enabled = false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ShowHighlight(SpriteRenderer icon)
	{
		icon.DOFade(0.5f, 1.0f).SetEase(Ease.OutBack);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void LoadNonValueIcon(int value)
	{
		if (value == 1)
		{
			sprite.color = new Color(1f, 1f, 1f, 0.5f);
			boxCollider.enabled = false;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SaveNonValueIcon(string value)
	{
		switch (value)
		{
			case "NonValue1":
				applicationManager.nonValue1 = 1;
				applicationManager.nonValue1IconName = "Product Promotion";
				break;

			case "NonValue2":
				applicationManager.nonValue2 = 1;
				applicationManager.nonValue2IconName = "Profit Making";
				break;

			case "NonValue3":
				applicationManager.nonValue3 = 1;
				applicationManager.nonValue3IconName = "Protection Against Scams";
				break;
		}
	}

	#endregion

}
