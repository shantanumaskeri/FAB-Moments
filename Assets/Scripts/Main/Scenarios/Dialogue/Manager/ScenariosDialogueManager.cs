using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ScenariosDialogueManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static ScenariosDialogueManager Instance;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Game Object References")]
	[SerializeField]
	private GameObject dialogueBox;
	[SerializeField]
	private GameObject startBtn;
	[SerializeField]
	private GameObject continueScenarioBtn;
	[SerializeField]
	private GameObject watchScenarioBtn;
	[SerializeField]
	private GameObject nextBtn;

	[Header("Animation Array References")]
	[SerializeField]
	public Animation[] characterAnimation;

	#endregion

	#region PRIVATE VARIABLES

	private string quizState;

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

		HideAllComponents();	
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void HideAllComponents()
	{
		HideDialogues();
		HideAllButtons();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void HideDialogues()
	{
		dialogueBox.SetActive(false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void HideAllButtons()
	{
		continueScenarioBtn.SetActive(false);
		startBtn.SetActive(false);
		watchScenarioBtn.SetActive(false);
		nextBtn.SetActive(false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void StartDialogueSequence(int switchID)
	{
		switch (switchID)
		{
			case -1:
				ShowDialogues("character", "Hello. Welcome to First Abu Dhabi Bank. Please follow the signs on the floor and reach the placeholder to watch the first scenario. Thank you.", "beforeQuiz", -1);
				break;

			case 0:
				ShowDialogues("player", "Hi Mr. Abu Baqar. Good Morning! Hope you are doing well. I’ve been hearing a lot about Customer First and it’s Behaviours at FAB, and your team is doing an exceptional job in displaying them. Can you tell me something more about it?", "beforeQuiz", 0);
				break;

			case 1:
				ShowDialogues("player", "Hi Mr. Ahmed. Good Morning! Hope you are doing well. I’ve been hearing a lot about Customer First and it’s Behaviours at FAB, and you are doing an exceptional job in displaying them. Can you tell me something more about it?", "beforeQuiz", 1);
				break;

			case 2:
				ShowDialogues("player", "Hi Nada, I have just heard about how your team is creating FAB moments. Ahmed was narrating the experience and asked me to come and speak with you.", "beforeQuiz", 2);
				break;

			case 3:
				ShowDialogues("player", "Hi Mr. Rony. Good Morning! Hope you are doing well. I’ve been hearing a lot about Customer First and it’s Behaviours at FAB, and you are doing an exceptional job in displaying them. Can you tell me something more about it?", "beforeQuiz", 3);
				break;

			case 4:
				ShowDialogues("player", "Hi Saba. How are you? I’ve been hearing a lot about Customer First and it’s Behaviours at FAB, and the team is doing a great job in displaying them. Can you tell me your experience?", "beforeQuiz", 4);
				break;
		}

		/*AudioManager.Instance.PlayAudio(applicationManager.playerGender + "VO");*/
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void EndDialogueSequence()
	{
		characterAnimation[ScenariosVideoManager.Instance.scenarioID].CrossFade("Idle");

		HideDialogues();
		HideAllButtons();

		switch (ScenariosVideoManager.Instance.scenarioID)
		{
			case 0:
			case 2:
			case 3:
			case 4:
				ScenesManager.Instance.ChangeSceneManual();
				break;

			case 1:
				ScenariosManager.Instance.DeactivateScenario();
				break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void MoveToNextDialogue()
	{
		switch (quizState)
		{
			case "beforeQuiz":
				switch (ScenariosVideoManager.Instance.scenarioID)
				{
					case 0:
						ShowDialogues("character", "Hello there! Please take a seat, and I'll tell you the story where we were able to create a FAB moment!", "beforeQuiz", 0);
						break;

					case 1:
						ShowDialogues("character", "Hello! Yes, thank you! Let me tell you about my own experience and how we were able to give the customer a FAB moment.", "beforeQuiz", 1);
						break;

					case 2:
						ShowDialogues("character", "Hi! Yes, our team is definitely focusing on creating FAB Moments. So let me tell you what happened next.", "beforeQuiz", 2);
						break;

					case 3:
						ShowDialogues("character", "Hello! Yes, thank you! Let me tell you about my own experience and how we were able to give the customer a FAB moment.", "beforeQuiz", 3);
						break;

					case 4:
						ShowDialogues("character", "Hello! Yes, thank you! Let me tell you about my own story and how FAB provided a top notch customer experience.", "beforeQuiz", 4);
						break;
				}
				break;

			case "afterQuiz":
				switch (ScenariosVideoManager.Instance.scenarioID)
				{
					case 0:
						ShowDialogues("character", "You're most welcome! Have a great day ahead.", "afterQuiz", 0);
						break;

					case 1:
						ShowDialogues("character", "Yes. For the next bit, you should walk up to Nada. She sits on the first floor. She will tell you all about it.", "afterQuiz", 1);
						break;

					case 2:
						ShowDialogues("character", "Oh! The pleasure was all mine. Hope to see you again soon. Bye!", "afterQuiz", 2);
						break;

					case 3:
						ShowDialogues("character", "You're most welcome! Have a great day ahead.", "afterQuiz", 3);
						break;

					case 4:
						ShowDialogues("character", "You're most welcome! Have a great day ahead.", "afterQuiz", 4);
						break;
				}
				break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ShowDialogues(string speaker, string dialogue, string state, int id)
	{
		dialogueBox.SetActive(true);
		dialogueBox.GetComponentInChildren<Text>().text = dialogue;

		quizState = state;

		switch (state)
		{
			case "beforeQuiz":
				switch (speaker)
				{
					case "player":
						characterAnimation[ScenariosVideoManager.Instance.scenarioID].CrossFade("Look Up");

						continueScenarioBtn.SetActive(true);
						break;

					case "character":
						switch (id)
						{
							case 0:
							case 1:
							case 2:
							case 3:
							case 4:
								continueScenarioBtn.SetActive(false);
								watchScenarioBtn.SetActive(true);
								break;

							case -1:
								startBtn.SetActive(true);
								break;
						}
						break;
				}
				break;

			case "afterQuiz":
				switch (speaker)
				{
					case "player":
						continueScenarioBtn.SetActive(true);
						break;

					case "character":
						continueScenarioBtn.SetActive(false);
						nextBtn.SetActive(true);
						break;
				}
				break;
		}
	}

	#endregion

}
