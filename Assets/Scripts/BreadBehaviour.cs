using UnityEngine;

public class BreadBehaviour : MonoBehaviour
{
    [SerializeField] private BreadType _breadType;
    [SerializeField] private float _height;

    // The speed of the bread of step per seconds (if 2, the bread will take 0.5s to make it accross the straight or curve path).
    public float Speed { get; set; } = 1f;

    private Vector2 _currentVelocity;
    private BreadState _breadState = BreadState.New;

    // In new state, the bread will be falling straight.
    // In flipped and ready state, the bread will taking quadratic curves.
    // In Kicked state, a special animation will play.
    // In Missed state, the bread will fall keeping its last curve (either straight or quadratic curve).

    public void Flip()
    {
        if (_breadState == BreadState.New)
        {
            _breadState = BreadState.Flipped;
        }

        // Calculate the new velocity based on the current velocity.
    }

    public enum BreadType
    {
        Small,
        Medium,
        Large
    }

    public enum BreadState
    {
        New,
        Flipped,
        Ready,
        Kicked,
        Missed
    }
}
