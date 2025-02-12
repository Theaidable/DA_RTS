using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test.Classes.World
{
    public enum TileType
    {
        Grass
    }
    public class Tile
    {
        //Fields
        private Vector2 position;
        private int width;
        private int height;
        private TileType type;
        private Texture2D texture;
        private Rectangle sourceRectangle;

        //Properties
        public Vector2 Position { get => position; set => position = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public TileType Type { get => type; set => type = value; }


        /// <summary>
        /// Constructor for Tile-klassen.
        /// </summary>
        /// <param name="position">Positionen for tile (øverste venstre hjørne).</param>
        /// <param name="width">Tile bredde.</param>
        /// <param name="height">Tile højde.</param>
        /// <param name="type">Tile type.</param>
        /// <param name="texture">Reference til texture/spritesheet, som indeholder tile graphics.</param>
        public Tile(Vector2 position, int width, int height, TileType type, Texture2D texture, Rectangle sourceRectangle)
        {
            Position = position;
            Width = width;
            Height = height;
            Type = type;
            this.texture = texture;

            switch (type)
            {
                case TileType.Grass:
                    this.sourceRectangle = new Rectangle(20, 20, 150, 150);
                    break;
                default:
                    this.sourceRectangle = new Rectangle(20, 20, 150, 150);
                    break;
            }
        }

        /// <summary>
        /// Tegner tile'en på skærmen.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch, der bruges til at tegne tile.</param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(texture, Position, sourceRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
