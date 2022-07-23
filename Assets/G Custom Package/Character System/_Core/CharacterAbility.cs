using UnityEngine;

namespace CustomCode
{
    public class CharacterAbility : MonoBehaviour
    {
        public bool isActive = true;
        private bool isInitialized;
        
        protected Character character;
        
        [Header("View")]
        protected SpriteRenderer spriteRenderer;

        protected virtual void Awake()
        {
            PreInit();
        }

        protected virtual void Start()
        {
            Init();
        }

        public bool IsInitialized => isInitialized;
        
        protected virtual void PreInit()
        {
            character = gameObject.GetComponent<Character>();
        }
        
        protected virtual void Init()
        {
            spriteRenderer = gameObject.GetComponentInParent<SpriteRenderer>();
            isInitialized = true;
        }
        
        public virtual void EarlyProcessAbility()
        {
            
        }

        public virtual void ProcessAbilities()
        {
            
        }
        
        public virtual void LateProcessAbility()
        {
			
        }

        public virtual void ProcessFixedUpdate()
        {
            
        }

        public virtual void CleanUp()
        {
            
        }
    }
}