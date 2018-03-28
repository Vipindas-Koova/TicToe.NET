using System;
namespace TicToeGameService.Model{
    public class TicToe{
        public TicToe(){}

        public string[,] Coordinates {get;set;}

        public TicToe Clone(){
            return new TicToe(){
                Coordinates = (string[,])this.Coordinates.Clone()
            };
        }
        
    }
}