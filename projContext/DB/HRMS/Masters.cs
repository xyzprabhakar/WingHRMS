using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB.HRMS
{
    public class tblHolidayMaster :d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HolidayId { get; set; }  
        public DateTime FromDate { get; set; }
        public DateTime Todate { get; set; }
        [MaxLength(64)]
        public string HolidayName { get; set; }        
        public bool IsActive { get; set; }
        public int OrgId { get; set; }
    }

    public class tblDepartment : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeptId { get; set; }  
        [MaxLength(16)] 
        public string Code { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int OrgId { get; set; }
    }

    public class tblDepartWorkingRole : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkingRoleId { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("tblDepartment")] // Foreign Key here
        public int? DeptId { get; set; }
        public tblDepartment tblDepartment { get; set; }
        public int OrgId { get; set; }
    }

    public class tblGrade : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        public int Level { get; set; }
        public bool IsActive { get; set; }
        public int OrgId { get; set; }
    }
    public class tblDesignation : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        public int Level { get; set; }
        public bool IsActive { get; set; }
    }
    public class tblReligionMaster : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        public int Level { get; set; }
        public bool IsActive { get; set; }
        public int OrgId { get; set; }
    }


}
