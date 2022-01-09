using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UISpriteAtlasController : MonoBehaviour
{

	#region EDITOR ASSIGNED VARIABLES

    [Header("Sprite Atlas References")]
	[SerializeField]
	private SpriteAtlas spriteAtlas;

    [Header("String References")]
	[SerializeField]
    private string spriteName;

	[Header("Image References")]
	[SerializeField]
	private Image image;

	[Header("Sprite Renderer References")]
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	#endregion

	#region PRIVATE VARIABLES

	private ApplicationManager applicationManager;

	#endregion

	#region UNITY MONOBEHAVIOURS

	private void Start()
    {
		Initialize();
    }

	#endregion

	#region CUSTOM METHODS

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize()
	{
		applicationManager = FindObjectOfType<ApplicationManager>();

		CheckObjectReferenceName();
		PrepareSpriteForAtlas();
	}

	private void CheckObjectReferenceName()
	{
		string objName = gameObject.name;
		if (objName.Contains("Avatar"))
		{
			spriteName = "avatar" + applicationManager.avatarSelected;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void PrepareSpriteForAtlas()
    {
		if (image != null)
		{
			image.sprite = spriteAtlas.GetSprite(spriteName);
			if (spriteName != "Rectangle" && spriteName != "ManaBar")
				image.preserveAspect = true;
		}
		
		if (spriteRenderer != null)
			spriteRenderer.sprite = spriteAtlas.GetSprite(spriteName);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PrepareSpriteForAtlas(string avatar)
	{
		if (image != null)
		{
			image.sprite = spriteAtlas.GetSprite(avatar);
			if (spriteName != "Rectangle" && spriteName != "ManaBar")
				image.preserveAspect = true;
		}

		if (spriteRenderer != null)
			spriteRenderer.sprite = spriteAtlas.GetSprite(avatar);
	}

	#endregion

}
