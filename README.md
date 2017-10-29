# Chess1
Simple chessgame with an AI in .Net WPF. 
The AI is a minimax-algorithm.

Gameplay: The player can control the white pieces, the AI responds to every move by moving one of the black ones in turn. 
In order to make a move, the player first selects the piece he wants to move by mouseclick, then uses the same method to select the position he wants to move it to. 
The application does not allow the player to break the basic rules of chess, instead the player can click on an "illegal" field if he wants to undo his prior selection of a piece.
It is forbidden to make moves that leave your king exposed. Doing this will result in a warning and the opportunity to make another move.

The game ends when a player cannot make any move that does not leave the king exposed. Here, there are two options, depending on whether or not this player is already in check in the current situation. 
If it is, the game ends in a checkmate. The "stuck" player loses. 
If the king of this player is not yet in check (but can't move without getting in check), the result is a stalemate. Neither player wins.
