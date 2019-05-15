﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.Web.Models
{
    public class DataTableUsersModel
    {
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }

        [DataMember(Name = "data")]
        public IList<UserData> Data { get; set; }
    }

    public class UserViewModel : DataTableModel
    {
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        public int Status { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
        public UserData UserList { get; set; }
    }

    public class UserData
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        public string Uuid { get; set; }

        public string Email { get; set; }

        [Display(Name = "Rol")]
        public string RoleName { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Fecha de actualización")]
        public DateTime UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        [Display(Name = "Estatus")]
        public bool Status { get; set; }
    }

    public class UserCreateViewModel
    {
        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Confirmar contraseña")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Rol")]
        [Required]
        public int Role { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }

    public class UserRoleViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }

    public class UserEditViewModel
    {
        public string Uuid { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        public string Email { get; set; }

        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Rol")]
        public int Role { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }
    public class UserImportViewModel
    {
        [Display(Name = "Subir archivo")]
        [Required]
        public HttpPostedFileBase ImportedFile { get; set; }

        public IList<UserRowImportResultViewModel> ImportResult { get; set; }
    }
    public class UserRowImportResultViewModel
    {
        public int RowNumber { get; set; }
        public string EmployeeNumber { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public IList<string> Messages { get; set; }
    }
}