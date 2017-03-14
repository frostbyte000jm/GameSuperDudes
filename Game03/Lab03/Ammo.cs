using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

namespace Lab03
{
	public class Ammo
	{
		private Sprite s;
		private GraphicsContext graphics;
		private int speed, characterIndex;
		private string weaponImage;
		private bool cleanUp;
		
		public bool CleanUp{get {return cleanUp;} set{cleanUp = value;}}
		public Rectangle imageBox {get {return new Rectangle(s.Position.X, s.Position.Y, s.Width, s.Height);}}
		public bool IsAlive;
		public int CharacterIndex{get {return characterIndex;}}
		
		public Ammo (GraphicsContext gc, float x, float y, string character, int spd, int uID)
		{
			//declarations
			speed = spd;			
			cleanUp = false;
			IsAlive = true;
			characterIndex = uID;
			
			//Comeback to me to use uID.
			if (character == "GoodGuy")
				weaponImage = "Laser";
			if (character == "GoodGuy2")
				weaponImage = "Baferang";
			if (character == "Villain")
				weaponImage = "CrypticNight";
			if (character == "Villain2")
				weaponImage = "Granade";
			
			//Create image.
			graphics = gc;
			Texture2D t = new Texture2D("/Application/Assets/" + weaponImage + ".png",false);
			s = new Sprite(graphics, t);
			s.Position.X = x;
			s.Position.Y = y;
			
			
		}
		
		public void Update()
		{
			s.Position.X += speed;
			
			if (s.Position.X <0 || s.Position.X > graphics.Screen.Rectangle.Width)
				cleanUp = true;
			
			
		}
		
		public void Render()
		{
			s.Render();
		}
	}
}

