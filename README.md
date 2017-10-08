# Chess1
Simple chessgame with an AI in .Net WPF The AI is a min-max algorithm.

Gameplay: The player can control the white pieces, the AI responds to every move by moving one of the black ones in turn. 
In order to make a move, the player first selects the piece he wants to move by mouseclick, and then repeats this action with the field he wants to move it to. 
After a move has been completed, none of the fields is selected anymore: this simplifies gameplay. 
The application does not allow the player to break the basic rules of chess, instead the player can click on an "illegal" field if he wants to undo his prior selection of a piece.

The current version does not yet inform the player that the game has ended: the game continues indefinitely even after one of the kings has died. 
The player has to use his own judgment to establish whether or not the game has ended and whether he has won or lost. 
A new feature that relieves the player from this small burden will be added soon, along with some multi-threading and some minor modifications to the algorithm in order to make it behave in a slightly more agressive manner.
