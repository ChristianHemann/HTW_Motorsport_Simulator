using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



namespace UnityInterface
{
   public class CameraFollowCar : MonoBehaviour
    {
        public GameObject target;
        public float xOffset = 0;
        public float yOffset = 0;
        public float zOffset = 0;

        void LateUpdate()
        {
            this.transform.position = new Vector3(target.transform.position.x + xOffset,
                                                  target.transform.position.y + yOffset,
                                                  target.transform.position.z + zOffset);
                   }
    }
}
