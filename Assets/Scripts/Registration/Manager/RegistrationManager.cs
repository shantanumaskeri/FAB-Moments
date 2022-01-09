using System.Runtime.CompilerServices;
using UnityEngine;

public class RegistrationManager : MonoBehaviour
{

	#region CONSTANTS

	private const string REGISTRATION_URL = "https://fabmoments.bellimmersive.com/register";

	#endregion

	#region SINGLETON INSTANCE

	public static RegistrationManager Instance;

	#endregion

	#region PRIVATE VARIABLES

	private float screenWidth;
	private float screenHeight;

	#endregion

	#region UUNITY MONOBEHAVIOURS

	private void Start()
    {
        Instance = this;

		Initialize();
	}

	#endregion

	#region CUSTOM METHODS

	private void Initialize()
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void OpenRegistration()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			// Add a full-screen UniWebView component.
			var webView = gameObject.AddComponent<UniWebView>();
			webView.Frame = new Rect(0, 0, screenWidth, screenHeight);

			// Load a URL.
			webView.Load(REGISTRATION_URL);

			// Show it.
			webView.Show();

			// Close the web view
			webView.OnMessageReceived += (view, message) =>
			{
				if (message.Path.Equals("close"))
				{
					Destroy(webView);
					webView = null;
				}
			};
		}
		else
			Application.OpenURL(REGISTRATION_URL);
	}

	#endregion

}
