using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mercier.Scripts.Abstract;

namespace Mercier.Scripts
{
    public class Enemy : Navigation
    {
        [SerializeField]
        private Renderer _renderer;
        private Material _material;

        private static Color32 _defaultColor = new Color32(192, 58, 17, 255);

        private void Start()
        {
            if (_renderer != null)
            {
                _material = _renderer.material;
            }
            else
            {
                if (gameObject.TryGetComponent(out _renderer))
                {
                    _material = _renderer.material;
                }
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            Player.onChangeColor += ChangeColor;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Player.onChangeColor -= ChangeColor;
        }

        private void ChangeColor (Enemy enemy, bool isClosest)
        {
            if (this == enemy)
            {
                if (isClosest)
                {
                    if (_material.color != Color.green)
                    {
                        _material.color = Color.green;
                    }
                }
                else
                {
                    if (_material.color != _defaultColor)
                    {
                        _material.color = _defaultColor;
                    }                    
                }
            }
        }
    }
}

