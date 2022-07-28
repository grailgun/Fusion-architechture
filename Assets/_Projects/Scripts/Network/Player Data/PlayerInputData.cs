using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
	[System.Serializable]
	public struct PlayerInputData : INetworkInput
	{
		public const byte ATTACK = 0x01;
		public const byte JUMP = 0x02;
		public const byte SPRINT = 0x03;
		public const byte INTERACT = 0x04;
		public byte buttons;

		public Vector2 move;
		public Vector2 look;

		public bool IsUp(uint button)
		{
			return IsDown(button) == false;
		}

		public bool IsDown(uint button)
		{
			return (buttons & button) == button;
		}
	}
}