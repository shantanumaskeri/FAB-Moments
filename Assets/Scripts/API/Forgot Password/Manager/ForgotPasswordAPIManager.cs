using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class ForgotPasswordAPIManager : MonoBehaviour
{

	#region CONSTANTS

	private const string FORGOT_PASSWORD_API = "https://fabmoments.bellimmersive.com/api/auth/forgetPassword?";

	#endregion

	#region SINGLETON INSTANCE

	public static ForgotPasswordAPIManager Instance;

	#endregion

	#region PRIVATE VARIABLES

	private string Email;

	#endregion

	#region UNITY MONOBEHAVIOURS

	private void Start()
	{
		Instance = this;
	}

	#endregion

	#region CUSTOM METHODS

	public void ForgotPassword()
	{
		StartCoroutine(ForgotPasswordCheck());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerator ForgotPasswordCheck()
	{
		LoginManager.Instance.ToggleLoadingSpinnerOnOff(true);

		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			LoginManager.Instance.ToggleLoadingSpinnerOnOff(false);
			LoginManager.Instance.ShowStatusMessage("Please check your internet connection.", "failure");
		}
		else
		{
			Email = LoginManager.Instance.loginEmail.text;

			WWWForm form = new WWWForm();
			form.AddField("email", Email);

			using (UnityWebRequest webRequest = UnityWebRequest.Post(FORGOT_PASSWORD_API, form))
			{
				yield return webRequest.SendWebRequest();

				if (webRequest.isNetworkError)
				{
					LoginManager.Instance.ToggleLoadingSpinnerOnOff(false);
					LoginManager.Instance.ShowStatusMessage(webRequest.error + ". Please try again later.", "failure");
				}
				else
				{
					Response response = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);
					if (response.success.message == "success")
					{
						LoginManager.Instance.ToggleLoadingSpinnerOnOff(false);
						LoginManager.Instance.ShowStatusMessage("Please check your inbox and click on the received link to reset your password.", "");
					}
					else
					{
						LoginManager.Instance.ToggleLoadingSpinnerOnOff(false);
						LoginManager.Instance.ShowStatusMessage("You have entered an incorrect email address.", "failure");
					}
				}
			}
		}
	}

	#endregion

	#region JSON RESPONSE ( FORGOT PASSWORD )

	[Serializable]
	public class Data
	{
		public string status;
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
