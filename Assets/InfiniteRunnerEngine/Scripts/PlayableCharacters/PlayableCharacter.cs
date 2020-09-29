using UnityEngine;
using System.Collections;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// You should extend this class for all your playable characters
	/// The asset includes a bunch of examples of how you can do that : Jumper, Flappy, Dragon, etc...
	/// </summary>
	public class PlayableCharacter : MonoBehaviour 
	{
	    /// should we use the default mecanim ?
	    public bool UseDefaultMecanim=true;	
		/// returns true if the character is currently grounded
		public bool IsGrounded { get{ return _grounded; } }	
		// if true, the object will try to go back to its starting position
		public bool ShouldResetPosition = true;
		// the speed at which the object should try to go back to its starting position
		public float ResetPositionSpeed = 0.5f;	
		
		protected Vector3 _initialPosition;
		protected bool _grounded;
		protected RigidbodyInterface _rigidbodyInterface;
		protected Animator _animator;
		
		// Use this for initialization
		protected virtual void Awake () 
		{
			Initialize();
		}
		
		// this method initializes all essential elements
		protected virtual void Initialize()
		{
			_rigidbodyInterface = GetComponent<RigidbodyInterface> ();		
			_animator = GetComponent<Animator>();
			
			if (_rigidbodyInterface == null)
			{
				CorgiTools.DebugLogTime("You need a rigidbody interface");
				return;
			}
		}
		
		// use this to define the initial position of the agent. Used mainly for reset position purposes
		public virtual void SetInitialPosition(Vector3 initialPosition)
		{
			_initialPosition=initialPosition;	
		}
		
		// Update is called once per frame
		protected virtual void FixedUpdate ()
	    {
	        // we send our various states to the animator.      
	        UpdateAnimator ();

	        // if we're supposed to reset the player's position, we lerp its position to its initial position
	        ResetPosition();
		}
		
		/// <summary>
		/// This is called at Update() and sets each of the animators parameters to their corresponding State values
		/// </summary>
		protected virtual void UpdateAnimator()
		{
	        if (_animator== null)
	        { return;  }

	        // we send our various states to the animator.		
	        if (UseDefaultMecanim)
	        {
				UpdateAllMecanimAnimators();
	        }
	    }
	    
	    /// <summary>
	    /// Updates all mecanim animators.
	    /// </summary>
	    protected virtual void UpdateAllMecanimAnimators()
	    {		
			CorgiTools.UpdateAnimatorBool(_animator,"Grounded",IsGrounded);
			CorgiTools.UpdateAnimatorFloat(_animator, "VerticalSpeed", _rigidbodyInterface.Velocity.y);
	    }

		/// <summary>
		/// Called on fixed update, tries to return the object to its initial position
		/// </summary>
	    protected virtual void ResetPosition()
	    {
	        if (ShouldResetPosition)
	        {
	            if (IsGrounded)
	            { 
	                _rigidbodyInterface.Velocity = new Vector3((_initialPosition.x - transform.position.x) * (ResetPositionSpeed), _rigidbodyInterface.Velocity.y, _rigidbodyInterface.Velocity.z);
	            }
	        }
	    }
		
		/// <summary>
		/// What happens when the main action button button is pressed
		/// </summary>
		public virtual void MainActionStart() {	}
		/// <summary>
		/// What happens when the main action button button is released
		/// </summary>
	    public virtual void MainActionEnd() { }
	    /// <summary>
		/// What happens when the main action button button is being pressed
	    /// </summary>
	    public virtual void MainActionOngoing() { }
	    
		/// <summary>
		/// What happens when the down button is pressed
		/// </summary>
		public virtual void DownStart() { }
		/// <summary>
		/// What happens when the down button is released
		/// </summary>
		public virtual void DownEnd() { }
		/// <summary>
		/// What happens when the down button is being pressed
		/// </summary>
	    public virtual void DownOngoing() { }

		/// <summary>
		/// What happens when the up button is pressed
		/// </summary>
		public virtual void UpStart() { }
		/// <summary>
		/// What happens when the up button is released
		/// </summary>
		public virtual void UpEnd() { }
		/// <summary>
		/// What happens when the up button is being pressed
		/// </summary>
	    public virtual void UpOngoing() { }

		/// <summary>
		/// What happens when the left button is pressed
		/// </summary>
		public virtual void LeftStart() { }
		/// <summary>
		/// What happens when the left button is released
		/// </summary>
		public virtual void LeftEnd() { }
		/// <summary>
		/// What happens when the left button is being pressed
		/// </summary>
	    public virtual void LeftOngoing() { }

		/// <summary>
		/// What happens when the right button is pressed
		/// </summary>
		public virtual void RightStart() { }
		/// <summary>
		/// What happens when the right button is released
		/// </summary>
		public virtual void RightEnd() { }
		/// <summary>
		/// What happens when the right button is being pressed
		/// </summary>
	    public virtual void RightOngoing() { }


		/// <summary>
		/// Disables the playable character
		/// </summary>
	    public virtual void Disable()
		{
	        gameObject.SetActive(false);
	    }   

		/// <summary>
		/// What happens when the object gets killed
		/// </summary>
	    public virtual void Die()
		{
			Destroy(gameObject);
		}
	    
		/// <summary>
		/// Handles enter collision with 2D colliders
		/// </summary>
		/// <param name="collidingObject">Colliding object.</param>
		protected virtual void OnCollisionEnter2D (Collision2D collidingObject)
		{
			CollisionEnter (collidingObject.collider.gameObject);
		}

		/// <summary>
		/// Handles exit collision with 2D colliders
		/// </summary>
		/// <param name="collidingObject">Colliding object.</param>
		protected virtual void OnCollisionExit2D (Collision2D collidingObject)
		{
			CollisionExit (collidingObject.collider.gameObject);
		}

		/// <summary>
		/// Handles enter collision with 3D colliders 
		/// </summary>
		/// <param name="collidingObject">Colliding object.</param>
		protected virtual void OnCollisionEnter (Collision collidingObject)
		{		
			CollisionEnter (collidingObject.collider.gameObject);
		}

		/// <summary>
		/// Handles exit collision with 3D colliders
		/// </summary>
		/// <param name="collidingObject">Other.</param>
	    protected virtual void OnCollisionExit (Collision collidingObject)
		{		
			CollisionExit (collidingObject.collider.gameObject);
		}

		/// <summary>
		/// Detects when the objects touches the ground (or any object on the Ground layer)
		/// </summary>
		/// <param name="collidingObject">Colliding object.</param>
		protected virtual void CollisionEnter(GameObject collidingObject)
		{
			// if we're entering a collision with the ground
			if (collidingObject.layer == LayerMask.NameToLayer ("Ground")) 
			{
				if (collidingObject.transform.position.y <= transform.position.y)
				{
					_grounded = true;
				}
			}
		}
		
		/// <summary>
		/// Detects when the object leaves the ground
		/// </summary>
		/// <param name="collidingObject">Colliding object.</param>
		protected virtual void CollisionExit (GameObject collidingObject)
		{
			// if we're leaving the ground
			if (collidingObject.layer == LayerMask.NameToLayer("Ground"))
			{
				_grounded=false;
			}
		}
	}
}