using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Playground
{
    public class CameraController : Entity
    {
        [SerializeField] private float rotationSpeed = 100f;

        protected override void Update()
        {
            base.Update();
            Movement();
        }

        private void Movement()
        {
            Vector2 inputVector = new Vector2(Input.GetAxis("SecondHorizontal"), Input.GetAxis("SecondVertical"));
            float horiSpeed = Input.GetAxis("SecondVertical") * rotationSpeed * Time.deltaTime;
            float vertiSpeed = Input.GetAxis("SecondHorizontal") * rotationSpeed * Time.deltaTime;
            Debug.Log("Hori : " + horiSpeed + " Verti : " + vertiSpeed);
            transform.rotation *= Quaternion.AngleAxis(horiSpeed, Vector3.up);
            //transform.rotation *= Quaternion.AngleAxis(vertiSpeed, Vector3.right);
        }
    }
}
