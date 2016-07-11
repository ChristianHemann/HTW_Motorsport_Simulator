using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using ImportantClasses;
using Output;


namespace UnityInterface
{
    class CarPosition : MonoBehaviour
    {
        void LateUpdate()
        {
            //this.transform.position = new ImportantClasses.Vector3();
            this.transform.position = OverallCarOutput.LastCalculation.Position.ToUnityVector3();
             this.transform.rotation = Quaternion.Euler(OverallCarOutput.LastCalculation.Direction.ToUnityVector3());

        }
    }


    // this.transform.position = OverallCarOutput.LastCalculation.Position.ToUnityVector3();
    // this.transform.rotation = Quaternion.Euler(OverallCarOutput.LastCalculation.Direction.ToUnityVector3());
}
 

