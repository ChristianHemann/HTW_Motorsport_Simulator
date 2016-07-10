using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Output;
using UnityEngine;



namespace UnityInterface
{
    public class CameraFollowCar : MonoBehaviour
    {
        public GameObject target;
        public float xOffset = 0;
        public float yOffset = 0;
        public float zOffset = 0;

        void Start()
        {
            transform.parent = target.transform;
        }

        void LateUpdate()
        {
            //this.transform.position = new Vector3(target.transform.position.x + xOffset,
            //                                      target.transform.position.y + yOffset,
            //                                      target.transform.position.z + zOffset);
            //transform.rotation = Quaternion.Euler(target.transform.rotation.x, target.transform.rotation.y+90, target.transform.rotation.z);
            this.transform.rotation = Quaternion.FromToRotation(new UnityEngine.Vector3(0, 0, 1),
                OverallCarOutput.LastCalculation.Direction.ToUnityVector3());
            this.transform.localPosition = new Vector3(xOffset, yOffset, zOffset);
        }
    }
}
