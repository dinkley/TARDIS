using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{

    private float lastPressed;
    private TARDISController TARDIS;
    private Transform TARDISInteriorPosition;
    public bool isMapOpen;
    private Camera mapCamera;
    private Camera playerCamera;

    private MasterInput input;
    private Vector2 move;

    // Start is called before the first frame update
    void Start()
    {
        input = new MasterInput();
        input.Enable();
        TARDIS = GameObject.Find("TARDIS").GetComponent<TARDISController>();
        lastPressed = Time.time;
        isMapOpen = false;
        mapCamera = GameObject.Find("Map Camera").GetComponent<Camera>();
        playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        TARDISInteriorPosition = GameObject.Find("InteriorSpawnPosition").transform;
        mapCamera.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        DebugCommandUpdate();
        MapCommandUpdate();
        UseKeyUpdate();
    }

    void DebugCommandUpdate()
    {
        
        if(input.Player.SetTARDISDestination.triggered)
        {
            if (lastPressed != Time.time)
            {
                lastPressed = Time.time;
                setTardisDestination();
            }
        }

        if(input.Player.CallTARDIS.triggered)
        {
            if(lastPressed != Time.time)
            {
                lastPressed = Time.time;
                TARDIS.BeginSequence(TARDISController.DematType.NORMAL);

            }
        }

        if(input.Player.CallTARDISEmergency.triggered)
        {
            if (lastPressed != Time.time)
            {
                lastPressed = Time.time;
                TARDIS.BeginSequence(TARDISController.DematType.EMERGENCY);

            }
        }
    }

    void MapCommandUpdate()
    {
        move = input.Player.Move.ReadValue<UnityEngine.Vector2>();
        float horizInput = move.y;
        float vertInput = move.x;

        //if(Input.GetKeyDown(KeyCode.M))
        if (input.Player.Map.triggered)
        {
            if(lastPressed != Time.time)
            {
                lastPressed = Time.time;
                toggleMap();
            }
        }

        if(isMapOpen)
        {
            //Change Y axis instead of Z, because the camera is pointing down, we have to change its relative rotation, or it'll go on the wrong axis
            mapCamera.transform.Translate(50 * horizInput * Time.deltaTime, 50 * vertInput * Time.deltaTime, 0f);
        }
    }

    void setTardisDestination()
    {
        RaycastHit hit;
        Ray ray;
        if (playerCamera.enabled)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        else
        {
            ray = mapCamera.ScreenPointToRay(Input.mousePosition);
        }


        //If the raycast hits an object in the world
        if (Physics.Raycast(ray, out hit))
        {
            TARDIS.SetDestination(hit.point);
        }
    }

    public void toggleMap()
    {
        if(!isMapOpen) isMapOpen = true;
        else isMapOpen = false;
        
        if(isMapOpen)
        {
            playerCamera.enabled = false;
            mapCamera.enabled = true;
        }
        else
        {
            playerCamera.enabled = true;
            mapCamera.enabled = false;
        }
    }

    public void UseKeyUpdate()
    {
       
    	bool pressed = false;
        if(input.Player.Interact.triggered && !pressed)
        {
            pressed = true;
            if(CanUse(TARDIS))
            {
                transform.position = TARDISInteriorPosition.position;
                Debug.Log(Vector3.Distance(transform.position, TARDIS.transform.position));
            }
        }
	    pressed = false;
    }

    private bool CanUse(TARDISController obj)
    {
        if(Vector3.Distance(transform.position, obj.transform.position) <= 20)
        {
            return true;
        }
        return false;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        input.Disable();
    }

}




