using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VTNetworking;
using VTOLVR_Multiplayer;

namespace A10Mod
{
    public class FormationLightSync : VTNetSyncRPCOnly
    {
		public VRTwistKnob knob;

		protected override void OnNetInitialized()
		{
			base.OnNetInitialized();
            if (base.isMine)
            {
				this.knob.OnSetState.AddListener(OnSetKnob);
            }
			
		}

		private void OnSetKnob(float state)
        {
			base.SendRPC("RPC_SetKnob", new object[]
			{
				state
			});
		}

		[VTRPC]
		public virtual void RPC_SetKnob(float state)
		{
			this.knob.SetKnobValue(state);
		}

	

	}
}
