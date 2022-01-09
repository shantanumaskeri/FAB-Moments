using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

public class BehavioursAnswersXMLManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static BehavioursAnswersXMLManager Instance;

	#endregion

	#region PUBLIC FIELDS

	[Header("Text Asset References")]
	public TextAsset answerDocument;
	
	[HideInInspector]
	public List<Dictionary<string,string>> quizData;

	#endregion

	#region PRIVATE VARIABLES

	private Dictionary<string,string> quizDetails;
	private int iconID;

	private ApplicationManager applicationManager;

	#endregion

	#region UNITY MONOBEHAVIOURS

	private void Start()
	{
		Instance = this;
	}

	#endregion

	#region CUSTOM METHODS

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ConfigureXML(int id)
	{
		iconID = id;
		quizData = new List<Dictionary<string, string>>();

		LoadXML();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void LoadXML()
    {
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(answerDocument.text);

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

		BehavioursAnswersManager.Instance.ConfigureAnswers(iconID);
	}

	#endregion

}