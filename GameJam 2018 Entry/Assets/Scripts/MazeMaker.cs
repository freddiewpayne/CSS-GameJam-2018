using System;
using UnityEngine;

public class MazeMaker : MonoBehaviour {

    // Objects we're adding into the maze
    public GameObject wall;
    public GameObject shootingEnemy;
    public GameObject allDirectionsEnemy;
    public GameObject swordEnemy;
    public GameObject torch;
    public GameObject sword;
    public GameObject key;

    private int roomIndex;
    private int x;
    private int y;

    private bool keySpawned = false;

    public int enemyCount;
    private MazeGenerator mazeGenerator;

    // Maze is created on Awake 
    private void Awake()
    {
        // Need to set value of 'maze' here
        mazeGenerator = new MazeGenerator();

        // Need to turn string of hashes and spaces into a maze 
        for (int i = 0; i <= MazeGenerator.mazeSize; i++ )
        {
            for (int j = MazeGenerator.mazeSize; j >= 0; j--)
            {
                // Make a wall if we encounter a '#'
                if ( MazeGenerator.maze[i, j] == '#')
                {
                    Vector3 wallPos = new Vector3( (float) ( i + 0.5 ), (float) ( j + 0.37 ) );
                    Instantiate(wall, wallPos, wall.transform.rotation);
                }
            }
        }

        // Need to add enemies into maze 
        for(int i = 0; i < enemyCount; i++)
        {
            addObject(shootingEnemy);
            /*System.Random rand = new System.Random();
            int index = (int) rand.Next( 0, 3 );

            if (index == 0)
                addObject(shootingEnemy);
            else if (index == 1)
                addObject(swordEnemy);
            else
                addObject(allDirectionsEnemy);*/
        }

        // Add a torch into the maze 
        addObject(torch);
        // Add a sword
        addObject(sword);
    }

    private void addObject( GameObject toAdd )
    {
        roomIndex = (int)Math.Floor((double)UnityEngine.Random.Range(0, MazeGenerator.roomCoords.Capacity));
        x = (int)UnityEngine.Random.Range(0, MazeGenerator.roomCoords[roomIndex][2]);
        y = (int)UnityEngine.Random.Range(0, MazeGenerator.roomCoords[roomIndex][2]);
        Instantiate(toAdd, new Vector3(MazeGenerator.roomCoords[roomIndex][0] + 0.5f + x,
                                       MazeGenerator.roomCoords[roomIndex][1] + 0.5f + y), toAdd.transform.rotation);
    }

    private void Update()
    {
        if ( enemyCount <= PlayerController.Instance.enemiesKilled && !keySpawned )
        {
            roomIndex = (int)Math.Floor((double)UnityEngine.Random.Range(0, MazeGenerator.roomCoords.Capacity));
            Instantiate(key, new Vector3( MazeGenerator.roomCoords[roomIndex][0] + 0.5f, MazeGenerator.roomCoords[roomIndex][1] + 0.5f ), PlayerController.Instance.transform.rotation);
            keySpawned = true;
        }
    }
}
