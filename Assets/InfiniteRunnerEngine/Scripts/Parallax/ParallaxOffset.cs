using UnityEngine;
using System.Collections;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Add this class to an object and it'll move in parallax based on the level's speed.
	/// This method moves the texture, not the object. It doesn't work for non-2D objects.
	/// </summary>
	public class ParallaxOffset : MonoBehaviour 
	{	
		/// the relative speed of the object
		public float Speed = 0;
		public static ParallaxOffset CurrentParallaxOffset;
		
		protected float _position = 0;

		/// <summary>
		/// On start, we store the current offset
		/// </summary>
	    protected virtual void Start () 
		{
			CurrentParallaxOffset=this;
		}

		/// <summary>
		/// On fixed update, we apply the offset to the texture
		/// </summary>
	    protected virtual void FixedUpdate()
		{
			// the new position is determined based on the level's speed and the object's speed
			_position += (Speed/300) * LevelManager.Instance.Speed * Time.fixedDeltaTime;
			// position reset
			if (_position > 1.0f)
			{
				_position -= 1.0f;
			}
			// we apply the offset to the object's texture
			GetComponent<Renderer>().material.mainTextureOffset = new Vector2(_position, 0);
		}
	}
}