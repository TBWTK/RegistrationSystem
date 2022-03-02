using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RegistrationSystem.Core
{
    class ObservableObjects : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
