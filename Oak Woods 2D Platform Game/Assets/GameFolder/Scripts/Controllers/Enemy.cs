using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] Transform _attackPoint;
    [SerializeField] Animator _skeletonAnim;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] float _detectRange;
    [SerializeField] float _attackRange = 0.5f;
    float _movespeed = 1f;
    const float ATTACK_TÝME = 1;
    float _attackTimer;

    [SerializeField] const int MAX_HEALTH = 50;
    int _currentHealth;
    int _moveDir;
   
    bool _isDead = false;
    bool _isPlayerDetected = false;
    bool _isCloseToAttack = false;
    bool _facingRight = true;
    
    private void Start()
    {
        _attackTimer = ATTACK_TÝME;
        _currentHealth = MAX_HEALTH;
    }

    private void Update()
    {
        DetectPlayer();
    }

    public void TakeDamage(int damage)
    {
        if(_isDead == false)
        {
            _currentHealth -= damage;
            //Debug.Log("enemy health " + _currentHealth);

            _skeletonAnim.SetTrigger("TakeDamage");

            if (_currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        _isDead = true;
        _skeletonAnim.SetTrigger("Death");
    }

    void DetectPlayer()
    {
        if (!_isDead)
        {
            _isPlayerDetected = Physics2D.OverlapCircle(transform.position, _detectRange, _playerLayer);
            _isCloseToAttack = Physics2D.OverlapCircle(_attackPoint.position, _attackRange, _playerLayer);

            if (!_isPlayerDetected)
            {
                _skeletonAnim.SetTrigger("Idle");
            }
            else if (_isPlayerDetected && !_isCloseToAttack)
            {
                if (_player.position.x < transform.position.x && _facingRight)
                {
                    Flip();
                }
                else if (_player.position.x > transform.position.x && !_facingRight)
                {
                    Flip();
                }

                MoveToPlayer();
            }
            else if(_isPlayerDetected && _isCloseToAttack)
            {
                Attack();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _detectRange);
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }

    void Flip()
    {
        _facingRight = !_facingRight;

        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }

    private void MoveToPlayer()
    {
        _skeletonAnim.SetTrigger("Walk");

        if (_player.position.x < transform.position.x)
        {
            _moveDir = -1;
        }
        else
        {
            _moveDir = 1;
        }

        transform.Translate(Vector3.right * _moveDir * _movespeed * Time.deltaTime);
    }

    private void Attack()
    {
        //_isCloseToAttack = Physics2D.OverlapCircle(_attackPoint.position, _attackRange, _playerLayer);

        if (_attackTimer >= ATTACK_TÝME)
        {
            Debug.Log("attack");
            _skeletonAnim.SetTrigger("Attack");
            //Collider2D hitPlayer = Physics2D.OverlapCircle(_attackPoint.position, _attackRange, _playerLayer);
            _attackTimer = 0;
        }
        _attackTimer += Time.deltaTime;
    }



    
}
