# Chess1
Simple chessgame with an AI in .Net WPF. 
The AI is a fairly standard minimax-algorithm.

Gameplay: The player can control the white pieces, the AI responds to every move by moving one of the black ones in turn. 
In order to make a move, the player first selects the piece he wants to move by mouseclick, then uses the same method to select the position he wants to move it to. 
The application does not allow the player to break the basic rules of chess, instead the player can click on an "illegal" field if he wants to undo his prior selection of a piece.

The current version does not yet inform the player that the game has ended: the game continues indefinitely even after one of the kings has died. The player has to use his own judgment to establish whether or not the game has ended and whether he has won or lost. 
