using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ControlsManager : MonoBehaviour
{

    #region SINGLETION INSTANCE

    public static ControlsManager Instance;

    #endregion

    #region PUBLIC FIELDS

    [Header("Script References")]
    public FixedJoystick MoveJoystick;

    [Header("Game Object References")]
    public GameObject camTouch;
    public GameObject camGyro;

    #endregion

    #region EDITOR ASSIGNED VARIABLES

    [SerializeField]
    private GameObject joystick;    
    [SerializeField]
    private GameObject touchCameraField;
    [SerializeField]
    private GameObject controlsCanvas;

	#endregion

	#region PRIVATE VARIABLES

	private Button btnCamTouch;
    private Button btnCamGyro;

    #endregion

    #region UNITY MONOBEHAVIORS

    private void Start()
    {
        Instance = this;

        Initialize();
    }

    private void Update()
    {
        CheckPlatformOnEveryFrame();
    }

    #endregion

    #region CUSTOM METHODS

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Initialize()
	{
        btnCamTouch = camTouch.GetComponent<Button>();
        btnCamGyro = camGyro.GetComponent<Button>();

        CheckPlatformOnSceneLoad();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CheckPlatformOnSceneLoad()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
            PreparePCStandaloneControls();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CheckPlatformOnEveryFrame()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            PrepareMobileControls();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void PreparePCStandaloneControls()
    {
        joystick.SetActive(false);
        touchCameraField.SetActive(false);
        camTouch.SetActive(false);
        camGyro.SetActive(false);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void PrepareMobileControls()
    {
        PlayerController.Instance.firstPersonController.RunAxis = MoveJoystick.Direction;
        PlayerController.Instance.firstPersonController.m_MouseLook.LookAxis = CameraManager.Instance.touchCamera.TouchDist;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ConfigureControlsForScenario()
	{
        DisableControls();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ConfigureControlsForInteraction()
    {
        EnableControls();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnableControls()
    {
        btnCamTouch.interactable = true;
        btnCamGyro.interactable = true;

        MoveJoystick.enabled = true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void DisableControls()
	{
        btnCamTouch.interactable = false;
        btnCamGyro.interactable = false;

        MoveJoystick.enabled = false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SwitchControlTo(string controlType)
    {
        if (controlType == "gyro")
        {
            camTouch.SetActive(false);
            camGyro.SetActive(true);
        }

        if (controlType == "touch")
        {
            camTouch.SetActive(true);
            camGyro.SetActive(false);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ToggleControlsOnOff(bool value)
	{
        controlsCanvas.SetActive(value);

    }

    #endregion

}
 