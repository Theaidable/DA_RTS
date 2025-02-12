using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using DA_RTS.Classes.Castle;
using DA_RTS.Classes.Mines;
using DA_RTS.Classes.Units;
using DA_RTS.Classes.UI;
using System;

namespace DA_RTS.Classes.World
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState previousKeyboardState;

        private TileMap tileMap;
        private Texture2D tileTexture;

        private Texture2D blueCastleTexture;
        private Texture2D redCastleTexture;
        private TownHall blueTownHall;
        private TownHall redTownHall;

        private Texture2D blueMineTexture;
        private Texture2D redMineTexture;
        private Mine blueMine;
        private Mine redMine;

        private Texture2D blueMinerTexture;
        private Texture2D redMinerTexture;

        private Button btnBuyMiner;
        private Button btnBuySoldier;
        private Texture2D buttonTexture;
        private Texture2D pressedButtonTexture;

        private Texture2D goldIcon;
        private Texture2D heartIcon;
        private Texture2D minerIcon;
        private Texture2D soldierIcon;

        private SpriteFont uiFont;

        private int gold = 500;
        private int life = 100;

        private List<Miner> miners;

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            previousKeyboardState = Keyboard.GetState();

            tileTexture = Content.Load<Texture2D>("Assets/TinySwords/Terrain/Ground/Tilemap_Flat");
            blueCastleTexture = Content.Load<Texture2D>("Assets/TinySwords/Factions/Knights/Buildings/Castle/Castle_Blue");
            redCastleTexture = Content.Load<Texture2D>("Assets/TinySwords/Factions/Knights/Buildings/Castle/Castle_Red");
            blueMineTexture = Content.Load<Texture2D>("Assets/TinySwords/Resources/Gold Mine/GoldMine_Inactive");
            redMineTexture = Content.Load<Texture2D>("Assets/TinySwords/Resources/Gold Mine/GoldMine_Inactive");

            blueMinerTexture = Content.Load<Texture2D>("Assets/TinySwords/Factions/Knights/Troops/Pawn/Pawn_Blue");
            redMinerTexture = Content.Load<Texture2D>("Assets/TinySwords/Factions/Knights/Troops/Pawn/Pawn_Red");

            uiFont = Content.Load<SpriteFont>("Assets/Fonts/UIFont");
            goldIcon = Content.Load<Texture2D>("Assets/TinySwords/Resources/Resources/G_Idle");
            heartIcon = Content.Load<Texture2D>("Assets/UI/Health/Health001");
            minerIcon = Content.Load<Texture2D>("Assets/TinySwords/Factions/Knights/Troops/Pawn/Pawn_Blue");
            soldierIcon = Content.Load<Texture2D>("Assets/TinySwords/Factions/Knights/Troops/Warrior/Warrior_Blue");

            buttonTexture = Content.Load<Texture2D>("Assets/TinySwords/UI/Buttons/Button_Blue");
            pressedButtonTexture = Content.Load<Texture2D>("Assets/TinySwords/UI/Buttons/Button_Blue_Pressed");

            btnBuyMiner = new Button(buttonTexture, new Vector2(50, 10))
            {
                Icon = minerIcon,
                IconSourceRect = new Rectangle(0, 0, 150, 150),
                PressedTexture = pressedButtonTexture,
                Font = uiFont
            };

            btnBuySoldier = new Button(buttonTexture, new Vector2(btnBuyMiner.Position.X + btnBuyMiner.Texture.Width + 75, 10))
            {
                Icon = soldierIcon,
                IconSourceRect = new Rectangle(0, 0, 150, 150),
                PressedTexture = pressedButtonTexture,
                Font = uiFont
            };

            btnBuyMiner.Click += BtnBuyMiner_Click;
            btnBuySoldier.Click += BtnBuySoldier_Click;

            LoadTileMap();
            LoadCastles();
            LoadMines();

            miners = new List<Miner>();
        }

        public void LoadTileMap()
        {
            int mapWidth = 40;
            int mapHeight = 30;
            int tileWidth = 150;
            int tileHeight = 150;

            tileMap = new TileMap(mapWidth, mapHeight, tileWidth, tileHeight, tileTexture);
        }

        public void LoadCastles()
        {
            int offsetX = 50;
            int windowHeight = GraphicsDevice.Viewport.Height;
            int windowWidth = GraphicsDevice.Viewport.Width;

            Vector2 bluePosition = new Vector2(offsetX, (windowHeight - blueCastleTexture.Height) / 2);
            Vector2 redPosition = new Vector2(windowWidth - redCastleTexture.Width - offsetX, (windowHeight - redCastleTexture.Height) / 2);

            blueTownHall = new TownHall(bluePosition, blueCastleTexture);
            redTownHall = new TownHall(redPosition, redCastleTexture);
        }

        public void LoadMines()
        {
            int verticalOffset = 50;
            int windowHeight = GraphicsDevice.Viewport.Height;
            int windowWidth = GraphicsDevice.Viewport.Width;

            Vector2 blueMinePosition = new Vector2((windowWidth - blueMineTexture.Width) / 2, verticalOffset);
            Vector2 redMinePosition = new Vector2((windowWidth - redMineTexture.Width) / 2, windowHeight - redMineTexture.Height - verticalOffset);

            blueMine = new Mine(blueMinePosition, blueMineTexture);
            redMine = new Mine(redMinePosition, redMineTexture);
        }

        private void BtnBuyMiner_Click(object sender, EventArgs e)
        {
            if (gold >= 100)
            {
                gold -= 100;
                Vector2 offset = new Vector2(100, -50);
                Vector2 minerStartPosition = blueTownHall.Position + offset;
                Miner newMiner = new Miner(minerStartPosition, blueMinerTexture, 100f, blueMine.GetPosition(), blueTownHall.Position);
                newMiner.GoldDelivered += Miner_GoldDelivered;
                miners.Add(newMiner);
            }
        }

        private void Miner_GoldDelivered(object sender, int deliveredGold)
        {
            gold += deliveredGold;
        }

        private void BtnBuySoldier_Click(object sender, EventArgs e)
        {
            if (gold >= 200)
            {
                gold -= 200;
                //Spawn logik for en soldier
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            btnBuyMiner.Update(gameTime);
            btnBuySoldier.Update(gameTime);

            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.M) && previousKeyboardState.IsKeyUp(Keys.M))
            {
                Vector2 offset = new Vector2(100, -80);
                Vector2 minerStartPosition = blueTownHall.Position + offset;
                Miner newMiner = new Miner(minerStartPosition, blueMinerTexture, 100f, blueMine.GetPosition(), blueTownHall.Position);
                miners.Add(newMiner);
            }
            previousKeyboardState = keyboardState;

            foreach (Miner miner in miners)
            {
                miner.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            tileMap.Draw(_spriteBatch, 1.0f);

            blueTownHall.Draw(_spriteBatch, 0.4f);
            redTownHall.Draw(_spriteBatch, 0.4f);

            blueMine.Draw(_spriteBatch, 0.6f);
            redMine.Draw(_spriteBatch, 0.6f);

            btnBuyMiner.Draw(_spriteBatch, 0.0f);
            btnBuySoldier.Draw(_spriteBatch, 0.0f);

            Vector2 goldIconPosition = new Vector2(-25, 100);
            Vector2 heartIconPosition = new Vector2(20, 200);
            _spriteBatch.Draw(goldIcon, goldIconPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            _spriteBatch.Draw(heartIcon, heartIconPosition, null, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.0f);

            Vector2 goldTextPosition = goldIconPosition + new Vector2(100, 70);
            Vector2 lifeTextPosition = heartIconPosition + new Vector2(53, 15);
            _spriteBatch.DrawString(uiFont, $"{gold}", goldTextPosition, Color.Yellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            _spriteBatch.DrawString(uiFont, $"{life}", lifeTextPosition, Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);

            foreach (Miner miner in miners)
            {
                miner.Draw(_spriteBatch, 0.5f);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
