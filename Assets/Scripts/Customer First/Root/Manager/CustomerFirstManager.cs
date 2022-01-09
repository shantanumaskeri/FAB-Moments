using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CustomerFirstManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static CustomerFirstManager Instance;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Game Object References")]
	[SerializeField]
	private GameObject spinner;

	[Header("Text References")]
	[SerializeField]
	private Text statusMessage;

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

		InvokeRepeating(nameof(CheckConnectionAndCallAPI), 0.1f, 0.1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckConnectionAndCallAPI()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
			ToggleLoadingSpinnerOnOff(false);
		else
		{
			CustomerFirstAPIManager.Instance.CustomerFirst("GET");

			CancelInvoke(nameof(CheckConnectionAndCallAPI));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void UpdateCFVideoData()
	{
		CustomerFirstAPIManager.Instance.CustomerFirst("POST");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ToggleLoadingSpinnerOnOff(bool value)
	{
		spinner.SetActive(value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ShowStatusMessage(string message)
	{
		statusMessage.text = message;

		StartCoroutine(HideStatusMessage());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerator HideStatusMessage()
	{
		yield return new WaitForSeconds(2.0f);

		statusMessage.text = "";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ConfigureQuizPopup()
	{
		StartCoroutine(ShowQuizPopupAlert());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerator ShowQuizPopupAlert()
	{
		yield return new WaitForSeconds(0.5f);

		PopupAlertsManager.Instance.ShowAlertPanelPopUp(PopupAlertsManager.Instance.extraAlertPanel);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void MoveToNextSection()
	{
		applicationManager.totalScore -= 10 * CustomerFirstXMLManager.Instance.totalQuestions;

		ScenesManager.Instance.ChangeSceneManual();
	}

	#endregion

}
