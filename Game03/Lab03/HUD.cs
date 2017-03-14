using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.UI;

namespace Lab03
{
	/*
	 * ALL New: added HUD to keep track of players and Score. 
	 * The HUD takes in the graphics and total player numbers. From this it builds a base with default info. 
	 * 
	 * OK, This may not be exactly how it reads in specs, but I tried for hours to get this to work by putting the HUD into a list. after LOTS
	 * of research it appears that there can only be 1 scene. I may be wrong, but that is what I found. so instead of making 1 hud and putting it
	 * into a list on AppMain, I decided to make 1 HUD and put a list in the HUD. This way there is 1 scene, with many "objects" on the 1 scene.
	 * 
	 * There are two methods for actions that will update the Name and the other will update the Score. 
	*/
	public class HUD
	{
		private List<Label> characterName, characterScore, characterLocation, characterHP;
		private Label pausedText, ammoText;
			
		public HUD (GraphicsContext graphics, int totalPlayerNum)
		{
			UISystem.Initialize(graphics);
						
			//decarations
			Scene scene = new Scene();
			Label lblName, lblScore, lblLocation, lblHP;
			characterName = new List<Label>();
			characterHP = new List<Label>();
			characterScore = new List<Label>();
			characterLocation = new List<Label>();
			float colorRed = 1, colorGreen = 1, colorBlue = 1;
			int labelSpacing = graphics.Screen.Rectangle.Width / Math.Max(totalPlayerNum,1);
			
			//Loop to create a list of labels. 
			for(int characterNum = 0; characterNum <totalPlayerNum; characterNum++)
			{	
				if(characterNum == 0)
				{
					colorRed = 1;
					colorGreen = 0;
					colorBlue = 0;
				}
				if(characterNum == 1)
				{
					colorRed = 0;
					colorGreen = 1;
					colorBlue = 0;
				}
				if(characterNum == 2)
				{
					colorRed = 0;
					colorGreen = 0;
					colorBlue = 1;
				}
				if(characterNum == 3)
				{
					colorRed = 1;
					colorGreen = 1;
					colorBlue = 0;
				}
				
				//Name
				lblName = new Label();
				lblName.X = 0 + labelSpacing * (characterNum);
				lblName.Y = 10;
				lblName.Width = graphics.Screen.Rectangle.Width / Math.Max(totalPlayerNum,1);
				lblName.Text = "Name: ";
				lblName.TextColor = new UIColor (colorRed, colorGreen, colorBlue, 1);
				lblName.HorizontalAlignment = HorizontalAlignment.Center;
				scene.RootWidget.AddChildLast(lblName);
				characterName.Add(lblName);
				
				//HP
				lblHP = new Label();
				lblHP.X = 0 + labelSpacing * (characterNum);
				lblHP.Y = characterName[characterNum].Y + characterName[characterNum].Height;
				lblHP.Width = graphics.Screen.Rectangle.Width / Math.Max(totalPlayerNum,1);
				lblHP.Text = "HP: X";
				lblHP.TextColor = new UIColor (colorRed, colorGreen, colorBlue, 1);
				lblHP.HorizontalAlignment = HorizontalAlignment.Center;
				scene.RootWidget.AddChildLast(lblHP);
				characterHP.Add(lblHP);
				
				//Score
				lblScore = new Label();
				lblScore.X = 0 + labelSpacing * (characterNum);
				lblScore.Y = characterHP[characterNum].Y + characterHP[characterNum].Height;
				lblScore.Width = graphics.Screen.Rectangle.Width / Math.Max(totalPlayerNum,1);
				lblScore.Text = "Score: 0";
				lblScore.TextColor = new UIColor (colorRed, colorGreen, colorBlue, 1);
				lblScore.HorizontalAlignment = HorizontalAlignment.Center;
				scene.RootWidget.AddChildLast(lblScore);
				characterScore.Add(lblScore);
				
//				//where am I? (I now can show you where everyone is)
//				lblLocation = new Label();
//				lblLocation.X = 0 + labelSpacing * (characterNum);
//				lblLocation.Y = characterScore[characterNum].Y + characterScore[characterNum].Height;
//				lblLocation.Width = graphics.Screen.Rectangle.Width / Math.Max(totalPlayerNum,1);
//				lblLocation.Text = "";
//				lblLocation.TextColor = new UIColor (colorRed, colorGreen, colorBlue, 1);
//				lblLocation.HorizontalAlignment = HorizontalAlignment.Center;
//				scene.RootWidget.AddChildLast(lblLocation);
//				characterLocation.Add(lblLocation);
			}
			
			//where am I? (This is outside the loop because it should only render once.)
			pausedText = new Label();
			pausedText.Width = 600;
			pausedText.X = graphics.Screen.Rectangle.Width /2 - pausedText.Width /2;
			pausedText.Y = graphics.Screen.Rectangle.Height /2 - pausedText.Height /2;
			pausedText.HorizontalAlignment = HorizontalAlignment.Center;
			pausedText.Text = "";
			pausedText.TextColor = new UIColor (1, 0, 0, 1);
			scene.RootWidget.AddChildLast(pausedText);
			
			//			//where am I? (This is outside the loop because it should only render once.)
//			instructionText = new Label();
//			instructionText.X = 10;
//			instructionText.Y = graphics.Screen.Rectangle.Height - instructionText.Height - 10;
//			instructionText.Width = 800;
//			instructionText.Text = "Press 'Start' (keyboard X) to show location.";
//			instructionText.TextColor = new UIColor (1, 1, 1, 1);
//			scene.RootWidget.AddChildLast(instructionText);
			
//			//ammo on the screen count
//			ammoText = new Label();
//			ammoText.X = 10;
//			ammoText.Y = graphics.Screen.Rectangle.Height - instructionText.Height - 20;
//			ammoText.Width = 800;
//			ammoText.Text = "ammo: ";
//			ammoText.TextColor = new UIColor (1, 1, 1, 1);
//			scene.RootWidget.AddChildLast(ammoText);
						
			//set scene
			UISystem.SetScene(scene, null);
		}
		
		//Update the character names.
		public void UpdateNames(string charName, int index)
		{
			characterName[index].Text = "Name: "+ charName;
		}
		
		//Update HPs.
		public void UpdateHitPoints(int hp, int index)
		{
			characterHP[index].Text = "HP: "+hp;
		}
		
		//Update the Scores.
		public void UpdateScoreBoard(int score, int index)
		{
			characterScore[index].Text = "Score: "+score;
		}
		
		//Update the locations of the PCs and NPCs
		public void UpdateLocation(string locationInfo, int index)
		{
			characterLocation[index].Text = locationInfo;
		}
		
		public void UpdatePausedText(string input)
		{
			pausedText.Text = input;
			
		}
		
//		//ammo stuff
//		public void UpdateAmmo(int ammoCount)
//		{
//			ammoText.Text = "ammo: "+ ammoCount;
//		}
		
//		//Clean up Location text
//		public void ClearLocation()
//		{
//			foreach (Label l in characterLocation)
//				l.Text = "";
//			instructionText.Text = "";
//		}
		
		public void Render()
		{
			UISystem.Render();
		}
	}
}

