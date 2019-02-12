using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Playground
{
    public class TPSCameraController : Entity
    {
        [SerializeField] private float rotationSpeed = 100f;
        [SerializeField] private float upMaxAngle = 310;
        [SerializeField] private float downMaxAngle = 50;
        [SerializeField] private Transform rotationTarget;
        [SerializeField] private Transform followTarget;

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
            rotationTarget.rotation *= Quaternion.AngleAxis(horiSpeed, Vector3.up);
            rotationTarget.rotation *= Quaternion.AngleAxis(vertiSpeed, Vector3.right);
            Quaternion rot = transform.rotation;
            float xAngle = rot.eulerAngles.x;
            if(xAngle < 360 && xAngle >= upMaxAngle - 10)
            {
                if(xAngle < upMaxAngle)
                {
                    xAngle = upMaxAngle;
                }
            }
            else if(xAngle >= 0 && xAngle <= downMaxAngle + 10)
            {
                if(xAngle > downMaxAngle)
                {
                    xAngle = downMaxAngle;
                }
            }
            rotationTarget.rotation = Quaternion.Euler(xAngle, rot.eulerAngles.y, 0);
            rotationTarget.position = followTarget.position;
        }
    }

    // Cinemachine version
    /*public class CameraController : Entity
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
            transform.rotation *= Quaternion.AngleAxis(horiSpeed, Vector3.up);
            transform.rotation *= Quaternion.AngleAxis(vertiSpeed, Vector3.right);
            Quaternion rot = transform.rotation;
            transform.rotation = Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y, 0);
        }
    }*/
}
