using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using DA_RTS.Classes.Castle;
using DA_RTS.Classes.Mines;
using DA_RTS.Classes.Units;
using DA_RTS.Classes.UI;
using System;
using System.Threading;
using DA_RTS.Classes.World.Environment;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

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

        private Thread minerUpdateThread;
        private bool minerThreadRunning = true;
        private readonly object minersLock = new object();
        private Thread soldierUpdateThread;
        private bool soldierThreadRunning = true;
        private readonly object soldierLock = new object();
        private List<Miner> miners;
        private List<Soldier> soldiers;

        private Texture2D smallBrushTexture;
        private Texture2D bigBrushTexture;
        private Texture2D smallRockTexture;
        private Texture2D mediumRockTexture;
        private Texture2D bigRockTexture;
        private Texture2D shroomTexture;
        private Texture2D treeTexture;

        private List<Tree> trees;
        private List<BigBrush> bigBrushes;
        private List<SmallBrush> smallBrushes;
        private List<BigRock> bigRocks;
        private List<MediumRock> mediumRocks;
        private List<SmallRock> smallRocks;
        private List<Shroom> shrooms;

        private int gold = 500;
        private int life = 100;
        private int soldierSpawnIndex = 0;

        private int enemyLife = 100;

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

            miners = new List<Miner>();
            soldiers = new List<Soldier>();

            minerUpdateThread = new Thread(UpdateMinerLogic);
            minerUpdateThread.IsBackground = true;
            minerUpdateThread.Start();

            soldierUpdateThread = new Thread(UpdateSoldierLogic);
            soldierUpdateThread.IsBackground = true;
            soldierUpdateThread.Start();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            previousKeyboardState = Keyboard.GetState();

            LoadTextures();
            LoadTileMap();
            LoadEnvironmentObjects();
            LoadCastles();
            LoadMines();
            LoadButtons();

            btnBuyMiner.Click += OnBuyMiner_Click;
            btnBuySoldier.Click += OnBuySoldier_Click;
        }

        public void LoadTextures()
        {
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
            treeTexture = Content.Load<Texture2D>("Assets/TinySwords/Resources/Trees/Cut/tile002");
            smallBrushTexture = Content.Load<Texture2D>("Assets/TinySwords/Deco/07");
            bigBrushTexture = Content.Load<Texture2D>("Assets/TinySwords/Deco/09");
            smallRockTexture = Content.Load<Texture2D>("Assets/TinySwords/Deco/04");
            mediumRockTexture = Content.Load<Texture2D>("Assets/TinySwords/Deco/05");
            bigRockTexture = Content.Load<Texture2D>("Assets/TinySwords/Deco/06");
            shroomTexture = Content.Load<Texture2D>("Assets/TinySwords/Deco/01");
        }

        public void LoadButtons()
        {
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

        public void LoadEnvironmentObjects()
        {
            trees = new List<Tree>();
            bigBrushes = new List<BigBrush>();
            smallBrushes = new List<SmallBrush>();
            shrooms = new List<Shroom>();
            smallRocks = new List<SmallRock>();
            mediumRocks = new List<MediumRock>();
            bigRocks = new List<BigRock>();

            //Her kan man indsætte objekter som følgende

            //trees.Add(new Tree(new Vector2(x, y), treeTexture));
            //bigBrushes.Add(new BigBrush(new Vector2(x, y), bigBrushTexture));
            //smallBrushes.Add(new SmallBrush(new Vector2(x, y), smallBrushTexture));
            //shrooms.Add(new Shroom(new Vector2(x, y), shroomTexture));
            //smallRocks.Add(new SmallRock(new Vector2(x, y), smallRockTexture));
            //mediumRocks.Add(new MediumRock(new Vector2(x, y), mediumRockTexture));
            //bigRocks.Add(new BigRock(new Vector2(x, y), bigRockTexture));
        }

        private void OnBuyMiner_Click(object sender, EventArgs e)
        {
            if (gold >= 100)
            {
                SpawnMiner();
            }
        }

        private void OnBuySoldier_Click(object sender, EventArgs e)
        {
            if (gold >= 200)
            {
                gold -= 200;
                // Juster spawn-position for at lave en række
                Vector2 offset = new Vector2(blueTownHall.Position.X + blueCastleTexture.Width - 250, blueTownHall.Position.Y + (blueCastleTexture.Height / 2) + (soldierSpawnIndex * 50));
                Soldier newSoldier = new Soldier(offset, soldierIcon, 100f, redTownHall.Position + new Vector2(-50, redCastleTexture.Height / 2));
                newSoldier.DamageDealt += Soldier_DamageDealt;
                soldiers.Add(newSoldier);

                // Skift spawnIndex for at placere næste soldat lidt forskudt
                soldierSpawnIndex = (soldierSpawnIndex + 1) % 5; // Maks 5 soldater i en række
            }
        }

        private void Miner_GoldDelivered(object sender, int deliveredGold)
        {
            gold += deliveredGold;
        }

        private void Soldier_DamageDealt(object sender, int damage)
        {
            enemyLife -= damage;
            if (enemyLife <= 0)
            {
                Console.WriteLine("Enemy Castle Destroyed");
            }
        }

        protected override void Update(GameTime gameTime)
        {
            btnBuyMiner.Update(gameTime);
            btnBuySoldier.Update(gameTime);
            PlayerInput(gameTime);

            base.Update(gameTime);
        }

        public void PlayerInput(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.M) && previousKeyboardState.IsKeyUp(Keys.M))
            {
                SpawnMiner();
            }
            previousKeyboardState = keyboardState;
        }

        public void SpawnMiner()
        {
            gold -= 100;
            Vector2 offset = new Vector2(100, -50);
            Vector2 minerStartPosition = blueTownHall.Position + offset;
            Miner newMiner = new Miner(minerStartPosition, blueMinerTexture, 100f, blueMine.GetPosition(), blueTownHall.Position);
            newMiner.GoldDelivered += Miner_GoldDelivered;
            miners.Add(newMiner);
        }
    
        

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            tileMap.Draw(_spriteBatch, 1.0f);
            DrawEnvironment(gameTime);

            blueTownHall.Draw(_spriteBatch, 0.4f);
            redTownHall.Draw(_spriteBatch, 0.4f);

            blueMine.Draw(_spriteBatch, 0.6f);
            redMine.Draw(_spriteBatch, 0.6f);

            btnBuyMiner.Draw(_spriteBatch, 0.0f);
            btnBuySoldier.Draw(_spriteBatch, 0.0f);

            DrawUI(gameTime);

            lock (minersLock)
            {
                foreach (Miner miner in miners)
                {
                    miner.Draw(_spriteBatch, 0.5f);
                }
            }

            lock (soldierLock)
            {
                foreach (Soldier soldier in soldiers)
                {
                    soldier.Draw(_spriteBatch, 0.3f);
                }
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DrawUI(GameTime gameTime)
        {
            Vector2 goldIconPosition = new Vector2(-25, 100);
            Vector2 heartIconPosition = new Vector2(20, 200);
            _spriteBatch.Draw(goldIcon, goldIconPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            _spriteBatch.Draw(heartIcon, heartIconPosition, null, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.0f);

            Vector2 goldTextPosition = goldIconPosition + new Vector2(100, 70);
            Vector2 lifeTextPosition = heartIconPosition + new Vector2(53, 15);
            _spriteBatch.DrawString(uiFont, $"{gold}", goldTextPosition, Color.Yellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            _spriteBatch.DrawString(uiFont, $"{life}", lifeTextPosition, Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
        }


        public void DrawEnvironment(GameTime gameTime)
        {
            if (trees != null)
            {
                foreach (Tree tree in trees)
                {
                    tree.Draw(_spriteBatch, 0.7f);
                }
            }

            if (bigBrushes != null)
            {
                foreach (BigBrush brush in bigBrushes)
                {
                    brush.Draw(_spriteBatch, 0.65f);
                }
            }

            if (smallBrushes != null)
            {
                foreach (SmallBrush brush in smallBrushes)
                {
                    brush.Draw(_spriteBatch, 0.66f);
                }
            }

            if (bigRocks != null)
            {
                foreach (BigRock rock in bigRocks)
                {
                    rock.Draw(_spriteBatch, 0.67f);
                }
            }

            if (mediumRocks != null)
            {
                foreach (MediumRock rock in mediumRocks)
                {
                    rock.Draw(_spriteBatch, 0.68f);
                }
            }

            if (smallRocks != null)
            {
                foreach (SmallRock rock in smallRocks)
                {
                    rock.Draw(_spriteBatch, 0.69f);
                }
            }

            if (shrooms != null)
            {
                foreach (Shroom shroom in shrooms)
                {
                    shroom.Draw(_spriteBatch, 0.7f);
                }
            }
        }

        protected override void UnloadContent()
        {
            // Stop miner opdateringstråden
            minerThreadRunning = false;
            if (minerUpdateThread != null && minerUpdateThread.IsAlive)
                minerUpdateThread.Join();

            soldierThreadRunning = false;
            if (soldierUpdateThread != null && soldierUpdateThread.IsAlive)
                    soldierUpdateThread.Join();

            base.UnloadContent();
        }

        private void UpdateMinerLogic()
        {
            while (minerThreadRunning)
            {
                float deltaTime = 0.035f;
                lock (minersLock)
                {
                    foreach (Miner miner in miners)
                    {
                        miner.UpdateLogic(deltaTime);
                    }
                }
                Thread.Sleep(16);
            }
        }

        private void UpdateSoldierLogic()
        {
            while (soldierThreadRunning)
            {
                float deltaTime = 0.035f;
                lock (soldierLock)
                {
                    foreach (Soldier soldier in soldiers)
                    {
                        soldier.UpdateLogic(deltaTime);
                    }
                }
                Thread.Sleep(16);
            }
        }
    }
}
