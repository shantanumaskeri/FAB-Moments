using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    #region SINGLETON INSTANCE

    public static CameraManager Instance;

    #endregion

    #region PUBLIC FIELDS

    [Header("Script References")]
    public GyroCameraController gyroCamera;
    public TouchCameraController touchCamera;

    #endregion

    #region PRIVATE VARIABLES

    private string cameraType;

	#endregion

	#region UNITY MONOBEHAVIOURS

	private void Start()
    {
        Instance = this;

        Initialize();
    }

    #endregion

    #region CUSTOM METHODS

    private void Initialize()
	{
        cameraType = "touch";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SwitchCameraTo(string type)
    {
        cameraType = type;
        if (cameraType == "gyro")
        {
            gyroCamera.enabled = true;
            touchCamera.enabled = false;
        }

        if (cameraType == "touch")
        {
            gyroCamera.enabled = false;
            touchCamera.enabled = true;
        }

        GameManager.Instance.CheckPlatformAndSceneToShowInstructions(2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ConfigureCameraForScenario()
	{
        DisableCameras();
        ResetCamera();
	}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ConfigureCameraForInteraction()
    {
        EnableCameras();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void EnableCameras()
    {
        SwitchCameraTo(cameraType);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void DisableCameras()
	{
        gyroCamera.enabled = false;
        touchCamera.enabled = false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ResetCamera()
	{
        gyroCamera.gameObject.transform.DOLocalMove(new Vector3(0f, 0.5f, 0f), 1.0f).SetEase(Ease.Linear);
        gyroCamera.gameObject.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 1.0f).SetEase(Ease.Linear);
    }

	#endregion

}
