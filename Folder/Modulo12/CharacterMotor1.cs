using System;
using UnityEngine;


public enum Direction
{
    up,
    down,
    left,
    right,
    None
}

public class CharacterMotor : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] public float speed = 5f;


    private Vector2 _currentmovimentDirection;
    private Vector2 _desiredMovimentDirection;
    private Vector2 _boxSize;


    public event Action<Direction> OnDirectionChanged;
    public event Action OnAlignedWithGrid;
    public event Action OnresetPosition;
    public event Action OnDisabled;

    public LayerMask CollistionLayerMask
    {
        get => _collisionLayerMask;
    }

    private Vector3 _initialPosition;

    private Rigidbody2D _rigidbody;

    private LayerMask _collisionLayerMask;

    public Direction CurrentMoveDirection
    {
        get
        {
            //up
            if (_currentmovimentDirection.y > 0)
            {
                return Direction.up;
            }
            //left
            else if (_currentmovimentDirection.x < 0)
            {
                return Direction.left;
            }
            //donw
            else if (_currentmovimentDirection.y < 0)
            {
                return Direction.down;
            }
            //Ri
            else if (_currentmovimentDirection.x > 0)
            {
                return Direction.right;
            }
            else
            {
                return Direction.None;
            }

        }
    }
    public void SetMoveDirection(Direction newMoveDirection)
    {

        switch (newMoveDirection)
        {

            default:
            case Direction.up:
                _desiredMovimentDirection = Vector2.up;
                break;
            case Direction.down:
                _desiredMovimentDirection = Vector2.down;
                break;
            case Direction.left:
                _desiredMovimentDirection = Vector2.left;
                break;
            case Direction.right:
                _desiredMovimentDirection = Vector2.right;
                break;
            case Direction.None:
                _desiredMovimentDirection = Vector2.zero;
                break;
        }


    }


    public void ResetPosition()
    {
        _desiredMovimentDirection = Vector2.zero;
        _currentmovimentDirection = Vector2.zero;
        transform.position = _initialPosition;
        OnresetPosition?.Invoke();
    }

    public void CollideWithGates(bool Shouldcollide)
    {
        if (Shouldcollide)
        {
            _collisionLayerMask = LayerMask.GetMask(new string[] { "Level", "Gates" });
        }
        else
        {
            _collisionLayerMask = LayerMask.GetMask(new string[] { "Level" });
        }



    }

    private void Start()
    {
        _desiredMovimentDirection = Vector2.zero;
        _currentmovimentDirection = Vector2.zero;
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxSize = GetComponent<BoxCollider2D>().size;
        CollideWithGates(true);
        _initialPosition = transform.position;
    }


    private void FixedUpdate()
    {
        float moveDistance = speed * Time.fixedDeltaTime;
        var nextMovePosition = _rigidbody.position + _currentmovimentDirection * moveDistance;

        //up
        if (_currentmovimentDirection.y > 0)
        {
            var maxy = Mathf.CeilToInt(_rigidbody.position.y);
            if (nextMovePosition.y >= maxy)
            {
                transform.position = new Vector2(_rigidbody.position.x, maxy);
                moveDistance = nextMovePosition.y - maxy;
            }
        }
        //left
        if (_currentmovimentDirection.x < 0)
        {
            var minX = Mathf.FloorToInt(_rigidbody.position.x);
            if (nextMovePosition.x <= minX)
            {
                transform.position = new Vector2(minX, _rigidbody.position.y);
                moveDistance = minX - nextMovePosition.x;
            }
        }
        //donw
        if (_currentmovimentDirection.y < 0)
        {
            var miny = Mathf.FloorToInt(_rigidbody.position.y);
            if (nextMovePosition.y <= miny)
            {
                transform.position = new Vector2(_rigidbody.position.x, miny);
                moveDistance = miny - nextMovePosition.y;
            }
        }
        //Ri
        if (_currentmovimentDirection.x > 0)
        {
            var maxX = Mathf.CeilToInt(_rigidbody.position.x);
            if (nextMovePosition.x >= maxX)
            {
                transform.position = new Vector2(maxX, _rigidbody.position.y);
                moveDistance = nextMovePosition.x - maxX;
            }
        }


        Physics2D.SyncTransforms();
        //verifica alinhamento
        if ((_rigidbody.position.y == Mathf.CeilToInt(_rigidbody.position.y) &&
            _rigidbody.position.x == Mathf.CeilToInt(_rigidbody.position.x)) || _currentmovimentDirection == Vector2.zero)

        {
            OnAlignedWithGrid?.Invoke();

            if (_currentmovimentDirection != _desiredMovimentDirection)
            {
                if (!Physics2D.BoxCast(_rigidbody.position, _boxSize, 0, _desiredMovimentDirection, 1f, _collisionLayerMask))
                {
                    _currentmovimentDirection = _desiredMovimentDirection;
                    OnDirectionChanged?.Invoke(CurrentMoveDirection);
                }
            }
            if (Physics2D.BoxCast(_rigidbody.position, _boxSize, 0, _currentmovimentDirection, 1f, _collisionLayerMask))
            {
                _currentmovimentDirection = Vector2.zero;
                OnDirectionChanged?.Invoke(CurrentMoveDirection);
            }

        }
        _rigidbody.MovePosition(_rigidbody.position + _currentmovimentDirection * moveDistance);
    }


    private void OnDisable()
    {
        OnDisabled?.Invoke();
    }
}
