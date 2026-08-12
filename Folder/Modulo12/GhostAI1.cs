using System;
using UnityEngine;

public enum GhostState
{
    Active,
    Vulnerable,
    VulnerabilityEnding,
    Defeat
}


[RequireComponent(typeof(GhostMove))]
public class GhostAI : MonoBehaviour
{
    public float VulnerabilityEndingTime;
    private GhostMove _ghostMove;

    private Transform _pacman;

    private GhostState _ghostState;

    private float vulnerabilityTimer;

    private bool _leaveHouse;

    public event Action<GhostState> OnGhostStateChanged;

    private Vector3 _position;

    public void Reset()
    {

        _ghostMove.CharacterMotor.ResetPosition();
        _ghostState = GhostState.Active;
        OnGhostStateChanged?.Invoke(_ghostState);
        _leaveHouse = false;
    }
    public void StartMoving()
    {
        _ghostMove.CharacterMotor.enabled = true;
    }
    public void StopMoving()
    {
        _ghostMove.CharacterMotor.enabled = false;
    }

    public void SetVulnerable(float duration)
    {
        vulnerabilityTimer = duration;
        _ghostState = GhostState.Vulnerable;
        OnGhostStateChanged?.Invoke(_ghostState);
        _ghostMove.AllowReverseDirection();
    }

    public void Recover()
    {
        _ghostMove.CharacterMotor.CollideWithGates(true);
        _ghostState = GhostState.Active;
        OnGhostStateChanged?.Invoke(_ghostState);
        _leaveHouse = false;
    }



    public void LeaveHouse()
    {
        _ghostMove.CharacterMotor.CollideWithGates(false);
        _leaveHouse = true;
    }
    private void Start()
    {
        _ghostMove = GetComponent<GhostMove>();
        _ghostMove.OnUpdateMoveTarget += GhostMove_OnUpdateMoveTarget;
        _pacman = GameObject.FindWithTag("Player").transform;
        _ghostState = GhostState.Active;

        _leaveHouse = false;

    }



    private void GhostMove_OnUpdateMoveTarget()
    {

        switch (_ghostState)
        {
            case GhostState.Active:
                if (_leaveHouse)
                {
                    if (transform.position == new Vector3(0, 3, 0))
                    {
                        _leaveHouse = false;
                        _ghostMove.CharacterMotor.CollideWithGates(true);
                        _ghostMove.SetTargetMoveLocation(_pacman.position);

                    }

                    else
                    {
                        _ghostMove.SetTargetMoveLocation(new Vector3(0, 3, 0));
                    }
                }
                else
                {

                    _ghostMove.SetTargetMoveLocation(_pacman.position);
                }
                break;

            case GhostState.Vulnerable:
            case GhostState.VulnerabilityEnding:

                _ghostMove.SetTargetMoveLocation((transform.position - _pacman.position) * 2);
                break;

            case GhostState.Defeat:

                _ghostMove.SetTargetMoveLocation(Vector3.zero);

                break;
        }

    }



    private void Update()
    {

        switch (_ghostState)
        {
            case GhostState.Vulnerable:
                vulnerabilityTimer -= Time.deltaTime;
                if (vulnerabilityTimer < VulnerabilityEndingTime)
                {
                    _ghostState = GhostState.VulnerabilityEnding;
                    OnGhostStateChanged?.Invoke(_ghostState);
                }
                break;
            case GhostState.VulnerabilityEnding:
                vulnerabilityTimer -= Time.deltaTime;
                if (vulnerabilityTimer <= 0)
                {
                    _ghostState = GhostState.Active;
                    OnGhostStateChanged?.Invoke(_ghostState);
                }
                break;
            case GhostState.Defeat:
                _ghostMove.CharacterMotor.speed = 2;

                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        switch (_ghostState)
        {
            case GhostState.Active:
                if (other.CompareTag("Player"))
                {
                    other.GetComponent<LIfe>().RemoveLife();
                }
                break;
            case GhostState.Vulnerable:
            case GhostState.VulnerabilityEnding:
                if (other.CompareTag("Player"))
                {
                    _ghostMove.CharacterMotor.CollideWithGates(false);
                    _ghostState = GhostState.Defeat;
                    OnGhostStateChanged?.Invoke(_ghostState);
                }
                break;
            case GhostState.Defeat:
                break;
        }

    }
}
