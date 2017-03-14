using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

namespace Lab03
{
	public class Enemy
	{
		private Sprite enemy;
		private GraphicsContext graphics;
		private float maxRight, maxDown, positionX, positionY;
		private int badGuySpeedX = 3, badGuySpeedY = 4, score, randomInt, hitPoints, uniqueID;
		private bool enemyStart, doAttack, isDead, cleanUp;
		private string enemyName, location, characterImage;
		private long attackTimer;
		
		//public names.
		public string PlayerName {get {return enemyName;}}
		public int Score{get {return score;}}
		public int HitPoints{get {return hitPoints;}}
		public string Location{get {return location;}}
		public string CharacterImage{get {return characterImage;}}
		public bool DoAttack{get {return doAttack;}}
		public float X{get {return positionX;}}
		public float Y{get {return positionY;}}
		public Rectangle imageBox {get {return new Rectangle(enemy.Position.X, enemy.Position.Y, enemy.Width, enemy.Height);}}
//		public bool IsHit;
		public int UniqueID {get {return uniqueID;}}
		public bool CleanUp {get {return cleanUp;}}
		public bool IsDead {get {return isDead;}}

		public Enemy (int uID, string ci, float startPointX, float startPointY, GraphicsContext gc, string en)
		{
			graphics = gc;
			enemyName = en;
			characterImage = ci;
			hitPoints = 25;
			uniqueID = uID;
			
			//create and place enemy
			Texture2D imageEnemy = new Texture2D("/Application/Assets/"+characterImage+".png",false);
			enemy = new Sprite(graphics, imageEnemy);
			enemy.Position.X = startPointX - enemy.Width /2; 
			enemy.Position.Y = startPointY - enemy.Height /2;
			
//			IsHit = false;
			enemyStart = false;
			isDead = false;
			cleanUp = false;
		}
		
		public void Update(bool anyKey, long timeDelta, int rand)
		{
			if (anyKey)
				enemyStart = true;
			
			// Alive? Play. Dead? Fall off the screen. 
			if (isDead)
			{
				enemy.Position.Y = Math.Min(enemy.Position.Y + 1, graphics.Screen.Rectangle.Height);
				if (graphics.Screen.Rectangle.Height == enemy.Position.Y)
					cleanUp = true;
			}
			else
			HandleMovement(enemyStart);
			
			//Update Location information
			positionX = enemy.Position.X;
			positionY = enemy.Position.Y;
			
			randomInt = rand;
			attackTimer += timeDelta;
			Attack();
		}
		
		private void HandleMovement(bool doMove)
		{
			// creating some max movements here.
			maxRight = graphics.Screen.Rectangle.Width - enemy.Width;
			maxDown = graphics.Screen.Rectangle.Height - enemy.Height;
			
			//Moving the NPCs
			if(doMove)
			{
				if ((enemy.Position.X + badGuySpeedX >maxRight) || (enemy.Position.X + badGuySpeedX <0))
				{
					badGuySpeedX = -badGuySpeedX;
				}
				enemy.Position.X += badGuySpeedX;
					
				if ((enemy.Position.Y + badGuySpeedY >maxDown) || (enemy.Position.Y + badGuySpeedY <0))
				{
					badGuySpeedY = -badGuySpeedY;
				}
				enemy.Position.Y += badGuySpeedY;
			}
		}
		
		//ScoreKeeper
		public void IncreaseScore()
		{
			score += 1;
		}
		
		public void DecreaseScore()
		{
			score -= 1;
		}
		
		public void TakeDamage()
		{
			hitPoints = Math.Max (hitPoints-1,0);
			if (hitPoints==0)
				isDead = true;
		}
		
		//Keep track of the NPCs
		public void WhereAmI()
		{
			location = "("+positionX+", "+positionY+")";
		}
		
		public void Attack()
		{
			doAttack = false;
			if (attackTimer > randomInt*1000)
			{
				doAttack = true;
				attackTimer = 0;
			}
		}
		
		public void Render()
		{
			enemy.Render();
		}
		
	}
}

