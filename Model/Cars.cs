//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegistrationSystem.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cars
    {
        public int Id { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> NameCar { get; set; }
        public Nullable<double> Price { get; set; }
        public byte[] PhotoCar { get; set; }
   
        public string ModelCar { get; set; }
        public string BrandCar { get; set; }
   
        public virtual Models Models { get; set; }
        public virtual Statuses Statuses { get; set; }
    }
}