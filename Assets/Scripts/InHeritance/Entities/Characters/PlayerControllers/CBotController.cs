using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Playground
{
    public class CBotController : PlayerController
    {
        [Header("Components")]
        [SerializeField] private Animator anim;
        [SerializeField] private GameObject attackCollider;
        [SerializeField] private CBotTpTarget tpTarget;
        [Header("Movement")]
        [SerializeField] private float groundMaxSpeed = 200f;
        [SerializeField] private float crouchedMaxSpeed = 100f;
        [SerializeField] private AnimationCurve ascendingCurve;
        [SerializeField] private AnimationCurve descendingCurve;
        [SerializeField] private float jumpForce = 100f;
        [SerializeField] private float descendingForce = 100f;
        [SerializeField] private float heightCurvesSpeed = 0.01f;
        [Header("Camera")]
        [SerializeField] private float cameraSpeed = 100f;
        [SerializeField] private Transform head;
        [SerializeField] private float upMaxAngle = 300f;
        [SerializeField] private float downMaxAngle = 50f;

        private float movementY = 0f;
        private float heigthInc = 0;
        private bool isJumping = false;
        private bool isDescending = false;
        private bool chargingTp = false;
        private bool castingTp = false;
        private bool attacking = false;
        private bool crouched = false;
        private bool wasMoving = false;
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
            Cursor.visible = false;
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
            movementFunctions[movementState].Invoke();
        }

        protected override void Animations()
        {
            base.Animations();
            animationsFunctions[movementState].Invoke();
        }

        #region Movement Functions

        private void MovementGrounded()
        {
            CameraControl();
            if(Input.GetKeyDown(KeyCode.C))
            {
                crouched = !crouched;
                anim.SetBool("crouched", crouched);
            }
            Vector2 inputVector = new Vector2(Input.GetAxis("KeyHorizontal"), Input.GetAxis("KeyVertical"));
            Vector2 rawInputVector = new Vector2(Input.GetAxisRaw("KeyHorizontal"), Input.GetAxisRaw("KeyVertical"));
            if(!crouched) movementDirection = new Vector3(rawInputVector.x, 0, rawInputVector.y) * groundMaxSpeed * Time.deltaTime;
            else movementDirection = new Vector3(rawInputVector.x, 0, rawInputVector.y) * crouchedMaxSpeed * Time.deltaTime;
            Debug.DrawRay(transform.position, movementDirection, Color.magenta);
            if (IsMovingCheck())
            {
                movementDirection = transform.localRotation * movementDirection;
                movementDirection = new Vector3(movementDirection.x, movementY, movementDirection.z);
                if (!wasMoving)
                {
                    wasMoving = true;
                }
            }
            else
            {
                if (wasMoving)
                {
                    wasMoving = false;
                }
            }
            if(IsGroundedCheck() && Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                
            }
            Debug.DrawRay(transform.position, movementDirection, Color.cyan);
            body.velocity = movementDirection;
            ActionsControl();
            HeightManager();
        }

        private void MovementInAir()
        {
            HeightManager();
        }

        private void CameraControl()
        {
            Vector2 inputVector = new Vector2(Input.GetAxis("MouseHorizontal"), Input.GetAxis("MouseVertical"));
            float horiSpeed = inputVector.x * cameraSpeed * Time.deltaTime;
            float vertiSpeed = inputVector.y * cameraSpeed * Time.deltaTime;
            transform.rotation *= Quaternion.AngleAxis(horiSpeed, Vector3.up);
            head.rotation *= Quaternion.AngleAxis(vertiSpeed, Vector3.right);
            Quaternion rot = head.localRotation;
            float xAngle = rot.eulerAngles.x;
            /*if (xAngle < 360 && xAngle >= upMaxAngle - 10)
            {
                if (xAngle < upMaxAngle)
                {
                    xAngle = upMaxAngle;
                }
            }
            else if (xAngle >= 0 && xAngle <= downMaxAngle + 10)
            {
                if (xAngle > downMaxAngle)
                {
                    xAngle = downMaxAngle;
                }
            }*/
            head.localRotation = Quaternion.Euler(xAngle, 0, 0);
        }

        private void ActionsControl()
        {
            if(Input.GetMouseButtonDown(0))
            {
                attacking = true;
                anim.SetBool("attacking", attacking);
            }

            if(Input.GetMouseButtonDown(1))
            {
                if (!chargingTp) chargingTp = true;
                anim.SetBool("chargingTp", chargingTp);
            }

            if(chargingTp)
            {
                tpTarget.ShowTargetAt(head.forward * 3, head);
            }

            if(Input.GetMouseButtonUp(1) && chargingTp)
            {
                chargingTp = false;
                castingTp = true;
                anim.SetBool("chargingTp", chargingTp);
                anim.SetBool("castingTp", castingTp);
            }
        }

        private void HeightManager()
        {
            if(movementState == CharacterState.Grounded)
            {
                //movementY = 0;
            }
            else if(movementState == CharacterState.InAir)
            {
              
            }
            if (isJumping)
            {
                movementY = ascendingCurve.Evaluate(heigthInc) * jumpForce;
                heigthInc += Time.deltaTime * heightCurvesSpeed;
                if (heigthInc > 1f)
                {
                    heigthInc = 0;
                    isJumping = false;
                    isDescending = true;
                }
            }
            if(isDescending)
            {
                movementY = descendingCurve.Evaluate(heigthInc) * descendingForce;
                heigthInc += Time.deltaTime * heightCurvesSpeed;
                if (heigthInc > 1f)
                {
                    heigthInc = 0;
                    isDescending = false;
                }
            }
            body.velocity = new Vector3(body.velocity.x, movementY, body.velocity.z);
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
            if (Input.GetAxisRaw("KeyHorizontal") != 0 || Input.GetAxisRaw("KeyVertical") != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Animation Events

        public void EndAttackState()
        {
            attacking = false;
            anim.SetBool("attacking", attacking);
        }

        public void EndCastingTpState()
        {
            castingTp = false;
            anim.SetBool("castingTp", castingTp);
        }

        public void EnableAttackHitbox()
        {
            attackCollider.SetActive(true);
        }

        public void DisableAttackHitbox()
        {
            attackCollider.SetActive(false);
        }

        #endregion
    }
}
