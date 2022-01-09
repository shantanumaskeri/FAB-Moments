using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardAPIManager : MonoBehaviour
{

	#region CONSTANTS

	private const string LEADERBOARD_API = "https://fabmoments.bellimmersive.com/api/user/leaderBoard?";

	#endregion

	#region SINGLETON INSTANCE

	public static LeaderboardAPIManager Instance;

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
	public void Leaderboard()
	{
		StartCoroutine(LeaderboardCheck());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerator LeaderboardCheck()
	{
		LeaderboardManager.Instance.ToggleLoadingSpinnerOnOff(true);

		if (Application.internetReachability == NetworkReachability.NotReachable)
			Leaderboard();
		else
		{
			WWWForm form = new WWWForm();
			form.AddField("limit", 10);

			using (UnityWebRequest webRequest = UnityWebRequest.Post(LEADERBOARD_API, form))
			{
				webRequest.SetRequestHeader("Authorization", "TOKEN " + applicationManager.token);
				yield return webRequest.SendWebRequest();

				if (webRequest.isNetworkError)
					Leaderboard();
				else
				{
					if (webRequest.downloadHandler != null)
					{
						Response response = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);
						if (response != null)
						{
							if (response.success.message == "success")
							{
								LeaderboardManager.Instance.ToggleLoadingSpinnerOnOff(false);

								RetrieveLeaderboardInformation(response);
							}
							else
								Leaderboard();
						}
						else
							Leaderboard();
					}
					else
						Leaderboard();
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void RetrieveLeaderboardInformation(Response response)
	{
		LeaderboardManager.Instance.PrepareLeaderboard(response);
	}

	#endregion

	#region JSON RESPONSE ( LEADERBOARD )

	[Serializable]
	public class Data
	{
		public int score;
		public string firstName;
		public string lastName;
		public string avatar;
		public int rank;
	}

	[Serializable]
	public class Success
	{
		public string message;
		public Data[] data;
	}

	[Serializable]
	public class Response
	{
		public Success success;
	}

	#endregion

}
