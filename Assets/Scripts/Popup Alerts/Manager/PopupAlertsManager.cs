using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PopupAlertsManager : MonoBehaviour
{

    #region SINGLETON INSTANCE

    public static PopupAlertsManager Instance;

    #endregion

    #region PUBLIC FIELDS

    [Header("Rect Transform References")]
    public RectTransform otherAlertPanel;
    public RectTransform extraAlertPanel;

    #endregion

    #region EDITOR ASSIGNED VARIABLES

    [SerializeField]
    private RectTransform exitAppAlertPanel;

    #endregion

    #region PRIVATE VARIABLES

    private float popupAlertPanelHeight;

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
        popupAlertPanelHeight = UICanvasScalerManager.Instance.canvasHeight;
        
        CheckForPopupAlertInScene();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CheckForPopupAlertInScene()
	{
        if (exitAppAlertPanel != null)
		{
            exitAppAlertPanel.sizeDelta = new Vector2(0f, popupAlertPanelHeight);
			exitAppAlertPanel.localScale = new Vector3(0f, 0f, 0f);
        }
            
        if (otherAlertPanel != null)
		{
            otherAlertPanel.sizeDelta = new Vector2(0f, popupAlertPanelHeight);
            otherAlertPanel.localScale = new Vector3(0f, 0f, 0f);
        }

        if (extraAlertPanel != null)
		{
            extraAlertPanel.sizeDelta = new Vector2(0f, popupAlertPanelHeight);
            extraAlertPanel.localScale = new Vector3(0f, 0f, 0f);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ShowAlertPanelPopUp(Transform alertPanel)
    {
        alertPanel.gameObject.GetComponent<Image>().raycastTarget = true;
        
        alertPanel.DOScale(1.0f, 0.5f).SetEase(Ease.OutBack);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void HideAlertPanelPopUp(Transform alertPanel)
    {
        alertPanel.gameObject.GetComponent<Image>().raycastTarget = false;

        alertPanel.DOScale(0.0f, 0.5f).SetEase(Ease.InBack);
    }

	#endregion

}
