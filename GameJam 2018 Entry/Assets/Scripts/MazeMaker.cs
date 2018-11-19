using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMaker : MonoBehaviour {

    public GameObject wall;
    public int size;
    private char[][] maze;

    // Maze is created on Awake 
    private void Awake()
    {
        // Need to set value of 'maze' here

        // Need to turn string of hashes and spaces into a maze 
        for ( int i = 0; i < size; i ++ )
        {
            for (int j = 0; j < size; j++)
            {
                // Make a wall if we encounter a '#'
                if (maze[i][j] == '#')
                {
                    Vector3 wallPos = new Vector3( (float) ( i + 0.5 ), (float) ( j + 0.5 ) );
                    Instantiate(wall, wallPos, wall.transform.rotation);
                }
            }
        }

    }

}
