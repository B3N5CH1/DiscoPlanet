using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Controller2D))]

public class Player : MonoBehaviour {

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    public float movespeed = 6;
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;

    int health = 100;

    float jumpVelocity;
    float gravity;
    float velocityXSmoothing;

    Vector3 velocity;

    Controller2D controller;

    private void Start()
    {
        controller = GetComponent<Controller2D>();


        // Calculate once the gravity and the jump velocity using those two physics functions :
        // dx = v*t + (a*t^2)/2
        // vf = v + a*t
        gravity = -(2 * jumpHeight / Mathf.Pow(timeToJumpApex, 2));
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity " + gravity + " jumpVelocity " + jumpVelocity);
    }

    public float getGravity()
    {
        return gravity;
    }
    private void Update()
    {
        // If the player stands on something the velocity is not accumulated
        if (controller.collisions.above || controller.collisions.bellow)
        {
            velocity.y = 0;
        }

        // Listen to the input of the keyboard
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.bellow)
        {
            velocity.y = jumpVelocity;
        }

        // Calculate the Vector3 which will result in the change of position with the call to the method Move()
        float targetVelocityX = input.x * movespeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.bellow)?accelerationTimeGrounded:accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void dealDMG(int damage)
    {
        health -= damage;
    }
}
