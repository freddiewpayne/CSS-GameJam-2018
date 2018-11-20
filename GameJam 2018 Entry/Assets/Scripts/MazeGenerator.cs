using System;
using System.Collections.Generic;

class MazeGenerator
{
    private static int inp = 20;
    public static int mazeSize = inp;
    private static int endX = -1;
    private static int endY = -1;
    private static int endXHard = -1;
    private static int endYHard = -1;

    private static int formEndX;
    private static int formEndY;
    private static int formEndXHard;
    private static int formEndYHard;

    private static bool endDone = false;
    private static int firstMove = -1;

    public static List<int[]> roomCoords;

    // The actual maze
    public static char[,] maze = new char[mazeSize + 1, mazeSize + 1];

    private static char[,] maze2 = new char[mazeSize + 1, mazeSize + 1];
    private static int split = 0;

    private static int strtX = 1;
    private static int strtY = 1;

    public MazeGenerator()
    {
        roomCoords = new List<int[]>();
        giveMaze();
    }

    public static void giveMaze()
    {
        mazeInit();
        for (int i = 0; i < mazeSize / 5; i++)
        {
            roomGen((mazeSize/5) - i);
        }
        List<int> past = new List<int>();            // stores all past directions moved
        endX = -1;                                   // allows endx and endy to be updated
        endXHard = -1;
        maze[strtX, strtY] = '-';
        genMaze(strtX, strtY, maze, inp, past, strtX, strtY);  // generates maze
        maze[strtX, strtY] = ' ';
    }

    public static void roomGen(int size)
    {
        int rand;
        int placeX;
        int placeY;
        int n = 0;
        System.Random ran = new Random();
        do
        {
            rand = System.Convert.ToInt32(Math.Floor((double)(size * (ran.Next(0, 100) / (double)100)))) * 2 + 3;
            placeX = (System.Convert.ToInt32(Math.Floor((double)((mazeSize - rand - 1) * (ran.Next(0,100) / (double)100)))) / 2) * 2 + 3;
            placeY = (System.Convert.ToInt32(Math.Floor((double)((mazeSize - rand - 1) * (ran.Next(0, 100) / (double) 100)))) / 2) * 2 + 3;
            n++;
        }
        while (colliding(placeX, placeY, rand) && n < 2000);

        int[] temp = new int[3] { placeX, placeY, rand };
        roomCoords.Add( temp );

        for (int i = placeX; i <= placeX + rand - 1; i++)
        {
            for (int j = placeY; j <= placeY + rand - 1; j++)
            {
                maze[i, j] = 'k';
            }
        }

        int x1 = 1;
        int y1 = 1;
        int x2 = 1;
        int y2 = 1;
        if(placeX + rand + 2 > mazeSize)
        {
            x1 = placeX - 1;
            y1 = placeY + rand / 2;
            x2 = placeX + (rand / 2) + 1;
            y2 = placeY - 2;
        }
        else
        {
            x1 = placeX - 1;
            y1 = placeY + rand / 2;
            x2 = placeX + rand;
            y2 = placeY + rand / 2;
        }
        maze[x1, y1 - 1] = ' ';
        maze[x2, y2 + 1] = ' ';
    }

