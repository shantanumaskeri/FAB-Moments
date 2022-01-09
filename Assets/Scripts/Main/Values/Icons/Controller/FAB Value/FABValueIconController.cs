using DG.Tweening;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class FABValueIconController : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static FABValueIconController Instance;

	#endregion

	#region PUBLIC FIELDS

	[Header("Image References")]
	public Image iconHUD;

	[Header("Game Object References")]
	public GameObject valueName;

	[Header("Sprite Renderer References")]
	public SpriteRenderer shadow;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Floating Point Values")]
	[SerializeField]
	private float duration = 1.5f;

	[SerializeField]
	private float defaultScale = 0.34f;

	[SerializeField]
	private float increasedScale = 0.68f;

	[Header("Box Collider References")]
	[SerializeField]
	private BoxCollider boxCollider;

	#endregion

	#region PRIVATE VARIABLES

	private string iconName;

	private Transform iconTransform;

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
			case "FabValue1":
				LoadValueIcon(applicationManager.fabValue1);
				break;

			case "FabValue2":
				LoadValueIcon(applicationManager.fabValue2);
				break;

			case "FabValue3":
				LoadValueIcon(applicationManager.fabValue3);
				break;

			case "FabValue4":
				LoadValueIcon(applicationManager.fabValue4);
				break;

			case "FabValue5":
				LoadValueIcon(applicationManager.fabValue5);
				break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ShowHighlight(GameObject valueName, SpriteRenderer shadow)
	{
		valueName.SetActive(true);
		shadow.DOFade(0.5f, 2.0f).SetEase(Ease.OutBack);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void LoadValueIcon(int value)
	{
		if (value == 1)
		{
			valueName.SetActive(true);
			boxCollider.enabled = false;
			shadow.color = new Color(0f, 0.6901960784313725f, 0.1764705882352941f, 0.5f);

			HUDManager.Instance.UpdateHUDIcons(iconHUD);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SaveValueIcon(string value)
	{
		switch (value)
		{
			case "FabValue1":
				applicationManager.fabValue1 = 1;
				applicationManager.fabValue1IconName = "Collaborative";
				break;

			case "FabValue2":
				applicationManager.fabValue2 = 1;
				applicationManager.fabValue2IconName = "Customer First";
				break;

			case "FabValue3":
				applicationManager.fabValue3 = 1;
				applicationManager.fabValue3IconName = "Enterprising";
				break;

			case "FabValue4":
				applicationManager.fabValue4 = 1;
				applicationManager.fabValue4IconName = "Knowledgeable";
				break;

			case "FabValue5":
				applicationManager.fabValue5 = 1;
				applicationManager.fabValue5IconName = "Trusted";
				break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PlayAnimation(Transform transform)
	{
		iconTransform = transform;
		InvokeRepeating(nameof(BreatheLoop), 0.0f, 2.0f);

		StartCoroutine(StopAnimation());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void BreatheLoop()
	{
		if (iconTransform.localScale.x == defaultScale)
			iconTransform.DOScale(increasedScale, duration).SetEase(Ease.Linear);
		else if (iconTransform.localScale.x == increasedScale)
			iconTransform.DOScale(defaultScale, duration).SetEase(Ease.Linear);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerator StopAnimation()
	{
		yield return new WaitForSeconds(5.0f);

		CancelInvoke(nameof(BreatheLoop));
	}

	#endregion

}
