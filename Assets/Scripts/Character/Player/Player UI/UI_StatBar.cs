using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ALEX
{
    public class UI_StatBar : MonoBehaviour
    {
        private Slider slider;
        private RectTransform rectTransform;

        [Header("BAR OPTIONS")]
        [SerializeField] protected bool scaleBarLengthWithStats = true;
        [SerializeField] protected float widthScaleMuliplier = 1;

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>();
        }

        public virtual void SetStats(int newVal)
        {
            slider.value = newVal;
        }

        public virtual void SetMaxStats(int maxVal)
        {
            slider.maxValue = maxVal;
            slider.value = maxVal;

            if (scaleBarLengthWithStats)
            {
                rectTransform.sizeDelta = new Vector2(maxVal * widthScaleMuliplier, rectTransform.sizeDelta.y);
                PlayerUIManager.instance.playerUIHudManager.RefreshHUD();
            }
        }
    }
}
