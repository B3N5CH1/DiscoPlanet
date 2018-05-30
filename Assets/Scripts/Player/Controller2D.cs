using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]

public class Controller2D : MonoBehaviour {

    public LayerMask collisionMask, collisionMaskCollectable;

	private int[,] checks = new int[2, 5] { {1, 2, 3, 4, 5}, {0, 0, 0, 0, 0} };

    const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float maxClimbAngle = 50;
    float maxDescentAngle = 50;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D collider;
    RaycastOrigins raycastOrigins;
    public CollisionInfo collisions;

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    // Handle the vertical Collisions
    private void VerticalCollision(ref Vector3 velocity)
    {

        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth; 

        // Draw the raycast going at the bottom of our object
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength,Color.red);

            // If the player hits an obstacle blocks that way and notice where the collision has occured
            if (hit)
            {
                velocity.y = (hit.distance-skinWidth) * directionY;
                rayLength = hit.distance;

                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }
                        
                collisions.bellow = directionY == -1;
                collisions.above = directionY == 1;
            }

            // Collision with checkpoint mask

			RaycastHit2D hitColl = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMaskCollectable);

            if (hitColl) {
				collectableCheck (hitColl);
            }

        }
    }

    // Handle the Horizontal Collisions
    private void HorizontalCollision(ref Vector3 velocity)
    {

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

                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    float distanceToSlope = 0;
                    if(slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlope = hit.distance - skinWidth;
                        velocity.x -= distanceToSlope * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlope * directionX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if(collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }

			RaycastHit2D hitColl = Physics2D.Raycast(rayOrigin, Vector2.up * directionX, rayLength, collisionMaskCollectable);

			if (hitColl) {
				collectableCheck (hitColl);
			}

        }
    }

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
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle*Mathf.Deg2Rad)*Mathf.Abs(velocity.x))
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
    // Move the player
    public void Move(Vector3 velocity)
    {

        UpdateRaycastOrigins();
        collisions.Reset();

        if (velocity.y < 0) DescentSlope(ref velocity);
        if (velocity.x != 0) HorizontalCollision(ref velocity);
        if (velocity.y != 0) VerticalCollision(ref velocity);

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

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

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



	public void collectableCheck(RaycastHit2D hit) {
		Inventory inv = new Inventory();

		string item = hit.collider.name;

        switch (item)
        {
            case "Shiny Rock":
                if (Input.GetKey(KeyCode.E))
                {
                    if (checks[1, 0] == 0)
                    {
                        print("[1, 0] == 0");
                        if (inv.dialog(item))
                        {
                            print(item + " should have been added");
                            GameObject.Find("Shiny Rock").SetActive(false);


                            checks[1, 0] = 1;
                        }
                    }
                }
                break;
            case "Chest":
                if (Input.GetKey(KeyCode.E))
                {
                    if (checks[1, 1] == 0)
                    {
                        if (inv.dialog(item))
                        {
                            print(item + " should have been added");
                            GameObject.Find("Chest").SetActive(false);

                            showBubble("I found a key! That should unlock a door somewhere.");

                            checks[1, 1] = 1;
                        }
                    }
                }
                break;
            case "Door":
                inv.addItem("Chest");
                if (Input.GetKey(KeyCode.E))
                {
                    if (inv.checkItem("Chest") == 1)
                    {
                        showBubble("Lucky me - that was the right key.");

                        GameObject.Find("Door").SetActive(false);
                        GameObject.Find("1-1f").SetActive(false);
                    } else
                    {
                        showBubble("This door seems locked. Maybe there is a key somewhere?");
                    }
                }
                break;
            case "Light Graviton Collector":
                if (checks[1, 2] == 0)
                {
                    if (inv.dialog(item))
                    {
                        checks[1, 2] = 1;
                    }
                }
                break;
            case "Light Gravitons":
                if (checks[1, 3] == 0)
                {
                    if (inv.dialog(item))
                    {
                        checks[1, 3] = 1;
                    }
                }
                break;
            case "Last Item":
                if (checks[1, 4] == 0)
                {
                    if (inv.dialog(item))
                    {
                        checks[1, 4] = 1;
                    }
                }
                break;


            default:
			break;
		}
	}

    private void showBubble(string msg)
    {
        DialogBubble dialogBubble = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogBubble>();

        AssemblyCSharp.PixelBubble message = new AssemblyCSharp.PixelBubble();
        message.vMessage = msg;

        dialogBubble.vBubble.Add(message);

        dialogBubble.ShowBubble(dialogBubble);

        dialogBubble.vBubble.Clear();

    }





}
