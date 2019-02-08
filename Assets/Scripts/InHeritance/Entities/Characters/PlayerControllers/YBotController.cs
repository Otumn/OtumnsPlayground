using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Playground
{
    public class YBotController : PlayerController
    {
        [Header("Components")]
        [SerializeField] private Animator anim;
        [SerializeField] private Transform followingCamera;
        [Header("Movement")]
        [SerializeField] private float groundMaxSpeed = 50f;
        [SerializeField] private AnimationCurve groundAccelerationCurve;

        private bool wasMoving = false;
        private float axesRatioedMagnitude;
        private float axesToAccelerationCurveValue;
        private Vector3 movementDirection;
        private Quaternion camRotationOnMovementStart;
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
            camRotationOnMovementStart = followingCamera.localRotation;
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
            Vector2 rawInputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            axesRatioedMagnitude = inputVector.magnitude;
            axesToAccelerationCurveValue = groundAccelerationCurve.Evaluate(axesRatioedMagnitude);
            movementDirection = new Vector3(rawInputVector.x * axesToAccelerationCurveValue, 0, rawInputVector.y * axesToAccelerationCurveValue) * groundMaxSpeed * Time.deltaTime;
            Debug.DrawRay(transform.position, movementDirection, Color.magenta);
            if(IsMovingCheck())
            {
                movementDirection = followingCamera.localRotation * movementDirection;
                movementDirection = new Vector3(movementDirection.x, 0, movementDirection.z);
                transform.rotation = Quaternion.LookRotation(movementDirection, transform.up);
                if(!wasMoving)
                {
                    wasMoving = true;
                    camRotationOnMovementStart = followingCamera.localRotation;
                }
            }
            else
            {
                if(wasMoving)
                {
                    wasMoving = false;
                }
            }
            Debug.DrawRay(transform.position, movementDirection, Color.cyan);
            //body.velocity = movementDirection;
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
