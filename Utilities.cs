

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dink.Util
{
    public class Utilities : MonoBehaviour
    {

        public static float DistanceFromPlayer(GameObject from)
        {
            Vector3 playerLocation = GameObject.Find("Player").transform.position;
            return Vector3.Distance(from.transform.position, playerLocation);
        }
    }
}
