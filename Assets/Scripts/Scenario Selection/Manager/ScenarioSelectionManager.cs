using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioSelectionManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static ScenarioSelectionManager Instance;

	#endregion

	#region PUBLIC FIELDS

	[HideInInspector]
	public int scenarioID = -1;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Button References")]
	[SerializeField]
	private Button nextButton;
	[SerializeField]
	private Button pbg;
	[SerializeField]
	private Button cib;
	[SerializeField]
	private Button cf;
	[SerializeField]
	private Button enab;

	[Header("Image References")]
	[SerializeField]
	private Image pbgLock;
	[SerializeField]
	private Image cibLock;
	[SerializeField]
	private Image cfLock;
	[SerializeField]
	private Image enabLock;
	[SerializeField]
	private Image pbgUnlock;
	[SerializeField]
	private Image cibUnlock;
	[SerializeField]
	private Image cfUnlock;
	[SerializeField]
	private Image enabUnlock;

	[Header("Game Object References")]
	[SerializeField]
	private GameObject spinner;

	[Header("Text References")]
	[SerializeField]
	private Text statusMessage;
	[SerializeField]
	private Text title;

	[Header("Game Object Array References")]
	[SerializeField]
	private GameObject[] blueCheckmarks;
	[SerializeField]
	private GameObject[] greenCheckmarks;

	#endregion

	#region PRIVATE VARIABLES

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
		spinner.SetActive(false);

		applicationManager = FindObjectOfType<ApplicationManager>();
		
		ClearSelection();
		SetPlayerName();

		ScenariosAPIManager.Instance.Scenarios("GET");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetPlayerName()
	{
		title.text = "Welcome " + applicationManager.playerFirstName;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void CheckDepartmentToUnlockScenario()
	{
		if (applicationManager.playerDepartment == "Corporate & Investment Banking Group")
		{
			cib.interactable = true;
			cibLock.enabled = false;
			cibUnlock.enabled = true;
			
			pbg.interactable = System.Convert.ToBoolean(applicationManager.pbgScenarioUnlocked);
			pbgLock.enabled = !System.Convert.ToBoolean(applicationManager.pbgScenarioUnlocked);
			pbgUnlock.enabled = System.Convert.ToBoolean(applicationManager.pbgScenarioUnlocked);

			cf.interactable = System.Convert.ToBoolean(applicationManager.cfScenarioUnlocked);
			cfLock.enabled = !System.Convert.ToBoolean(applicationManager.cfScenarioUnlocked);
			cfUnlock.enabled = System.Convert.ToBoolean(applicationManager.cfScenarioUnlocked);

			enab.interactable = System.Convert.ToBoolean(applicationManager.enabScenarioUnlocked);
			enabLock.enabled = !System.Convert.ToBoolean(applicationManager.enabScenarioUnlocked);
			enabUnlock.enabled = System.Convert.ToBoolean(applicationManager.enabScenarioUnlocked);
		}
		if (applicationManager.playerDepartment == "Personal Banking Group")
		{
			pbg.interactable = true;
			pbgLock.enabled = false;
			pbgUnlock.enabled = true;

			cib.interactable = System.Convert.ToBoolean(applicationManager.cibScenarioUnlocked);
			cibLock.enabled = !System.Convert.ToBoolean(applicationManager.cibScenarioUnlocked);
			cibUnlock.enabled = System.Convert.ToBoolean(applicationManager.cibScenarioUnlocked);

			cf.interactable = System.Convert.ToBoolean(applicationManager.cfScenarioUnlocked);
			cfLock.enabled = !System.Convert.ToBoolean(applicationManager.cfScenarioUnlocked);
			cfUnlock.enabled = System.Convert.ToBoolean(applicationManager.cfScenarioUnlocked);

			enab.interactable = System.Convert.ToBoolean(applicationManager.enabScenarioUnlocked);
			enabLock.enabled = !System.Convert.ToBoolean(applicationManager.enabScenarioUnlocked);
			enabUnlock.enabled = System.Convert.ToBoolean(applicationManager.enabScenarioUnlocked);
		}
		if (applicationManager.playerDepartment == "Control Functions")
		{
			cf.interactable = true;
			cfLock.enabled = false;
			cfUnlock.enabled = true;

			pbg.interactable = System.Convert.ToBoolean(applicationManager.pbgScenarioUnlocked);
			pbgLock.enabled = !System.Convert.ToBoolean(applicationManager.pbgScenarioUnlocked);
			pbgUnlock.enabled = System.Convert.ToBoolean(applicationManager.pbgScenarioUnlocked);

			cib.interactable = System.Convert.ToBoolean(applicationManager.cibScenarioUnlocked);
			cibLock.enabled = !System.Convert.ToBoolean(applicationManager.cibScenarioUnlocked);
			cibUnlock.enabled = System.Convert.ToBoolean(applicationManager.cibScenarioUnlocked);

			enab.interactable = System.Convert.ToBoolean(applicationManager.enabScenarioUnlocked);
			enabLock.enabled = !System.Convert.ToBoolean(applicationManager.enabScenarioUnlocked);
			enabUnlock.enabled = System.Convert.ToBoolean(applicationManager.enabScenarioUnlocked);
		}
		if (applicationManager.playerDepartment == "Enablement Functions")
		{
			enab.interactable = true;
			enabLock.enabled = false;
			enabUnlock.enabled = true;
			
			cib.interactable = System.Convert.ToBoolean(applicationManager.cibScenarioUnlocked);
			cibLock.enabled = !System.Convert.ToBoolean(applicationManager.cibScenarioUnlocked);
			cibUnlock.enabled = System.Convert.ToBoolean(applicationManager.cibScenarioUnlocked);

			pbg.interactable = System.Convert.ToBoolean(applicationManager.pbgScenarioUnlocked);
			pbgLock.enabled = !System.Convert.ToBoolean(applicationManager.pbgScenarioUnlocked);
			pbgUnlock.enabled = System.Convert.ToBoolean(applicationManager.pbgScenarioUnlocked);

			cf.interactable = System.Convert.ToBoolean(applicationManager.cfScenarioUnlocked);
			cfLock.enabled = !System.Convert.ToBoolean(applicationManager.cfScenarioUnlocked);
			cfUnlock.enabled = System.Convert.ToBoolean(applicationManager.cfScenarioUnlocked);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SelectScenario(int id)
	{
		ClearSelection();

        scenarioID = id;

		switch (scenarioID)
		{
			case 0:
				applicationManager.selectedDepartment = "Corporate & Investment Banking Group";
				CheckIfScenarioCompleted(applicationManager.cibScenarioCompleted);
				break;

			case 1:
				applicationManager.selectedDepartment = "Personal Banking Group";
				CheckIfScenarioCompleted(applicationManager.pbgScenarioCompleted);
				break;

			case 2:
				applicationManager.selectedDepartment = "Control Functions";
				CheckIfScenarioCompleted(applicationManager.cfScenarioCompleted);
				break;

			case 3:
				applicationManager.selectedDepartment = "Enablement Functions";
				CheckIfScenarioCompleted(applicationManager.enabScenarioCompleted);
				break;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ClearSelection()
	{
		int length = blueCheckmarks.Length;
		for (int i = 0; i < length; i++)
		{
			blueCheckmarks[i].SetActive(false);
			greenCheckmarks[i].SetActive(false);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckIfScenarioCompleted(int scenarioCompletedID)
	{
		if (scenarioCompletedID == 1)
			greenCheckmarks[scenarioID].SetActive(true);
		else if (scenarioCompletedID == 0)
			blueCheckmarks[scenarioID].SetActive(true);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void CheckScenarioValidation()
	{
		nextButton.interactable = false;

		if (scenarioID > -1)
			ScenesManager.Instance.ChangeSceneManual();
		else
			ShowStatusMessage("Please select a department before proceeding.", "failure");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ShowStatusMessage(string message, string result)
	{
		statusMessage.text = message;

		StartCoroutine(HideStatusMessage(result));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerator HideStatusMessage(string result)
	{
		yield return new WaitForSeconds(2.0f);

		statusMessage.text = "";

		if (result == "failure")
			nextButton.interactable = true;
	}

	#endregion

}
