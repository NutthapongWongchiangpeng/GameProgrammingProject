using UnityEngine;
using System.Collections;


public class RaycastController : MonoBehaviour
{

    public LayerMask collisionMask;
    public const float skinWidth = .0001f;
    const float dstBetweenRays = .1f;
    [HideInInspector]
    public int horizontalRayCount;
    [HideInInspector]
    public int verticalRayCount;

    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;

    [HideInInspector]
    public BoxCollider2D collider_box;
    public RaycastOrigins raycastOrigins;

    public virtual void Awake()
    {
        collider_box = GetComponent<BoxCollider2D>();
    }

    public virtual void Start()
    {
        CalculateRaySpacing();
    }

    public void UpdateRaycastOrigins()
    {
        Bounds bounds = collider_box.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        raycastOrigins.midLeft = new Vector2(bounds.min.x, (bounds.max.y + bounds.min.y) / 2);
        raycastOrigins.midRight = new Vector2(bounds.max.x, (bounds.max.y + bounds.min.y) / 2);
        raycastOrigins.midTop = new Vector2((bounds.max.x + bounds.min.x) / 2, bounds.max.y);
        raycastOrigins.midBot = new Vector2((bounds.max.x + bounds.min.x) / 2, bounds.min.y);
        raycastOrigins.center = bounds.center;
    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = collider_box.bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
        public Vector2 midLeft, midRight, midTop, midBot;
        public Vector2 center;
    }
}
