using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class BehavioursAPIManager : MonoBehaviour
{

	#region CONSTANTS

	private const string BEHAVIOURS_API = "https://fabmoments.bellimmersive.com/api/quiz/behaviour?";

	#endregion

	#region SINGLETON INSTANCE

	public static BehavioursAPIManager Instance;

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
	public void Behaviours(string method)
	{
		StartCoroutine(BehavioursCheck(method));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerator BehavioursCheck(string method)
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
			Behaviours(method);
		else
		{
			switch (method)
			{
				case "GET":
					using (UnityWebRequest webRequest = UnityWebRequest.Get(BEHAVIOURS_API))
					{
						webRequest.SetRequestHeader("Authorization", "TOKEN " + applicationManager.token);
						yield return webRequest.SendWebRequest();

						if (webRequest.isNetworkError)
							Behaviours("GET");
						else
						{
							if (webRequest.downloadHandler != null)
							{
								Response response = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);
								if (response != null)
								{
									if (response.success.message == "success")
										RetrieveBehavioursInformation(response);
									else
										Behaviours("GET");
								}
								else
									Behaviours("GET");
							}
							else
								Behaviours("GET");
						}
					}
					break;

				case "POST":
					WWWForm form_post = new WWWForm();
					form_post.AddField("behaviourScore", applicationManager.behaviourScore);
					form_post.AddField("behaviourQuizAnswers", applicationManager.behaviourQuizAnswers);
					form_post.AddField("fabBehaviour1State", applicationManager.fabBehaviour1);
					form_post.AddField("fabBehaviour2State", applicationManager.fabBehaviour2);
					form_post.AddField("fabBehaviour3State", applicationManager.fabBehaviour3);
					form_post.AddField("fabBehaviour4State", applicationManager.fabBehaviour4);
					form_post.AddField("fabBehaviour5State", applicationManager.fabBehaviour5);
					form_post.AddField("teleportState", applicationManager.teleportState);
					form_post.AddField("consecutiveCorrectAnswers", applicationManager.consecutiveCorrectAnswers);
					form_post.AddField("maximumWrongAttempts", applicationManager.maximumWrongAttempts);
					form_post.AddField("fabBehaviour1Question", applicationManager.fabBehaviour1Question);
					form_post.AddField("fabBehaviour1Answer", applicationManager.fabBehaviour1Answer);
					form_post.AddField("fabBehaviour2Question", applicationManager.fabBehaviour2Question);
					form_post.AddField("fabBehaviour2Answer", applicationManager.fabBehaviour2Answer);
					form_post.AddField("fabBehaviour3Question", applicationManager.fabBehaviour3Question);
					form_post.AddField("fabBehaviour3Answer", applicationManager.fabBehaviour3Answer);
					form_post.AddField("fabBehaviour4Question", applicationManager.fabBehaviour4Question);
					form_post.AddField("fabBehaviour4Answer", applicationManager.fabBehaviour4Answer);
					form_post.AddField("fabBehaviour5Question", applicationManager.fabBehaviour5Question);
					form_post.AddField("fabBehaviour5Answer", applicationManager.fabBehaviour5Answer);

					using (UnityWebRequest webRequest = UnityWebRequest.Post(BEHAVIOURS_API, form_post))
					{
						webRequest.SetRequestHeader("Authorization", "TOKEN " + applicationManager.token);
						yield return webRequest.SendWebRequest();

						if (webRequest.isNetworkError)
							Behaviours("POST");
						else
						{
							if (webRequest.downloadHandler != null)
							{
								Response response = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);
								if (response != null)
								{
									if (response.success.message == "record created successfully" || response.success.message == "success")
									{

									}
									else
										Behaviours("POST");
								}
								else
									Behaviours("POST");
							}
							else
								Behaviours("POST");
						}
					}
					break;

				case "DELETE":
					using (UnityWebRequest webRequest = UnityWebRequest.Delete(BEHAVIOURS_API))
					{
						webRequest.SetRequestHeader("Authorization", "TOKEN " + applicationManager.token);
						yield return webRequest.SendWebRequest();

						if (webRequest.isNetworkError)
							Behaviours("DELETE");
						else
							applicationManager.apisDeleted++;
					}
					break;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void RetrieveBehavioursInformation(Response response)
	{
		applicationManager.behaviourScore = response.success.data.behaviourScore;
		applicationManager.behaviourQuizAnswers = response.success.data.behaviourQuizAnswers;
		applicationManager.fabBehaviour1 = response.success.data.fabBehaviour1State;
		applicationManager.fabBehaviour2 = response.success.data.fabBehaviour2State;
		applicationManager.fabBehaviour3 = response.success.data.fabBehaviour3State;
		applicationManager.fabBehaviour4 = response.success.data.fabBehaviour4State;
		applicationManager.fabBehaviour5 = response.success.data.fabBehaviour5State;
		applicationManager.teleportState = response.success.data.teleportState;
		applicationManager.consecutiveCorrectAnswers = response.success.data.consecutiveCorrectAnswers;
		applicationManager.maximumWrongAttempts = response.success.data.maximumWrongAttempts;
		
		applicationManager.totalScore = applicationManager.valueScore + applicationManager.behaviourScore;

		if (applicationManager.behaviourQuizAnswers == 5)
			applicationManager.behaviourLevelCompleted = 1;

		GameManager.Instance.LoadGameStatus();
	}

	#endregion

	#region JSON RESPONSE ( BEHAVIOURS )

	[Serializable]
	public class Data
	{
		public int behaviourScore;
		public int behaviourQuizAnswers;
		public float zPosition;
		public int fabBehaviour1State;
		public int fabBehaviour2State;
		public int fabBehaviour3State;
		public int fabBehaviour4State;
		public int fabBehaviour5State;
		public int teleportState;
		public int consecutiveCorrectAnswers;
		public int maximumWrongAttempts;
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

