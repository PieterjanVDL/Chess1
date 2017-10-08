using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chess1.Model
{
    public class ChessPosition : INotifyPropertyChanged
    {

        // indien surpressNotification==false wordt verandering in model niet doorgegeven aan View
        // -> zie hieronder in functie RaisePropertyChanged

        private bool supressNotification;
        public bool SupressNotification
        {
            get
            {
                return supressNotification;
            }
            set
            {

                supressNotification = value;
            }


        }




        private ChessPiece pieceValue;
        public ChessPiece Piece {
            get { return pieceValue; }
            set { pieceValue = value;

            
                RaisePropertyChanged("Piece");

            }
        }


        
        public Point Pos { get; }

        private Boolean allowedValue;
        public Boolean Allowed{
            get { return allowedValue; }
            set { allowedValue = value;
                RaisePropertyChanged("Allowed");
            } }


        // allowed: onthoudt of het geselecteerde item al dan niet naar de positie in kwestie verplaatst kan worden
        public ChessPosition(ChessPiece piece, Point pos, bool allowed)
        {

            Pos = pos;
            Piece = piece;
           
            Allowed = allowed;
           
        }


        public void RaisePropertyChanged(String info)
        {
            if (PropertyChanged != null&&SupressNotification==false)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
                
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
       
      
    }
}
