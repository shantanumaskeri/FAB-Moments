using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimationController : MonoBehaviour
{

    #region SINGLETON INSTANCE

    public static UIAnimationController Instance;

	#endregion

	#region PRIVATE VARIABLES

	[Header("Image References")]
    [SerializeField]
    private Image arrowIndicatorImg;

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
        InvokeRepeating(nameof(FadeInOutLoop), 0.0f, 1.0f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void FadeInOutLoop()
    {
        if (arrowIndicatorImg.color.a == 1.0f)
            arrowIndicatorImg.DOFade(0.0f, 1.0f).SetEase(Ease.Linear);
        else if (arrowIndicatorImg.color.a == 0.0f)
            arrowIndicatorImg.DOFade(1.0f, 1.0f).SetEase(Ease.Linear);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Terminate()
	{
        CancelInvoke(nameof(FadeInOutLoop));
	}

	#endregion

}
