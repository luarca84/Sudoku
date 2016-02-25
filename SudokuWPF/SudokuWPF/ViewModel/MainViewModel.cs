using SudokuWPF.Generator;
using SudokuWPF.Model;
using SudokuWPF.Solver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SudokuWPF.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        List<Celda> celdas = new List<Celda>();
        int numRows = 9;
        int numColumns = 9;
        int[,] BoardInitial;
        int[,] BoardSolved;
        int[,] BoardCurrent;
        int numHoles = 40;
        bool btnResolverPulsado = false;
        List<string> lstDificultad = new List<string>();
        string selectedDificultad = DIFICULTAD_FACIL;
        const string DIFICULTAD_FACIL = "Facil";
        const string DIFICULTAD_INTERMEDIO = "Intermedio";
        const string DIFICULTAD_DIFICIL = "Dificil";

        public MainViewModel()
        {
            LstDificultad = new List<string>();
            LstDificultad.Add(DIFICULTAD_FACIL);
            LstDificultad.Add(DIFICULTAD_INTERMEDIO);
            LstDificultad.Add(DIFICULTAD_DIFICIL);

            NuevoJuego();
        }

        private void NuevoJuego()
        {
            numHoles = GetNumHolesByDificultad();

            btnResolverPulsado = false;
            SudokuGenerator sg = new SudokuGenerator();
            bool solved = false;
            do
            {
                sg.nextBoard(numHoles);
                BoardInitial = sg.Board;
                BoardSolved = (int[,])sg.Board.Clone();
                SudokuSolver ssolver = new SudokuSolver(9, BoardSolved);
                solved = ssolver.SolveSudokuMethod(BoardSolved);
            }
            while (!solved);

            celdas = new List<Celda>();
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numColumns; j++)
                {
                    Celda c = new Celda();
                    int index = i * NumRows + j;
                    int value = sg.Board[i, j];
                    if (value != 0)
                    {
                        c.Texto = value.ToString();
                        c.IsFixed = true;
                    }
                    else
                    {
                        c.Texto = "";
                        c.IsFixed = false;
                    }
                    c.Index = index;
                    c.Number = value;
                    c.Row = i;
                    c.Column = j;
                    c.NumberChanged += CellVMNumberChanged;
                    c.UpdateBackground();
                    celdas.Add(c);
                }
            }

            RaisePropertyChanged("Celdas");
        }

        private int GetNumHolesByDificultad()
        {
            switch (SelectedDificultad)
            {
                case DIFICULTAD_FACIL:
                    return 20;
                case DIFICULTAD_INTERMEDIO:
                    return 30;
                case DIFICULTAD_DIFICIL:
                    return 40;
            }
            return 40; 
        }

        private void CellVMNumberChanged(object sender, NumberChangedEventArgs e)
        {
            if (!btnResolverPulsado)
            {
                BoardCurrent = new int[9, 9];
                foreach (Celda c in celdas)
                    BoardCurrent[c.Row, c.Column] = c.Number;

                SudokuStatus sudokustatus = new SudokuStatus();
                if (sudokustatus.checkSudokuStatus(BoardCurrent))
                {
                    MessageBox.Show("Ganaste");
                }
                else if (IsCompleted())
                {
                    MessageBox.Show("La solucion no es correcta");
                }
            }

        }

        public bool IsCompleted()
        {
            Celda c = celdas.Where(e => e.Number == 0).FirstOrDefault();
            if (c != null)
                return false;
            return true;
        }

        public List<Celda> Celdas
        {
            get
            {
                return celdas;
            }

            set
            {
                celdas = value;
                RaisePropertyChanged("Celdas");
            }
        }

        public int NumRows
        {
            get
            {
                return numRows;
            }

            set
            {
                numRows = value;
                RaisePropertyChanged("NumRows");
            }
        }

        public int NumColumns
        {
            get
            {
                return numColumns;
            }

            set
            {
                numColumns = value;
                RaisePropertyChanged("NumColumns");
            }
        }

        public string SelectedDificultad
        {
            get
            {
                return selectedDificultad;
            }

            set
            {
                selectedDificultad = value;
                RaisePropertyChanged("SelectedDificultad");
            }
        }

        public List<string> LstDificultad
        {
            get
            {
                return lstDificultad;
            }

            set
            {
                lstDificultad = value;
                RaisePropertyChanged("LstDificultad");
            }
        }


        #region ClickNuevoJuegoCommand

        private ICommand _clickNuevoJuegoCommand;
        public ICommand ClickNuevoJuegoCommand
        {
            get
            {
                return _clickNuevoJuegoCommand ?? (_clickNuevoJuegoCommand = new CommandHandler(() => MyActionNuevoJuego(), CanExecuteActionNuevoJuego()));
            }
        }

        private bool CanExecuteActionNuevoJuego()
        {
            return true;
        }

        public void MyActionNuevoJuego()
        {
            NuevoJuego();
        }
        #endregion

        #region ClickPistaCommand

        private ICommand _clickPistaCommand;
        public ICommand ClickPistaCommand
        {
            get
            {
                return _clickPistaCommand ?? (_clickPistaCommand = new CommandHandler(() => MyActionPista(), CanExecuteActionPista()));
            }
        }

        private bool CanExecuteActionPista()
        {
            return true;
        }

        public void MyActionPista()
        {
            Celda c = celdas.Where(e => e.Number == 0).FirstOrDefault();
            if (c != null)
            {
                c.Texto = BoardSolved[c.Row, c.Column].ToString();
                c.Number = BoardSolved[c.Row, c.Column];
            }
        }
        #endregion


        #region ClickResolverCommand

        private ICommand _clickResolverCommand;
        public ICommand ClickResolverCommand
        {
            get
            {
                return _clickResolverCommand ?? (_clickResolverCommand = new CommandHandler(() => MyActionResolver(), CanExecuteActionResolver()));
            }
        }

        

        private bool CanExecuteActionResolver()
        {
            return true;
        }

        public void MyActionResolver()
        {
            btnResolverPulsado = true;
            foreach (Celda c in celdas)
            {
                c.Texto = BoardSolved[c.Row, c.Column].ToString();
                c.Number = BoardSolved[c.Row, c.Column];
            }
        }
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }


    public class CommandHandler : ICommand
    {
        private Action _action;
        private bool _canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
