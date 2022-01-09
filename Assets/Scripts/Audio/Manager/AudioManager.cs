using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static AudioManager Instance;

	#endregion

	/*#region PUBLIC FIELDS

	[Header("Audio Clip Array References")]
	public AudioClip[] playerMaleVOClips;
	public AudioClip[] playerFemaleVOClips;
	
	#endregion*/

	#region EDITOR ASSIGNED VARIABLES

	[Header("Audio Clip References")]
    [SerializeField]
    private AudioClip correctAnswerClip;
    [SerializeField]
    private AudioClip incorrectAnswerClip;
	[SerializeField]
	private AudioClip doorOpenClip;

	[Header("Audio Source References")]
	[SerializeField]
	private AudioSource audioSource;

	#endregion

	#region UNITY MONOBEHAVIOURS

	private void Start()
	{
        Instance = this;
	}

	#endregion

	#region CUSTOM METHODS

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PlayAudio(string audioName)
	{
        switch (audioName)
		{
            case "CorrectAnswer":
                audioSource.clip = correctAnswerClip;
                break;

            case "IncorrectAnswer":
                audioSource.clip = incorrectAnswerClip;
                break;

			case "DoorOpen":
				audioSource.clip = doorOpenClip;
				break;

			/*case "MaleVO":
				audioSource.clip = playerMaleVOClips[ScenariosVideoManager.Instance.scenarioID];
				break;

			case "FemaleVO":
				audioSource.clip = playerFemaleVOClips[ScenariosVideoManager.Instance.scenarioID];
				break;*/
		}
        
        audioSource.Play();
	}

	#endregion

}
