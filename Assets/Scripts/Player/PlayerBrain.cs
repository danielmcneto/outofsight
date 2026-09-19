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
    public GameObject raycastLeft;
    public GameObject raycastRight;
    public GameObject shouldertLeft;
    public GameObject shoulderRight;


    //internal variables accessible across the script
    private Vector3 movement;
    private float speed_y = 0f;
    
    //this is for the double press in the WallAttach function
    private float timebetweenpress = 0.25f;
    private float lasttimepressed = -999f;
    
    //bools to control what the player can or cant do
    public bool canmove = true;
    public bool sholderpeeking = false;
    public bool wallglued = false;

    //IMPORTANT used to get the wall normals
    private Vector3 attachedWallNormal;
    private Quaternion cameraOgRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeComponents();
        cameraOgRotation = rig.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {   //initialize movement direction
        movement = Vector3.zero;
        PlayerMove();
        Gravity(movement);
        WallAttach();
        
        Debug.DrawRay(transform.position, Vector3.forward, Color.green);
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

        if (!sholderpeeking)
        {
            //calculate camera offset
            Vector3 camerafinalposition = transform.position + rigoffset;

            //camera follow
            rig.transform.position = Vector3.Lerp(rig.transform.position, camerafinalposition, 1 - Mathf.Exp(-cameraspeed * Time.deltaTime));
        }
        
        

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
        //If the player is glued to the wall shoots the raycast on the back of the player
        Vector3 rayDirection = wallglued ? -attachedWallNormal : transform.forward;

        if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, 0.75f))
        {
            if (hit.transform.tag == "Wall")
            {
                if(!wallglued){Debug.Log("player is infront of a wall");}
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //check if press is within double press window
                    if (Time.time - lasttimepressed <= timebetweenpress)
                    {
                        Debug.Log("attach logic triggered");
                        //execute what to change here
                        canmove = false;
                        wallglued = true;

                        //makes the player look the same way as the wall
                        attachedWallNormal = hit.normal;
                        attachedWallNormal.y = 0;
                        Quaternion targetRotation = transform.rotation = Quaternion.LookRotation(attachedWallNormal);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
                        
                        //reset time and prevent anything beyond a double press
                        lasttimepressed = -999f;
                    }
                    else
                    {
                        //if this is the first press make sure a double press can be executed
                        lasttimepressed = Time.time;
                    }
                }
                if (wallglued){
                    Vector3 wallMovement = Vector3.zero;

                    if (Input.GetKey(KeyCode.W))
                        wallMovement += Vector3.forward;

                    if (Input.GetKey(KeyCode.S))
                        wallMovement -= Vector3.forward;

                    if (Input.GetKey(KeyCode.D))
                        wallMovement -= Vector3.left;

                    if (Input.GetKey(KeyCode.A))
                        wallMovement += Vector3.left;

                    controller.Move(wallMovement * speed * Time.deltaTime);

                    // Peek from the left shoulder when that side is clear.
                    if(!Physics.Raycast(raycastLeft.transform.position, rayDirection, out RaycastHit hitshoulderLeft, 0.75f))
                    {
                        sholderpeeking = true;
                        Quaternion cameraFinalAngle = Quaternion.LookRotation(-attachedWallNormal, Vector3.up);
                        rig.transform.position = Vector3.Lerp(rig.transform.position, shouldertLeft.transform.position, 1 - Mathf.Exp(-cameraspeed * Time.deltaTime));
                        rig.transform.rotation = Quaternion.Lerp(rig.transform.rotation, cameraFinalAngle, 1 - Mathf.Exp(-cameraspeed * Time.deltaTime));
                    }
                    // Otherwise peek from the right shoulder when that side is clear.
                    else if(!Physics.Raycast(raycastRight.transform.position, rayDirection, out RaycastHit hitshoulderRight, 0.75f))
                    {
                        sholderpeeking = true;
                        Quaternion cameraFinalAngle = Quaternion.LookRotation(-attachedWallNormal, Vector3.up);
                        rig.transform.position = Vector3.Lerp(rig.transform.position, shoulderRight.transform.position, 1 - Mathf.Exp(-cameraspeed * Time.deltaTime));
                        rig.transform.rotation = Quaternion.Lerp(rig.transform.rotation, cameraFinalAngle, 1 - Mathf.Exp(-cameraspeed * Time.deltaTime));
                    }
                    // Stop peeking and restore the original camera rotation when both sides are blocked.
                    else
                    {
                        sholderpeeking = false;
                        rig.transform.rotation = Quaternion.Lerp(rig.transform.rotation, cameraOgRotation, 1 - Mathf.Exp(-cameraspeed * Time.deltaTime));
                    }
                }
                else
                {
                    sholderpeeking = false;
                    rig.transform.rotation = Quaternion.Lerp(rig.transform.rotation, cameraOgRotation, 1 - Mathf.Exp(-cameraspeed * Time.deltaTime));
                }

                
                
                //Now the player can simply press the oposite key
                #region GetUnglued
                //leave wall execution
                //if (wallglued && Input.GetKeyDown(KeyCode.E))
                //{
                    //double press
                    //if (Time.time - lasttimepressed <= timebetweenpress)
                    //{
                        //wallglued = false;
                        //canmove = true;
                    //}
                    //else
                    //{
                        //lasttimepressed = Time.time;
                    //}
                //}
                #endregion
            }
        }
        else
        {
            wallglued = false;
            sholderpeeking = false;
            canmove = true;
        }
    }
}
