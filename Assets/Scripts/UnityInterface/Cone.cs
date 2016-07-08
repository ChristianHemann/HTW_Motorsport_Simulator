using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityInterface
{
    public class Cone : MonoBehaviour
    {
        private Vector3 _normalPosition;
        void Start()
        {
            _normalPosition = transform.position;
        }

        void OnCollisionEnter() //nachschauen ob die function wirklich so heißt
        {
            //setze timer und rufe function ResetCone nach x sekunden auf
        }

        void ResetCone()
        {
            transform.position = _normalPosition;
        }



        
        //der code ist zum position des autos setzen; löschen sobald benutzt
            //this.transform.position = OverallCarOutput.LastCalculation.Position.ToUnityVector3();
            //this.transform.rotation = Quaternion.Euler(OverallCarOutput.LastCalculation.Direction.ToUnityVector3());
    }
}
