using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using static LeaderboardAPIManager;

public class LeaderboardManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static LeaderboardManager Instance;

	#endregion

	#region EDITOR ASSIGNED VARIABLES

	[Header("Game Object References")]
	[SerializeField]
	private GameObject spinner;

	[Header("Text References")]
	[SerializeField]
	private Text statusMessage;
	[SerializeField]
	private Text playerName;
	[SerializeField]
	private Text playerScore;
	
	[Header("Text Array References")]
	[SerializeField]
	private Text[] allPlayerNames;
	[SerializeField]
	private Text[] allPlayerScores;
	[SerializeField]
	private Text[] allPlayerRanks;

	[Header("Script Array References")]
	[SerializeField]
	private UISpriteAtlasController[] uISpriteAtlasControllers;

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
		applicationManager = FindObjectOfType<ApplicationManager>();

		PreparePlayerDetails();

		InvokeRepeating(nameof(CheckConnectionAndCallAPI), 0.1f, 0.1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckConnectionAndCallAPI()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
			ToggleLoadingSpinnerOnOff(false);
		else
		{
			LeaderboardAPIManager.Instance.Leaderboard();
			
			CancelInvoke(nameof(CheckConnectionAndCallAPI));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PreparePlayerDetails()
	{
		playerName.text = applicationManager.playerFirstName + " " + applicationManager.playerLastName;
		playerScore.text = applicationManager.totalScore.ToString();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PrepareLeaderboard(Response response)
    {
		playerScore.text = applicationManager.totalScore.ToString();

		int cLength = uISpriteAtlasControllers.Length;
		int length = response.success.data.Length;

		for (int i = 0; i < cLength; i++)
		{
			allPlayerNames[i].color = allPlayerScores[i].color = allPlayerRanks[i].color = new Color(0f, 0f, 0f, 0f);
			uISpriteAtlasControllers[i].gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
		}
		
		for (int i = 0; i < length; i++)
		{
			allPlayerNames[i].color = allPlayerScores[i].color = allPlayerRanks[i].color = new Color(0f, 0f, 0f, 1f);

			uISpriteAtlasControllers[i].gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

			allPlayerNames[i].text = response.success.data[i].firstName + " " + response.success.data[i].lastName;
			allPlayerScores[i].text = response.success.data[i].score.ToString();
			allPlayerRanks[i].text = response.success.data[i].rank.ToString();

			uISpriteAtlasControllers[i].PrepareSpriteForAtlas(response.success.data[i].avatar);
		}
		
		for (int i = length; i < cLength; i++)
		{
			allPlayerNames[i].color = allPlayerScores[i].color = allPlayerRanks[i].color = new Color(0f, 0f, 0f, 0f);
			uISpriteAtlasControllers[i].gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
		}
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

	#endregion

}
