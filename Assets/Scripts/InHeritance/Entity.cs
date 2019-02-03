using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Playground
{
    public class Entity : MonoBehaviour
    {
        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

        protected virtual void LateUpdate()
        {

        }

        protected virtual void FixedUpdate()
        {

        }

        protected virtual void OnEnable()
        {
            GameManager.state.RegisterEntity(this);
        }

        protected virtual void OnDisable()
        {
            GameManager.state.UnRegisterEntity(this);
        }
    }
}
