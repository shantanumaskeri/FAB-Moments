using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class AvatarSelectionManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static AvatarSelectionManager Instance;

	#endregion

	#region PUBLIC FIELDS

	[HideInInspector]
	public int avatarID;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Button References")]
	[SerializeField]
	private Button nextButton;

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
	private GameObject[] checkmarks;

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
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ClearSelection()
	{
		for (int i = 0; i < checkmarks.Length; i++)
		{
			checkmarks[i].SetActive(false);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetPlayerName()
	{
		title.text = "Welcome " + applicationManager.playerFirstName;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SelectAvatar(int id)
	{
		ClearSelection();

        avatarID = id;
		checkmarks[avatarID - 1].SetActive(true);

		/*SetAvatarGender();*/
	}

	/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetAvatarGender()
	{
		if ((avatarID % 2) == 0)
			applicationManager.playerGender = "Female";
		else
			applicationManager.playerGender = "Male";

		PlayerPrefs.SetString("PlayerGender", applicationManager.playerGender);
	}*/

	public void CheckAvatarValidation()
	{
		nextButton.interactable = false;

		if (avatarID != 0)
			AvatarAPIManager.Instance.AvatarSelection();
		else
			ShowStatusMessage("Please select an avatar before proceeding.", "failure");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ToggleLoadingSpinnerOnOff(bool value)
	{
		spinner.SetActive(value);
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

		if (result == "success")
			ScenesManager.Instance.ChangeSceneManual();
		else
			nextButton.interactable = true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SaveAvatarSelectionStatus()
	{
		applicationManager.avatarSelected = avatarID;
	}

	#endregion

}
