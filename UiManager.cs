using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD39
{
    class UiElement
    {
        private Vector2 position;
        public Vector2 getPosition() { return position; }
        public void setPosition(Vector2 val) { position = val; }
        private Vector2 size;
        public Vector2 getSize() { return size; }
        public void setSize(Vector2 val) { size = val; }
        private Texture2D texture;
        string content;
        private Color color;
        public void setColor(Color col) { color = col; }
        public bool isVisible;

        public UiElement(Vector2 pos, Vector2 siz, string cont) {
            position = pos;
            size = siz;
            if (cont.Contains("<IMG>")) {
                texture = ArtManager.Instance.getTexture(cont.Substring(5));
            }
            else{
                content = cont;
            }
            color = Color.White;
            isVisible = true;
        }

        public void Draw(SpriteBatch sb){
            if (isVisible)
            {
                Rectangle destRect = new Rectangle(position.ToPoint(), size.ToPoint());
                if (texture != null)
                {
                    sb.Draw(texture, destRect, color);
                }
                else
                {
                    //Draw string
                    sb.DrawString(UiManager.Instance.getFont(), content, position, color);
                }
            }
        }

        public void setContent(string cont){
            if (cont.Contains("<IMG>"))
            {
                texture = ArtManager.Instance.getTexture(cont.Substring(5));
            }
            else
            {
                content = cont;
            }
        }


    }

    class UiManager
    {
        #region SINGLETON IMPL
        private static UiManager instance;

        private UiManager()
        {
            UiElements = new Dictionary<string, UiElement>();
        }

        public static UiManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UiManager();
                }
                return instance;
            }
        }
        #endregion

        private Dictionary<string, UiElement> UiElements;
        private SpriteBatch spriteBatch;
        private SpriteFont font;

        public void createSpriteBatch(GraphicsDevice gd){
            spriteBatch = new SpriteBatch(gd);
        }

        public void Update(float deltaTime){

        }

        public void Draw(){
            spriteBatch.Begin();
            foreach(KeyValuePair<string, UiElement> Ui in UiElements) {
                Ui.Value.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        public UiElement addUi(string name, Vector2 pos, Vector2 size, string content){
            UiElement elem = new UiElement(pos, size, content);
            UiElements.Add(name, elem);
            return elem;
        }

        public UiElement getUi(string name){
            return UiElements[name];
        }

        public void setFont(SpriteFont f) { font = f; }
        public SpriteFont getFont() { return font; }
    }
}
