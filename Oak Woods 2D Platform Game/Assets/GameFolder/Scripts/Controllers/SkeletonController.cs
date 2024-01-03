using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OakWoods2DPlatformGame.Controller
{
    public class SkeletonController : MonoBehaviour
    {
        Rigidbody2D _rb;
        [SerializeField] Transform _player;
        [SerializeField] LayerMask _playerLayer;
        bool _facingRight = true;
        public float _detectRange;
        public float _attackRange;
        [SerializeField] int _health;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        public void LookAtPlayer()
        {
            Vector3 flipped = transform.localScale;
            flipped.z *= -1;

            if (transform.position.x > _player.position.x && _facingRight)
            {
                _facingRight = !_facingRight;

                Vector3 currentScale = transform.localScale;
                currentScale.x *= -1;
                transform.localScale = currentScale;
            }
            else if (transform.position.x < _player.position.x && !_facingRight)
            {
                _facingRight = !_facingRight;

                Vector3 currentScale = transform.localScale;
                currentScale.x *= -1;
                transform.localScale = currentScale;
            }
        }

        public void Attack()
        {
            Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, _attackRange, _playerLayer);
            if(hitPlayer != null)
            {
                hitPlayer.GetComponent<PlayerController>().Takedamage();
            }
        }

        public void TakeDamage(int damage)
        {
            gameObject.GetComponent<Animator>().SetTrigger("TakeDamage");
            _health -= damage;
            if(_health <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            gameObject.GetComponent<Animator>().SetTrigger("Death");
            _rb.bodyType = RigidbodyType2D.Static;
            gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, 3);
           
        }
    }
}

