using System;
using Managers.Impl;
using UnityEngine;

namespace ScriptableObjects
{
    [Serializable]
    public class PowerUpConfiguration :  IPowerUpConfiguration
    {
        [Header("Power up Config")]
        [SerializeField]
        private PowerUpInfo[] _powerUpInfos;
        [SerializeField]
        private float _powerUpSpeed;
        [SerializeField]
        private float _occurenceDelay;

        public PowerUpInfo[] PowerUpInfos => _powerUpInfos;
        public float PowerUpSpeed => _powerUpSpeed;
        public float OccurenceDelay => _occurenceDelay;
    }
}