using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Mercier.Scripts.Abstract
{
    public abstract class Navigation : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent _navMeshAgent;
        private NavMeshHit _navMeshHit;

        [SerializeField]
        private static float _maxRange = 2.5f; 
        private float _randomRange;
        private float _targetDistance;
        private static float _targetOffset = 1f;
        private Vector3 _targetDirection;
        private Vector3 _targetDestination;

        private bool _posAvalable = false;

        protected static WaitForSeconds _waitForSeconds = new WaitForSeconds(0.25f);

        protected virtual void OnEnable()
        {
            GameManager.onStart += StartRandomNavigation;
        }

        protected virtual void OnDisable()
        {
            GameManager.onStart -= StartRandomNavigation;
        }

        private void StartRandomNavigation()
        {
            StartCoroutine(RandomNavigationRoutine());
        }

        private IEnumerator RandomNavigationRoutine()
        {
            _targetDestination = ReturnRandomPoint();
            _navMeshAgent.SetDestination(_targetDestination);

            while (GameManager.Instance.IsRunning)
            {
                _targetDistance = Vector3.Distance(transform.position, _targetDestination);

                if (_targetDistance < _targetOffset)
                {
                    _targetDestination = ReturnRandomPoint();
                    _navMeshAgent.SetDestination(_targetDestination);
                }

                yield return _waitForSeconds;
            }
        }

        private Vector3 ReturnRandomPoint()
        {
            for (int i = 0; i < 30; i++)
            {
                _targetDirection = transform.position + Random.insideUnitSphere * _maxRange;
                _randomRange = Random.Range(0f, _maxRange);
                _posAvalable = NavMesh.SamplePosition(_targetDirection, out _navMeshHit, _randomRange, NavMesh.AllAreas);

                if (_posAvalable)
                {
                    return _navMeshHit.position;
                }
            }

            return Vector3.zero;
        }
    }
}

