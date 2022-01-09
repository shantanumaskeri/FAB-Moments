using UnityEngine;

public class AspectRatioManager : MonoBehaviour
{

	#region SINGLETON INSTANCE

	public static AspectRatioManager Instance;

	#endregion

	#region PUBLIC FIELDS

	[HideInInspector]
	public float aspectRatio = 0.0f;

	#endregion

	#region PRIVATE VARIABLES

	private int width;
	private int height;

	#endregion

	#region UNITY MONOBEHAVIOURS

	private void Start()
    {
		Instance = this;

		Initialize();    
    }

	#endregion

	#region CUSTOM METHODS

	private void Initialize()
	{
		width = Screen.width;
		height = Screen.height;
		
		CalculateAspectRatio();
	}

	private void CalculateAspectRatio()
	{
		Vector2 ratio = GetAspectRatio(width, height);
		
		aspectRatio = ratio.x / ratio.y;
	}

	private static Vector2 GetAspectRatio(int x, int y)
	{
		float f = (float)x / (float)y;
		int i = 0;
		while (true)
		{
			i++;
			if (System.Math.Round(f * i, 2) == Mathf.RoundToInt(f * i))
				break;
		}

		return new Vector2((float)System.Math.Round(f * i, 2), i);
	}

	#endregion

}
