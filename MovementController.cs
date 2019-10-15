using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float moveSpeed = .1f;
    public Vector3 movementPos;
    public Vector3 rotation;
    public float mouseSensitivity = 100.0f;
    public float rotY = 0.0f;
    public float rotX = 0.0f;
    public LayerMask layer;
    public Vector3 raycastTarget = Vector3.zero;
    public Color col = Color.yellow;
    public bool moveControl;
    void Start()
    {
        moveControl = true;
    }


    // Make a chase the object game loop
        // need an object that moves away or randomly from player
        // player that moves
        // detect when you catch the object
        // inform the player that they've caught it 
    void Update()
    {
        rotation = transform.localRotation.eulerAngles;
        rotation.y += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //rotation.x += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(rotation);
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        { 
            float moveForward = Input.GetAxis("Vertical");
            float moveSideways = Input.GetAxis("Horizontal");
            transform.position = transform.position + Camera.main.transform.forward * (moveSpeed * moveForward) * Time.deltaTime;
            transform.position = transform.position + Camera.main.transform.right * (moveSpeed * moveSideways) * Time.deltaTime;
        }
        if(Vector3.Distance(transform.position, target) < 1)
        {
            pickRandomTarget();
        }
        Vector3.Lerp(transform.position, target, Time.deltaTime);
        //Vector3.MoveTowards(transform.position, target, .5f);
    }
    Vector3 target;
    public Transform player;
    void pickRandomTarget()
    {
        target = new Vector3((float)(Random.Range(-5, 5)), 1f, (float)(Random.Range(-5, 5)));
        //target = target + (player.position - transform.position);
    }
    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 500, Color.green);
        
        if (Physics.Raycast(ray, out hit, 500, layer))
        {
            if(hit.transform.gameObject.name == "Cube")
            {
                hit.transform.gameObject.GetComponent<Animator>().SetBool("move", moveControl);
                moveControl = !moveControl;
                hit.transform.gameObject.GetComponent<Renderer>().material.SetColor("_Color",col);
                col = new Color((int)(Random.Range(0, 255)), (int)(Random.Range(0, 255)), (int)(Random.Range(0, 255)));
            }
        }
    }
}
