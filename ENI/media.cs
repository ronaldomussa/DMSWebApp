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
    
    public partial class media
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public media()
        {
            this.report_insertions = new HashSet<report_insertions>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public Nullable<int> media_type_id { get; set; }
        public string media_type { get; set; }
        public string media_url { get; set; }
        public Nullable<System.DateTime> start_date { get; set; }
        public Nullable<System.DateTime> end_date { get; set; }
        public Nullable<int> insertions_limit { get; set; }
        public Nullable<int> expose_timing { get; set; }
        public bool expose_at_all { get; set; }
        public string expose_in_groups { get; set; }
        public string expose_in { get; set; }
        public bool is_active { get; set; }
        public string created_by { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<System.DateTime> last_modified_date { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<report_insertions> report_insertions { get; set; }
    }
}
