using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

namespace Lab03
{
	public class Foreground
	{
		private Sprite clouds, clouds2;
		private GraphicsContext graphics;
		
		public Foreground (GraphicsContext gc)
		{
			graphics = gc;
			
			/*
			 * I am bringing in two pictures of random clouds stacked on top of each other.
			 * I am moving them up at an angle. Once the first box leaves the screen (Both X&Y) it resets.
			 */
			Texture2D imageClouds = new Texture2D("/Application/Assets/Clouds Front.png",false);
			clouds = new Sprite(graphics, imageClouds);
			clouds.Position.X = 0; 
			clouds.Position.Y = 0;
			clouds2 = new Sprite(graphics, imageClouds);
			clouds2.Position.X = 0; 
			clouds2.Position.Y = clouds.Height;			
		}
		
		public void Update()
		{
			//moving the clouds.
			clouds.Position.X -= 5;
			clouds2.Position.X -= 5;
			clouds.Position.Y -= 0.15f;
			clouds2.Position.Y -= 0.15f;
			
			//Cloud Reset
			if (clouds.Position.X <= -clouds.Width)
			{
				clouds.Position.X = 0;
				clouds2.Position.X = 0;
			}
			if (clouds.Position.Y <= -clouds.Height)
			{
				clouds.Position.Y = 0;
				clouds2.Position.Y = clouds.Height;				
			}
			
			
		}
		
		public void Render()
		{
			clouds.Render();
			clouds2.Render();
		}
		
	}
}

