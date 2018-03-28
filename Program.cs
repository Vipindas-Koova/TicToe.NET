using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using thread = System.Threading;
using System.Threading.Tasks;
using TicToeGameService.Processors;
using TicToeGameService.Model;

namespace TicToeGameService
{
    class Program
    {
        static void Main(string[] args)
        {
           TicToe t = new TicToe();
           t.Coordinates = new string[3,3];
           t.Coordinates[1,1] = "O";
           PrintTicToeOutput(t);
           PlayGame(t);
        }

        static void PlayGame(TicToe gameState){
            bool gameStateOver = false;
            while(!gameStateOver){
                string response = string.Empty;
                bool responseFormatCorrect = false;
                int row = 0;
                int column = 0;
                while(!responseFormatCorrect){
                    try{
                        Console.WriteLine("Enter your selection in a,b format as shown above");
                        //response = "0,0";
                        response = Console.ReadLine();
                        row = int.Parse(response.Split(",".ToCharArray())[0]);
                        column = int.Parse(response.Split(",".ToCharArray())[1]);
                        responseFormatCorrect = true;
                    }catch(Exception){
                        Console.WriteLine("Entered value not in correct format");
                    }
                }
                responseFormatCorrect = false;
                gameState.Coordinates[row,column] = "X";
                TicToePrcoessor processor = new TicToePrcoessor(gameState,"O","X");
                processor.ReplaceNullWithEmptyString(gameState);
                TicToe newGameState = processor.Output(gameState);
                int gameScore = processor.CalculateScore(newGameState);
                if( gameScore!= 0)
                {
                    gameStateOver = true;
                    Console.WriteLine(gameScore.ToString() + " - Game Over");
                }
                else{
                    Console.WriteLine("Your turn");
                }
                PrintTicToeOutput(processor.ReplaceEmptyStringWithNull(newGameState));
            }
        }

        static void PrintTicToeOutput(TicToe gameState){
            if(gameState == null){
                if(gameState.Coordinates == null){
                    //gameState.Coordinates = new string[,];
                }
            }
            for(int j=0;j < 3;j++){
                int screenLength = 50;
                for(int i=0; i < screenLength; i++){
                    if(i == 1*screenLength/6){
                        Console.Write(gameState.Coordinates[j,0]==null?j.ToString()+",0":"'"+gameState.Coordinates[j,0]+"'");
                    }
                    if(i == 3*screenLength/6){
                        Console.Write(gameState.Coordinates[j,1]==null?j.ToString()+",1":"'" +gameState.Coordinates[j,1]+"'");
                    }
                    if(i == 5*screenLength/6){
                        Console.Write(gameState.Coordinates[j,2]==null?j.ToString()+",2":"'"+gameState.Coordinates[j,0]+"'");
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
