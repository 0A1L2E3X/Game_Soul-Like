using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ALEX
{
    public class UI_FollowScrollBar : MonoBehaviour
    {
        [SerializeField] GameObject currentSelected;
        [SerializeField] GameObject previousSelected;
        [SerializeField] RectTransform curentSelectedTransform;

        [SerializeField] RectTransform contentPanel;
        [SerializeField] ScrollRect scrollRect;

        private void Update()
        {
            currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected != null)
            {
                if (currentSelected != previousSelected)
                {
                    previousSelected = currentSelected;
                    curentSelectedTransform = currentSelected.GetComponent<RectTransform>();
                    SnapTo(curentSelectedTransform);
                }
            }
        }

        private void SnapTo(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();

            Vector2 newPos = (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) - 
                (Vector2)scrollRect.transform.InverseTransformPoint(target.position);

            newPos.x = 0;

            contentPanel.anchoredPosition = newPos;
        }
    }
}