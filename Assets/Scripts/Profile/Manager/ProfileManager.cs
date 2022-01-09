using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{

    #region SINGLETON INSTANCE

    public static ProfileManager Instance;

    #endregion

    #region EDITOR ASSIGNED VARIABLES

    [Header("Text References")]
    [SerializeField]
    private Text playerNameProfile;
    [SerializeField]
    private Text playerEmailProfile;

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
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetProfileDetails()
	{
        playerNameProfile.text = applicationManager.playerFirstName + " " + applicationManager.playerLastName;
        playerEmailProfile.text = applicationManager.playerEmailAddress;

    }

    #endregion

}
