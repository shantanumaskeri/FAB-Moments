using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : MonoBehaviour
{

    #region SINGLETON INSTANCE

    public static PlayerController Instance;

    #endregion

    #region PUBLIC FIELDS

    [Header("Script References")]
    public FirstPersonController firstPersonController;

    #endregion

	#region UNITY MONOBEHAVIOURS

	private void Start()
    {
        Instance = this;    
    }

	#endregion

	#region CUSTOM METHODS

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ConfigurePlayerForScenario(GameObject switchInstance)
	{
        DisablePlayer();
        ResetPlayer(switchInstance);
	}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ConfigurePlayerForInteraction()
	{
        EnablePlayer();
	}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnablePlayer()
    {
        firstPersonController.enabled = true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void DisablePlayer()
	{
        firstPersonController.enabled = false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ResetPlayer(GameObject switchInstance)
	{
        transform.DOMove(switchInstance.transform.position, 1f).SetEase(Ease.Linear);

        switch (switchInstance.name)
		{
            case "-1 BO Switch Receptionist":
                transform.DORotate(new Vector3(0f, 180f, 0f), 1f).SetEase(Ease.Linear);
                break;

            case "0 BO Switch CIB":
                transform.DORotate(new Vector3(0f, 198f, 0f), 1f).SetEase(Ease.Linear);
                break;

            case "1 BO Switch PBG 1":
                transform.DORotate(new Vector3(0f, 235f, 0f), 1f).SetEase(Ease.Linear);
                break;

            case "2 BO Switch PBG 2":
                transform.DORotate(new Vector3(0f, 60f, 0f), 1f).SetEase(Ease.Linear);
                break;

            case "3 BO Switch CF":
                transform.DORotate(new Vector3(0f, 65f, 0f), 1f).SetEase(Ease.Linear);
                break;

            case "4 BO Switch ENAB":
                transform.DORotate(new Vector3(0f, 300f, 0f), 1f).SetEase(Ease.Linear);
                break;
        }

        ScenariosDialogueManager.Instance.StartDialogueSequence(switchInstance.GetComponent<SwitchController>().switchID);
    }

    #endregion

}
