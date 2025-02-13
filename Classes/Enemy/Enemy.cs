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
    internal class Enemy
    {
        private AIController aiController;

        public Enemy(TownHall enemyTownHall, Mine playerMine, Texture2D minerTexture, Texture2D soldierTexture)
        {
            aiController = new AIController(enemyTownHall, playerMine, minerTexture, soldierTexture);
        }

        public void StopAI()
        {
            aiController.StopAI();
        }
    }
}
