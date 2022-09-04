using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
	[System.Serializable]
	public struct PlayerInputData : INetworkInput
	{
		public Vector3 move;
		public Vector3 look;
		public NetworkBool attackButton;
		public NetworkBool defenseButton;
	}
}