using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Playground
{
    [RequireComponent(typeof(Rigidbody))]
    public class Character : Entity
    {
        [Header("Character attributes")]
        [SerializeField] protected CharacterState movementState;
        [SerializeField] protected Rigidbody body;

        protected virtual void Movement()
        {

        }

        protected virtual void Animations()
        {

        }

        protected CharacterState MovementState { get => movementState; set => movementState = value; }
    }

    public enum CharacterState
    {
        Grounded,
        InAir
    };
}
