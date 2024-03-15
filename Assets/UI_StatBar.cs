using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ALEX
{
    public class UI_StatBar : MonoBehaviour
    {
        private Slider slider;

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public virtual void SetStats(int newVal)
        {
            slider.value = newVal;
        }

        public virtual void SetMaxStats(int maxVal)
        {
            slider.maxValue = maxVal;
            slider.value = maxVal;
        }
    }
}
