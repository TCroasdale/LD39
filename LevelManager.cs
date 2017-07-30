using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD39
{
    struct Tile
    {
        public int x, y, tx, ty, tSize;
        public Tile(int ecks, int why, int tEcks, int tWhy, int ts)
        {
            x = ecks;
            y = why;
            tx = tEcks;
            ty = tWhy;
            tSize = ts;
        }
    }

    class LevelManager
    {
        #region SINGLETON IMPL
        private static LevelManager instance;

        private LevelManager() {
            files = new List<string>();
            files.Add("rand_0.oel");
            files.Add("rand_1.oel");
        }

        public static LevelManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LevelManager();
                }
                return instance;
            }
        }
        #endregion

        public void reset()
        {
            tiles.Clear();
            detailTiles.Clear();
        }

        public void loadFile(string fileName)
        {
            using (XmlReader reader = XmlReader.Create(levelLocation + fileName)){
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "rect":
                                do
                                {
                                    int x = int.Parse(reader.GetAttribute("x"));
                                    int y = int.Parse(reader.GetAttribute("y"));
                                    int w = int.Parse(reader.GetAttribute("w"));
                                    int h = int.Parse(reader.GetAttribute("h"));
                                    PhysicsManager.Instance.addStaticCollider(x*32, y*32, w*32, h*32);
                                    reader.Read();
                                } while (reader.Name == "rect");
                                break;
                            case "PlayerStart":
                                int xPos = int.Parse(reader.GetAttribute("x"));
                                int yPos = int.Parse(reader.GetAttribute("y"));
                                ActorManager.Instance.createActor<PlayerActor>("Player", new Vector2(xPos, yPos));
                                break;
                            case "Battery":
                                xPos = int.Parse(reader.GetAttribute("x"));
                                yPos = int.Parse(reader.GetAttribute("y"));
                                ActorManager.Instance.createActor<PowerPlusActor>("Battery", new Vector2(xPos, yPos));
                                break;
                            case "Coin_gold":
                                xPos = int.Parse(reader.GetAttribute("x"));
                                yPos = int.Parse(reader.GetAttribute("y"));
                                CoinActor actor = ActorManager.Instance.createActor<CoinActor>("GoldCoin", new Vector2(xPos, yPos)) as CoinActor;
                                actor.amount = 250;
                                actor.setSprite(ArtManager.Instance.getTexture("GoldCoin"));
                                break;
                            case "Coin_Silver":
                                xPos = int.Parse(reader.GetAttribute("x"));
                                yPos = int.Parse(reader.GetAttribute("y"));
                                actor = ActorManager.Instance.createActor<CoinActor>("SilvCoin", new Vector2(xPos, yPos)) as CoinActor;
                                actor.amount = 100;
                                break;
                            case "tile":
                                do
                                {
                                    int x = int.Parse(reader.GetAttribute("x"));
                                    int y = int.Parse(reader.GetAttribute("y"));
                                    int tx = int.Parse(reader.GetAttribute("tx"));
                                    int ty = int.Parse(reader.GetAttribute("ty"));

                                    addTile(x*32, y*32, tx, ty);

                                    reader.Read();
                                } while (reader.Name == "tile");
                                break;

                            case "detail":
                                do
                                {
                                    int x = int.Parse(reader.GetAttribute("x"));
                                    int y = int.Parse(reader.GetAttribute("y"));
                                    int tx = int.Parse(reader.GetAttribute("tx"));
                                    int ty = int.Parse(reader.GetAttribute("ty"));

                                    addDetail(x*32, y*32, tx, ty);

                                    reader.Read();
                                } while (reader.Name == "tile");
                                break;
                            default:
                                //Console.WriteLine("Fucked up with: " + reader.Name);
                                break;

                        }
                    }
                }
            }
        }


        public void AddRandomLevel(int offset)
        {
            Random rand = new Random();
            string fileName = files[rand.Next(0, files.Count)];
            using (XmlReader reader = XmlReader.Create(levelLocation + fileName))
            {
                Console.WriteLine("Reading file: " + fileName);
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "rect":
                                do
                                {
                                    int x = int.Parse(reader.GetAttribute("x"));
                                    int y = int.Parse(reader.GetAttribute("y"));
                                    int w = int.Parse(reader.GetAttribute("w"));
                                    int h = int.Parse(reader.GetAttribute("h"));
                                    Console.WriteLine("Adding collider at: {0}, {1}", x * 32, y * 32 - offset);
                                    PhysicsManager.Instance.addStaticCollider(x * 32, y * 32 - offset, w * 32, h * 32);
                                    reader.Read();
                                } while (reader.Name == "rect");
                                break;
                            case "Battery":
                                int xPos = int.Parse(reader.GetAttribute("x"));
                                int yPos = int.Parse(reader.GetAttribute("y"));
                                Console.WriteLine("Adding Battery at: {0}, {1}", xPos, offset + yPos);
                                ActorManager.Instance.createActor<PowerPlusActor>("Battery", new Vector2(xPos, yPos - offset));
                                break;
                            case "Coin_gold":
                                xPos = int.Parse(reader.GetAttribute("x"));
                                yPos = int.Parse(reader.GetAttribute("y"));
                                Console.WriteLine("Adding gold coin at: {0}, {1}", xPos, offset + yPos);
                                CoinActor actor = ActorManager.Instance.createActor<CoinActor>("GoldCoin", new Vector2(xPos, yPos - offset)) as CoinActor;
                                actor.amount = 250;
                                actor.setSprite(ArtManager.Instance.getTexture("GoldCoin"));
                                break;
                            case "Enemy":
                                xPos = int.Parse(reader.GetAttribute("x"));
                                yPos = int.Parse(reader.GetAttribute("y"));
                                Console.WriteLine("Adding enemy at: {0}, {1}", xPos, offset + yPos);
                                EnemyActor enemy = ActorManager.Instance.createActor<EnemyActor>("Enemy", new Vector2(xPos, yPos - offset)) as EnemyActor;
                                enemy.setActor(ActorManager.Instance.getFirstWithTag("Player"));
                                break;
                            case "Coin_Silver":
                                xPos = int.Parse(reader.GetAttribute("x"));
                                yPos = int.Parse(reader.GetAttribute("y"));
                                Console.WriteLine("Adding silver coin at: {0}, {1}", xPos, offset + yPos);
                                actor = ActorManager.Instance.createActor<CoinActor>("SilvCoin", new Vector2(xPos, yPos - offset)) as CoinActor;
                                actor.amount = 100;
                                break;
                            case "tile":
                                do
                                {
                                    int x = int.Parse(reader.GetAttribute("x"));
                                    int y = int.Parse(reader.GetAttribute("y"));
                                    int tx = int.Parse(reader.GetAttribute("tx"));
                                    int ty = int.Parse(reader.GetAttribute("ty"));
                                    Console.WriteLine("Adding Tile at: {0}, {1}", x, y - offset);
                                    addTile(x * 32, y * 32 - offset, tx, ty);

                                    reader.Read();
                                } while (reader.Name == "tile");
                                break;

                            case "detail":
                                do
                                {
                                    int x = int.Parse(reader.GetAttribute("x"));
                                    int y = int.Parse(reader.GetAttribute("y"));
                                    int tx = int.Parse(reader.GetAttribute("tx"));
                                    int ty = int.Parse(reader.GetAttribute("ty"));
                                    Console.WriteLine("Adding detail at: {0}, {1}", x, y - offset);
                                    addDetail(x * 32, y * 32 - offset, tx, ty);

                                    reader.Read();
                                } while (reader.Name == "tile");
                                break;
                            default:
                                //Console.WriteLine("Fucked up with: " + reader.Name);
                                break;

                        }
                    }
                }
            }
        }


        public void drawLevel(SpriteBatch spriteBatch){
            foreach(Tile tile in tiles)
            {
                Rectangle destRect = new Rectangle(tile.x, tile.y, 32, 32);
                Rectangle srcRect = new Rectangle(tile.tx * tile.tSize, tile.ty * tile.tSize, 32, 32);
                spriteBatch.Draw(Tileset, destRect, srcRect, Color.White);
            }
        }

        public void drawLevelDetails(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in detailTiles)
            {
                Rectangle destRect = new Rectangle(tile.x, tile.y, 32, 32);
                Rectangle srcRect = new Rectangle(tile.tx * tile.tSize, tile.ty * tile.tSize, 32, 32);
                spriteBatch.Draw(Tileset, destRect, srcRect, Color.White);
            }
        }


        public void addTile(int x, int y, int tx, int ty)
        {
            tiles.Add(new Tile(x, y, tx, ty, 32));
        }
        public void addDetail(int x, int y, int tx, int ty)
        {
            detailTiles.Add(new Tile(x, y, tx, ty, 32));
        }

        public void initialise(string locale = "Levels/")
        {
            levelLocation = locale;
            tiles = new List<Tile>();
            detailTiles = new List<Tile>();
        }

        public void setTileset(string ts)
        {
            Tileset = ArtManager.Instance.getTexture(ts);
        }

        private string levelLocation;
        private Texture2D Tileset;
        private List<Tile> tiles;
        private List<Tile> detailTiles;
        private List<string> files;
    }
}
