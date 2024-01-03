using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OakWoods2DPlatformGame.Controller
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] Transform _attackPoint;
        [SerializeField] LayerMask _enemyLayers;
        [SerializeField] float _attackRange = 0.5f;
        [SerializeField] int _attackDamage = 2;
        bool _isAttacking = false;

        void Update()
        {
            Attack();
        }

        void Attack()
        {
            if (Input.GetMouseButtonDown(0) && !_isAttacking)
            {
                _isAttacking = true;

                _animator.SetTrigger("AttackTrigger");

                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayers);

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
        }
    }
}

