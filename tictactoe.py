import tkinter as tk
import random

# Constants for players
PLAYER_X = "X"
PLAYER_O = "O"

# Winning combinations
WIN_COMBOS = [
    (0, 1, 2),
    (3, 4, 5),
    (6, 7, 8),
    (0, 3, 6),
    (1, 4, 7),
    (2, 5, 8),
    (0, 4, 8),
    (2, 4, 6),
]

class TicTacToe(tk.Tk):
    def __init__(self):
        super().__init__()
        self.title("Tic Tac Toe")
        self.resizable(False, False)
        self.mode = tk.StringVar(value="2P")
        self.game_running = False
        self.current_player = PLAYER_X
        self.board = ["" for _ in range(9)]
        self.create_widgets()

    def create_widgets(self):
        toolbar = tk.Frame(self)
        toolbar.grid(row=0, column=0, padx=5, pady=5)

        mode_frame = tk.Frame(toolbar)
        mode_frame.pack(side=tk.LEFT)
        tk.Radiobutton(mode_frame, text="2 Player", variable=self.mode, value="2P").pack(side=tk.LEFT)
        tk.Radiobutton(mode_frame, text="Play Against Computer", variable=self.mode, value="AI").pack(side=tk.LEFT)

        self.start_btn = tk.Button(toolbar, text="Start", command=self.start_game)
        self.start_btn.pack(side=tk.LEFT, padx=10)

        self.status = tk.Label(toolbar, text="Click Start to begin Game.")
        self.status.pack(side=tk.LEFT)

        board_frame = tk.Frame(self)
        board_frame.grid(row=1, column=0, padx=5, pady=5)
        self.buttons = []
        for i in range(9):
            btn = tk.Button(board_frame, text="", width=5, height=2, command=lambda idx=i: self.spot_clicked(idx))
            btn.grid(row=i//3, column=i%3)
            self.buttons.append(btn)

    def start_game(self):
        self.board = ["" for _ in range(9)]
        for btn in self.buttons:
            btn.config(text="", bg="SystemButtonFace")
        self.current_player = PLAYER_X
        self.game_running = True
        self.status.config(text=f"Current Turn: {self.current_player}")

    def spot_clicked(self, index):
        if not self.game_running:
            return
        if self.board[index]:
            return
        self.make_move(index, self.current_player)
        if self.check_end():
            return
        self.switch_turns()
        if self.mode.get() == "AI" and self.game_running:
            self.after(200, self.computer_move)

    def make_move(self, index, player):
        self.board[index] = player
        self.buttons[index].config(text=player)

    def switch_turns(self):
        self.current_player = PLAYER_O if self.current_player == PLAYER_X else PLAYER_X
        self.status.config(text=f"Current Turn: {self.current_player}")

    def computer_move(self):
        index = self.choose_computer_move()
        if index is not None:
            self.make_move(index, PLAYER_O)
            if self.check_end():
                return
            self.switch_turns()

    def choose_computer_move(self):
        # Offense: winning move
        for idx in self.available_moves():
            copy = self.board.copy()
            copy[idx] = PLAYER_O
            if self.winner(copy) == PLAYER_O:
                return idx
        # Defense: block opponent
        for idx in self.available_moves():
            copy = self.board.copy()
            copy[idx] = PLAYER_X
            if self.winner(copy) == PLAYER_X:
                return idx
        # Ranking: center, corners, then random
        center = 4
        if center in self.available_moves():
            return center
        corners = [i for i in [0,2,6,8] if i in self.available_moves()]
        if corners:
            return random.choice(corners)
        moves = self.available_moves()
        return random.choice(moves) if moves else None

    def available_moves(self):
        return [i for i, spot in enumerate(self.board) if spot == ""]

    def winner(self, board):
        for a,b,c in WIN_COMBOS:
            if board[a] and board[a] == board[b] == board[c]:
                return board[a]
        return None

    def check_end(self):
        winner = self.winner(self.board)
        if winner:
            for a,b,c in WIN_COMBOS:
                if self.board[a] == self.board[b] == self.board[c] == winner:
                    for i in (a,b,c):
                        self.buttons[i].config(bg="lightgreen")
                    break
            self.status.config(text=f"Winner is: {winner}")
            self.game_running = False
            return True
        if not self.available_moves():
            for btn in self.buttons:
                btn.config(bg="lightgray")
            self.status.config(text="Tie")
            self.game_running = False
            return True
        return False

if __name__ == "__main__":
    app = TicTacToe()
    app.mainloop()
