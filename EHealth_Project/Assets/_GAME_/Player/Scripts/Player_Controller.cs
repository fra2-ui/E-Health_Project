using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[SelectionBase]
public class Player_Controller : MonoBehaviour
{
    public bool canMove = true;

    private enum Directions { Up, Down, Left, Right }

    private void Start()
    {
        // Logica originale per la cutscene
        if (GameSession.playedBusCutscene)
        {
            canMove = true;
            Debug.Log($"[Player_Controller] Start in scena {SceneManager.GetActiveScene().name} → cutscene già vista, abilito movimento");
        }
        else
        {
            Debug.Log($"[Player_Controller] Start in scena {SceneManager.GetActiveScene().name} → cutscene NON ancora vista, movimento gestito da BusAnimation");
        }
    }

    private void Update()
    {
        if (!canMove)
        {
            _moveDir = Vector2.zero;
            _animator.CrossFade(_animIdle, 0);
            return;
        }

        CalculateFacingDirections();
        UpdateAnimations();
    }

    private Vector2 _moveDir = Vector2.zero;
    private Directions _facingDirections = Directions.Right;

    private readonly int _animMoveRight = Animator.StringToHash("Anim_Move_Right");
    private readonly int _animIdle = Animator.StringToHash("Anim_Idle_Front");
    private readonly int _animMoveUp = Animator.StringToHash("Anim_Move_Up");
    private readonly int _animMoveDown = Animator.StringToHash("Anim_Move_Down");

    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 50f;

    [Header("Dependencies")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;

    private readonly List<Vector2> _posHistory = new();
    private const int MaxHistory = 40;
    private float _traumaTimer = 0f;

    public void OnMove(InputValue value)
    {
        if (!canMove)
        {
            _moveDir = Vector2.zero;
            return;
        }

        Vector2 v = value.Get<Vector2>();

        if (StatusManager.Instance != null && StatusManager.Instance.controlsInverted)
            v = -v;

        _moveDir = v;
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        MovementUpdate();
        RecordPosition();
        ApplyTraumaRollback();
    }

    private void MovementUpdate()
    {
        _rb.linearVelocity = Vector2.zero;
        Vector2 movement = _moveDir * _moveSpeed * Time.deltaTime;
        _rb.MovePosition(_rb.position + movement);
    }

    private void RecordPosition()
    {
        _posHistory.Add(_rb.position);
        if (_posHistory.Count > MaxHistory)
            _posHistory.RemoveAt(0);
    }

    private void ApplyTraumaRollback()
    {
        var sm = StatusManager.Instance;
        if (sm == null) return;

        if (!sm.traumaLagActive)
        {
            _traumaTimer = 0f;
            return;
        }

        if (_moveDir.sqrMagnitude < 0.0001f) return;

        _traumaTimer += Time.fixedDeltaTime;
        if (_traumaTimer < sm.traumaTickInterval) return;
        _traumaTimer = 0f;

        if (UnityEngine.Random.value > sm.traumaTickChance) return;

        if (_posHistory.Count < 3) return;

        int framesBack = Mathf.Clamp(sm.traumaFramesBack, 1, MaxHistory - 1);
        int idx = _posHistory.Count - 1 - framesBack;
        if (idx < 0 || idx >= _posHistory.Count) return;

        Vector2 rollbackPos = _posHistory[idx];
        _rb.position = rollbackPos;

        _posHistory.Clear();
        _posHistory.Add(_rb.position);
    }

    private void CalculateFacingDirections()
    {
        if (_moveDir.x > 0) _facingDirections = Directions.Right;
        else if (_moveDir.x < 0) _facingDirections = Directions.Left;
        else if (_moveDir.y > 0) _facingDirections = Directions.Up;
        else if (_moveDir.y < 0) _facingDirections = Directions.Down;
    }

    private void UpdateAnimations()
    {
        if (_facingDirections == Directions.Right)
        {
            _spriteRenderer.flipX = false;
            if (_moveDir.SqrMagnitude() > 0) _animator.CrossFade(_animMoveRight, 0);
            else _animator.CrossFade(_animIdle, 0);
        }
        else if (_facingDirections == Directions.Left)
        {
            _spriteRenderer.flipX = true;
            if (_moveDir.SqrMagnitude() > 0) _animator.CrossFade(_animMoveRight, 0);
            else _animator.CrossFade(_animIdle, 0);
        }
        else if (_facingDirections == Directions.Up)
        {
            _spriteRenderer.flipX = false;
            if (_moveDir.SqrMagnitude() > 0) _animator.CrossFade(_animMoveUp, 0);
            else _animator.CrossFade(_animIdle, 0);
        }
        else if (_facingDirections == Directions.Down)
        {
            _spriteRenderer.flipX = false;
            if (_moveDir.SqrMagnitude() > 0) _animator.CrossFade(_animMoveDown, 0);
            else _animator.CrossFade(_animIdle, 0);
        }
    }

    public event Action PlayerDied;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Object") || collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collided with Enemy!");
            
            
            PlayerDied?.Invoke();
            
            
        }
    }
}