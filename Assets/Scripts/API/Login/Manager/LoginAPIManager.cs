using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class LoginAPIManager : MonoBehaviour
{

	#region CONSTANTS

	private const string LOGIN_API = "https://fabmoments.bellimmersive.com/api/auth/login?";

    #endregion

    #region SINGLETON INSTANCE

    public static LoginAPIManager Instance;

	#endregion

	#region PRIVATE VARIABLES

	private string Email;
	private string Password;

	private ApplicationManager applicationManager;

	#endregion

	#region UNITY MONOBEHAVIOURS

	private void Start()
    {
        Instance = this;

		applicationManager = FindObjectOfType<ApplicationManager>();
    }

	#endregion

	#region CUSTOM METHODS

	public void Login()
	{
        StartCoroutine(LoginCheck());
    }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerator LoginCheck()
	{
		LoginManager.Instance.signInButton.interactable = false;

		LoginManager.Instance.ToggleLoadingSpinnerOnOff(true);

		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			LoginManager.Instance.ToggleLoadingSpinnerOnOff(false);
			LoginManager.Instance.ShowStatusMessage("Please check your internet connection.", "failure");
		}
		else
		{
			Email = LoginManager.Instance.loginEmail.text.ToLower();
			Password = LoginManager.Instance.loginPassword.text;

			WWWForm form = new WWWForm();
			form.AddField("email", Email);
			form.AddField("password", Password);

			using (UnityWebRequest webRequest = UnityWebRequest.Post(LOGIN_API, form))
			{
				yield return webRequest.SendWebRequest();

				if (webRequest.isNetworkError)
				{
					LoginManager.Instance.ToggleLoadingSpinnerOnOff(false);
					LoginManager.Instance.ShowStatusMessage(webRequest.error+". Please try again later.", "failure");
				}
				else
				{
					Response response = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);
					if (response.success.message == "success")
					{
						applicationManager.token = response.success.data.token;
						PlayerPrefs.SetString("Token", applicationManager.token);

						LoginManager.Instance.ToggleEmailValidityLineOnOff(false);
						LoginManager.Instance.ToggleLoadingSpinnerOnOff(false);
						LoginManager.Instance.SaveLoginStatus();
						LoginManager.Instance.DisableInputFieldValidation();

						ProfileAPIManager.Instance.Profile();
					}
					else
					{
						LoginManager.Instance.ToggleLoadingSpinnerOnOff(false);
						LoginManager.Instance.ShowStatusMessage("Ah! Did you not verify yourself on your email? Did you forget your password? Click on ‘Forgot Password?’ and we will send you the link again.", "failure");
					}
				}
			}
		}
	}

	#endregion

	#region JSON RESPONSE ( LOGIN )

	[Serializable]
	public class User
	{
		public string email;
	}

	[Serializable]
	public class Data
	{
		public string auth;
		public string token;
		public string status;
		public User user;
	}

	[Serializable]
	public class Success
	{
		public string message;
		public Data data;
	}

	[Serializable]
	public class Response
	{
		public Success success;
	}

	#endregion

}
