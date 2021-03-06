﻿using Dominus_RPG_Core;
using Dominus_RPG_Core.Commands;
using Main_Test_Project.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Main_Test_Project
{
    internal class Game : DominusRPGGame
    {
        public Game(RPGGameProperties rpgGameProperties)
            : base(rpgGameProperties)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            this.Graphics.PreferredBackBufferWidth = 1024;
            this.Graphics.PreferredBackBufferHeight = 768;
            this.Graphics.ApplyChanges();

            this.GameConsole.Options.OpenOnWrite = false;
        }

        protected override void InitalizeScreens()
        {
            RPGGameProperties properties = new RPGGameProperties();
            properties.PlayerTexturePath = @"C:\Users\General\Desktop\char.png";

            var gameScreen = new GameScreen(this.Content, properties);
            this.ScreenManager.AddScreen(gameScreen, "GameScreen");
            this.ScreenManager.SetActiveScreen("GameScreen");
        }

        protected override void InitalizeConsoleCommands()
        {
            this.GameConsole.AddCommand(new MapInformationCommand(this.ScreenManager.GetScreen<GameScreen>("GameScreen")));
        }
    }
}