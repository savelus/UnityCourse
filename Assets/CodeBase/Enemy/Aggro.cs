using System;
using UnityEngine;

namespace CodeBase.Enemy
{
	public class Aggro : MonoBehaviour
	{
		public TriggerObserver TriggerObserver;
		public AgentMoveToPlayer Follow;
		
		private void Start()
		{
			TriggerObserver.TriggerEnter += TriggerEnter;
			TriggerObserver.TriggerExit += TriggerExit;

			SwitchFollowOff();
		}
		private void TriggerExit(Collider obj) =>
			SwitchFollowOff();
		private void TriggerEnter(Collider obj) =>
			SwitchFollowOn();
		private void SwitchFollowOff() =>
			Follow.enabled = false;
		private void SwitchFollowOn() =>
			Follow.enabled = true;
	}
}