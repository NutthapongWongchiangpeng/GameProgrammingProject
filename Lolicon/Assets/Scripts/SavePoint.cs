using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : RaycastController
{

    Player player;
    GameObject[] box;
    PushAble pushAble;
    GameObject[] movePlatform;
    MovingPlatformController mpController;
    GameObject[] unStalePlatform;
    UnStablePlatformController usController;


    public Vector3 save_point;
    bool saved;
    public override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        box = GameObject.FindGameObjectsWithTag("PushAble");
        movePlatform = GameObject.FindGameObjectsWithTag("MoveAble");
        unStalePlatform = GameObject.FindGameObjectsWithTag("UnStable");

        save_point = player.transform.position;
        saved = false;
    }
    void Update()
    {
        UpdateRaycastOrigins();
        HitSavePoint();
    }
    void HitSavePoint()
    {

        float rayLength = collider_box.bounds.size.y;
        Vector2 rayOrigin = raycastOrigins.midBot;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, collisionMask);

        //Debug.DrawRay(rayOrigin, Vector2.up, Color.red);
        if (hit && !saved)
        {
            player.savePoint = this.transform.position;
            foreach (GameObject b in box)
            {
                pushAble = b.GetComponent<PushAble>();
                pushAble.savePoint = pushAble.transform.position;
            }
            foreach (GameObject m in movePlatform) {
                mpController = m.GetComponent<MovingPlatformController>();
                mpController.SavedPlatformMovement();

            }
            foreach (GameObject u in unStalePlatform)
            {
                usController = u.GetComponent<UnStablePlatformController>();
                usController.SaveIsHide();
            }
            saved = true;
        }
    }
}

