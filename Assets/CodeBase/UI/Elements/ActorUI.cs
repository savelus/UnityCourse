using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI.Elements
{
	public class ActorUI : MonoBehaviour
	{
		public HpBar HpBar;

		private IHealth _heroHealth;

		public void Construct(IHealth health)
		{
			_heroHealth = health;
			
			_heroHealth.HealthChanged += UpdateHpBar;
		}
		private void Start()
		{
			IHealth health = GetComponent<IHealth>();
			if(health!=null)
				Construct(health);
		}
		private void OnDestroy() {
			if(_heroHealth == null) return;
			_heroHealth.HealthChanged -= UpdateHpBar;
		}

		private void UpdateHpBar() =>
			HpBar.SetValue(_heroHealth.Current, _heroHealth.Max);

	}
}