//namespace TMS.DAL
//{
//    using System;
//    using System.Collections.Generic;
//    using System.ComponentModel.DataAnnotations;
//    using System.ComponentModel.DataAnnotations.Schema;
//    using System.Data.Entity.Spatial;

//    [Table("User")]
//    public partial class User
//    {
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
//        public User()
//        {
//            Customers = new HashSet<Customer>();
//            Dealers = new HashSet<Dealer>();
//            Distributors = new HashSet<Distributor>();
//        }

//        public Guid UserId { get; set; }

//        public string Username { get; set; }

//        public string Password { get; set; }

//        [StringLength(50)]
//        public string FirstName { get; set; }

//        [StringLength(50)]
//        public string LastName { get; set; }

//        public string Address_AddressLine1 { get; set; }

//        public string Address_AddressLine2 { get; set; }

//        public string Address_AddressLine3 { get; set; }

//        public string Address_City { get; set; }

//        public string Address_State { get; set; }

//        public string Address_Country { get; set; }

//        public string Address_PostalCode { get; set; }

//        public string PhoneNo { get; set; }

//        public string Email { get; set; }

//        public string WebSite { get; set; }

//        public bool Status { get; set; }

//        public byte[] RowVersion { get; set; }

//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
//        public virtual ICollection<Customer> Customers { get; set; }

//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
//        public virtual ICollection<Dealer> Dealers { get; set; }

//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
//        public virtual ICollection<Distributor> Distributors { get; set; }
//    }
//}
