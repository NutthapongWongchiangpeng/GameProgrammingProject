using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : RaycastController
{
    Player player;
    GameObject[] box;
    PushAble pushAble;
    GameObject[] movingPlatform;
    MovingPlatformController mpController;
    GameObject[] unStalePlatform;
    UnStablePlatformController usController;
    SavePoint savePoint;
    new AudioSource audio;
    AudioClip clip;

    public override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        box = GameObject.FindGameObjectsWithTag("PushAble");
        movingPlatform = GameObject.FindGameObjectsWithTag("MoveAble");
        unStalePlatform = GameObject.FindGameObjectsWithTag("UnStable");
        gameObject.AddComponent<AudioSource>();
        clip = Resources.Load<AudioClip>("Sound/Dead");
        audio = GetComponent<AudioSource>();
    }
    void Update()
    {
        UpdateRaycastOrigins();
        CheckHit();
    }

    void CheckHit()
    {
        Bounds bounds = this.collider_box.bounds;
        float rayLength = bounds.size.x;

        for (int i = 0; i < horizontalRayCount; i++)
        {

            Vector2 rayOrigin = raycastOrigins.bottomLeft;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right, Color.red);

            if (hit)
            {
                audio.PlayOneShot(clip, 0.7F);
                player.GetSavePointPosition();
                player.velocity = Vector2.zero;
                foreach (GameObject b in box)
                {
                    pushAble = b.GetComponent<PushAble>();
                    pushAble.GetSavePointPosition();
                }
                foreach (GameObject m in movingPlatform)
                {
                    mpController = m.GetComponent<MovingPlatformController>();
                    mpController.ResetPlatformMovement();
                    mpController.loadedPlatformMovement();
                }
                foreach (GameObject u in unStalePlatform)
                {
                    usController = u.GetComponent<UnStablePlatformController>();
                    usController.IsThisHide();
                    }
                }
            }
        }
    }
