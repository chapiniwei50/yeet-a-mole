using UnityEngine;

public class PlayerBoundary : MonoBehaviour
{
    [Header("Settings")]
    public Transform centerPoint; // Scene center point
    public float maxRadius = 1.5f; // 1.5 meter radius (slightly forgiving)

    void Update()
    {
        if (centerPoint == null) return;

        // Calculate the player's current horizontal distance from the center (ignoring the Y-axis/height)
        Vector3 playerPos = transform.position;
        Vector3 centerPos = centerPoint.position;
        float distance = Vector2.Distance(new Vector2(playerPos.x, playerPos.z), new Vector2(centerPos.x, centerPos.z));

        if (distance > maxRadius)
        {
            // Simple penalty: forcibly push back to the edge
            Vector3 dir = (playerPos - centerPos).normalized;
            Vector3 constrainedPos = centerPos + dir * maxRadius;
            transform.position = new Vector3(constrainedPos.x, playerPos.y, constrainedPos.z);
            
            // Alpha debug message
            Debug.LogWarning("Boundary Limit: You cannot walk outside the circle!");
        }
    }
    
    // Draw the circle in the editor for visualization
    void OnDrawGizmos()
    {
        if (centerPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(centerPoint.position, maxRadius);
        }
    }
}