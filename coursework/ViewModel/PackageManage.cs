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
        private List<Interface> allInterfaces = PackageWorker.GetAllInterfaces();
        public List<Interface> AllInterfaces
        {
            get { return allInterfaces; }
            set
            {
                allInterfaces = value;
                NotifyPropertyChanged("AllInterfaces");
            }
        }

        private string loginLabelContent = DataWorker.SetLabelContent();
        public string LoginLabelContent
        {
            get { return loginLabelContent; }
            set
            {
                loginLabelContent = value;
                NotifyPropertyChanged("LoginLabelContent");
            }
        }
        private string loginOrRegisterButtonContent = DataWorker.SetLoginOrRegisterButtonContent();
        public string LoginOrRegisterButtonContent
        {
            get { return loginOrRegisterButtonContent; }
            set
            {
                LoginOrRegisterButtonContent = value;
                NotifyPropertyChanged("LoginOrRegisterButtonContent");
            }
        }

        private string passwordBoxContent;
        public string PasswordBoxContent
        {
            get { return passwordBoxContent; }
            set
            {
                passwordBoxContent = value;
                NotifyPropertyChanged("PasswordBoxContent");
            }
        }


        private RelayCommand registerOrLoginCommand;
        public RelayCommand RegisterOrLoginCommand
        {
            get
            {
                return registerOrLoginCommand ?? new RelayCommand(obj =>
                {
                    
                    List<User> usr = DataWorker.GetAllUsers();
                    Window wnd = obj as Window;
                    if (usr.Count() == 0)
                    {
                        DataWorker.CreatUser(PasswordBoxContent);
                       
                        OpenMainWindowMethod();
                        wnd.Close();

                    }
                    else
                    {
                        foreach (User u in usr)
                        {
                            //passwordBoxContent = "";
                            if (u.Password.ToString() == DataWorker.GetStringSha256Hash(passwordBoxContent))
                            {
                                OpenMainWindowMethod();
                            }
                            else
                            {
                                MessageBox.Show("Введён неверный пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                passwordBoxContent = null;
                            }
                        }
                    }
                }
                );
            }
        }
        public Interface CurrentInterface { get; set; }
        private static Interface SelectedInterface { get; set; }

        private RelayCommand setCurrentInterface;
        public RelayCommand SetCurrentInterface
        {
            get
            {
                return setCurrentInterface ?? new RelayCommand(obj =>
                {
                    SelectedInterface = CurrentInterface;
                    Window wnd = obj as Window;
                    wnd.Close();
                    
                }
                );
            }
        }


        #region COMMANDS
        //Команды открытия окон 
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
        private RelayCommand startCollectingPackageInfo;
        public RelayCommand StartCollectingPackageInfo
        {
            get
            {
                return startCollectingPackageInfo ?? new RelayCommand(obj =>
                {
                    StartCollectingPackageInfoMethod();
                }
                );
            }
        }
        private RelayCommand closeInterfaceSelectionWindow;
        public RelayCommand CloseInterfaceSelectionWindow
        {
            get
            {
                return closeInterfaceSelectionWindow ?? new RelayCommand(obj =>
                {
                    CloseInterfaceSeletionWindowMethod();
                }
                );
            }
        }
        #endregion

        #region METHODS
        // Метод открытия окон
        private void OpenInterfaceSelectionWindowMethod()
        {
            InterfaceSelectionWindow newInterfaceSelectionWindow = new InterfaceSelectionWindow();
            SetCenterPositionAndOpen(newInterfaceSelectionWindow);
        }
        private void OpenMainWindowMethod()
        {
            MainWindow newMainWindow = new MainWindow();
            SetCenterPositionAndOpen(newMainWindow);
        }
        private void CloseInterfaceSeletionWindowMethod()
        {
            InterfaceSelectionWindow newInterfaceSelectionWindow = new InterfaceSelectionWindow();
            CloseWindow(newInterfaceSelectionWindow);
        }
        public void StartCollectingPackageInfoMethod()
        {
            List<Package> res = new List<Package>();
            res = PackageWorker.GetSomePackageInfo(SelectedInterface);
            foreach (var pack in res)
                DataWorker.UploadPackage(pack.IpAddress,pack.Protocol,pack.SourcePort,pack.DestinationPort, pack.SourceHardwareAddress,DateTime.Now,pack.DestinationHardwareAddress,pack.PayLoadData);
            CloseInterfaceSeletionWindowMethod();
            UpdateAllPackages();
        }
        private void CloseWindow(Window window)
        {
            window.Close();
        }
        private void SetCenterPositionAndOpen(Window window)
        {
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Show();
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void UpdateAllPackages()
        {
            AllPackages = DataWorker.GetAllPackages();
            MainWindow.AllPackages.ItemsSource = null;
            MainWindow.AllPackages.Items.Clear();
            MainWindow.AllPackages.ItemsSource = AllPackages;
            MainWindow.AllPackages.Items.Refresh();
        }
    }
}
