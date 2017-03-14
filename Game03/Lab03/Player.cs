using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

namespace Lab03
{
	public class Player
	{
		private Sprite player;
		private GraphicsContext graphics;
		private int speed, score, uniqueID, hitPoints;
		private float maxRight, maxDown, playerMoveX, playerMoveY;
		private string playerName, characterImage;
		private Rectangle nextPlayerMoveX, nextPlayerMoveY;
		private bool isDead, cleanUp;
		
		//public names.
		public string PlayerName {get {return playerName;}}
		public int Score{get {return score;}}
		public int HitPoints{get {return hitPoints;}}
		public float X{get {return player.Position.X;}}
		public float Y{get {return player.Position.Y;}}
		public string CharacterImage{get {return characterImage;}}
		public Rectangle SpriteBox {get {return new Rectangle(player.Position.X, player.Position.Y, player.Width, player.Height);}}
		public int UniqueID {get {return uniqueID;}}
		public bool CleanUp {get {return cleanUp;}}
		public bool IsDead {get {return isDead;}}
		
		public Player (int uID, string ci, float startPointX, float startPointY, GraphicsContext gc, string pn)
		{
			uniqueID = uID;
			graphics = gc;
			playerName = pn;
			speed = 30;
			score = 0;
			hitPoints = 25;
			characterImage = ci;
						
			//create and place player
			Texture2D imagePlayer = new Texture2D("/Application/Assets/"+characterImage+".png",false);
			player = new Sprite(graphics, imagePlayer);
			player.Position.X = startPointX - player.Width /2; 
			player.Position.Y = startPointY - player.Height /2;
			
//			IsHit = false;
			isDead = false;
			cleanUp = false;
		}
		
		
		public void Update(int X_change, int Y_change, List<Player> heroes)
		{
			// Alive? Play. Dead? Fall off the screen. 
			if (isDead)
			{
				player.Position.Y = Math.Min(player.Position.Y + 1, graphics.Screen.Rectangle.Height);
				if (graphics.Screen.Rectangle.Height == player.Position.Y)
					cleanUp = true;
			}
			else
			HandleMovement(X_change, Y_change, heroes);
		}
		
		private void HandleMovement(int X_change, int Y_change, List<Player> heroes)
		{
			// creating some max movements here.
			maxRight = graphics.Screen.Rectangle.Width - player.Width;
			maxDown = graphics.Screen.Rectangle.Height - player.Height;
			playerMoveX = player.Position.X + X_change * speed;
			playerMoveY = player.Position.Y + Y_change * speed;
			nextPlayerMoveX = new Rectangle(playerMoveX+2, player.Position.Y+2, player.Width-5, player.Height-2);
			nextPlayerMoveY = new Rectangle(player.Position.X+2, playerMoveY+2, player.Width-5, player.Height-2);
			
			//X movement
			if (playerMoveX < 0)
				player.Position.X = 0;
			else if (playerMoveX > maxRight)
				player.Position.X = maxRight;
			else
			{
				foreach (Player p in heroes)
				{
					if (p.UniqueID != uniqueID)
					{
						if (Overlaps(nextPlayerMoveX, p.SpriteBox) != true)
						{
							player.Position.X = playerMoveX;
						}
					}
				}
			}
			
			//Y movement
			if (playerMoveY < 0)
				player.Position.Y = 0;
			else if (playerMoveY > maxDown)
				player.Position.Y = maxDown;
			else
			{
				foreach (Player p in heroes)
				{
					if (p.UniqueID != uniqueID)
					{
						if (Overlaps(nextPlayerMoveY, p.SpriteBox) != true)
						{
							player.Position.Y = playerMoveY;
						}
					}
				}
			}
		}
				
		//Score++
		public void IncreaseScore()
		{
			score++;
		}
		
		//Score--
		public void DecreaseScore()
		{
			score--;
		}
		
		//Track Damage
		public void TakeDamage()
		{
			hitPoints = Math.Max (hitPoints-1,0);
			if (hitPoints==0)
				isDead = true;
		}
		
		//Draw
		public void Render()
		{
			player.Render();
		}
		
		//Overlap Function
		private static bool Overlaps(Rectangle r1, Rectangle r2)
		{
			if (r1.X+r1.Width <r2.X)
				return false;
			if (r1.X> r2.X+r2.Width)
				return false;
			if (r1.Y+r1.Height <r2.Y)
				return false;
			if (r1.Y> r2.Y+r2.Height)
				return false;
			
			return true;
		}
		
	}
}

