using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Playground
{
    public class CBotController : PlayerController
    {
        [Header("Components")]
        [SerializeField] private Animator anim;
        [SerializeField] private Transform followingCamera;
        [Header("Movement")]
        [SerializeField] private float groundMaxSpeed = 50f;

        private Dictionary<CharacterState, System.Action> movementFunctions;
        private Dictionary<CharacterState, System.Action> animationsFunctions;

        protected override void Awake()
        {
            base.Awake();
            movementFunctions = new Dictionary<CharacterState, System.Action>()
            {
                {CharacterState.Grounded, MovementGrounded },
                {CharacterState.InAir, MovementInAir }
            };
            animationsFunctions = new Dictionary<CharacterState, System.Action>()
            {
                {CharacterState.Grounded, AnimationsGrounded },
                {CharacterState.InAir, AnimationsInAir }
            };
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
            CharacterStateManager();
            Movement();
            Animations();
        }

        private void CharacterStateManager()
        {
            if (IsGroundedCheck())
            {
                movementState = CharacterState.Grounded;
            }
            else
            {
                movementState = CharacterState.InAir;
            }
        }

        protected override void Movement()
        {
            base.Movement();
            movementFunctions[MovementState].Invoke();
        }

        protected override void Animations()
        {
            base.Animations();
            animationsFunctions[MovementState].Invoke();
        }

        #region Movement Functions

        private void MovementGrounded()
        {
           
        }

        private void MovementInAir()
        {

        }

        #endregion

        #region Animation Functions

        private void AnimationsGrounded()
        {
            
        }

        private void AnimationsInAir()
        {

        }

        #endregion

        #region Check Functions

        private bool IsGroundedCheck()
        {
            return true;
        }

        private bool IsMovingCheck()
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
