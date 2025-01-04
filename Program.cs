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
            Console.WriteLine("Your Input was Invalid, it defaulted to 3\n");
        }
        Console.WriteLine("Please Input, if the Board should be rendered using ASCII or Unicode. [a,\u001b[1mu\u001b[0m]");
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

    // static void IterativeSolver(int n, Board board) {
    //     for (int i = 1; i <= (1 << n) - 1; i++) {
    //         // int startPillar, destPillar;
    //         int startPillar = (i & i - 1) % 3; // Get the source pillar
    //         int destPillar = ((i | i - 1) + 1) % 3; // Get the destination pillar

    //         board.moveDisc(board.pillars[startPillar], board.pillars[destPillar]);
    //         board.draw();
    //         // Thread.Sleep(100);
    //     }
    // }

    static void IterativeSolver(int n, Board board) {
        int startPillar = 0; // Source pillar (Left)
        int midPillar = 1;   // Auxiliary pillar (Middle)
        int destPillar = 2;  // Destination pillar (Right)

        // If the number of disks is even, swap the destination and auxiliary pillars
        if (n % 2 == 0) {
            int temp = destPillar;
            destPillar = midPillar;
            midPillar = temp;
        }

        // Perform the moves in a very janky way, I hate this it makes my eyes burn, but I have not found better solutions
        // these commented ouputs are there in case I want to have the raw output ever again
        for (int i = 1; i <= (1 << n) - 1; i++) {
            if (i % 3 == 1) {
                if (board.pillars[startPillar].Count == 0) {
                    board.moveDisc(board.pillars[destPillar], board.pillars[startPillar]);
                    // Console.WriteLine("3");
                    // Console.WriteLine("1");
                } else if (board.pillars[destPillar].Count == 0) {
                    board.moveDisc(board.pillars[startPillar], board.pillars[destPillar]);
                    // Console.WriteLine("1");
                    // Console.WriteLine("3");
                } else if (board.pillars[startPillar][board.pillars[startPillar].Count - 1].radius <= board.pillars[destPillar][board.pillars[destPillar].Count - 1].radius) {
                    board.moveDisc(board.pillars[startPillar], board.pillars[destPillar]);
                    // Console.WriteLine("1");
                    // Console.WriteLine("3");
                } else {
                    board.moveDisc(board.pillars[destPillar], board.pillars[startPillar]);
                    // Console.WriteLine("3");
                    // Console.WriteLine("1");
                }
            } else if (i % 3 == 2) {
                if (board.pillars[startPillar].Count == 0) {
                    board.moveDisc(board.pillars[midPillar], board.pillars[startPillar]);
                    // Console.WriteLine("2");
                    // Console.WriteLine("1");
                } else if (board.pillars[midPillar].Count == 0) {
                    board.moveDisc(board.pillars[startPillar], board.pillars[midPillar]);
                    // Console.WriteLine("1");
                    // Console.WriteLine("2");
                } else if (board.pillars[startPillar][board.pillars[startPillar].Count - 1].radius <= board.pillars[midPillar][board.pillars[midPillar].Count - 1].radius) {
                    board.moveDisc(board.pillars[startPillar], board.pillars[midPillar]);
                    // Console.WriteLine("1");
                    // Console.WriteLine("2");
                } else {
                    board.moveDisc(board.pillars[midPillar], board.pillars[startPillar]);
                    // Console.WriteLine("2");
                    // Console.WriteLine("1");
                }
            } else if (i % 3 == 0) {
                if (board.pillars[midPillar].Count == 0) {
                    board.moveDisc(board.pillars[destPillar], board.pillars[midPillar]);
                    // Console.WriteLine("3");
                    // Console.WriteLine("2");
                } else if (board.pillars[destPillar].Count == 0) {
                    board.moveDisc(board.pillars[midPillar], board.pillars[destPillar]);
                    // Console.WriteLine("2");
                    // Console.WriteLine("3");
                } else if (board.pillars[midPillar][board.pillars[midPillar].Count - 1].radius <= board.pillars[destPillar][board.pillars[destPillar].Count - 1].radius) {
                    board.moveDisc(board.pillars[midPillar], board.pillars[destPillar]);
                    // Console.WriteLine("2");
                    // Console.WriteLine("3");
                } else {
                    board.moveDisc(board.pillars[destPillar], board.pillars[midPillar]);
                    // Console.WriteLine("3");
                    // Console.WriteLine("2");
                }
            }
            // Thread.Sleep(100);
            board.draw();
        }
    }
}
