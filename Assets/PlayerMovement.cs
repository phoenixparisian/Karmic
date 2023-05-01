using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;  // Import the new Input System package

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;  // The movement speed of the player

    [SerializeField] private float collisionOffset = 0.05f;

    [SerializeField] private ContactFilter2D contactFilter;

    private Vector2 _moveInput;

    private Rigidbody2D _rb;

    private List<RaycastHit2D> _castCollisions = new List<RaycastHit2D>();

    private void Start() {
        _rb = GetComponent<Rigidbody2D>();
        
    }

    private void FixedUpdate() {
        if (_moveInput != Vector2.zero) {
            int count = _rb.Cast(
                _moveInput,
                contactFilter,
                _castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset);
            if (count == 0) {
                _rb.MovePosition(_rb.position + _moveInput * (moveSpeed * Time.fixedDeltaTime));
            }
        }
    }

    void OnMove(InputValue movementValue) {
        _moveInput = movementValue.Get<Vector2>();
    }
}
