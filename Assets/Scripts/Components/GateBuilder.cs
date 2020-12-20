namespace Assets.Scripts.Components
{
    using System.Collections.Generic;
    using UnityEngine;

    public class GateBuilder : MonoBehaviour
    {
        #region Private & Const Variables

        [SerializeField]
        private GameObject _gate0;

        [SerializeField]
        private GameObject _gate1;

        [SerializeField]
        private GameObject _middleGate;

        [SerializeField]
        private GameObject _gateCollider;

        [SerializeField]
        private CustomParticleSystem _particleSystem;

        [SerializeField]
        private float _particleEmitInterval = 1f;

        private float _gateLength;

        private float _timer;

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

        private void Init()
        {
            var startPosition = _gate0.transform.localPosition;
            var middlePosition = _middleGate.transform.localPosition;
            var endPosition = _gate1.transform.localPosition;
            
            _gatePoints = CreateGatePoints( startPosition, middlePosition, endPosition);
            CreateGateCollider();
        }

        private void CreateGateCollider()
        {
            var edgeCollider = GetComponent<EdgeCollider2D>();
            edgeCollider.points = _gatePoints.ToArray();
        }
        private void Update()
        {
            EmitParticles();
        }

        private void EmitParticles()
        {
            _timer += Time.deltaTime; 

            if (_timer < _particleEmitInterval)
            {
                return;
            }

            _timer = 0;

            var minIndexLimit = 2;
            var maxIndexLimit = _gatePoints.Count - 3;
            var randomIndex = Random.Range(minIndexLimit, maxIndexLimit);
            _particleSystem.EmitParticle(_gatePoints[randomIndex]);
        }

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
            var startPosition = _gate0.transform.position;
            var middlePosition = _middleGate.transform.position;
            var endPosition = _gate1.transform.position;

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