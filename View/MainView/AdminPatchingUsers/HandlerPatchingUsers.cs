using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using RegistrationSystem.Core;
using RegistrationSystem.Model;

namespace RegistrationSystem.View.MainView.AdminPatchingUsers
{

    class HandlerPatchingUsers : ObservableObjects
    {
        public List<Users> users { get; set; }

        public ICollectionView EmployeesCollectionView { get; }

        public HandlerPatchingUsers()
        {
            users = new List<Users>();
            users.Add(new Users() { Name = "Наруто", LastName = "Фамилия1" });
            users.Add(new Users() { Name = "Имя2", LastName = "Фамилия2" });
            users.Add(new Users() { Name = "Имя3", LastName = "Фамилия3" });
            users.Add(new Users() { Name = "Имя4", LastName = "Фамилия4" });
            users.Add(new Users() { Name = "Саске", LastName = "Фамилия5" });

            EmployeesCollectionView = CollectionViewSource.GetDefaultView(users);

            EmployeesCollectionView.Filter = FilterEmployees;
            EmployeesCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Users.Name)));
            EmployeesCollectionView.SortDescriptions.Add(new SortDescription(nameof(Users.LastName), ListSortDirection.Ascending));
        }

        private string _employeesFilter = string.Empty;
        public string EmployeesFilter
        {
            get
            {
                return _employeesFilter;
            }
            set
            {
                _employeesFilter = value;
                OnPropertyChanged(nameof(EmployeesFilter));
                EmployeesCollectionView.Refresh();
            }
        }

        private bool FilterEmployees(object obj)
        {
            if (obj is Users employeeViewModel)
            {
                return employeeViewModel.Name.Contains(EmployeesFilter) ||
                    employeeViewModel.LastName.Contains(EmployeesFilter);
            }

            return false;
        }

        private Users _NodeCategoryUser { get; set; }
        public Users NodeCategoryUser
        {
            get => _NodeCategoryUser;
            set
            {
                _NodeCategoryUser = value;
                OnPropertyChanged("NodeCategoryUser");
            }
        }



        private string _Name { get; set; }
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _LastName { get; set; }

        public string LastName
        {
            get { return _LastName; }
            set
            {
                _LastName = value;
                OnPropertyChanged("LastName");
            }
        }
    }
}
