﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{

  // singleton
  public static LevelSettings Instance;
  void Awake()
  {
    Instance = this;
  }

  // player stats
  public bool canMove = true; // if player can use input to influence movement of character
  public bool canJump = true; // if player can jump by key
  public bool canMorph = true; // if player can change the form of the character
  public bool isDead = false; // if the player is currently dead

  // world stats
  public Vector2 worldSpawn;
  public Vector2 playerSpawn;

  // objects
  public GameObject playerObject;

  private void Start()
  {

    // set spawn points at beginning to location of player object on entry in level
    worldSpawn = playerObject.transform.localPosition;
    playerSpawn = playerObject.transform.localPosition;

  }

}
