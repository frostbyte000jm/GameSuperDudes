using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

namespace Lab03
{
	public class BackGround
	{
		private Sprite clouds, clouds2, clouds3, clouds4;
		private GraphicsContext graphics;
		
		public BackGround (GraphicsContext gc)
		{
			graphics = gc;
			
			/*
			 * I am bringing in four clouds here. (think box split into 4s)
			 * I am moving them up at an angle. Once the first box leaves the screen (both X&Y) it resets.
			 */
			Texture2D imageClouds = new Texture2D("/Application/Assets/Clouds Back.png",false);
			clouds = new Sprite(graphics, imageClouds);
			clouds.Position.X = 0; 
			clouds.Position.Y = 0;
			clouds2 = new Sprite(graphics, imageClouds);
			clouds2.Position.X = clouds.Width; 
			clouds2.Position.Y = 0;
			clouds3 = new Sprite(graphics, imageClouds);
			clouds3.Position.X = 0; 
			clouds3.Position.Y = clouds.Height;
			clouds4 = new Sprite(graphics, imageClouds);
			clouds4.Position.X = clouds.Width; 
			clouds4.Position.Y = clouds.Height;
		}
		
		public void Update()
		{
			
			//moving the clouds.
			clouds.Position.X--;
			clouds2.Position.X--;
			clouds3.Position.X--;
			clouds4.Position.X--;
			clouds.Position.Y -= 0.25f;
			clouds2.Position.Y -= 0.25f;
			clouds3.Position.Y -= 0.25f;
			clouds4.Position.Y -= 0.25f;
			
			//Clouds Reset
			if (clouds.Position.X <= -clouds.Width)
			{
				clouds.Position.X = 0;
				clouds2.Position.X = clouds.Width;
				clouds3.Position.X = 0;
				clouds4.Position.X = clouds.Width;
			}
			if (clouds.Position.Y <= -clouds.Height)
			{
				clouds.Position.Y = 0;
				clouds2.Position.Y = 0;
				clouds3.Position.Y = clouds.Height;
				clouds4.Position.Y = clouds.Height;
			}
			
			
		}
		
		public void Render()
		{
			clouds.Render();
			clouds2.Render();
			clouds3.Render();
			clouds4.Render();
		}
		
	}
}

