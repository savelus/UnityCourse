using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
	public class Aggro : MonoBehaviour
	{
		public TriggerObserver TriggerObserver;
		public Follow Follow;

		public float Cooldown;
		private Coroutine _agroCoroutine;
		private bool _hasAgroTarget;

		private void Start()
		{
			TriggerObserver.TriggerEnter += TriggerEnter;
			TriggerObserver.TriggerExit += TriggerExit;

			SwitchFollowOff();
		}

		private void StopAgroCoroutine()
		{
			if (_agroCoroutine == null) return;
			StopCoroutine(_agroCoroutine);
			_agroCoroutine = null;
		}

		private IEnumerator SwitchFollowOffAfterCooldown()
		{
			yield return new WaitForSeconds(Cooldown);
			
			SwitchFollowOff();
		}

		private void TriggerEnter(Collider obj)
		{
			if (_hasAgroTarget) return;
			_hasAgroTarget = true;
			StopAgroCoroutine();
			SwitchFollowOn();
		}

		private void TriggerExit(Collider obj)
		{
			if(!_hasAgroTarget) return;
			_hasAgroTarget = false;
			_agroCoroutine = StartCoroutine(SwitchFollowOffAfterCooldown());
		}

		private void SwitchFollowOff() =>
			Follow.enabled = false;
		
		private void SwitchFollowOn() =>
			Follow.enabled = true;
	}
}