using Dink.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TARDISController : MonoBehaviour
{

    public Vector3 destinationLocation;
    private Vector3 playerLocation;

    private Animation anim;
    private Animator animator;
    private GameObject player;

    private bool isTeleporting;
    public bool isDematerializing;
    public bool isRematerializing;

    Rigidbody rb;
    Shader standard;
    Shader transparent;

    private Material tardisMaterial;

    Renderer rend;

    public enum DematType
    {
        NORMAL,
        EMERGENCY,
    }

    private AudioSource dematSpeaker;
    private AudioSource rematSpeaker;

    private AudioClip dematSound;
    private AudioClip rematSound;
    private AudioClip emergencyDematSound;
    private AudioClip emergencyRematSound;

    private int health;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        rematSpeaker = GetComponent<AudioSource>();
        anim = GetComponent<Animation>();
        animator = GetComponent<Animator>();

        player = GameObject.Find("Player");
        standard = Shader.Find("Standard");
        transparent = Shader.Find("Custom/Transparency");

        dematSound = Resources.Load<AudioClip>("Sounds/tardis_takeoff");
        rematSound = Resources.Load<AudioClip>("Sounds/tardis_landing");
        emergencyDematSound = Resources.Load<AudioClip>("Sounds/tardis_emergencyTakeoff");
        emergencyRematSound = Resources.Load<AudioClip>("Sounds/tardis_emergencyLanding");

        //Stop NullReferenceException when teleporting without a destination selected (Will now show the appropriate error)
        destinationLocation = Vector3.zero;

        rematSpeaker.loop = false;
        isTeleporting = false;

        animator.Play("idle");
    }

    private void Update()
    {
        playerLocation = player.transform.position;
    }

    void Teleport(Vector3 destination)
    {
        transform.position = destination;
    }

    public void SetDestination(Vector3 destination)
    {
        destinationLocation = destination;
        NotificationManager.Instance.SetNewNotification("Target destination set to " + destination);
        Debug.Log("TARDIS: Set destination to " + destination);

    }

    public void BeginSequence(DematType type)
    {
        if (!isTeleporting)
        {
            if(destinationLocation == Vector3.zero)
            {
                NotificationManager.Instance.SetNewNotification("Teleport cancelled! No destination!");
                Debug.Log("Dematerialization cancelled: No destination");
            }
            else
            {
                Debug.Log("Beginning take-off sequence");
                StartCoroutine(MovingSequence(type));
            }
        }
    }

    IEnumerator MovingSequence(DematType type)
    {
        NotificationManager.Instance.SetNewNotification("Taking off...");

        yield return StartCoroutine(Dematerialize(type));
        Vector3 lookAtLand = new Vector3(playerLocation.x, 0, playerLocation.z);
        transform.Rotate(0, -180, 0);
        transform.LookAt(lookAtLand);

        if (type == DematType.EMERGENCY)
        {
            yield return new WaitForSeconds(7f);
        }
        else
        {
            yield return new WaitForSeconds(11f);
        }
        

        NotificationManager.Instance.SetNewNotification("Landing...");

        yield return StartCoroutine(Rematerialize(type));

        NotificationManager.Instance.SetNewNotification("Landed!");

        destinationLocation = Vector3.zero;

        yield break;
    }

    IEnumerator Dematerialize(DematType dematType)
    {
        isTeleporting = true;

	// Disable gravity
        rb.isKinematic = true;
        isDematerializing = true;

        //Find a way to condense this and make it less ugly :)
        if(dematType == DematType.NORMAL)
        {
            GetComponent<AudioSource>().PlayOneShot(dematSound);
            yield return new WaitForSeconds(2f);

            rend.material.shader = transparent;
            animator.Play("demat");
            yield return new WaitForSeconds(8.2f);
        }
        else if(dematType == DematType.EMERGENCY)
        {
            NotificationManager.Instance.SetNewNotification("EMERGENCY: Fast Dematerializing...");
            GetComponent<AudioSource>().PlayOneShot(emergencyDematSound);

            rend.material.shader = transparent;
            animator.Play("emergencyDemat");
            yield return new WaitForSeconds(5.0f);
        }
        

        animator.Play("invisible");

        transform.eulerAngles = new Vector3(transform.rotation.x, 0, transform.rotation.z);
        isDematerializing = false;
        isTeleporting = false;
        yield break;
    }

    IEnumerator Rematerialize(DematType dematType)
    {
        // TODO: FIX TARDIS NOT USING GRAVITY WHEN FULLY REMATERIALIZED
        isRematerializing = true;
        Teleport(destinationLocation);

        // This too :(
        if (dematType == DematType.NORMAL)
        {
            GetComponent<AudioSource>().PlayOneShot(rematSound);
            yield return new WaitForSeconds(10.5f);

            animator.Play("remat");
            yield return new WaitForSeconds(9f);
            animator.Play("idle");
            isRematerializing = false;
            yield break;
        }
        else if(dematType == DematType.EMERGENCY)
        {
            NotificationManager.Instance.SetNewNotification("EMERGENCY: Unstable Landing");
            GetComponent<AudioSource>().PlayOneShot(emergencyRematSound);
            yield return new WaitForSeconds(1.3f);

            animator.Play("emergencyRemat");
            yield return new WaitForSeconds(15.2f);

            animator.Play("idle");
            isRematerializing = false;
            yield break;
        }
        rend.material.shader = standard;
	// Re-enable gravity
        rb.isKinematic = false;


    }
}
