using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnStablePlatformController : RaycastController
{
    public float timeToHide;
    public float timeToShow;
    [HideInInspector]public bool isHide;
    [HideInInspector]public bool saveIsHide;

    public override void Start()
    {
        base.Start();
        isHide = false;
    }
    void Update()
    {
        UpdateRaycastOrigins();
        HitUnstable();
    }
    public void SaveIsHide() {
        saveIsHide = isHide;
    }
    public void IsThisHide() {
        if(saveIsHide)
            this.transform.gameObject.SetActive(false);
        else
            this.transform.gameObject.SetActive(true);
    }

    void HitUnstable()
    {
        float rayLength = skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {

            Vector2 rayOrigin = raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, collisionMask);

            //Debug.DrawRay(rayOrigin, Vector2.up, Color.red);

            if (hit)
            {
                Invoke("HidePlatform", timeToHide);
                Invoke("ShowPlatform", timeToShow);
            }
        }
    }

    void HidePlatform()
    {
        isHide = true;
        this.transform.gameObject.SetActive(false);
    }

    public void ShowPlatform()
    {
        this.transform.gameObject.SetActive(true);
        isHide = false;
    }

}