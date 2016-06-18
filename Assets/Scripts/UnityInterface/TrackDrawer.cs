using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

namespace UnityInterface
{
    public class TrackDrawer : MonoBehaviour
    {
        private ImportantClasses.Vector2[] _positions;
        public GameObject cone;

        void Start()
        {
            _positions = Track.Instance.GetConePositions();
            foreach (ImportantClasses.Vector2 position in _positions)
            {
                GameObject.Instantiate(cone, new Vector3(position.X, 0f, position.Y), new Quaternion(0, 0, 0, 0));
            }
        }
    }
}
