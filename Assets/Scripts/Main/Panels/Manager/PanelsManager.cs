using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PanelsManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static PanelsManager Instance;

	#endregion

	#region PUBLIC FIELDS

	[HideInInspector]
	public bool isPanelOpen;

	[Header("Rect Transform References")]
	public RectTransform quizPanel;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[SerializeField]
	private RectTransform profilePanel;

	[SerializeField]
	private RectTransform leaderboardPanel;

	[Header("Floating Point Values")]
	[SerializeField]
	private float scale = 0.0f;

	#endregion

	#region PRIVATE VARIABLES

	private float panelHeight;

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
		isPanelOpen = false;

		panelHeight = UICanvasScalerManager.Instance.canvasHeight;

		CheckForPanelsInScene();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckForPanelsInScene()
	{
		if (quizPanel != null)
		{
			quizPanel.sizeDelta = new Vector2(0f, panelHeight);
			quizPanel.localScale = new Vector3(0f, 0f, 0f);
		}

		if (profilePanel != null)
		{
			profilePanel.sizeDelta = new Vector2(0f, panelHeight);
			profilePanel.localScale = new Vector3(0f, 0f, 0f);
		}

		if (leaderboardPanel != null)
		{
			leaderboardPanel.sizeDelta = new Vector2(0f, panelHeight);
			leaderboardPanel.localScale = new Vector3(0f, 0f, 0f);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Expand(Transform panelTransform)
	{
		isPanelOpen = true;

		panelTransform.gameObject.GetComponent<Image>().raycastTarget = true;

		panelTransform.DOScale(scale, 0.5f).SetEase(Ease.OutBack);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Shrink(Transform panelTransform)
    {
		isPanelOpen = false;

		panelTransform.gameObject.GetComponent<Image>().raycastTarget = false;

		panelTransform.DOScale(0.0f, 0.5f).SetEase(Ease.InBack);
    }

	#endregion

}
