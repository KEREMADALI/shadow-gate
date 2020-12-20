namespace Assets.Scripts.Components
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class GateBuilder : MonoBehaviour
    {
        #region Private & Const Variables

        private const int s_GateCount = 2;
        private const float s_ParticleLifeTimeMultiplier = .07f;
        private const float s_OrbitalConstantA = -.00027777777f;
        private const float s_OrbitalConstantB = -.00833333333f;

        [SerializeField]
        private GameObject _gateTop;

        [SerializeField]
        private GameObject _gateBottom;

        [SerializeField]
        private GameObject _middleGate;

        [SerializeField]
        private GameObject _gateCollider;

        [SerializeField]
        private CustomParticleSystem _butterflyParticleSystem;

        [SerializeField]
        private ParticleSystem[] _lightningParticleSystems;

        [SerializeField]
        private float _butterflyEmitInterval = .5f;

        private float _lightningSubEmitInterval = .05f;

        private float _gateLength;

        private float _butterflyTimer;

        private float _lightningTimer;

        private List<Vector2> _gatePoints = new List<Vector2>();

        #endregion

        #region Public & Protected Variables

        #endregion

        #region Constructors

        #endregion

        #region Private Methods

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            EmitButterflies();
            LightningUpdate();
        }

        private void LightningUpdate()
        {
            _lightningTimer += 0.01f;

            if (_lightningTimer < _lightningSubEmitInterval)
            {
                return;
            }

            _lightningTimer = 0;

            foreach (var lightningParticleSystem in _lightningParticleSystems)
            {
                //lightningParticleSystem.TriggerSubEmitter(0);
            }
        }

        /// <summary>
        /// Initializes gate collider and sets particle settings
        /// </summary>
        private void Init()
        {
            var startPosition = _gateBottom.transform.localPosition;
            var middlePosition = _middleGate.transform.localPosition;
            var endPosition = _gateTop.transform.localPosition;
            
            _gatePoints = CreateGatePoints( startPosition, middlePosition, endPosition);
            EditLightnings();
            CreateGateCollider();
        }

        /// <summary>
        /// Edits lightning settings according to distance and angle between gates
        /// </summary>
        private void EditLightnings()
        {
            if (_lightningParticleSystems.Length != s_GateCount) 
            {
                throw new System.Exception($"Not all Lightning particle are defined. There should be {s_GateCount}");
            }

            var distanceBetweenGates = _gateBottom.transform.position.DistanceTo(_gateTop.transform.position);

            EditLightningLength(distanceBetweenGates);
            EditLightningCurve(distanceBetweenGates);
        }

        /// <summary>
        /// Edits lightning length according to distance between gates
        /// </summary>
        /// <param name="distanceBetweenGates"></param>
        private void EditLightningLength(float distanceBetweenGates)
        {
            var lifeTimeMultiplier = distanceBetweenGates * s_ParticleLifeTimeMultiplier;

            foreach (var lightningParticleSystem in _lightningParticleSystems)
            {
                ParticleSystem.MainModule main = lightningParticleSystem.main;
                main.startLifetimeMultiplier = lifeTimeMultiplier;
            }
        }

        /// <summary>
        /// Edits lightning curve according to angle between gates
        /// </summary>
        /// <param name="distanceBetweenGates"></param>
        private void EditLightningCurve(float distanceBetweenGates)
        {
            var rotationDifference = _gateTop.transform.rotation.eulerAngles.z
                - _gateBottom.transform.eulerAngles.z;

            float orbitalY = CalculateIdealOrbitalValue(rotationDifference);

            foreach (var lightningParticleSystem in _lightningParticleSystems)
            {
                ParticleSystem.VelocityOverLifetimeModule module = lightningParticleSystem.velocityOverLifetime;
                module.orbitalY = orbitalY;
            }
        }

        /// <summary>
        /// Calculated by f(x) = a*x^2 + b*x + c
        /// </summary>
        /// <param name="rotationDifference"></param>
        /// <returns></returns>
        private float CalculateIdealOrbitalValue(float rotationDifference)
        {
            return (s_OrbitalConstantA * Mathf.Pow(rotationDifference, 2))
                + (s_OrbitalConstantB * rotationDifference);
        }

        /// <summary>
        /// Create an edge collider between gates
        /// </summary>
        private void CreateGateCollider()
        {
            var edgeCollider = GetComponent<EdgeCollider2D>();

            if (edgeCollider == null) 
            {
                throw new System.Exception($"EdgeCollider2D is missing on {gameObject.name}");
            }

            edgeCollider.points = _gatePoints.ToArray();
        }

        /// <summary>
        /// Emit butterflies according to given interval values
        /// </summary>
        private void EmitButterflies()
        {
            _butterflyTimer += Time.deltaTime;

            if (_butterflyTimer < _butterflyEmitInterval)
            {
                return;
            }

            _butterflyTimer = 0;

            var minIndexLimit = 2;
            var maxIndexLimit = _gatePoints.Count - 3;
            var randomIndex = Random.Range(minIndexLimit, maxIndexLimit);
            _butterflyParticleSystem.EmitParticle(_gatePoints[randomIndex]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="middlePosition"></param>
        /// <param name="endPosition"></param>
        /// <returns></returns>
        private List<Vector2> CreateGatePoints(Vector2 startPosition, Vector2 middlePosition, Vector2 endPosition)
        {
            _gateLength = startPosition.DistanceTo(middlePosition) + middlePosition.DistanceTo(endPosition);
            var gatePoints = new List<Vector2>();
            var incrementer = (1 / _gateLength) /1.5f;


            for (float t = 0; t < 1 + incrementer; t += incrementer)
            {
                var position0 = Vector2.Lerp(startPosition, middlePosition, t);
                var position1 = Vector2.Lerp(middlePosition, endPosition, t);
                var curvePoint = Vector2.Lerp(position0, position1, t);

                gatePoints.Add(curvePoint);
            }

            return gatePoints;
        }

        private void OnDrawGizmos()
        {
            var startPosition = _gateBottom.transform.position;
            var middlePosition = _middleGate.transform.position;
            var endPosition = _gateTop.transform.position;

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(middlePosition, 0.2f);

            var gatePoints = CreateGatePoints(startPosition, middlePosition, endPosition);

            Gizmos.color = Color.red;
            gatePoints.ForEach(x => { Gizmos.DrawSphere(x, 0.1f); });
        }

        #endregion

        #region Public & Protected Methods		

        #endregion
    }
}