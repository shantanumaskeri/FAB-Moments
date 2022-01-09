using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class AvatarAPIManager : MonoBehaviour
{

	#region CONSTANTS

	private const string AVATAR_SELECTION_API = "https://fabmoments.bellimmersive.com/api/user/avatar?";

	#endregion

	#region SINGLETON INSTANCE

	public static AvatarAPIManager Instance;

	#endregion

	#region PRIVATE VARIABLES

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
		applicationManager = FindObjectOfType<ApplicationManager>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AvatarSelection()
	{
		StartCoroutine(AvatarSelectionCheck());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerator AvatarSelectionCheck()
	{
		AvatarSelectionManager.Instance.ToggleLoadingSpinnerOnOff(true);

		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			AvatarSelectionManager.Instance.ToggleLoadingSpinnerOnOff(false);
			AvatarSelectionManager.Instance.ShowStatusMessage("Please check your internet connection.", "failure");
		}
		else
		{
			WWWForm form = new WWWForm();
			form.AddField("avatar", "avatar"+AvatarSelectionManager.Instance.avatarID);

			using (UnityWebRequest webRequest = UnityWebRequest.Post(AVATAR_SELECTION_API, form))
			{
				webRequest.SetRequestHeader("Authorization", "TOKEN " + applicationManager.token);
				yield return webRequest.SendWebRequest();

				if (webRequest.isNetworkError)
				{
					AvatarSelectionManager.Instance.ToggleLoadingSpinnerOnOff(false);
					AvatarSelectionManager.Instance.ShowStatusMessage(webRequest.error+". Please try again later.", "failure");
				}
				else
				{
					Response response = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);
					if (response.success.message == "success")
					{
						AvatarSelectionManager.Instance.ToggleLoadingSpinnerOnOff(false);
						AvatarSelectionManager.Instance.ShowStatusMessage("Avatar selected successfully.", "success");

						AvatarSelectionManager.Instance.SaveAvatarSelectionStatus();
					}
					else
					{
						AvatarSelectionManager.Instance.ToggleLoadingSpinnerOnOff(false);
						AvatarSelectionManager.Instance.ShowStatusMessage("An error occurred. Please try again.", "failure");
					}
				}
			}
		}
	}

	#endregion

	#region JSON RESPONSE ( AVATAR )

	[Serializable]
	public class Success
	{
		public string message;
		public string data;
	}

	[Serializable]
	public class Response
	{
		public Success success;
	}

	#endregion

}
