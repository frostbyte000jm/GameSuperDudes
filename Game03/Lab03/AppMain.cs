using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.UI;
using Sce.PlayStation.Core.Audio;

namespace Lab03
{
	public class AppMain
	{
		private static GraphicsContext graphics;
		private static BackGround background;
		private static Foreground foreground;
		private static List<Player> heroes;
		private static List<Enemy> npcs;
		private static List<Ammo> npcAmmo, pcAmmo;
		private static Stopwatch clock;
		private static long startTime, stopTime, timeDelta, buttonStartTimeCounter,P1AtkTimeCounter, P2AtkTimeCounter;
		private static bool doRunGame = true, isPC1, isPC2, isPC3, isPC4;
		private static bool player1IsRight, player2IsRight;
		private static bool doButtonCircle, doButtonCross, doButtonDown, doButtonL, doButtonLeft, doButtonR, doButtonRight, doButtonSelect, doButtonSquare, doButtonStart, doButtonTriangle, doButtonUp, anyKey;
		private static int heroMoveX, heroMoveY, hero2MoveX, hero2MoveY, totalCharacterNum, tempDeadCount, UniqueIDChar1, UniqueIDChar2, UniqueIDChar3, UniqueIDChar4;
		private static string character1, character2, character3, character4, characterName1, characterName2, characterName3, characterName4;
		private static HUD hud;
		private static Random rand;
		private enum GameState{Menu, Playing, Paused, Dead, GameOver};
		private static GameState currentState;
		private static Sprite menuScreen;
		private static Texture2D imageMenu;
		private static BgmPlayer bgmp;
		
		public static void Main (string[] args)
		{
			Initialize ();

			while (doRunGame) 
			{
				startTime = clock.ElapsedMilliseconds;
				SystemEvents.CheckEvents ();
				Update ();
				Render ();
				stopTime = clock.ElapsedMilliseconds;
				timeDelta = stopTime - startTime;
			}
		}

		public static void Initialize ()
		{
			// Set up the graphics system
			graphics = new GraphicsContext ();
			
			//Background and Foreground
			background = new BackGround(graphics);
			foreground = new Foreground(graphics);
			
			//added stopwatch
			clock = new Stopwatch();
			clock.Start();
			
			
			//Name the Characters
			character1 = "GoodGuy";
			character2 = "GoodGuy2";
			character3 = "Villain";
			character4 = "Villain2";
			characterName1 = "Stuper Man";
			characterName2 = "BafMan";
			characterName3 = "Lexus Luthor";
			characterName4 = "Trickster";
			isPC1 = true;
			isPC2 = true;
			isPC3 = false;
			isPC4 = false;
			UniqueIDChar1 = 0;
			UniqueIDChar2 = 1;
			UniqueIDChar3 = 0;
			UniqueIDChar4 = 1;
			totalCharacterNum = 4;
			
			//background Music
			Bgm bgm = new Bgm("/Application/Assets/fight.mp3");
			bgmp = bgm.CreatePlayer();
			bgmp.Loop = true;
			bgmp.Play();
			
						
			//Creating image for Menu Screen
			imageMenu = new Texture2D("/Application/Assets/Menu Screen.png",false);
			menuScreen = new Sprite(graphics, imageMenu);
			
			//setting state to Menu.
			currentState = GameState.Menu;
		}
		
		public static void NewGame()
		{
			//declarations
			string characterImage = "", characterName = "";
			float startPosX, startPosY, startYPcnt;
			bool isPC = true;
			int UniqueIDChar = 0;
			Player pc;
			Enemy npc;
			heroes = new List<Player>();
			npcs = new List<Enemy>();
			hud = new HUD (graphics, totalCharacterNum);
			npcAmmo = new List<Ammo>();
			pcAmmo = new List<Ammo>();
			buttonStartTimeCounter = 0;
//			locationTimer = 0;
			rand = new Random();
			
			
			//Create PCs and NPCs
			for (int i = 0; i <4; i++)
			{
				startPosX = graphics.Screen.Rectangle.Width;
				startPosY = graphics.Screen.Rectangle.Height;
				if(i == 0)
				{
					characterImage = character1;
					characterName = characterName1;
					isPC = isPC1;
					UniqueIDChar = UniqueIDChar1;
					startPosX = startPosX * 0.25f;
					if (character2 != "")
						startYPcnt = 0.33f;
					else
						startYPcnt = 0.5f;
					startPosY = startPosY * startYPcnt;
				}
				
				else if (i == 1)
				{
					characterImage = character2;
					characterName = characterName2;
					isPC = isPC2;
					UniqueIDChar = UniqueIDChar2;
					startPosX = startPosX * 0.25f;
					startPosY = startPosY * 0.66f;
				}
				
				else if (i == 2)
				{
					characterImage = character3;
					characterName = characterName3;
					isPC = isPC3;
					UniqueIDChar = UniqueIDChar3;
					startPosX = startPosX * 0.75f;
					if (character4 != "")
						startYPcnt = 0.33f;
					else
						startYPcnt = 0.5f;
					startPosY = startPosY * startYPcnt;
				}
				else if (i == 3)
				{
					characterImage = character4;
					characterName = characterName4;
					isPC = isPC4;
					UniqueIDChar = UniqueIDChar4;
					startPosX = startPosX * 0.75f;
					startPosY = startPosY * 0.66f;
				}
								
				if (characterImage != "")
				{
					//build player/npc and add to list.
					if (isPC)
					{
						pc = new Player(UniqueIDChar, characterImage, startPosX, startPosY, graphics, characterName);
						heroes.Add(pc);
					}
					else
					{
						npc = new Enemy(UniqueIDChar, characterImage, startPosX, startPosY, graphics, characterName);
						npcs.Add(npc);
					}
					//add info to HUD
					hud.UpdateNames(characterName, i);
				}
			}
		}
		
