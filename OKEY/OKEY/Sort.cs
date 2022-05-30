using System;
using System.Collections;
using System.Collections.Generic;

namespace OKEY
{
    public class Sort
    {
        public static List<List<Tile>> GroupByColor = new List<List<Tile>>();
        public static List<HashSet<Tile>> GroupByNumber = new List<HashSet<Tile>>();
        public static List<List<Tile>> GroupByDuble = new List<List<Tile>>();

        public static List<HashSet<Tile>> ClusteredWithColor = new List<HashSet<Tile>>();
       
        public static void ParseForColorPer(List<Tile>ColorGroup)
        {
       
            for (int i = 0; i < ColorGroup.Count - 1; i++) 
            {
                
                if (ColorGroup[i+1].GetNumber() == 13 && ColorGroup[0].GetNumber() == 1)
                {
                    HashSet<Tile> pers = new HashSet<Tile>();
                    pers.Add(ColorGroup[i+1]);
                    pers.Add(ColorGroup[0]);
                    ClusteredWithColor.Add(pers);
                }
                if (ColorGroup[i].GetNumber() + 1 == ColorGroup[i + 1].GetNumber()) 
                {
                    HashSet<Tile> pers = new HashSet<Tile>();
                       
                    pers.Add(ColorGroup[i]);pers.Add(ColorGroup[i+1]);
                    for (int j = 2; j < ColorGroup.Count-i; j++) 
                    {
                        if (ColorGroup[i].GetNumber() + j == ColorGroup[j + i].GetNumber()) 
                        {
                            
                            pers.Add(ColorGroup[j + i]);
                            if (ColorGroup[i+j].GetNumber() == 13 && ColorGroup[0].GetNumber() == 1)
                            {
                                pers.Add(ColorGroup[0]);
                            }
                        }
                        else 
                        { 
                            
                                    break;
                        }
                    }
                           
                    ClusteredWithColor.Add(pers);
                    foreach (var per in pers)
                    { 
                        ColorGroup.Remove(per);
                    }
                    
                    i = -1;
                }
                else 
                { 
                   
                }
                        
            }

            int k = 0;
            while (ColorGroup.Count>=1) 
            { 
               
                HashSet<Tile> onesGroup = new HashSet<Tile>();
                onesGroup.Add(ColorGroup[k]);
                ColorGroup.Remove(ColorGroup[k]);
                ClusteredWithColor.Add(onesGroup);
            }



        }
        public static List<Tile> NumberBasedOrder(List<Tile> tiles)
        {
            List<Tile> orderByNum = new List<Tile>();
            
            for (int i = 0; i < Config.ColorCount; i++)
            {
                List<Tile> ColorGroup = new List<Tile>();
                foreach (var tile in tiles)
                {
                    if (i == (int) tile.GetColor())
                    {
                        ColorGroup.Add(tile);
                        if (tile.IsOkey())
                        {
                            ColorGroup.Remove(tile);
                        }
                        
                    }
                }

                ColorGroup=  ColorBasedOrder(ColorGroup);
                foreach (var tile in ColorGroup)
                {
                    orderByNum.Add(tile);
                }
                ParseForColorPer(ColorGroup);
                GroupByColor.Add(ColorGroup);
            }
            GroupByNumber.Clear();
           
            return orderByNum;
        }

             
        public static List<Tile> ColorBasedOrder(List<Tile> tiles)
        {
            List<Tile> orderByColor = new List<Tile>();
           
            for (int i = 0; i < Config.OneSet; i++)  //13 kez döner
            {  //Hashset elemanları unique olduğu için daha kullanışlı bir veri yapısıdır
                HashSet<Tile> NumberGroup = new HashSet<Tile>();
                bool flag = true;
                foreach (var tile in tiles)
                {
                    
                    if ((i+1) == tile.GetNumber() )
                    {
                        foreach (var group in NumberGroup)
                        {
                            if (group.GetColor() == tile.GetColor())
                            {
                                flag = false;
                            }
                        }

                        if (flag)
                        {
                            NumberGroup.Add(tile);
                            orderByColor.Add(tile);
                            if (tile.IsOkey())
                            {
                                NumberGroup.Remove(tile);
                            }
                        }
                    }
                }
                
                
                
                
                
                GroupByNumber.Add(NumberGroup);
                
            }
           
            return orderByColor; 
        }
        
        
        public static void DubleBasedOrder(List<Tile> tiles)
        {
            List<Tile> orderByDuble = new List<Tile>();
            for (int i = 0; i < tiles.Count; i++)
            {
                List<Tile> DubleGroup = new List<Tile>();
                for (int j = i+1; j < tiles.Count; j++)
                {
                    if (tiles[i].GetNumber() == tiles[j].GetNumber() && tiles[i].GetColor() == tiles[j].GetColor())
                    {
                        DubleGroup.Add(tiles[i]);DubleGroup.Add(tiles[j]);
                        
                    }
                }
                
                GroupByDuble.Add(DubleGroup);
            }
          
        }

    }
}