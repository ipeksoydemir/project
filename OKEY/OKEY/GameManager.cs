using System;
using System.Collections.Generic;

namespace OKEY
{
    public class GameManager
    {
        static Random random = new Random();
        private static List<Tile> allTiles = new List<Tile>();
        private static List<Player> players = new List<Player>();
        
        // taş çekme/atma işlemleri için pop push sayesinde stack veri yapısı daha kullanışlıdır
        private static Stack<Tile> supplyStackOfTiles = new Stack<Tile>();
        
        public void CreateTile()
        {
            for (int i = 0; i < Config.TilesCopy; i++)
            {
                for (int j = 0; j < Config.TilesIds ; j++)
                {
                    Tile tile = new Tile(j);
                   
                    allTiles.Add(tile);
                }
            }
           
        }

        public void CreatePlayers()
        {
            for (int i = 0; i < Config.PlayerCount; i++)
            {
                players.Add(new Player(i));
            }
            ChooseFirstPlayer();
        }

        public void ChooseFirstPlayer()
        {
            int firstPlayerNo = random.Next(0,Config.PlayerCount);
            players[firstPlayerNo].SetFirstPlayer();
        }
        public void OkeyGenerator()
        {
            int okey;
            int indicator = random.Next(0, Config.TilesIds); //sahte okey çekilme ihtimali yok
           
            if (indicator + 1 % Config.OneSet == 0)
            { //gösterge 13 ise okey 1'dir
                okey = indicator - (Config.OneSet - 1);
            }
            else
            { //aksi durumda okey göstergenin 1 fazlasıdır
                okey = indicator + 1;
            }
            Console.WriteLine("Okey: "+allTiles[okey].GetNumber() +"-"+ allTiles[okey].GetColor());
            allTiles[okey].setOkey(true);
            allTiles[okey + Config.TilesIds].setOkey(true);
            allTiles.Remove(allTiles[indicator]);
            
            //Sahte okeyler eklenir
            for (int i = 0; i < Config.TilesCopy; i++)
            {
                allTiles.Add(new Tile(okey, true));
            }
            
            
        }

