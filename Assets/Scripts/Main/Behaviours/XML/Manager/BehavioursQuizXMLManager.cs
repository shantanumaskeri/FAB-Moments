using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

public class BehavioursQuizXMLManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static BehavioursQuizXMLManager Instance;

	#endregion

	#region PUBLIC FIELDS

	[Header("Text Asset References")]
	public TextAsset quizDocument;
	
	[HideInInspector]
	public List<Dictionary<string,string>> quizData;

	[HideInInspector]
	public int totalQuestions;

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

		Initialize();
	}

	#endregion

	#region CUSTOM METHODS

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize()
	{
		applicationManager = FindObjectOfType<ApplicationManager>();
	}

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
		xmlDoc.LoadXml(quizDocument.text);

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

		totalQuestions = quizData.Count;

		BehavioursQuizManager.Instance.ConfigureQuiz(iconID);
	}

	#endregion

}