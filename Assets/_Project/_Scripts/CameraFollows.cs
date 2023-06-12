using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MirrorDemoSks
{
    public class CameraFollows : MonoBehaviour
    {
        public Transform target;
        public float smoothSpeed = 0.125f;
        public Vector3 offset;
        void LateUpdate()
        {
            //Vector3 desiredPosition = target.position + offset;
            Vector3 desiredPosition = target.transform.TransformPoint(offset);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
            transform.LookAt(target);
        }

    }
}

