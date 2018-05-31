using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent (typeof(Controller2D), typeof(DeletePlayerPrefs))]

/*
 * This script handle the behaviour of the player.
 */

public class Player : MonoBehaviour {

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    public float movespeed = 6;
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;

    public Image healthBar;
    public GameObject _healthCanvas;

    float maxHealth = 100f;
    float currentHealth;
    float jumpVelocity;
    float gravity;
    float velocityXSmoothing;

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

    private void Update()
    {
        // If the player stands on something the velocity is not accumulated
        if (_controller.collisions.above || _controller.collisions.bellow)
        {
            velocity.y = 0;
        }

        // Listen to the input of the keyboard
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // If Space is pushed and the player stands on the ground, change the y component of the velocity
        if (Input.GetKeyDown(KeyCode.Space) && _controller.collisions.bellow)
        {
            velocity.y = jumpVelocity;
        }

        // Calculate the Vector3 which will result in the change of position with the call to the method Move()
        float targetVelocityX = input.x * movespeed;
        // Smooth the acceleration of the player with two different variable, one if he is standing on the ground and one if he is in the air
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (_controller.collisions.bellow)?accelerationTimeGrounded:accelerationTimeAirborne);
        // Update the velocity.y based on the player's gravity
        velocity.y += gravity * Time.deltaTime;
        _controller.Move(velocity * Time.deltaTime);

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

    /**
     * Handle the damage system of the player
     * 
     * @damage the amount of damage dealt 
     */
    public void dealDMG(int damage)
    {

        _animator.SetTrigger(Animator.StringToHash("AnimatorAttack"));
        // If  it's te first bruise the player recieve enables the health bar
        if (currentHealth == maxHealth)_healthCanvas.SetActive(true);
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
