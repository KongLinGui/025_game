﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// The game manager is a persistent singleton that handles points and time
	/// </summary>
	public class GameManager : MonoBehaviour
	{			
		/// the number of lives the player gets (you lose a life when your character (or your characters all) die.
		/// lose all lives you lose the game and your points.
		public int TotalLives = 3;
	    /// The current number of lives
	    public int CurrentLives { get; protected set;  }
		/// the list of playable characters in the game
		public List<PlayableCharacter> CurrentPlayableCharacters { get; set; }
		/// the current number of game points
		public float Points { get; protected set; }
		/// the current time scale
		public float TimeScale;
	    /// the various states the game can be in
	    public enum GameStatus { BeforeGameStart, GameInProgress, Paused, GameOver, LifeLost };
	    /// the current status of the game
	    public GameStatus Status{ get; protected set; }

	    public delegate void GameManagerInspectorRedraw();
	    // Declare the event to which editor code will hook itself.
	    public event GameManagerInspectorRedraw GameManagerInspectorNeedRedraw;

	    // storage
	    protected float _savedTimeScale;
	    protected IEnumerator _scoreCoroutine;
	    protected float _pointsPerSecond;
	    
		// singleton pattern
		static public GameManager Instance { get { return _instance; } }
		static protected GameManager _instance;
		public void Awake()
		{
			_instance = this;        
	    }

		/// <summary>
		/// Initialization
		/// </summary>
	    protected virtual void Start()
		{
	        CurrentLives = TotalLives;
	        _savedTimeScale = TimeScale;
	        Time.timeScale = TimeScale;
	        GUIManager.Instance.Initialize();
	    }

	    public virtual void SetPointsPerSecond(float newPointsPerSecond)
	    {
	        _pointsPerSecond = newPointsPerSecond;
	    }
		
		/// <summary>
		/// Sets the status. Status can be accessed by other classes to check if the game is paused, starting, etc
		/// </summary>
		/// <param name="newStatus">New status.</param>
		public virtual void SetStatus(GameStatus newStatus)
		{
			Status=newStatus;
	        if (GameManagerInspectorNeedRedraw != null) { GameManagerInspectorNeedRedraw(); }
	    }
				
		/// <summary>
		/// this method resets the whole game manager
		/// </summary>
		public virtual void Reset()
		{
			Points = 0;
			TimeScale = 1f;
			GameManager.Instance.SetStatus(GameStatus.GameInProgress);
			EventManager.TriggerGameStart();
			GUIManager.Instance.RefreshPoints ();
		}

		/// <summary>
		/// Starts or stops the autoincrement of the score
		/// </summary>
		/// <param name="status">If set to <c>true</c> autoincrements the score, if set to false, stops the autoincrementation.</param>
	    public virtual void AutoIncrementScore(bool status)
	    {
	        if (status)
	        {
	            StartCoroutine(IncrementScore());
	        }
	        else
	        {
	            StopCoroutine(IncrementScore());
	        }
	    }

		/// <summary>
		/// Each 0.01 second, increments the score by 1/100th of the number of points it's supposed to increase each second
		/// </summary>
		/// <returns>The score.</returns>
	    protected virtual IEnumerator IncrementScore()
		{
	        while (true)
	        {
	            if (GameManager.Instance.Status == GameStatus.GameInProgress)
	            {
	                AddPoints(_pointsPerSecond / 100);
	            }
	            yield return new WaitForSeconds(0.01f);
	        }
	    }
		
		
		/// <summary>
		/// Adds the points in parameters to the current game points.
		/// </summary>
		/// <param name="pointsToAdd">Points to add.</param>
		public virtual void AddPoints(float pointsToAdd)
		{
			Points += pointsToAdd;
			GUIManager.Instance.RefreshPoints ();
		}
		
		/// <summary>
		/// use this to set the current points to the one you pass as a parameter
		/// </summary>
		/// <param name="points">Points.</param>
		public virtual void SetPoints(float points)
		{
			Points = points;
			GUIManager.Instance.RefreshPoints ();
		}

		/// <summary>
		/// use this to set the number of lives currently available
		/// </summary>
		/// <param name="lives">the new number of lives.</param>
	    public virtual void SetLives(int lives)
	    {
	        CurrentLives = lives;
	        GUIManager.Instance.InitializeLives();
	    }
	    
	    /// <summary>
	    /// use this to remove lives from the current amount
	    /// </summary>
	    /// <param name="lives">the number of lives you want to lose.</param>
	    public virtual void LoseLives(int lives)
	    {
	        CurrentLives -= lives;
	        GUIManager.Instance.InitializeLives();
	    }

	    /// <summary>
	    /// sets the timescale to the one in parameters
	    /// </summary>
	    /// <param name="newTimeScale">New time scale.</param>
	    public virtual void SetTimeScale(float newTimeScale)
		{
			_savedTimeScale = Time.timeScale;
			Time.timeScale = newTimeScale;
		}
		
		/// <summary>
		/// Resets the time scale to the last saved time scale.
		/// </summary>
		public virtual void ResetTimeScale()
		{
			Time.timeScale = _savedTimeScale;
		}
		
		/// <summary>
		/// Pauses the game
		/// </summary>
		public virtual void Pause()
		{
			// if time is not already stopped		
			if (Time.timeScale>0.0f)
			{
				Instance.SetTimeScale(0.0f);
				Instance.SetStatus(GameStatus.Paused);
				GUIManager.Instance.SetPause(true);
			}
			else
			{
	            UnPause();	
			}		
		}

	    /// <summary>
	    /// Unpauses the game
	    /// </summary>
	    public virtual void UnPause()
	    {
	        Instance.ResetTimeScale();
	        Instance.SetStatus(GameStatus.GameInProgress);
	        GUIManager.Instance.SetPause(false);
	    }
	}
}