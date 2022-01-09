using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ScenariosAPIManager : MonoBehaviour
{

	#region CONSTANTS

	private const string SCENARIOS_API = "https://fabmoments.bellimmersive.com/api/quiz/scenario/";
	//private const string SCENARIOS_API = "http://3.15.8.226:3100/api/quiz/scenario/";
	#endregion

	#region SINGLETON INSTANCE

	public static ScenariosAPIManager Instance;

	#endregion

	#region PRIVATE VARIABLES

	private int currentSceneIndex;

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
		currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

		applicationManager = FindObjectOfType<ApplicationManager>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Scenarios(string method)
	{
		StartCoroutine(ScenariosCheck(method));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerator ScenariosCheck(string method)
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
			Scenarios(method);
		else
		{
			switch (method)
			{
				case "GET":
					using (UnityWebRequest webRequest = UnityWebRequest.Get(SCENARIOS_API))
					{
						webRequest.SetRequestHeader("Authorization", "TOKEN " + applicationManager.token);
						yield return webRequest.SendWebRequest();

						if (webRequest.isNetworkError)
							Scenarios("GET");
						else
						{
							if (webRequest.downloadHandler != null)
							{
								Response response = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);
								if (response != null)
								{
									if (response.success.message == "success")
										RetrieveScenariosInformation(response);
									else
										Scenarios("GET");
								}
								else
									Scenarios("GET");
							}
							else
								Scenarios("GET");
						}
					}
					break;

				case "POST":
					WWWForm form_post = new WWWForm();
					form_post.AddField("scenarioScore", applicationManager.scenarioScore);
					form_post.AddField("scenario1Score", applicationManager.scenario1Score);
					form_post.AddField("scenario2Score", applicationManager.scenario2Score);
					form_post.AddField("scenario3Score", applicationManager.scenario3Score);
					form_post.AddField("scenario4Score", applicationManager.scenario4Score);
					form_post.AddField("consecutiveCorrectAnswers", applicationManager.consecutiveCorrectAnswers);
					
					form_post.AddField("cibSwitchState", applicationManager.cibSwitchState);
					form_post.AddField("pbg1SwitchState", applicationManager.pbg1SwitchState);
					form_post.AddField("pbg2SwitchState", applicationManager.pbg2SwitchState);
					form_post.AddField("cfSwitchState", applicationManager.cfSwitchState);
					form_post.AddField("enabSwitchState", applicationManager.enabSwitchState);

					form_post.AddField("pbgScenarioUnlocked", applicationManager.pbgScenarioUnlocked);
					form_post.AddField("cibScenarioUnlocked", applicationManager.cibScenarioUnlocked);
					form_post.AddField("cfScenarioUnlocked", applicationManager.cfScenarioUnlocked);
					form_post.AddField("enabScenarioUnlocked", applicationManager.enabScenarioUnlocked);

					form_post.AddField("pbgScenarioCompleted", applicationManager.pbgScenarioCompleted);
					form_post.AddField("cibScenarioCompleted", applicationManager.cibScenarioCompleted);
					form_post.AddField("cfScenarioCompleted", applicationManager.cfScenarioCompleted);
					form_post.AddField("enabScenarioCompleted", applicationManager.enabScenarioCompleted);

					form_post.AddField("scenario1SubLevelsCompleted", applicationManager.scenario1SubLevelsCompleted);
					form_post.AddField("scenario2SubLevelsCompleted", applicationManager.scenario2SubLevelsCompleted);
					form_post.AddField("scenario3SubLevelsCompleted", applicationManager.scenario3SubLevelsCompleted);
					form_post.AddField("scenario4SubLevelsCompleted", applicationManager.scenario4SubLevelsCompleted);

					form_post.AddField("fabScenario1Question1", applicationManager.fabScenario1Question1);
					form_post.AddField("fabScenario1Answer1", applicationManager.fabScenario1Answer1);
					form_post.AddField("fabScenario1Question2", applicationManager.fabScenario1Question2);
					form_post.AddField("fabScenario1Answer2", applicationManager.fabScenario1Answer2);
					form_post.AddField("fabScenario1Question3", applicationManager.fabScenario1Question3);
					form_post.AddField("fabScenario1Answer3", applicationManager.fabScenario1Answer3);
					form_post.AddField("fabScenario1Question4", applicationManager.fabScenario1Question4);
					form_post.AddField("fabScenario1Answer4", applicationManager.fabScenario1Answer4);
					
					form_post.AddField("fabScenario2Question1", applicationManager.fabScenario2Question1);
					form_post.AddField("fabScenario2Answer1", applicationManager.fabScenario2Answer1);
					form_post.AddField("fabScenario2Question2", applicationManager.fabScenario2Question2);
					form_post.AddField("fabScenario2Answer2", applicationManager.fabScenario2Answer2);
					form_post.AddField("fabScenario2Question3", applicationManager.fabScenario2Question3);
					form_post.AddField("fabScenario2Answer3", applicationManager.fabScenario2Answer3);
					form_post.AddField("fabScenario2Question4", applicationManager.fabScenario2Question4);
					form_post.AddField("fabScenario2Answer4", applicationManager.fabScenario2Answer4);
					form_post.AddField("fabScenario2Question5", applicationManager.fabScenario2Question5);
					form_post.AddField("fabScenario2Answer5", applicationManager.fabScenario2Answer5);
					form_post.AddField("fabScenario2Question6", applicationManager.fabScenario2Question6);
					form_post.AddField("fabScenario2Answer6", applicationManager.fabScenario2Answer6);
					form_post.AddField("fabScenario2Question7", applicationManager.fabScenario2Question7);
					form_post.AddField("fabScenario2Answer7", applicationManager.fabScenario2Answer7);
					form_post.AddField("fabScenario2Question8", applicationManager.fabScenario2Question8);
					form_post.AddField("fabScenario2Answer8", applicationManager.fabScenario2Answer8);
					
					form_post.AddField("fabScenario3Question1", applicationManager.fabScenario3Question1);
					form_post.AddField("fabScenario3Answer1", applicationManager.fabScenario3Answer1);
					form_post.AddField("fabScenario3Question2", applicationManager.fabScenario3Question2);
					form_post.AddField("fabScenario3Answer2", applicationManager.fabScenario3Answer2);
					form_post.AddField("fabScenario3Question3", applicationManager.fabScenario3Question3);
					form_post.AddField("fabScenario3Answer3", applicationManager.fabScenario3Answer3);
					form_post.AddField("fabScenario3Question4", applicationManager.fabScenario3Question4);
					form_post.AddField("fabScenario3Answer4", applicationManager.fabScenario3Answer4);
					form_post.AddField("fabScenario3Question5", applicationManager.fabScenario3Question5);
					form_post.AddField("fabScenario3Answer5", applicationManager.fabScenario3Answer5);
					form_post.AddField("fabScenario3Question6", applicationManager.fabScenario3Question6);
					form_post.AddField("fabScenario3Answer6", applicationManager.fabScenario3Answer6);
					form_post.AddField("fabScenario3Question7", applicationManager.fabScenario3Question7);
					form_post.AddField("fabScenario3Answer7", applicationManager.fabScenario3Answer7);
					form_post.AddField("fabScenario3Question8", applicationManager.fabScenario3Question8);
					form_post.AddField("fabScenario3Answer8", applicationManager.fabScenario3Answer8);
					form_post.AddField("fabScenario3Question9", applicationManager.fabScenario3Question9);
					form_post.AddField("fabScenario3Answer9", applicationManager.fabScenario3Answer9);

					form_post.AddField("fabScenario4Question1", applicationManager.fabScenario4Question1);
					form_post.AddField("fabScenario4Answer1", applicationManager.fabScenario4Answer1);
					form_post.AddField("fabScenario4Question2", applicationManager.fabScenario4Question2);
					form_post.AddField("fabScenario4Answer2", applicationManager.fabScenario4Answer2);
					form_post.AddField("fabScenario4Question3", applicationManager.fabScenario4Question3);
					form_post.AddField("fabScenario4Answer3", applicationManager.fabScenario4Answer3);
					form_post.AddField("fabScenario4Question4", applicationManager.fabScenario4Question4);
					form_post.AddField("fabScenario4Answer4", applicationManager.fabScenario4Answer4);
					form_post.AddField("fabScenario4Question5", applicationManager.fabScenario4Question5);
					form_post.AddField("fabScenario4Answer5", applicationManager.fabScenario4Answer5);

					using (UnityWebRequest webRequest = UnityWebRequest.Post(SCENARIOS_API, form_post))
					{
						webRequest.SetRequestHeader("Authorization", "TOKEN " + applicationManager.token);
						yield return webRequest.SendWebRequest();

						if (webRequest.isNetworkError)
							Scenarios("POST");
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
										Scenarios("POST");
								}
								else
									Scenarios("POST");
							}
							else
								Scenarios("POST");
						}
					}
					break;

				case "DELETE":
					using (UnityWebRequest webRequest = UnityWebRequest.Delete(SCENARIOS_API))
					{
						webRequest.SetRequestHeader("Authorization", "TOKEN " + applicationManager.token);
						yield return webRequest.SendWebRequest();

						if (webRequest.isNetworkError)
							Scenarios("DELETE");
						else
							applicationManager.apisDeleted++;
					}
					break;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void RetrieveScenariosInformation(Response response)
	{
		applicationManager.scenarioScore = response.success.data.scenarioScore;
		applicationManager.scenario1Score = response.success.data.scenario1Score;
		applicationManager.scenario2Score = response.success.data.scenario2Score;
		applicationManager.scenario3Score = response.success.data.scenario3Score;
		applicationManager.scenario4Score = response.success.data.scenario4Score;
		applicationManager.consecutiveCorrectAnswers = response.success.data.consecutiveCorrectAnswers;
		
		applicationManager.cibSwitchState = response.success.data.cibSwitchState;
		applicationManager.pbg1SwitchState = response.success.data.pbg1SwitchState;
		applicationManager.pbg2SwitchState = response.success.data.pbg2SwitchState;
		applicationManager.cfSwitchState = response.success.data.cfSwitchState;
		applicationManager.enabSwitchState = response.success.data.enabSwitchState;

		applicationManager.scenario1SubLevelsCompleted = response.success.data.scenario1SubLevelsCompleted;
		applicationManager.scenario2SubLevelsCompleted = response.success.data.scenario2SubLevelsCompleted;
		applicationManager.scenario3SubLevelsCompleted = response.success.data.scenario3SubLevelsCompleted;
		applicationManager.scenario4SubLevelsCompleted = response.success.data.scenario4SubLevelsCompleted;

		applicationManager.pbgScenarioUnlocked = response.success.data.pbgScenarioUnlocked;
		applicationManager.cibScenarioUnlocked = response.success.data.cibScenarioUnlocked;
		applicationManager.cfScenarioUnlocked = response.success.data.cfScenarioUnlocked;
		applicationManager.enabScenarioUnlocked = response.success.data.enabScenarioUnlocked;

		applicationManager.pbgScenarioCompleted = response.success.data.pbgScenarioCompleted;
		applicationManager.cibScenarioCompleted = response.success.data.cibScenarioCompleted;
		applicationManager.cfScenarioCompleted = response.success.data.cfScenarioCompleted;
		applicationManager.enabScenarioCompleted = response.success.data.enabScenarioCompleted;

		applicationManager.totalScore = applicationManager.valueScore + applicationManager.behaviourScore + applicationManager.scenario1Score + applicationManager.scenario2Score + applicationManager.scenario3Score + applicationManager.scenario4Score;
		
		if (currentSceneIndex == 8)
			GameManager.Instance.LoadGameStatus();
		else if (currentSceneIndex == 7)
			ScenarioSelectionManager.Instance.CheckDepartmentToUnlockScenario();
	}

	#endregion

	#region JSON RESPONSE ( SCENARIOS )

	[Serializable]
	public class Data
	{
		public int scenarioScore;
		public int scenario1Score;
		public int scenario2Score;
		public int scenario3Score;
		public int scenario4Score;
		public int consecutiveCorrectAnswers;
		public int cibSwitchState;
		public int pbg1SwitchState;
		public int pbg2SwitchState;
		public int cfSwitchState;
		public int enabSwitchState;
		public int scenario1SubLevelsCompleted;
		public int scenario2SubLevelsCompleted;
		public int scenario3SubLevelsCompleted;
		public int scenario4SubLevelsCompleted;
		public int pbgScenarioUnlocked;
		public int cibScenarioUnlocked;
		public int cfScenarioUnlocked;
		public int enabScenarioUnlocked;
		public int pbgScenarioCompleted;
		public int cibScenarioCompleted;
		public int cfScenarioCompleted;
		public int enabScenarioCompleted;
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

