namespace Assets.Scripts.Components
{
	using UnityEngine;

	[RequireComponent(typeof(SpriteRenderer))]
	public class ButterflyController : MonoBehaviour
	{
		#region Private & Const Variables
		
		private float _lifeSpanTimer;
		private float _lifeExpectancy = 2;
		private float _minVelocityLimit = .3f;
		private float _maxVelocityLimit = .5f;

		private Vector2 _velocity;
		private SpriteRenderer _spriteRenderer;

		private bool _isLifeEnded => CheckLifeSpan();

        #endregion

        #region Public & Protected Variables

        #endregion

        #region Constructors

        #endregion

        #region Private Methods

        private void Start()
        {
			SetComponents();
		}

        private void OnEnable() 
		{
			SetRandomVelocity();
		}

        private void Update()
        {
			if (_isLifeEnded)
			{
				DisableButterfly();
			}

			Move();
			DecreaseAlpha();
		}

		/// <summary>
		/// Sets required components
		/// </summary>
		private void SetComponents()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		/// <summary>
		/// Fade-out effect
		/// </summary>
		private void DecreaseAlpha()
        {
			var color = _spriteRenderer.color;
			var alpha = color.a - (Time.deltaTime / _lifeExpectancy);
			_spriteRenderer.color = color.With(a: alpha);
		}

		/// <summary>
		/// Moves butterfly
		/// </summary>
        private void Move()
        {
			transform.position += (Vector3)_velocity * Time.deltaTime;
        }

		/// <summary>
		/// Checks if the lifespan is over
		/// </summary>
		/// <returns></returns>
        private bool CheckLifeSpan()
        {
			_lifeSpanTimer += Time.deltaTime;

			if (_lifeSpanTimer < _lifeExpectancy)
			{
				return false;
			}

			_lifeSpanTimer = 0;
			return true;
		}

		/// <summary>
		/// Disables object and resets alpha value
		/// </summary>
        private void DisableButterfly()
        {
			gameObject.SetActive(false);

			_spriteRenderer.color = _spriteRenderer.color.With(a: 1);
		}

		/// <summary>
		/// 
		/// </summary>
        private void SetRandomVelocity()
		{
			var randomX = Random.Range(-1f, 1f);
			var randomY = Random.Range(-1f, 1f);
			var randomMagnitude = Random.Range(_minVelocityLimit, _maxVelocityLimit);

			var randomNormal = new Vector2(randomX, randomY).normalized;
			var randomVelocity = randomNormal * randomMagnitude;
			_velocity = randomVelocity;
		}

		#endregion

		#region Public & Protected Methods		

		/// <summary>
		/// Called from emitter
		/// </summary>
		/// <param name="velocityLimit"></param>
		/// <param name="lifeExpectancy"></param>
		public void Setup(float minVelocityLimit, float maxVelocityLimit, float lifeExpectancy) 
		{
			_minVelocityLimit = minVelocityLimit;
			_maxVelocityLimit = maxVelocityLimit;
			_lifeExpectancy = lifeExpectancy;
		}

		#endregion
	}
}