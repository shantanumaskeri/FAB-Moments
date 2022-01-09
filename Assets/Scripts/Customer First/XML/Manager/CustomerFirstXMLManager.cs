using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;

public class CustomerFirstXMLManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static CustomerFirstXMLManager Instance;

	#endregion

	#region PUBLIC FIELDS

	[Header("Text Asset References")]
	public TextAsset inFile;

	[HideInInspector]
	public int totalQuestions;

	[HideInInspector]
	public List<Dictionary<string, string>> quizData;

	#endregion

	#region PRIVATE VARIABLES

	private Dictionary<string, string> quizDetails;

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
	public void Initialize()
	{
		quizData = new List<Dictionary<string, string>>();

		applicationManager = FindObjectOfType<ApplicationManager>();

		LoadXML();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void LoadXML()
	{
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(inFile.text);

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

		CustomerFirstQuestionsManager.Instance.Initialize();
	}

	#endregion

}
