using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridMath
{

    private static Dictionary<int[,], string> WallComparisonGrids = new Dictionary<int[,], string>()
    {

        // COVER
        {new int[,] {{2, 2, 2},
                     {2, 0, 2},
                     {2, 2, 2}}, "wall_cover"},

       
        // CORNERS 
        {new int[,] {{2, 2, 2},
                     {2, 0, 2},
                     {2, 2, 0}}, "wall_corner_top_left"},

        {new int[,] {{2, 2, 2},
                     {2, 0, 2},
                     {0, 2, 2}}, "wall_corner_top_right"},

        {new int[,] {{2, 2, 0},
                     {2, 0, 2},
                     {2, 2, 2}}, "wall_corner_bottom_left"},

        {new int[,] {{0, 2, 2},
                     {2, 0, 2},
                     {2, 2, 2}}, "wall_corner_bottom_right"},

                
        // PENINSULA
        {new int[,] {{0, 2, 0},
                     {2, 0, 2},
                     {2, 2, 2}}, "wall_bottom"},

        {new int[,] {{2, 2, 2},
                     {2, 0, 2},
                     {0, 2, 0}}, "wall_top"},

        {new int[,] {{0, 2, 2},
                     {2, 0, 2},
                     {0, 2, 2}}, "wall_right"},

        {new int[,] {{2, 2, 0},
                     {2, 0, 2},
                     {2, 2, 0}}, "wall_left"},


        // COVER
        {new int[,] {{0, 2, 0},
                     {2, 0, 2},
                     {0, 2, 0}}, "wall_cover"},

        
        // WALLS
        {new int[,] {{2, 2, 0},
                     {2, 0, 0},
                     {2, 2, 0}}, "wall_left"},

        {new int[,] {{0, 2, 2},
                     {0, 0, 2},
                     {0, 2, 2}}, "wall_right"},

        {new int[,] {{2, 2, 2},
                     {2, 0, 2},
                     {0, 0, 0}}, "wall_top"},

        {new int[,] {{0, 0, 0},
                     {2, 0, 2},
                     {2, 2, 2}}, "wall_bottom_grass"},

        
        // BENDS
        {new int[,] {{0, 0, 0},
                     {0, 0, 2},
                     {2, 2, 2}}, "wall_bend_top_left_grass"},

        {new int[,] {{0, 0, 0},
                     {2, 0, 0},
                     {2, 2, 2}}, "wall_bend_top_right_grass"},

        {new int[,] {{2, 2, 2},
                     {0, 0, 2},
                     {0, 0, 0}}, "wall_bend_bottom_left"},

        {new int[,] {{2, 2, 2},
                     {2, 0, 0},
                     {0, 0, 0}}, "wall_bend_bottom_right"},


        {new int[,] {{0, 0, 2},
                     {0, 0, 2},
                     {0, 2, 2}}, "wall_bend_top_left"},

        {new int[,] {{2, 0, 0},
                     {2, 0, 0},
                     {2, 2, 0}}, "wall_bend_top_right"},

        {new int[,] {{2, 2, 0},
                     {2, 0, 0},
                     {2, 0, 0}}, "wall_bend_bottom_right"},

        {new int[,] {{0, 2, 2},
                     {0, 0, 2},
                     {0, 0, 2}}, "wall_bend_bottom_left"},


        // GATEWAY EDGES
        {new int[,] {{0, 0, 0},
                     {0, 0, 2},
                     {0, 2, 2}}, "wall_bend_top_left_grass"},

        {new int[,] {{0, 0, 0},
                     {2, 0, 0},
                     {2, 2, 0}}, "wall_bend_top_right_grass"},

        {new int[,] {{0, 2, 2},
                     {0, 0, 2},
                     {0, 0, 0}}, "wall_bend_bottom_left"},

        {new int[,] {{2, 2, 0},
                     {2, 0, 0},
                     {0, 0, 0}}, "wall_bend_bottom_right"},
   
        
        // DEFAULT
        {new int[,] {{0, 0, 0},
                     {0, 0, 0},
                     {0, 0, 0}}, "wall"},


    };

    public static int[,] GetNeighbours(int[,] grid, Vector2Int loc, int outOfBoundsVal = 2)
    {
        int[,] neighbours = new int[3, 3];

        // top row
        try
        {
            neighbours[0, 0] = grid[loc.y + 1, loc.x - 1];
        }
        catch (System.IndexOutOfRangeException)
        {
            neighbours[0, 0] = outOfBoundsVal;
        }

        try
        {
            neighbours[0, 1] = grid[loc.y + 1, loc.x];
        }
        catch (System.IndexOutOfRangeException)
        {
            neighbours[0, 1] = outOfBoundsVal;
        }

        try
        {
            neighbours[0, 2] = grid[loc.y + 1, loc.x + 1];
        }
        catch (System.IndexOutOfRangeException)
        {
            neighbours[0, 2] = outOfBoundsVal;
        }

        // middle row
        try
        {
            neighbours[1, 0] = grid[loc.y, loc.x - 1];
        }
        catch (System.IndexOutOfRangeException)
        {
            neighbours[1, 0] = outOfBoundsVal;
        }

        try
        {
            neighbours[1, 1] = grid[loc.y, loc.x];
        }
        catch (System.IndexOutOfRangeException)
        {
            neighbours[1, 1] = outOfBoundsVal;
        }

        try
        {
            neighbours[1, 2] = grid[loc.y, loc.x + 1];
        }
        catch (System.IndexOutOfRangeException)
        {
            neighbours[1, 2] = outOfBoundsVal;
        }

        // bottom row
        try
        {
            neighbours[2, 0] = grid[loc.y - 1, loc.x - 1];
        }
        catch (System.IndexOutOfRangeException)
        {
            neighbours[2, 0] = outOfBoundsVal;
        }

        try
        {
            neighbours[2, 1] = grid[loc.y - 1, loc.x];
        }
        catch (System.IndexOutOfRangeException)
        {
            neighbours[2, 1] = outOfBoundsVal;
        }

        try
        {
            neighbours[2, 2] = grid[loc.y - 1, loc.x + 1];
        }
        catch (System.IndexOutOfRangeException)
        {
            neighbours[2, 2] = outOfBoundsVal;
        }
        
        return neighbours;
    }

    public static string WallComparison(int[,] grid)
    {

        bool skip = false;

        foreach (var wallComparisonGridPair in WallComparisonGrids)
        {
            int[,] comparisonGrid = wallComparisonGridPair.Key;
            skip = false;

            for (int y = 0; y < 3; y++)
            {

                if (skip) { break; }

                for (int x = 0; x < 3; x++)
                {
                
                    if (comparisonGrid[y, x] == 0) { continue; }
                    if (grid[y, x] != comparisonGrid[y, x]) 
                    { 
                        skip = true;
                        break; 
                    }
                }  
            }

            if (!skip) { return wallComparisonGridPair.Value; }

        }

        return null;
        
    }

    public static bool CheckSurroundedCardinal(int[,] neighbourGrid, int surroundingBlockType)
    {
        int empty = 87;
        
        int[,] surrounded = new int[3,3] {{empty, surroundingBlockType, empty}, {surroundingBlockType, empty, surroundingBlockType}, {empty, surroundingBlockType, empty}};

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if (surrounded[y, x] == empty) { continue; }
                if (surrounded[y, x] != neighbourGrid[y, x]) { return false; }
            }
        }

        return true;
    }

    public static bool CheckSurroundedFully(int[,] neighbourGrid, int surroundingBlockType)
    {
        int empty = 87;
        
        int[,] surrounded = new int[3,3] {{surroundingBlockType, surroundingBlockType, surroundingBlockType}, {surroundingBlockType, empty, surroundingBlockType}, {surroundingBlockType, surroundingBlockType, surroundingBlockType}};

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if (surrounded[y, x] == empty) { continue; }
                if (surrounded[y, x] != neighbourGrid[y, x]) { return false; }
            }
        }

        return true;
    }

    public static bool NeighboursContainsX(int[,] grid, int searchValue)
    {
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if (grid[y, x] == searchValue) { return true; }
            }
        }

        return false;
    }




    public static int PointDistance(Vector2Int A, Vector2Int B)
    {
        int distx = Mathf.Abs(A.x - B.x);
        int disty = Mathf.Abs(A.y - B.y);

        int dist = distx + disty;

        return dist;
    }

}
