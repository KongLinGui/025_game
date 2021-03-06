﻿using UnityEngine;
using System.Collections;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// This class acts as an interface to allow the demo levels to work whether the environment (colliders, rigidbodies) are set as 2D or 3D.
	/// If you already know for sure that you're going for a 2D or 3D game, I suggest you replace the use of this class with the appropriate classes.
	/// </summary>
	public class RigidbodyInterface : MonoBehaviour 
	{	
	    public Vector3 position
	    {
	        get
	        {
	            if (_rigidbody2D != null)
	            {
	                return _rigidbody2D.position;
	            }
	            if (_rigidbody != null)
	            {
	                return _rigidbody.position;
	            }
	            return Vector3.zero;
	        }
	        set { }
	    }

		/// <summary>
		/// Initialization
		/// </summary>
		protected virtual void Awake () 
		{
			// we check for rigidbodies, and depending on their presence determine if the interface will work with 2D or 3D rigidbodies and colliders.
			_rigidbody2D=GetComponent<Rigidbody2D>();
			_rigidbody=GetComponent<Rigidbody>();
			
			if (_rigidbody2D != null) 
			{
				_mode="2D";
			}
			if (_rigidbody != null) 
			{
				_mode="3D";
			}
			//TODO add an assertion if there's no rigidbody
		}

		/// <summary>
		/// Gets or sets the velocity of the rigidbody associated to the interface.
		/// </summary>
		/// <value>The velocity.</value>
		public Vector3 Velocity 
		{
			get 
			{ 
				if (_mode == "2D") 
				{
					return(_rigidbody2D.velocity);
				}
				else 
				{
					if (_mode == "3D") 
					{
						return(_rigidbody.velocity);
					}
					else
					{
						return new Vector3(0,0,0);
					}
				}
			}
			set 
			{
				if (_mode == "2D") {
					_rigidbody2D.velocity = value;
				}
				if (_mode == "3D") {
					_rigidbody.velocity = value;
				}
			}
		}

		protected string _mode;
		protected Rigidbody2D _rigidbody2D;
		protected Rigidbody _rigidbody;
		
		/// <summary>
		/// Adds the specified force to the rigidbody associated to the interface..
		/// </summary>
		/// <param name="force">Force.</param>
		public virtual void AddForce(Vector3 force)
		{
			if (_mode == "2D") 
			{
				_rigidbody2D.AddForce(force,ForceMode2D.Impulse);
			}
			if (_mode == "3D")
			{
				_rigidbody.AddForce(force);
			}
		}

	    /// <summary>
	    /// Move the rigidbody to the position vector specified
	    /// </summary>
	    /// <param name="newPosition"></param>
	    public virtual void MovePosition(Vector3 newPosition)
	    {
	        if (_mode == "2D")
	        {
	            _rigidbody2D.MovePosition(newPosition);
	        }
	        if (_mode == "3D")
	        {
	            _rigidbody.MovePosition(newPosition);
	        }
	    }
			
		
		/// <summary>
		/// Determines whether the rigidbody associated to the interface is kinematic
		/// </summary>
		/// <returns><c>true</c> if this instance is kinematic the specified status; otherwise, <c>false</c>.</returns>
		/// <param name="status">If set to <c>true</c> status.</param>
		public virtual void IsKinematic(bool status)
		{
			if (_mode == "2D") 
			{
				GetComponent<Rigidbody2D>().isKinematic=status;
			}
			if (_mode == "3D")
			{			
				GetComponent<Rigidbody>().isKinematic=status;
			}
		}
		
		/// <summary>
		/// Enables the box collider associated to the interface.
		/// </summary>
		/// <param name="status">If set to <c>true</c> status.</param>
		public virtual void EnableBoxCollider(bool status)
		{
			if (_mode == "2D") 
			{
				GetComponent<Collider2D>().enabled=status;
			}
			if (_mode == "3D")
			{			
				GetComponent<Collider>().enabled=status;
			}
		}
	}
}
