using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Model
{
    public class NoDuesModel
    {
    }

    public class NoDues_ClearenceForm
    {
        [Key]
        public int part_id { get; set; }
        public List<int> lst_part_id { get; set; }
        public int dept_id { get; set; }
        public List<int> lst_dept_id { get; set; }
        public List<int> lst_empSep_ids { get; set; }
        public string particular_name { get; set; }
        public List<string> particularList { get; set; }
        public string final_Remark { get; set; }
        public List<string> lst_final_Remarks { get; set; }
        public int outstanding { get; set; }
        public List<int> lst_outstandings { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }

    }

    public class NoDues_particular_master
    {
        [Key]
        public int pkid_ParticularMaster { get; set; }
        public int company_id { get; set; }
        public Company comp_details { get; set; }
        public int department_id { get; set; }
        public int employee_id { get; set; }
        public List<string> employee_ids { get; set; }
        public string particular_name { get; set; }
        public string monthYear { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public List<string> particularList { get; set; }
        public string remarks { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_date { get; set; }
    }
    public class No_dues_particular_responsible
    {
        [Key]
        public int pkid_ParticularResponsible { get; set; }
        public int emp_id { get; set; }
        public EmployeeMaster emp_details { get; set; }
        public List<int> emp_ids { get; set; }
        public int company_id { get; set; }
        public int department_id { get; set; }
        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_date { get; set; }
    }
    public class No_dues_clearance_Department
    {
        [Key]
        public int pkid_ClearanceDepartment { get; set; }
        public int fkid_EmpSaperationId { get; set; }
        public Emp_Separation empSepration_details { get; set; }
        public int department_id { get; set; }
        public int is_Cleared { get; set; }
        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_date { get; set; }
    }
    public class No_dues_emp_particular_Clearence_detail
    {
        [Key]
        public int pkid_EmpParticularClearance { get; set; }
        public int fkid_EmpSaperationId { get; set; }
        public int fkid_EmpId { get; set; }
        public int fkid_CompanyId { get; set; }
        public Emp_Separation empSepration_details { get; set; }
        public int fkid_department_id { get; set; }
        public int fkid_ParticularId { get; set; }
        public NoDues_particular_master particular_details { get; set; }
        public int is_Outstanding { get; set; }
        public string remarks { get; set; }
        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_date { get; set; }
    }




}
