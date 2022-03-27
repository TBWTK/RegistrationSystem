using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using RegistrationSystem.Core;
using RegistrationSystem.Model;

namespace RegistrationSystem.View.MainView.AdminPatchingUsers
{

    class HandlerPatchingUsers : ObservableObjects
    {
        public RelayCommand DownloadImage { get; set; }
        public RelayCommand RegisterUpdate { get; set; }
        public RelayCommand CreateUser { get; set; }



        private string _employeesFilter = string.Empty;
        public List<Users> users { get; set; }
        public List<Genders> genders { get; set; }
        public List<Statuses> statuses { get; set; }
        public List<Roles> roles { get; set; }


        public ICollectionView EmployeesCollectionView { get;}

        public HandlerPatchingUsers()
        {
            using (var context = new TestDataBaseEntities())
            {
                users = context.Users.ToList();
                genders = context.Genders.ToList();
                statuses = context.Statuses.ToList();
                roles = context.Roles.ToList();
            }


            EmployeesCollectionView = CollectionViewSource.GetDefaultView(users);
            EmployeesCollectionView.Filter = FilterEmployees;
            EmployeesCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Users.Login)));
            EmployeesCollectionView.SortDescriptions.Add(new SortDescription(nameof(Users.Login), ListSortDirection.Ascending));

            RegisterUpdate = new RelayCommand(o =>
            {
                if(NodeCategoryUser != null)
                {
                    using (var context = new TestDataBaseEntities())
                    {
                        var us = context.Users.SingleOrDefault(x => x.Id == NodeCategoryUser.Id);
                        if (us != null)
                        {
                            us.Name = NodeCategoryUser.Name;
                            us.SurName = NodeCategoryUser.SurName;
                            us.LastName = NodeCategoryUser.LastName;
                            us.Password = NodeCategoryUser.Password;
                            if (NodeCategoryRoles != null)
                                us.Role = NodeCategoryRoles.Id;
                            if (NodeCategoryGender != null)
                                us.Gender = NodeCategoryGender.Id;
                            if (NodeCategoryStatuses != null)
                                us.Status = NodeCategoryStatuses.Id;
                            context.Entry(us).State = EntityState.Modified;
                            context.SaveChanges();
                            //  System.Windows.MessageBox.Show("Изменения прошли успешно");
                        }
                        users = context.Users.ToList();
                        //EmployeesCollectionView = CollectionViewSource.GetDefaultView(users);
                    }
                }

            });

            CreateUser = new RelayCommand(o =>
            {
                if (NodeCategoryUser != null)
                {
                    using (var context = new TestDataBaseEntities())
                    {
                        //if (NodeCategoryRoles != null)
                        //    Role = NodeCategoryRoles.Id;
                        //if (NodeCategoryGender != null)
                        //    us.Gender = NodeCategoryGender.Id;
                        //if (NodeCategoryStatuses != null)
                        //    us.Status = NodeCategoryStatuses.Id;
                        //Role != null ? Roles.NameRole : "";
                        var us = new Users()
                        {
                            Name = NodeCategoryUser.Name,
                            SurName = NodeCategoryUser.SurName,
                            LastName = NodeCategoryUser.LastName,
                            Login = NodeCategoryUser.Login,
                            Password = NodeCategoryUser.Password,
                            Role = NodeCategoryRoles?.Id,
                            Gender = NodeCategoryGender?.Id,
                            Status = NodeCategoryStatuses?.Id

                        };
                        context.Users.Add(us);
                        context.SaveChanges();
                    }
                }

            });

        }

        public string EmployeesFilter
        {
            get
            {
                return _employeesFilter;
            }
            set
            {
                _employeesFilter = value;
                OnPropertyChanged("EmployeesFilter");
                EmployeesCollectionView.Refresh();
            }
        }

        private bool FilterEmployees(object obj)
        {
            if (obj is Users employeeViewModel)
            {
                if (employeeViewModel.Login == null)
                    return false;
                if (employeeViewModel.Name == null || employeeViewModel.LastName == null || employeeViewModel.SurName == null || employeeViewModel.Role == null || employeeViewModel.Gender == null || employeeViewModel.Status == null)
                    return employeeViewModel.Login.Contains(EmployeesFilter);
                else
                    return employeeViewModel.Login.Contains(EmployeesFilter) || employeeViewModel.Name.Contains(EmployeesFilter) ||
                           employeeViewModel.LastName.Contains(EmployeesFilter) || employeeViewModel.SurName.Contains(EmployeesFilter) || 
                           employeeViewModel.Statuses.NameStatus.Contains(EmployeesFilter) || employeeViewModel.Roles.NameRole.Contains(EmployeesFilter) || employeeViewModel.Genders.NameGender.Contains(EmployeesFilter);
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
                if(NodeCategoryUser != null)
                {
                    if (NodeCategoryUser.Gender != null)
                        UserGender = NodeCategoryUser.Genders.NameGender;
                    else
                        UserGender = "";

                    if (NodeCategoryUser.Role != null)
                        UserRole = NodeCategoryUser.Roles.NameRole;
                    else
                        UserRole = "";

                    if (NodeCategoryUser.Status != null)
                        UserStatus = NodeCategoryUser.Statuses.NameStatus;
                    else
                        UserStatus = "";


                    OnPropertyChanged("NodeCategoryUser");
                }

            }
        }

        private Genders _NodeCategoryGender { get; set; }
        public Genders NodeCategoryGender
        {
            get => _NodeCategoryGender;
            set
            {
                _NodeCategoryGender = value;
                NodeCategoryUser.Gender = NodeCategoryGender.Id;
                UserGender = NodeCategoryGender.NameGender;
                OnPropertyChanged("NodeCategoryGender");
            }
        }

        private Roles _NodeCategoryRoles { get; set; }
        public Roles NodeCategoryRoles
        {
            get => _NodeCategoryRoles;
            set
            {
                _NodeCategoryRoles = value;
                UserRole = _NodeCategoryRoles.NameRole;
                NodeCategoryUser.Role = NodeCategoryRoles.Id;

                OnPropertyChanged("NodeCategoryRoles");
            }
        }

        private Statuses _NodeCategoryStatuses { get; set; }
        public Statuses NodeCategoryStatuses
        {
            get => _NodeCategoryStatuses;
            set
            {
                _NodeCategoryStatuses = value;
                NodeCategoryUser.Status = NodeCategoryStatuses.Id;
                UserStatus = _NodeCategoryStatuses.NameStatus;
                OnPropertyChanged("NodeCategoryStatuses");
            }
        }

        private string _UserGender { get; set; }
        public string UserGender
        {
            get
            {
                return _UserGender;
            }
            set
            {
                _UserGender = value;
                OnPropertyChanged("UserGender");
            }
        }
        private string _UserRole { get; set; }
        public string UserRole
        {
            get
            {
                return _UserRole;
            }
            set
            {
                _UserRole = value;
                OnPropertyChanged("UserRole");
            }
        }
        private string _UserStatus { get; set; }
        public string UserStatus
        {
            get
            {
                return _UserStatus;
            }
            set
            {
                _UserStatus = value;
                OnPropertyChanged("UserStatus");
            }
        }


        public Image byteArrayToImage(byte[] byteArray)
        {
            Image image = new Image();
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                image.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
            return image;
        }
        public byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
    }
}
