using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchCameraController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    #region PUBLIC FIELDS

    [HideInInspector]
    public Vector2 TouchDist;
    
    [HideInInspector]
    public Vector2 PointerOld;
    
    [HideInInspector]
    protected int PointerId;
    
    [HideInInspector]
    public bool Pressed;

    #endregion

    #region UNITY MONOBEHAVIOURS

    private void Update()
    {
        RotateView();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        PointerId = eventData.pointerId;
        PointerOld = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }

    #endregion

    #region CUSTOM METHODS

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void RotateView()
	{
        if (Pressed)
        {
            if (PointerId >= 0 && PointerId < Input.touches.Length)
            {
                TouchDist = Input.touches[PointerId].position - PointerOld;
                PointerOld = Input.touches[PointerId].position;
            }
            else
            {
                TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
                PointerOld = Input.mousePosition;
            }

            GameManager.Instance.CheckPlatformAndSceneToShowInstructions(1);
        }
        else
            TouchDist = new Vector2();
    }

    #endregion

}