    public static bool colliding(int placex, int placey, int width)
    {
        int placeX = placex - 2;
        int placeY = placey - 2;

        int endxs = placex + width + 1;
        int endys = placey + width + 1;

        if (placeX < 0)
        {
            placeX = 0;
        }
        if(placeY < 0)
        {
            placeY = 0;
        }
        if(endxs > mazeSize)
        {
            endxs = mazeSize;
        }
        if (endys > mazeSize)
        {
            endys = mazeSize;
        }

        for (int i = placeX; i <= endxs; i++)
        {
            for (int j = placeY; j <= endys; j++)
            {
                if (maze[i, j] == 'k')
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static void mazeInit()
    {
        for (int i = 0; i <= mazeSize; i++)
        {
            for (int j = 0; j <= mazeSize; j++)
                maze[i, j] = '#';
        }
    }

    private static void shuffle(ref int[] arr)
    {
        int rand;
        int temp;
        System.Random ran = new Random();
        for (int k = 0; k <= 3; k++)
        {
            rand = System.Convert.ToInt32(Math.Floor((double)((4) * (ran.Next(0, 1)) / (double)100)));
            temp = arr[k];
            arr[k] = arr[rand];
            arr[rand] = temp;
        }
    }

    private static void forward(int changeX, int changeY, int plusX, int plusY, ref int curr, int change, ref bool done, ref List<int> past, int num)
    {
        if (maze[changeX, changeY] == '#')
        {
            maze[changeX + plusX, changeY + plusY] = '-';
            maze[changeX, changeY] = '-';
            curr += change;
            done = true;
            endDone = true;
            past.Add(num);
        }
    }

    private static void back(int currX, int currY, int xChange, int yChange, ref int toUpdate, int num)
    {
        maze[currX + xChange, currY + yChange] = ' ';
        maze[currX, currY] = ' ';
        toUpdate += num;
    }

    private static void genMaze(int currX, int currY, char[,] maze, int size, List<int> past, int strtX, int strtY)
    {
        bool valid = true;
        int[] dir = new[] { 1, 2, 3, 4 };
        int itr = 0;
        bool done = false;
        endDone = false;

        do
        {
            shuffle(ref dir);
            done = false;
            itr = 0;
            while (itr < 4 & done == false)
            {
                switch (dir[itr])
                {
                    case 1:
                        {
                            if (currX - 2 > 0)
                                forward(currX - 2, currY, 1, 0, ref currX, -2, ref done, ref past, 4);
                            break;
                        }

                    case 4:
                        {
                            if (currX + 2 < size)
                                forward(currX + 2, currY, -1, 0, ref currX, +2, ref done, ref past, 1);
                            break;
                        }

                    case 3:
                        {
                            if (currY - 2 > 0)
                                forward(currX, currY - 2, 0, 1, ref currY, -2, ref done, ref past, 2);
                            break;
                        }

                    case 2:
                        {
                            if (currY + 2 < size)
                                forward(currX, currY + 2, 0, -1, ref currY, 2, ref done, ref past, 3);
                            break;
                        }
                }

                itr += 1;
            }

            if (itr == 4 & done == false)
                valid = false;
            if (itr < 4 & firstMove == -1)
                firstMove = dir[itr - 1];
        }
        while (!valid == false)// up// down// left// right
;

        if (endX == -1)
        {
            endX = ((currY + 1) / 2);
            endY = ((currX + 1) / 2);
            formEndX = ((endX - 1) * split) + 5;
            formEndY = ((endY - 1) * split) + 5;
            formEndXHard = formEndX;                 // This prevents false end in case of a labyrinth(only one finish)
            formEndYHard = formEndY;
        }
        else if (endXHard == -1 & endDone == true)
        {
            endXHard = ((currY + 1) / 2);
            endYHard = ((currX + 1) / 2);
            formEndXHard = ((endXHard - 1) * split) + 5;
            formEndYHard = ((endYHard - 1) * split) + 5;
        }

        if (currX != strtX | currY != strtY)
        {
            switch (past[past.Count - 1])
            {
                case 1 // up
               :
                    {
                        back(currX, currY, -1, 0, ref currX, -2);
                        break;
                    }

                case 4 // down
         :
                    {
                        back(currX, currY, 1, 0, ref currX, 2);
                        break;
                    }

                case 3 // left
         :
                    {
                        back(currX, currY, 0, -1, ref currY, -2);
                        break;
                    }

                case 2 // right
         :
                    {
                        back(currX, currY, 0, 1, ref currY, 2);
                        break;
                    }
            }
            past.RemoveAt(past.Count - 1);

            genMaze(currX, currY, maze, size, past, strtX, strtY);
        }
    }
}
