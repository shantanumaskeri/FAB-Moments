using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CustomerFirstQuestionsManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static CustomerFirstQuestionsManager Instance;

	#endregion

	#region PUBLIC FIELDS

	[HideInInspector]
	public List<int> allLevels;

	#endregion

	#region PRIVATE VARIABLES

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
    public void Initialize()
    {
		applicationManager = FindObjectOfType<ApplicationManager>();

		AddQuestionToQuiz();  
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void AddQuestionToQuiz()
	{
		allLevels = new List<int>();

		for (int i = 0; i < CustomerFirstXMLManager.Instance.totalQuestions; i++)
		{
			allLevels.Add(i);
		}

		for (int i = 0; i < applicationManager.CFVideoIds.Count; i++)
		{
			allLevels.Remove(applicationManager.CFVideoIds[i]);
		}

		CustomerFirstXMLManager.Instance.totalQuestions = allLevels.Count;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void RemoveQuestionFromQuiz(int id)
	{
		applicationManager.CFVideoIds.Add(id);

		allLevels.Remove(id);

		CustomerFirstXMLManager.Instance.totalQuestions--;
	}

	#endregion

}
