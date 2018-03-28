using System;
using System.Collections.Generic;
using TicToeGameService.Model;
namespace TicToeGameService.Processors{
    public class TicToePrcoessor{
        public TicToe CurrentGameState{get;set;}
        public TicToePrcoessor(TicToe gameState, string meIdentifierChar, string opponentIdentifierChar){
            this.CurrentGameState = gameState;
            this.MeIdentifierChar = meIdentifierChar;
            this.OpponentIdentifierChar = opponentIdentifierChar;
        }

        public TicToePrcoessor(string html, string meIdentifierChar, string opponentIdentifierChar){
            //this.CurrentGameState = GetModelFromHTML(html);
            this.MeIdentifierChar = meIdentifierChar;
            this.OpponentIdentifierChar = opponentIdentifierChar;
        }

        public string MeIdentifierChar{get;set;}
        public string OpponentIdentifierChar{get;set;}

        public TicToe Output(TicToe gameState){
            //algorithm to find the next best match and return the next state
            Dictionary<KeyValuePair<int,int>,int> scoreState = new Dictionary<KeyValuePair<int,int>,int>();
            List<KeyValuePair<int,int>> emptyCells = GetEmptyCellCoordinates(gameState);
            foreach(KeyValuePair<int,int> cell in emptyCells){
                TicToe tempGameState = gameState.Clone();
                int depth = 0;
                scoreState.Add(KeyValuePair.Create(cell.Key,cell.Value), 
                    MinMax(tempGameState,cell.Key,cell.Value, ref depth,true) - depth);
                tempGameState.Coordinates[cell.Key,cell.Value] = "";
            }

            //find max value scorestate item
            KeyValuePair<int,int> winningNextMove =  scoreState.FindMaxValue();
            gameState.Coordinates[winningNextMove.Key,winningNextMove.Value] = this.MeIdentifierChar;
            return gameState;
        }

        public int MinMax(TicToe gameState, int row, int column, ref int depth, bool meTurn){
            int score=0;  
            List<int> scores = new List<int>();  
            if(meTurn){
                score = NextStateScore(gameState,row,column,this.MeIdentifierChar);
            }else{
                 score = NextStateScore(gameState,row,column,this.OpponentIdentifierChar);
            }
            if(score == 10 || score == -10){return score;}//game is over hence return with value indicating the winner
            List<KeyValuePair<int,int>> emptyCells = GetEmptyCellCoordinates(gameState);
            foreach(KeyValuePair<int,int> cell in emptyCells){
                depth++;
                int score1 = NextStateScore(gameState,cell.Key,cell.Value,
                                (meTurn == true?this.MeIdentifierChar:this.OpponentIdentifierChar));
                scores.Add(score1);
                gameState.Coordinates[cell.Key,cell.Value] = "";
                score =  MinMax(gameState, cell.Key, cell.Value, ref depth,!meTurn);
                if(score1 == 10 || score1 == -10){return score1;}//game is over hence return with value indicating the winner
                gameState.Coordinates[cell.Key,cell.Value] = "";
            }

            //create array of scores and check for min/max
            int finalScore = 0;
            for(int i=0;i< scores.Count;i++){
                if(meTurn){
                    if(scores[i] > finalScore){
                        finalScore = scores[i];
                    }
                }else{
                    if(scores[i] < finalScore){
                        finalScore = scores[i];
                    }
                }
            }

            return finalScore;
        }
        
        public int NextStateScore(TicToe gameState, int row, int column, string identifierChar){
            gameState.Coordinates[row,column] = identifierChar;
            return CalculateScore(gameState);
        }

