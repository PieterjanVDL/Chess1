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
   public class ChessVM:ViewModelBase
    {
        //initialiseren ViewModel: positions is beginsituatie van het schaakspel (zie App.xaml.cs)
        public ChessVM(ObservableCollection<ChessPosition> positions)
        {
            ChessPieces = positions;



        }

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
            get {

             

                return selectedItem; }

            set
            {
                // als er reeds een wit schaakstuk geselecteerd is

                if (selectedItem != null&& selectedItem.Piece!=null&&selectedItem.Piece.Player==true)
                {
                 


                        var temp = new ChessPiece(selectedItem.Piece.Type, selectedItem.Piece.Player);





                    //laten regels toe dat je dit schaakstuk naar de aangeklikte plaatse verplaatst
                
                    if (value.Allowed == true)
                    {

                       

                        // indien ja:
                        ChessPieces[Convert.ToInt32((selectedItem.Pos.X + 8 * selectedItem.Pos.Y) / 60)].Piece = null;






                        ChessPieces[(Convert.ToInt32((value.Pos.X + 8 * value.Pos.Y) / 60))].Piece = temp;
                        RaisePropertyChanged("ChessPieces");


                        MinMax();
                    }
                    else {
                        //indien de zet niet is toegestaan: aangeklikte element selecteren en regels raadplegen
                        selectedItem = value;
                        CheckRegels(selectedItem);
                    }
                    //nadat zet is gemaakt: geen enkel element geselecteerd
                    SelectedIndex = -1;
                     }


              // indien er nog geen ChessPosition  geselecteerd was of deze bevatte geen wit schaakstuk
                 // => selecteren van de aangeklikte positie en regels toepassen
                else
                {

                    selectedItem = value;
                    CheckRegels(selectedItem);


                }
                RaisePropertyChanged("SelectedItem");


            }
        }




        private ObservableCollection<ChessPosition> chessPieces = new ObservableCollection<ChessPosition>();


        public ObservableCollection<ChessPosition> ChessPieces
        {
            get
            {
                return chessPieces;
            }
            set
            {
               
                chessPieces = value;
                RaisePropertyChanged("ChessPieces");

            }

        }

       
        // functie CheckRegels() bepaald naar welke posities een schaakstuk verplaatst kan worden
        public void CheckRegels(ChessPosition vorig)
        {

            // indien er geen schaakstuk op de betreffende positie staat kan je dit ook niet verplaatsen
            //=> meteen afbreken van CheckRegels()
            

            if (vorig.Piece == null)
            {
                return;
            }

            // coördinaten van geselecteerde item 
            int xCoordinate = Convert.ToInt32(vorig.Pos.X)/60;
            int yCoordinate = Convert.ToInt32(vorig.Pos.Y)/60;


            // kopie van deze coördinaten

            int x = xCoordinate;
            int y= yCoordinate;


            //aanvankelijk wordt de property "Allowed" van alle elementen in de array chessPieces op 'false' gezet
            //= > het schaakstuk kan niet bewegen
        
            for (var i = 0; i < chessPieces.Count; i++)
            {
                chessPieces[i].Allowed = false;
            }

            // vervolgens passen we de regels toe die horen bij een schaakstuk van dit type en deze speler
            // d.w.z.: e.v.t. toestemming geven om naar een bepaalde positie te bewegen door de property "Allowed" van deze ChessPosition op 'true' te zetten

            // zwarte pion: kan alleen "omlaag" bewegen

            if (vorig.Piece.Type == PieceType.Pawn && vorig.Piece.Player == false)
            {
               y= yCoordinate;

               
                
                    y++;

                if ((y<=7)&& ChessPieces[xCoordinate + 8 * y].Piece == null)
                {
                    ChessPieces[xCoordinate + 8 * y].Allowed = true;

                    y++;
                
                 if (yCoordinate==1 &&ChessPieces[xCoordinate + 8 * y].Piece == null)
                {
                    ChessPieces[xCoordinate + 8 * y].Allowed = true;
                }

                    }


                y = yCoordinate;
                x = xCoordinate;

               

                if ((xCoordinate + 1) <= 7 && ((yCoordinate+1)<=7)&& ChessPieces[(xCoordinate + 1) + 8 * (yCoordinate + 1)].Piece != null && ChessPieces[(xCoordinate + 1) + 8 * (yCoordinate + 1)].Piece.Player == true)
                {

                    ChessPieces[xCoordinate + 1 + 8 * (yCoordinate + 1)].Allowed = true;
                }

                if((xCoordinate-1)>=0 && ((yCoordinate+1)<=7)&&ChessPieces[(xCoordinate-1) +8*(yCoordinate+1)].Piece!=null && ChessPieces[xCoordinate-1+8*(yCoordinate+1)].Piece.Player==true)
                    {

                    ChessPieces[xCoordinate - 1 + 8 * (yCoordinate + 1)].Allowed = true;

                }

               

                }
                
                


            
            // witte pion: kan alleen "omhoog" bewegen

            else if (vorig.Piece.Type == PieceType.Pawn && vorig.Piece.Player == true)
            {
                y = yCoordinate;



                y--;

                if (ChessPieces[xCoordinate + 8 * y].Piece == null)
                {
                    ChessPieces[xCoordinate + 8 * y].Allowed = true;

                    y--;

                    if (yCoordinate == 6 && ChessPieces[xCoordinate + 8 * y].Piece == null)
                    {
                        ChessPieces[xCoordinate + 8 * y].Allowed = true;
                    }

                }


                y = yCoordinate;
                x = xCoordinate;



                if ((xCoordinate + 1) <= 7 && ((yCoordinate -1)>=0) && ChessPieces[(xCoordinate + 1) + 8 * (yCoordinate -1)].Piece != null && ChessPieces[(xCoordinate + 1) + 8 * (yCoordinate -1)].Piece.Player == false)
                {

                    ChessPieces[xCoordinate + 1 + 8 * (yCoordinate - 1)].Allowed = true;
                }

                if ((xCoordinate - 1) >= 0 && ((yCoordinate - 1)>=0) && ChessPieces[(xCoordinate - 1) + 8 * (yCoordinate - 1)].Piece != null && ChessPieces[xCoordinate - 1 + 8 * (yCoordinate - 1)].Piece.Player == false)
                {

                    ChessPieces[xCoordinate - 1 + 8 * (yCoordinate - 1)].Allowed = true;

                }



            }

            // loper

            else if (vorig.Piece.Type == PieceType.Bishop)
            {

                x = xCoordinate;
                y = yCoordinate;

                x++;
                y++;
                while (x <= 7 && y <= 7 && (chessPieces[x + 8 * y].Piece == null || chessPieces[x + 8 * y].Piece.Player != vorig.Piece.Player) && (chessPieces[x + 8 * y - 9].Piece == null || chessPieces[x + 8 * y - 9].Piece.Player != (!vorig.Piece.Player)))
                {




                    chessPieces[x + 8 * y].Allowed = true;
                    x++;
                    y++;

                }

                x = xCoordinate;
                y = yCoordinate;
                x--;
                y++;
                while (x >= 0 && y <= 7 && (chessPieces[x + 8 * y].Piece == null || chessPieces[x + 8 * y].Piece.Player != vorig.Piece.Player) && (chessPieces[x + 8 * y - 7].Piece == null || chessPieces[x + 8 * y - 7].Piece.Player != (!vorig.Piece.Player)))
                {

                    chessPieces[x + 8 * y].Allowed = true;
                    x--;
                    y++;

                }

                x = xCoordinate;
                y = yCoordinate;
                x++;
                y--;
                while (x <= 7 && y >= 0 && (chessPieces[x + 8 * y].Piece == null || chessPieces[x + 8 * y].Piece.Player != vorig.Piece.Player) && (chessPieces[x + 8 * y + 7].Piece == null || chessPieces[x + 8 * y + 7].Piece.Player != (!vorig.Piece.Player)))
                {

                    chessPieces[x + 8 * y].Allowed = true;
                    x++;
                    y--;

                }

                x = xCoordinate;
                y = yCoordinate;

                x--;
                y--;
                while (x >= 0 && y >= 0 && (chessPieces[x + 8 * y].Piece == null || chessPieces[x + 8 * y].Piece.Player != vorig.Piece.Player) && (chessPieces[x + 8 * y + 9].Piece == null || chessPieces[x + 8 * y + 9].Piece.Player != (!vorig.Piece.Player)))
                {

                    chessPieces[x + 8 * y].Allowed = true;
                    x--;
                    y--;

                }



            }

            
            // koning

           else if (vorig.Piece.Type == PieceType.King)
            {

                x = xCoordinate + 1;
                y = yCoordinate;
               
                if ((x<=7)&&(ChessPieces[x+y*8].Piece==null || ChessPieces[x  + y * 8].Piece.Player!=vorig.Piece.Player))
                {
                    ChessPieces[x + 8*y].Allowed = true;
                }

                x = xCoordinate + 1;
                y = yCoordinate +1;

                if ((x <= 7) && (y<=7) && (ChessPieces[x + y * 8].Piece == null || ChessPieces[x + y * 8].Piece.Player != vorig.Piece.Player))
                {
                    ChessPieces[x + 8*y].Allowed = true;
                }

                x = xCoordinate;
                y = yCoordinate + 1;

                if ((y <= 7) && (ChessPieces[x + y * 8].Piece == null || ChessPieces[x + y * 8].Piece.Player != vorig.Piece.Player))
                {
                    ChessPieces[x + 8 * y].Allowed = true;
                }

                x = xCoordinate -1;
                y = yCoordinate;

                if ((x>=0) && (ChessPieces[x + y * 8].Piece == null || ChessPieces[x + y * 8].Piece.Player != vorig.Piece.Player))
                {
                    ChessPieces[x + 8 * y].Allowed = true;
                }

                x = xCoordinate - 1;
                y = yCoordinate-1;

                if ((y>= 0) && (x>=0)&&(ChessPieces[x + y * 8].Piece == null || ChessPieces[x + y * 8].Piece.Player != vorig.Piece.Player))
                {
                    ChessPieces[x + 8 * y].Allowed = true;
                }

                x = xCoordinate;
                y = yCoordinate - 1;

                if ( (y >= 0) && (ChessPieces[x + y * 8].Piece == null || ChessPieces[x + y * 8].Piece.Player != vorig.Piece.Player))
                {
                    ChessPieces[x + 8 * y].Allowed = true;
                }

                x = xCoordinate+1;
                y = yCoordinate - 1;

                if ((y >= 0)&&(x<=7) && (ChessPieces[x + y * 8].Piece == null || ChessPieces[x + y * 8].Piece.Player != vorig.Piece.Player))
                {
                    ChessPieces[x + 8 * y].Allowed = true;
                }

                x = xCoordinate-1;
                y = yCoordinate + 1;

                if ((y<=7) && (x>=0)&&(ChessPieces[x + y * 8].Piece == null || ChessPieces[x + y * 8].Piece.Player != vorig.Piece.Player))
                {
                    ChessPieces[x + 8 * y].Allowed = true;
                }



            }


            //paard
            else if (vorig.Piece.Type == PieceType.Knight)
            {
                x = xCoordinate+2;
                y = yCoordinate+1;

                if (x <= 7 && y <= 7 && (ChessPieces[x + 8 * y].Piece == null || ChessPieces[x + 8 * y].Piece.Player!= vorig.Piece.Player) )
                {
                    ChessPieces[x + 8 * y].Allowed = true;
                }



                x = xCoordinate + 1;
                y = yCoordinate + 2;

                if (x <= 7 && y <= 7&&(ChessPieces[x + 8 * y].Piece == null || ChessPieces[x + 8 * y].Piece.Player != vorig.Piece.Player))
                {
                    ChessPieces[x + 8 * y].Allowed = true;
                }


                x = xCoordinate - 1;
                y = yCoordinate + 2;

                if (x >=0 && y<= 7 && (ChessPieces[x + 8 * y].Piece == null || ChessPieces[x + 8 * y].Piece.Player != vorig.Piece.Player))
                {
                    ChessPieces[x + 8 * y].Allowed = true;
                }


                x = xCoordinate + 1;
                y = yCoordinate - 2;

                if (x <= 7 && y >= 0 && (ChessPieces[x + 8 * y].Piece == null || ChessPieces[x + 8 * y].Piece.Player != vorig.Piece.Player))
                {
                    ChessPieces[x + 8 * y].Allowed = true;
                }


                x = xCoordinate +2;
                y = yCoordinate - 1;

                if (x <= 7 && y >= 0 && (ChessPieces[x + 8 * y].Piece == null || ChessPieces[x + 8 * y].Piece.Player != vorig.Piece.Player))
                {
                    ChessPieces[x + 8 * y].Allowed = true;
                }

                x = xCoordinate - 2;
                y = yCoordinate + 1;

                if (x >=0 && y <=7&&(ChessPieces[x + 8 * y].Piece == null || ChessPieces[x + 8 * y].Piece.Player != vorig.Piece.Player))
                {
                    ChessPieces[x + 8 * y].Allowed = true;
                }

                x = xCoordinate - 1;
                y = yCoordinate - 2;

                if (x >=0 && y >= 0 && (ChessPieces[x + 8 * y].Piece == null || ChessPieces[x + 8 * y].Piece.Player != vorig.Piece.Player))
                {
                    ChessPieces[x + 8 * y].Allowed = true;
                }

                x = xCoordinate - 2;
                y = yCoordinate - 1;


                if (x >= 0 && y >= 0 && (ChessPieces[x+8*y].Piece==null || ChessPieces[x+8*y].Piece.Player!=vorig.Piece.Player))
                {

                    
                        ChessPieces[x + 8 * y].Allowed = true;
                    }

               


            }
                    
            //toren
           else if (vorig.Piece.Type == PieceType.Rook)
            {
                x = xCoordinate+1;
                y = yCoordinate;

                

                while (x<=7 && (ChessPieces[x+y*8].Piece==null || ChessPieces[x+y*8].Piece.Player!= vorig.Piece.Player)&& (chessPieces[(x-1) + 8 * y ].Piece == null || chessPieces[(x-1) + 8 * y].Piece.Player != (!vorig.Piece.Player)))
                {
                    ChessPieces[x + y * 8].Allowed = true;
                    x++;
                }

                x = xCoordinate-1;
                y = yCoordinate;
               
                while (x>=0 && (ChessPieces[x + y * 8].Piece == null || ChessPieces[x + y * 8].Piece.Player != vorig.Piece.Player)&&(chessPieces[x+1 + 8 *y].Piece == null || chessPieces[(x+1) + 8 * y ].Piece.Player != (!vorig.Piece.Player)))
                {
                    ChessPieces[x + y * 8].Allowed = true;
                    x--;
                }

                x = xCoordinate;
                y = yCoordinate+1;
           

                while (y <= 7 && (ChessPieces[x + y * 8].Piece == null || ChessPieces[x + y * 8].Piece.Player != vorig.Piece.Player)&& (chessPieces[x + 8 * (y-1)].Piece == null || chessPieces[x + 8 *( y -1)].Piece.Player != (!vorig.Piece.Player)))
                {
                    ChessPieces[x + y * 8].Allowed = true;
                    y++;
                }

                x = xCoordinate;
                y = yCoordinate-1;
            

                while (y >= 0 && (ChessPieces[x + y * 8].Piece == null || ChessPieces[x + y * 8].Piece.Player != vorig.Piece.Player)&& (chessPieces[x + 8 * (y+1)].Piece == null || chessPieces[x + 8 * (y+1)].Piece.Player != (!vorig.Piece.Player)))
                {
                    ChessPieces[x + y * 8].Allowed = true;
                    y--;
                }

            }

            //koningin
            else if (vorig.Piece.Type == PieceType.Queen)
            {
                x = xCoordinate + 1;
                y = yCoordinate;



                while (x <= 7 && (ChessPieces[x + y * 8].Piece == null || ChessPieces[x + y * 8].Piece.Player != vorig.Piece.Player) && (chessPieces[(x - 1) + 8 * y].Piece == null || chessPieces[(x - 1) + 8 * y].Piece.Player != (!vorig.Piece.Player)))
                {
                    ChessPieces[x + y * 8].Allowed = true;
                    x++;
                }

                x = xCoordinate - 1;
                y = yCoordinate;

                while (x >= 0 && (ChessPieces[x + y * 8].Piece == null || ChessPieces[x + y * 8].Piece.Player != vorig.Piece.Player) && (chessPieces[x + 1 + 8 * y].Piece == null || chessPieces[(x + 1) + 8 * y].Piece.Player != (!vorig.Piece.Player)))
                {
                    ChessPieces[x + y * 8].Allowed = true;
                    x--;
                }

                x = xCoordinate;
                y = yCoordinate + 1;


                while (y <= 7 && (ChessPieces[x + y * 8].Piece == null || ChessPieces[x + y * 8].Piece.Player != vorig.Piece.Player) && (chessPieces[x + 8 * (y - 1)].Piece == null || chessPieces[x + 8 * (y - 1)].Piece.Player != (!vorig.Piece.Player)))
                {
                    ChessPieces[x + y * 8].Allowed = true;
                    y++;
                }

                x = xCoordinate;
                y = yCoordinate - 1;


                while (y >= 0 && (ChessPieces[x + y * 8].Piece == null || ChessPieces[x + y * 8].Piece.Player != vorig.Piece.Player) && (chessPieces[x + 8 * (y + 1)].Piece == null || chessPieces[x + 8 * (y + 1)].Piece.Player != (!vorig.Piece.Player)))
                {
                    ChessPieces[x + y * 8].Allowed = true;
                    y--;
                }

                x = xCoordinate;
                y = yCoordinate;

                x++;
                y++;
                while (x <= 7 && y <= 7 && (chessPieces[x + 8 * y].Piece == null || chessPieces[x + 8 * y].Piece.Player != vorig.Piece.Player) && (chessPieces[x + 8 * y - 9].Piece == null || chessPieces[x + 8 * y - 9].Piece.Player != (!vorig.Piece.Player)))
                {




                    chessPieces[x + 8 * y].Allowed = true;
                    x++;
                    y++;

                }

                x = xCoordinate;
                y = yCoordinate;
                x--;
                y++;
                while (x >= 0 && y <= 7 && (chessPieces[x + 8 * y].Piece == null || chessPieces[x + 8 * y].Piece.Player != vorig.Piece.Player) && (chessPieces[x + 8 * y - 7].Piece == null || chessPieces[x + 8 * y - 7].Piece.Player != (!vorig.Piece.Player)))
                {

                    chessPieces[x + 8 * y].Allowed = true;
                    x--;
                    y++;

                }

                x = xCoordinate;
                y = yCoordinate;
                x++;
                y--;
                while (x <= 7 && y >= 0 && (chessPieces[x + 8 * y].Piece == null || chessPieces[x + 8 * y].Piece.Player != vorig.Piece.Player) && (chessPieces[x + 8 * y + 7].Piece == null || chessPieces[x + 8 * y + 7].Piece.Player != (!vorig.Piece.Player)))
                {

                    chessPieces[x + 8 * y].Allowed = true;
                    x++;
                    y--;

                }

                x = xCoordinate;
                y = yCoordinate;

                x--;
                y--;
                while (x >= 0 && y >= 0 && (chessPieces[x + 8 * y].Piece == null || chessPieces[x + 8 * y].Piece.Player != vorig.Piece.Player) && (chessPieces[x + 8 * y + 9].Piece == null || chessPieces[x + 8 * y + 9].Piece.Player != (!vorig.Piece.Player)))
                {

                    chessPieces[x + 8 * y].Allowed = true;
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
            foreach (var piece in ChessPieces.Where(piece=>piece.Piece!=null))
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
        // op einde algoritme wordt de door de computerspeler gekozen wijziging doorgevoerd
        public void MinMax()

        {

            // we zullen verschillende situaties simuleren 
            //=> vooraleer algoritme één bepaalde zet geselecteerd heeft als optimaal moeten positieveranderingen niet doorgegeven worden aan View
           
            foreach (var locatie in ChessPieces)
            {
                locatie.SupressNotification = true;
            }
            
            int teller = 0;
            int maxScore = -999999;
            var coordinaten = new List<Move>();

            // de computerspeler bekijkt beurtelings alle zwarte stukken

            foreach (var stuk in ChessPieces.Where(stuk=>((stuk.Piece!=null)&&(stuk.Piece.Player==false))))
            {

                // de computerspeler bepaalt welke zetten legaal zijn voor dit stuk en stopt alle toegestane bestemmingen in een List 
                    CheckRegels(stuk);

                var pieces1 = ChessPieces.Where(piece => piece.Allowed == true).ToList();

                // de computerspeler simuleert beurtelings alle toegestane zetten voor het stuk "stuk.Piece" en beoordeelt hun mate van wenselijkheid

                    foreach (var piece in pieces1) 
                    {
                  
                
                            var temp = piece.Piece; 
                            piece.Piece = stuk.Piece;
                            stuk.Piece = null;

                    // welke score zou dit opleveren, aangenomen dat speler 'wit' de score functie wil minimaliseren
                            int score = Min(teller);
                   
                    // indien deze score groter is dan bij eerder gesimuleerde zetten: score onthouden en coördinaten onthouden van zowel het verzette stuk als zijn bestemming 
                    // eerder onthouden bewegingen verwijderen uit lijst 'coordinaten' aangezien deze een lagere score hebben

                            if (maxScore < score)
                            {
                                maxScore = score;
                        coordinaten.Clear(); 
                        coordinaten.Add(new Move { Pos1 = stuk.Pos, Pos2 = piece.Pos });
                        
                            }

                            // indien score even groot is als de maximale score van vorige 'moves': toevoegen aan lijst coordinaten

                    if (maxScore == score)
                        {
                        coordinaten.Add(new Move { Pos1 = stuk.Pos, Pos2 = piece.Pos });

                    }

                            // stukken terugzetten naar originele positie
                    stuk.Piece = piece.Piece;
                    piece.Piece = temp;



                }

                   
           
            }

            // aanpassingen aan View toestaan

            foreach (var locatie in ChessPieces)
            {
                locatie.SupressNotification = false;
            }


            // willekeurig één van de optimale bewegingen kiezen

            var numberOfMoves = coordinaten.Count;


            // random één van de onthouden (en dus optimale) moves kiezen
            // dit maakt het spel minder voorspelbaar en saai dan als je slechts één move zou onthouden
            var index = new Random().Next(numberOfMoves);


           
            // verandering doorvoeren: de zet die de hoogste score opleverde wordt uitgevoerd
            ChessPieces[(Convert.ToInt32(coordinaten[index].Pos2.X)+ 8*Convert.ToInt32(coordinaten[index].Pos2.Y))/60].Piece= ChessPieces[(Convert.ToInt32(coordinaten[index].Pos1.X) + 8*Convert.ToInt32(coordinaten[index].Pos1.Y))/60].Piece;
            ChessPieces[(Convert.ToInt32(coordinaten[index].Pos1.X) + 8 * Convert.ToInt32(coordinaten[index].Pos1.Y)) / 60].Piece = null;



        }


        

        public int Min(int teller)
        {
            int minValue = 999999;
            
            // indien recursief algoritme bepaald aantal stappen heeft gemaakt: algoritme niet meer uitvoeren maar score berekenen en deze teruggeven
            if (teller == 2)
            {
               minValue= CalculateScore();

            }

            else { 
            teller++;


                foreach (var stuk in ChessPieces)
                {
                    if (stuk.Piece != null && (stuk.Piece.Player==true))
                    {

                        CheckRegels(stuk);
                        var pieces2 = ChessPieces.Where(piece => piece.Allowed == true).ToList();
                        foreach (var piece in pieces2)
                        {

                            
                                var temp = piece.Piece;
                                piece.Piece = stuk.Piece;
                                stuk.Piece = null;

                                int score = Max(teller);

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

        public int Max(int teller)
        {
            int maxScore = -999999;


            if (teller ==2)
            {

               maxScore= CalculateScore();

            }
            else { 

            teller++;
          


            foreach (var stuk in ChessPieces)
            {
                    if (stuk.Piece != null && (stuk.Piece.Player==false))
                    {

                        CheckRegels(stuk);
                        var pieces = ChessPieces.Where(piece => piece.Allowed == true).ToList();
                        foreach (var piece in pieces)
                        {

                          
                                var temp = piece.Piece;
                                piece.Piece = stuk.Piece;
                                stuk.Piece = null;

                                int score = Max(teller);

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

       



        }
        }

