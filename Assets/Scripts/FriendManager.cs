using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendManager : MonoBehaviour
{
    [SerializeField] private List<Friend> friends;
    [SerializeField] Transform friendMarker;

    private void Start()
    {
        friends = new List<Friend>();        
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
                newFriend.Followed = friendMarker; // On indique au premier ami trouvé qu'il doit suivre le joueur
            }
            else
            {
                // Les autres amis suivent le dernier ami trouvé
                newFriend.Followed = friends[friends.Count - 1].FriendMarker.transform;
            }
            friends.Add(newFriend);
        }
    }
}
