using System;
using System.Linq;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        public EnemyAnimator Animator;

        public float AttackCooldown = 3f;
        public float Cleavage =0.5f;
        public float EffectiveDistance = 0.5f;

        private IGameFactory _factory;
        private Transform _heroTransform;
        private float _attackCooldown;
        private bool _isAttacking;
        private int _layerMask;
        private Collider[] _hits = new Collider[1];
        private bool _attackIsActive;
        public float Damage = 10f;

        private void Awake()
        {
            _factory = AllServices.Container.Single<IGameFactory>();

            _layerMask = 1 << LayerMask.NameToLayer("Player");
            _factory.HeroCreated += OnHeroCreated;
        }

        private void Update()
        {
            UpdateCooldown();
            if(CanAttack())
                StartAttack();
        }

        public void EnableAttack() =>
            _attackIsActive = true;

        public void DisableAttack() => 
            _attackIsActive = false;

        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawDebug(GetStartPoint(), Cleavage, 1f);
                hit.transform.GetComponent<HeroHealth>().TakeDamage(Damage);
            }
        }


        private void OnAttackEnded()
        {
            _attackCooldown = AttackCooldown;
            _isAttacking = false;
        }

        private bool Hit(out Collider hit)
        {
            int hitsCount = Physics.OverlapSphereNonAlloc(GetStartPoint(), Cleavage, _hits, _layerMask);
            
            hit = _hits.FirstOrDefault();
            
            return hitsCount > 0;
        }

        private Vector3 GetStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * EffectiveDistance;
        }

        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
                _attackCooldown -= Time.deltaTime;
        }

        private void StartAttack()
        {
            transform.LookAt(_heroTransform);
            Animator.PlayAttack();
            _isAttacking = true;
        }

        private bool CanAttack() => 
           _attackIsActive && !_isAttacking && CooldownIsUp();

        private bool CooldownIsUp() =>
            _attackCooldown <= 0;


        private void OnHeroCreated() => 
            _heroTransform = _factory.HeroGameObject.transform;
    }
}