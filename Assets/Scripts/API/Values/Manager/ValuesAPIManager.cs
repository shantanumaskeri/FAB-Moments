using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class ValuesAPIManager : MonoBehaviour
{

	#region CONSTANTS

	private const string VALUES_API = "https://fabmoments.bellimmersive.com/api/quiz/value/";

	#endregion

	#region SINGLETON INSTANCE

	public static ValuesAPIManager Instance;

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
	public void Values(string method)
	{
		StartCoroutine(ValuesCheck(method));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerator ValuesCheck(string method)
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
			Values(method);
		else
		{
			switch (method)
			{
				case "GET":
					using (UnityWebRequest webRequest = UnityWebRequest.Get(VALUES_API))
					{
						webRequest.SetRequestHeader("Authorization", "TOKEN " + applicationManager.token);
						yield return webRequest.SendWebRequest();

						if (webRequest.isNetworkError)
							Values("GET");
						else
						{
							if (webRequest.downloadHandler != null)
							{
								Response response = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);
								if (response != null)
								{
									if (response.success.message == "success")
										RetrieveValuesInformation(response);
									else
										Values("GET");
								}
								else
									Values("GET");
							}
							else
								Values("GET");
						}
					}
					break;

				case "POST":
					WWWForm form_post = new WWWForm();
					form_post.AddField("valueScore", applicationManager.valueScore);
					form_post.AddField("valueIconsCollected", applicationManager.valueIconsCollected);
					form_post.AddField("fabValue1State", applicationManager.fabValue1);
					form_post.AddField("fabValue2State", applicationManager.fabValue2);
					form_post.AddField("fabValue3State", applicationManager.fabValue3);
					form_post.AddField("fabValue4State", applicationManager.fabValue4);
					form_post.AddField("fabValue5State", applicationManager.fabValue5);
					form_post.AddField("nonValue1State", applicationManager.nonValue1);
					form_post.AddField("nonValue2State", applicationManager.nonValue2);
					form_post.AddField("nonValue3State", applicationManager.nonValue3);
					form_post.AddField("doorState", applicationManager.doorState);
					form_post.AddField("consecutiveCorrectIconsFound", applicationManager.consecutiveCorrectIconsFound);
					form_post.AddField("fabValue1IconName", applicationManager.fabValue1IconName);
					form_post.AddField("fabValue2IconName", applicationManager.fabValue2IconName);
					form_post.AddField("fabValue3IconName", applicationManager.fabValue3IconName);
					form_post.AddField("fabValue4IconName", applicationManager.fabValue4IconName);
					form_post.AddField("fabValue5IconName", applicationManager.fabValue5IconName);
					form_post.AddField("nonValue1IconName", applicationManager.nonValue1IconName);
					form_post.AddField("nonValue2IconName", applicationManager.nonValue2IconName);
					form_post.AddField("nonValue3IconName", applicationManager.nonValue3IconName);

					using (UnityWebRequest webRequest = UnityWebRequest.Post(VALUES_API, form_post))
					{
						webRequest.SetRequestHeader("Authorization", "TOKEN " + applicationManager.token);
						yield return webRequest.SendWebRequest();

						if (webRequest.isNetworkError)
							Values("POST");
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
										Values("POST");
								}
								else
									Values("POST");
							}
							else
								Values("POST");
						}
					}
					break;

				case "DELETE":
					using (UnityWebRequest webRequest = UnityWebRequest.Delete(VALUES_API))
					{
						webRequest.SetRequestHeader("Authorization", "TOKEN " + applicationManager.token);
						yield return webRequest.SendWebRequest();

						if (webRequest.isNetworkError)
							Values("DELETE");
						else
							applicationManager.apisDeleted++;
					}
					break;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void RetrieveValuesInformation(Response response)
	{
		applicationManager.valueScore = response.success.data.valueScore;
		applicationManager.valueIconsCollected = response.success.data.valueIconsCollected;
		applicationManager.fabValue1 = response.success.data.fabValue1State;
		applicationManager.fabValue2 = response.success.data.fabValue2State;
		applicationManager.fabValue3 = response.success.data.fabValue3State;
		applicationManager.fabValue4 = response.success.data.fabValue4State;
		applicationManager.fabValue5 = response.success.data.fabValue5State;
		applicationManager.nonValue1 = response.success.data.nonValue1State;
		applicationManager.nonValue2 = response.success.data.nonValue2State;
		applicationManager.nonValue3 = response.success.data.nonValue3State;
		applicationManager.doorState = response.success.data.doorState;
		applicationManager.consecutiveCorrectIconsFound = response.success.data.consecutiveCorrectIconsFound;
		
		applicationManager.totalScore = applicationManager.valueScore;

		if (applicationManager.valueIconsCollected == 5)
			applicationManager.valueLevelCompleted = 1;

		GameManager.Instance.LoadGameStatus();
	}

	#endregion

	#region JSON RESPONSE ( VALUES )

	[Serializable]
	public class Data
	{
		public int valueScore;
		public int valueIconsCollected;
		public float zPosition;
		public int fabValue1State;
		public int fabValue2State;
		public int fabValue3State;
		public int fabValue4State;
		public int fabValue5State;
		public int nonValue1State;
		public int nonValue2State;
		public int nonValue3State;
		public int doorState;
		public int consecutiveCorrectIconsFound;
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