		//Choose statements for which State we are in.
		public static void Update ()
		{
			switch(currentState)
			{
				case GameState.Menu : UpdateMenu(); break;
				case GameState.Dead : UpdateDead(); break;
				case GameState.Paused : UpdatePaused(); break;
				case GameState.Playing : UpdatePlaying(); break;
				case GameState.GameOver : UpdateGameOver(); break;
			}
		}
		
		//GameOver Dude!
		public static void UpdateDead ()
		{
			GetGamePadData();
			hud.UpdatePausedText("You Dead, press 'Start' for new game.");
			
			buttonStartTimeCounter += timeDelta;
			if (buttonStartTimeCounter >500)
			{
				if (doButtonStart)
				{
					buttonStartTimeCounter = 0;
					currentState = GameState.Menu;					
				}
			}
			
			ClearGamePadData();
		}
		
		//You Win Dude!
		public static void UpdateGameOver ()
		{
			GetGamePadData();
			hud.UpdatePausedText("Congratz you are a winner! Press Start.");
			
			buttonStartTimeCounter += timeDelta;
			if (buttonStartTimeCounter >500)
			{
				if (doButtonStart)
				{
					buttonStartTimeCounter = 0;
					currentState = GameState.Menu;					
				}
			}
			
			ClearGamePadData();
		}
		
		//Paused Screen
		public static void UpdatePaused ()
		{
			GetGamePadData();
			hud.UpdatePausedText("Paused! Press 'Start' to Continue or 'Select' to Quit.");
			
			buttonStartTimeCounter += timeDelta;
			if (buttonStartTimeCounter >500)
			{
				if (doButtonStart)
				{
					buttonStartTimeCounter = 0;
					currentState = GameState.Playing;
					hud.UpdatePausedText("");
					
				}
				
				//End Game 
				if (doButtonSelect)
				doRunGame = false;
			}
			
			ClearGamePadData();
		}
		
		//Game Menu
		public static void UpdateMenu ()
		{
			GetGamePadData();
			
						
			
			buttonStartTimeCounter += timeDelta;
			if (buttonStartTimeCounter >500)
			{
				if (doButtonStart)
				{
					NewGame();
					currentState = GameState.Playing;
					buttonStartTimeCounter = 0;
				}
				
				//End Game 
				if (doButtonSelect)
				doRunGame = false;				
			}
			
			ClearGamePadData();
			
		}
		
		//Game Update
		public static void UpdatePlaying ()
		{	
			GetGamePadData();
								
			// Enter Pause State
			buttonStartTimeCounter += timeDelta;
			if (buttonStartTimeCounter > 500)
			{
				if (doButtonStart)
				{
					buttonStartTimeCounter = 0;
					currentState = GameState.Paused;
				}
			}
			
			HandlePlayer();
			HandleEnemy();		
			HandleHit();
			
			//Sending Score and HP to HUD.
			for (int i = 0; i < heroes.Count; i++)
			{
				hud.UpdateScoreBoard(heroes[i].Score, i);
				hud.UpdateHitPoints(heroes[i].HitPoints, i);
			}
			for (int i = 0; i < npcs.Count; i++)
			{
				hud.UpdateScoreBoard(npcs[i].Score, i+heroes.Count);
				hud.UpdateHitPoints(npcs[i].HitPoints, i+heroes.Count);
			}
			
			//Dead
			tempDeadCount = heroes[0].Score + heroes[1].Score;
			if (tempDeadCount == -10)
				currentState = GameState.Dead;
			
			//images Foreground & background.
			background.Update();
			foreground.Update();
			
			ClearGamePadData();
		}
		
