using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity.VideoHelper.Animation
{

    /// <summary>
    /// Animates <see cref="CanvasGroup.alpha"/>.
    /// </summary>
    public class CanvasGroupAnimator : AnimationCurveAnimator, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {

        public CanvasGroup Group;

        private bool isFadingOut = true;
        private bool isScreenClicked = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isScreenClicked)
            {
                isFadingOut = true;

                Animate(In, InDuration, x => Group.alpha = x);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isScreenClicked)
            {
                isFadingOut = false;

                Animate(Out, OutDuration, x => Group.alpha = x);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isScreenClicked = true;

            if (!isFadingOut)
            {
                Animate(In, InDuration, x => Group.alpha = x);
            }
            
            if (isFadingOut)
            {
                Animate(Out, OutDuration, x => Group.alpha = x);
            }

            isFadingOut = !isFadingOut;
        }

    }

}
