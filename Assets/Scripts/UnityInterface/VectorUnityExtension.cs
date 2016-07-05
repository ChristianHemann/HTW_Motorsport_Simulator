using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityInterface
{
    public static class VectorUnityExtension
    {
        /// <summary>
        /// Converts the ImportantClasses.Vector3 into a UnityEngine.Vector3
        /// </summary>
        /// <param name="vector">the ImportantClasses.Vector3</param>
        /// <returns>The UnityEngine.Vector3</returns>
        public static Vector3 ToUnityVector3(this ImportantClasses.Vector3 vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        /// Converts the ImportantClasses.Vector2 into a UnityEngine.Vector2
        /// </summary>
        /// <param name="vector">the ImportantClasses.Vector2</param>
        /// <returns>The UnityEngine.Vector2</returns>
        public static Vector2 ToUnityVector2(this ImportantClasses.Vector2 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        /// <summary>
        /// Converts the UnityEngine.Vector2 into a ImportantClasses.Vector2
        /// </summary>
        /// <param name="vector">the UnityEngine.Vector2</param>
        /// <returns>The ImportantClasses.Vector2</returns>
        public static ImportantClasses.Vector2 ToImportantClassesVector2(this Vector2 vector)
        {
            return new ImportantClasses.Vector2(vector.x, vector.y);
        }

        /// <summary>
        /// Converts the UnityEngine.Vector2 into a ImportantClasses.Vector3
        /// </summary>
        /// <param name="vector">the UnityEngine.Vector3</param>
        /// <returns>The ImportantClasses.Vector3</returns>
        public static ImportantClasses.Vector3 ToImportantClassesVector3(this Vector3 vector)
        {
            return new ImportantClasses.Vector3(vector.x, vector.y, vector.z);
        }
    }
}