        //returns score from the next state
        //10 - me win
        //-10 - opponent win
        //0 - No one wins
        public int CalculateScore(TicToe gameState){
            int scoreX = 0;
            int scoreY = 0;
            int scoreDgnl = 0;
            int scoreOppX = 0;
            int scoreOppY = 0;
            int scoreOppDgnl = 0;
            
            for(int i = 0; i < 3; i++){
                for(int j = 0; j < 3; j++){
                    //horizontal sequencing t0 find match
                    if(gameState.Coordinates[i,j].ToString().ToLower() == this.MeIdentifierChar.ToLower()){
                        scoreX++;
                    }
                    else if(gameState.Coordinates[i,j].ToString().ToLower() == this.OpponentIdentifierChar.ToLower()){
                        scoreOppX++;
                    }
                    //Vertical sequencing t0 find match
                    if(gameState.Coordinates[j,i].ToString().ToLower() == this.MeIdentifierChar.ToLower()){
                        scoreY++;
                    }else if(gameState.Coordinates[j,i].ToString().ToLower() == this.OpponentIdentifierChar.ToLower()){
                        scoreOppY++;
                    }
                    //Diagonal sequencing to find match
                    if(gameState.Coordinates[j,j].ToLower() == this.MeIdentifierChar.ToLower()){
                        scoreDgnl++;
                    }else if(gameState.Coordinates[j,j].ToLower() == this.OpponentIdentifierChar.ToLower()){
                        scoreOppDgnl++;
                    }
                }
                if(scoreX >= 3 || scoreY >= 3 || scoreDgnl >= 3){
                    return 10;
                } else if(scoreOppX >= 3 || scoreOppY >= 3 || scoreOppDgnl >= 3){
                    return -10;
                }
                else{
                    scoreX = scoreY = scoreDgnl = scoreOppX = scoreOppY = scoreOppDgnl = 0;
                }
            }
            return 0;
        }

        public List<KeyValuePair<int,int>> GetEmptyCellCoordinates(TicToe gameState){
            List<KeyValuePair<int,int>> emptyCells = new List<KeyValuePair<int,int>>();
            for(int i=0;i < 3;i++){
                for(int j=0;j<3;j++){
                    if(gameState.Coordinates[i,j] == string.Empty){
                        emptyCells.Add(KeyValuePair.Create(i,j));
                    }
                }
            }
            return emptyCells;
        }

        public TicToe ReplaceNullWithEmptyString(TicToe t){
            for(int i=0;i< 3; i++){
                for(int j=0;j<3;j++){
                    if(t.Coordinates[i,j] == null){
                        t.Coordinates[i,j] = string.Empty;
                    }
                }
            }
            return t;
        }

        public TicToe ReplaceEmptyStringWithNull(TicToe t){
            for(int i=0;i< 3; i++){
                for(int j=0;j<3;j++){
                    if(t.Coordinates[i,j] == string.Empty){
                        t.Coordinates[i,j] = null;
                    }
                }
            }
            return t;
        }

        // public TicToe GetModelFromHTML(string html){
        //     string[,] coordinates = new string[3,3];
        //     HtmlDocument htmlDoc = new HtmlDocument();
        //     htmlDoc.LoadHtml(html);
        //     //parse html
        //     for(int i = 0 ; i < coordinates.Length; i++){
        //         for(int j = 0; j < 3; j++){
        //             HtmlNode cellNode = htmlDoc.DocumentNode.FirstChild.SelectSingleNode("//td[@id='r"
        //                 +i.ToString()+"c"+j.ToString()+"']");
        //             if(cellNode != null){
        //                 coordinates[i,j] = cellNode.ChildNodes[1].InnerText;
        //             }
        //         }
        //     }
        //     return new TicToe(){
        //         Coordinates = coordinates
        //     };
        // }

        // public string GetHTMLFromModel(TicToe gameState, string html){
        //     HtmlDocument htmlDoc = new HtmlDocument();
        //     htmlDoc.LoadHtml(html);
        //     //parse html
        //     for(int i = 0 ; i < gameState.Coordinates.Length; i++){
        //         for(int j = 0; j < 3; j++){
        //             htmlDoc.DocumentNode.SelectSingleNode("//td['id']=r"
        //                 +i.ToString()+"c"+j.ToString()+"'").FirstChild.InnerHtml = gameState.Coordinates[i,j];
        //         }
        //     }
        //     return htmlDoc.ToString();
        // }

    }

    public static class DictionaryExtended
    {
        public static KeyValuePair<int,int> FindMaxValue(this Dictionary<KeyValuePair<int,int>,int> ScoreState){
            KeyValuePair<int,int> maxValueCell;
            int maxValue =-10000;

            foreach(KeyValuePair<KeyValuePair<int,int>,int> cell in ScoreState){
                if( cell.Value > maxValue){
                    maxValue = cell.Value;
                    maxValueCell = cell.Key;
                }
            }
            return maxValueCell;
        } 
    }
}