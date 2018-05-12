using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : RaycastController
{
    public GameObject targetObject;
    private MovingPlatformController mpController;
    //private UnStablePlatformController usController;
    public bool ActiveOnly;

    public override void Start()
    {
        TargetOject();
        base.Start();
    }
    void FixedUpdate()
    {
        UpdateRaycastOrigins();
        HitSwitch();
    }
    void HitSwitch()
    {
        float rayLength = skinWidth + 0.1f;
        Vector2 rayTopLeft = raycastOrigins.topLeft;
        Vector2 rayMidTop = raycastOrigins.midTop;
        Vector2 rayTopRight = raycastOrigins.topRight;

        RaycastHit2D hitTopLeft = Physics2D.Raycast(rayTopLeft, Vector2.up, rayLength, collisionMask);
        RaycastHit2D hitMidTop = Physics2D.Raycast(rayMidTop, Vector2.up, rayLength, collisionMask);
        RaycastHit2D hitTopRight = Physics2D.Raycast(rayTopRight, Vector2.up, rayLength, collisionMask);

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, collisionMask);

            if (hitTopLeft || hitMidTop || hitTopRight)
            {
                if (hit)
                {
                    switch (targetObject.tag)
                    {
                        case "MoveAble":
                            mpController.switchFunction = true;
                            break;
                        case "PassAble":
                            targetObject.tag = "Untagged";
                            break;
                    }
                }
            }
            else if (!ActiveOnly)
            {
                switch (targetObject.tag)
                {
                    case "MoveAble":
                        mpController.switchFunction = false;
                        break;
                }
            }
        }
    }

    void TargetOject()
    {
        switch (targetObject.tag)
        {

            case "MoveAble":
                mpController = targetObject.GetComponent<MovingPlatformController>();
                mpController.startFunction = false;
                break;
            case "PassAble":
                break;
        }
    }
}
