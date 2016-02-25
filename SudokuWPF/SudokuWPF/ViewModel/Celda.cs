using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SudokuWPF.ViewModel
{
    class Celda : INotifyPropertyChanged
    {
        string texto = string.Empty;
        int row;
        int column;
        SolidColorBrush background = Brushes.Gold;
        bool isFixed = false;
        int _number;
        int index;

        public void UpdateBackground()
        {
            if (row <= 2 && column <= 2)
                background = Brushes.LightBlue;
            if (row <= 2 && column >= 3 && column <=5)
                background = Brushes.LightGreen;
            if (row <= 2 && column >=6)
                background = Brushes.LightBlue;

            if (row >=3 && row <=5 && column <= 2)
                background = Brushes.LightGreen;
            if (row >= 3 && row <= 5 && column >= 3 && column <= 5)
                background = Brushes.LightBlue;
            if (row >= 3 && row <= 5 && column >= 6)
                background = Brushes.LightGreen;

            if (row >= 6 && column <= 2)
                background = Brushes.LightBlue;
            if (row >= 6 && column >= 3 && column <= 5)
                background = Brushes.LightGreen;
            if (row >= 6 && column >= 6)
                background = Brushes.LightBlue;
        }

        public string Texto
        {
            get
            {
                return texto;
            }

            set
            {
                texto = value;
                RaisePropertyChanged("Texto");

                _number = 0;
                int.TryParse(texto, out _number);
                RaisePropertyChanged("Number");
                RaiseNumberChanged(new NumberChangedEventArgs(_number));

            }
        }

        public int Row
        {
            get
            {
                return row;
            }

            set
            {
                row = value;
                RaisePropertyChanged("Row");
            }
        }

        public int Column
        {
            get
            {
                return column;
            }

            set
            {
                column = value;
                RaisePropertyChanged("Column");
            }
        }

        public SolidColorBrush Background
        {
            get
            {
                return background;
            }

            set
            {
                background = value;
                RaisePropertyChanged("Background");
            }
        }

        public bool IsFixed
        {
            get
            {
                return isFixed;
            }

            set
            {
                isFixed = value;
                RaisePropertyChanged("IsFixed");
            }
        }

        public int Number
        {
            get
            {
                return _number;
            }

            set
            {
                _number = value;
                RaisePropertyChanged("Number");
                RaiseNumberChanged(new NumberChangedEventArgs(_number));
            }
        }

        public int Index
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
                RaisePropertyChanged("Index");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event EventHandler<NumberChangedEventArgs> NumberChanged;
        private void RaiseNumberChanged(NumberChangedEventArgs e)
        {
            var handler = NumberChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
