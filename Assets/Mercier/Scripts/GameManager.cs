using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mercier.Scripts
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("GameManager::Instance{}:: GameManager is NULL.");
                }

                return _instance;
            }
        }

        private bool _isRunning = false;
        public bool IsRunning { get { return _isRunning; } }

        public static event Action onStart;

        private void Awake()
        {
            _instance = this;
        }

        private void Update()
        {
            if (!_isRunning && Input.GetKeyDown(KeyCode.Space))
            {
                _isRunning = true;

                OnStart();
            }
        }

        private void OnStart()
        {
            onStart?.Invoke();
        }
    }
}

