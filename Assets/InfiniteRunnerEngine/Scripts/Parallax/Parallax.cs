using UnityEngine;
using System.Collections;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Add this class to an object so it'll move in parallax based on the level's speed.
	/// Note that this method duplicates the object
	/// </summary>
	public class Parallax : MonoBehaviour 
	{
		/// the relative speed of the object
		public float ParallaxSpeed=0;
		
		protected GameObject _clone;
		protected Vector3 _movement;
		protected Vector3 _initialPosition;
		protected float _width;

		/// <summary>
		/// On start, we store various variables and clone the object
		/// </summary>
	    protected virtual void Start()
		{
			_width = GetComponent<Renderer>().bounds.size.x;
			_initialPosition=transform.position	;	
		
			// we clone the object and position the clone at the end of the initial object
			_clone = (GameObject)Instantiate(gameObject, new Vector3(transform.position.x+_width, transform.position.y, transform.position.z), transform.rotation);
			// we remove the parallax component from the clone to prevent an infinite loop
			Parallax parallaxComponent = _clone.GetComponent<Parallax>();
			Destroy(parallaxComponent);		
		}

		/// <summary>
		/// On FixedUpdate, we move the object and its clone
		/// </summary>
	    protected virtual void FixedUpdate()
		{		
			// we determine the movement the object and its clone need to apply, based on their speed and the level's speed
	        if (LevelManager.Instance!= null)
	        { 
	    		_movement = Vector3.left * (ParallaxSpeed/10) * LevelManager.Instance.Speed * Time.fixedDeltaTime;
	        }
	        else
	        {
	            _movement = Vector3.left * (ParallaxSpeed / 10) * Time.fixedDeltaTime;
	        }
	        // we move both objects
	        _clone.transform.Translate(_movement);
			transform.Translate(_movement);
			
			// if the object has reached its left limit, we teleport both objects to the right
			if (transform.position.x +_width < _initialPosition.x)
			{
				transform.Translate(Vector3.right * _width);
				_clone.transform.Translate(Vector3.right * _width);
			}
		
		}
	}
}