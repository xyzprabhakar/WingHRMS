using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projContext;
using projContext.DB;


namespace projAPI
{
    #region ***********************************************Created By : Amarjeet ***********************************************
    //Fetch System Key
    public class clsPayrollSystemKey
    {
        //Declare variables
        public readonly int _emp_id;
        public readonly DateTime _from_date, _to_date;

        //Declare context
        private readonly Context _context;

        public clsPayrollSystemKey(Context context, int emp_id, DateTime from_date, DateTime to_date)
        {
            _context = context;
            _emp_id = emp_id;
            _from_date = from_date;
            _to_date = to_date;
        }

        //DOJ
        public string func_dateofjoining()
        {
            throw new NotImplementedException();

            //var get_date_of_joining = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == _emp_id && a.is_deleted == 0).FirstOrDefault();
            //if (get_date_of_joining == null)
            //{
            //    throw new Exception("Invalid DOJ");
            //}
            //return get_date_of_joining.date_of_joining.ToString("dd-MM-yyyy");
        }

        //Employee Code
        public string func_employeecode()
        {
            var get_employee_code = _context.tbl_employee_master.Where(a => a.employee_id == _emp_id && a.is_active == 1).FirstOrDefault();
            if (get_employee_code == null)
            {
                throw new Exception("Invalid Employee Code");
            }
            return get_employee_code.emp_code;
        }

        //Grade
        public string func_grade()
        {
            var get_emp_grade = _context.tbl_emp_grade_allocation.Where(ga => ga.employee_id == _emp_id).Join
                (_context.tbl_grade_master,
                ga => ga.grade_id,
                g => g.grade_id,
                (ga, g) => new
                {
                    g.grade_name
                }).FirstOrDefault();

            if (get_emp_grade == null)
            {
                throw new Exception("Invalid Grade");
            }
            return get_emp_grade.grade_name.ToString();

        }
     
        //Get Designation
        public string func_designationid()
        {
            var get_emp_designation = _context.tbl_emp_desi_allocation.Where(ga => ga.employee_id == _emp_id).Join
               (_context.tbl_designation_master,
               ga => ga.desig_id,
               g => g.designation_id,
               (ga, g) => new
               {
                   g.designation_name
               }).FirstOrDefault();
            if (get_emp_designation == null)
            {
                throw new Exception("Invalid Designation Name");
            }
            return get_emp_designation.designation_name.ToString();
        }

        //Get Absent In Month
        public double func_absnt_count_in_mnth()
        {
            //Full Day Absent
            int absnt_count_in_mnth_full_day = _context.tbl_daily_attendance.Where(a => a.emp_id == _emp_id && a.day_status == 2
             && a.attendance_dt >= _from_date && a.attendance_dt <= _to_date).Count();

            //helf day Absent 
            int absnt_count_in_mnth_helf_day = _context.tbl_daily_attendance.Where(a => a.emp_id == _emp_id && a.day_status == 4
             && a.attendance_dt >= _from_date && a.attendance_dt <= _to_date).Count();

            //helf day Absent with leave
            int absnt_count_in_mnth_helf_day_and_leave = _context.tbl_daily_attendance.Where(a => a.emp_id == _emp_id && a.day_status == 6
             && a.attendance_dt >= _from_date && a.attendance_dt <= _to_date).Count();

            //helf day Absent / 2
            double count_absnt_helf_day = Convert.ToDouble(absnt_count_in_mnth_helf_day) / 2;

            //helf day Absent with leave / 2
            double count_absnt_helf_day_and_leave = Convert.ToDouble(absnt_count_in_mnth_helf_day_and_leave) / 2;

            //Total Absent
            double total_absent_count = absnt_count_in_mnth_full_day + count_absnt_helf_day + count_absnt_helf_day_and_leave;

            return total_absent_count;
        }

        //Get Present 
        public double func_prsnt_count_in_mnth()
        {
            //Full Day Present
            int prsnt_count_in_mnth_full_day = _context.tbl_daily_attendance.Where(a => a.emp_id == _emp_id && a.day_status == 1
             && a.attendance_dt >= _from_date && a.attendance_dt <= _to_date).Count();

            //helf day Present 
            int prsnt_count_in_mnth_helf_day = _context.tbl_daily_attendance.Where(a => a.emp_id == _emp_id && a.day_status == 4
             && a.attendance_dt >= _from_date && a.attendance_dt <= _to_date).Count();

            //helf day Present with leave
            int prsnt_count_in_mnth_helf_day_and_leave = _context.tbl_daily_attendance.Where(a => a.emp_id == _emp_id && a.day_status == 5
             && a.attendance_dt >= _from_date && a.attendance_dt <= _to_date).Count();

            //helf day Present / 2
            double count_prsnt_helf_day = Convert.ToDouble(prsnt_count_in_mnth_helf_day) / 2;

            //helf day Present with leave / 2
            double count_prsnt_helf_day_and_leave = Convert.ToDouble(prsnt_count_in_mnth_helf_day_and_leave) / 2;

            //Total Present
            double total_prsnt_count = prsnt_count_in_mnth_full_day + count_prsnt_helf_day + count_prsnt_helf_day_and_leave;

            return total_prsnt_count;
        }

        //Get WeekOff
        public int func_weekoff_count_in_mnth()
        {
            //Get Weekoff
            int weekoff_count_in_mnth = _context.tbl_daily_attendance.Where(a => a.emp_id == _emp_id && a.is_weekly_off == 1
           && a.attendance_dt >= _from_date && a.attendance_dt <= _to_date).Count();

            return weekoff_count_in_mnth;
        }

        //Get Leave in month 
        public double func_Leave_count_in_mnth()
        {
            //Get Leave
            int leave_count_in_mnth = _context.tbl_daily_attendance.Where(a => a.emp_id == _emp_id && a.day_status == 3
           && a.attendance_dt >= _from_date && a.attendance_dt <= _to_date).Count();

            // Get helf day leave 
            int leave_count_half_day_leave_in_mnth = _context.tbl_daily_attendance.Where(a => a.emp_id == _emp_id
            && a.day_status == 5 || a.day_status == 6
            && a.attendance_dt >= _from_date && a.attendance_dt <= _to_date).Count();
            
            double leave_count_half_day_leave = Convert.ToDouble(leave_count_half_day_leave_in_mnth) / 2;
            
            double total_leave = leave_count_in_mnth + leave_count_half_day_leave;

            return total_leave;
        }

        //Grade
        public string func_sgid()
        {
            var get_emp_salarygroup = _context.tbl_sg_maping.Where(sgm => sgm.emp_id == _emp_id).Join
                (_context.tbl_salary_group, sgm =>sgm.salary_group_id, sg => sg.group_id, (sgm, sg) => new
                {
                    sg.group_name
                }).FirstOrDefault();

            if (get_emp_salarygroup == null)
            {
                throw new Exception("Invalid Grade");
            }
            return get_emp_salarygroup.group_name.ToString();

        }


    }
       
    #endregion *****************************************************************************************************************
}
