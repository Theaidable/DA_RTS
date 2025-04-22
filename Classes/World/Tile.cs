using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DA_RTS.Classes.World
{
    /// <summary>
    /// Enum der repræsenterer de forskellige typer af tiles.
    /// </summary>
    public enum TileType
    {
        Grass,
        Wall,
        Path
    }

    /// <summary>
    /// Repræsenterer en enkelt tile i spillets tilemap.
    /// Tile'en håndterer sin position, dimensioner og den del af teksturet, der skal tegnes.
    /// </summary>
    public class Tile
    {
        // Private felter
        private Vector2 position;
        private int width;
        private int height;
        private TileType type;
        private Texture2D texture;
        private Rectangle sourceRectangle;

        /// <summary>
        /// Gets eller sets positionen for tile'en (øverste venstre hjørne).
        /// </summary>
        public Vector2 Position { get => position; set => position = value; }

        /// <summary>
        /// Gets eller sets tile'ens bredde.
        /// </summary>
        public int Width { get => width; set => width = value; }

        /// <summary>
        /// Gets eller sets tile'ens højde.
        /// </summary>
        public int Height { get => height; set => height = value; }

        /// <summary>
        /// Gets eller sets tile-typen (fx Grass).
        /// </summary>
        public TileType Type { get => type; set => type = value; }

        /// <summary>
        /// Konstruerer en ny tile med den angivne position, dimensioner, type og tekstur.
        /// Den definerer også, hvilken del af teksturet (sourceRectangle) der skal bruges.
        /// </summary>
        /// <param name="position">Tile'ens position (øverste venstre hjørne).</param>
        /// <param name="width">Tile'ens bredde.</param>
        /// <param name="height">Tile'ens højde.</param>
        /// <param name="type">Tile-typen (f.eks. Grass).</param>
        /// <param name="texture">Teksturen eller spritesheetet der indeholder tile grafik.</param>
        /// <param name="sourceRectangle">En rectangle der angiver, hvilken del af teksturet der skal bruges.</param>
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
                    // Denne rectangle udvælger en del af teksturet (her: x=20, y=20, bredde=150, højde=150) hvilket giver os græs
                    this.sourceRectangle = new Rectangle(20, 20, 150, 150);
                    break;
                case TileType.Wall:
                    this.sourceRectangle = new Rectangle(20, 20, 150, 150);
                    break;
            }
        }

        /// <summary>
        /// Tegner tile'en på skærmen med den angivne lagdybde.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch, der bruges til at tegne tile'en.</param>
        /// <param name="layerDepth">Lagdybden, der bestemmer, hvor tile'en placeres i forhold til andre objekter (lavere værdi vises forrest).</param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(texture, Position, sourceRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
