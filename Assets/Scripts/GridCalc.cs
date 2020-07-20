using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridCalc
{

    private static Dictionary<int[,], string> GridTypes = new Dictionary<int[,], string>()
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
                     {2, 2, 2}}, "wall_bottom"},

        
        // BENDS
        {new int[,] {{0, 0, 0},
                     {0, 0, 2},
                     {2, 2, 2}}, "wall_bend_top_left"},

        {new int[,] {{0, 0, 0},
                     {2, 0, 0},
                     {2, 2, 2}}, "wall_bend_top_right"},

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
   
        
        // DEFAULT
        {new int[,] {{0, 0, 0},
                     {0, 0, 0},
                     {0, 0, 0}}, "wall"},


    };

    public static int[,] GetNeighbours(int[,] grid, Vector2Int loc)
    {
        int[,] neighbours = new int[3, 3];

        // top row
        try
        {
            neighbours[0, 0] = grid[loc.y + 1, loc.x - 1];
        }
        catch (System.IndexOutOfRangeException)
        {
            neighbours[0, 0] = 2;
        }

        try
        {
            neighbours[0, 1] = grid[loc.y + 1, loc.x];
        }
        catch (System.IndexOutOfRangeException)
        {
            neighbours[0, 1] = 2;
        }

        try
        {
            neighbours[0, 2] = grid[loc.y + 1, loc.x + 1];
        }
        catch (System.IndexOutOfRangeException)
        {
            neighbours[0, 2] = 2;
        }

        // middle row
        try
        {
            neighbours[1, 0] = grid[loc.y, loc.x - 1];
        }
        catch (System.IndexOutOfRangeException)
        {
            neighbours[1, 0] = 2;
        }

        try
        {
            neighbours[1, 1] = grid[loc.y, loc.x];
        }
        catch (System.IndexOutOfRangeException)
        {
            neighbours[1, 1] = 2;
        }

        try
        {
            neighbours[1, 2] = grid[loc.y, loc.x + 1];
        }
        catch (System.IndexOutOfRangeException)
        {
            neighbours[1, 2] = 2;
        }

        // bottom row
        try
        {
            neighbours[2, 0] = grid[loc.y - 1, loc.x - 1];
        }
        catch (System.IndexOutOfRangeException)
        {
            neighbours[2, 0] = 2;
        }

        try
        {
            neighbours[2, 1] = grid[loc.y - 1, loc.x];
        }
        catch (System.IndexOutOfRangeException)
        {
            neighbours[2, 1] = 2;
        }

        try
        {
            neighbours[2, 2] = grid[loc.y - 1, loc.x + 1];
        }
        catch (System.IndexOutOfRangeException)
        {
            neighbours[2, 2] = 2;
        }
        
        return neighbours;
    }

    public static string CompareGrid(int[,] grid)
    {

        bool skip = false;

        foreach (var TileGridStringPair in GridTypes)
        {
            int[,] gridType = TileGridStringPair.Key;
            skip = false;

            Debug.Log(TileGridStringPair.Value);

            for (int y = 0; y < 3; y++)
            {

                if (skip) { break; }

                for (int x = 0; x < 3; x++)
                {
                
                    if (gridType[y, x] == 0) { continue; }
                    if (grid[y, x] != gridType[y, x]) 
                    { 
                        skip = true;
                        break; 
                    }
                }  
            }

            if (!skip) { return TileGridStringPair.Value; }

        }

        return null;
        
    }


}
