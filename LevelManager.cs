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

        private LevelManager() { }

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
                                Console.WriteLine("found static collider.");
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
                            case "actors":
                                break;
                            case "tile":
                                Console.WriteLine("Found Tile");
                                do
                                {
                                    int x = int.Parse(reader.GetAttribute("x"));
                                    int y = int.Parse(reader.GetAttribute("y"));
                                    int tx = int.Parse(reader.GetAttribute("tx"));
                                    int ty = int.Parse(reader.GetAttribute("ty"));
                                    addTile(x, y, tx, ty);
                                    reader.Read();
                                } while (reader.Name == "tile");
                                break;
                            case "detailTiles":
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
                Rectangle destRect = new Rectangle(tile.x * tile.tSize, tile.y * tile.tSize, 32, 32);
                Rectangle srcRect = new Rectangle(tile.tx * tile.tSize, tile.ty * tile.tSize, 32, 32);
                spriteBatch.Draw(Tileset, destRect, srcRect, Color.White);
            }
        }

        public void addTile(int x, int y, int tx, int ty)
        {
            tiles.Add(new Tile(x, y, tx, ty, 32));
        }

        public void initialise(string locale = "Levels/")
        {
            levelLocation = locale;
            tiles = new List<Tile>();
        }

        public void setTileset(string ts)
        {
            Tileset = ArtManager.Instance.getTexture(ts);
        }

        private string levelLocation;
        private Texture2D Tileset;
        private List<Tile> tiles;
    }
}
