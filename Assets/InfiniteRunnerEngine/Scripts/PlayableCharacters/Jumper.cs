using UnityEngine;
using System.Collections;

namespace MoreMountains.InfiniteRunnerEngine
{	
	public class Jumper : PlayableCharacter 
	{
		/// the vertical force applied to the character when jumping
		public float JumpForce = 20f;
		/// the number of jumps allowed
		public int NumberOfJumpsAllowed=2;
	    /// the minimum time (in seconds) allowed between two consecutive jumps
	    public float CooldownBetweenJumps = 0f;
		/// can the character jump only when grounded ?
		public bool JumpsAllowedWhenGroundedOnly;
		/// the speed at which the character falls back down again when the jump button is released
		public float JumpReleaseSpeed = 50f; 
			
		protected int _numberOfJumpsLeft;
		protected bool _jumping=false;
	    protected float _lastJumpTime;
		
		/// <summary>
		/// On fixed update, we update the animator and try to reset the jumper's position
		/// </summary>
		protected override void FixedUpdate ()
		{
			_jumping = false;
			
			// we send our various states to the animator.      
			UpdateAnimator ();		
			// if we're supposed to reset the player's position, we lerp its position to its initial position
			ResetPosition();
		}
		
		/// <summary>
		/// Updates all mecanim animators.
		/// </summary>
		protected override void UpdateAllMecanimAnimators()
		{		
			CorgiTools.UpdateAnimatorBool(_animator,"Grounded",IsGrounded);
			CorgiTools.UpdateAnimatorBool(_animator, "Jumping", _jumping);
			CorgiTools.UpdateAnimatorFloat(_animator, "VerticalSpeed", _rigidbodyInterface.Velocity.y);
		}
		
		/// <summary>
		/// What happens when the main action button button is pressed
		/// </summary>
		public override void MainActionStart()
		{		

			// if the character is not grounded and is only allowed to jump when grounded, we do nothing
			if (JumpsAllowedWhenGroundedOnly && !IsGrounded)
			{
				return;
			}
			
			// if the character doesn't have any jump left, we do nothing
			if (_numberOfJumpsLeft==0)
			{
				return;
			}

	        // if we're still in cooldown from the last jump
	        if (Time.time - _lastJumpTime < CooldownBetweenJumps)
	        {
	            return;
	        }
			
			// we jump and decrease the number of jumps left
			_numberOfJumpsLeft--;
			
			// if the character is falling down, we reset its velocity
			if (_rigidbodyInterface.Velocity.y < 0)
			{
				_rigidbodyInterface.Velocity = Vector3.zero;
			}
			
			// we make our character jump
			_rigidbodyInterface.AddForce(Vector3.up * JumpForce);

	        _lastJumpTime = Time.time;
	        _jumping =true;
		}
		
		/// <summary>
		/// What happens when the main action button button is released
		/// </summary>
		public override void MainActionEnd()
		{
			// we initiate the descent
			StartCoroutine(JumpSlow());
		}
		
		/// <summary>
		/// Slows the player's jump
		/// </summary>
		/// <returns>The slow.</returns>
		public virtual IEnumerator JumpSlow()
		{
			while (_rigidbodyInterface.Velocity.y > 0)
			{			
				_rigidbodyInterface.Velocity = Vector3.up * (_rigidbodyInterface.Velocity.y - JumpReleaseSpeed * Time.deltaTime);
				yield return 0;
			}
		}
		
		/// <summary>
		/// Detects when the objects touches the ground (or any object on the Ground layer)
		/// </summary>
		/// <param name="collidingObject">Colliding object.</param>
		protected override void CollisionEnter(GameObject collidingObject)
		{
			// if we're entering a collision with the ground
			if (collidingObject.layer == LayerMask.NameToLayer ("Ground")) 
			{
				if (collidingObject.transform.position.y <= transform.position.y)
				{
					// we reset our jump variables
					_grounded = true;
					_jumping = false;
					_numberOfJumpsLeft = NumberOfJumpsAllowed;
				}
			}
		}		
	}
}