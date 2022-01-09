using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class LaunchManager : MonoBehaviour
{

    #region SINGLETON INSTANCE

    public static LaunchManager Instance;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Button References")]
	[SerializeField]
	private Button enterGameButton;

	[Header("Text References")]
	[SerializeField]
    private Text statusMessage;

	[Header("Game Object References")]
	[SerializeField]
	private GameObject spinner;

	#endregion

	#region PRIVATE VARIABLES

	private int loginState;
    private int avatarState;

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
		ToggleLoadingSpinnerOnOff(false);

		applicationManager = FindObjectOfType<ApplicationManager>();
	}

	public void LoadPlayerInformation()
	{
		enterGameButton.interactable = false;

		ProfileAPIManager.Instance.Profile();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SaveLoginAndAvatarStatus()
    {
        loginState = applicationManager.loggedIn;
        avatarState = applicationManager.avatarSelected;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ToggleLoadingSpinnerOnOff(bool value)
	{
		spinner.SetActive(value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ShowStatusMessage(string message, string result)
	{
		statusMessage.text = message;

		StartCoroutine(HideStatusMessage(result));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IEnumerator HideStatusMessage(string result)
	{
        yield return new WaitForSeconds(2.0f);

        statusMessage.text = "";

		if (result == "success")
			ScenesManager.Instance.ChangeSceneManual();
		else
			enterGameButton.interactable = true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void CheckStatusAndLoadNextScene()
	{
		if (loginState == 0 && avatarState == 0)
			LoadingManager.Instance.FadeToLevel(1);
		else if (loginState == 1 && avatarState == 0)
			LoadingManager.Instance.FadeToLevel(2);
		else if (loginState == 1 && avatarState != 0)
			LoadingManager.Instance.FadeToLevel(3);
	}

	#endregion

}
