using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

namespace MirrorDemoSks
{
    public class PlayerMovement1 : NetworkBehaviour
    {
        [SerializeField] private CharacterController charController = null;
        [SerializeField] private float speed = 30f;
        private Animator animator;
        private float maximumRunVelocity = 2.0f;
        private float maximumWalkVelocity = 0.5f;
        private float velocityZ = 0.0f;
        private float velocityX= 0.0f; 
        private float decceleration= 2f; 
        private float acceleration = 2f;

        private int VelocityZHash = Animator.StringToHash("Velocity Z");
        private int VelocityXHash = Animator.StringToHash("Velocity X");


        [SerializeField]private FixedJoystick joystick;
        [SerializeField] private TextMeshProUGUI text_tmp;
        private NetworkIdentity networkIdentity = null;
        private void Awake()
        {
            animator = transform.GetComponent<Animator>();
        }
        private void Start()
        {
            GameObject[] joystickGos = GameObject.FindGameObjectsWithTag("JoyStick");
            foreach(var go in joystickGos)
            {
                /*if (go.GetComponent<>)
                {
                    //>>joystick = go.GetComponent<FixedJoystick>();
                }*/
                joystick = go.GetComponent<FixedJoystick>();
            }

            foreach(var go in Utility.ReturnChildrenGos(transform))
            {
                if (go.name == "PlayerCanvas")
                {
                    
                }
            }


        }

        [ClientCallback]
        private void Update()
        {
            if (!hasAuthority) return;
            Vector3 movement = new Vector3();

            float vFloat = Input.GetAxisRaw("Horizontal");
            float hFloat = Input.GetAxisRaw("Vertical");
            vFloat = joystick.Horizontal;
             hFloat = joystick.Vertical;
            movement.z += hFloat * Time.deltaTime * speed;
            movement.x += vFloat * Time.deltaTime * speed;
            charController.Move(movement);
            if (charController.velocity.magnitude > 0.2f)
            {
                transform.rotation = Quaternion.LookRotation(movement);
            }

            HandleInput();

            //set the parameters to our local variables
            animator.SetFloat(VelocityZHash, velocityZ);
            animator.SetFloat(VelocityXHash, velocityX);
        }

        public void HandleInput()
        {
            //if using keyboard
            bool forwardPressed = Input.GetKey(KeyCode.W);
            bool backwardPressed = Input.GetKey(KeyCode.S);
            bool leftPressed = Input.GetKey(KeyCode.A);
            bool rightPressed = Input.GetKey(KeyCode.D);
            bool runPressed = Input.GetKey(KeyCode.LeftShift);


            //IF using joystick
            if (joystick.Vertical > 0.051f )
            {
                forwardPressed = true;
            }
            else if (joystick.Vertical < -0.051f)
            {
                backwardPressed = true;
            }
            if (joystick.Horizontal  < -0.051f)
            {
                leftPressed = true;
            }
            else if (joystick.Horizontal > 0.051f)
            {
                rightPressed = true;
            }


            if (joystick.Vertical >= 0.65f && joystick.Vertical <= 1f)
            {
                runPressed = true;
            }
            if (joystick.Vertical <= -0.65f && joystick.Vertical >= -1f)
            {
                runPressed = true;
            }
            if (joystick.Horizontal >= 0.65f && joystick.Horizontal <= 1f)
            {
                runPressed = true;
            }
            if (joystick.Horizontal <= -0.65f && joystick.Horizontal >= -1f)
            {
                runPressed = true;
            }


            //set current max Velocity
            float currentMaxVelocity = runPressed ? maximumRunVelocity : maximumWalkVelocity;

            //handle changes in velocity
            ChangeVelocity(forwardPressed, backwardPressed, leftPressed, rightPressed, runPressed, currentMaxVelocity);
            LockOrResetVelocity(forwardPressed, backwardPressed, leftPressed, rightPressed, runPressed, currentMaxVelocity);
        }


