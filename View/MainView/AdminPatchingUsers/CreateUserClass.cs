using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegistrationSystem.Model;
using RegistrationSystem.Core;
using System.Windows;

namespace RegistrationSystem.View.MainView.AdminPatchingUsers
{
    class CreateUserClass : ObservableObjects
    {
        public RelayCommand CreateUser { get; set; }


        public List<Users> users { get; set; }
        public List<Genders> genders { get; set; }
        public List<Statuses> statuses { get; set; }
        public List<Roles> roles { get; set; }


        public CreateUserClass()
        {
            NodeCategoryUser = new Users();
            using (var context = new TestDataBaseEntities())
            {
                users = context.Users.ToList();
                genders = context.Genders.ToList();
                statuses = context.Statuses.ToList();
                roles = context.Roles.ToList();
            }

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
                        MessageBox.Show("Пользователь зарегестрирован");
                        context.Users.Add(us);
                        context.SaveChanges();
                    }
                }
            });

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
        private Genders _NodeCategoryGender { get; set; }
        public Genders NodeCategoryGender
        {
            get => _NodeCategoryGender;
            set
            {
                _NodeCategoryGender = value;
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
                OnPropertyChanged("NodeCategoryStatuses");
            }
        }
    }
}
