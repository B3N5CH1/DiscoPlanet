using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

[RequireComponent(typeof(CanvasGroup), typeof(AIController2D))]

public class AI : MonoBehaviour {

    public Player player;

    public float movespeed = 4;
    public float sightRange = 10;
    public int damage = 50;
    public float meleeRange = 5;
    public float gravity = -100;

    public StateMachine<AI> stateMachine { get; set; }

    public Animator _animator;

    SpriteRenderer _spriteR;
    AIController2D _controller;
    Vector3 velocity;

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _controller = GetComponent<AIController2D>();
        _spriteR = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        stateMachine = new StateMachine<AI>(this);

        stateMachine.changeState(IdleState.Instance);

    }

    public void attack()
    {

        player.dealDMG(damage);
    }

    public bool detect()
    {

        return getDistance() <= sightRange;
    }

    private float getDistance()
    {
      
        return Vector3.Distance(transform.position, player.transform.position);
    }

    public bool melee()
    {
        return getDistance() <= meleeRange;
    }

    public void chase()
    {
        Vector3 heading = player.transform.position - transform.position;
        float dirX = AngleDir(transform.forward, heading, transform.up);
        if (dirX > 0.0f)
        {
            _spriteR.flipX = true;
        }
        else if(dirX < 0.0f)
        {
            _spriteR.flipX = false;
        }
        velocity.x = dirX * movespeed;
       
    }

    //returns -1 when to the left, 1 to the right, and 0 for forward/backward
    public float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0.0f)
        {
            return 1.0f;
        }
        else if (dir < 0.0f)
        {
            return -1.0f;
        }
        else
        {
            return 0.0f;
        }
    }

    private void Update()
    {

        velocity.x = 0;
        velocity.y = gravity * Time.deltaTime;
        stateMachine.Update();
        _controller.Move(velocity * Time.deltaTime);
    }
}
