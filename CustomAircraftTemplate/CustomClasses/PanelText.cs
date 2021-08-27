using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
namespace A10Mod
{
    public abstract class PanelText : MonoBehaviour
    {
        public TextMeshPro text;
        public Battery battery;

        private void Start()
        {
        }
        private void Update()
        {
            if (!battery.connected)
            {
                SetText("");
                return;
            }


            UpdateText();

        }

        protected virtual void UpdateText()
        {

        }


        public void SetText(string text)
        {
            this.text.text = text;
        }

    }
}