        public void ShuffleTiles()
        {
            int n = allTiles.Count;  
            while (n > 1) 
            {  
                n--;  
                int k = random.Next(n + 1);  
                //random index ile yer değiştirir
                Tile value = allTiles[k];  
                allTiles[k] = allTiles[n];  
                allTiles[n] = value;  
            }  
        }
        
        
        public void Run()
        {
            
            CreateTile();
            OkeyGenerator();
            ShuffleTiles();
            CreatePlayers();
            
            // Masanın ortasındaki tedarik destesi
            foreach (var tile in allTiles)
            {
                supplyStackOfTiles.Push(tile);
            }
           
            #region Distribute the tiles to the players
            //ortadoğuda kartlar yeniden dağıtılıyor :)
            for (int i = 0; i < Config.PlayerCount; i++)
            {
                if (players[i].IsFirstPlayer())
                {
                    for (int j = 0; j < Config.FirstPlayersTilesCount; j++)
                    {
                        players[i].AddToMyTiles(supplyStackOfTiles.Pop());
                    }
                }
                else
                {
                    for (int j = 0; j < Config.FirstPlayersTilesCount-1; j++)
                    {
                        players[i].AddToMyTiles(supplyStackOfTiles.Pop());
                    }
                }
                
                
            }
            #endregion


            #region Order and Get Score for All Players

            float maxScore = 0;int maxplayerNo=-1;
              foreach (var player in players)
              {
                  Tile okey = null;int okeyAmount = player.GetOkeyCount();
                  float colorScore=0, numberScore=0,dubleScore=0;
                Console.WriteLine("\nPlayer " + (player.GetPlayerNo()+1) + "   Okey count: " + player.GetOkeyCount());
                Console.WriteLine("Given Order:");
                foreach (var tile in player.GetTiles())
                {
                    if (tile.IsOkey())
                    { // okey ise ** 'lar arasında yazılır
                        Console.Write("**"+tile.GetNumber() + "-"+tile.GetColor() +"** | ");
                        okey = tile;
                    }
                    else
                    {
                        Console.Write(tile.GetNumber() + "-"+tile.GetColor() +" | ");
                    }
                    
                }
                
                Console.WriteLine("\nNumber Based Order:");
             

               Sort.NumberBasedOrder(player.GetTiles());
               int count = 0;
               for (int i = 0; i < Sort.ClusteredWithColor.Count; i++)
               {
                   if (Sort.ClusteredWithColor[i].Count == 2)
                   {
                       count++;
                      
                       if (okeyAmount > 0)
                       {
                           Sort.ClusteredWithColor[i].Add(okey);
                           okeyAmount--;
                           colorScore += 3;
                       }
                       else
                       {
                           colorScore += 0.5f;
                       }
                      
                   }
                   if (Sort.ClusteredWithColor[i].Count > 2)
                   {
                       if (count == 0)
                       {
                           if (okeyAmount > 0)
                           {
                               Sort.ClusteredWithColor[i].Add(okey);
                               okeyAmount--;
                               colorScore = colorScore + Sort.ClusteredWithColor[i].Count +
                                              ((Sort.ClusteredWithColor[i].Count - 3) * 0.5f);
                           }
                           else
                           {
                               colorScore = colorScore + Sort.ClusteredWithColor[i].Count +
                                              ((Sort.ClusteredWithColor[i].Count - 3) * 0.5f);
                           }
                       }
                   }

                  
                   foreach (var data in Sort.ClusteredWithColor[i])
                   {

                       Console.Write(data.GetColor()+"-"+data.GetNumber()+" ");
                   }
                  
                    Console.Write(" | ");
               }

               okeyAmount = player.GetOkeyCount();
               Console.WriteLine("\nNumber Score:" + colorScore);
               
               Console.WriteLine("Color Based Order:");
               Sort.ColorBasedOrder(player.GetTiles());


               count = 0;
               
               for (int i = 0; i < Sort.GroupByNumber.Count; i++)
               {
                   if (Sort.GroupByNumber[i].Count == 2)
                   {
                       count++;
                      
                       if (okeyAmount > 0)
                       {
                           Sort.GroupByNumber[i].Add(okey);
                           okeyAmount--;
                           numberScore += 3;
                       }
                       else
                       {
                           numberScore += 0.5f;
                       }

                   }
                   if (Sort.GroupByNumber[i].Count > 2)
                   {
                       
                       
                       if (count == 0)
                       {
                           if (okeyAmount > 0)
                           {
                               Sort.GroupByNumber[i].Add(okey);
                               okeyAmount--;
                               numberScore = numberScore + Sort.GroupByNumber[i].Count +
                                              ((Sort.GroupByNumber[i].Count - 3) * 0.5f);
                           }
                           else
                           {
                               numberScore = numberScore + Sort.GroupByNumber[i].Count +
                                              ((Sort.GroupByNumber[i].Count - 3) * 0.5f);
                           }
                       }
                       
                       
                       
                       
                       
                       
                   }

                   
                   foreach (var data in Sort.GroupByNumber[i])
                   {

                       Console.Write(data.GetColor()+"-"+data.GetNumber()+" ");
                   }
                  
                   Console.Write(" | ");
               }
               Console.WriteLine("\nColor Score:" + numberScore);
               Console.WriteLine("Duble Based Order:");
             
               Sort.DubleBasedOrder(player.GetTiles());
                
               for (int i = 0; i < Sort.GroupByDuble.Count; i++)
               {
                   dubleScore += Sort.GroupByDuble[i].Count * 1.5f;
                  
                   foreach (var data in Sort.GroupByDuble[i])
                   {

                       Console.Write(data.GetColor()+"-"+data.GetNumber()+" ");
                   }
                   
                   
               }Console.Write("\n");
               Console.WriteLine("Duble Score:" + dubleScore);

               Sort.GroupByColor.Clear();
               Sort.GroupByNumber.Clear();
               Sort.ClusteredWithColor.Clear();
               Sort.GroupByDuble.Clear();



              
               
             
               player.SetColorScore(colorScore);
               player.SetNumberScore(numberScore);
               player.SetDoubleScore(dubleScore);



               if (player.GetScore() > maxScore)
               {
                   maxScore = player.GetScore();
                   maxplayerNo = player.GetPlayerNo();
               }
              
              }

            #endregion
            
            
            Console.WriteLine("Closest player to finish => Player " +maxplayerNo+ " with " + maxScore + " score");
           
          
            
        }
        
    }
}