		//Player Controls
		private static void HandlePlayer()
		{
			//player 1
			if (doButtonLeft)
			{
				heroMoveX--;
				player1IsRight = false;
			}
			if (doButtonRight)
			{
				heroMoveX++;
				player1IsRight = true;
			}
			if (doButtonUp)
				heroMoveY--;
			if (doButtonDown)
				heroMoveY++;
			heroes[0].Update(heroMoveX, heroMoveY, heroes);
			
			//player 2
			if (character2 != "")
			{
				if (doButtonSquare)
				{
					hero2MoveX--;
					player2IsRight = false;
				}
				if (doButtonCircle)
				{
					hero2MoveX++;
					player2IsRight = true;
				}
				if (doButtonTriangle)
					hero2MoveY--;
				if (doButtonCross)
					hero2MoveY++;
				heroes[1].Update(hero2MoveX, hero2MoveY, heroes);
			}
			
			//Player 1 atk
			P1AtkTimeCounter += timeDelta;
			if (P1AtkTimeCounter> 1000)
			{
				if (doButtonR && heroes[0].IsDead == false)
				{

					int speed = 0;
					if(player1IsRight)
						speed = 5;
					else
						speed = -5;
						
					Ammo aa = new Ammo(graphics, heroes[0].X, heroes[0].Y, heroes[0].CharacterImage, speed, heroes[0].UniqueID);
					pcAmmo.Add(aa);
					
					P1AtkTimeCounter = 0;
				}
				
			}
			
			//Player 2 Attack
			P2AtkTimeCounter += timeDelta;
			if (P2AtkTimeCounter> 1000)
			{
				if (doButtonL && heroes[1].IsDead == false)
				{

					int speed = 0;
					if(player2IsRight)
						speed = 5;
					else
						speed = -5;
						
					Ammo aa = new Ammo(graphics, heroes[1].X, heroes[1].Y, heroes[1].CharacterImage, speed, heroes[1].UniqueID);
					pcAmmo.Add(aa);
					
					P2AtkTimeCounter = 0;
				}
					
			}
			
			//Player Attack Update
			foreach (Ammo a in pcAmmo)
			{
				a.Update();
			}
			
			//Player Attack Cleanup
			for (int i = pcAmmo.Count-1; i>=0; i--)
			{
				
				if (pcAmmo[i].CleanUp)
					pcAmmo.RemoveAt(i);
			}
			
			//Both Players are Dead
			if (heroes[0].CleanUp && heroes[1].CleanUp)
				currentState = GameState.Dead;
		}

		//Enemy
		public static void HandleEnemy()
		{
			//Update Enemy Animation
			foreach (Enemy e in npcs)
			{
				int r = rand.Next(1,10);
				e.Update(anyKey, timeDelta, r);
			}
			
			//Enemy Attack calcs
			foreach (Enemy bg in npcs)
			{
				if (bg.DoAttack && bg.IsDead == false)
				{
					int speed=0;
					int r = rand.Next(2);
					if(r == 1)
						speed = 5;
					else
						speed = -5;
					Ammo aa = new Ammo(graphics, bg.X, bg.Y, bg.CharacterImage, speed, bg.UniqueID);
					npcAmmo.Add(aa);
				}
			}
			
			////Enemy Attack Update
			foreach (Ammo a in npcAmmo)
			{
				a.Update();
			}
			
			//Enemy Attack Cleanup
			for (int i = npcAmmo.Count-1; i>=0; i--)
			{
				
				if (npcAmmo[i].CleanUp)
					npcAmmo.RemoveAt(i);
			}
			
			bool isBGDead = true;
			foreach (Enemy bg in npcs)
			{
				if (bg.CleanUp && isBGDead)
					isBGDead = true;
				else
					isBGDead = false;
			}
			if (isBGDead)
				currentState = GameState.GameOver;
		}
		
		//Track Damage
		public static void HandleHit()
		{
			//Player Damage
			for (int a = 0; a < npcAmmo.Count; a++)
			{
				for (int h =0; h <heroes.Count; h++)
				{
					Rectangle ammoBox = npcAmmo[a].imageBox;
					Rectangle heroBox = heroes[h].SpriteBox;
					if (Overlaps(heroBox, ammoBox) == true)
					{
						npcAmmo[a].CleanUp = true;
						heroes[h].TakeDamage();
						npcs[npcAmmo[a].CharacterIndex].IncreaseScore();
					}
				}
			}
			
			//NPC Damage
			for (int a = 0; a < pcAmmo.Count; a++)
			{
				for (int h =0; h <npcs.Count; h++)
				{
					Rectangle ammoBox = pcAmmo[a].imageBox;
					Rectangle npcBox = npcs[h].imageBox;
					if (Overlaps(npcBox, ammoBox) == true)
					{
						pcAmmo[a].CleanUp = true;
						npcs[h].TakeDamage();
						heroes[pcAmmo[a].CharacterIndex].IncreaseScore();
					}
				}
			}
		}


