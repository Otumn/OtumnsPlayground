using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Playground
{
    public class CBotTpTarget : Entity
    {
        [SerializeField] private Transform visual;
        [SerializeField] private Animator anim;
        private bool isShown = false;

        public void ShowTargetAt(Vector3 target, Transform origin)
        {
            transform.position = origin.position + target;
            //visual.rotation = Quaternion.LookRotation(-(origin - target), Vector3.up);
        }


    }
}