        //handles reset or lock of velocity
        private void LockOrResetVelocity(bool forwardPressed, bool backwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
        {
            //reset velocity z
            /*if(!forwardPressed && velocityZ < 0.0f)
            {
                velocityZ = 0.0f;
            }*/
            if (!forwardPressed && !backwardPressed && velocityZ != 0.0f && (velocityZ > -0.05f && velocityZ < 0.05f))
            {
                velocityZ = 0.0f;
            }
            //reset velocity x
            if (!leftPressed && !rightPressed && velocityX != 0.0f&&(velocityX>-0.05f&&velocityX<0.05f))
            {
                velocityX = 0.0f;
            }



            //lock forward
            if (forwardPressed && runPressed && velocityZ >currentMaxVelocity)
            {
                velocityZ = currentMaxVelocity;
            }
            //decellerate to maximum walk velocity
            else if(forwardPressed && velocityZ > currentMaxVelocity)
            {
                velocityZ -= Time.deltaTime * decceleration;
                //round to currentMaxVelocity if within offset

                if( velocityZ >currentMaxVelocity && velocityZ < (currentMaxVelocity + 0.05f))
                {
                    velocityZ = currentMaxVelocity;
                }
            }
            else if (forwardPressed && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.05f))
            {
                //round to currentMaxVelocity if within offset
                velocityZ = currentMaxVelocity;
            }
            
            
            //lock backward
            if (backwardPressed && runPressed && velocityZ <currentMaxVelocity)
            {
                velocityZ = -currentMaxVelocity;
            }
            //decellerate to maximum walk velocity
            else if(backwardPressed && velocityZ < -currentMaxVelocity)
            {
                velocityZ += Time.deltaTime * acceleration;
                //round to currentMaxVelocity if within offset

                if( velocityZ >-currentMaxVelocity && velocityZ < (-currentMaxVelocity + 0.05f))
                {
                    velocityZ = -currentMaxVelocity;
                }
            }
            else if (backwardPressed && velocityZ > -currentMaxVelocity && velocityZ < (-currentMaxVelocity + 0.05f))
            {
                //round to currentMaxVelocity if within offset
                velocityZ = -currentMaxVelocity;
            }

            //locking left
            if (leftPressed && runPressed && velocityX < -currentMaxVelocity)
            {
                velocityX = -currentMaxVelocity;
            }
            //decellerate to maximum walk velocity
            else if (leftPressed && velocityX <- currentMaxVelocity)
            {
                velocityX += Time.deltaTime * decceleration;

                //round to currentMaxVelocity if within offset
                if (velocityX < -currentMaxVelocity && velocityX > (-currentMaxVelocity - 0.05f))
                {
                    velocityX = -currentMaxVelocity;
                }
            }
            else if (leftPressed && velocityX >- currentMaxVelocity && velocityX < (-currentMaxVelocity + 0.05f))
            {
                velocityX = -currentMaxVelocity;
            }


            //locking right
            if (rightPressed && runPressed && velocityX > currentMaxVelocity)
            {
                velocityX = currentMaxVelocity;
            }
            //decellerate to maximum walk velocity
            else if (rightPressed && velocityX > currentMaxVelocity)
            {
                velocityX -= Time.deltaTime * decceleration;

                //round to currentMaxVelocity if within offset
                if (velocityX > currentMaxVelocity && velocityX < (currentMaxVelocity + 0.05f))
                {
                    velocityX = currentMaxVelocity;
                }
            }
            else if (rightPressed && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity - 0.05f))
            {
                velocityX = currentMaxVelocity ;
            }
        }


        //handles change in aceelration and decellaration
        private void ChangeVelocity(bool forwardPressed, bool backwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
        {
            //increase velocity in forward direction
            if (forwardPressed&& velocityZ < currentMaxVelocity)
            {
                velocityZ += Time.deltaTime*acceleration;
            }
            //increase velocity in forward direction
            if (backwardPressed&& velocityZ > -currentMaxVelocity)
            {
                velocityZ -= Time.deltaTime*acceleration;
            }
            //increase velocity in left direction
            if (leftPressed && velocityX > -currentMaxVelocity)
            {
                velocityX -= Time.deltaTime*acceleration;
            }
            //increase velocity in forward direction
            if (rightPressed && velocityX < currentMaxVelocity)
            {
                velocityX += Time.deltaTime * acceleration;
            }

            //decrease velocityz  if forward is not pressed
            if(!forwardPressed && velocityZ > 0.0f)
            {
                velocityZ -= Time.deltaTime * acceleration;
            }
            //decrease velocityz  if forward is not pressed
            if(!backwardPressed && velocityZ < 0.0f)
            {
                velocityZ += Time.deltaTime * acceleration;
            }
            // increase velocityX if left is not pressed and velocityX<0 
            if(!leftPressed && velocityX < 0.0f)
            {
                velocityX += Time.deltaTime * acceleration;
            }
            // decrease velocityX if right is not pressed and velocityX>0 
            if(!rightPressed && velocityX > 0.0f)
            {
                velocityX -= Time.deltaTime * decceleration;
            }
        }
    }
}

