using System;
using UnityEngine;


[RequireComponent(typeof(CharacterMotor))]

public class GhostMove : MonoBehaviour
{

    private CharacterMotor _motor;
    private Vector2 _boxSize;
    private Vector2 _targetMoveLocation;
    public event Action OnUpdateMoveTarget;
    private bool _allowReverseDirection;


    public CharacterMotor CharacterMotor { get => _motor; }


    public void AllowReverseDirection()
    {
        _allowReverseDirection = true;
    }
    public void SetTargetMoveLocation(Vector2 targetMoveLocation)
    {
        _targetMoveLocation = targetMoveLocation;
    }
    // Start is called before the first frame update
    void Awake()
    {
        _motor = GetComponent<CharacterMotor>();
        _boxSize = GetComponent<BoxCollider2D>().size;

        _motor.OnAlignedWithGrid += CharacterMotor_OnalignedWithGrid;

        CharacterMotor_OnalignedWithGrid();

        _allowReverseDirection = false;
    }
    private void CharacterMotor_OnalignedWithGrid()
    {
        OnUpdateMoveTarget?.Invoke();
        ChangeDirection();
    }


    private void ChangeDirection()
    {


        float closestDistance = float.MaxValue;
        Direction finalDirection = Direction.None;

        UpdateFinalDirection(Direction.up, Vector3.up, ref closestDistance, ref finalDirection);
        UpdateFinalDirection(Direction.left, Vector3.left, ref closestDistance, ref finalDirection);
        UpdateFinalDirection(Direction.down, Vector3.down, ref closestDistance, ref finalDirection);
        UpdateFinalDirection(Direction.right, Vector3.right, ref closestDistance, ref finalDirection);

        _motor.SetMoveDirection(finalDirection);
        _allowReverseDirection = false;
    }


    private void UpdateFinalDirection(Direction direction, Vector3 offset, ref float closestDistance, ref Direction finalDirection)
    {
        if (CheckIfDirectionMovable(direction))
        {
            var pacman = GameObject.FindWithTag("Player").transform;

            var dist = Vector2.Distance(transform.position + offset, _targetMoveLocation);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                finalDirection = direction;
            }
        }
    }


    private bool CheckIfDirectionMovable(Direction direction)
    {
        switch (direction)
        {
            case Direction.up:
                return !Physics2D.BoxCast(transform.position, _boxSize, 0, Vector2.up, 1f, _motor.CollistionLayerMask) && (_motor.CurrentMoveDirection != Direction.down || _allowReverseDirection);

            case Direction.left:
                return !Physics2D.BoxCast(transform.position, _boxSize, 0, Vector2.left, 1f, _motor.CollistionLayerMask) && (_motor.CurrentMoveDirection != Direction.right || _allowReverseDirection);

            case Direction.down:
                return !Physics2D.BoxCast(transform.position, _boxSize, 0, Vector2.down, 1f, _motor.CollistionLayerMask) && (_motor.CurrentMoveDirection != Direction.up || _allowReverseDirection);

            case Direction.right:
                return !Physics2D.BoxCast(transform.position, _boxSize, 0, Vector2.right, 1f, _motor.CollistionLayerMask) && (_motor.CurrentMoveDirection != Direction.left || _allowReverseDirection);

        }

        return false;
    }
}
