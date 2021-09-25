
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace A10Mod
{
    class DashClock : MonoBehaviour
    {
        public TextMeshPro text;
      
        private void Update()
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            string top = time.Substring(0, 5);
            string bottom = time.Substring(6);

            this.text.text = top + "\n" + bottom;
        }

        

    }
}
