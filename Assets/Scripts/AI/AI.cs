using UnityEngine;
using StateMachine;

[RequireComponent(typeof(CanvasGroup), typeof(AIController2D))]

public class AI : MonoBehaviour{

    public Player player;

    public float movespeed = 4;
    public float sightRange = 10;
    public int damage = 2;
    public float meleeRange = 5;

    public StateMachine<AI> stateMachine { get; set; }

    public Animator _animator;

    float gravity = -20;
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
        // If the player stands on something the velocity is not accumulated
        if (_controller.collisions.above || _controller.collisions.bellow)
        {
            velocity.y = 0;
        }

        velocity.x = 0;
        velocity.y += 10 * gravity * Time.deltaTime;
        stateMachine.Update();
        _controller.Move(velocity * Time.deltaTime);
    }
}
