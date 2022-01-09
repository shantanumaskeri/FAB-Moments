using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static ScoreManager Instance;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Text References")]
	[SerializeField]
	private Text scoreTextHUD;

	[SerializeField]
	private Text scoreTextProfile;

	#endregion

	#region PRIVATE VARIABLES

	private bool incrementing;
	private bool decrementing;
	private int currentScore;
	private int scoreValue;
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
	public void ConfigureScore()
	{
		UpdateScoreLimit();
		DisplayScore();

		LeaderboardManager.Instance.PreparePlayerDetails();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void IncrementScore(int value)
	{
		incrementing = true;
		decrementing = false;

		scoreValue = value;

		CancelInvoke("CheckScore");
		InvokeRepeating(nameof(CheckScore), 0.05f, 0.05f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DecrementScore(int value)
	{
		decrementing = true;
		incrementing = false;

		scoreValue = value;

		CancelInvoke("CheckScore");
		InvokeRepeating(nameof(CheckScore), 0.05f, 0.05f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckScore()
	{
		if (incrementing)
		{
			UpdateScoresBy(1);
			
			if (applicationManager.totalScore >= (currentScore + scoreValue))
				SaveScores();
		}
		else if (decrementing)
		{
			UpdateScoresBy(-1);
			
			if (applicationManager.totalScore <= (currentScore - scoreValue))
				SaveScores();
		}

		DisplayScore();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void UpdateScoresBy(int value)
	{
		if (applicationManager.valueLevelCompleted == 0 && applicationManager.behaviourLevelCompleted == 0)
			applicationManager.valueScore += value;

		if (applicationManager.valueLevelCompleted == 1 && applicationManager.behaviourLevelCompleted == 0)
			applicationManager.behaviourScore += value;

		if (applicationManager.valueLevelCompleted == 1 && applicationManager.behaviourLevelCompleted == 1)
			applicationManager.scenarioScore += value;

		if (applicationManager.valueLevelCompleted == 1 && applicationManager.behaviourLevelCompleted == 1)
		{
			if (applicationManager.selectedDepartment == "Corporate & Investment Banking Group")
				applicationManager.scenario1Score += value;

			if (applicationManager.selectedDepartment == "Personal Banking Group")
				applicationManager.scenario2Score += value;

			if (applicationManager.selectedDepartment == "Control Functions")
				applicationManager.scenario3Score += value;

			if (applicationManager.selectedDepartment == "Enablement Functions")
				applicationManager.scenario4Score += value;
		}

		applicationManager.totalScore += value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SaveScores()
	{
		CancelInvoke("CheckScore");

		GameManager.Instance.SaveGameStatus();

		CheckProgressAndCallLevelAPI();
		UpdateScoreLimit();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void UpdateScoreLimit()
	{
		currentScore = applicationManager.totalScore;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void DisplayScore()
	{
		scoreTextHUD.text = applicationManager.totalScore.ToString();
		scoreTextProfile.text = applicationManager.totalScore.ToString();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckProgressAndCallLevelAPI()
	{
		if (currentSceneIndex == 5 || currentSceneIndex == 8)
		{
			ValuesAPIManager.Instance.Values("POST");
			BehavioursAPIManager.Instance.Behaviours("POST");
		}
			
		if (currentSceneIndex == 8)
			ScenariosAPIManager.Instance.Scenarios("POST");

		ScoreAPIManager.Instance.Score();
	}

	#endregion

}
