using DG.Tweening;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CustomerFirstLandingPageManager : MonoBehaviour
{

    #region SINGLETON INSTANCE

    public static CustomerFirstLandingPageManager Instance;

    #endregion

    #region PUBLIC FIELDS

    [HideInInspector]
    public int selectedVideoId;

    #endregion

    #region EDITOR ASSIGNED VARIABLES

    [Header("Image Array References")]
    [SerializeField]
    private Image[] videoBackgrounds;
    [SerializeField]
    private Image[] thumbnails;

    [Header("Game Object Array References")]
    [SerializeField]
    private GameObject[] thumbnailOutlines;

    [Header("Rect Transform References")]
    [SerializeField]
    private RectTransform bgLandingPage;

    [Header("Game Object References")]
    [SerializeField]
    private GameObject continueBtn;
    [SerializeField]
    private GameObject skipVideosBtn;

    #endregion

    #region PRIVATE VARIABLES

    private float landingPageHeight;
    private GameObject selectedBackground;

    private ApplicationManager applicationManager;

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
        applicationManager = FindObjectOfType<ApplicationManager>();

        landingPageHeight = UICanvasScalerManager.Instance.canvasHeight;
        
        AdjustLandingPageBackground();
        ConfigureVideoInformation(0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AdjustLandingPageBackground()
	{
        bgLandingPage.sizeDelta = new Vector2(0f, landingPageHeight);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void FadeAllVideoBackgrounds()
    {
        for (int i = 0; i < videoBackgrounds.Length; i++)
        {
            videoBackgrounds[i].DOFade(0.0f, 0.5f).SetEase(Ease.Linear);
        }

        StartCoroutine(HideAllVideoBackgrounds());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IEnumerator HideAllVideoBackgrounds()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < videoBackgrounds.Length; i++)
        {
            videoBackgrounds[i].gameObject.SetActive(false);
        }

        if (selectedBackground != null)
            selectedBackground.SetActive(true);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ResetMainMenuButton()
    {
        for (int i = 0; i < thumbnailOutlines.Length; i++)
        {
            thumbnailOutlines[i].SetActive(false);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ConfigureVideoInformation(int id)
	{
        FadeAllVideoBackgrounds();
        ResetMainMenuButton();
        LoadSelectedVideoBackground(id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void LoadSelectedVideoBackground(int id)
    {
        selectedBackground = videoBackgrounds[id].gameObject;
        selectedBackground.SetActive(true);
        
        videoBackgrounds[id].DOFade(1.0f, 0.5f).SetEase(Ease.Linear);
        thumbnailOutlines[id].SetActive(true);

        selectedVideoId = id;

        if (selectedVideoId > 0)
            if (CustomerFirstInstructionsManager.Instance.instructionID == 0)
                    CustomerFirstInstructionsManager.Instance.ShowNextInstruction();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ToggleLandingPageButtons(bool value)
	{
        continueBtn.SetActive(value);
        skipVideosBtn.SetActive(!value);
	}

    #endregion

}
