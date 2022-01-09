using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{

	#region PUBLIC FIELDS

	#region LOGIN API

	[HideInInspector]
	public int loggedIn;

	[HideInInspector]
	public string token;

	#endregion

	#region PROFILE API

	[HideInInspector]
	public int avatarSelected;

	[HideInInspector]
	public string playerFirstName;

	[HideInInspector]
	public string playerLastName;

	[HideInInspector]
	public string playerEmailAddress;

	[HideInInspector]
	public string playerDepartment;

	#endregion

	#region CUSTOMER FIRST API

	[HideInInspector]
	public int watchCount;

	[HideInInspector]
	public List<string> CFVideoNames = new List<string>();

	[HideInInspector]
	public List<int> CFVideoIds = new List<int>();

	#endregion

	#region VALUES API

	[HideInInspector]
	public int valueScore;

	[HideInInspector]
	public int valueIconsCollected;

	[HideInInspector]
	public int fabValue1;

	[HideInInspector]
	public int fabValue2;

	[HideInInspector]
	public int fabValue3;

	[HideInInspector]
	public int fabValue4;

	[HideInInspector]
	public int fabValue5;

	[HideInInspector]
	public int nonValue1;

	[HideInInspector]
	public int nonValue2;

	[HideInInspector]
	public int nonValue3;

	[HideInInspector]
	public int doorState;

	[HideInInspector]
	public int consecutiveCorrectIconsFound;

	[HideInInspector]
	public int valueLevelCompleted;

	[HideInInspector]
	public string fabValue1IconName;

	[HideInInspector]
	public string fabValue2IconName;

	[HideInInspector]
	public string fabValue3IconName;

	[HideInInspector]
	public string fabValue4IconName;

	[HideInInspector]
	public string fabValue5IconName;

	[HideInInspector]
	public string nonValue1IconName;

	[HideInInspector]
	public string nonValue2IconName;

	[HideInInspector]
	public string nonValue3IconName;

	#endregion

	#region BEHAVIOURS API

	[HideInInspector]
	public int behaviourScore;

	[HideInInspector]
	public int fabBehaviour1;

	[HideInInspector]
	public int fabBehaviour2;

	[HideInInspector]
	public int fabBehaviour3;

	[HideInInspector]
	public int fabBehaviour4;

	[HideInInspector]
	public int fabBehaviour5;

	[HideInInspector]
	public int behaviourQuizAnswers;

	[HideInInspector]
	public int teleportState;

	[HideInInspector]
	public int consecutiveCorrectAnswers;

	[HideInInspector]
	public int maximumWrongAttempts;

	[HideInInspector]
	public int behaviourLevelCompleted;

	[HideInInspector]
	public string fabBehaviour1Question;

	[HideInInspector]
	public string fabBehaviour2Question;

	[HideInInspector]
	public string fabBehaviour3Question;

	[HideInInspector]
	public string fabBehaviour4Question;

	[HideInInspector]
	public string fabBehaviour5Question;

	[HideInInspector]
	public string fabBehaviour1Answer;

	[HideInInspector]
	public string fabBehaviour2Answer;

	[HideInInspector]
	public string fabBehaviour3Answer;

	[HideInInspector]
	public string fabBehaviour4Answer;

	[HideInInspector]
	public string fabBehaviour5Answer;

	#endregion

	#region SCENARIOS API

	[HideInInspector]
	public int scenarioScore;

	[HideInInspector]
	public int scenario1Score;

	[HideInInspector]
	public int scenario2Score;

	[HideInInspector]
	public int scenario3Score;

	[HideInInspector]
	public int scenario4Score;

	[HideInInspector]
	public int cibSwitchState;

	[HideInInspector]
	public int pbg1SwitchState;

	[HideInInspector]
	public int pbg2SwitchState;

	[HideInInspector]
	public int cfSwitchState;

	[HideInInspector]
	public int enabSwitchState;

	[HideInInspector]
	public int scenario1SubLevelsCompleted;

	[HideInInspector]
	public int scenario2SubLevelsCompleted;

	[HideInInspector]
	public int scenario3SubLevelsCompleted;

	[HideInInspector]
	public int scenario4SubLevelsCompleted;

	[HideInInspector]
	public string fabScenario1Question1;

	[HideInInspector]
	public string fabScenario1Question2;

	[HideInInspector]
	public string fabScenario1Question3;

	[HideInInspector]
	public string fabScenario1Question4;

	[HideInInspector]
	public string fabScenario1Answer1;

	[HideInInspector]
	public string fabScenario1Answer2;

	[HideInInspector]
	public string fabScenario1Answer3;

	[HideInInspector]
	public string fabScenario1Answer4;

	[HideInInspector]
	public string fabScenario2Question1;

	[HideInInspector]
	public string fabScenario2Question2;

	[HideInInspector]
	public string fabScenario2Question3;

	[HideInInspector]
	public string fabScenario2Question4;

	[HideInInspector]
	public string fabScenario2Question5;

	[HideInInspector]
	public string fabScenario2Question6;

	[HideInInspector]
	public string fabScenario2Question7;

	[HideInInspector]
	public string fabScenario2Question8;

	[HideInInspector]
	public string fabScenario2Answer1;

	[HideInInspector]
	public string fabScenario2Answer2;

	[HideInInspector]
	public string fabScenario2Answer3;

	[HideInInspector]
	public string fabScenario2Answer4;

	[HideInInspector]
	public string fabScenario2Answer5;

	[HideInInspector]
	public string fabScenario2Answer6;

	[HideInInspector]
	public string fabScenario2Answer7;

	[HideInInspector]
	public string fabScenario2Answer8;

	[HideInInspector]
	public string fabScenario3Question1;

	[HideInInspector]
	public string fabScenario3Question2;

	[HideInInspector]
	public string fabScenario3Question3;

	[HideInInspector]
	public string fabScenario3Question4;

	[HideInInspector]
	public string fabScenario3Question5;

	[HideInInspector]
	public string fabScenario3Question6;

	[HideInInspector]
	public string fabScenario3Question7;

	[HideInInspector]
	public string fabScenario3Question8;

	[HideInInspector]
	public string fabScenario3Question9;

	[HideInInspector]
	public string fabScenario3Answer1;

	[HideInInspector]
	public string fabScenario3Answer2;

	[HideInInspector]
	public string fabScenario3Answer3;

	[HideInInspector]
	public string fabScenario3Answer4;

	[HideInInspector]
	public string fabScenario3Answer5;

	[HideInInspector]
	public string fabScenario3Answer6;

	[HideInInspector]
	public string fabScenario3Answer7;

	[HideInInspector]
	public string fabScenario3Answer8;

	[HideInInspector]
	public string fabScenario3Answer9;

	[HideInInspector]
	public string fabScenario4Question1;

	[HideInInspector]
	public string fabScenario4Question2;

	[HideInInspector]
	public string fabScenario4Question3;

	[HideInInspector]
	public string fabScenario4Question4;

	[HideInInspector]
	public string fabScenario4Question5;

	[HideInInspector]
	public string fabScenario4Answer1;

	[HideInInspector]
	public string fabScenario4Answer2;

	[HideInInspector]
	public string fabScenario4Answer3;

	[HideInInspector]
	public string fabScenario4Answer4;

	[HideInInspector]
	public string fabScenario4Answer5;

	[HideInInspector]
	public int pbgScenarioUnlocked;

	[HideInInspector]
	public int cibScenarioUnlocked;

	[HideInInspector]
	public int cfScenarioUnlocked;

	[HideInInspector]
	public int enabScenarioUnlocked;

	[HideInInspector]
	public int pbgScenarioCompleted;

	[HideInInspector]
	public int cibScenarioCompleted;

	[HideInInspector]
	public int cfScenarioCompleted;

	[HideInInspector]
	public int enabScenarioCompleted;

	#endregion

	#region SHARED VARIABLES

	[HideInInspector]
	public int totalScore;

	[HideInInspector]
	public int launchDay;

	[HideInInspector]
	public int terminationDay;

	[HideInInspector]
	public int apisDeleted;

	[HideInInspector]
	public int gameInstructionsViewed;

	[HideInInspector]
	public int cfInstructionsViewed;

	[HideInInspector]
	public string selectedDepartment;

	/*[HideInInspector]
	public string playerGender;*/

	#endregion

	#endregion

	#region PRIVATE VARIABLES

	private DateTime currentDateTime;

	#endregion

	#region UNITY MONOBEHAVIOURS

	private void Start()
	{
		Initialize();
	}

	private void OnApplicationQuit()
	{
		SetTimeOfApplicationTermination();
		CancelInvoke();
		StopAllCoroutines();
	}

	#endregion

	#region CUSTOM METHODS

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize()
	{
		Application.targetFrameRate = 60;

		//PlayerPrefs.DeleteAll();

		LoadVariablesSavedInPlayerPrefs();
		SetTimeOfApplicationLaunch();
		CheckSessionTimeoutStatus();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LoadVariablesSavedInPlayerPrefs()
	{
		loggedIn = PlayerPrefs.GetInt("LoggedIn");
		token = PlayerPrefs.GetString("Token");
		launchDay = PlayerPrefs.GetInt("LaunchDay");
		terminationDay = PlayerPrefs.GetInt("TerminationDay");
		gameInstructionsViewed = PlayerPrefs.GetInt("GameInstructionsViewed");
		cfInstructionsViewed = PlayerPrefs.GetInt("CFInstructionsViewed");
		
		//Debug.Log(token);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void DeleteVariablesSavedInPlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetTimeOfApplicationLaunch()
	{
		currentDateTime = DateTime.Now;
		launchDay = currentDateTime.DayOfYear;
		PlayerPrefs.SetInt("LaunchDay", launchDay);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetTimeOfApplicationTermination()
	{
		currentDateTime = DateTime.Now;
		terminationDay = currentDateTime.DayOfYear;
		PlayerPrefs.SetInt("TerminationDay", terminationDay);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckSessionTimeoutStatus()
	{
		if ((launchDay - terminationDay) >= 7)
		{
			DeleteVariablesSavedInPlayerPrefs();
			LoadVariablesSavedInPlayerPrefs();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Reset()
	{
		totalScore = valueIconsCollected = valueLevelCompleted = valueLevelCompleted = fabValue1 = fabValue2 = fabValue3 = fabValue4 = fabValue5 = nonValue1 = nonValue2 = nonValue3 = consecutiveCorrectIconsFound = doorState = behaviourLevelCompleted = behaviourQuizAnswers = fabBehaviour1 = fabBehaviour2 = fabBehaviour3 = fabBehaviour4 = fabBehaviour5 = teleportState = consecutiveCorrectAnswers = maximumWrongAttempts = scenarioScore = scenario1Score = scenario2Score = scenario3Score = scenario4Score = watchCount = apisDeleted = 0;

		CFVideoIds.Clear();
		CFVideoIds = new List<int>();

		CFVideoNames.Clear();
		CFVideoNames = new List<string>();
	}

	#endregion

}
