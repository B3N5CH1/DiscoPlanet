using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]

/*
 * This script handle controlls the behaviour of the player.
 */

public class Controller2D : MonoBehaviour
{

    public LayerMask collisionMask, collisionMaskCollectable;

    private int[,] checks = new int[2, 5] { { 1, 2, 3, 4, 5 }, { 0, 0, 0, 0, 0 } };

    const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float maxClimbAngle = 50;
    float maxDescentAngle = 50;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    bool isJumping;
    bool isFalling;
    BoxCollider2D collider;
    RaycastOrigins raycastOrigins;
    public CollisionInfo collisions;

    // Called when the scene is loaded, instanciate a few gameojbect and calculate the spacing of the raycast on our player
    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    /** 
     * Handle the vertical Collisions
     * 
     * @velocity the vector we use to move our player (called by reference)
     */
    private void VerticalCollision(ref Vector3 velocity)
    {

        // Assign the direction of Y
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        // Draw the raycast going at the bottom of our object
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            // If the player hits an obstacle blocks that way and notice where the collision has occured
            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                // If our player is climbing a slope his x velocity is recalculated using trigonometry
                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                collisions.bellow = directionY == -1;
                collisions.above = directionY == 1;
                isFalling = false;
            }
            if (collisions.bellow == false && directionY == 1)
            {
                isJumping = true;
                isFalling = false;
            }
            else if (collisions.bellow == false && directionY == -1)
            {
                isJumping = false;
                isFalling = true;
            }
            else
            {
                isJumping = false;
                isFalling = false;
            }
            // Collision with checkpoint mask

