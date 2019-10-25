using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Map
{
    public enum MapTileType
    {
        Forest = 1,
        Grass = 2,
        Road = 3,
        Impassable = 4
    }

    MapTileType[][] grid;

    float height, width;

    public Map()
    {
    }

    public Vector3 GetMapCoords(Vector3 position)
    {
        // FIXME: This can't be here.
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);

        float x = (screenPosition.x / width) * grid[0].Length;
        float y = (screenPosition.y / height) * grid.Length;

        // Screen coordinates have 0,0 in the bottom left. 
        y = grid.Length - y - 1;
        return new Vector3(x, y);
    }

    public MapTileType Get(Vector3 position)
    {
        //position = Camera.main.WorldToScreenPoint(position);
        // Get the x and y. 
        position = GetMapCoords(position);
        return grid[(int)position.y][(int)position.x];
    }

    public void Load(String csvData, GameObject mapImage)
    {
        List<MapTileType[]> lines = new List<MapTileType[]>();

        width = Screen.width;  
        height = Screen.height;  

        string line;
        StringReader strReader = new StringReader(csvData);
        int lineLength = 0;
        while (true)
        {
            line = strReader.ReadLine();
            if (line != null)
            {
                String[] csvLine = line.Split(',');

                if (lineLength != 0 && (csvLine.Length == 0 || csvLine.Length != lineLength))
                {
                    throw new Exception("Bad Map");
                }
                else if (lineLength == 0)
                {
                    lineLength = csvLine.Length;
                }
                lines.Add( Array.ConvertAll<String, MapTileType>(csvLine, s => (MapTileType) int.Parse(s)));
            }
            else
            {
                break;
            }
        }
        grid = lines.ToArray();

    }


}
