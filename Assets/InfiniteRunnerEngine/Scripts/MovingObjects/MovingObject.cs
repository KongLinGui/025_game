using UnityEngine;
using System.Collections;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Add this class to an object and it'll move according to the level's speed. 
	/// </summary>
	public class MovingObject : MonoBehaviour 
	{
		/// the speed of the object (relative to the level's speed)
		public float Speed=0;
		/// the acceleration of the object over time. Starts accelerating on enable.
	    public float Acceleration = 0;
	    
	    protected Vector3 _movement;
	    protected float _initialSpeed;

	    /// <summary>
		/// On awake, we store the initial speed of the object 
	    /// </summary>
	    protected virtual void Awake () 
		{
	        _initialSpeed = Speed;
	    }

		/// <summary>
		/// On enable, we reset the object's speed
		/// </summary>
	    protected virtual void OnEnable()
	    {
	        Speed = _initialSpeed;
	    }
		
		// On FixedUpdate(), we move the object based on the level's speed and the object's speed, and apply acceleration
		protected virtual void FixedUpdate ()
	    {
	        _movement = Vector3.left * (Speed / 10) * LevelManager.Instance.Speed * Time.fixedDeltaTime;
	        transform.Translate(_movement,Space.World);

	        // We apply the acceleration to increase the speed
			Speed += Acceleration * Time.fixedDeltaTime;
	    }
	}
}