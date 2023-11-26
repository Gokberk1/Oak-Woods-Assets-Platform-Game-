using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D _rb;
    float _horizontal;
    [SerializeField] float _moveSpeed = 3;
    [SerializeField] float _jumpForce;
    [SerializeField] float _groundCheckRadius = 0.2f;
    [SerializeField] Animator _animator;
    [SerializeField] Transform _groundDetector;
    [SerializeField] LayerMask _groundLayer;
    bool _isOnTheGround;
    

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        Jump();
    }

    private void FixedUpdate()
    {
        HorizontalMove();
    }

    void HorizontalMove()
    {
        if (_horizontal == 0) 
        {
            _animator.SetInteger("StateNumber", 0);
        }
        else 
        {
            transform.Translate(Vector3.right * _horizontal * _moveSpeed * Time.fixedDeltaTime);
            _animator.SetInteger("StateNumber", 1);
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(_groundDetector.position, _groundCheckRadius);
    //}
    void Jump()
    {
        _isOnTheGround = Physics2D.OverlapCircle(_groundDetector.transform.position, _groundCheckRadius, _groundLayer);
        

        if (Input.GetKeyDown(KeyCode.Space) && _isOnTheGround == true)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        }
        if (_rb.velocity.y > 0 && !_isOnTheGround)
        {
            _animator.SetInteger("StateNumber", 2);
        }
        else if(_rb.velocity.y < 0 && !_isOnTheGround)
        {
            _animator.SetInteger("StateNumber", 3);

            if(_animator.GetInteger("StateNumber") == 3 && _isOnTheGround)
            {
                _animator.SetInteger("StateNumber", 4);
            }
        }
    }
}
