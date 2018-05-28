using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using System;

[RequireComponent(typeof(CanvasGroup))]

public class AI : MonoBehaviour {

    public static int AnimatorWalk = Animator.StringToHash("Walk");
    public static int AnimatorAttack = Animator.StringToHash("Attack");

    public bool meleed = false;
    public bool detected = false;
    public int speed;
    public int sightRange;
    public int damage;

    public StateMachine<AI> stateMachine { get; set; }

    public Animator _animator;
    CircleCollider2D _collider;

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponentInChildren<CircleCollider2D>();
    }

    private void Start()
    {
        stateMachine = new StateMachine<AI>(this);

        stateMachine.changeState(IdleState.Instance);

        _collider.radius = sightRange/10;

        //StartCoroutine(Animate());
    }

    public void attack()
    {

        _animator.SetTrigger(AnimatorAttack);
    }

    public bool detect()
    {

        return detected;
    }

    public bool melee()
    {

        return meleed;
    }

    internal void chase()
    {
        
    }

    private void Update()
    {
        /*if (Time.time > gameTimer + 1)
        {
            gameTimer = Time.time;
            seconds++;
            Debug.Log(seconds);
        }

        if (seconds == 5)
        {
            seconds = 0;
            switchState = !switchState;
        }*/

        stateMachine.Update();

    }

    IEnumerator Animate()
    {
        yield return new WaitForSeconds(5f);
    }
}
