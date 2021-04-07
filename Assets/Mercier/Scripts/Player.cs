using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mercier.Scripts.Abstract;

namespace Mercier.Scripts
{
    public class Player : Navigation
    {
        [SerializeField]
        private Enemy _closestEnemy;
        private Enemy _currentEnemy;

        [SerializeField]
        private List<Enemy> _enemiesInRange = new List<Enemy>();

        private float _iDist;
        private float _jDist;
        private Enemy _tempEnemy;

        public static event Action<Enemy, bool> onChangeColor;

        protected override void OnEnable()
        {
            base.OnEnable();

            GameManager.onStart += StartCalculateDistance;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            GameManager.onStart -= StartCalculateDistance;
        }

        private void StartCalculateDistance()
        {
            StartCoroutine(CalculateDistanceRoutine());
        }

        private IEnumerator CalculateDistanceRoutine()
        {
            while (GameManager.Instance.IsRunning)
            {
                BubbleSortList(); // faster if not return type

                _closestEnemy = _enemiesInRange[0]; 

                if (_currentEnemy != _closestEnemy)
                {
                    if (_currentEnemy != null)
                    {
                        OnChangeColor(_currentEnemy, false);
                    }

                    _currentEnemy = _closestEnemy;
                }

                OnChangeColor(_currentEnemy, true);

                yield return _waitForSeconds;
            }
        }

        private void BubbleSortList()
        {
            for (int i = 0; i < _enemiesInRange.Count; i++)
            {
                for (int j = i + 1; j < _enemiesInRange.Count; j++)
                {
                    _iDist = Vector3.Distance(transform.position, _enemiesInRange[i].transform.position);
                    _jDist = Vector3.Distance(transform.position, _enemiesInRange[j].transform.position);

                    if (_iDist > _jDist)
                    {
                        _tempEnemy = _enemiesInRange[i];

                        _enemiesInRange[i] = _enemiesInRange[j];
                        _enemiesInRange[j] = _tempEnemy;
                    }
                }
            }
        }

        private void OnChangeColor(Enemy enemy, bool isClosest)
        {
            onChangeColor?.Invoke(enemy, isClosest);
        }
    }
}

