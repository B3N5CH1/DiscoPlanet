using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

[RequireComponent(typeof(CanvasGroup), typeof(AIController2D), typeof(CircleCollider2D))]

public class AI : MonoBehaviour {

    public static int AnimatorWalk = Animator.StringToHash("Walk");
    public static int AnimatorAttack = Animator.StringToHash("Attack");

    public Player player;

    public bool meleed = false;
    public bool detected = false;
    public float movespeed = 6;
    public float sightRange = 10;
    public int damage = 2;
    public float meleeRange = 2;
    public float gravity = -100;

    public StateMachine<AI> stateMachine { get; set; }

    public Animator _animator;

    CircleCollider2D _collider;
    AIController2D _controller;
    Vector3 velocity;
    float velocityXSmoothing;

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponentInChildren<CircleCollider2D>();
        _controller = GetComponent<AIController2D>();
    }

    private void Start()
    {
        stateMachine = new StateMachine<AI>(this);

        stateMachine.changeState(IdleState.Instance);

        _collider.radius = sightRange/10;

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
        //return getDistance() < meleeRange;
    }

    public void chase()
    {
        if (detect())
        {
            transform.position += transform.forward * movespeed * Time.deltaTime;
        }
        //velocity.y = gravity * Time.deltaTime;
       // _controller.Move(velocity * Time.deltaTime);
    }

    private void Update()
    {

        transform.LookAt(player.transform);

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        velocity.x = input.x * movespeed;
        velocity.y = gravity * Time.deltaTime;
        chase();
        //stateMachine.Update();
    }
}
