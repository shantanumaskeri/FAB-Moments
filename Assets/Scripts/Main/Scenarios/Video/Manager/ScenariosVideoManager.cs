using DG.Tweening;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ScenariosVideoManager : MonoBehaviour
{

    #region SINGLETON INSTANCE

    public static ScenariosVideoManager Instance;

    #endregion

    #region PUBLIC FIELDS

    [HideInInspector]
    public int scenarioID;

    #endregion

    #region EDITOR ASSIGNED VARIABLES

    [Header("Integer Array References")]
    [SerializeField]
    private int[] CIBScenarioTimeStamps;
    [SerializeField]
    private int[] PBGAhmedScenarioTimeStamps;
    [SerializeField]
    private int[] PBGNadaScenarioTimeStamps;
    [SerializeField]
    private int[] CFScenarioTimeStamps;
    [SerializeField]
    private int[] ENABScenarioTimeStamps;

    [Header("Video Player References")]
    [SerializeField]
    private VideoPlayer scenarioPlayer;

    [Header("Raw Image References")]
    [SerializeField]
    private RawImage scenarioTexture;

    [Header("Transform References")]
    [SerializeField]
    private Transform scenarioButton;

    [Header("Video Clip Array References")]
    [SerializeField]
    private VideoClip[] scenarioVideos;

    [Header("String Array References")]
    [SerializeField]
    private string[] scenarioUrls;

    #endregion

    #region PRIVATE VARIABLES

    private int timeStampID;
    private bool isVideoPaused;

	#endregion

	#region UNITY MONOBEHAVIOURS

	private void Start()
    {
        Instance = this;

        Initialize();
    }

	private void Update()
	{
        if (!isVideoPaused)
            CheckVideoPlayback();
    }

    #endregion

    #region CUSTOM METHODS

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Initialize()
	{
        isVideoPaused = true;
	}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ConfigureVideoForScenario(GameObject switchInstance)
    {
        isVideoPaused = false;

        timeStampID = 0;
        scenarioID = switchInstance.GetComponent<SwitchController>().switchID;

        StartCoroutine(ToggleScenarioOnOff(true, 0.0f));

        /*scenarioPlayer.clip = scenarioVideos[scenarioID];
        scenarioPlayer.Prepare();
        scenarioPlayer.Play();
        scenarioPlayer.Pause();*/

        scenarioPlayer.source = VideoSource.Url;
        scenarioPlayer.url = scenarioUrls[scenarioID];
        scenarioPlayer.Prepare();
        scenarioPlayer.Play();
        scenarioPlayer.Pause();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IEnumerator ToggleScenarioOnOff(bool value, float delay)
	{
        yield return new WaitForSeconds(delay);

        if (value == true)
            scenarioTexture.gameObject.SetActive(true);

        scenarioTexture.enabled = value;

        if (value == false)
            scenarioTexture.gameObject.SetActive(false);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void PlayScenarioVideo()
	{
        HUDManager.Instance.ToggleHUDOnOff(false);
        ControlsManager.Instance.ToggleControlsOnOff(false);
        ScenariosDialogueManager.Instance.HideAllComponents();

        scenarioTexture.DOFade(1.0f, 1.0f).SetEase(Ease.OutBack);

        scenarioPlayer.Play();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void StopScenarioVideo()
    {
        scenarioPlayer.Stop();
        
        scenarioTexture.DOFade(0.0f, 1.0f).SetEase(Ease.OutBack);

        StartCoroutine(ToggleScenarioOnOff(false, 1.0f));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void TogglePlayPauseVideo()
	{
        if (scenarioPlayer.isPlaying)
            scenarioPlayer.Pause();
        else
            scenarioPlayer.Play();

        isVideoPaused = !scenarioPlayer.isPlaying;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CheckVideoPlayback()
	{
        switch (scenarioID)
		{
            case 0:
                OnSpecificTime(CIBScenarioTimeStamps);
                break;

            case 1:
                OnSpecificTime(PBGAhmedScenarioTimeStamps);
                break;

            case 2:
                OnSpecificTime(PBGNadaScenarioTimeStamps);
                break;

            case 3:
                OnSpecificTime(CFScenarioTimeStamps);
                break;

            case 4:
                OnSpecificTime(ENABScenarioTimeStamps);
                break;
        }
	}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnSpecificTime(int[] timestamps)
    {
        if (timeStampID < timestamps.Length)
        {
            if (scenarioPlayer.frame == timestamps[timeStampID])
            {
                StartCoroutine(ScenariosQuizManager.Instance.ShowQuiz());

                TogglePlayPauseVideo();
            }
        }
        else
		{
            if ((int)scenarioPlayer.frame == ((int)scenarioPlayer.frameCount - 4))
            {
                ScenariosQuizManager.Instance.EndQuiz();

                isVideoPaused = true;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ResumeScenarioVideo()
    {
        timeStampID++;

        TogglePlayPauseVideo();

        PanelsManager.Instance.Shrink(PanelsManager.Instance.quizPanel);
    }

    #endregion

}
