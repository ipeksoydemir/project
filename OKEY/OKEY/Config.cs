using System;

namespace OKEY
{
    public class Config
    {
        public static readonly int PlayerCount = 4;
        public static readonly int OneSet = 13; 
        public static readonly int FirstPlayersTilesCount = 15; 
        public static readonly int TilesIds = 52; 
        public static readonly int TilesCopy = 2;
        public static readonly int ColorCount = 4;
        public enum Colors
        {
            Yellow=0,
            Blue=1,
            Black=2,
            Red=3
        }
    }
}