using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BehavioursAnswersManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static BehavioursAnswersManager Instance;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

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

	#endregion

	#region PRIVATE VARIABLES

	private string question;
	private string option1;
	private string option2;
	private string option3;
	private string option4;

	#endregion

	#region UNITY MONOBEHAVIOURS

	private void Start()
	{
		Instance = this;
	}

	#endregion

	#region CUSTOM METHODS

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ConfigureAnswers(int level)
	{
		CreateAnswers(level);
	}

	private void CreateAnswers(int level)
	{
		BehavioursAnswersXMLManager.Instance.quizData[level].TryGetValue("question", out question);
		BehavioursAnswersXMLManager.Instance.quizData[level].TryGetValue("option1", out option1);
		BehavioursAnswersXMLManager.Instance.quizData[level].TryGetValue("option2", out option2);
		BehavioursAnswersXMLManager.Instance.quizData[level].TryGetValue("option3", out option3);
		BehavioursAnswersXMLManager.Instance.quizData[level].TryGetValue("option4", out option4);

		questionText.text = question;
		option1Text.text = "• " + option1;
		option2Text.text = "• " + option2;
		option3Text.text = "• " + option3;
		option4Text.text = "• " + option4;

		AdjustOptionsLayout(option2Text);
		AdjustOptionsLayout(option3Text);
		AdjustOptionsLayout(option4Text);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void AdjustOptionsLayout(Text optionText)
	{
		if (optionText.text == "• ")
			optionText.gameObject.SetActive(false);
		else
			optionText.gameObject.SetActive(true);
	}

	#endregion

}
