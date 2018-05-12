using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAble : RaycastController
{

    Controller2D controller;
    Player player;
    Vector3 velocity;
    Vector2 directionalInput;

    private float velocityXSmoothing;
    private float accelerationTimeAirborne = .2f;
    private float accelerationTimeGrounded = .1f;

    public float pushSpeed = 3;
    private float originPushSpeed;

    float gravity = -50f;
    private bool pushBlock = false;
    int hitDirX;

    [HideInInspector]
    public Vector2 savePoint;

    public override void Start()
    {
        base.Start();
        controller = GetComponent<Controller2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        originPushSpeed = pushSpeed;
        velocityXSmoothing = player.velocityXSmoothing;
        savePoint = this.transform.position;
    }

    void Update()
    {
        UpdateRaycastOrigins();
        CheckHit();
        CalculateVelocity();
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, directionalInput);
        if (controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else {
                velocity.y = 0;
            }
        }
    }

    void CheckHit()
    {
        float rayLength = skinWidth + 0.1f;

        RaycastHit2D hitTopLeft = Physics2D.Raycast(raycastOrigins.topLeft, Vector2.left, rayLength, collisionMask);
        RaycastHit2D hitTopRight = Physics2D.Raycast(raycastOrigins.topRight, Vector2.right, rayLength, collisionMask);
        RaycastHit2D hitBotLeft = Physics2D.Raycast(raycastOrigins.bottomLeft, Vector2.left, rayLength, collisionMask);
        RaycastHit2D hitBotRight = Physics2D.Raycast(raycastOrigins.bottomRight, Vector2.right, rayLength, collisionMask);
        RaycastHit2D hitMidLeft = Physics2D.Raycast(raycastOrigins.midLeft, Vector2.left, rayLength, collisionMask);
        RaycastHit2D hitMidRight = Physics2D.Raycast(raycastOrigins.midRight, Vector2.right, rayLength, collisionMask);


        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayLeft = raycastOrigins.bottomLeft;
            Vector2 rayRight = raycastOrigins.bottomRight;
            rayLeft += Vector2.up * (horizontalRaySpacing * i);
            rayRight += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hitLeft = Physics2D.Raycast(rayLeft, Vector2.left, rayLength, collisionMask);
            RaycastHit2D hitRight = Physics2D.Raycast(rayRight, Vector2.right, rayLength, collisionMask);

            //Debug.DrawRay(rayLeft, Vector2.left, Color.red);
            //Debug.DrawRay(rayRight, Vector2.right, Color.red);

            if (hitTopLeft || hitTopRight || hitBotLeft || hitBotRight || hitMidLeft || hitMidRight)
            {
                if (hitLeft || hitRight)
                {
                    RaycastHit2D hit = (hitLeft) ? hitLeft : hitRight;
                    if (hit.collider.tag == "Player" && (Input.GetKeyDown(KeyCode.Z)|| Input.GetKeyDown(KeyCode.Joystick1Button1)))
                    {
                        pushBlock = true;
                    }
                    if (hit.collider.tag == "Player" && Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.Joystick1Button1))
                    {
                        pushBlock = false;
                    }
                }
            }
            else
                pushBlock = false;
        }
    }

    void CalculateVelocity()
    {

        if (pushBlock)
        {
            hitDirX = 0;
            if (controller.collisions.right)
                hitDirX = 1;
            if (controller.collisions.left)
                hitDirX = -1;
            pushSpeed = originPushSpeed;
            player.moveSpeed = player.originMoveSpeed;

            directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (hitDirX == directionalInput.x)
            {
                pushSpeed = player.originMoveSpeed;
                player.moveSpeed = originPushSpeed;
            }
        }
        else {
            pushSpeed = originPushSpeed;
            player.moveSpeed = player.originMoveSpeed;
            directionalInput.x = 0;
        }
        float targetVelocityX = directionalInput.x * pushSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
    }
    public void GetSavePointPosition()
    {
        this.transform.position = savePoint;
    }
}