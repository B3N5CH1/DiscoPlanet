using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Controller2D), typeof(DeletePlayerPrefs))]

/*
 * This script handle the behaviour of the player.
 */

public class Player : MonoBehaviour
{

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    public float movespeed = 6;
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;

    // The left and right limits of the maps
    // Used to teleport the player to the other end to create the illusion of a round/looped map
    private float l1Left = -124f;
    private float l1Right = 764f;
    private float l2Left = 29f;
    private float l2Right = 532f;

    public Image healthBar;
    public GameObject _healthCanvas;
    public GameObject _gameMenu;

    float maxHealth = 100f;
    float currentHealth;
    float jumpVelocity;
    float gravity;
    float velocityXSmoothing;
    bool inMenu = false;

    SpriteRenderer _spriteR;
    DeletePlayerPrefs _del;
    Vector3 velocity;
    Animator _animator;
    Controller2D _controller;

    // Called when the scene is loaded, instanciate a few gameojbect
    private void Start()
    {
        _controller = GetComponent<Controller2D>();
        _animator = GetComponentInChildren<Animator>();
        _del = GetComponent<DeletePlayerPrefs>();
        _spriteR = GetComponentInChildren<SpriteRenderer>();

        // Reset health on scene start
        maxHealth = 100f;
        currentHealth = maxHealth;

        // Calculate once the gravity and the jump velocity using those two physics formula :
        // dx = v*t + (a*t^2)/2
        // vf = v + a*t
        gravity = -(2 * jumpHeight / Mathf.Pow(timeToJumpApex, 2));
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity " + gravity + " jumpVelocity " + jumpVelocity);
    }

    /**
     * @return gravity The gravity used by the player
     */
    public float getGravity()
    {
        return gravity;
    }

    public void setInMenu(bool inMenu)
    {
        this.inMenu = inMenu;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _gameMenu.SetActive(true);
            inMenu = true;
        }
        // If the player stands on something the velocity is not accumulated
        if (_controller.collisions.above || _controller.collisions.bellow)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(0, 0);
        // Listen to the input of the keyboard, bléock the inputs if in the menu
        if (!inMenu)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            // If Space is pushed and the player stands on the ground, change the y component of the velocity
            if (Input.GetKeyDown(KeyCode.Space) && _controller.collisions.bellow)
            {
                velocity.y = jumpVelocity;
            }
        }
        // Calculate the Vector3 which will result in the change of position with the call to the method Move()
        float targetVelocityX = input.x * movespeed;
        // flip the player sprite
        if (input.x < 0.0f)
        {
            _spriteR.flipX = true;
        }
        else if (input.x > 0.0f)
        {
            _spriteR.flipX = false;
        }
        // Smooth the acceleration of the player with two different variable, one if he is standing on the ground and one if he is in the air
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (_controller.collisions.bellow) ? accelerationTimeGrounded : accelerationTimeAirborne);
        // Update the velocity.y based on the player's gravity
        velocity.y += gravity * Time.deltaTime;
        _controller.Move(velocity * Time.deltaTime, _animator);


        float currX = this.transform.position.x;
        float currY = this.transform.position.y;
        
        // To check which scene is active and change certain values accordingly. Also used to teleport the player properly
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level1"))
        {
            // If the player is in the map limit, he gets teleportad to the other end
            if (currX >= l1Right && currX <= l1Right+1)
            {
                this.transform.Translate(new Vector3(-883f, 0f));
            }
            else if (currX <= l1Left+1 && currX >= l1Left)
            {
                this.transform.Translate(new Vector3(883f, 0f));
            }
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level2"))
        {
            // In level 2 the player also gets some jumping modification based on his height on the map
            // (The higher he is, the higher and longer he can jump simulating a decrease of gravity)
            this.jumpHeight = (float)(7 + (3.5 * (Mathf.Floor(currY / 32))));
            this.timeToJumpApex = (float)(0.4 + (0.15 * (Mathf.Floor(currY / 32))));

            // Same as in the first part, if he's near the limit, he gets teleported to the other end.
            if (currX >= l2Left && currX <= l2Left+1)
            {
                this.transform.Translate(new Vector3(500f, 0f));
            }
            else if (currX >= l2Right && currX <= l2Right+1)
            {
                this.transform.Translate(new Vector3(-500f, 0f));
            }

        }
    }

    /**
     * Handle the damage system of the player
     * 
     * @damage the amount of damage dealt 
     */
    public void dealDMG(int damage)
    {

        _animator.SetTrigger(Animator.StringToHash("AnimatorAttack"));
        // If  it's te first bruise the player recieve enables the health bar
        if (currentHealth == maxHealth) _healthCanvas.SetActive(true);
        // Deduct the damage dealt to the player's health
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / 100;
        // If the player is out of health, dies
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Used when the player is suppposed to die, return to MainMenu scene and deletes player preferences about his progression
    public void Die()
    {
        currentHealth = 0;
        _del.DeleteAll();
        SceneManager.LoadScene(1);
    }
}
