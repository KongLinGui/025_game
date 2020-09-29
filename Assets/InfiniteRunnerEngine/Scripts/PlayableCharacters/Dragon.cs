using UnityEngine;
using System.Collections;

namespace MoreMountains.InfiniteRunnerEngine
{
	/// <summary>
	/// Extends playable character to implement the specific gameplay of the Dragon level
	/// </summary>
	public class Dragon : Flappy
	{
		/// the flame explosion triggered by the dragon at each jump
	    public GameObject Flame;
		/// the explosion that happens when the dragon hits the ground
	    public GameObject Explosion;

	    protected Animator _flameAnimator;
	    protected Animator _explosionAnimator;
	    protected CameraBehavior _camera;
	    protected Renderer _renderer;

		/// <summary>
		/// On awake, we initialize our stuff
		/// </summary>
	    protected override void Awake()
	    {
	        Initialize();
	        _renderer = GetComponent<Renderer>();
	        _rigidbodyInterface.IsKinematic(true);
	        _flameAnimator = Flame.GetComponent<Animator>();
	        _explosionAnimator = Explosion.GetComponent<Animator>();
	        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehavior>();
	    }

		/// <summary>
		/// On start, we make our dragon flicker
		/// </summary>
	    protected virtual void Start()
	    {
	        Color flickerColor = new Color(1, 1, 1, 0.5f);
	        StartCoroutine(CorgiTools.Flicker(GameManager.Instance.CurrentPlayableCharacters[0].GetComponent<Renderer>(), flickerColor, 0.1f, 3f));
	    }

		/// <summary>
		/// Updates all mecanim animators.
		/// </summary>
	    protected override void UpdateAllMecanimAnimators()
	    {
	        CorgiTools.UpdateAnimatorBool(_animator, "Grounded", IsGrounded);
	        CorgiTools.UpdateAnimatorFloat(_animator, "VerticalSpeed", _rigidbodyInterface.Velocity.y);
	        CorgiTools.UpdateAnimatorBool(_animator, "Jumping", _jumping);
	        CorgiTools.UpdateAnimatorBool(_flameAnimator, "Jumping", _jumping);
	    }

		/// <summary>
		/// On fixed update
		/// </summary>
	    protected override void FixedUpdate()
	    {
	        // we send our various states to the animator.      
	        UpdateAnimator();
	        // if jumping is true, we've just passed this info to the animator and reset it.
	        if (_jumping) { _jumping = false; }

			// if the dragon becomes grounded, we instantiate an explosion and kill it
	        if (IsGrounded)
	        {
	            // we shake the camera
	            //Vector3 ShakeParameters = new Vector3(0.3f, 0.2f, 0.3f);
	            //_camera.Shake(ShakeParameters);

	            GameObject explosion = (GameObject)Instantiate(Explosion);
	            explosion.transform.position = transform.GetComponent<Renderer>().bounds.center+1*Vector3.down;
	            CorgiTools.UpdateAnimatorBool(explosion.GetComponent<Animator>(), "Grounded", _grounded);

	            LevelManager.Instance.KillCharacter(this);
	        }


	        // if we're supposed to reset the player's position, we lerp its position to its initial position
	        ResetPosition();
		}
	}
}
