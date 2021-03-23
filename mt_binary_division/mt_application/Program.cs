using System;
using System.IO;
using System.Collections.Generic;

namespace mt_application {
    class MT {
        private int current_pos = 5;
        private string current_state = "q0";
        private HashSet<char> sigma;
        private List<char> tape;
        private List<string> final_states;
        Dictionary<string, Dictionary<char, Tuple<string, char, char>>> delta;

        public MT(int n_states, string a, string b, List<Tuple<string, char, string, char, char>> d_values, List<string> final_states) {
            this.sigma = new HashSet<char>();
            this.sigma.Add('0');
            this.sigma.Add('1');
            this.sigma.Add(' ');

            this.tape = new List<char>();
            for (int i = 0; i < 5; i++) {
                this.tape.Add(' ');
            }
            foreach (char bit in a) {
                this.tape.Add(bit);
            }
            this.tape.Add(' ');
            foreach (char bit in b) {
                this.tape.Add(bit);
            }
            this.tape.Add(' ');

            this.delta = new Dictionary<string, Dictionary<char, Tuple<string, char, char>>>();
            for (int i = 0; i < n_states; i++) {
                string state = String.Concat("q", i);
                Dictionary<char, Tuple<string, char, char>> edge = new Dictionary<char, Tuple<string, char, char>>();
                this.delta.Add(state, edge);
            }
            foreach (Tuple<string, char, string, char, char> d_value in d_values) {
                Tuple<string, char, char> change = new Tuple<string, char, char>(d_value.Item3, d_value.Item4, d_value.Item5);
                this.delta[d_value.Item1][d_value.Item2] = change;
            }

            this.final_states = final_states;
        }

        private void print_result() {
            string res = "";
            foreach (char a in this.tape) {
                res += a;
            }
            string[] values = res.Trim().Split(' ');
            Console.WriteLine("Cociente: " + values[0]);
            Console.WriteLine("Residuo: " + values[1]);
        }

        public void start() {
            this.transition();
        }

        private void transition() {
            foreach (string state in this.final_states) {
                if (state.Equals(this.current_state)) {
                    this.print_result();
                    return;
                }
            }
            char character = this.tape[this.current_pos];
            Tuple<string, char, char> action = this.delta[this.current_state][character];
            this.current_state = action.Item1;
            this.tape[this.current_pos] = action.Item2;
            if (action.Item3.Equals('L')) {
                this.current_pos--;
            } else if (action.Item3.Equals('R')) {
                this.current_pos++;
            }
            transition();
        }
    }

    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Turing Machine for Binary Division of 3-Digit Numbers");
            Console.WriteLine("Ingrese el valor de a:");
            string a = Console.ReadLine();
            Console.WriteLine("Ingrese el valor de b:");
            string b = Console.ReadLine();
            bool one = false;
            foreach (char bit in b) {
                if (bit.Equals('1')) {
                    one = true;
                }
            }
            if (!one) {
                Console.WriteLine("No se permite la división por cero.");
                System.Environment.Exit(1);
            }
            string[] lines = File.ReadAllLines("../../../mt_description.txt");
            string[] n_s = lines[0].Split(' ');
            int n_states = Int32.Parse(n_s[0]);
            List<string> final_states = new List<string>();
            foreach (string fs in lines[1].Split(' ')) {
                final_states.Add(fs);
            }
            List<Tuple<string, char, string, char, char>> d_values = new List<Tuple<string, char, string, char, char>>();
            for (int i = 2; i < lines.Length; i++) {
                string[] info = lines[i].Split(' ');
                string from = info[0];
                char c1 = info[1].ToCharArray()[0];
                if (c1.Equals('#')) {
                    c1 = ' ';
                }
                string to = info[2];
                char c2 = info[3].ToCharArray()[0];
                if (c2.Equals('#')) {
                    c2 = ' ';
                }
                char move = info[4].ToCharArray()[0];
                Tuple<string, char, string, char, char> entry = new Tuple<string, char, string, char, char>(from, c1, to, c2, move);
                d_values.Add(entry);
            }
            MT mt = new MT(n_states, a, b, d_values, final_states);
            mt.start();
            Console.WriteLine("\nPress any key to close this window...");
            Console.ReadKey();
        }
    }
}
