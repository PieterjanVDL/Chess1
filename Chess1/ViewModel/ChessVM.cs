using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess1.Model;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using System.Windows;
using Chess1.View;
using System.Threading;

namespace Chess1.ViewModel
{
    public class ChessVM : ViewModelBase
    {
       

        private int level = 2;
        //initialiseren ViewModel: positions is beginsituatie van het schaakspel (zie App.xaml.cs)
        public ChessVM(ObservableCollection<ChessPosition> positions)
        {
           
            ChessPositions = positions;
           


        }

        public bool EndOfGame { get; set; }


        private int selectedIndex=-1;
        
        public int SelectedIndex
        {
            get {
                return selectedIndex;
                
            }

            set {
              
                selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
            }
        }

       
        private ChessPosition selectedItem;

        public ChessPosition SelectedItem
        {
            get
            {



                return selectedItem;
            }

            set
            {

                

                if (!EndOfGame)
               {
                    

                    if (selectedItem != null && selectedItem.Piece != null && selectedItem.Piece.Player == true)
                    {

                        // indien er reeds een wit schaakspel geselecteerd is: naar waar mag je dat stuk verplaatsen?
                        CheckRules(selectedItem);

                        
                       





                        //laten regels toe dat je het geselecteerde schaakstuk naar de aangeklikte plaats verplaatst

                        if (value.Allowed == true)
                        {

                            // onthouden van oorspronkelijke situatie
                            var tempOriginalPosition = new ChessPiece(selectedItem.Piece.Type, selectedItem.Piece.Player);
                            var tempDestination = (value.Piece==null)?null:new ChessPiece(value.Piece.Type, value.Piece.Player);

                            // indien ja: verplaats geselecteerde schaakstuk naar aangeklikte plaats
                            ChessPositions[Convert.ToInt32((selectedItem.Pos.X + 8 * selectedItem.Pos.Y) / 60)].Piece = null;
                            ChessPositions[(Convert.ToInt32((value.Pos.X + 8 * value.Pos.Y) / 60))].Piece = tempOriginalPosition;

                           // controleren of de speler zichzelf heeft schaak gezet
                            var check = Check();

                            if (check)
                            {
                                // Indien speler zichzelf heeft schaak gezet: waarschuwing, zet wordt ongedaan gemaakt en speler is opnieuw aan de beurt
                                MessageBox.Show("You are not allowed to commit suicide!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                                // en oorspronkelijke situatie herstellen

                              ChessPositions[Convert.ToInt32((selectedItem.Pos.X + 8 * selectedItem.Pos.Y) / 60)].Piece = tempOriginalPosition;
                             ChessPositions[(Convert.ToInt32((value.Pos.X + 8 * value.Pos.Y) / 60))].Piece = tempDestination;



                            }

                            else
                            {
                                // Indien speler zichzelf niet heeft schaak gezet: verandering wordt doorgevoerd

                                RaisePropertyChanged("ChessPieces");

                                // Is spel hiermee ten einde?

                                EndOfGame = HasGameEnded(false);

                                // indien spel niet ten einde: zwart aan zet
                                if (!EndOfGame)
                                {
                                    MinMax();
                                }
                            }
                        }
                        else
                        {
                            //indien de zet niet is toegestaan: aangeklikte element selecteren
                            selectedItem = value;

                        }
                        //nadat zet is gemaakt: geen enkel element geselecteerd
                        SelectedIndex = -1;
                    }


                    // indien er nog geen ChessPosition  geselecteerd was of deze bevatte geen wit schaakstuk
                    // => selecteren van de aangeklikte positie en regels toepassen
                    else
                    {

                        selectedItem = value;



                    }
                    RaisePropertyChanged("SelectedItem");



                }
            }

        }

        private ObservableCollection<ChessPosition> chessPositions = new ObservableCollection<ChessPosition>();


        public ObservableCollection<ChessPosition> ChessPositions
        {
            get
            {
                return chessPositions;
            }
            set
            {
               
                chessPositions = value;
                RaisePropertyChanged("ChessPieces");

            }

        }

       
        // functie CheckRegels() bepaald naar welke posities een schaakstuk verplaatst kan worden
        public void CheckRules(ChessPosition selectedPosition)
        {

            // indien er geen schaakstuk op de betreffende positie staat kan je dit ook niet verplaatsen
            //=> meteen afbreken van methode CheckRegels()
            

            if (selectedPosition.Piece == null)
            {
                return;
            }

            // coördinaten van geselecteerde item 
            int xCoordinate = Convert.ToInt32(selectedPosition.Pos.X)/60;
            int yCoordinate = Convert.ToInt32(selectedPosition.Pos.Y)/60;


            // kopie van deze coördinaten

            int x = xCoordinate;
            int y= yCoordinate;


            //aanvankelijk wordt de property "Allowed" van alle elementen in de array chessPieces op 'false' gezet
            //= > het schaakstuk kan niet bewegen
        
            for (var i = 0; i < chessPositions.Count; i++)
            {
                chessPositions[i].Allowed = false;
            }

            // vervolgens passen we de regels toe die horen bij een schaakstuk van dit type en deze speler
            // d.w.z.: e.v.t. toestemming geven om naar een bepaalde positie te bewegen door de property "Allowed" van deze ChessPosition op 'true' te zetten

            // zwarte pion: kan alleen "omlaag" bewegen

            if (selectedPosition.Piece.Type == PieceType.Pawn && selectedPosition.Piece.Player == false)
            {
               y= yCoordinate;

               
                
                    y++;

                if ((y<=7)&& ChessPositions[xCoordinate + 8 * y].Piece == null)
                {
                    ChessPositions[xCoordinate + 8 * y].Allowed = true;

                    y++;
                
                 if (yCoordinate==1 &&ChessPositions[xCoordinate + 8 * y].Piece == null)
                {
                    ChessPositions[xCoordinate + 8 * y].Allowed = true;
                }

                    }


                y = yCoordinate;
                x = xCoordinate;

               

                if ((xCoordinate + 1) <= 7 && ((yCoordinate+1)<=7)&& ChessPositions[(xCoordinate + 1) + 8 * (yCoordinate + 1)].Piece != null && ChessPositions[(xCoordinate + 1) + 8 * (yCoordinate + 1)].Piece.Player == true)
                {

                    ChessPositions[xCoordinate + 1 + 8 * (yCoordinate + 1)].Allowed = true;
                }

                if((xCoordinate-1)>=0 && ((yCoordinate+1)<=7)&&ChessPositions[(xCoordinate-1) +8*(yCoordinate+1)].Piece!=null && ChessPositions[xCoordinate-1+8*(yCoordinate+1)].Piece.Player==true)
                    {

                    ChessPositions[xCoordinate - 1 + 8 * (yCoordinate + 1)].Allowed = true;

                }

               

                }
                
                


            
            // witte pion: kan alleen "omhoog" bewegen

            else if (selectedPosition.Piece.Type == PieceType.Pawn && selectedPosition.Piece.Player == true)
            {
                y = yCoordinate;



                y--;

                if (ChessPositions[xCoordinate + 8 * y].Piece == null)
                {
                    ChessPositions[xCoordinate + 8 * y].Allowed = true;

                    y--;

                    if (yCoordinate == 6 && ChessPositions[xCoordinate + 8 * y].Piece == null)
                    {
                        ChessPositions[xCoordinate + 8 * y].Allowed = true;
                    }

                }


                y = yCoordinate;
                x = xCoordinate;



                if ((xCoordinate + 1) <= 7 && ((yCoordinate -1)>=0) && ChessPositions[(xCoordinate + 1) + 8 * (yCoordinate -1)].Piece != null && ChessPositions[(xCoordinate + 1) + 8 * (yCoordinate -1)].Piece.Player == false)
                {

                    ChessPositions[xCoordinate + 1 + 8 * (yCoordinate - 1)].Allowed = true;
                }

                if ((xCoordinate - 1) >= 0 && ((yCoordinate - 1)>=0) && ChessPositions[(xCoordinate - 1) + 8 * (yCoordinate - 1)].Piece != null && ChessPositions[xCoordinate - 1 + 8 * (yCoordinate - 1)].Piece.Player == false)
                {

                    ChessPositions[xCoordinate - 1 + 8 * (yCoordinate - 1)].Allowed = true;

                }



            }

            // loper

            else if (selectedPosition.Piece.Type == PieceType.Bishop)
            {

                x = xCoordinate;
                y = yCoordinate;

                x++;
                y++;
                while (x <= 7 && y <= 7 && (chessPositions[x + 8 * y].Piece == null || chessPositions[x + 8 * y].Piece.Player != selectedPosition.Piece.Player) && (chessPositions[x + 8 * y - 9].Piece == null || chessPositions[x + 8 * y - 9].Piece.Player != (!selectedPosition.Piece.Player)))
                {




                    chessPositions[x + 8 * y].Allowed = true;
                    x++;
                    y++;

                }

                x = xCoordinate;
                y = yCoordinate;
                x--;
                y++;
                while (x >= 0 && y <= 7 && (chessPositions[x + 8 * y].Piece == null || chessPositions[x + 8 * y].Piece.Player != selectedPosition.Piece.Player) && (chessPositions[x + 8 * y - 7].Piece == null || chessPositions[x + 8 * y - 7].Piece.Player != (!selectedPosition.Piece.Player)))
                {

                    chessPositions[x + 8 * y].Allowed = true;
                    x--;
                    y++;

                }

                x = xCoordinate;
                y = yCoordinate;
                x++;
                y--;
                while (x <= 7 && y >= 0 && (chessPositions[x + 8 * y].Piece == null || chessPositions[x + 8 * y].Piece.Player != selectedPosition.Piece.Player) && (chessPositions[x + 8 * y + 7].Piece == null || chessPositions[x + 8 * y + 7].Piece.Player != (!selectedPosition.Piece.Player)))
                {

                    chessPositions[x + 8 * y].Allowed = true;
                    x++;
                    y--;

                }

                x = xCoordinate;
                y = yCoordinate;

                x--;
                y--;
                while (x >= 0 && y >= 0 && (chessPositions[x + 8 * y].Piece == null || chessPositions[x + 8 * y].Piece.Player != selectedPosition.Piece.Player) && (chessPositions[x + 8 * y + 9].Piece == null || chessPositions[x + 8 * y + 9].Piece.Player != (!selectedPosition.Piece.Player)))
                {

                    chessPositions[x + 8 * y].Allowed = true;
                    x--;
                    y--;

                }



            }

            
            // koning

           else if (selectedPosition.Piece.Type == PieceType.King)
            {

                x = xCoordinate + 1;
                y = yCoordinate;
               
                if ((x<=7)&&(ChessPositions[x+y*8].Piece==null || ChessPositions[x  + y * 8].Piece.Player!=selectedPosition.Piece.Player))
                {
                    ChessPositions[x + 8*y].Allowed = true;
                }

                x = xCoordinate + 1;
                y = yCoordinate +1;

                if ((x <= 7) && (y<=7) && (ChessPositions[x + y * 8].Piece == null || ChessPositions[x + y * 8].Piece.Player != selectedPosition.Piece.Player))
                {
                    ChessPositions[x + 8*y].Allowed = true;
                }

                x = xCoordinate;
                y = yCoordinate + 1;

                if ((y <= 7) && (ChessPositions[x + y * 8].Piece == null || ChessPositions[x + y * 8].Piece.Player != selectedPosition.Piece.Player))
                {
                    ChessPositions[x + 8 * y].Allowed = true;
                }

                x = xCoordinate -1;
                y = yCoordinate;

                if ((x>=0) && (ChessPositions[x + y * 8].Piece == null || ChessPositions[x + y * 8].Piece.Player != selectedPosition.Piece.Player))
                {
                    ChessPositions[x + 8 * y].Allowed = true;
                }

                x = xCoordinate - 1;
                y = yCoordinate-1;

                if ((y>= 0) && (x>=0)&&(ChessPositions[x + y * 8].Piece == null || ChessPositions[x + y * 8].Piece.Player != selectedPosition.Piece.Player))
                {
                    ChessPositions[x + 8 * y].Allowed = true;
                }

                x = xCoordinate;
                y = yCoordinate - 1;

                if ( (y >= 0) && (ChessPositions[x + y * 8].Piece == null || ChessPositions[x + y * 8].Piece.Player != selectedPosition.Piece.Player))
                {
                    ChessPositions[x + 8 * y].Allowed = true;
                }

                x = xCoordinate+1;
                y = yCoordinate - 1;

                if ((y >= 0)&&(x<=7) && (ChessPositions[x + y * 8].Piece == null || ChessPositions[x + y * 8].Piece.Player != selectedPosition.Piece.Player))
                {
                    ChessPositions[x + 8 * y].Allowed = true;
                }

                x = xCoordinate-1;
                y = yCoordinate + 1;

                if ((y<=7) && (x>=0)&&(ChessPositions[x + y * 8].Piece == null || ChessPositions[x + y * 8].Piece.Player != selectedPosition.Piece.Player))
                {
                    ChessPositions[x + 8 * y].Allowed = true;
                }



            }


            //paard
            else if (selectedPosition.Piece.Type == PieceType.Knight)
            {
                x = xCoordinate+2;
                y = yCoordinate+1;

                if (x <= 7 && y <= 7 && (ChessPositions[x + 8 * y].Piece == null || ChessPositions[x + 8 * y].Piece.Player!= selectedPosition.Piece.Player) )
                {
                    ChessPositions[x + 8 * y].Allowed = true;
                }



                x = xCoordinate + 1;
                y = yCoordinate + 2;

                if (x <= 7 && y <= 7&&(ChessPositions[x + 8 * y].Piece == null || ChessPositions[x + 8 * y].Piece.Player != selectedPosition.Piece.Player))
                {
                    ChessPositions[x + 8 * y].Allowed = true;
                }


                x = xCoordinate - 1;
                y = yCoordinate + 2;

                if (x >=0 && y<= 7 && (ChessPositions[x + 8 * y].Piece == null || ChessPositions[x + 8 * y].Piece.Player != selectedPosition.Piece.Player))
                {
                    ChessPositions[x + 8 * y].Allowed = true;
                }


                x = xCoordinate + 1;
                y = yCoordinate - 2;

                if (x <= 7 && y >= 0 && (ChessPositions[x + 8 * y].Piece == null || ChessPositions[x + 8 * y].Piece.Player != selectedPosition.Piece.Player))
                {
                    ChessPositions[x + 8 * y].Allowed = true;
                }


                x = xCoordinate +2;
                y = yCoordinate - 1;

                if (x <= 7 && y >= 0 && (ChessPositions[x + 8 * y].Piece == null || ChessPositions[x + 8 * y].Piece.Player != selectedPosition.Piece.Player))
                {
                    ChessPositions[x + 8 * y].Allowed = true;
                }

                x = xCoordinate - 2;
                y = yCoordinate + 1;

                if (x >=0 && y <=7&&(ChessPositions[x + 8 * y].Piece == null || ChessPositions[x + 8 * y].Piece.Player != selectedPosition.Piece.Player))
                {
                    ChessPositions[x + 8 * y].Allowed = true;
                }

                x = xCoordinate - 1;
                y = yCoordinate - 2;

                if (x >=0 && y >= 0 && (ChessPositions[x + 8 * y].Piece == null || ChessPositions[x + 8 * y].Piece.Player != selectedPosition.Piece.Player))
                {
                    ChessPositions[x + 8 * y].Allowed = true;
                }

                x = xCoordinate - 2;
                y = yCoordinate - 1;


                if (x >= 0 && y >= 0 && (ChessPositions[x+8*y].Piece==null || ChessPositions[x+8*y].Piece.Player!=selectedPosition.Piece.Player))
                {

                    
                        ChessPositions[x + 8 * y].Allowed = true;
                    }

               


            }
                    
            //toren
           else if (selectedPosition.Piece.Type == PieceType.Rook)
            {
                x = xCoordinate+1;
                y = yCoordinate;

                

                while (x<=7 && (ChessPositions[x+y*8].Piece==null || ChessPositions[x+y*8].Piece.Player!= selectedPosition.Piece.Player)&& (chessPositions[(x-1) + 8 * y ].Piece == null || chessPositions[(x-1) + 8 * y].Piece.Player != (!selectedPosition.Piece.Player)))
                {
                    ChessPositions[x + y * 8].Allowed = true;
                    x++;
                }

                x = xCoordinate-1;
                y = yCoordinate;
               
                while (x>=0 && (ChessPositions[x + y * 8].Piece == null || ChessPositions[x + y * 8].Piece.Player != selectedPosition.Piece.Player)&&(chessPositions[x+1 + 8 *y].Piece == null || chessPositions[(x+1) + 8 * y ].Piece.Player != (!selectedPosition.Piece.Player)))
                {
                    ChessPositions[x + y * 8].Allowed = true;
                    x--;
                }

                x = xCoordinate;
                y = yCoordinate+1;
           

                while (y <= 7 && (ChessPositions[x + y * 8].Piece == null || ChessPositions[x + y * 8].Piece.Player != selectedPosition.Piece.Player)&& (chessPositions[x + 8 * (y-1)].Piece == null || chessPositions[x + 8 *( y -1)].Piece.Player != (!selectedPosition.Piece.Player)))
                {
                    ChessPositions[x + y * 8].Allowed = true;
                    y++;
                }

                x = xCoordinate;
                y = yCoordinate-1;
            

                while (y >= 0 && (ChessPositions[x + y * 8].Piece == null || ChessPositions[x + y * 8].Piece.Player != selectedPosition.Piece.Player)&& (chessPositions[x + 8 * (y+1)].Piece == null || chessPositions[x + 8 * (y+1)].Piece.Player != (!selectedPosition.Piece.Player)))
                {
                    ChessPositions[x + y * 8].Allowed = true;
                    y--;
                }

            }

            //koningin
            else if (selectedPosition.Piece.Type == PieceType.Queen)
            {
                x = xCoordinate + 1;
                y = yCoordinate;



                while (x <= 7 && (ChessPositions[x + y * 8].Piece == null || ChessPositions[x + y * 8].Piece.Player != selectedPosition.Piece.Player) && (chessPositions[(x - 1) + 8 * y].Piece == null || chessPositions[(x - 1) + 8 * y].Piece.Player != (!selectedPosition.Piece.Player)))
                {
                    ChessPositions[x + y * 8].Allowed = true;
                    x++;
                }

                x = xCoordinate - 1;
                y = yCoordinate;

                while (x >= 0 && (ChessPositions[x + y * 8].Piece == null || ChessPositions[x + y * 8].Piece.Player != selectedPosition.Piece.Player) && (chessPositions[x + 1 + 8 * y].Piece == null || chessPositions[(x + 1) + 8 * y].Piece.Player != (!selectedPosition.Piece.Player)))
                {
                    ChessPositions[x + y * 8].Allowed = true;
                    x--;
                }

                x = xCoordinate;
                y = yCoordinate + 1;


                while (y <= 7 && (ChessPositions[x + y * 8].Piece == null || ChessPositions[x + y * 8].Piece.Player != selectedPosition.Piece.Player) && (chessPositions[x + 8 * (y - 1)].Piece == null || chessPositions[x + 8 * (y - 1)].Piece.Player != (!selectedPosition.Piece.Player)))
                {
                    ChessPositions[x + y * 8].Allowed = true;
                    y++;
                }

                x = xCoordinate;
                y = yCoordinate - 1;


                while (y >= 0 && (ChessPositions[x + y * 8].Piece == null || ChessPositions[x + y * 8].Piece.Player != selectedPosition.Piece.Player) && (chessPositions[x + 8 * (y + 1)].Piece == null || chessPositions[x + 8 * (y + 1)].Piece.Player != (!selectedPosition.Piece.Player)))
                {
                    ChessPositions[x + y * 8].Allowed = true;
                    y--;
                }

                x = xCoordinate;
                y = yCoordinate;

                x++;
                y++;
                while (x <= 7 && y <= 7 && (chessPositions[x + 8 * y].Piece == null || chessPositions[x + 8 * y].Piece.Player != selectedPosition.Piece.Player) && (chessPositions[x + 8 * y - 9].Piece == null || chessPositions[x + 8 * y - 9].Piece.Player != (!selectedPosition.Piece.Player)))
                {




                    chessPositions[x + 8 * y].Allowed = true;
                    x++;
                    y++;

                }

                x = xCoordinate;
                y = yCoordinate;
                x--;
                y++;
                while (x >= 0 && y <= 7 && (chessPositions[x + 8 * y].Piece == null || chessPositions[x + 8 * y].Piece.Player != selectedPosition.Piece.Player) && (chessPositions[x + 8 * y - 7].Piece == null || chessPositions[x + 8 * y - 7].Piece.Player != (!selectedPosition.Piece.Player)))
                {

                    chessPositions[x + 8 * y].Allowed = true;
                    x--;
                    y++;

                }

                x = xCoordinate;
                y = yCoordinate;
                x++;
                y--;
                while (x <= 7 && y >= 0 && (chessPositions[x + 8 * y].Piece == null || chessPositions[x + 8 * y].Piece.Player != selectedPosition.Piece.Player) && (chessPositions[x + 8 * y + 7].Piece == null || chessPositions[x + 8 * y + 7].Piece.Player != (!selectedPosition.Piece.Player)))
                {

                    chessPositions[x + 8 * y].Allowed = true;
                    x++;
                    y--;

                }

                x = xCoordinate;
                y = yCoordinate;

                x--;
                y--;
                while (x >= 0 && y >= 0 && (chessPositions[x + 8 * y].Piece == null || chessPositions[x + 8 * y].Piece.Player != selectedPosition.Piece.Player) && (chessPositions[x + 8 * y + 9].Piece == null || chessPositions[x + 8 * y + 9].Piece.Player != (!selectedPosition.Piece.Player)))
                {

                    chessPositions[x + 8 * y].Allowed = true;
                    x--;
                    y--;

                }

            }

        }



        
        // wenselijkheid van huidige situatie berekenen vanuit het standpunt van computerspeler
        
        public int CalculateScore()
        {
            int score = 0;

            
            // =>alle stukken op het bord een waarde geven, hoe hoger hoe belangrijker
            // waarde witte stukken negatief maken
            // optellen van al deze waardes 
            foreach (var piece in ChessPositions.Where(piece=>piece.Piece!=null))
            {
                int value=0;

       
                    if (piece.Piece.Type == PieceType.Bishop)
                    {
                        value = 3;

                    }
    

                   else if (piece.Piece.Type == PieceType.King)
                    {
                        value = 100;

                    }
                   else if (piece.Piece.Type == PieceType.Knight)
                    {
                        value = 3;

                    }
                   else if (piece.Piece.Type == PieceType.Pawn)
                    {
                        value = 1;

                    }
                   else if (piece.Piece.Type == PieceType.Queen)
                    {
                        value = 10;

                    }
                   else if (piece.Piece.Type == PieceType.Rook)
                    {
                        value = 5;

                    }

                //witte stukken zijn de stukken van de spelende persoon=> hebben negatieve waarde vanuit oogpunt computerspeler
               
                    if (piece.Piece.Player==true)
                    {
                        value = -value;
                    }
                
                score += value;
            }

            return score;


        }


        // recursief algoritme: MinMax()=> Min() => Max()=> Min()= > Max()... totdat variabele 'teller' bepaalde waarde heeft bereikt
        // computerspeler maximaliseert score in de veronderstelling dat speler deze vervolgens zal proberen te minimaliseren enz....
        // het programa houdt een lijst bij van moves die een optimaal rendement opleveren voor de computerspeler
        // op einde algoritme wordt een willekeurige move uit deze lijst uitgevoerd
        public void MinMax()

        {

            // we zullen verschillende situaties simuleren 
            //=> vooraleer algoritme één bepaalde zet geselecteerd heeft als optimaal moeten positieveranderingen niet doorgegeven worden aan View
           
            foreach (var position in ChessPositions)
            {
                position.SupressNotification = true;
            }
            
            int counter = 0;
            int maxScore = -999999;
            var optimalMoves = new List<Move>();

            // de computerspeler bekijkt beurtelings alle zwarte stukken

            foreach (var blackPiece in (ChessPositions.Where(stuk=>((stuk.Piece!=null)&&(stuk.Piece.Player==false)))).ToList())
            {

                // de computerspeler bepaalt welke zetten legaal zijn voor dit stuk en stopt alle toegestane bestemmingen in een List 
                    CheckRules(blackPiece);

                var allowedPositions = ChessPositions.Where(piece => piece.Allowed == true).ToList();

                // de computerspeler simuleert beurtelings alle toegestane zetten voor het stuk "stuk.Piece" en beoordeelt hun mate van wenselijkheid

                    foreach (var piece in allowedPositions) 
                    {

                    // indien een koning genomen wordt: spel voorbij => teller op maximum zetten
                    if (piece.Piece!=null &&piece.Piece.Type == PieceType.King)
                        counter = level;
                    
                
                            var temp = piece.Piece; 
                            piece.Piece = blackPiece.Piece;
                            blackPiece.Piece = null;

                    // welke score zou dit opleveren, aangenomen dat speler 'wit' de score functie wil minimaliseren
                            int score = Min(counter);
                   
                    // indien deze score groter is dan bij eerder gesimuleerde zetten: score onthouden en coördinaten onthouden van zowel het verzette stuk als zijn bestemming 
                    // eerder onthouden bewegingen verwijderen uit lijst 'coordinaten' aangezien deze een lagere score hebben

                            if (maxScore < score)
                            {
                                maxScore = score;
                        optimalMoves.Clear(); 
                        optimalMoves.Add(new Move { Pos1 = blackPiece.Pos, Pos2 = piece.Pos });
                        
                            }

                            // indien score even groot is als de maximale score van vorige 'moves': toevoegen aan lijst coordinaten

                    if (maxScore == score)
                        {
                        optimalMoves.Add(new Move { Pos1 = blackPiece.Pos, Pos2 = piece.Pos });

                    }

                            // stukken terugzetten naar originele positie
                    blackPiece.Piece = piece.Piece;
                    piece.Piece = temp;
                   

                }

                   
           
            }

            // aanpassingen aan View toestaan

            foreach (var positon in ChessPositions)
            {
                positon.SupressNotification = false;
            }


            // willekeurig één van de optimale bewegingen kiezen

            var numberOfMoves = optimalMoves.Count;


            // random één van de onthouden (en dus optimale) moves kiezen
            // dit maakt het spel minder voorspelbaar en saai dan als je slechts één move zou onthouden
            var index = new Random().Next(numberOfMoves);


           
            // verandering doorvoeren: de zet die de hoogste score opleverde wordt uitgevoerd
            ChessPositions[(Convert.ToInt32(optimalMoves[index].Pos2.X)+ 8*Convert.ToInt32(optimalMoves[index].Pos2.Y))/60].Piece= ChessPositions[(Convert.ToInt32(optimalMoves[index].Pos1.X) + 8*Convert.ToInt32(optimalMoves[index].Pos1.Y))/60].Piece;
            ChessPositions[(Convert.ToInt32(optimalMoves[index].Pos1.X) + 8 * Convert.ToInt32(optimalMoves[index].Pos1.Y)) / 60].Piece = null;
            // Einde spel???
            EndOfGame = HasGameEnded(true);


        }


        

        public int Min(int counter)
        {
            int minValue = 999999;
            
            // indien recursief algoritme bepaald aantal stappen heeft gemaakt: algoritme niet meer uitvoeren maar score berekenen en deze teruggeven
            if (counter == level)
            {
               minValue= CalculateScore();

            }

            else { 
            counter++;


                foreach (var stuk in ChessPositions)
                {
                    if (stuk.Piece != null && (stuk.Piece.Player==true))
                    {

                        CheckRules(stuk);
                        var pieces2 = ChessPositions.Where(piece => piece.Allowed == true).ToList();
                        foreach (var piece in pieces2)
                        {

                            // indien een koning genomen wordt: spel voorbij => teller op 2 zetten
                            if (piece.Piece != null && piece.Piece.Type == PieceType.King)
                                counter = level;
                            
                                var temp = piece.Piece;
                                piece.Piece = stuk.Piece;
                                stuk.Piece = null;

                                int score = Max(counter);

                                if (minValue > score)
                                {
                                    minValue = score;

                                }
                                stuk.Piece = piece.Piece;
                                piece.Piece = temp;

                 

                        }



                    }

                }


            }

            return minValue;

        }

        // indien recursief algoritme bepaald aantal stappen heeft gemaakt: algoritme niet meer uitvoeren maar score berekenen en deze teruggeven

        public int Max(int counter)
        {
            int maxScore = -999999;


            if (counter ==level)
            {

               maxScore= CalculateScore();

            }
            else { 

            counter++;
          


            foreach (var stuk in ChessPositions)
            {
                    if (stuk.Piece != null && (stuk.Piece.Player==false))
                    {

                        CheckRules(stuk);
                        var pieces = ChessPositions.Where(piece => piece.Allowed == true).ToList();
                        foreach (var piece in pieces)
                        {
                            // indien een koning genomen wordt: spel voorbij => teller op maximum: je hoeft geen verdere zetten meer te bekijken
                            if (piece.Piece != null && piece.Piece.Type == PieceType.King)
                            {
                                counter = level;
                            }
                          
                                var temp = piece.Piece;
                                piece.Piece = stuk.Piece;
                                stuk.Piece = null;

                                int score = Max(counter);

                                if (maxScore < score)
                                {
                                    maxScore = score;

                                }
                                stuk.Piece = piece.Piece;
                                piece.Piece = temp;

                            



                        }


                    }
                }




            }

            return maxScore;
        }

        public bool HasGameEnded(bool player)
        {

            // veranderingen in ChessPieces voorlopig niet doorgeven aan View
            foreach (var location in ChessPositions)
                location.SupressNotification = true;



            // loopen door alle stukken van de kleur die aan zet is 
            foreach (var ownPiece in (ChessPositions.Where(stuk => stuk.Piece != null && stuk.Piece.Player == player)).ToList())
            {
              
                CheckRules(ownPiece);
                // lijst van moves die dit stuk kan maken
                var allowedPositions = ChessPositions.Where(position => position.Allowed == true).ToList();



                // alle moves uitproberen
                foreach (var position in allowedPositions)
                {

                    var temp = position.Piece;
                    position.Piece = ownPiece.Piece;
                    ownPiece.Piece = null;

                    // alle stukken van de andere kleur ophalen
                    var opposingPieces = (ChessPositions.Where(opposingPiece => opposingPiece.Piece != null && opposingPiece.Piece.Player == !player)).ToList();


                    // vraag: kan één van hen meteen de koning nemen na deze move
                    // in dat geval: move is verboden want speler mag zichzelf niet schaak zetten


                    // eerst referentievariabele naar koning maken
                    var king = ChessPositions.First(stuk1 => stuk1.Piece != null && stuk1.Piece.Type == PieceType.King && stuk1.Piece.Player == player);


                    var count = 0;
                    king.Allowed = false;
                    // voor alle stukken afzonderlijk controleren of het de koning kan nemen
                    // indien ja: uit de loop: move is verboden
                    while (king.Allowed == false && count < opposingPieces.Count())
                    {
                        CheckRules(opposingPieces.ElementAt(count));



                        count++;
                    }

                    


                   

                    // indien de koning bij deze zet buiten schot blijft (king.Allowed==false)
                    // => deze move is toegestaan en dus is spel is niet gedaan

                    // => methode geeft'false" terug (nadat beginsituatie is hersteld en UI terug is geactiveerd)
                    if (king.Allowed == false)
                    {
                        ownPiece.Piece = position.Piece;
                        position.Piece = temp;
                        foreach (var location in ChessPositions)
                        {

                            location.SupressNotification = false;


                        }

                        return false;
                    }

                    // anders: beginsituatie herstellen en volgende move proberen
                    ownPiece.Piece = position.Piece;
                    position.Piece = temp;


                }

                // indien alle moves zijn geprobeerd maar koning stond telkens schaak: 
                // spel gedaan = > true teruggeven

               
                        
                    }

            //wijzigingen terug doorgeven aan View
            foreach (var location in ChessPositions)
            {

                location.SupressNotification = false;


            }
            // spel is gedaan, maar is het schaakmat of pat?
            CheckmateOrStalemate(player);
            return true;

                }

        // bepalen of er sprake is van schaakmat of pat? En wie is de winnaar?
        // d.w.z.: staat de koning van de speler die aan zet is al schaak in zijn huidige positie?
        //ja => mat, nee => pat
        public void CheckmateOrStalemate(bool player)
        {
            // voor alle stukken van de tegenspeler afzonderlijk controleren of het de koning kan bereiken

            bool checkmate = false;

            var opposingPieces = (ChessPositions.Where(piece => piece.Piece != null && piece.Piece.Player != player)).ToList();

            var king = ChessPositions.First(piece => piece.Piece != null && piece.Piece.Player == player && piece.Piece.Type == PieceType.King);

            var counter = 0;

            while(checkmate==false && counter<opposingPieces.Count())
            {

                CheckRules(opposingPieces.ElementAt(counter));

                if (king.Allowed == true)
                {
                    checkmate = true;

                }

                counter++;

            }

            // gepaste boodschap geven

            if (!checkmate)
            {
                MessageBox.Show("Stalemate", "The end", MessageBoxButton.OK);

            }
            else {
                MessageBox.Show(player ? "Checkmate. You lost. :(" : "Checkmate. Congratulations, you won! :)","The end", MessageBoxButton.OK);

            }


            

        }

        // controleren of de speler zichzelf schaak heeft gezet
        public bool Check()
        {

            var blackPieces = ChessPositions.Where(piece=>piece.Piece!=null && piece.Piece.Player==false).ToList();




            // voor alle zwarte stukken nagaan wat de legale zetten zijn
            foreach (var piece in blackPieces)
            {

                CheckRules(piece);

                // kijken of één van de zwarte stukken een koning kan nemen
                // indien ja: is hoe dan ook witte koning, want stukken mogen geen andere stukken van de eigen kleur nemen
                var allowed = ChessPositions.FirstOrDefault(location => location.Allowed == true &&location.Piece!=null && location.Piece.Type == PieceType.King);

                // in dat geval: wit staat inderdaad schaak => methode retourneert schaak
                if (allowed != null)
                {
                    return true;
                }


            }

            // als deze code gelezen wordt: witte koning staat niet schaak =>return false

            return false;


        }


            }
    


        }
       
  