using System;
using System.Collections.Generic;

namespace pizzashop_Repository.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public int Roleid { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string Username { get; set; } = null!;

    public string? Profileimage { get; set; }

    public int? Countryid { get; set; }

    public int? Stateid { get; set; }

    public int? Cityid { get; set; }

    public string? Zipcode { get; set; }

    public string? Address { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int? Modifiedby { get; set; }

    public bool? Isactive { get; set; }

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public string? ResetPasswordToken { get; set; }

    public virtual ICollection<City> CityCreatedbyNavigations { get; set; } = new List<City>();

    public virtual ICollection<City> CityModifiedbyNavigations { get; set; } = new List<City>();

    public virtual ICollection<Country> CountryCreatedbyNavigations { get; set; } = new List<Country>();

    public virtual ICollection<Country> CountryModifiedbyNavigations { get; set; } = new List<Country>();

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual ICollection<Customer> CustomerCreatedbyNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<Customer> CustomerModifiedbyNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<Feedback> FeedbackCreatedbyNavigations { get; set; } = new List<Feedback>();

    public virtual ICollection<Feedback> FeedbackModifiedbyNavigations { get; set; } = new List<Feedback>();

    public virtual ICollection<User> InverseCreatedbyNavigation { get; set; } = new List<User>();

    public virtual ICollection<User> InverseModifiedbyNavigation { get; set; } = new List<User>();

    public virtual ICollection<Invoice> InvoiceCreatedbyNavigations { get; set; } = new List<Invoice>();

    public virtual ICollection<Invoice> InvoiceModifiedbyNavigations { get; set; } = new List<Invoice>();

    public virtual ICollection<Mappingmenuitemwithmodifier> MappingmenuitemwithmodifierCreatedbyNavigations { get; set; } = new List<Mappingmenuitemwithmodifier>();

    public virtual ICollection<Mappingmenuitemwithmodifier> MappingmenuitemwithmodifierModifiedbyNavigations { get; set; } = new List<Mappingmenuitemwithmodifier>();

    public virtual ICollection<Menucategory> MenucategoryCreatedbyNavigations { get; set; } = new List<Menucategory>();

    public virtual ICollection<Menucategory> MenucategoryModifiedbyNavigations { get; set; } = new List<Menucategory>();

    public virtual ICollection<Menuitem> MenuitemCreatedbyNavigations { get; set; } = new List<Menuitem>();

    public virtual ICollection<Menuitem> MenuitemModifiedbyNavigations { get; set; } = new List<Menuitem>();

    public virtual User? ModifiedbyNavigation { get; set; }

    public virtual ICollection<Modifier> ModifierCreatedbyNavigations { get; set; } = new List<Modifier>();

    public virtual ICollection<Modifier> ModifierModifiedbyNavigations { get; set; } = new List<Modifier>();

    public virtual ICollection<Modifiergroup> ModifiergroupCreatedbyNavigations { get; set; } = new List<Modifiergroup>();

    public virtual ICollection<Modifiergroup> ModifiergroupModifiedbyNavigations { get; set; } = new List<Modifiergroup>();

    public virtual ICollection<Modifiergroupmodifier> ModifiergroupmodifierCreatedbyNavigations { get; set; } = new List<Modifiergroupmodifier>();

    public virtual ICollection<Modifiergroupmodifier> ModifiergroupmodifierModifiedbyNavigations { get; set; } = new List<Modifiergroupmodifier>();

    public virtual ICollection<Order> OrderCreatedbyNavigations { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderModifiedbyNavigations { get; set; } = new List<Order>();

    public virtual ICollection<Ordereditem> OrdereditemCreatedbyNavigations { get; set; } = new List<Ordereditem>();

    public virtual ICollection<Ordereditem> OrdereditemModifiedbyNavigations { get; set; } = new List<Ordereditem>();

    public virtual ICollection<Ordereditemmodifiermapping> OrdereditemmodifiermappingCreatedbyNavigations { get; set; } = new List<Ordereditemmodifiermapping>();

    public virtual ICollection<Ordereditemmodifiermapping> OrdereditemmodifiermappingModifiedbyNavigations { get; set; } = new List<Ordereditemmodifiermapping>();

    public virtual ICollection<Ordertaxmapping> OrdertaxmappingCreatedbyNavigations { get; set; } = new List<Ordertaxmapping>();

    public virtual ICollection<Ordertaxmapping> OrdertaxmappingModifiedbyNavigations { get; set; } = new List<Ordertaxmapping>();

    public virtual ICollection<Payment> PaymentCreatedbyNavigations { get; set; } = new List<Payment>();

    public virtual ICollection<Payment> PaymentModifiedbyNavigations { get; set; } = new List<Payment>();

    public virtual ICollection<Permission> PermissionCreatedbyNavigations { get; set; } = new List<Permission>();

    public virtual ICollection<Permission> PermissionModifiedbyNavigations { get; set; } = new List<Permission>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Rolepermission> RolepermissionCreatedbyNavigations { get; set; } = new List<Rolepermission>();

    public virtual ICollection<Rolepermission> RolepermissionModifiedbyNavigations { get; set; } = new List<Rolepermission>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<Section> SectionCreatedbyNavigations { get; set; } = new List<Section>();

    public virtual ICollection<Section> SectionModifiedbyNavigations { get; set; } = new List<Section>();

    public virtual ICollection<State> StateCreatedbyNavigations { get; set; } = new List<State>();

    public virtual ICollection<State> StateModifiedbyNavigations { get; set; } = new List<State>();

    public virtual ICollection<Table> TableCreatedbyNavigations { get; set; } = new List<Table>();

    public virtual ICollection<Table> TableModifiedbyNavigations { get; set; } = new List<Table>();

    public virtual ICollection<Tableordermapping> TableordermappingCreatedbyNavigations { get; set; } = new List<Tableordermapping>();

    public virtual ICollection<Tableordermapping> TableordermappingModifiedbyNavigations { get; set; } = new List<Tableordermapping>();

    public virtual ICollection<Taxesandfee> TaxesandfeeCreatedbyNavigations { get; set; } = new List<Taxesandfee>();

    public virtual ICollection<Taxesandfee> TaxesandfeeModifiedbyNavigations { get; set; } = new List<Taxesandfee>();

    public virtual ICollection<Unit> UnitCreatedbyNavigations { get; set; } = new List<Unit>();

    public virtual ICollection<Unit> UnitModifiedbyNavigations { get; set; } = new List<Unit>();

    public virtual ICollection<Waitingtoken> WaitingtokenCreatedbyNavigations { get; set; } = new List<Waitingtoken>();

    public virtual ICollection<Waitingtoken> WaitingtokenModifiedbyNavigations { get; set; } = new List<Waitingtoken>();
}
