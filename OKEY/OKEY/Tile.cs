using System;

namespace OKEY
{
    public class Tile:IComparable
    {
        private int id;
        private int num;
        private Config.Colors color;
        private bool okey = false;

        public Tile(int id,bool isFakeOkey=false)
        {
            this.id = id;
            if (isFakeOkey)
                this.id =Config.TilesIds;
            num = (id + 1) % Config.OneSet;
            if (num == 0)
                num = Config.OneSet;

            color =( (Config.Colors) ((int)(id / Config.OneSet)));

        }
        
        public int GetId() {
            return id;
        }
        public Config.Colors GetColor() {
            return color;
        }
      
        
        
        public int GetNumber()
        {
            return num;
        }
        
        public bool IsOkey() {
            return okey;
        }

        public void setOkey(bool okey) {
            this.okey = okey;
        }


        public int CompareTo(object? obj)
        {
            return num.CompareTo(obj);
        }
    }
}