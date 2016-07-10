using Output;
using UnityEngine;
using UnityInterface;

namespace Assets.Scripts.UnityInterface
{
    /// <summary>
    /// connect this script to the camera
    /// </summary>
    public class SetPositions : MonoBehaviour
    {
        public GameObject Car;

        public void Start()
        {
            transform.parent = Car.transform;
        }

        public void LateUpdate()
        {
            Car.transform.position = OverallCarOutput.LastCalculation.Position.ToUnityVector3();
            Car.transform.rotation = Quaternion.FromToRotation(new Vector3(1, 0, 0),
                OverallCarOutput.LastCalculation.Direction.ToUnityVector3());
            
        }
    }
}
