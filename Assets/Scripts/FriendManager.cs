using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendManager : MonoBehaviour
{
    [SerializeField] private List<Friend> friends;

    private void Start()
    {
        friends = new List<Friend>();
        //friends.Add()
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Friend")
        {
            Friend newFriend = collision.gameObject.GetComponent<Friend>();
            // On ne veut plus que le joueur entre en collision avec celui-ci
            newFriend.gameObject.GetComponent<CircleCollider2D>().enabled = false; 
            if(friends.Count == 0)
            {
                newFriend.Followed = this.transform;
            }
            else
            {
                //newFriend.Followed = friends
            }
            friends.Add(newFriend);
        }
    }
}
