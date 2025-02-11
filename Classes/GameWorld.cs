using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using DA_RTS.Classes.Units;

namespace DA_RTS.Classes
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Lister til enheder
        private List<Miner> miners = new List<Miner>();
        private List<Soldier> soldiers = new List<Soldier>();

        // Spillerens base
        private TownHall townHall;

        // Teksturer til baggrund, objekter og enheder
        private Texture2D backgroundTexture;
        private Texture2D mineTexture;
        private Texture2D enemyBaseTexture;
        private Texture2D baseTexture;
        private Texture2D minerUnitTexture;
        private Texture2D soldierUnitTexture;

        // Positioner for de statiske objekter
        private Vector2 minePosition = new Vector2(410, 70);
        private Vector2 minePosition2 = new Vector2(780, 620);
        private Vector2 basePosition = new Vector2(110, 325);
        private Vector2 enemyBasePosition = new Vector2(970, 170);

        // Input-håndtering (edge detection)
        private KeyboardState previousKeyboardState;

        // Enhedspriser
        private const int MinerCost = 100;
        private const int SoldierCost = 200;

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
            // Opret TownHall med en startposition og et standard max health (fx 1000)
            townHall = new TownHall(basePosition, maxHealth: 1000);

            // Gem den oprindelige keyboardstate for edge detection
            previousKeyboardState = Keyboard.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load baggrund og statiske objekter
            backgroundTexture = Content.Load<Texture2D>("Assets/TinySwords/Background");
            mineTexture = Content.Load<Texture2D>("Assets/TinySwords/Resources/Gold Mine/GoldMine_Inactive");
            baseTexture = Content.Load<Texture2D>("Assets/TinySwords/Factions/Knights/Buildings/Castle/Castle_Blue");
            enemyBaseTexture = Content.Load<Texture2D>("Assets/TinySwords/Factions/Goblins/Buildings/Wood_House/Goblin_House");

            // Load enhedsteksturer
            minerUnitTexture = Content.Load<Texture2D>("Assets/Humans/Outline/MiniSwordMan/MiniSwordMan_Run001");
            soldierUnitTexture = Content.Load<Texture2D>("Assets/Humans/Outline/MiniHalberdMan/MiniHalberdMan_Run001");

            // Tildel TownHall sin tekstur (basen vises med baseTexture)
            townHall.Texture = baseTexture;
        }

        protected override void Update(GameTime gameTime)
        {
            // Afslut spillet ved Escape eller Back-knap
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState currentKeyboardState = Keyboard.GetState();

            // Køb Miner ved tryk på "M"
            if (currentKeyboardState.IsKeyDown(Keys.M) && previousKeyboardState.IsKeyUp(Keys.M))
            {
                if (townHall.UseGold(MinerCost))
                {
                    // Opret en ny Miner:
                    // - Startposition: TownHall-position
                    // - Bevægelseshastighed: 50f, health: 100
                    // - Mine-position: Brug den definerede minePosition
                    // - Base-position: Brug TownHall's position
                    // - Reference til TownHall for at aflevere guld
                    Miner newMiner = new Miner(townHall.Position, 50f, 100, minePosition, townHall.Position, townHall);
                    newMiner.Texture = minerUnitTexture;
                    newMiner.Start();
                    miners.Add(newMiner);
                }
            }

            // Køb Soldier ved tryk på "S"
            if (currentKeyboardState.IsKeyDown(Keys.S) && previousKeyboardState.IsKeyUp(Keys.S))
            {
                if (townHall.UseGold(SoldierCost))
                {
                    // Opret en ny Soldier:
                    // - Start ved TownHall-position, bevægelseshastighed: 80f, health: 100
                    // - Mål: enemyBasePosition
                    Soldier newSoldier = new Soldier(townHall.Position, 80f, 100, enemyBasePosition);
                    newSoldier.Texture = soldierUnitTexture;
                    newSoldier.Start();
                    soldiers.Add(newSoldier);
                }
            }

            // Gem den aktuelle tastaturtilstand til næste update (edge detection)
            previousKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Tegn baggrund og statiske objekter
            _spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
            _spriteBatch.Draw(mineTexture, minePosition, Color.White);
            _spriteBatch.Draw(mineTexture, minePosition2, Color.White);
            _spriteBatch.Draw(enemyBaseTexture, enemyBasePosition, Color.White);

            // Tegn TownHall med UI-overlay (gold counter og health)
            townHall.Draw(_spriteBatch);

            // Tegn alle Miner-enheder
            foreach (Miner miner in miners)
            {
                miner.Draw(_spriteBatch);
            }

            // Tegn alle Soldier-enheder
            foreach (Soldier soldier in soldiers)
            {
                soldier.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            // Stop alle enhedstråde for at undgå hængende tråde
            foreach (var miner in miners)
            {
                miner.Stop();
            }
            foreach (var soldier in soldiers)
            {
                soldier.Stop();
            }
            base.OnExiting(sender, args);
        }
    }
}
