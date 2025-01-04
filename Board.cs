namespace towerOfHanoi {
    public class Board {
        public List<Disc>[] pillars;
        public int discCount;
        public bool enableAscii;

        public Board(int _discCount, bool _enableAscii) {
            pillars = new List<Disc>[] {new List<Disc>(), new List<Disc>(), new List<Disc>()};
            discCount = _discCount;
            enableAscii = _enableAscii;
            // Fills the First Pillar with Discs
            for (int i = discCount; i > 0; i--) {
                pillars[0].Add(new Disc(i));
            }
        }

        public void moveDisc(List<Disc> startPillar, List<Disc> destPillar) {
            if (startPillar.Count == 0) return;
            Disc discToMove = startPillar[startPillar.Count - 1];

            if (destPillar.Count == 0 || destPillar[destPillar.Count - 1].radius >= discToMove.radius) {
                startPillar.RemoveAt(startPillar.Count - 1);
                destPillar.Add(discToMove);
            } else {
                Console.WriteLine("Invalid move!");
            }
        }

        public bool win() {
            return pillars[2].Count == discCount;
        }

        public void draw() {
            string output = "";
            if (enableAscii) {
                output += "\n";
            } else {
                output += "┏━";
                output += stringRepeat("━━", discCount);
                output += "━┳━";
                output += stringRepeat("━━", discCount);
                output += "━┳━";
                output += stringRepeat("━━", discCount);
                output += "━┓\n";
            }

            for (int i = discCount - 1; i >= 0; i--) {
                output += enableAscii ? "| " : "┃ ";

                foreach(List<Disc> pillar in pillars) {
                    if (i < pillar.Count) {
                        output += stringRepeat(" ", discCount - pillar[i].radius);
                        if (enableAscii) {
                            output += $"\x1B[38;5;{pillar[i].radius}m"; // Colors the Disc according to its radius
                            output += stringRepeat("#", 2 * pillar[i].radius - 1);
                            output += "\x1B[0m";
                        } else {
                            output += stringRepeat("⬜", pillar[i].radius);
                        }
                        output += stringRepeat(" ", discCount - pillar[i].radius);
                    } else {
                        if (enableAscii) {
                            output += stringRepeat(" ", 2 * discCount - 1);
                        } else {
                            output += stringRepeat("  ", discCount);
                        }
                    }
                    // Space between pillars
                    output += enableAscii ? " | " : " ┃ ";
                }
                // the next row
                output += "\n";
            }
            if (!enableAscii) {
                output += "┗━";
                output += stringRepeat("━━", discCount);
                output += "━┻━";
                output += stringRepeat("━━", discCount);
                output += "━┻━";
                output += stringRepeat("━━", discCount);
                output += "━┛";
            }

            Console.WriteLine(output);
        }

        public string stringRepeat(string filling, int count) {
            return string.Concat(Enumerable.Repeat(filling, count));
        }
    }
}
