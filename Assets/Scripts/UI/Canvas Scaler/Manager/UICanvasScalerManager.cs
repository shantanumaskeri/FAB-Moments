using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UICanvasScalerManager : MonoBehaviour
{

    #region SINGLETON INSTANCE

    public static UICanvasScalerManager Instance;

    #endregion

    #region PUBLIC FIELDS

    [HideInInspector]
    public float canvasHeight;

	#endregion

	#region PRIVATE VARIABLES

	private float screenRatio;

    private CanvasScaler[] canvasScalers;

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
        screenRatio = AspectRatioManager.Instance.aspectRatio;

        FindAllCanvasScalerComponentsInScene();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void FindAllCanvasScalerComponentsInScene()
	{
        canvasScalers = FindObjectsOfType<CanvasScaler>();

        CheckAspectRatioAndUpdateCanvasScalerLayout();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CheckAspectRatioAndUpdateCanvasScalerLayout()
	{
        switch (screenRatio)
		{
            case 1.25f:
                ConfigureCanvasScalerResolutionHeight(1536f);
                break;

            case 1.333333333333333f:
            case 1.333984375f:
                ConfigureCanvasScalerResolutionHeight(1440f);
                break;

            case 1.5f:
                ConfigureCanvasScalerResolutionHeight(1280f);
                break;

            case 1.6f:
                ConfigureCanvasScalerResolutionHeight(1200f);
                break;

            default:
                ConfigureCanvasScalerResolutionHeight(1080f);
                break;
		}
	}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ConfigureCanvasScalerResolutionHeight(float height)
	{
        canvasHeight = height;

        for (int i = 0; i < canvasScalers.Length; i++)
        {
            canvasScalers[i].referenceResolution = new Vector2(1920f, height);
        }
    }

	#endregion

}
