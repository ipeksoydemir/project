using System.Collections.Generic;

namespace OKEY
{
    public class Player
    {
        private int playerNo;
        private bool firstPlayer = false;
        private List<Tile> tiles = new List<Tile>();
        private int okeyCount = 0;
        private float scoreColor = 0,scoreNumber=0, scoreDuble=0,totalScore=0;
    
         public Player(int playerNo)
        {
            this.playerNo = playerNo;
        }

         public void SetColorScore(float score) { scoreColor = score;}
         public void SetNumberScore(float score) { scoreNumber = score;}
         public void SetDoubleScore(float score) { scoreDuble = score;}
        public void SetFirstPlayer() { firstPlayer = true; }
        public bool IsFirstPlayer() { return firstPlayer; }
        public int GetOkeyCount() { return okeyCount; }
        public int GetPlayerNo() { return playerNo; }
        public List<Tile> GetTiles() { return tiles; }
        public void AddToMyTiles(Tile tile)
        {
            okeyCount = 0;
            tiles.Add(tile);
            foreach (var item in tiles)
            {
                if (item.IsOkey())
                    okeyCount++;
            }
        }
        public float GetScore()
        {
            totalScore = (okeyCount * 2) + scoreDuble + scoreNumber + scoreColor;
            return totalScore;
        }
    }
}