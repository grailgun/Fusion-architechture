using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Joyseed.QuantumArk
{
    public abstract class MobaIndicator : MonoBehaviour
    {
        [SerializeField]
        protected float scale = 7f;
        [SerializeField]
        protected float width;
        [SerializeField]
        protected SpriteRenderer indicatorRenderer;

        public Vector2 FinalDirection { get; set; }

        public virtual float Scale {
            get => scale;
            set { 
                scale = value;
            }
        }
        
        public virtual float Width {
            get => width;
            set { 
                width = value;
            }
        }

        public virtual void OnShow() { }
        
        public virtual void OnHide() { }

        public virtual void Show(Vector2 target, float scale, float width)
        {

        }
        
        public virtual void OnValueChanged()
        {
            
        }

        public void SetActive(bool condition)
        {
            gameObject.SetActive(condition);
        }

        protected void SetIndicator(float width, float scale)
        {
            indicatorRenderer.size = new Vector2(width, scale);
        }
    }
}