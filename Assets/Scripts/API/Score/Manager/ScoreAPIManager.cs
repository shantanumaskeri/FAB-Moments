using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreAPIManager : MonoBehaviour
{

	#region CONSTANTS

	private const string SCORE_API = "https://fabmoments.bellimmersive.com/api/user/score/";

	#endregion

	#region SINGLETON INSTANCE

	public static ScoreAPIManager Instance;

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
	public void Score()
	{
		StartCoroutine(ScoreCheck());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerator ScoreCheck()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
			Score();
		else
		{
			WWWForm form = new WWWForm();
			form.AddField("score", applicationManager.totalScore);

			using (UnityWebRequest webRequest = UnityWebRequest.Post(SCORE_API, form))
			{
				webRequest.SetRequestHeader("Authorization", "TOKEN " + applicationManager.token);
				yield return webRequest.SendWebRequest();

				if (webRequest.isNetworkError)
					Score();
				else
				{
					if (webRequest.downloadHandler != null)
					{
						Response response = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);
						if (response != null)
						{
							if (response.success.message == "success")
								yield return null;
							else
								Score();
						}
						else
							Score();
					}
					else
						Score();
				}
			}
		}
	}

	#endregion

	#region JSON RESPONSE ( SCORE )

	[Serializable]
	public class Success
	{
		public string message;
	}

	[Serializable]
	public class Response
	{
		public Success success;
	}

	#endregion

}
