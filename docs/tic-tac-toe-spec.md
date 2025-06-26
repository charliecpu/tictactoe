# Tic Tac Toe  

## Overview  
This document describes requirements for the software implementation of the classic game of Tic Tac Toe. This software will allow two players to play against each other or for one player to play against the computer. A description of the game plus the requirements of this implementation are provided below.

## The Game  
Two players are presented with a 3x3 grid of empty spots. The players take turns marking an empty spot with their letter: the first player to move is **X** and the second player is **O**. The first player to fill a winning set defined as a row, column, or diagonal line of three spots wins. If all spots are filled and neither player has a winning set, then the game results in a tie.

## Software Implementation  

### UI Elements  
The software will present the Tic Tac Toe game with the following UI elements:

- **3x3 grid of buttons**
- **Toolbar (top of screen):**
  - Option buttons to choose between *2 Player* or *Play Against the Computer*
  - *Start* button
  - *Hint* button shows a possible winning move
  - Label to display *Game Status* message

### Player Modes  
There are two modes of play:

#### Two Player  
Each user clicks an empty button and their letter is displayed on the button.

#### Play Against the Computer  
The computer is letter **O**. After **X** s turn, the computer makes its move using the following algorithm:

1. **Offense:** If there is a winning spot, take it.
2. **Defense:** If the other player can win, take the spot to block.
3. **Take the spot with the highest rank:**
   - a. Center button  
   - b. Any corner  
   - c. Any available button chosen randomly

## Game Process and Rules  

- The game starts when the player clicks **Start**. Until then, all clicks on the spot buttons are ignored. Game status message:  
  > "Click Start to begin Game."

- After **Start** is clicked:
  - Status message:  
    > "Current Turn: X"
  - X clicks an empty spot; the button displays "X"
  - Turn changes to O, reflected in status
  - Clicking a taken spot has no effect
  - Clicking **Hint** highlights a winning spot in purple if one is available

- **Winning:**
  - Winning spots change background color
  - Status message:  
    > "Winner is: X/O"

- **Tie:**
  - All buttons change to a neutral color
  - Status message:  
    > "Tie"

- During **Win** or **Tie** state, spot clicks are ignored (but buttons should not be disabled to avoid faded appearance)

- Clicking **Start** during a game interrupts and begins a new game