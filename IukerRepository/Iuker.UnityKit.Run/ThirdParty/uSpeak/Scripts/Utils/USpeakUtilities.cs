using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Some helper utilities for working with USpeak
/// </summary>
public class USpeakUtilities : MonoBehaviour
{
	/// <summary>
	/// The path to the USpeaker Prefab, relative to a Resources folder
	/// </summary>
	public static string USpeakerPrefabPath = "USpeakerPrefab";

	/// <summary>
	/// Call this when a new player joins
	/// </summary>
	public static void PlayerJoined( string PlayerID )
	{
		GameObject uspeaker = (GameObject)Instantiate( Resources.Load( "USpeakerPrefab" ) );
		USpeakOwnerInfo ownerInfo = uspeaker.AddComponent<USpeakOwnerInfo>();

		USpeakPlayer player = new USpeakPlayer();
		player.PlayerID = PlayerID;

		ownerInfo.Init( player );
	}

	/// <summary>
	/// Call this when a player leaves
	/// </summary>
	public static void PlayerLeft( string PlayerID )
	{
		USpeakOwnerInfo.FindPlayerByID( PlayerID ).DeInit();
	}

	/// <summary>
	/// Call this when you first join a room, pass the IDs of every player already in the room
	/// </summary>
	public static void ListPlayers( IEnumerable<string> PlayerIDs )
	{
		foreach( string str in PlayerIDs )
			PlayerJoined( str );
	}

	/// <summary>
	/// Call this to clear out all players
	/// </summary>
	public static void Clear()
	{
		foreach( string id in USpeakOwnerInfo.USpeakPlayerMap.Keys )
		{
			USpeakOwnerInfo.USpeakPlayerMap[ id ].DeInit();
		}
	}
}