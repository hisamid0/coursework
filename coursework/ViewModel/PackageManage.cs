using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseWork.Model;
using CourseWork.View;
using System.ComponentModel;
using System.Windows;
using ManageStaffDBApp.Model;

namespace CourseWork.ViewModel
{
    class PackageManage : INotifyPropertyChanged
    {
        //получить данные администратора
        private List<User> allUsers = DataWorker.GetAllUsers();
        public List<User> AllUsers
        {
            get { return allUsers; }
            set
            {
                allUsers = value;
                NotifyPropertyChanged("AllUsers");
            }
        }
        private List<Package> allPackages = DataWorker.GetAllPackages();
        public List<Package> AllPackages
        {
            get { return allPackages; }
            set
            {
                allPackages = value;
                NotifyPropertyChanged("AllPackages");
            }
        }

        //Команды открытия окон 
        // TO DO ПОНЯТЬ ПОЧЕМУ НЕ МОГУ ИСПОЛЬЗОВАТЬ RELAYCOMMAND ТУТА
        private RelayCommand openInterfaceSelectionWindow;
        public RelayCommand OpenInterfaceSelectionWindow
        {
            get
            {
                return openInterfaceSelectionWindow ?? new RelayCommand(obj =>
                {
                    OpenInterfaceSelectionWindowMethod();
                }
                );
            }
        }
        
        // Метод открытия окон
        private void OpenInterfaceSelectionWindowMethod()
        {
            InterfaceSelectionWindow newInterfaceSelectionWindow = new InterfaceSelectionWindow();
            SetCenterPositionAndOpen(newInterfaceSelectionWindow);
        }
        private void SetCenterPositionAndOpen(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
