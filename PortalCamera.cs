// Unfinished code. This was intended to give the player the bigger-on-the-inside effect that the TARDIS has in the TV show.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public Transform player;
    public Transform playerCamera;
    public Transform portal;
    public Transform otherPortal;

    private void Update()
    {
        //playerCamera = new Vector3(player.rotation)
        Vector3 playerOffsetFromportal = playerCamera.position - otherPortal.position;
        transform.position = portal.position + playerOffsetFromportal;

        float angularDifferenceBetweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);

        Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);

        Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }
}
