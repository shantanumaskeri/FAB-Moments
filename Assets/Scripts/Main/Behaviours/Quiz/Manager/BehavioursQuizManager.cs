using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BehavioursQuizManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static BehavioursQuizManager Instance;

	#endregion

	#region PUBLIC FIELDS

	[HideInInspector]
	public int questionID;

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
	private Text option7Text;
	[SerializeField]
	private Text feedbackText;

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
	[SerializeField]
	private Image option7Img;

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
	[SerializeField]
	private UIQuizButtonsController option7BtnController;

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
	private string option7;
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
	public void ConfigureQuiz(int id)
    {
		questionID = id;

		ResetQuiz();
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
		BehavioursQuizXMLManager.Instance.quizData[questionID - 1].TryGetValue("question", out question);
		BehavioursQuizXMLManager.Instance.quizData[questionID - 1].TryGetValue("option1", out option1);
		BehavioursQuizXMLManager.Instance.quizData[questionID - 1].TryGetValue("option2", out option2);
		BehavioursQuizXMLManager.Instance.quizData[questionID - 1].TryGetValue("option3", out option3);
		BehavioursQuizXMLManager.Instance.quizData[questionID - 1].TryGetValue("option4", out option4);
		BehavioursQuizXMLManager.Instance.quizData[questionID - 1].TryGetValue("option5", out option5);
		BehavioursQuizXMLManager.Instance.quizData[questionID - 1].TryGetValue("option6", out option6);
		BehavioursQuizXMLManager.Instance.quizData[questionID - 1].TryGetValue("option7", out option7);
		BehavioursQuizXMLManager.Instance.quizData[questionID - 1].TryGetValue("correctAnswer", out correctAnswer);

		questionText.text = question;
		option1Text.text = option1;
		option2Text.text = option2;
		option3Text.text = option3;
		option4Text.text = option4;
		option5Text.text = option5;
		option6Text.text = option6;
		option7Text.text = option7;
		feedbackText.text = "";

		ResetUIColors();
		AdjustOptionsLayout(option5Text);
		AdjustOptionsLayout(option6Text);
		AdjustOptionsLayout(option7Text);

		StartTimer();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void AdjustOptionsLayout(Text optionText)
	{
		if (optionText.text == "")
			optionText.gameObject.transform.parent.gameObject.SetActive(false);
		else
			optionText.gameObject.transform.parent.gameObject.SetActive(true);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ToggleOptionsOnOff(GameObject instance)
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
	private void ResetOptions()
	{
		submittedAnswerList.Clear();

		finalAnswerToSubmit = semiFinalString = finalString = "";

		option1BtnController.isSelected = option2BtnController.isSelected = option3BtnController.isSelected = option4BtnController.isSelected = option5BtnController.isSelected = option6BtnController.isSelected = option7BtnController.isSelected = false;
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
		continueBtn2.SetActive(false);

		if (value == correctAnswer)
		{
			applicationManager.consecutiveCorrectAnswers++;

			int finalScore = (int)((500 * applicationManager.consecutiveCorrectAnswers) - (int)timer);
			ShowFeedbackInformation("CorrectAnswer", "You are right, you earned " + finalScore + " points.");
		}
		else
		{
			applicationManager.consecutiveCorrectAnswers = 0;

			GameObject.Find("FabBehaviour" + questionID).GetComponent<BehaviourIconController>().maximumWrongAttempts++;
			applicationManager.maximumWrongAttempts = GameObject.Find("FabBehaviour" + questionID).GetComponent<BehaviourIconController>().maximumWrongAttempts;

			if (applicationManager.maximumWrongAttempts == 1)
			{
				ShowFeedbackInformation("IncorrectAnswer", "Not quite right, you lost 50 points. Please try again.");
			}
			if (applicationManager.maximumWrongAttempts == 2)
			{
				continueBtn2.SetActive(true);
				continueBtn1.SetActive(false);

				ShowFeedbackInformation("IncorrectAnswer", "Not quite right, you lost 50 points.\nClick on continue to see the right answers.");
				UpdateSelectedBehaviourQuiz();

				applicationManager.maximumWrongAttempts = 0;
			}
		}

		StopTimer();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ShowFeedbackInformation(string option, string feedback)
	{
		switch (option)
		{
			case "CorrectAnswer":
				int finalScore = (int)((500 * applicationManager.consecutiveCorrectAnswers) - (int)timer);
				ScoreManager.Instance.IncrementScore(finalScore);

				UpdateSelectedBehaviourQuiz();
				break;

			case "IncorrectAnswer":
				ScoreManager.Instance.DecrementScore(50);
				break;
		}

		feedbackText.text = feedback;

		AudioManager.Instance.PlayAudio(option);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void UpdateSelectedBehaviourQuiz()
	{
		applicationManager.behaviourQuizAnswers++;
		
		GameObject.Find("FabBehaviour" + questionID).GetComponent<BoxCollider>().enabled = false;
		GameObject.Find("FabBehaviour" + questionID).GetComponent<BehaviourIconController>().ShowHighlight(GameObject.Find("FabBehaviour" + questionID).GetComponent<BehaviourIconController>().fadedPatch);
		GameObject.Find("FabBehaviour" + questionID).GetComponent<BehaviourIconController>().SaveBehaviourIcon(GameObject.Find("FabBehaviour" + questionID).name, question, finalString);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PrepareCorrectAnswers()
	{
		feedbackHolder.SetActive(false);
		answerHolder.SetActive(true);

		BehavioursAnswersXMLManager.Instance.ConfigureXML(questionID - 1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void EndQuiz()
	{
		PanelsManager.Instance.Shrink(PanelsManager.Instance.quizPanel);
		
		if (applicationManager.behaviourQuizAnswers == BehavioursQuizXMLManager.Instance.totalQuestions)
		{
			applicationManager.teleportState = 1;
			
			TeleportController.Instance.ActivateTeleport();

			HUDManager.Instance.UpdateHUDLevelMessage("Stand on the Teleport\nto go to the next stage");

			BehavioursAPIManager.Instance.Behaviours("POST");
		}
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

		questionText.text = option1Text.text = option2Text.text = option3Text.text = option4Text.text = option5Text.text = option6Text.text = option7Text.text = "";

		submitBtn.interactable = false;

		ResetOptions();
		ResetUIColors();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ResetUIColors()
	{
		option1Text.color = option2Text.color = option3Text.color = option4Text.color = option5Text.color = option6Text.color = option7Text.color = Color.black;
		option1Img.color = option2Img.color = option3Img.color = option4Img.color = option5Img.color = option6Img.color = option7Img.color = Color.white;
	}

	#endregion

}
