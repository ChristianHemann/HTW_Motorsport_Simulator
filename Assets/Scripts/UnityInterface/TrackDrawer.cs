using CalculationComponents;
using CalculationComponents.TrackComponents;
using Output;
using UnityEngine;

namespace UnityInterface
{
    public class TrackDrawer : MonoBehaviour
    {
        private ImportantClasses.Vector2[] _positions;
        public GameObject Cone;

        void Start()
        {
            //sets the car on the starting line
            foreach (TrackSegment trackSegment in Track.Instance.TrackSegments)
            {
                if (trackSegment is StartLine)
                {
                    OverallCarOutput.LastCalculation.Direction = trackSegment.EndDirection;
                    OverallCarOutput.LastCalculation.Position = trackSegment.EndPoint;
                    break;
                }
            }

            //place cones
            _positions = Track.Instance.GetConePositions();
            foreach (ImportantClasses.Vector2 position in _positions)
            {
                Instantiate(Cone, new Vector3(position.X, 0f, position.Y), new Quaternion(0, 0, 0, 0));
            }
        }
    }
}
