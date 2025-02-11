using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Threading;
using DA_RTS.Classes.Units;

namespace DA_RTS.Classes
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<Miner> miners = new List<Miner>();
        private Texture2D backgroundTexture;
        private Texture2D mineTexture, baseTexture, enemyBaseTexture;
        private Vector2 minePosition = new Vector2(410, 70);
        private Vector2 minePosition2 = new Vector2(780, 620);
        private Vector2 basePosition = new Vector2(110, 325);
        private Vector2 enemyBasePosition = new Vector2(970, 170);

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1200,
                PreferredBackBufferHeight = 900
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            miners.Add(new Miner()); // Starter med én miner
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundTexture = Content.Load<Texture2D>("Assets/TinySwords/Background");
            mineTexture = Content.Load<Texture2D>("Assets/TinySwords/Resources/Gold Mine/GoldMine_Inactive");
            baseTexture = Content.Load<Texture2D>("Assets/TinySwords/Factions/Knights/Buildings/Castle/Castle_Blue");
            enemyBaseTexture = Content.Load<Texture2D>("Assets/TinySwords/Factions/Goblins/Buildings/Wood_House/Goblin_House");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
            _spriteBatch.Draw(mineTexture, minePosition, Color.White);
            _spriteBatch.Draw(mineTexture, minePosition2, Color.White);
            _spriteBatch.Draw(baseTexture, basePosition, Color.White);
            _spriteBatch.Draw(enemyBaseTexture, enemyBasePosition, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, System.EventArgs args)
        {
            foreach (var miner in miners)
            {
                miner.Stop();
            }
            base.OnExiting(sender, args);
        }
    }
}
