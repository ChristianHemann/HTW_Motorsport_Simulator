using System.Timers;
using UnityEngine;

namespace UnityInterface
{
    public class Cone : MonoBehaviour
    {
        public float SecondsUntillReset = 2;
        private Vector3 _normalPosition;
        private Rigidbody _rigidbody;
        private float _time;

        void Start()
        {
            _normalPosition = transform.position;
            if (SecondsUntillReset <= 0f)
                SecondsUntillReset = 2;
            _rigidbody = GetComponent<Rigidbody>();

        }

        void Update()
        {
            if (_time > 0f)
            {
                _time -= Time.deltaTime;
                if (_time <= 0)
                {
                    _rigidbody.angularVelocity = Vector3.zero;
                    _rigidbody.velocity = Vector3.zero;
                    transform.position = _normalPosition;
                    transform.rotation = Quaternion.identity;
                }
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name != "Plane")
                _time = SecondsUntillReset;
        }
    }
}
