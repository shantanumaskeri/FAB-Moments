using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

public class ScenariosAnswersXMLManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static ScenariosAnswersXMLManager Instance;

	#endregion

	#region PUBLIC FIELDS

	[Header("Text Asset References")]
	public TextAsset[] answerDocuments;
	
	[HideInInspector]
	public List<Dictionary<string,string>> quizData;

	#endregion

	#region PRIVATE VARIABLES

	private Dictionary<string,string> quizDetails;
	
	#endregion

	#region UNITY MONOBEHAVIOURS

	private void Start()
	{
		Instance = this;
	}

	#endregion

	#region CUSTOM METHODS

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ConfigureXML()
	{
		quizData = new List<Dictionary<string, string>>();

		LoadXML();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void LoadXML()
    {
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(answerDocuments[ScenariosVideoManager.Instance.scenarioID].text);

		ParseXML(xmlDoc);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ParseXML(XmlDocument document)
	{
		XmlNodeList questionnaireList = document.GetElementsByTagName("questionnaire");
		foreach (XmlNode questionnaireInfo in questionnaireList)
		{
			XmlNodeList quiz = questionnaireInfo.ChildNodes;
			quizDetails = new Dictionary<string, string>();

			foreach (XmlNode quizItems in quiz)
			{
				quizDetails.Add(quizItems.Name, quizItems.InnerText);
			}

			quizData.Add(quizDetails);
		}

		ScenariosAnswersManager.Instance.ConfigureAnswers(ScenariosQuizManager.Instance.actualLevel);
	}

	#endregion

}