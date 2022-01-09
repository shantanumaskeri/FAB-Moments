using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static LoginManager Instance;

	#endregion

	#region PUBLIC FIELDS

	[Header("Input Field References")]
	public InputField loginEmail;
	public InputField loginPassword;

	[Header("Button References")]
	public Button signInButton;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Game Object References")]
	[SerializeField]
	private GameObject emailValidityLine;
	[SerializeField]
	private GameObject spinner;
	[SerializeField]
	private GameObject passwordEye;
	[SerializeField]
	private GameObject passwordToggleLine;

	[Header("Text References")]
	[SerializeField]
	private Text statusMessage;

	#endregion

	#region PRIVATE VARIABLES

	private bool showPassword;
	private int avatarState;

	private ApplicationManager applicationManager;

	#endregion

	#region UNITY MONOBEHAVIOURS

	private void Start()
	{
		Instance = this;

		Initialize();
	}

	private void Update()
	{
		CheckPlatformForKeyboard();
	}

	private void OnEnable()
	{
		EnableInputFieldValidation();
	}

	private void OnDisable()
	{
		DisableInputFieldValidation();
	}

	#endregion

	#region CUSTOM METHODS

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize()
	{
		showPassword = false;

		passwordEye.SetActive(false);
		passwordToggleLine.SetActive(false);
		
		ToggleEmailValidityLineOnOff(false);
		ToggleLoadingSpinnerOnOff(false);

		applicationManager = FindObjectOfType<ApplicationManager>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckPlatformForKeyboard()
	{
		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
		{
			HandleKeyboardInput();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void EnableInputFieldValidation()
	{
		loginEmail.onValueChanged.AddListener(delegate { CheckEmailEntered(); });
		loginPassword.onValueChanged.AddListener(delegate { CheckPasswordEntered(); });
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DisableInputFieldValidation()
	{
		loginEmail.onValueChanged.RemoveAllListeners();
		loginPassword.onValueChanged.RemoveAllListeners();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void HandleKeyboardInput()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (loginEmail.isFocused)
				loginPassword.Select();
			else
				loginEmail.Select();
		}

		if (Input.GetKeyDown(KeyCode.Return))
			LoginAPIManager.Instance.Login();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void TogglePasswordOnOff()
	{
		showPassword = !showPassword;
		switch (showPassword)
		{
			case true:
				loginPassword.contentType = InputField.ContentType.Standard;
				passwordToggleLine.SetActive(true);
				break;

			case false:
				loginPassword.contentType = InputField.ContentType.Password;
				passwordToggleLine.SetActive(false);
				break;
		}

		loginPassword.ForceLabelUpdate();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ToggleEmailValidityLineOnOff(bool value)
	{
		emailValidityLine.SetActive(value);
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
	public IEnumerator HideStatusMessage(string result)
	{
		if (result == "success")
		{
			yield return new WaitForSeconds(2.0f);

			statusMessage.text = "";

			ScenesManager.Instance.ChangeSceneManual();
		}
		else
		{
			yield return new WaitForSeconds(5.0f);

			statusMessage.text = "";

			signInButton.interactable = true;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SaveLoginStatus()
	{
		applicationManager.loggedIn = 1;
		PlayerPrefs.SetInt("LoggedIn", applicationManager.loggedIn);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SaveAvatarStatus()
	{
		avatarState = applicationManager.avatarSelected;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckEmailEntered()
	{
		if (ValidEmailAddress(loginEmail.text))
			ToggleEmailValidityLineOnOff(true);
		else
			ToggleEmailValidityLineOnOff(false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private bool ValidEmailAddress(string email)
	{
		#region general conditions

		if (email.IndexOf("@") == -1)
			return false;


		if (email.IndexOf("!") >= 0 || email.IndexOf("#") >= 0 || email.IndexOf("$") >= 0 || email.IndexOf("%") >= 0 || email.IndexOf("^") >= 0 || email.IndexOf("&") >= 0 || email.IndexOf("*") >= 0 || email.IndexOf("(") >= 0 || email.IndexOf(")") >= 0 || email.IndexOf("=") >= 0 || email.IndexOf("+") >= 0 || email.IndexOf("[") >= 0 || email.IndexOf("{") >= 0 || email.IndexOf("]") >= 0 || email.IndexOf("}") >= 0 || email.IndexOf(":") >= 0 || email.IndexOf(";") >= 0 || email.IndexOf("'") >= 0 || email.IndexOf("|") >= 0 || email.IndexOf(",") >= 0 || email.IndexOf("<") >= 0 || email.IndexOf(">") >= 0 || email.IndexOf("/") >= 0 || email.IndexOf("?") >= 0)
			return false;


		if (email.IndexOf("0") == 0 || email.IndexOf("1") == 0 || email.IndexOf("2") == 0 || email.IndexOf("3") == 0 || email.IndexOf("4") == 0 || email.IndexOf("5") == 0 || email.IndexOf("6") == 0 || email.IndexOf("7") == 0 || email.IndexOf("8") == 0 || email.IndexOf("9") == 0 || email.IndexOf("@") == 0 || email.IndexOf("-") == 0 || email.IndexOf("_") == 0 || email.IndexOf(".") == 0)
			return false;


		if (email.IndexOf("@") != email.LastIndexOf("@"))
			return false;

		#endregion

		if (email != "")
		{
			#region divide email into two parts - name & domain

			string[] email_division = email.Split("@"[0]);
			string email_name = email_division[0];
			string email_domain = "@" + email_division[1];

			#endregion

			#region email name conditions

			if (email_name.IndexOf(".") != -1)
			{
				if (email_name.IndexOf(".") == 0)
					return false;

				if (email_name.IndexOf(".") != email_name.LastIndexOf("."))
					return false;

				if (email_name.IndexOf(".") == (email_name.Length - 1))
					return false;
			}

			if (email_name.IndexOf("-") != -1)
			{
				if (email_name.IndexOf("-") != email_name.LastIndexOf("-"))
					return false;

				if (Mathf.Abs(email_name.IndexOf("-") - email_name.IndexOf(".")) == 1)
					return false;
			}

			if (email_name.IndexOf("_") != -1)
			{
				if (email_name.IndexOf("_") != email_name.LastIndexOf("_"))
					return false;

				if (Mathf.Abs(email_name.IndexOf("_") - email_name.IndexOf(".")) == 1)
					return false;

				if (Mathf.Abs(email_name.IndexOf("_") - email_name.IndexOf("@")) == 1)
					return false;
			}

			#endregion

			#region email domain conditions

			if (email_domain.IndexOf(".") == -1)
				return false;

			if (email_domain.IndexOf(".") != email_domain.LastIndexOf("."))
			{
				if ((email_domain.LastIndexOf(".") - email_domain.IndexOf(".")) < 3)
					return false;
			}
			else
			{
				if ((email_domain.IndexOf(".") - email_domain.IndexOf("@")) < 3)
					return false;
			}

			if (email_domain.IndexOf("-") != -1)
			{
				if (email_domain.IndexOf("-") != email_domain.LastIndexOf("-"))
					return false;

				if (Mathf.Abs(email_domain.IndexOf("-") - email_domain.IndexOf(".")) == 1)
					return false;
			}

			if (email_domain.IndexOf("_") != -1)
				return false;

			#endregion
		}

		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckPasswordEntered()
	{
		if (ValidPassword(loginPassword.text))
			passwordEye.SetActive(true);
		else
			passwordEye.SetActive(false);

		ToggleEmailValidityLineOnOff(false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private bool ValidPassword(string password)
	{
		if (password == "")
			return false;

		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void CheckStatusAndLoadNextScene()
	{
		if (avatarState == 0)
			LoadingManager.Instance.FadeToLevel(1);
		else
			LoadingManager.Instance.FadeToLevel(2);
	}

	#endregion

}


