// By Y2kun
// Link to my Github: https://github.com/Y2kun
// An Overdesigned Tui for the Tower of Hanoi

namespace towerOfHanoi;
class Program {
    public const bool ENABLE_ASCII = false; // Try flipping this, It has color.

    static void Solver(int n, Board board, List<Disc> startPillar, List<Disc> midPillar, List<Disc> destPillar) {
        // Recursive Brute Force Algorithm for reaching the desired Disc Configuration
        if (n <= 0) return; // Leave current Functioncall as it has reached the end
        if (board.win()) return; // Leave Recursion when won
        Solver(n - 1, board, startPillar, destPillar, midPillar);
        // board.draw();
        board.moveDisc(startPillar, destPillar); // Move Disc
        Solver(n - 1, board, midPillar, destPillar, startPillar);
    }

    static void Main(string[] args) {
        // Unicode Box for Introduction
        Console.WriteLine(" ┏━━━━━━━━━━Tower of Hanoi━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
        Console.WriteLine(" ┃ Rules:                                                       ┃");
        Console.WriteLine(" ┃       1. Only one disc can be moved at a time.               ┃");
        Console.WriteLine(" ┃       2. No disc may placed on top of a smaller disc.        ┃");
        Console.WriteLine(" ┃                                                              ┃");
        Console.WriteLine(" ┃ Controles:                                                   ┃");
        Console.WriteLine(" ┃       Input 1 to select the Left Pillar                      ┃");
        Console.WriteLine(" ┃       Input 2 to select the Middle Pillar                    ┃");
        Console.WriteLine(" ┃       Input 3 to select the Right Pillar                     ┃");
        Console.WriteLine(" ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
        Console.WriteLine("\n Please Input with how many Discs you want to play.");
        // Reading User Input, and making sure it's usable
        int discCount = 3;
        try {
            discCount = Convert.ToInt32(Console.ReadLine());
        } catch {
            Console.WriteLine("Your Input was Invalid, it defaulted to 3");
        }
        
        Board board = new Board(discCount, ENABLE_ASCII);
        Console.WriteLine("\n Please Input to play ToH or to let an AI solve it for you ( p | s )");
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
        } else {
            // This is the recursive version
            board.draw();
            Solver(2 * (discCount - 1) + discCount, board, board.pillars[0], board.pillars[1], board.pillars[2]);
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
}
