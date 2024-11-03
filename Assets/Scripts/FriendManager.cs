using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendManager : MonoBehaviour
{
    // Manage the collection of friends the player can find and have them follow in the the game

    [SerializeField] private List<Friend> friends; // List that keep track of all the friends found
    [SerializeField] Transform friendMarker; // Position that must follow the new found friend (only the first one)
    [SerializeField] public int NumberFriends { get; private set; }

    private void Start()
    {
        friends = new List<Friend>();
        NumberFriends = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player trigger a collision with a friend
        if(collision.gameObject.tag == "Friend")
        {
            SoundManager.Instance.PlaySound("Friend");
            Friend newFriend = collision.gameObject.GetComponent<Friend>();

            // We don't want the player to trigger a collision again with him
            newFriend.gameObject.GetComponent<CircleCollider2D>().enabled = false; 

            if(friends.Count == 0)
            {
                newFriend.Followed = friendMarker; // Set the first friend to follow the player
            }
            else
            {
                // The other friends must follow the last friend found
                newFriend.Followed = friends[friends.Count - 1].FriendMarker.transform;
            }
            friends.Add(newFriend);
            NumberFriends++; // Update the number of friends found
        }
    }
}
