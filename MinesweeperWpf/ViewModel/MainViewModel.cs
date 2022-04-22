using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Input;

namespace MinesweeperWpf
{
    public enum GameMode
    {
        Easy,
        Medium,
        Hard
    }

    public enum GameState
    {
        Success,
        InProgress,
        Failed
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly bool[] modeArray = new bool[] { true, false, false };
        private readonly Random random = new Random(Environment.TickCount);
        private readonly Timer timer = new Timer(1000);

        private int columnsCount;
        private int rowsCount;
        private int minesCount;
        private GameState state;
        private int secondsPassed;

        public MainViewModel()
        {
            NewGameCommand = new Command(NewGame);
            timer.Elapsed += (s, e) => SecondsPassed++;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand NewGameCommand { get; }

        public ObservableCollection<MineCellViewModel> MineCells { get; } = new ObservableCollection<MineCellViewModel>();

        public int ColumnsCount
        {
            get => columnsCount;
            set
            {
                columnsCount = value;
                OnPropertyChanged();
            }
        }

        public int RowsCount
        {
            get => rowsCount;
            set
            {
                rowsCount = value;
                OnPropertyChanged();
            }
        }

        public int MinesCount
        {
            get => minesCount;
            set
            {
                minesCount = value;
                OnPropertyChanged();
            }
        }

        public int SecondsPassed
        {
            get => secondsPassed;
            set
            {
                secondsPassed = value;
                OnPropertyChanged();
            }
        }

        public GameState State
        {
            get => state;
            set
            {
                state = value;
                OnPropertyChanged();

                if (state != GameState.InProgress)
                {
                    timer.Stop();

                    foreach (MineCellViewModel cell in MineCells)
                    {
                        if (state == GameState.Success)
                        {
                            if (!cell.HasMine)
                            {
                                cell.IsOpened = true;
                            }
                        }
                        else
                        {
                            cell.IsOpened = true;
                        }
                    }
                }
            }
        }

        public bool[] ModeArray => modeArray;

        public GameMode Mode => (GameMode)Array.IndexOf(modeArray, true);

        private void NewGame()
        {
            timer.Stop();
            SecondsPassed = 0;
            State = GameState.InProgress;
            MineCells.Clear();

            switch (Mode)
            {
                case GameMode.Easy:
                    {
                        RowsCount = 9;
                        ColumnsCount = 9;
                        MinesCount = 10;

                        break;
                    }
                case GameMode.Medium:
                    {
                        RowsCount = 16;
                        ColumnsCount = 16;
                        MinesCount = 40;

                        break;
                    }
                case GameMode.Hard:
                    {
                        RowsCount = 16;
                        ColumnsCount = 30;
                        MinesCount = 99;

                        break;
                    }
            }

            for (int i = 0; i < RowsCount * ColumnsCount; i++)
            {
                MineCells.Add(new MineCellViewModel(this));
            }

            PlaceMines();
            timer.Start();
        }

        private void PlaceMines()
        {
            int mines = MinesCount;

            while (mines != 0)
            {
                int index = random.Next(0, MineCells.Count);
                var cell = MineCells[index];

                if (!cell.HasMine)
                {
                    cell.HasMine = true;
                    mines--;
                }
            }

            foreach (var cell in MineCells)
            {
                MineCellHelper.UpdateMinesAround(MineCells, cell, ColumnsCount);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
