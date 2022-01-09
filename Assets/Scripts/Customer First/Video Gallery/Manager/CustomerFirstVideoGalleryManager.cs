using System.Runtime.CompilerServices;
using UnityEngine;

public class CustomerFirstVideoGalleryManager : MonoBehaviour
{

    #region SINGLETON INSTANCE

    public static CustomerFirstVideoGalleryManager Instance;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Rect Transform References")]
    [SerializeField]
    private RectTransform videoPlayer;
    [SerializeField]
    private RectTransform controlsContainer;
    [SerializeField]
    private RectTransform bgPanel;
    
    #endregion

    #region PRIVATE VARIABLES

    private float componentHeight;

	#endregion

	#region UNITY MONOBEHAVIOURS

	private void Start()
    {
        Instance = this;
    }

	#endregion

	#region CUSTOM METHODS

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Initialize()
	{
        componentHeight = UICanvasScalerManager.Instance.canvasHeight;

        AdjustGalleryComponents();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AdjustGalleryComponents()
    {
        videoPlayer.sizeDelta = new Vector2(0f, componentHeight);
        controlsContainer.sizeDelta = new Vector2(0f, componentHeight);
        bgPanel.sizeDelta = new Vector2(0f, componentHeight);
    }

	#endregion

}
