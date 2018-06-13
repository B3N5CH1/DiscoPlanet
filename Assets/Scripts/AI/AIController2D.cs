/**
*   Filename: AIController2D.cs
*   Author: Flückiger, Graf
*   
*   Description:
*       This script controls the behaviour of the AI
*   
**/
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class AIController2D : MonoBehaviour
{
    const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    public LayerMask collisionMask;

    float horizontalRaySpacing;
    float verticalRaySpacing;
    float maxClimbAngle = 50;
    float maxDescentAngle = 50;

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
   * @velocity the vector we use to move our AI (called by reference)
   */
    void VerticalCollision(ref Vector3 velocity)
    {
        // Assign the direction of Y
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y)+skinWidth;

        // Draw the raycast going at the bottom of our object
        for (int i = 0; i < verticalRayCount; i++)
        {
            
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            // If the AI hits an obstacle blocks that way and notice where the collision has occured
            if (hit)
            {
                
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                // If our player is climbing a slope his x velocity is recalculated using trigonometry
                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                    Debug.Log("climb");
                }

                collisions.bellow = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    /** 
    * Handle the horizontal Collisions
    * 
    * @velocity the vector we use to move our AI (called by reference)
    */
    void HorizontalCollision(ref Vector3 velocity)
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

            // If the AI hits an obstacle blocks that way and notice where the collision has occured
            if (hit)
            {
                
                //  Gets the angle between the normal of the hit vector and the vector up
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                // If the slope angle is smaller than our max climb angle we can climb that object
                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    // Smooth the approach of our AI
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
        }
    }

    /**
     * Move the AI
     * 
     * @param velocity the vector used to move the AI
     */
    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        if (velocity.y != 0) VerticalCollision(ref velocity);
        if (velocity.x != 0) HorizontalCollision(ref velocity);

        transform.Translate(velocity);
    }

    // Update the position of the differents raycast of our object
    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * 2);

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
   * @velocity the vector we use to move our AI (called by reference)
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
}
