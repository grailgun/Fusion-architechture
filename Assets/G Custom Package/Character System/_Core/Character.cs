using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomCode
{
    public class Character : MonoBehaviour
    {
        public CharacterType CharacterType = CharacterType.AI;

        protected CharacterAbility[] characterAbilities;

        private bool hasCacheAbility;

        protected void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            hasCacheAbility = false;
            SetInputManager();
            CacheCharacterAbilities();
        }

        private void SetInputManager()
        {
            
        }

        private void CacheCharacterAbilities()
        {
            if (hasCacheAbility) return;
            characterAbilities = this.gameObject.GetComponents<CharacterAbility>();
            hasCacheAbility = true;
        }

        private void Update()
        {
            EveryFrame();
        }

        private void EveryFrame()
        {
            EarlyProcessAbilities();
            ProcessAbilities();
            LateProcessAbilities();
        }
        
        protected virtual void EarlyProcessAbilities()
        {
            foreach (CharacterAbility ability in characterAbilities)
            {
                if (ability.enabled && ability.IsInitialized)
                {
                    ability.EarlyProcessAbility();
                }
            }
        }

        protected virtual void ProcessAbilities()
        {
            foreach (CharacterAbility ability in characterAbilities)
            {
                if (ability.enabled && ability.IsInitialized)
                {
                    ability.ProcessAbilities();
                }
            }
        }

        protected virtual void LateProcessAbilities()
        {
            foreach (CharacterAbility ability in characterAbilities)
            {
                if (ability.enabled && ability.IsInitialized)
                {
                    ability.LateProcessAbility();
                }
            }
        }

        private void FixedUpdate()
        {
            ProcessFixedUpdate();
        }
        
        protected virtual void ProcessFixedUpdate()
        {
            foreach (CharacterAbility ability in characterAbilities)
            {
                if (ability.isActive && ability.IsInitialized)
                {
                    ability.ProcessFixedUpdate();
                }
            }
        }

        private void OnDestroy()
        {
            foreach (CharacterAbility ability in characterAbilities)
            {
                ability.CleanUp();
            }
        }
    }
    
    public enum CharacterType { Player , AI }
}
