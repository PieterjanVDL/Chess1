using Chess1.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Chess1.ViewModel;
using System.Collections.ObjectModel;

namespace Chess1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {

            //ObservableCollection van elk vakje op schaakbord met eventueel aanwezige schaakstuk
            // volledige beginsituatie
            var chessPositions = new ObservableCollection<ChessPosition>();
          
      // toevoegen zwarte stukken
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Rook, false), new Point(0, 0), false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Knight,false), new Point(60, 0),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Bishop,false), new Point(120, 0),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.King, false), new Point(180, 0),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Queen, false), new Point(240, 0),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Bishop,false), new Point(300, 0),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Knight,false), new Point(360, 0),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Rook, false), new Point(420, 0),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Pawn, false), new Point(0, 60),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Pawn, false), new Point(60, 60),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Pawn, false), new Point(120, 60),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Pawn, false), new Point(180, 60),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Pawn, false), new Point(240, 60),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Pawn, false), new Point(300, 60),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Pawn, false), new Point(360, 60),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Pawn, false), new Point(420, 60),false));

            //toevoegen lege plaatsen 
            chessPositions.Add(new ChessPosition(null, new Point(0,120),false));
            chessPositions.Add(new ChessPosition(null, new Point(60,120),false));
            chessPositions.Add(new ChessPosition(null, new Point(120,120),false));
            chessPositions.Add(new ChessPosition(null, new Point(180,120),false));
            chessPositions.Add(new ChessPosition(null, new Point(240,120),false));
            chessPositions.Add(new ChessPosition(null, new Point(300,120),false));
            chessPositions.Add(new ChessPosition(null, new Point(360,120),false));
            chessPositions.Add(new ChessPosition(null, new Point(420,120),false));
            chessPositions.Add(new ChessPosition(null, new Point(0,180),false));
            chessPositions.Add(new ChessPosition(null, new Point(60,180),false));
            chessPositions.Add(new ChessPosition(null, new Point(120,180),false));
            chessPositions.Add(new ChessPosition(null, new Point(180,180),false));
            chessPositions.Add(new ChessPosition(null, new Point(240,180),false));
            chessPositions.Add(new ChessPosition(null, new Point(300,180),false));
            chessPositions.Add(new ChessPosition(null, new Point(360,180),false));
            chessPositions.Add(new ChessPosition(null, new Point(420,180),false));
            chessPositions.Add(new ChessPosition(null, new Point(0,240),false));
            chessPositions.Add(new ChessPosition(null, new Point(60,240),false));
            chessPositions.Add(new ChessPosition(null, new Point(120,240),false));
            chessPositions.Add(new ChessPosition(null, new Point(180,240),false));
            chessPositions.Add(new ChessPosition(null, new Point(240,240),false));
            chessPositions.Add(new ChessPosition(null, new Point(300,240),false));
            chessPositions.Add(new ChessPosition(null, new Point(360,240),false));
            chessPositions.Add(new ChessPosition(null, new Point(420,240),false));
            chessPositions.Add(new ChessPosition(null, new Point(0,300),false));
            chessPositions.Add(new ChessPosition(null, new Point(60,300),false));
            chessPositions.Add(new ChessPosition(null, new Point(120,300),false));
            chessPositions.Add(new ChessPosition(null, new Point(180,300),false));
            chessPositions.Add(new ChessPosition(null, new Point(240,300),false));
            chessPositions.Add(new ChessPosition(null, new Point(300,300),false));
            chessPositions.Add(new ChessPosition(null, new Point(360,300),false));
            chessPositions.Add(new ChessPosition(null, new Point(420,300),false));
            

            // toevoegen witte stukken
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Pawn, true), new Point(0, 360),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Pawn, true), new Point(60, 360),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Pawn, true), new Point(120, 360),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Pawn, true), new Point(180, 360),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Pawn, true), new Point(240, 360),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Pawn, true), new Point(300, 360),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Pawn, true), new Point(360, 360),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Pawn, true), new Point(420, 360),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Rook, true), new Point(0, 420),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Knight,true), new Point(60, 420),false));
                             
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Bishop, true), new Point(120, 420),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.King, true), new Point(180, 420),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Queen, true), new Point(240, 420),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Bishop, true), new Point(300, 420),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Knight, true), new Point(360, 420),false));
            chessPositions.Add(new ChessPosition(new ChessPiece(PieceType.Rook, true), new Point(420, 420),false));



            // Viewmodel initializeren, beginsituatie meegeven als parameter
            ViewModel.ChessVM vm = new ViewModel.ChessVM(chessPositions);



            View.ChessWindow chessboard = new View.ChessWindow();
          
            // dit viewmodel als datacontext van view instellen
            chessboard.DataContext = vm;

           
           
            chessboard.Show();

            






        }
    }
}
