using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test.Classes.World
{
    public class TileMap
    {
        //Fields
        private Tile[,] tiles;

        private int mapWidth;
        private int mapHeight;

        private int tileWidth;
        private int tileHeight;

        private Texture2D tileTexture;

        public TileMap(int mapWidth, int mapHeight, int tileWidth, int tileHeight, Texture2D tileTexture)
        {
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.tileTexture = tileTexture;

            tiles = new Tile[mapWidth, mapHeight];

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    Vector2 position = new Vector2(x * tileWidth, y * tileHeight);
                    Rectangle sourceRectangle = new Rectangle(0, 0, tileWidth, tileHeight);
                    tiles[x, y] = new Tile(position, tileWidth, tileHeight, TileType.Grass, tileTexture, sourceRectangle);
                }
            }
        }


        /// <summary>
        /// Tegner hele tilemap'et.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch, der bruges til at tegne tiles.</param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    tiles[x, y].Draw(spriteBatch, layerDepth);
                }
            }
        }
    }
}
