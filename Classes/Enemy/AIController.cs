using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Threading;
using SharpDX.Direct2D1.Effects;
using DA_RTS.Classes.Units;
using DA_RTS.Classes.Castle;
using DA_RTS.Classes.Mines;

namespace DA_RTS.Classes.AI
{
    internal class AIController
    {
        private int gold = 500;
        private TownHall aiTownHall;
        private Mine enemyMine;
        private List<Miner> miners;
        private List<Soldier> soldiers;
        private Texture2D minerTexture;
        private Texture2D soldierTexture;
        private bool aiRunning = true;
        private readonly object aiLock = new object();

        private Thread aiThread;

        public AIController(TownHall aiTownHall, Mine enemyMine, Texture2D minerTexture, Texture2D soldierTexture)
        {
            this.aiTownHall = aiTownHall;
            this.enemyMine = enemyMine;
            this.minerTexture = minerTexture;
            this.soldierTexture = soldierTexture;
            miners = new List<Miner>();
            soldiers = new List<Soldier>();

            aiThread = new Thread(RunAILogic);
            aiThread.IsBackground = true;
            aiThread.Start();
        }

        private void RunAILogic()
        {
            while (aiRunning)
            {
                lock (aiLock)
                {
                    if (gold >= 100)
                    {
                        SpawnMiner();
                    }
                    else if (gold >= 400)
                    {
                        SpawnSoldier();
                    }
                }
                Thread.Sleep(2000); // Opdaterer AI hvert andet sekund
            }
        }

        private void SpawnMiner()
        {
            gold -= 100;
            Vector2 minerPosition = aiTownHall.Position + new Vector2(-50, 0);
            Miner miner = new Miner(minerPosition, minerTexture, 100f, enemyMine.GetPosition(), aiTownHall.Position);
            miner.GoldDelivered += Miner_GoldDelivered;
            miners.Add(miner);
        }

        private void SpawnSoldier()
        {
            gold -= 200;
            Vector2 soldierPosition = aiTownHall.Position + new Vector2(-50, 50 * soldiers.Count);
            Soldier soldier = new Soldier(soldierPosition, soldierTexture, 100f, new Vector2(100, 100)); // Spillerens TownHall
            soldier.DamageDealt += Soldier_DamageDealt;
            soldiers.Add(soldier);
        }

        private void Miner_GoldDelivered(object sender, int deliveredGold)
        {
            lock (aiLock)
            {
                gold += deliveredGold;
            }
        }

        private void Soldier_DamageDealt(object sender, int damage)
        {
            // Angriber spillerens base
        }

        public void StopAI()
        {
            aiRunning = false;
            if (aiThread != null && aiThread.IsAlive)
            {
                aiThread.Join();
            }
        }
    }
}