		//Choose for Render
		public static void Render ()
		{
			switch(currentState)
			{
				case GameState.Menu : RenderMenu(); break;
				case GameState.Dead : RenderDead(); break;
				case GameState.Paused : RenderPaused(); break;
				case GameState.Playing : RenderPlaying(); break;
				case GameState.GameOver : RenderGameOver(); break;
			}
		}
		
		//Dead Screen
		public static void RenderDead ()
		{
			/// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear ();
			
			hud.Render();
						
			// Present the screen
			graphics.SwapBuffers ();
		}
		
		//Game Over Winner
		public static void RenderGameOver ()
		{
			/// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear ();
			
			hud.Render();
						
			// Present the screen
			graphics.SwapBuffers ();
		}
		
		//Paused Screen
		public static void RenderPaused ()
		{
			/// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear ();
			
			hud.Render();
						
			// Present the screen
			graphics.SwapBuffers ();
		}
		
		//Menu Screen
		public static void RenderMenu ()
		{
			/// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear ();
			
			menuScreen.Render();
			
			// Present the screen
			graphics.SwapBuffers ();
		}
		
		//Game Screen
		public static void RenderPlaying ()
		{
			/// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear ();

			// Render Stuff use Order of Operations here
			background.Render();
			//new: this now loads all pc's. 
			foreach (Player pc in heroes)
				pc.Render();
			foreach (Enemy e in npcs)
				e.Render();
			foreach (Ammo a in npcAmmo)
				a.Render();
			foreach (Ammo a in pcAmmo)
				a.Render();
			foreground.Render();
			hud.Render();

			// Present the screen
			graphics.SwapBuffers ();
		}
		
		
		//************Game Pad Data. Everything below here should not need any upkeep.********************************************
		public static void GetGamePadData()
		{
			// Query gamepad for current state
			var gamePadData = GamePad.GetData (0);
			
			/*
			 * I am not sure I like this, but the below will check to see if the any button is being pressed.
			 * once this method is completed all buttons are set back to false.
			 * Now I will be able to call "doButton???" where ??? is the button that is being pushed and manipulate it from there.
			 * I will no longer have the check the big if ((gamePadData.Buttons & GamePadButtons.blah) !0) anymore.
			 * I also went ahead and added all the buttons because at some point I will use them.
			*/
			if ((gamePadData.Buttons & GamePadButtons.Circle) !=0)	
			{	
				doButtonCircle = true;
				anyKey = true;
			}	
			if ((gamePadData.Buttons & GamePadButtons.Cross) !=0)	
			{	
				doButtonCross = true;
				anyKey = true;
			}	
			if ((gamePadData.Buttons & GamePadButtons.Down) !=0)	
			{	
				doButtonDown = true;
				anyKey = true;
			}	
			if ((gamePadData.Buttons & GamePadButtons.L) !=0)	
			{	
				doButtonL = true;
				anyKey = true;
			}	
			if ((gamePadData.Buttons & GamePadButtons.Left) !=0)	
			{	
				doButtonLeft = true;
				anyKey = true;
			}	
			if ((gamePadData.Buttons & GamePadButtons.R) !=0)	
			{	
				doButtonR = true;
				anyKey = true;
			}	
			if ((gamePadData.Buttons & GamePadButtons.Right) !=0)	
			{	
				doButtonRight = true;
				anyKey = true;
			}	
			if ((gamePadData.Buttons & GamePadButtons.Select) !=0)	
			{	
				doButtonSelect = true;
				anyKey = true;
			}	
			if ((gamePadData.Buttons & GamePadButtons.Square) !=0)	
			{	
				doButtonSquare = true;
				anyKey = true;
			}	
			if ((gamePadData.Buttons & GamePadButtons.Start) !=0)	
			{	
				doButtonStart = true;
				anyKey = true;
			}	
			if ((gamePadData.Buttons & GamePadButtons.Triangle) !=0)	
			{	
				doButtonTriangle = true;
				anyKey = true;
			}	
			if ((gamePadData.Buttons & GamePadButtons.Up) !=0)	
			{	
				doButtonUp = true;
				anyKey = true;
			}
		}
		
		//button and movement reset
		public static void ClearGamePadData()
		{
			heroMoveX = 0;
			heroMoveY = 0;
			hero2MoveX = 0;
			hero2MoveY = 0;
			doButtonCircle = false;
			doButtonCross = false;
			doButtonDown = false;
			doButtonL = false;
			doButtonLeft = false;
			doButtonR = false;
			doButtonRight = false;
			doButtonSelect = false;
			doButtonSquare = false;
			doButtonStart = false;
			doButtonTriangle = false;
			doButtonUp = false;
			anyKey = false;
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