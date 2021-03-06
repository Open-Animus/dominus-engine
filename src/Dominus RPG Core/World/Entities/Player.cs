﻿using Dominus_Core.Graphics;
using Dominus_RPG_Core.Utilities;
using Dominus_RPG_Core.World.Entities.Stats;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Dominus_RPG_Core.World.Entities
{
    [Serializable]
    public class Player : IEntity
    {
        private Vector2 _position;
        private SpriteSheet _spriteSheet;
        private StatHandler _stats;
        private IEntityCombatHandler _combatHandler;

        public SpriteSheet SpriteSheet
        {
            get { return _spriteSheet; }
        }

        public bool Dead { get { throw new NotImplementedException(); } }

        public string Name { get; set; }

        public int Level { get; set; }

        public float Speed { get; set; }

        public Vector2 Range { get; set; }

        public Vector2 Position { get { return _position; } private set { _position = value; this.SpriteSheet.Position = value; } }

        public Rectangle Bounds { get; set; }

        public StatHandler Stats { get { return _stats; } }

        public IEntityCombatHandler CombatHandler { get { return _combatHandler; } }

        public event EventHandler Moved;

        public Player(Texture2D playerTexture)
        {
            _spriteSheet = new SpriteSheet(playerTexture, 3, 4, 32, 32);
            _stats = new StatHandler();
            _stats.AddStat(new Strength(100), "Strength");
            _stats.AddStat(new Defence(100), "Defence");
            _stats.AddStat(new Health(100), "Health");
            _combatHandler = new DefaultCombatHandler();
        }

        private void CheckInput()
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                this.Move(new Vector2(0, -this.Speed));
                this.SpriteSheet.HorizontalFrameIndex++;
                this.SpriteSheet.VerticalFrameIndex = 3;
            }

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                this.Move(new Vector2(0, this.Speed));
                this.SpriteSheet.HorizontalFrameIndex++;
                this.SpriteSheet.VerticalFrameIndex = 0;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                this.Move(new Vector2(-this.Speed, 0));
                this.SpriteSheet.HorizontalFrameIndex++;
                this.SpriteSheet.VerticalFrameIndex = 1;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                this.Move(new Vector2(this.Speed, 0));
                this.SpriteSheet.HorizontalFrameIndex++;
                this.SpriteSheet.VerticalFrameIndex = 2;
            }
        }

        private void Move(Vector2 distance)
        {
            if (this.Bounds.Contains(new Point((int)(this.Position.X + distance.X), (int)(this.Position.Y + distance.Y))))
            {
                this.Position += distance;

                if (this.Moved != null)
                    this.Moved.Invoke(this, new EntityMovedEventArgs(distance));
            }
        }

        public void Update(GameTime gameTime)
        {
            this.CheckInput();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _spriteSheet.Draw(spriteBatch);
        }

        public virtual void Save(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, this);
            }
        }

        public static Player Load(string filePath)
        {
            Player player;

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                player = formatter.Deserialize(fileStream) as Player;
            }

            return player;
        }
    }
}