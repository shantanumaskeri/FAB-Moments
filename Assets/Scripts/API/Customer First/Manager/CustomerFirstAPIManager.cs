using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VideoHelper;
using UnityEngine;
using UnityEngine.Networking;

public class CustomerFirstAPIManager : MonoBehaviour
{

    #region CONSTANTS

    private const string CUSTOMER_FIRST_API = "https://fabmoments.bellimmersive.com/api/quiz/customerFirst/";

    #endregion

    #region SINGLETON INSTANCE

    public static CustomerFirstAPIManager Instance;

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

	private void Initialize()
	{
		applicationManager = FindObjectOfType<ApplicationManager>();
	}

	public void CustomerFirst(string method)
	{
        StartCoroutine(CustomerFirstCheck(method));
	}

    private IEnumerator CustomerFirstCheck(string method)
	{
		if (CustomerFirstManager.Instance != null)
			CustomerFirstManager.Instance.ToggleLoadingSpinnerOnOff(true);

		if (Application.internetReachability == NetworkReachability.NotReachable)
			CustomerFirst(method);
		else
		{
			switch (method)
			{
				case "GET":
					using (UnityWebRequest webRequest = UnityWebRequest.Get(CUSTOMER_FIRST_API))
					{
						webRequest.SetRequestHeader("Authorization", "TOKEN " + applicationManager.token);
						yield return webRequest.SendWebRequest();

						if (webRequest.isNetworkError)
							CustomerFirst("GET");
						else
						{
							if (webRequest.downloadHandler != null)
							{
								Response response = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);
								if (response != null)
								{
									if (response.success.message == "success")
									{
										CustomerFirstManager.Instance.ToggleLoadingSpinnerOnOff(false);

										RetrieveVideoInformation(response);
									}
									else
										CustomerFirst("GET");
								}
								else
									CustomerFirst("GET");
							}
							else
								CustomerFirst("GET");
						}
					}
					break;

				case "POST":
					WWWForm form_post = new WWWForm();
					form_post.AddField("videoId", (int)(VideoController.Instance.videoClipId + 1));
					form_post.AddField("isWatched", 1);
					form_post.AddField("watchCount", VideoController.Instance.numberOfVideosWatched);
					form_post.AddField("videoName", VideoController.Instance.videoNames[VideoController.Instance.videoClipId]);

					using (UnityWebRequest webRequest = UnityWebRequest.Post(CUSTOMER_FIRST_API, form_post))
					{
						webRequest.SetRequestHeader("Authorization", "TOKEN " + applicationManager.token);
						yield return webRequest.SendWebRequest();

						if (webRequest.isNetworkError)
							CustomerFirst("POST");
						else
						{
							if (webRequest.downloadHandler != null)
							{
								Response response = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);
								if (response != null)
								{
									if (response.success.message == "record created successfully" || response.success.message == "success")
										CustomerFirstManager.Instance.ToggleLoadingSpinnerOnOff(false);
									else
										CustomerFirst("POST");
								}
								else
									CustomerFirst("POST");
							}
							else
								CustomerFirst("POST");
						}
					}
					break;

				case "DELETE":
					using (UnityWebRequest webRequest = UnityWebRequest.Delete(CUSTOMER_FIRST_API))
					{
						webRequest.SetRequestHeader("Authorization", "TOKEN " + applicationManager.token);
						yield return webRequest.SendWebRequest();

						if (webRequest.isNetworkError)
							CustomerFirst("DELETE");
						else
							applicationManager.apisDeleted++;
					}
					break;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void RetrieveVideoInformation(Response response)
	{
		int length = response.success.data.Length;
		for (int i = 0; i < length; i++)
		{
			applicationManager.CFVideoNames.Add(response.success.data[i].videoName);
			applicationManager.CFVideoIds.Add((int)(response.success.data[i].videoId - 1));
		}

		applicationManager.watchCount = applicationManager.CFVideoNames.Count;

		CustomerFirstXMLManager.Instance.Initialize();
		CustomerFirstVideoGalleryManager.Instance.Initialize();
		CustomerFirstLandingPageManager.Instance.Initialize();

		VideoController.Instance.UpdateVideosWatchedInformation();
	}

	#endregion

	#region JSON RESPONSE ( CUSTOMER FIRST )

	[Serializable]
	public class Data
	{
		public int videoId;
		public int watchCount;
		public string videoName;
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

