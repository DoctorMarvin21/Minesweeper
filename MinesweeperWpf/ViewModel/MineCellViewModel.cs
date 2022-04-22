using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MinesweeperWpf
{
    public class MineCellViewModel : INotifyPropertyChanged
    {
        private bool hasMine;
        private bool isMarked;
        private bool isOpened;
        private int minesAround;
        private string text = " ";

        public MineCellViewModel(MainViewModel owner)
        {
            Owner = owner;
            OpenCommand = new Command(Open, () => !IsMarked && Owner.State == GameState.InProgress);
            MarkCommand = new Command(Mark, () => !IsOpened && Owner.State == GameState.InProgress);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel Owner { get; }

        public ICommand OpenCommand { get; }

        public ICommand MarkCommand { get; }

        public bool HasMine
        {
            get => hasMine;
            set
            {
                hasMine = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsEmpty));
                UpdateText();
            }
        }

        public bool IsMarked
        {
            get => isMarked;
            set
            {
                isMarked = value;
                OnPropertyChanged();
                UpdateText();
            }
        }

        public bool IsOpened
        {
            get => isOpened;
            set
            {
                isOpened = value;
                OnPropertyChanged();
                UpdateText();
            }
        }

        public int MinesAround
        {
            get => minesAround;
            set
            {
                minesAround = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsEmpty));
                UpdateText();
            }
        }

        public string Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }

        public bool IsEmpty => !HasMine && MinesAround == 0;

        public void UpdateText()
        {
            if (IsOpened)
            {
                if (HasMine)
                {
                    Text = "💣";
                }
                else if (MinesAround == 0)
                {
                    Text = " ";
                }
                else
                {
                    Text = MinesAround.ToString();
                }
            }
            else
            {
                if (IsMarked)
                {
                    Text = "🚩";
                }
                else
                {
                    Text = " ";
                }
            }
        }

        private void Open()
        {
            IsOpened = true;

            if (IsEmpty)
            {
                MineCellHelper.TryOpenEmptyFields(Owner.MineCells, this, Owner.ColumnsCount);
            }

            if (HasMine)
            {
                Owner.State = GameState.Failed;
            }
            else
            {
                if (MineCellHelper.FieldIsOpen(Owner.MineCells))
                {
                    Owner.State = GameState.Success;
                }
            }
        }

        private void Mark()
        {
            if (IsMarked)
            {
                IsMarked = false;
                Owner.MinesCount++;
            }
            else
            {
                IsMarked = true;
                Owner.MinesCount--;

                if (MineCellHelper.FieldIsOpen(Owner.MineCells))
                {
                    Owner.State = GameState.Success;
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
