using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(Controller2D), typeof(DeletePlayerPrefs))]

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

    DeletePlayerPrefs del;
    Vector3 velocity;
    Animator _animator;
    Controller2D controller;

    private void Start()
    {
        controller = GetComponent<Controller2D>();
        _animator = GetComponentInChildren<Animator>();
        del = GetComponent<DeletePlayerPrefs>();

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

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level1"))
        {
            if (this.transform.position.x >= 764 && this.transform.position.x <= 765)
            {
                this.transform.Translate(new Vector3(-883f, 0f));
            }
            else if (this.transform.position.x <= -124 && this.transform.position.x >= -125)
            {
                this.transform.Translate(new Vector3(883f, 0f));
            }
        } else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level2"))
        {

        }



    }

    public void dealDMG(int damage)
    {

        _animator.SetTrigger(Animator.StringToHash("AnimatorAttack"));
        health -= damage;
        if (health == 0)
        {
            del.DeleteAll();
            SceneManager.LoadScene(1);
        }
    }
}
