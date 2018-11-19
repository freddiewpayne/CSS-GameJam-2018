using System;
using UnityEngine;

public class MazeMaker : MonoBehaviour {

    public GameObject wall;
    public GameObject shootingEnemy;
    public int enemyCount;
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

        // Need to add enemies into maze 
        for(int i = 0; i < enemyCount; i++)
        {
            int roomIndex = (int) Math.Floor( (double) UnityEngine.Random.Range(0, MazeGenerator.roomCoords.Capacity) );

            // Get x and y in random square of the room 
            int x = (int) UnityEngine.Random.Range(0, MazeGenerator.roomCoords[0][2]);
            int y = (int) UnityEngine.Random.Range(0, MazeGenerator.roomCoords[0][2]);

            Instantiate(shootingEnemy, new Vector3(MazeGenerator.roomCoords[roomIndex][0] + 0.5f + x,
                                                   MazeGenerator.roomCoords[roomIndex][1] + 0.5f + y), shootingEnemy.transform.rotation );  
        }
    }
}
