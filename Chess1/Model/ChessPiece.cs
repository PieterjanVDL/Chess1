using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chess1.Model
{
   public class ChessPiece
    {


        public PieceType Type
        {
            get; set;
        }

        public Boolean Player
        { get; set; }


        public ChessPiece(PieceType type, Boolean player)
        {
            this.Type = type;
            this.Player = player;
           

        }

    }
}
