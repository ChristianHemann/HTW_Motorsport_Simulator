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

        void OnCollisionEnter() 
        {
            //call function Resetcone
            Time time = new Time();
            ResetCone();
            
        }

        void ResetCone()
        {
            transform.position = _normalPosition;
        }



        
        
    }
}
