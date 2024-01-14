using System;
using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroMove : MonoBehaviour, ISavedProgress
    {
        public float MovementSpeed = 10f;
        
        private CharacterController _characterController;
        private IInputService _inputService;
        private HeroAnimator _heroAnimator;


        private void Awake()
        {
            _inputService = AllServices.Container.Single<IInputService>();

            _characterController = GetComponent<CharacterController>();
            _heroAnimator = GetComponent<HeroAnimator>();
        }
        private void Update()
        {
            Vector3 movementVector = Vector3.zero;

            if(!_heroAnimator.IsAttacking && _inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                movementVector = Camera.main.transform.TransformDirection(_inputService.Axis);
                movementVector.y = 0;
                movementVector.Normalize();
                transform.forward = movementVector;
            }
            
            movementVector += Physics.gravity;
            _characterController.Move( MovementSpeed * movementVector * Time.deltaTime);
        }
        public void UpdateProgress(PlayerProgress progress) =>
            progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());

        public void LoadProgress(PlayerProgress progress)
        {
            if (CurrentLevel() == progress.WorldData.PositionOnLevel.Level)
            {
                var savedPosition = progress.WorldData.PositionOnLevel.Position;
                if (savedPosition != null) {
                    Warp(to: savedPosition);
                }
            }
        }

        private void Warp(Vector3Data to)
        {
            _characterController.enabled = false;           
            transform.position = to.AsUnityVector().AddY(_characterController.height);
            _characterController.enabled = true;           
        }

        private static string CurrentLevel() => 
            SceneManager.GetActiveScene().name;
    }
}