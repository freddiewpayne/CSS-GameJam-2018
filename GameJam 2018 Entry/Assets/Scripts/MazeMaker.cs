using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMaker : MonoBehaviour {

    public GameObject wall;
    private MazeGenerator mazeGenerator;

    // Maze is created on Awake 
    private void Awake()
    {
        // Need to set value of 'maze' here
        mazeGenerator = new MazeGenerator();

        // Need to turn string of hashes and spaces into a maze 
        for ( int i = 0; i < MazeGenerator.mazeSize + 1; i ++ )
        {
            for (int j = 0; j < MazeGenerator.mazeSize + 1; j++)
            {
                // Make a wall if we encounter a '#'
                if ( MazeGenerator.maze[i, j] == '#')
                {
                    Vector3 wallPos = new Vector3( (float) ( i + 0.5 ), (float) ( j + 0.5 ) );
                    Instantiate(wall, wallPos, wall.transform.rotation);
                }
            }
        }

    }

}
