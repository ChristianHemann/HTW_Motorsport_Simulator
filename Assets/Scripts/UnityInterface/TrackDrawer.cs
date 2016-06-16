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
        private Vector<float>[] _positions;
        public GameObject cone;

        void Start()
        {
            _positions = Track.Instance.GetConePositions();
            foreach (Vector<float> position in _positions)
            {
                GameObject.Instantiate(cone, new Vector3(position.At(0), 0f, position.At(1)), new Quaternion(0, 0, 0, 0));
            }
        }
    }
}
