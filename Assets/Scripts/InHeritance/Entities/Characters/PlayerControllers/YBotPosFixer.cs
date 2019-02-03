using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Playground
{
    public class YBotPosFixer : Entity
    {
        protected override void LateUpdate()
        {
            base.LateUpdate();
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
}