            RaycastHit2D hitColl = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMaskCollectable);

            if (hitColl)
            {
                collectableCheck(hitColl);
            }

        }
    }

    /** 
     * Handle the horizontal Collisions
     * 
     * @velocity the vector we use to move our player (called by reference)
     */
    private void HorizontalCollision(ref Vector3 velocity)
    {
        // Assign the direction of Y
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        // Draw the raycast going at the bottom of our object
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            // If the player hits an obstacle blocks that way and notice where the collision has occured
            if (hit)
            {
                //  Gets the angle between the normal of the hit vector and the vector up
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                // If the slope angle is smaller than our max climb angle we can climb that object
                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    // Smooth the approach of our player
                    float distanceToSlope = 0;
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlope = hit.distance - skinWidth;
                        velocity.x -= distanceToSlope * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlope * directionX;
                }

                // Notify where the collision(s) occur, and calculate a new component x for the velocity
                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }

            RaycastHit2D hitColl = Physics2D.Raycast(rayOrigin, Vector2.up * directionX, rayLength, collisionMaskCollectable);

            if (hitColl)
            {
                collectableCheck(hitColl);
            }

        }
    }

    /**
     * Calculate new value for the components x,y of the vector velocity when climbing a slope.
     * 
     * @velocity the vector we use to move our player (called by reference)
     * @slopeAngle the angle of the slope
    */
    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {

        // Trigonometry :
        //              y = moveDistance*sin(slopeAngle)
        //              x = moveDistance*cos(slopeAngle)
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.bellow = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    /** 
     * Calculate new value for the components x,y of the vector velocity when descending a slope.
     * 
     * @velocity the vector we use to move our player (called by reference)
     * @slopeAngle the angle of the slope
    */
    void DescentSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescentAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descentVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descentVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.bellow = true;
                    }
                }
            }
        }
    }

    /**
     * Move the player
     * 
     * @param velocity the vector used to move the player
     */
    public void Move(Vector3 velocity, Animator animator)
    {

        UpdateRaycastOrigins();
        collisions.Reset();

        if (velocity.y < 0) DescentSlope(ref velocity);
        if (velocity.x != 0) HorizontalCollision(ref velocity);
        if (velocity.y != 0) VerticalCollision(ref velocity);

        if (velocity.x > -0.001 && velocity.x < 0.001 && !isJumping && !isFalling)
        {
            animator.SetBool(Animator.StringToHash("Walks"), false);
        }
        else if (isJumping)
        {
            animator.SetBool(Animator.StringToHash("Jumps"), true);
        }
        else if (isFalling)
        {
            animator.SetBool(Animator.StringToHash("Walks"), false);
            animator.SetBool(Animator.StringToHash("Jumps"), false);
            animator.SetBool(Animator.StringToHash("Idle"), false);
            animator.SetBool(Animator.StringToHash("Falls"), true);
        }
        else
        {
            animator.SetBool(Animator.StringToHash("Jumps"), false);
            animator.SetBool(Animator.StringToHash("Falls"), false);
            animator.SetBool(Animator.StringToHash("Idle"), false);
            animator.SetBool(Animator.StringToHash("Walks"), true);
        }
        transform.Translate(velocity);
    }

    // Update the position of the differents raycast of our object
    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

    }

    // Calculate the spacing between two raycast
    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.y / (verticalRayCount - 1);
    }

    // A struct used to create and handle the raycast on our player
    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    // A struct which stores where is the collisions occuring
    // with a Reset method
    public struct CollisionInfo
    {
        public bool above, bellow;
        public bool left, right;
        public bool climbingSlope, descendingSlope;
        public float slopeAngle, slopeAngleOld;

        public void Reset()
        {
            above = bellow = false;
            left = right = false;
            climbingSlope = descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }



    public void collectableCheck(RaycastHit2D hit)
    {
        Inventory inv = new Inventory();

        string item = hit.collider.name;

        switch (item)
        {
            case "Shiny Rock":
                if (Input.GetKey(KeyCode.E))
                {
                    if (inv.addItem(item))
                    {
                        showTextBubble("That looks like a nice shiny rock! It looks valuable, I'll take that.");
                        GameObject.Find(item).SetActive(false);
                        checks[1, 0] = 1;
                    }
                }
                break;
            case "Chest":
                if (Input.GetKey(KeyCode.E))
                {
                    if (inv.addItem(item))
                    {
                        GameObject.Find("Chest").SetActive(false);
                        showTextBubble("Of all the possible treasures, I found a key... Well at least it should unlock something somewhere.");
                        checks[1, 1] = 1;
                    }
                }
                break;
            case "Door":
                if (Input.GetKey(KeyCode.E))
                {
                    if (checks[1, 1] == 1)
                    {
                        showTextBubble("Lucky me - the key I found was for this door.");
                        GameObject.Find("Door").SetActive(false);
                        GameObject.Find("1-1f").SetActive(false);
                    }
                    else
                    {
                        showTextBubble("This door seems locked. Maybe there is a key somewhere?");
                    }
                }
                break;
            case "Light Graviton Collector":
                showTextBubble("This looks like a Light Graviton Collector!");
                if (Input.GetKey(KeyCode.E))
                {
                    if (inv.addItem(item))
                    {
                        showTextBubble("Thanks for that! Now I have to find a place, where I can collect some.");
                        GameObject.Find("Light Graviton Collector").GetComponent<SlimeSpawner>().activateSlime();
                        GameObject.Find("Light Graviton Collector").SetActive(false);
                        checks[1, 2] = 1;
                    }
                }
                break;
            case "Teleporter":
                if (Input.GetKey(KeyCode.E))
                {
                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level1"))
                    {
                        inv.addItem("Shiny Rock");
                        checks[1, 0] = 1;
                        if (checks[1, 0] == 1)
                        {
                            showTextBubble("As it looks like, that shiny rock was the special gem, which is used for the teleporter.");
                            if (GameObject.Find("TPPanel (inactive)"))
                                GameObject.Find("TPPanel (inactive)").SetActive(false);
                            SceneManager.UnloadSceneAsync("Level1");
                            SceneManager.LoadScene("Level2");
                        }
                        else
                        {
                            showTextBubble("It seems not functional. There is a slot for a gem which seems important to focus the laser.");
                        }
                    }
                    else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level2"))
                    {
                        if (Input.GetKey(KeyCode.E))
                        {
                            if (checks[1, 3] == 0)
                            {
                                showTextBubble("Without any Light Gravitons it makes no sense to try to go back. I need them first");
                            }
                            else
                            {
                                SceneManager.UnloadSceneAsync("Level2");
                                SceneManager.LoadScene("Level1");
                                GameObject.Find("Player").transform.Translate(new Vector3(288.5f, -74.7f));
                            }
                        }
                    }
                }
                break;
            case "Light Gravitons":
                if (Input.GetKey(KeyCode.E))
                {
                    if (checks[1, 3] == 0)
                    {
                        if (inv.addItem(item))
                        {
                            showTextBubble("Perfect, that's exactly what I need, now I can go ba - wait, do I smell Ice Cream?!");
                            GameObject.Find("Light Gravitons").GetComponent<IceCreamSpawner>().activateIceCream();
                            GameObject.Find("Light Gravitons").SetActive(false);
                            checks[1, 3] = 1;
                        }
                    }
                }
                break;
            case "Ice Cream":
                if (checks[1, 4] == 0)
                {
                    if (inv.addItem(item))
                    {
                        showTextBubble("Why would there be Ice Cream here? Anyway, that's mine now");
                        GameObject.Find("Ice Cream").GetComponent<SlimeSpawner>().activateSlime();
                        GameObject.Find("Ice Cream").SetActive(false);
                        checks[1, 4] = 1;
                    }
                }
                break;


            default:
                break;
        }
    }

    public void showTextBubble(string msg)
    {
        DialogBubble dialogBubble = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<DialogBubble>();

        AssemblyCSharp.PixelBubble message = new AssemblyCSharp.PixelBubble();
        message.vMessage = msg;

        dialogBubble.vBubble.Add(message);

        dialogBubble.ShowBubble(dialogBubble);

        dialogBubble.vBubble.Clear();

    }


}
