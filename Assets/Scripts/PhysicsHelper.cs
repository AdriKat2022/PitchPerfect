using UnityEngine;

public static class PhysicsHelper2D
{
    /// <summary>
    /// Launches a Rigidbody2D in a quadratic arc from start to target with a given height.
    /// Automatically adjusts gravity scale based on a speed multiplier.
    /// If height is 0, the path remains quadratic but behaves as a direct trajectory.
    /// </summary>
    /// <param name="rb">The Rigidbody2D to launch.</param>
    /// <param name="startPos">The starting position.</param>
    /// <param name="targetPos">The target position.</param>
    /// <param name="height">The peak height relative to max(start.y, target.y).</param>
    /// <param name="speed">How fast the arc should be performed (1 = 1s, 2 = 0.5s, etc.).</param>
    public static void LaunchRigidbody2D(Rigidbody2D rb, Vector2 start, Vector2 end, Vector2 peak, float speed)
    {
        float duration = 1f / speed;

        // Handle cases where peak is at start or end
        if (peak == start || peak == end)
        {
            Vector2 directVelocity = (end - start) / duration;
            rb.gravityScale = 0; // No gravity needed for direct movement
            rb.linearVelocity = directVelocity;
            return;
        }

        // Calculate initial velocity
        float vx = (end.x - start.x) / duration;
        float vy = (2 * (peak.y - start.y)) / (duration / 2); // Derived from kinematics

        // Calculate required gravity
        float gravity = (-2 * (peak.y - start.y)) / Mathf.Pow(duration / 2, 2);

        // Apply impulse and gravity scale
        rb.gravityScale = gravity / Physics2D.gravity.y;
        rb.linearVelocity = new Vector2(vx, vy);
    }
}
