using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.VideoHelper
{

	[Serializable]
	public class VolumeInfo
	{
		public float Minimum;
		public Sprite Sprite;
	}

	/// <summary>
	/// Handles UI state for the video player.
	/// </summary>
	[RequireComponent(typeof(VideoController))]
	public class VideoPresenter : MonoBehaviour, ITimelineProvider
	{

		#region CONSTANTS

		private const string MinutesFormat = "{0:00}:{1:00}";
		private const string HoursFormat = "{0:00}:{1:00}:{2:00}";

		#endregion

		#region SINGLETON INSTANCE

		public static VideoPresenter Instance;

        #endregion

        #region PUBLIC FIELDS

        [Header("Controls")]
		public Transform Screen;
		public Transform ControlsPanel;
		public Transform LoadingIndicator;
		public Transform ToggleNext;
		public Transform TogglePrevious;
		public Transform NextVideo;

		public Timeline Timeline;
		public Slider Volume;
		public Image Play;
		public Image Pause;
		public Image VolumeHigh;
		public Image VolumeLow;
		public Image VolumeMuted;
		public Text Current;
		public Text Duration;
		public Text NextVideoTime;
		public Text NextVideoName;
		
		[Space(10)]
		public bool TogglePlayPauseOnClick = true;

		public VolumeInfo[] Volumes = new VolumeInfo[0];
		public Transform[] NextVideoThumbHolders;
		
		#endregion

		#region PRIVATE VARIABLES

		private VideoController controller;
		private float previousVolume;
		private string nextVideoName;

		#endregion

		#region UNITY MONOBEHAVIOURS

		private void Start()
		{
			Instance = this;

			controller = GetComponent<VideoController>();

			if (controller == null)
			{
				DestroyImmediate(this);
				return;
			}

			controller.OnStartedPlaying.AddListener(OnStartedPlaying);
			Volume.onValueChanged.AddListener(OnVolumeChanged);

			VolumeHigh.OnClick(ToggleMute);
			VolumeLow.OnClick(ToggleMute);
			VolumeMuted.OnClick(ToggleMute);

			Play.OnClick(ToggleIsPlaying);
			Pause.OnClick(ToggleIsPlaying);

			ControlsPanel.SetGameObjectActive(false);
			LoadingIndicator.SetGameObjectActive(false);
			ToggleNext.SetGameObjectActive(false);
			TogglePrevious.SetGameObjectActive(false);
			NextVideo.SetGameObjectActive(false);

			NextVideo.gameObject.GetComponent<Image>().OnClick(VideoController.Instance.PrepareNextVideo);

			Array.Sort(Volumes, (v1, v2) =>
			{
				if (v1.Minimum > v2.Minimum)
					return 1;
				else if (v1.Minimum == v2.Minimum)
					return 0;
				else
					return -1;
			});
		}

		private void Update()
		{
			if (controller.IsPlaying)
				Timeline.Position = controller.NormalizedTime;
		}

		#endregion

		#region CUSTOM METHODS

		public string GetFormattedPosition(float time)
		{
			return PrettyTimeFormat(TimeSpan.FromSeconds(time * controller.Duration));
		}

		public void ResetComponents()
		{
			LoadingIndicator.SetGameObjectActive(false);
			ControlsPanel.SetGameObjectActive(false);
			Screen.SetGameObjectActive(false);
		}

		public void PrepareComponents()
		{
			StopAllCoroutines();

			LoadingIndicator.SetGameObjectActive(true);
			ControlsPanel.SetGameObjectActive(false);
			Screen.SetGameObjectActive(false);
			NextVideo.SetGameObjectActive(false);

			NextVideoName.text = NextVideoTime.text = "";

			if (VideoController.Instance.videoClipId < (VideoController.Instance.videoNames.Length - 1))
            {
				for (int i = 0; i < NextVideoThumbHolders.Length; i++)
				{
					NextVideoThumbHolders[i].SetGameObjectActive(false);
				}

				NextVideoThumbHolders[VideoController.Instance.videoClipId + 1].SetGameObjectActive(true);
				nextVideoName = VideoController.Instance.videoNames[VideoController.Instance.videoClipId + 1];
			}
		}

		private void ToggleMute()
		{
			if (Volume.value == 0)
			{
				Volume.value = previousVolume;

				CheckVolumeLevels();
			}
			else
			{
				previousVolume = Volume.value;
				Volume.value = 0f;

				VolumeHigh.gameObject.SetActive(false);
				VolumeLow.gameObject.SetActive(false);
				VolumeMuted.gameObject.SetActive(true);
			}
		}

		private void ToggleIsPlaying()
		{
			if (controller.IsPlaying)
			{
				controller.Pause();
				
				Play.gameObject.SetActive(true);
				Pause.gameObject.SetActive(false);
			}
			else
			{
				controller.Play();
				
				Play.gameObject.SetActive(false);
				Pause.gameObject.SetActive(true);
			}
		}

		private void OnStartedPlaying()
		{
			Screen.SetGameObjectActive(true);
			ControlsPanel.SetGameObjectActive(true);
			LoadingIndicator.SetGameObjectActive(false);

			if(Duration != null)
				Duration.text = PrettyTimeFormat(TimeSpan.FromSeconds(controller.Duration));

			StartCoroutine(SetCurrentPosition());

			Volume.value = controller.Volume;
			
			Play.gameObject.SetActive(false);
			Pause.gameObject.SetActive(true);
		}

		private void OnVolumeChanged(float volume)
		{
			controller.Volume = volume;

			for (int i = 0; i < Volumes.Length; i++)
			{
				var current = Volumes[i];
				var next = Volumes.Length - 1 >= i + 1 ? Volumes[i + 1].Minimum : 2;

				if (current.Minimum <= volume && next > volume)
				{
					CheckVolumeLevels();
				}
			}
		}

		private void CheckVolumeLevels()
		{
			if (Volume.value >= 0.5f)
			{
				VolumeHigh.gameObject.SetActive(true);
				VolumeLow.gameObject.SetActive(false);
				VolumeMuted.gameObject.SetActive(false);
			}
			if (Volume.value < 0.5f && Volume.value > 0f)
			{
				VolumeHigh.gameObject.SetActive(false);
				VolumeLow.gameObject.SetActive(true);
				VolumeMuted.gameObject.SetActive(false);
			}
			if (Volume.value <= 0f)
			{
				VolumeHigh.gameObject.SetActive(false);
				VolumeLow.gameObject.SetActive(false);
				VolumeMuted.gameObject.SetActive(true);
			}
		}

		private IEnumerator SetCurrentPosition()
		{
			while (controller.IsPlaying)
			{
				if (VideoController.Instance.videoClipId < (VideoController.Instance.videoNames.Length - 1))
                {
					if ((controller.Duration - controller.Time) <= 5)
					{
						NextVideo.SetGameObjectActive(true);
						//NextVideoTime.text = (controller.Duration - controller.Time) + " s...";
						string[] NextVideoDetails = nextVideoName.Split(':');
						string NextVideoLabel = NextVideoDetails[1].Substring(1);
						NextVideoName.text = NextVideoDetails[0] + ":\n" + NextVideoLabel;
					}
					else
                    {
						NextVideo.SetGameObjectActive(false);
						NextVideoTime.text = NextVideoName.text = "";
					}
				}
				
				if (Current != null)
					Current.text = PrettyTimeFormat(TimeSpan.FromSeconds(controller.Time));
				
				yield return new WaitForSeconds(1);
			}
		}

		private string PrettyTimeFormat(TimeSpan time)
		{
			if (time.TotalHours <= 1)
				return string.Format(MinutesFormat, time.Minutes, time.Seconds);
			else
				return string.Format(HoursFormat, time.Hours, time.Minutes, time.Seconds);
		}

		#endregion

	}

}
