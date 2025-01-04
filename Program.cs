// By Y2kun
// Link to my Github: https://github.com/Y2kun
// An Overdesigned Tui for the Tower of Hanoi

namespace towerOfHanoi;
class Program {
    static void Main(string[] args) {
        // Unicode Box for Introduction
        Console.WriteLine("┏━━━━━━━━━━Tower of Hanoi━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
        Console.WriteLine("┃ Rules:                                                       ┃");
        Console.WriteLine("┃       1. Only one disc can be moved at a time.               ┃");
        Console.WriteLine("┃       2. No disc may be placed on top of a smaller disc.     ┃");
        Console.WriteLine("┃                                                              ┃");
        Console.WriteLine("┃ Controles:                                                   ┃");
        Console.WriteLine("┃       Input 1 to select the Left Pillar                      ┃");
        Console.WriteLine("┃       Input 2 to select the Middle Pillar                    ┃");
        Console.WriteLine("┃       Input 3 to select the Right Pillar                     ┃");
        Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
        Console.WriteLine("\nPlease Input with how many Discs you want to play.");
        int discCount = 3;
        // Reading User Input, and making sure it's usable
        try {
            discCount = Convert.ToInt32(Console.ReadLine());
        } catch {
            Console.WriteLine("Your Input was Invalid, it defaulted to 3");
        }
        Console.WriteLine("\nPlease Input, if the Board should be rendered using ASCII or Unicode. [a,\u001b[1mu\u001b[0m]");
        bool enabledASCII = false;
        if(Console.ReadLine() == "a") {
            enabledASCII = true;
        }        
        Board board = new Board(discCount, enabledASCII);
        Console.WriteLine("Please Input to play ToH or to let solve it automaticly [p,\u001b[1ms\u001b[0m]");
        // The following is for the Player
        DateTime now = DateTime.Now;
        if(Console.ReadLine() == "p") {
            board.draw();
            while(!board.win()) {
                // making sure the Input is acceptable
                int start = getNumber(board);
                int dest = getNumber(board);
                board.moveDisc(board.pillars[start], board.pillars[dest]);
                board.draw();
            }
            DateTime end = DateTime.Now;
            TimeSpan duration = end - now;
            // Yay the win message
            Console.WriteLine("Congratulations! The Tower of Hanoi has been succesfully completed.");
            Console.WriteLine("To Solve the Tower of Hanoi it took " + duration.ToString("m' mins and 's' secs'"));
            string filePath = "Scores.txt";
            // if (File.Exists(filePath)) {
            //     text = File.ReadAllLines(filePath);
            // }
            string output = "\nscore:\n" + duration.ToString("m' mins and 's' secs'");
            output += "\ndisks: " + discCount;
            File.AppendAllText(filePath, output);
            Console.WriteLine("The Score has been saved.");
        } else {
            // This is the recursive version
            board.draw();
            // RecursivSolver(2 * (discCount - 1) + discCount, board, board.pillars[0], board.pillars[1], board.pillars[2]);
            IterativeSolver(discCount, board);
            if (board.win()) {
                Console.WriteLine("The Tower of Hanoi has been succesfully solved!");
            }
        }
        
    }
    
    static int getNumber(Board board) {
        while (true) {
            try {
                return Math.Clamp(Convert.ToInt32(Console.ReadLine()), 1, board.pillars.Count()) - 1;
            } catch {
                Console.WriteLine("This is not a valid pillar index!");
            }
        }
    }

    static void RecursivSolver(int n, Board board, List<Disc> startPillar, List<Disc> midPillar, List<Disc> destPillar) {
        // Recursive Brute Force Algorithm for reaching the desired Disc Configuration
        if (n <= 0) return; // Leave current Functioncall as it has reached the end
        if (board.win()) return; // Leave Recursion when won
        RecursivSolver(n - 1, board, startPillar, destPillar, midPillar);
        // board.draw();
        board.moveDisc(startPillar, destPillar); // Move Disc
        RecursivSolver(n - 1, board, midPillar, destPillar, startPillar);
    }

    static void IterativeSolver(int n, Board board) {
        int totalMoves = (1 << n) - 1; // 2^n - 1
        List<Disc> startPillar = board.pillars[0];
        List<Disc> destPillar = board.pillars[2];
        List<Disc> midPillar = board.pillars[1];

        if (n % 2 == 0) {
            var temp = destPillar;
            destPillar = midPillar;
            midPillar = temp;
        }

        for (int i = 1; i <= totalMoves; i++) {
            int fromPillar = (i & i - 1) % 3; // Get the source pillar
            int toPillar = ((i | i - 1) + 1) % 3; // Get the destination pillar

            board.moveDisc(board.pillars[fromPillar], board.pillars[toPillar]);
            board.draw();
        }
    }
}
