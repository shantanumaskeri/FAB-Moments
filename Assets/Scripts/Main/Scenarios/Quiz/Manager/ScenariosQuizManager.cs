using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ScenariosQuizManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static ScenariosQuizManager Instance;

	#endregion

	#region PUBLIC FIELDS

	[HideInInspector]
	public int actualLevel;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Game Object References")]
	[SerializeField]
	private GameObject quizHolder;
	[SerializeField]
	private GameObject feedbackHolder;
	[SerializeField]
	private GameObject answerHolder;
	[SerializeField]
	private GameObject continueBtn1;
	[SerializeField]
	private GameObject continueBtn2;

	[Header("Text References")]
	[SerializeField]
	private Text questionText;
	[SerializeField]
	private Text option1Text;
	[SerializeField]
	private Text option2Text;
	[SerializeField]
	private Text option3Text;
	[SerializeField]
	private Text option4Text;
	[SerializeField]
	private Text option5Text;
	[SerializeField]
	private Text option6Text;
	[SerializeField]
	private Text feedbackText;
	[SerializeField]
	private Text selectText;

	[Header("Image References")]
	[SerializeField]
	private Image option1Img;
	[SerializeField]
	private Image option2Img;
	[SerializeField]
	private Image option3Img;
	[SerializeField]
	private Image option4Img;
	[SerializeField]
	private Image option5Img;
	[SerializeField]
	private Image option6Img;

	[Header("Script References")]
	[SerializeField]
	private UIQuizButtonsController option1BtnController;
	[SerializeField]
	private UIQuizButtonsController option2BtnController;
	[SerializeField]
	private UIQuizButtonsController option3BtnController;
	[SerializeField]
	private UIQuizButtonsController option4BtnController;
	[SerializeField]
	private UIQuizButtonsController option5BtnController;
	[SerializeField]
	private UIQuizButtonsController option6BtnController;

	[Header("Button References")]
	[SerializeField]
	private Button submitBtn;

	#endregion

	#region PRIVATE VARIABLES

	private float timer;
	private string question;
	private string option1;
	private string option2;
	private string option3;
	private string option4;
	private string option5;
	private string option6;
	private string correctAnswer;
	private string finalAnswerToSubmit;
	private string semiFinalString;
	private string finalString;
	private readonly List<string> submittedAnswerList = new List<string>();
	
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
		finalAnswerToSubmit = "";

		applicationManager = FindObjectOfType<ApplicationManager>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public IEnumerator ShowQuiz()
	{
		yield return new WaitForSeconds(2.0f);

		PanelsManager.Instance.Expand(PanelsManager.Instance.quizPanel);

		ScenariosQuizXMLManager.Instance.ConfigureXML();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ConfigureQuiz()
    {
		CreateQuiz();
    }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void StartTimer()
	{
		InvokeRepeating(nameof(UpdateTimer), 1.0f, 1.0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void StopTimer()
	{
		CancelInvoke(nameof(UpdateTimer));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void UpdateTimer()
	{
		timer += 1.0f;

		if (timer > 500.0f)
		{
			timer = 500.0f;

			StopTimer();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CreateQuiz()
	{
		ResetQuiz();

		ScenariosQuizXMLManager.Instance.quizData[actualLevel].TryGetValue("question", out question);
		ScenariosQuizXMLManager.Instance.quizData[actualLevel].TryGetValue("option1", out option1);
		ScenariosQuizXMLManager.Instance.quizData[actualLevel].TryGetValue("option2", out option2);
		ScenariosQuizXMLManager.Instance.quizData[actualLevel].TryGetValue("option3", out option3);
		ScenariosQuizXMLManager.Instance.quizData[actualLevel].TryGetValue("option4", out option4);
		ScenariosQuizXMLManager.Instance.quizData[actualLevel].TryGetValue("option5", out option5);
		ScenariosQuizXMLManager.Instance.quizData[actualLevel].TryGetValue("option6", out option6);
		ScenariosQuizXMLManager.Instance.quizData[actualLevel].TryGetValue("correctAnswer", out correctAnswer);

		questionText.text = question;
		option1Text.text = option1;
		option2Text.text = option2;
		option3Text.text = option3;
		option4Text.text = option4;
		option5Text.text = option5;
		option6Text.text = option6;
		feedbackText.text = "";

		StartTimer();

		AdjustOptionsLayout(option3Text);
		AdjustOptionsLayout(option4Text);
		AdjustOptionsLayout(option5Text);
		AdjustOptionsLayout(option6Text);

		CheckQuestionToAddHint();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void AdjustOptionsLayout(Text optionText)
	{
		if (optionText.text == "")
			optionText.gameObject.transform.parent.gameObject.SetActive(false);
		else
			optionText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	private void CheckQuestionToAddHint()
	{
		switch (ScenariosVideoManager.Instance.scenarioID)
		{
			case 0:
				selectText.text = "Select all that apply";
				break;

			case 1:
				switch (actualLevel)
				{
					case 0:
						selectText.text = "Select all that apply";
						break;

					case 1:
					case 2:
						selectText.text = "";
						break;
				}
				break;

			case 2:
				switch (actualLevel)
				{
					case 0:
					case 1:
					case 2:
					case 3:
						selectText.text = "";
						break;

					case 4:
						selectText.text = "Select all that apply";
						break;
				}
				break;

			case 3:
				switch (actualLevel)
				{
					case 0:
					case 2:
					case 4:
						selectText.text = "Select all that apply";
						break;

					case 1:
					case 3:
					case 5:
					case 6:
					case 7:
					case 8:
						selectText.text = "";
						break;
				}
				break;

			case 4:
				switch (actualLevel)
				{
					case 0:
					case 4:
						selectText.text = "";
						break;

					case 1:
					case 2:
					case 3:
						selectText.text = "Select all that apply";
						break;
				}
				break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ToggleOptionsOnOff(GameObject instance)
    {
		switch (ScenariosVideoManager.Instance.scenarioID)
		{
			case 0:
				SelectMultipleOptions(instance);
				break;

			case 1:
				switch (actualLevel)
				{
					case 0:
						SelectMultipleOptions(instance);
						break;

					case 1:
					case 2:
						SelectSingleOption(instance);
						break;
				}
				break;

			case 2:
				switch (actualLevel)
				{
					case 0:
					case 1:
					case 2:
					case 3:
						SelectSingleOption(instance);
						break;

					case 4:
						SelectMultipleOptions(instance);
						break;
				}
				break;

			case 3:
				switch (actualLevel)
				{
					case 0:
					case 2:
					case 4:
						SelectMultipleOptions(instance);
						break;

					case 1:
					case 3:
					case 5:
					case 6:
					case 7:
					case 8:
						SelectSingleOption(instance);
						break;
				}
				break;

			case 4:
				switch (actualLevel)
				{
					case 0:
					case 4:
						SelectSingleOption(instance);
						break;

					case 1:
					case 2:
					case 3:
						SelectMultipleOptions(instance);
						break;
				}
				break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SelectMultipleOptions(GameObject instance)
	{
		string option = instance.GetComponentInChildren<Text>().text + ",";
		instance.GetComponent<UIQuizButtonsController>().isSelected = !instance.GetComponent<UIQuizButtonsController>().isSelected;
		switch (instance.GetComponent<UIQuizButtonsController>().isSelected)
		{
			case true:
				instance.GetComponent<Image>().color = new Color(0f, 0.2745098039215686f, 0.5490196078431373f);
				instance.GetComponentInChildren<Text>().color = Color.white;

				finalAnswerToSubmit += option;
				break;

			case false:
				instance.GetComponent<Image>().color = Color.white;
				instance.GetComponentInChildren<Text>().color = Color.black;

				string result = finalAnswerToSubmit.Replace(option, "");
				finalAnswerToSubmit = result;
				break;
		}

		if (finalAnswerToSubmit == "")
			submitBtn.interactable = false;
		else
			if (!submitBtn.interactable)
			submitBtn.interactable = true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SelectSingleOption(GameObject instance)
	{
		ResetUIColors();

		submittedAnswerList.Clear();

		finalAnswerToSubmit = semiFinalString = feedbackText.text = "";

		if (instance.name == "Option 1")
			option2BtnController.isSelected = option3BtnController.isSelected = option4BtnController.isSelected = option5BtnController.isSelected = option6BtnController.isSelected = false;
		if (instance.name == "Option 2")
			option1BtnController.isSelected = option3BtnController.isSelected = option4BtnController.isSelected = option5BtnController.isSelected = option6BtnController.isSelected = false;
		if (instance.name == "Option 3")
			option2BtnController.isSelected = option1BtnController.isSelected = option4BtnController.isSelected = option5BtnController.isSelected = option6BtnController.isSelected = false;
		if (instance.name == "Option 4")
			option2BtnController.isSelected = option3BtnController.isSelected = option1BtnController.isSelected = option5BtnController.isSelected = option6BtnController.isSelected = false;
		if (instance.name == "Option 5")
			option2BtnController.isSelected = option3BtnController.isSelected = option1BtnController.isSelected = option4BtnController.isSelected = option6BtnController.isSelected = false;
		if (instance.name == "Option 6")
			option2BtnController.isSelected = option3BtnController.isSelected = option1BtnController.isSelected = option5BtnController.isSelected = option4BtnController.isSelected = false;

		string option = instance.GetComponentInChildren<Text>().text + ",";
		instance.GetComponent<UIQuizButtonsController>().isSelected = !instance.GetComponent<UIQuizButtonsController>().isSelected;
		switch (instance.GetComponent<UIQuizButtonsController>().isSelected)
		{
			case true:
				instance.GetComponent<Image>().color = new Color(0f, 0.2745098039215686f, 0.5490196078431373f);
				instance.GetComponentInChildren<Text>().color = Color.white;

				finalAnswerToSubmit += option;
				break;

			case false:
				instance.GetComponent<Image>().color = Color.white;
				instance.GetComponentInChildren<Text>().color = Color.black;

				string result = finalAnswerToSubmit.Replace(option, "");
				finalAnswerToSubmit = result;
				break;
		}

		if (finalAnswerToSubmit == "")
			submitBtn.interactable = false;
		else
			if (!submitBtn.interactable)
			submitBtn.interactable = true;	
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ResetOptions()
	{
		submittedAnswerList.Clear();

		finalAnswerToSubmit = semiFinalString = finalString = "";

		option1BtnController.isSelected = option2BtnController.isSelected = option3BtnController.isSelected = option4BtnController.isSelected = option5BtnController.isSelected = option6BtnController.isSelected = false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PrepareAnswersToSubmit()
	{
		CheckAnswerLength();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckAnswerLength()
	{
		if (finalAnswerToSubmit.Length > 0)
		{
			submitBtn.interactable = false;

			CalculateAnswer();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CalculateAnswer()
	{
		string strToRemove = finalAnswerToSubmit.Remove(finalAnswerToSubmit.Length - 1);

		string[] stringDiv = strToRemove.Split(',');
		for (int i = 0; i < stringDiv.Length; i++)
		{
			submittedAnswerList.Add(stringDiv[i]);
		}
		submittedAnswerList.Sort();

		for (int i = 0; i < submittedAnswerList.Count; i++)
		{
			semiFinalString += submittedAnswerList[i] + ",";
		}
		finalString = semiFinalString.Remove(semiFinalString.Length - 1);
		
		CheckSubmittedAnswer(finalString);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckSubmittedAnswer(string value)
	{
		quizHolder.SetActive(false);
		feedbackHolder.SetActive(true);

		if (value == correctAnswer)
		{
			continueBtn2.SetActive(false);

			applicationManager.consecutiveCorrectAnswers++;

			ShowFeedbackBasedOnDepartment("CorrectAnswer", "Personal Banking Group", applicationManager.pbgScenarioCompleted);
			ShowFeedbackBasedOnDepartment("CorrectAnswer", "Corporate & Investment Banking Group", applicationManager.cibScenarioCompleted);
			ShowFeedbackBasedOnDepartment("CorrectAnswer", "Control Functions", applicationManager.cfScenarioCompleted);
			ShowFeedbackBasedOnDepartment("CorrectAnswer", "Enablement Functions", applicationManager.enabScenarioCompleted);
		}
		else
		{
			continueBtn1.SetActive(false);

			applicationManager.consecutiveCorrectAnswers = 0;

			ShowFeedbackBasedOnDepartment("IncorrectAnswer", "Personal Banking Group", applicationManager.pbgScenarioCompleted);
			ShowFeedbackBasedOnDepartment("IncorrectAnswer", "Corporate & Investment Banking Group", applicationManager.cibScenarioCompleted);
			ShowFeedbackBasedOnDepartment("IncorrectAnswer", "Control Functions", applicationManager.cfScenarioCompleted);
			ShowFeedbackBasedOnDepartment("IncorrectAnswer", "Enablement Functions", applicationManager.enabScenarioCompleted);
		}

		StopTimer();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ShowFeedbackBasedOnDepartment(string answer, string departmentName, int scenarioCompleted)
	{
		if (answer == "CorrectAnswer")
		{
			if (applicationManager.selectedDepartment == departmentName)
			{
				if (applicationManager.playerDepartment == applicationManager.selectedDepartment)
				{
					if (scenarioCompleted == 0)
					{
						int finalScore = (int)((500 * applicationManager.consecutiveCorrectAnswers) - (int)timer);
						ShowFeedbackInformation(answer, "You are right, you earned " + finalScore + " points.");
					}
					else
					{
						ShowFeedbackInformation(answer, "You are right, well done!");
					}
				}
				else
				{
					ShowFeedbackInformation(answer, "You are right, well done!");
				}
			}
		}
		else
		{
			if (applicationManager.selectedDepartment == departmentName)
			{
				if (applicationManager.playerDepartment == applicationManager.selectedDepartment)
				{
					if (scenarioCompleted == 0)
					{
						ShowFeedbackInformation(answer, "Not quite right, you lost 50 points.\nClick on continue to see the right answers.");
					}
					else
					{
						ShowFeedbackInformation(answer, "Not quite right.\nClick on continue to see the right answers.");
					}
				}
				else
				{
					ShowFeedbackInformation(answer, "Not quite right.\nClick on continue to see the right answers.");
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ShowFeedbackInformation(string answer, string feedback)
	{
		UpdateScoreBasedOnDepartment(answer, "Personal Banking Group", applicationManager.pbgScenarioCompleted);
		UpdateScoreBasedOnDepartment(answer, "Corporate & Investment Banking Group", applicationManager.cibScenarioCompleted);
		UpdateScoreBasedOnDepartment(answer, "Control Functions", applicationManager.cfScenarioCompleted);
		UpdateScoreBasedOnDepartment(answer, "Enablement Functions", applicationManager.enabScenarioCompleted);

		feedbackText.text = feedback;

		AudioManager.Instance.PlayAudio(answer);

		SaveScenarioQuizDetails(actualLevel, question, finalString);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void UpdateScoreBasedOnDepartment(string answer, string departmentName, int scenarioCompleted)
	{
		if (answer == "CorrectAnswer")
		{
			if (applicationManager.selectedDepartment == departmentName)
			{
				if (applicationManager.playerDepartment == applicationManager.selectedDepartment)
				{
					if (scenarioCompleted == 0)
					{
						int finalScore = (int)((500 * applicationManager.consecutiveCorrectAnswers) - (int)timer);
						ScoreManager.Instance.IncrementScore(finalScore);
					}
				}
			}
		}
		else
		{
			if (applicationManager.selectedDepartment == departmentName)
			{
				if (applicationManager.playerDepartment == applicationManager.selectedDepartment)
				{
					if (scenarioCompleted == 0)
					{
						ScoreManager.Instance.DecrementScore(50);
					}
				}
			}
		}			
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SaveScenarioQuizDetails(int level, string question, string answer)
	{
		switch (ScenariosVideoManager.Instance.scenarioID)
		{
			case 0:
				switch (level)
				{
					case 0:
						applicationManager.fabScenario1Question1 = question;
						applicationManager.fabScenario1Answer1 = answer;
						break;

					case 1:
						applicationManager.fabScenario1Question2 = question;
						applicationManager.fabScenario1Answer2 = answer;
						break;

					case 2:
						applicationManager.fabScenario1Question3 = question;
						applicationManager.fabScenario1Answer3 = answer;
						break;

					case 3:
						applicationManager.fabScenario1Question4 = question;
						applicationManager.fabScenario1Answer4 = answer;
						break;
				}
				break;

			case 1:
				switch (level)
				{
					case 0:
						applicationManager.fabScenario2Question1 = question;
						applicationManager.fabScenario2Answer1 = answer;
						break;

					case 1:
						applicationManager.fabScenario2Question2 = question;
						applicationManager.fabScenario2Answer2 = answer;
						break;

					case 2:
						applicationManager.fabScenario2Question3 = question;
						applicationManager.fabScenario2Answer3 = answer;
						break;
				}
				break;

			case 2:
				switch (level)
				{
					case 0:
						applicationManager.fabScenario2Question4 = question;
						applicationManager.fabScenario2Answer4 = answer;
						break;

					case 1:
						applicationManager.fabScenario2Question5 = question;
						applicationManager.fabScenario2Answer5 = answer;
						break;

					case 2:
						applicationManager.fabScenario2Question6 = question;
						applicationManager.fabScenario2Answer6 = answer;
						break;

					case 3:
						applicationManager.fabScenario2Question7 = question;
						applicationManager.fabScenario2Answer7 = answer;
						break;

					case 4:
						applicationManager.fabScenario2Question8 = question;
						applicationManager.fabScenario2Answer8 = answer;
						break;
				}
				break;

			case 3:
				switch (level)
				{
					case 0:
						applicationManager.fabScenario3Question1 = question;
						applicationManager.fabScenario3Answer1 = answer;
						break;

					case 1:
						applicationManager.fabScenario3Question2 = question;
						applicationManager.fabScenario3Answer2 = answer;
						break;

					case 2:
						applicationManager.fabScenario3Question3 = question;
						applicationManager.fabScenario3Answer3 = answer;
						break;

					case 3:
						applicationManager.fabScenario3Question4 = question;
						applicationManager.fabScenario3Answer4 = answer;
						break;

					case 4:
						applicationManager.fabScenario3Question5 = question;
						applicationManager.fabScenario3Answer5 = answer;
						break;

					case 5:
						applicationManager.fabScenario3Question6 = question;
						applicationManager.fabScenario3Answer6 = answer;
						break;

					case 6:
						applicationManager.fabScenario3Question7 = question;
						applicationManager.fabScenario3Answer7 = answer;
						break;

					case 7:
						applicationManager.fabScenario3Question8 = question;
						applicationManager.fabScenario3Answer8 = answer;
						break;

					case 8:
						applicationManager.fabScenario3Question9 = question;
						applicationManager.fabScenario3Answer9 = answer;
						break;
				}
				break;

			case 4:
				switch (level)
				{
					case 0:
						applicationManager.fabScenario4Question1 = question;
						applicationManager.fabScenario4Answer1 = answer;
						break;

					case 1:
						applicationManager.fabScenario4Question2 = question;
						applicationManager.fabScenario4Answer2 = answer;
						break;

					case 2:
						applicationManager.fabScenario4Question3 = question;
						applicationManager.fabScenario4Answer3 = answer;
						break;

					case 3:
						applicationManager.fabScenario4Question4 = question;
						applicationManager.fabScenario4Answer4 = answer;
						break;

					case 4:
						applicationManager.fabScenario4Question5 = question;
						applicationManager.fabScenario4Answer5 = answer;
						break;
				}
				break;
		}

		ScenariosAPIManager.Instance.Scenarios("POST");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PrepareCorrectAnswers()
	{
		feedbackHolder.SetActive(false);
		answerHolder.SetActive(true);

		ScenariosAnswersXMLManager.Instance.ConfigureXML();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void MoveToNextLevel()
	{
		actualLevel++;

		switch (ScenariosVideoManager.Instance.scenarioID)
		{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
				ScenariosVideoManager.Instance.ResumeScenarioVideo();
				break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void EndQuiz()
	{
		actualLevel = 0;

		ScenariosManager.Instance.CheckScenarioStatus();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ResetQuiz()
	{
		continueBtn1.SetActive(true);
		continueBtn2.SetActive(true);

		quizHolder.SetActive(true);
		feedbackHolder.SetActive(false);
		answerHolder.SetActive(false);

		timer = 0f;

		feedbackText.text = questionText.text = option1Text.text = option2Text.text = option3Text.text = option4Text.text = option5Text.text = option6Text.text = "";

		submitBtn.interactable = false;

		ResetOptions();
		ResetUIColors();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ResetUIColors()
	{
		option1Text.color = option2Text.color = option3Text.color = option4Text.color = option5Text.color = option6Text.color = Color.black;
		option1Img.color = option2Img.color = option3Img.color = option4Img.color = option5Img.color = option6Img.color = Color.white;
	}

	#endregion

}
