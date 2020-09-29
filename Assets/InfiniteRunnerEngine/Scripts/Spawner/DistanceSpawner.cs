using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Spawns and positions/resizes objects based on the distance traveled 
	/// </summary>
	public class DistanceSpawner : Spawner 
	{
		[Header("Gap between objects")]
		/// the minimum gap bewteen two spawned objects
		public Vector3 MinimumGap ;
		/// the maximum gap between two spawned objects
		public Vector3 MaximumGap ;
		[Space(10)]	
		
		/// the x distance we spawn the player at
		public float SpawnDistanceFromPlayer;

	    [Space(10)]
	    [Header("Activity")]

	    protected Transform _lastSpawnedTransform;
		protected float _nextSpawnDistance;


	    /// <summary>
	    /// Triggered at the start of the level
	    /// </summary>
	    protected virtual void Start () 
		{
			/// we get the object pooler component
			_objectPooler = GetComponent<ObjectPooler> ();	
	        
	        //FirstSpawn();
	        	
		}

	    protected virtual void FirstSpawn()
	    {
	        /// we define the initial spaw position
			Vector2 spawnPosition = transform.position;
	        spawnPosition.x = transform.position.x + _nextSpawnDistance;
	        spawnPosition.y += Random.Range(MinimumGap.y, MaximumGap.y);

			DistanceSpawn(spawnPosition);	
	    }

	    /// <summary>
	    /// Triggered every frame
	    /// </summary>
	    protected virtual void FixedUpdate () 
		{
	        if (OnlySpawnWhileGameInProgress)
	        {
	            if (GameManager.Instance.Status != GameManager.GameStatus.GameInProgress)
	            {
	                _lastSpawnedTransform = null;
	                return ;
	            }
	        }


	        if ((_lastSpawnedTransform== null) || (!_lastSpawnedTransform.gameObject.activeInHierarchy))
	        {
	            FirstSpawn();
	        }

	        /// if we've reached the next spawn position, we spawn a new object
	        if (transform.position.x - _lastSpawnedTransform.position.x >= _nextSpawnDistance) 
			{
				/// we reposition the object
				Vector3 spawnPosition = transform.position;		
				spawnPosition.x = _lastSpawnedTransform.position.x + _nextSpawnDistance;
				spawnPosition.y += Random.Range(MinimumGap.y, MaximumGap.y);
			
				DistanceSpawn(spawnPosition);					
			}		
		}
		
		/// <summary>
		/// Spawns an object at the specified position and determines the next spawn position
		/// </summary>
		/// <param name="spawnPosition">Spawn position.</param>
		protected virtual void DistanceSpawn(Vector3 spawnPosition)
		{
			GameObject spawnedObject = Spawn(spawnPosition);
			
			if (spawnedObject==null)
			{
				if (_lastSpawnedTransform==null)
				{
					_lastSpawnedTransform = this.transform;
				}
				_nextSpawnDistance = Random.Range(MinimumGap.x, MaximumGap.x) ;
			}
			else
			{
				_lastSpawnedTransform = spawnedObject.transform;
				// we define the next spawn position based on the size of the current object and the specified gaps		
				_nextSpawnDistance = Random.Range(MinimumGap.x, MaximumGap.x) + spawnedObject.GetComponent<Renderer>().bounds.size.x / 2;
			}
		}

	    /// <summary>
	    /// Draws on the scene view cubes to show the minimum and maximum gaps, for tweaking purposes.
	    /// </summary>
	    protected override void OnDrawGizmosSelected()
	    {
	        DrawClamps();
	        Gizmos.color = Color.yellow;
	        Gizmos.DrawWireCube(new Vector3(transform.position.x - SpawnDistanceFromPlayer + MinimumGap.x/2, transform.position.y, transform.position.z), MinimumGap);
	        Gizmos.color = Color.red;
	        Gizmos.DrawWireCube(new Vector3(transform.position.x - SpawnDistanceFromPlayer + MaximumGap.x / 2, transform.position.y, transform.position.z), MaximumGap);
	    }
	}
}