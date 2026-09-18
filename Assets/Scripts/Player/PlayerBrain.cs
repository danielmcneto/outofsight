using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    //player stuff
    public GameObject rig;
    public Vector3 rigoffset;
    public CharacterController controller;
    public float speed = 4f;
    //never make gravitystrength negative!!! it does that in the code already
    public float gravitystrength = 12f;
    public float cameraspeed = -5f;

    //internal variables accessible across the script
    private Vector3 movement;
    private float speed_y = 0f;
    
    //this is for the double press in the WallAttach function
    private float timebetweenpress = 0.25f;
    private float lasttimepressed = -999f;
    
    //bools to control what the player can or cant do
    public bool canmove = true;
    public bool wallglued = false;
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeComponents();
    }

    // Update is called once per frame
    void Update()
    {   //initialize movement direction
        movement = Vector3.zero;
        PlayerMove();
        Gravity(movement);
        WallAttach();
        
        Debug.DrawRay(transform.position, transform.forward, Color.green);
    }

    //try and call all needed requirements by calling this
    private void InitializeComponents()
    {
        if(controller == null) { TryGetComponent<CharacterController>(out controller); }
    }
    
    //this allows the player move and everything (this allows the player to move but gravity executes the final Move call)
    private void PlayerMove()
    {
        //input stuff that tells the camera rig where the player is heading
        if (canmove)
        {
            if (Input.GetKey(KeyCode.W)) movement += rig.transform.forward;
            if (Input.GetKey(KeyCode.A)) movement -= rig.transform.right;
            if (Input.GetKey(KeyCode.S)) movement -= rig.transform.forward;
            if (Input.GetKey(KeyCode.D)) movement += rig.transform.right;
        }

        //keep movement horizontal
        movement.y = 0f;
        
        //calculate camera offset
        Vector3 camerafinalposition = transform.position + rigoffset;
        
        //camera follow
        rig.transform.position = Vector3.Lerp(rig.transform.position, camerafinalposition, 1 - Mathf.Exp(-cameraspeed * Time.deltaTime));
        
        //if the player is actually inputting something in via wasd
        if (movement != Vector3.zero)
        {
            //ensure diagonal movement speed is the same as nondiagonal
            movement.Normalize();
            //make player face the movement direction
            if(canmove) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 10f * Time.deltaTime);
        }
    }

    //this applies the vertical gravity on the player, it also allows the player to actually move
    private void Gravity(Vector3 vector)
    {
        if (controller.isGrounded)
        {
            //stay on the ground when grounded
            speed_y = -2f;
        }
        else
        {
            //apply physics on player via modifying the vertical speed
            float gravity = gravitystrength * -1;
            speed_y += gravity * Time.deltaTime;
        }
        
        //calculate gravity
        Vector3 finalmove = (vector * speed) + new Vector3(0f, speed_y, 0f);
        //apply by moving the player + applied gravity
        if(canmove) controller.Move(finalmove* Time.deltaTime);
    }
    
    //the wall gluing thing
    private void WallAttach()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 0.75f))
        {
            if (hit.transform.tag == "Wall")
            {
                Debug.Log("player is infront of a wall");
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //check if press is within double press window
                    if (Time.time - lasttimepressed <= timebetweenpress)
                    {
                        Debug.Log("attach logic triggered");
                        //execute what to change here
                        canmove = false;
                        wallglued = true;
                        transform.forward = -hit.normal;
                        
                        //reset time and prevent anything beyond a double press
                        lasttimepressed = -999f;
                    }
                    else
                    {
                        //if this is the first press make sure a double press can be executed
                        lasttimepressed = Time.time;
                    }
                }
                if (wallglued && Input.GetKey(KeyCode.D))
                {
                    controller.transform.position += rig.transform.right * 2f * Time.deltaTime;
                }
                if (wallglued && Input.GetKey(KeyCode.A))
                {
                    controller.transform.position -= rig.transform.right * 2f * Time.deltaTime;
                }
                
                //leave wall execution
                if (wallglued && Input.GetKeyDown(KeyCode.S))
                {
                    //double press
                    if (Time.time - lasttimepressed <= timebetweenpress)
                    {
                        wallglued = false;
                        canmove = true;
                    }
                    else
                    {
                        lasttimepressed = Time.time;
                    }
                }
            }
        }
        else
        {
            wallglued = false;
            canmove = true;
        }
    }
}
