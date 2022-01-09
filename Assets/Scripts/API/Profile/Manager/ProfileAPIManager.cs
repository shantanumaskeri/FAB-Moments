using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class ProfileAPIManager : MonoBehaviour
{

	#region CONSTANTS

	private const string PROFILE_API = "https://fabmoments.bellimmersive.com/api/user/profile?";

	#endregion

	#region SINGLETON INSTANCE

	public static ProfileAPIManager Instance;

	#endregion

	#region PRIVATE VARIABLES

	private int currentSceneIndex;

	private ApplicationManager applicationManager;

	#endregion

	#region UNITY MONOBEHAVIOURS

	private void Start()
	{
		Instance = this;

		currentSceneIndex = ScenesManager.Instance.currentSceneIndex;

		applicationManager = FindObjectOfType<ApplicationManager>();
	}

	#endregion

	#region CUSTOM METHODS

	public void Profile()
	{
		StartCoroutine(ProfileCheck());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerator ProfileCheck()
	{
		ToggleLoadingSpinnerOnOffBasedOnSceneLoaded(true);
		
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			ToggleLoadingSpinnerOnOffBasedOnSceneLoaded(false);
			ShowStatusMessageBasedOnSceneLoaded("Please check your internet connection.");
		}
		else
		{
			if (applicationManager.token == "")
			{
				ToggleLoadingSpinnerOnOffBasedOnSceneLoaded(false);

				ScenesManager.Instance.ChangeSceneManual();

				yield return null;
			}
			else
			{
				using (UnityWebRequest webRequest = UnityWebRequest.Get(PROFILE_API))
				{
					webRequest.SetRequestHeader("Authorization", "TOKEN " + applicationManager.token);
					yield return webRequest.SendWebRequest();

					if (webRequest.isNetworkError)
					{
						ToggleLoadingSpinnerOnOffBasedOnSceneLoaded(false);
						ShowStatusMessageBasedOnSceneLoaded(webRequest.error+". Please try again later.");
					}
					else
					{
						Response response = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);
						if (response.success.message == "success")
						{
							ToggleLoadingSpinnerOnOffBasedOnSceneLoaded(false);
							ShowStatusMessageBasedOnSceneLoaded(currentSceneIndex);

							RetrievePlayerInformation(response);
						}
						else
						{
							ToggleLoadingSpinnerOnOffBasedOnSceneLoaded(false);
							//ShowStatusMessageBasedOnSceneLoaded("An error has occurred. Please try again.");

							PlayerPrefs.DeleteAll();

							applicationManager.LoadVariablesSavedInPlayerPrefs();

							ScenesManager.Instance.ChangeSceneManual();
						}
					}
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void RetrievePlayerInformation(Response response)
	{
		applicationManager.playerFirstName = response.success.data.firstName;
		applicationManager.playerLastName = response.success.data.lastName;
		applicationManager.playerEmailAddress = response.success.data.email;
		applicationManager.playerDepartment = response.success.data.department;

		if (response.success.data.avatar != "null")
		{
			string avatarID = response.success.data.avatar.Substring(response.success.data.avatar.Length - 1);
			applicationManager.avatarSelected = Convert.ToInt32(avatarID);
		}

		if (currentSceneIndex == 2)
			LaunchManager.Instance.SaveLoginAndAvatarStatus();
		else if (currentSceneIndex == 3)
			LoginManager.Instance.SaveAvatarStatus();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ToggleLoadingSpinnerOnOffBasedOnSceneLoaded(bool value)
	{
		switch (currentSceneIndex)
		{
			case 2:
				LaunchManager.Instance.ToggleLoadingSpinnerOnOff(value);
			break;

			case 3:
				LoginManager.Instance.ToggleLoadingSpinnerOnOff(value);
				break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ShowStatusMessageBasedOnSceneLoaded(string message)
	{
		switch (currentSceneIndex)
		{
			case 2:
				LaunchManager.Instance.ShowStatusMessage(message, "failure");
				break;

			case 3:
				LoginManager.Instance.ShowStatusMessage(message, "failure");
				break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ShowStatusMessageBasedOnSceneLoaded(int sceneIndex)
	{
		switch (sceneIndex)
		{
			case 2:
				LaunchManager.Instance.ShowStatusMessage("Successfully found player information.", "success");
				break;

			case 3:
				LoginManager.Instance.ShowStatusMessage("Successfully logged in.", "success");
				break;
		}
	}

	#endregion

	#region JSON RESPONSE ( PROFILE )

	[Serializable]
	public class Data
	{
		public string _id;
		public string email;
		public string firstName;
		public string lastName;
		public string avatar;
		public string department; 
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
