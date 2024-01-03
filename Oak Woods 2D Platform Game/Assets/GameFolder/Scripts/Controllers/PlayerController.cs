using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OakWoods2DPlatformGame.Controller
{
    public class PlayerController : MonoBehaviour
    {
        Rigidbody2D _rb;
        float _horizontal;
        [SerializeField] float _moveSpeed = 3;
        [SerializeField] float _jumpForce;
        [SerializeField] float _groundCheckRadius = 0.2f;
        [SerializeField] Animator _animator;
        [SerializeField] Transform _groundDetector;
        [SerializeField] Transform _attackPoint;
        [SerializeField] LayerMask _groundLayer;
        [SerializeField] LayerMask _enemyLayer;

        [SerializeField] float _attackRange = 0.5f;
        [SerializeField] int _attackDamage = 2;
        bool _isAttacking = false;


        bool _isOnTheGround;
        bool _facingRight = true;

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            Jump();
            Attack();
        }

        private void FixedUpdate()
        {
            HorizontalMove();
        }

        void HorizontalMove()
        {
            if (!_isAttacking)
            {
                if (_horizontal == 0)
                {
                    _animator.SetInteger("StateNumber", 0);
                }
                else
                {
                    transform.Translate(Vector3.right * _horizontal * _moveSpeed * Time.fixedDeltaTime);
                    _animator.SetInteger("StateNumber", 1);

                    if (_horizontal > 0 && !_facingRight)
                    {
                        Flip();
                    }
                    else if (_horizontal < 0 && _facingRight)
                    {
                        Flip();
                    }
                }
            }
        }

        void Flip()
        {
            _facingRight = !_facingRight;

            Vector3 currentScale = transform.localScale;
            currentScale.x *= -1;
            transform.localScale = currentScale;
        }

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
            else if (_rb.velocity.y < 0 && !_isOnTheGround)
            {
                _animator.SetInteger("StateNumber", 3);
            }
        }

        public void Takedamage()
        {
            Debug.Log("take damage");
        }

        void Attack()
        {
            if (Input.GetMouseButtonDown(0) && !_isAttacking)
            {
                _isAttacking = true;

                _animator.SetTrigger("AttackTrigger");

                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);

                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<SkeletonController>().TakeDamage(_attackDamage);
                }

                StartCoroutine(AttackTimer());
            }
        }

        IEnumerator AttackTimer()
        {
            yield return new WaitForSeconds(0.5f);
            _isAttacking = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
            Gizmos.DrawWireSphere(_groundDetector.position, _groundCheckRadius);
        }
    }
}

