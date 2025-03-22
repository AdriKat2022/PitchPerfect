using System;
using UnityEngine;

public class BreadBehaviour : MonoBehaviour
{
    [SerializeField] private ScoreTextNotification _scoreTextNotification;
    [SerializeField] private BreadType _breadType;
    [SerializeField] private float _height; // How much the bread will go up when flipped (purely cosmetic).
    [Range(1, 4)]
    [SerializeField] private int _requiredFlips = 1;

    // The speed of the bread of step per seconds (if 2, the bread will take 0.5s to make it accross the straight or curve path).
    public float Speed { get; set; } = 1f;

    private bool _intialized = false;
    private int _currentFlip = 0;
    private float _originalXPosition;
    private float _ovenXPosition;
    private float _lineYPosition;
    private RhythmAction _rhythmAction;
    private Vector2[] _points;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Initialize(RhythmAction rhythmAction, Vector2 startPos, float ovenXPos, float lineYPos, float speed = 1f, float heightMult = 1f)
    {
        _rhythmAction = rhythmAction;
        transform.position = startPos;
        _originalXPosition = startPos.x;
        _ovenXPosition = ovenXPos;
        _height *= heightMult;
        Speed = speed;
        _lineYPosition = lineYPos;
        _intialized = true;

        InitializePoints();

        PhysicsHelper2D.LaunchRigidbody2D(_rigidbody, startPos, _points[0], startPos, Speed);
    }

    private void InitializePoints()
    {
        // Initialize the points for the quadratic curves according to the required flips.
        _points = new Vector2[_requiredFlips + 1];
        _points[0] = new(_originalXPosition, _lineYPosition);
        _points[^1] = new(_ovenXPosition, _lineYPosition);
        for (int i = 1; i < _requiredFlips; i++)
        {
            float x = _originalXPosition + ((_ovenXPosition - _originalXPosition) / _requiredFlips) * i;
            _points[i] = new(x, _lineYPosition);
        }
    }

    public void Flip(RewardType rewardType)
    {
        if (_currentFlip < _requiredFlips)
        {
            _scoreTextNotification.MakeNotification(rewardType);

            transform.position = _points[_currentFlip];

            // Adujst the speed based on the signed beat distance from the rhythm
            float beatDistance = RhythmCore.GetBeatDistance();
            float speedMultiplier = Mathf.Clamp(1 + beatDistance, 0.5f, 1.5f);

            PhysicsHelper2D.LaunchRigidbody2D(_rigidbody, _points[_currentFlip], _points[_currentFlip + 1], GetMidPointWithHeight(_points[_currentFlip], _points[_currentFlip + 1]), Speed * speedMultiplier);
            _currentFlip++;
        }
        else
        {
            // If the bread is already flipped the required number of times, it should have been kicked.
        }
    }

    private Vector2 GetMidPointWithHeight(Vector2 vector21, Vector2 vector22)
    {
        return new Vector2((vector21.x + vector22.x) / 2, _height);
    }

    public void Kick(float force, RewardType rewardType)
    {
        // TODO: Camera shake
        // TODO: if required flips are not reached ignore it.

        if (_currentFlip < _requiredFlips) return;

        // TODO: quick reward animation
    }

    private void FixedUpdate()
    {
        if (!_intialized)
        {
            return;
        }

        if (transform.position.y < _rhythmAction.transform.position.y - 2)
        {
            // If the bread is missed, destroy it.
            _rhythmAction.UnregisterBread(this);

            // TODO: Miss animation
            _scoreTextNotification.MakeNotification(RewardType.None);
            // Stop the bread from moving.
            _rigidbody.linearVelocityY = 0;
            _rigidbody.gravityScale = 0;
        }
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
