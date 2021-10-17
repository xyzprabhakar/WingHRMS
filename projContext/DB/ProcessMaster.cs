using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB
{
    public class tblProcessMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProcessId { get; set; }
        [MaxLength(64)]
        public string ProcessName { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public enmApplication ApplicationId { get; set; }
        public enmModule ModuleId { get; set; }
    }

    public class tblDependentProcess
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public int DependentProcessId { get; set; }
        public bool IsActive { get; set; }
    }

    public class tblProcessExecution
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int MonthYear { get; set; }
        public int ProcessId { get; set; }
        public enmProcessStatus ProcessStatus { get; set; }
        public DateTime created_date { get; set; }
        public int created_by { get; set; }
        public DateTime modified_date { get; set; }
        public int modified_by { get; set; }
    }
}
