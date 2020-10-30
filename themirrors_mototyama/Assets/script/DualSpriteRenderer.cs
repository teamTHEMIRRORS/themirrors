using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace script
{
    public class DualSpriteRenderer : MonoBehaviour
    {
        public List<Sprite> _sprites;
        public Boolean _isPoltergeisted;
        private SpriteRenderer _attachedSpriteRenderer;
        private float randamvol = 0;
        private Boolean baseSpr = true;
        private void Start()
        {
            _attachedSpriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        /// <summary>
        /// _isPoltergeistedがオンの時にスプライトをランダム時間差し替えるよ。
        /// </summary>
        private void Update()
        {
            if (_isPoltergeisted)
            {
                randamvol += Random.Range(0, 0.03f);
                if (randamvol > 1.0)
                {
                    randamvol = 0;
                    baseSpr = !baseSpr;
                    _attachedSpriteRenderer.sprite = _sprites[baseSpr ? 0:1];
                }
            }
            else
            {
                baseSpr = true;
                _attachedSpriteRenderer.sprite = _sprites[0];
            }
        }
        
    }
}