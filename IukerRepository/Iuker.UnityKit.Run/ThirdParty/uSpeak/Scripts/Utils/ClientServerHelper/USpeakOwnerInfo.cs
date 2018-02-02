using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Placed on a USpeaker, will identify which USpeakPlayer owns it
/// </summary>
public class USpeakOwnerInfo : MonoBehaviour
{
	/// <summary>
	/// Provides a convenient mapping from USpeakPlayer -> USpeaker
	/// </summary>
	public static Dictionary<USpeakOwnerInfo, USpeaker> USpeakerMap = new Dictionary<USpeakOwnerInfo, USpeaker>();

	/// <summary>
	/// Provides a convenient mapping from Player ID -> USpeakOwnerInfo
	/// </summary>
	public static Dictionary<string, USpeakOwnerInfo> USpeakPlayerMap = new Dictionary<string, USpeakOwnerInfo>();

	/// <summary>
	/// The Owner of this USpeaker
	/// </summary>
	public USpeakPlayer Owner
	{
		get
		{
			return m_Owner;
		}
	}

	private USpeakPlayer m_Owner;

	/// <summary>
	/// Find a USpeakOwnerInfo by Player ID
	/// </summary>
	public static USpeakOwnerInfo FindPlayerByID( string PlayerID )
	{
		return USpeakPlayerMap[ PlayerID ];
	}

	/// <summary>
	/// Initialize with the given owner
	/// </summary>
	public void Init( USpeakPlayer owner )
	{
		this.m_Owner = owner;
		USpeakPlayerMap.Add( owner.PlayerID, this );
		USpeakerMap.Add( this, GetComponent<USpeaker>() );

		// we don't need scene loads destroying the game object... it will automatically be handled by players leaving
		DontDestroyOnLoad( gameObject );
	}

	/// <summary>
	/// Called when the player has left
	/// </summary>
	public void DeInit()
	{
		USpeakPlayerMap.Remove( m_Owner.PlayerID );
		USpeakerMap.Remove( this );

		Destroy( gameObject );
	}
}