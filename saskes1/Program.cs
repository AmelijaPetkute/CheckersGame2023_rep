using System;
using System.Globalization;

class Program
{
	static void Main(string[] args)
	{
		//char[,] board = new[,]{
		//{'x', '-', 'x', '-', 'x', '-', 'x', '-'},
		//{'-', 'x', '-', 'x', '-', 'x', '-', 'x'},
		//{'x', '-', 'x', '-', 'x', '-', 'x', '-'},
		//{'-', '-' ,'-', '-', '-', '-', '-', '-'},
		//{'-', '-' ,'-', '-', '-', '-', '-', '-'},
		//{'-', 'o' ,'-', 'o', '-', 'o', '-', 'o'},
		//{'o' ,'-', 'o', '-', 'o', '-', 'o', '-'},
		//{'-', 'o' ,'-', 'o', '-', 'o', '-', 'o'}
		//};

		char[,] board = new[,]{
		{'x', '-', 'x', '-', 'x', '-', 'x', '-'},
		{'-', 'x', '-', 'x', '-', 'x', '-', 'x'},
		{'x', '-', 'x', '-', 'x', '-', '-', '-'},
		{'-', '-' ,'-', 'o', '-', 'o', '-', 'x'},
		{'-', '-' ,'-', '-', '-', '-', '-', '-'},
		{'-', 'o' ,'-', '-', '-', '-', '-', 'o'},
		{'o' ,'-', 'o', '-', 'o', '-', 'o', '-'},
		{'-', 'o' ,'-', 'o', '-', 'o', '-', 'o'}
		};

		int[] pieceCount = new []{ 12, 12 };//Xs, Os

		string input = "";
		int X1 = 0, Y1 = 0, X2 = 0, Y2 = 0;
		bool game = true;
		bool turn = false;
		char playerSymbol = 'x';
		char playerSuperSymbol = 'K';
		char opponentSymbol = 'o';
		char opponentSuperSymbol = 'Q';

		while (game)
		{
			displayBoard(board);

			if (turn)
			{
				playerSymbol = 'o';
				playerSuperSymbol= 'Q';
				opponentSymbol= 'x';
				opponentSuperSymbol = 'K';
			}
			else
			{
				playerSymbol = 'x';
				playerSuperSymbol = 'K';
				opponentSymbol = 'o';
				opponentSuperSymbol = 'Q';

			}

			if (pieceCount[0] == 0) //Xs=0
			{
				Console.WriteLine("PLAYER o WON !!!");
				game = false;
				continue;
			}
			else if (pieceCount[1] == 0)//Os=0
			{
				Console.WriteLine("PLAYER x WON !!!");
				game = false;
				continue;
			}
			
			Console.WriteLine("Player X has " + pieceCount[0] + " pieces remaining");
			Console.WriteLine("Player O has " + pieceCount[1] + " pieces remaining");
							
			if (HasCaptureOpportunity(board, playerSymbol)|| HasCaptureOpportunity(board, playerSuperSymbol))
			{
				Console.WriteLine("\nPlayer " + playerSymbol + " has capture opportunity!");
			}

			Console.WriteLine("Player " + playerSymbol + " turn - enter 2 positions (ex. 3C 4D):");
			
			input = Console.ReadLine();
			input = input.ToLower();
			if (input == "q")
			{
				game = false;
				Console.WriteLine(" E x i t i n g . . .");
				continue;
			}

			input = input.Replace(" ", string.Empty);
			 
			X1 = ((int)input[0]) - 48 - 1;
			Y1 = ((int)input[1]) - 96 - 1;
			X2 = ((int)input[2]) - 48 - 1;
			Y2 = ((int)input[3]) - 96 - 1;

			if(board[X1, Y1] != playerSymbol && board[X1, Y1]!= playerSuperSymbol)
			{
				Console.WriteLine("Invalid piece, try again");
				continue;
			}
			if (X1 < 0 || X1 > 7 || Y1 < 0 || Y1 > 7 || 
			    X2 < 0 || X2 > 7 || Y2 < 0 || Y2 > 7 ||
				 board[X2, Y2] != '-')				
			{
				Console.WriteLine("Invalid position, try again");
				continue;
			}

			else
			{				
				if((!turn && X1==7)||(turn && X1==0)) 
				{
					board[X2, Y2] = playerSuperSymbol;
				}
				else
				{
					board[X2, Y2] = board[X1, Y1];
				}
				board[X1, Y1] = '-';
			}

			if (Math.Abs(X2 - X1) > 1)
			{
				while (X2 != X1)
				{
					X1 = X1 + ((X2 - X1) / Math.Abs(X2 - X1));
					Y1 = Y1 + ((Y2 - Y1) / Math.Abs(Y2 - Y1));
					if (board[X1, Y1] == playerSymbol || board[X1, Y1] == playerSuperSymbol) break;

					if (board[X1, Y1] == opponentSymbol || board[X1, Y1] == opponentSuperSymbol)
					{
						pieceCount[Convert.ToInt32(!turn)]--;
						board[X1, Y1] = '-';
					}
					
					if (CanCapture(board, X2, Y2, playerSymbol, -1, -1) ||
					CanCapture(board, X2, Y2, playerSymbol, 1, 1) ||
					CanCapture(board, X2, Y2, playerSymbol, -1, 1) ||
					CanCapture(board, X2, Y2, playerSymbol, 1, -1) ||
					CanCapture(board, X2, Y2, playerSuperSymbol, -1, -1) ||
					CanCapture(board, X2, Y2, playerSuperSymbol, 1, 1) ||
					CanCapture(board, X2, Y2, playerSuperSymbol, -1, 1) ||
					CanCapture(board, X2, Y2, playerSuperSymbol, 1, -1)
					)
					{
						turn = !turn;
					}
				}
			}
				
			turn = !turn;
			Console.Clear();
		}

	}
	static bool HasCaptureOpportunity(char[,] board, char playerSymbol)
	{
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				if (board[i, j] == playerSymbol)
				{
					if (CanCapture(board, i, j, playerSymbol, -1, -1) ||
						CanCapture(board, i, j, playerSymbol, 1, 1) ||
						CanCapture(board, i, j, playerSymbol, -1, 1) ||
						CanCapture(board, i, j, playerSymbol, 1, -1))
					{
						return true;
					}

				}
			}
		}
		return false;
	}
	static bool CanCapture(char[,] board, int i, int j, char playerSymbol, int Xinc, int Yinc)
	{
		int opponentPositionX = i + Xinc;
		int opponentPositionY = j + Yinc;
		int landingPositionX = opponentPositionX + Xinc;
		int landingPositionY = opponentPositionY + Yinc;

		if (landingPositionX < 8 && landingPositionY < 8 &&
			landingPositionX >= 0 && landingPositionY >= 0)
		{
			if (board[landingPositionX, landingPositionY] == '-')
			{
				if ((board[opponentPositionX, opponentPositionY] == 'x'|| board[opponentPositionX, opponentPositionY] == 'K')
					&& (playerSymbol == 'o' ||	playerSymbol == 'Q'))
				{ return true; }
				else if ((board[opponentPositionX, opponentPositionY] == 'o' || board[opponentPositionX, opponentPositionY] == 'Q')
					&& (playerSymbol == 'x' || playerSymbol == 'K'))
					{ return true; }
				else return false;
			}
			else return false;

		}
		else return false;
	}
	
	static void displayBoard(char[,] board)
	{
		Console.WriteLine("        * C H E C K E R S *\n");

		Console.WriteLine("    A   B   C   D   E   F   G   H");
		Console.WriteLine("  |---+---+---+---+---+---+---+---|");

		for (int i = 0; i < 8; i++)
		{
			Console.Write(i + 1 + " |");
			for (int j = 0; j < 8; j++)
			{
				if (board[i, j] != '-')
				{
					Console.Write(" " + board[i, j] + " |");
				}
				else
				{
					Console.Write("   |");
				}

			}
			Console.Write("\n");
			Console.WriteLine("  |---+---+---+---+---+---+---+---|");
		}
	}
}
	




