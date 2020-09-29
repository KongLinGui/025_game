using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// This class handles the start screen.
	/// All it does is check for the player pressing the main action button, and if pressed, it loads the specified level
	/// </summary>
	public class StartScreen : MonoBehaviour
	{
	    /// the name of the next level. You have to make sure the name is the exact name of your scene.
	    public string NextLevelName;

		// singleton
	    static public StartScreen Instance { get { return _instance; } }
	    static protected StartScreen _instance;
	    public void Awake()
	    {
	        _instance = this;
	    }
		
		/// <summary>
		/// On each frame, we check for input
		/// </summary>
		protected virtual void Update () 
		{		
			if (Input.GetButtonDown("MainAction")) { GoToLevel(); }
		}

		/// <summary>
		/// Loads the level specified in parameters (usually called via MapButton.cs in our case)
		/// </summary>
		/// <param name="levelName">Level name.</param>
	    public virtual void GoToLevel()
	    {
			SceneManager.LoadScene(NextLevelName);
	    }    
	}
}
