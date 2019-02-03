using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Playground
{
    public class YBotController : PlayerController
    {
        [Header("Components")]
        [SerializeField] private Animator anim;
        [Header("Movement")]
        [SerializeField] private float groundMaxSpeed = 50f;
        [SerializeField] private AnimationCurve groundAccelerationCurve;

        private float axesRatioedMagnitude;
        private float axesToAccelerationCurveValue;
        private Vector3 movementDirection;
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

        protected override void Update()
        {
            base.Update();
            CharacterStateManager();
            Movement();
            Animations();
        }

        private void CharacterStateManager()
        {
            if(IsGroundedCheck())
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
            Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            axesRatioedMagnitude = inputVector.magnitude;
            axesToAccelerationCurveValue = groundAccelerationCurve.Evaluate(axesRatioedMagnitude);
            //Debug.Log(" Magnitude : " + inputVector.magnitude + " Ratioed : " + axesRatioedMagnitude + " Acceleration : " + axesToAccelerationCurveValue);
            movementDirection = new Vector3(Input.GetAxisRaw("Horizontal") * axesToAccelerationCurveValue, 0, Input.GetAxisRaw("Vertical") * axesToAccelerationCurveValue) * groundMaxSpeed * Time.deltaTime;
            body.velocity = movementDirection;
            if(IsMovingCheck()) transform.rotation = Quaternion.LookRotation(movementDirection, transform.up);
        }

        private void MovementInAir()
        {

        }

        #endregion

        #region Animation Functions

        private void AnimationsGrounded()
        {
            anim.SetFloat("acceleration", axesRatioedMagnitude);
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
