//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LambdaProject
{
    using System;
    using System.Collections.Generic;
    
    public partial class StudentCourses
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StudentCourses()
        {
            this.StudentAssignments = new HashSet<StudentAssignments>();
        }
    
        public int StudentCoursesID { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentAssignments> StudentAssignments { get; set; }
    }
}
