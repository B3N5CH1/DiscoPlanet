using UnityEngine;
using StateMachine;

[RequireComponent(typeof(CanvasGroup), typeof(AIController2D))]

/*
 * This script handle the behaviour of the AI.
 */

public class AI : MonoBehaviour
{

    public Player player;

    public float movespeed = 4;
    public float sightRange = 10;
    public int damage = 2;
    public float meleeRange = 5;
    public float soundToPlay = -1.0f;

    public StateMachine<AI> stateMachine { get; set; }

    public Animator _animator;

    AudioSource _audio;
    float gravity = -20;
    SpriteRenderer _spriteR;
    AIController2D _controller;
    Vector3 velocity;

    // Instanciate a few gameojbect
    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _animator = GetComponentInChildren<Animator>();
        _controller = GetComponent<AIController2D>();
        _spriteR = GetComponent<SpriteRenderer>();
    }

    // Called when the scene is loaded, instanciate a few gameojbect
    private void Start()
    {
        stateMachine = new StateMachine<AI>(this);

        stateMachine.changeState(IdleState.Instance);

    }

    // Attack the player
    public void attack()
    {

        player.dealDMG(damage);
    }

    /**
     * If the player is within sight range returns true
     * 
     * @return 
     */
    public bool detect()
    {

        return getDistance() <= sightRange;
    }

    /**
     * Get the distance between this AI and the player
     * 
     * @return
     */
    private float getDistance()
    {

        return Vector3.Distance(transform.position, player.transform.position);
    }

    /**
     * If the player is within melee range returns true
     *
     * @return
     */
    public bool melee()
    {
        return getDistance() <= meleeRange;
    }

    /**
     * Calculate the x component of the AI, based on the player's position.
     * Flip the sprite to always face the player (left,right).
     * 
     */
    public void chase()
    {
        Vector3 heading = player.transform.position - transform.position;
        float dirX = AngleDir(transform.forward, heading, transform.up);
        if (dirX > 0.0f)
        {
            _spriteR.flipX = true;
        }
        else if (dirX < 0.0f)
        {
            _spriteR.flipX = false;
        }
        velocity.x = dirX * movespeed;

    }

    /**
     * Calculate the cross product between the forward vector of our AI and the one from the player.
     * Then process a dot product on the newly calculated vector and the vector up from our AI.
     * Finally we read compare the result with 0, and get the direction.
     * 
     * @param fwd
     * @param targetDir
     * @param up
     * 
     * @return -1 when to the left, 1 to the right, and 0 for forward/backward
     */
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

    // Update method called once per frame
    private void Update()
    {
        // If the player stands on something the velocity is not accumulated
        if (_controller.collisions.above || _controller.collisions.bellow)
        {
            velocity.y = 0;
        }

        velocity.x = 0;
        velocity.y += 10 * gravity * Time.deltaTime;
        // Update the state machine
        stateMachine.Update();
        // Move the AI with the newly calculated vector velocity
        _controller.Move(velocity * Time.deltaTime);
    }

    // Play the audio attached to this gameobject
    void PlaySound()
    {
        _audio.Play();
    }
}