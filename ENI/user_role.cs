//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ENI
{
    using System;
    using System.Collections.Generic;
    
    public partial class user_role
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user_role()
        {
            this.user = new HashSet<user>();
            this.user_role_permission = new HashSet<user_role_permission>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public Nullable<bool> is_active { get; set; }
        public string created_by { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user> user { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user_role_permission> user_role_permission { get; set; }
    }
}