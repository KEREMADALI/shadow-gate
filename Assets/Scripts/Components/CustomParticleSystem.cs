namespace Assets.Scripts.Components 
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class CustomParticleSystem : MonoBehaviour
    {
        #region Private & Const Variables

        [SerializeField]
        private bool _isAutoEmitOn = false;

        [SerializeField]
        private float _autoEmitInterval = 0.5f;

        [SerializeField]
        private GameObject _particlePrefab;

        [SerializeField]
        private int _numberOfTotalParticles = 50;

        [SerializeField]
        private float _minParticleSize = 0.9f;

        [SerializeField]
        private float _maxParticleSize = 1.1f;

        [SerializeField]
        private float _minParticleVelocityLimit = 0.3f;

        [SerializeField]
        private float _maxParticleVelocityLimit = 0.5f;

        [SerializeField]
        private float _particleLifeTime = 10f;

        private List<GameObject> _particlePool = new List<GameObject>();

        private float _time;

        #endregion

        #region Public & Protected Variables

        #endregion

        #region Constructors

        #endregion

        #region Private Methods

        private void Start()
        {
            FillParticlePool();
        }

        private void Update()
        {
            if (_isAutoEmitOn) 
            {
                AutoEmitParticle();
            }
        }

        private void AutoEmitParticle()
        {
            _time += Time.deltaTime;

            if (_time < _autoEmitInterval)
            {
                return;
            }

            _time = 0;

            EmitParticle(transform.position);
        }

        private void SetParticleSettings(GameObject particleObject, Vector2 position)
        {
            // Set position
            particleObject.transform.localPosition = position;

            // Set size
            var sizeScale = Random.Range(_minParticleSize, _maxParticleSize);
            particleObject.transform.localScale = 
                particleObject.transform.localScale.With(x: sizeScale, y: sizeScale);
        }

        private void FillParticlePool()
        {
            for (int i = 0; i < _numberOfTotalParticles; i++)
            {
                CreateParticle();                
            }
        }

        private GameObject CreateParticle() 
        {
            var particleObject = Instantiate(_particlePrefab, transform);
            particleObject.SetActive(false);

            var butterflyFly = particleObject.GetComponent<ButterflyController>();
            butterflyFly.Setup(_minParticleVelocityLimit, _maxParticleVelocityLimit, _particleLifeTime);

            _particlePool.Add(particleObject);

            return particleObject;
        }

        #endregion

        #region Public & Protected Methods		

        public void EmitParticle(Vector2 position)
        {
            var particleObject = _particlePool.FirstOrDefault(x => !x.activeSelf);

            if (particleObject == null)
            {
                particleObject = CreateParticle();
            }

            SetParticleSettings(particleObject, position);
            particleObject.SetActive(true);
        }

        #endregion
    }

}

