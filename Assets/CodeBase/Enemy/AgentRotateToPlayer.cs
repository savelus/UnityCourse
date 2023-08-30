using System;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
	public class AgentRotateToPlayer : Follow
	{
		public float _speed;
		private Transform _heroTransform;
		private IGameFactory _gameFactory;
		private Vector3 _positionToLook;

		private void Start()
		{
			_gameFactory = AllServices.Container.Single<IGameFactory>();
			if(HeroExist())
				InitializeHeroTransform();
			else
				_gameFactory.HeroCreated += InitializeHeroTransform;
		}

		private bool HeroExist() => _gameFactory.HeroGameObject != null;

		private void InitializeHeroTransform() =>
			_heroTransform = _gameFactory.HeroGameObject.transform;

		private void Update()
		{
			if (Initialized())
				RotateTowardsHero();
		}

		private void RotateTowardsHero()
		{
			UpdatePositionToLookAt();

			transform.rotation = SmoothedRotation(transform.rotation, _positionToLook);
		}

		private void UpdatePositionToLookAt()
		{
			Vector3 positionDiff = _heroTransform.position - transform.position;
			_positionToLook = new Vector3(positionDiff.x, transform.position.y, positionDiff.z);
		}

		private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook) => 
			Quaternion.Lerp(rotation, TargetRotation(positionToLook), SpeedFactor());

		private Quaternion TargetRotation(Vector3 position) => 
			Quaternion.LookRotation(position);

		private float SpeedFactor() => _speed * Time.deltaTime;

		private bool Initialized() =>
			_heroTransform != null;
	}
}