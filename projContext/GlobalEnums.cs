using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace projContext
{

    

    
    public enum enmApprovalStatus
    {
        Pending =0,
        Approve = 1,
        Rejected = 2,
        InProcessing = 4
    }

    public enum enmSchdulerType
    {
        Daily = 1,
        Weekly = 2,
        FortNight = 3,
        Monthly = 4,
        Quaterly = 5,
        Halfyearly = 6,
        Yearly = 7,
        NoDues = 8
    }


    public enum enmDocumentPermissionType : byte
    {
        Create = 1,
        Update = 2,
        Approval = 4,
        Delete = 8,
        Report = 16,

    }

    public class MenuComponent : Attribute
    {
        public MenuComponent(string menu_name, enmMenuMaster parent_menu, string icon, string link, byte type, int display_order, enmRoleMaster[] RoleMaster)
        {
            this.menu_name = menu_name;
            this.parent_menu = parent_menu;
            this.icon = icon;
            this.link = link;
            this.type = type;
            this.display_order = display_order;
            this.RoleMaster = RoleMaster;

        }
        public string menu_name { get; set; }
        public enmMenuMaster parent_menu { get; set; }
        public string icon { get; set; }
        public string link { get; set; }
        public byte type { get; set; }
        public int display_order { get; set; }
        public IEnumerable<enmRoleMaster> RoleMaster { get; set; }

    }
    public enum enmMenuMaster
    {
        None = 0,
        [MenuComponent("Dashboard", enmMenuMaster.None, "fa fa-home", "Dashboard", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.PayrollAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, enmRoleMaster.NoDuesAdmin, })] Dashboard = 1,
        [MenuComponent("Organisation", enmMenuMaster.None, "fa fa-building", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, })] Organisation = 101,
        [MenuComponent("Create Company", enmMenuMaster.Organisation, "fa fa-gopuram", "CompanyMaster/Create", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, })] CreateCompany = 102,
        [MenuComponent("Create Location", enmMenuMaster.Organisation, "fa fa-gopuram", "Masters/AddLocationMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, })] CreateLocation = 103,
        [MenuComponent("Master", enmMenuMaster.None, "fa fa-building", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.PayrollAdmin, })] Master = 201,
        [MenuComponent("Department", enmMenuMaster.Master, "fa fa-hospital", "Masters/AddDepartmentMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, })] Department = 202,
        [MenuComponent("Designation", enmMenuMaster.Master, "fa fa-wheelchair", "masters/AddDesignationMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, })] Designation = 203,
        [MenuComponent("Grade", enmMenuMaster.Master, "fa fa-chalkboard-teacher", "Masters/AddGradeMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, })] Grade = 204,
        [MenuComponent("Machine Master", enmMenuMaster.Master, "fa fa-digital-tachograph", "Masters/MachineMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, })] MachineMaster = 205,
        [MenuComponent("Bank Master", enmMenuMaster.Master, "fa fa-university", "Masters/BankMasters", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.MastersAdmin, })] BankMaster = 301,
        [MenuComponent("Event", enmMenuMaster.Master, "fa fa-map-marker-alt", "Masters/addEvent", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.MastersAdmin, })] Event = 302,
        [MenuComponent("Holiday Master", enmMenuMaster.Master, "fa fa-clipboard-list", "Masters/CompanyHoliday", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.AttendanceAdmin, })] HolidayMaster = 303,
        [MenuComponent("Country", enmMenuMaster.Master, "fa fa-clipboard-list", "Masters/Addcountry", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, })] Country = 304,
        [MenuComponent("State", enmMenuMaster.Master, "fa fa-clipboard-list", "Masters/addstate", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, })] State = 305,
        [MenuComponent("City", enmMenuMaster.Master, "fa fa-clipboard-list", "Masters/addcity", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, })] City = 306,
        [MenuComponent("Shift", enmMenuMaster.None, "fa fa-user-clock", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.AttendanceAdmin, })] Shift = 401,
        [MenuComponent("Create Shift", enmMenuMaster.Shift, "fa fa-user-tie", "shift/createshift", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.AttendanceAdmin, })] CreateShift = 402,
        [MenuComponent("Shift Roster", enmMenuMaster.Shift, "fa fa-gopuram", "Shift/AddRoasterMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.AttendanceAdmin, })] ShiftRoster = 403,
        [MenuComponent("Shift Assignemnt", enmMenuMaster.Shift, "fa fa-clipboard-list", "Shift/ShiftAllignment", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.AttendanceAdmin, })] ShiftAssignemnt = 404,
        [MenuComponent("Attendance", enmMenuMaster.None, "fa fa-bars", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] Attendance = 501,
        [MenuComponent("Manual Compoff", enmMenuMaster.Attendance, "fa fa-file-word-o", "Leave/MannualCompoff", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, })] ManualCompoff = 502,
        [MenuComponent("Outdoor Application", enmMenuMaster.Attendance, "fa fa-user-circle-o", "view/OutdoorApplication", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, enmRoleMaster.Employee, })] OutdoorApplication = 503,
        [MenuComponent("Outdoor Approval", enmMenuMaster.Attendance, "fa fa-user-circle-o", "Leave/OutdoorLeaveApproval", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] OutdoorApproval = 504,
        [MenuComponent("Outdoor Cancel", enmMenuMaster.Attendance, "fa fa-user-circle-o", "Admin/OutdoorLeaveApproval", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, })] OutdoorCancel = 505,
        [MenuComponent("Attendance Application", enmMenuMaster.Attendance, "fa fa-user-circle-o", "view/AttendenceApplication", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, enmRoleMaster.Employee, })] AttendanceApplication = 506,
        [MenuComponent("Attendance Approval", enmMenuMaster.Attendance, "fa fa-user-circle-o", "Leave/AttandenceApproval", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] AttendanceApproval = 507,
        [MenuComponent("Attendance Cancel", enmMenuMaster.Attendance, "fa fa-user-circle-o", "Admin/AttandenceApproval", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, })] AttendanceCancel = 508,
        [MenuComponent("Compoff", enmMenuMaster.Attendance, "fa fa-user-circle-o", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] Compoff = 509,
        [MenuComponent("Compoff Credit Application", enmMenuMaster.Compoff, "fa fa-futbol-o", "view/compoffdetails", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] CompoffCreditApplication = 510,
        [MenuComponent("Compoff Credit Approval", enmMenuMaster.Compoff, "fa fa-futbol-o", "view/CompoffRaisedApproval", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] CompoffCreditApproval = 511,
        [MenuComponent("Compoff Credit Cancel", enmMenuMaster.Compoff, "fa fa-futbol-o", "Admin/CompoffAdditionApplication", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, })] CompoffCreditCancel = 512,
        [MenuComponent("Compoff Application", enmMenuMaster.Compoff, "fa fa-futbol-o", "view/CompOffApplication", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] CompoffApplication = 513,
        [MenuComponent("Compoff Approval", enmMenuMaster.Compoff, "fa fa-futbol-o", "Leave/CompOffLeaveApproval", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] CompoffApproval = 514,
        [MenuComponent("Compoff Cancel", enmMenuMaster.Compoff, "fa fa-futbol-o", "Admin/CompOffLeaveApproval", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, })] CompoffCancel = 515,
        [MenuComponent("Leave", enmMenuMaster.None, "fa fa-coffee", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] Leave = 601,
        [MenuComponent("Leave Type", enmMenuMaster.Leave, "fa fa-coffee", "Masters/AddLeaveType", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.AttendanceAdmin, })] LeaveType = 602,
        [MenuComponent("Leave Setting", enmMenuMaster.Leave, "fa fa-coffee", "Leave/AddLeaveSetting", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.AttendanceAdmin, })] LeaveSetting = 603,
        [MenuComponent("Manual Leave", enmMenuMaster.Leave, "fa fa-coffee", "leave/mannualleave", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, })] ManualLeave = 604,
        [MenuComponent("Leave Application", enmMenuMaster.Leave, "fa fa-coffee", "View/LeaveApplication", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] LeaveApplication = 605,
        [MenuComponent("Leave Approval", enmMenuMaster.Leave, "fa fa-coffee", "Leave/LeaveApproval", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] LeaveApproval = 606,
        [MenuComponent("Leave Cancel", enmMenuMaster.Leave, "fa fa-coffee", "Admin/LeaveApproved", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, })] LeaveCancel = 607,
        [MenuComponent("Payroll", enmMenuMaster.None, "fa fa-credit-card", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.PayrollAdmin, })] Payroll = 701,
        [MenuComponent("Payroll Setting", enmMenuMaster.Payroll, "fa fa-money", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.PayrollAdmin, })] PayrollSetting = 702,
        [MenuComponent("Salary Group", enmMenuMaster.PayrollSetting, "fa fa-money", "payroll/SalGroupMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.PayrollAdmin, })] SalaryGroup = 703,
        [MenuComponent("Salary Group Alignment", enmMenuMaster.PayrollSetting, "fa fa-money", "payroll/FormulaComponent", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, })] SalaryGroupAlignment = 704,
        [MenuComponent("Total Payroll Days Setting", enmMenuMaster.PayrollSetting, "fa fa-money", "Payroll/LODSetting", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, })] TotalPayrollDaysSetting = 705,
        [MenuComponent("Payroll Cycle Setting", enmMenuMaster.PayrollSetting, "fa fa-money", "payroll/PayrollMonthCircle", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, })] PayrollCycleSetting = 706,
        [MenuComponent("OT Compoff Setting", enmMenuMaster.PayrollSetting, "fa fa-money", "Shift/Settings", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.PayrollAdmin, })] OTCompoffSetting = 707,
        [MenuComponent("OT Rate", enmMenuMaster.PayrollSetting, "fa fa-money", "payroll/OTRateMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.PayrollAdmin, })] OTRate = 708,
        [MenuComponent("Repository", enmMenuMaster.PayrollSetting, "fa fa-money", "payroll/Repository", 0, 0, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, })] Repository = 709,
        [MenuComponent("Emp Salary Group", enmMenuMaster.Payroll, "fa fa-money", "Payroll/SalaryGroupMapEmp", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.EmployeeMasterAdmin, })] EmpSalaryGroup = 710,
        [MenuComponent("Emp Salary", enmMenuMaster.Payroll, "fa fa-money", "Payroll/SalaryRevision", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.NoDuesAdmin, })] EmpSalary = 711,
        [MenuComponent("LOP", enmMenuMaster.Payroll, "fa fa-money", "payroll/LODMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.PayrollAdmin, })] LOP = 712,
        [MenuComponent("Payroll Input", enmMenuMaster.Payroll, "fa fa-money", "payroll/SalaryInput", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.PayrollAdmin, })] PayrollInput = 713,
        [MenuComponent("Process Payroll", enmMenuMaster.Payroll, "fa fa-money", "payroll/ProcessPayroll", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] ProcessPayroll = 714,
        [MenuComponent("Musters", enmMenuMaster.Payroll, "fa fa-money", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] Musters = 715,
        [MenuComponent("Muster Setting", enmMenuMaster.Musters, "fa fa-diamond", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] MusterSetting = 716,
        [MenuComponent("Fine Master Form-I", enmMenuMaster.MusterSetting, "fa fa-diamond", "payroll/RegisterOfFinesFormIMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] FineMasterForm1 = 717,
        [MenuComponent("Deduction Form-II", enmMenuMaster.MusterSetting, "fa fa-diamond", "payroll/RegisterOfDeductionsFormIIMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] DeductionForm2 = 718,
        [MenuComponent("Advance Form-III", enmMenuMaster.MusterSetting, "fa fa-diamond", "payroll/RegisterofAdvanceFormIIIMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] AdvanceForm3 = 719,
        [MenuComponent("Overtime Form -IV", enmMenuMaster.MusterSetting, "fa fa-diamond", "payroll/RegisterofOvertimeFormIVMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] OverTimeForm4 = 720,
        [MenuComponent("Register of Wages Form-X", enmMenuMaster.MusterSetting, "fa fa-diamond", "payroll/RegisterOfWagesFormXMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] RegisterOfWagesForm10 = 721,
        [MenuComponent("Register of Wages Slip Form XI", enmMenuMaster.MusterSetting, "fa fa-diamond", "payroll/RegisterOfWagesSlipFormXIMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] RegisterOfWagesSlipForm11 = 722,
        [MenuComponent("Tax", enmMenuMaster.None, "fa fa-money", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] Tax = 801,
        [MenuComponent("Employee Tax", enmMenuMaster.Tax, "fa fa-gift", "payroll/EmployeeIncomeTaxAmount", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] EmployeeTax = 802,
        [MenuComponent("Loan", enmMenuMaster.None, "fa fa-money", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin,/* enmRoleMaster.FinanceAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.PayrollAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee,*/ })] Loan = 901,
        [MenuComponent("Loan Setting", enmMenuMaster.Loan, "fa fa-briefcase", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.MastersAdmin, })] LoanSetting = 902,
        //[MenuComponent("Config-I", enmMenuMaster.LoanSetting, "fa fa-briefcase", "payroll/LoanMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.MastersAdmin, })] LoanConfiguration1 = 903,
        [MenuComponent("Config-I", enmMenuMaster.LoanSetting, "fa fa-briefcase", "Payroll/LoanRequestMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.MastersAdmin, })] LoanConfiguration2 = 904,
        [MenuComponent("Loan Request", enmMenuMaster.Loan, "fa fa-briefcase", "payroll/LoanRequest", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] LoanRequest = 905,
        [MenuComponent("Loan Approval", enmMenuMaster.Loan, "fa fa-briefcase", "Payroll/LoanRequestApproval", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] LoanApproval = 906,
        [MenuComponent("Loan Repayment", enmMenuMaster.Loan, "fa fa-briefcase", "payroll/LoanRepayments", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, })] LoanRepayment = 907,
        [MenuComponent("Asset", enmMenuMaster.None, "fa fa-font", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, enmRoleMaster.Employee, })] Asset = 1001,
        [MenuComponent("Asset Master", enmMenuMaster.Asset, "fa fa-font", "Masters/AssetMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.MastersAdmin, })] AssetMaster = 1002,
        [MenuComponent("Asset Request", enmMenuMaster.Asset, "fa fa-font", "view/AssetRequest", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] AssetRequest = 1003,
        [MenuComponent("Asset Approval", enmMenuMaster.Asset, "fa fa-font", "Payroll/AssetRequestApproval", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] AssetApproval = 1004,
        [MenuComponent("Employee", enmMenuMaster.None, "fa fa-user", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] Employee = 1101,
        [MenuComponent("Create Employee", enmMenuMaster.Employee, "fa fa-user-clock", "Employee/CreateUser", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] CreateEmployee = 1102,
        [MenuComponent("Official Section", enmMenuMaster.Employee, "fa fa-user-clock", "Employee/OfficaialSection", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] OfficialSection = 1103,
        [MenuComponent("Qualification", enmMenuMaster.Employee, "fa fa-user-clock", "Employee/QualificationSection", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] Qualification = 1104,
        [MenuComponent("Family Details", enmMenuMaster.Employee, "fa fa-user-clock", "Employee/FamilySection", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] EmpFamily = 1105,
        [MenuComponent("Personal Details", enmMenuMaster.Employee, "fa fa-user-clock", "Employee/PersonalDetails", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] PersonalDetails = 1106,
        [MenuComponent("Emp Allocation", enmMenuMaster.Employee, "fa fa-user-clock", "Employee/Allocation", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] EmpAllocation = 1107,
        [MenuComponent("Emp Helth Card", enmMenuMaster.Employee, "fa fa-user-clock", "Employee/HealthCard", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] HelthCard = 1108,
        [MenuComponent("Emp Documents", enmMenuMaster.Employee, "fa fa-user-clock", "Employee/MaintainDocuments", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] EmpDocuments = 1109,
        [MenuComponent("EmpStatus", enmMenuMaster.Employee, "fa fa-user-clock", "Employee/EmploymentType", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] EmpStatus = 1110,
        [MenuComponent("Prev Employer", enmMenuMaster.Employee, "fa fa-user-clock", "Employee/EmpPreviousCompanyDetail", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] PreviousEmployer = 1111,
        [MenuComponent("Employee Setting", enmMenuMaster.Employee, "fa fa-user-clock", "Employee/ActiveInActiveUser", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] EmployeeSetting = 1112,
        [MenuComponent("Saturity", enmMenuMaster.Employee, "fa fa-user-clock", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] Saturity = 1113,
        [MenuComponent("Provident Fund", enmMenuMaster.Saturity, "fa fa-money", "Employee/UANDetails", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] ProvidentFund = 1114,
        [MenuComponent("Account Details", enmMenuMaster.Saturity, "fa fa-money", "Employee/AccountDetails", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] AccountDetails = 1115,
        [MenuComponent("Upload", enmMenuMaster.None, "fa fa-file", "Employee/BulkUpload", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, })] Upload = 1201,
        [MenuComponent("EPA", enmMenuMaster.None, "fa fa-file", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] EPA = 1301,
        [MenuComponent("WrokingRole", enmMenuMaster.EPA, "fa fa-tachometer", "Employee/WorkingRoleAllocation", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.SectionHead, })] EmpWorkingRole = 1302,
        [MenuComponent("EPA Setting", enmMenuMaster.EPA, "fa fa-tachometer", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] EPASetting = 1303,
        [MenuComponent("Fiscal Year", enmMenuMaster.EPASetting, "fa fa-tachometer", "epa/FinancialYear", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, })] EPAFiscalYear = 1304,
        [MenuComponent("EPA Cycle", enmMenuMaster.EPASetting, "fa fa-tachometer", "epa/epa_cycle", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, })] EPACycle = 1305,
        [MenuComponent("Working Role", enmMenuMaster.EPASetting, "fa fa-tachometer", "epa/ePAWorkingrole", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, })] EPAWorkingRole = 1306,
        [MenuComponent("KPI Objective", enmMenuMaster.EPASetting, "fa fa-tachometer", "epa/KPIObjective", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, })] KPIObjective = 1307,
        [MenuComponent("KPI Criteria", enmMenuMaster.EPASetting, "fa fa-tachometer", "epa/KpiCriteria", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, })] KPICriteria = 1308,
        [MenuComponent("KRA Rating Master", enmMenuMaster.EPASetting, "fa fa-tachometer", "epa/KRA_Rating_Master", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, })] KRARatingMaster = 1309,
        [MenuComponent("KPI Key Area Master", enmMenuMaster.EPASetting, "fa fa-tachometer", "epa/KpiKeyAreaMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] KPIKeyAreaMaster = 1310,
        [MenuComponent("KRA Creation", enmMenuMaster.EPASetting, "fa fa-tachometer", "epa/KRACreationMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] KRACreation = 1311,
        [MenuComponent("EPA Status Master", enmMenuMaster.EPASetting, "fa fa-tachometer", "epa/StatusMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, })] EPAStatusMaster = 1312,
        [MenuComponent("EPA Status Flow", enmMenuMaster.EPASetting, "fa fa-tachometer", "epa/StatusFlowMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, })] EPAStatusFlow = 1313,
        [MenuComponent("EPA Tab Master", enmMenuMaster.EPASetting, "fa fa-tachometer", "epa/TabMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.Manager, enmRoleMaster.HRAdmin, enmRoleMaster.TeamLeader, })] EPATabMaster = 1314,
        [MenuComponent("EPA Question Master", enmMenuMaster.EPASetting, "fa fa-tachometer", "epa/QuestionMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.Manager, enmRoleMaster.HRAdmin, enmRoleMaster.TeamLeader, })] EPAQuestionMaster = 1315,
        [MenuComponent("EPA Allignment", enmMenuMaster.EPASetting, "fa fa-tachometer", "epa/WorkingRoleComponent", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] EPAAllignment = 1316,
        [MenuComponent("EPA Submission", enmMenuMaster.EPA, "fa fa-tachometer", "EPA/EpaSubmissionForm", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] EPASubmission = 1317,
        [MenuComponent("Report", enmMenuMaster.None, "fas fa-chart-pie", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.PayrollAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] Report = 1401,
        [MenuComponent("Organization Report", enmMenuMaster.Report, "fa fa-list-alt", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, })] OrganizationReport = 1402,
        [MenuComponent("Company Report", enmMenuMaster.OrganizationReport, "fa fa-gopuram", "CompanyMaster/View", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, })] CompanyReport = 1403,
        [MenuComponent("Location Report", enmMenuMaster.OrganizationReport, "fa fa-book-open", "Masters/DetailLocationMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, })] LocationReport = 1404,
        [MenuComponent("Shift Report", enmMenuMaster.Report, "fa fa-list-alt", "Shift/View", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.AttendanceAdmin, })] ShiftReport = 1405,
        [MenuComponent("Emp Report", enmMenuMaster.Report, "fa fa-list-alt", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] EmpReport = 1406,
        [MenuComponent("Employee", enmMenuMaster.EmpReport, "fa fa-user-clock", "Employee/ViewEmployee", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.PayrollAdmin, })] EmpBasicReport = 1407,
        [MenuComponent("Employee Shift", enmMenuMaster.EmpReport, "fa fa-user-clock", "Shift/ShiftAllignmentDetail", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] EmployeeShift = 1408,
        [MenuComponent("Attendance Reports", enmMenuMaster.Report, "fa fa-list-alt", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.PayrollAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, enmRoleMaster.Employee, })] AttendanceReports = 1409,
        [MenuComponent("Calender", enmMenuMaster.AttendanceReports, "fa fa-rocket", "view/Dashboard", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.SectionHead, })] Calender = 1410,
        [MenuComponent("Application Reports", enmMenuMaster.AttendanceReports, "fa fa-rocket", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] ApplicationReports = 1411,
        [MenuComponent("Outdoor Application Report", enmMenuMaster.ApplicationReports, "fa fa-rocket", "view/OutdoorApplicationReport", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] OutdoorApplicationReport = 1412,
        [MenuComponent("Attendance Application Report", enmMenuMaster.ApplicationReports, "fa fa-rocket", "view/AttendenceApplicationReport", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] AttendanceApplicationReport = 1413,
        [MenuComponent("Compoff Credit Application Report", enmMenuMaster.ApplicationReports, "fa fa-rocket", "view/CompoffRaiseReport", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] CompoffCreditApplicationReport = 1414,
        [MenuComponent("Compoff Application Report", enmMenuMaster.ApplicationReports, "fa fa-rocket", "view/CompOffApplicationReport", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] CompoffApplicationReport = 1415,
        [MenuComponent("Attendance Report", enmMenuMaster.AttendanceReports, "fa fa-rocket", "Report/Attendance", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.PayrollAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] AttendanceReport = 1416,
        [MenuComponent("Absent Report", enmMenuMaster.AttendanceReports, "fa fa-rocket", "Report/Absent", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] AbsentReport = 1417,
        [MenuComponent("Present Report", enmMenuMaster.AttendanceReports, "fa fa-rocket", "Report/Present", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] PresentReport = 1418,
        [MenuComponent("Mispunch Report", enmMenuMaster.AttendanceReports, "fa fa-rocket", "Report/Mispunch", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] MispunchReport = 1419,
        [MenuComponent("Manul Punch", enmMenuMaster.AttendanceReports, "fa fa-rocket", "Report/ManualPunch", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] ManulPunch = 1420,
        [MenuComponent("Late Punch In", enmMenuMaster.AttendanceReports, "fa fa-rocket", "Report/LatePunchIn", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] LatePunchIn = 1421,
        [MenuComponent("Early Punch out", enmMenuMaster.AttendanceReports, "fa fa-rocket", "Report/EarlyPunchOut", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] EarlyPunchout = 1422,
        [MenuComponent("Location Punch Report", enmMenuMaster.AttendanceReports, "fa fa-rocket", "Report/LocationBy", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] LocationPunchReport = 1423,
        [MenuComponent("Leave Report", enmMenuMaster.Report, "fa fa-list-alt", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] LeaveReport = 1424,
        [MenuComponent("Leave Balance", enmMenuMaster.LeaveReport, "fa fa-list-alt", "Leave/LeaveBalance", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] LeaveBalance = 1425,
        [MenuComponent("Leave Application Report", enmMenuMaster.LeaveReport, "fa fa-list-alt", "view/LeaveApplicationReport", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] LeaveApplicationReport = 1426,
        [MenuComponent("Leave Ledeger", enmMenuMaster.LeaveReport, "fa fa-list-alt", "Leave/MannualLeaveReport", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, })] LeaveLedeger = 1444,
        [MenuComponent("Payroll Report", enmMenuMaster.Report, "fa fa-list-alt", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] PayrollReport = 1427,
        [MenuComponent("Muster Report", enmMenuMaster.PayrollReport, "fa fa-money", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] MusterReport = 1428,
        [MenuComponent("Fine Master Form-I", enmMenuMaster.MusterReport, "fa fa-diamond", "payroll/RegisterOfFinesFormIMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] FineMasterForm1Report = 1429,
        [MenuComponent("Deduction Form-II", enmMenuMaster.MusterReport, "fa fa-diamond", "payroll/RegisterOfDeductionsFormIIMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] DeductionForm2Report = 1430,
        [MenuComponent("Advance Form-III", enmMenuMaster.MusterReport, "fa fa-diamond", "payroll/RegisterofAdvanceFormIIIMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] AdvanceForm3Report = 1431,
        [MenuComponent("Overtime Form -IV", enmMenuMaster.MusterReport, "fa fa-diamond", "payroll/RegisterofOvertimeFormIVMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] OverTimeForm4Report = 1432,
        [MenuComponent("Minimum Wages Central Rules FORM-V", enmMenuMaster.MusterReport, "fa fa-diamond", "payroll/MinimumWagesCentralRulesFORMV", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] MinimumWagesCentralRulesFORM5 = 1433,
        [MenuComponent("Register of Wages Form-X", enmMenuMaster.MusterReport, "fa fa-diamond", "payroll/RegisterOfWagesFormXMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] RegisterOfWagesForm10Report = 1434,
        [MenuComponent("Register of Wages Slip Form XI", enmMenuMaster.MusterReport, "fa fa-diamond", "payroll/RegisterOfWagesSlipFormXIMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] RegisterOfWagesSlipForm11Report = 1435,
        [MenuComponent("Salary Slip", enmMenuMaster.PayrollReport, "fa fa-money", "view/SalarySlips", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] SalarySlip = 1436,
        [MenuComponent("SalaryReport (Arrea)", enmMenuMaster.PayrollReport, "fa fa-money", "Report/DynamicReport?1", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] SalaryReportArrea = 1437,
        [MenuComponent("Salary Report", enmMenuMaster.PayrollReport, "fa fa-money", "Report/DynamicReport?2", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.PayrollAdmin, })] SalaryReport = 1438,
        [MenuComponent("Emp Asset Reports", enmMenuMaster.Report, "fa fa-list-alt", "View/EmpReqAssetReport", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.FinanceAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader, enmRoleMaster.Employee, })] EmpAssetReports = 1439,
        [MenuComponent("EPAReport", enmMenuMaster.Report, "fa fa-list-alt", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] EPAReport = 1440,
        [MenuComponent("EPA Details", enmMenuMaster.EPAReport, "fa fa-tachometer", "epa/TabDetails", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] EPATabDetails = 1441,
        [MenuComponent("Bar Chart", enmMenuMaster.EPAReport, "fa fa-bar-chart", "epa/EpaBarchart", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] EPABarChart = 1442,
        [MenuComponent("EPA Graph", enmMenuMaster.EPAReport, "fa fa-pie-chart", "epa/EPAGraphChart", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] EPAGraph = 1443,
        [MenuComponent("Resignation Cancellation", enmMenuMaster.Report, "fa fa-pie-chart", "View/EmpSeprationDetail", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.EmployeeMasterAdmin,/* enmRoleMaster.Manager, enmRoleMaster.Employee,*/ })] ESeprationReport = 1603,
        [MenuComponent("Setting", enmMenuMaster.None, "fa fas fa-cog", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, })] Setting = 1501,
        [MenuComponent("Loan Approval Setting", enmMenuMaster.Setting, "fa fa-cog", "Payroll/LoanApprovalSettingMaster", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, })] LoanApprovalSettingMaster = 1502,
        [MenuComponent("Role Allocation", enmMenuMaster.Setting, "fa fa-cog", "Masters/assignrolemenu", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, })] RoleAllocation = 1503,
        [MenuComponent("Emp Sepration", enmMenuMaster.None, "fa fa-cog", "#", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.AttendanceAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, })] Esepration = 1601,
        [MenuComponent("Resignation Application", enmMenuMaster.Esepration, "fa fa-cog", "View/EmpSepration", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, })] EseprationApplication = 1445,
        [MenuComponent("Resignation Approval", enmMenuMaster.Esepration, "fa fa-cog", "View/EmpSeprationApproval", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.Manager, enmRoleMaster.TeamLeader })] EseprationApproval = 1602,
        [MenuComponent("Employee Withdrawal", enmMenuMaster.Payroll, "fa fa-paypal", "Employee/Withdrawal", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, })] EmployeeWithdrawal = 1607,
        [MenuComponent("Employee FNF", enmMenuMaster.Esepration, "fa fa-paypal", "", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, })] EmployeeFNF = 1608,
        [MenuComponent("FNF Process", enmMenuMaster.EmployeeFNF, "fa fa-paypal", "Payroll/EmployeeFNF", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, })] EmpFNFProcess = 1609,
        [MenuComponent("Documents Type", enmMenuMaster.Master, "far fa-file-image", "Masters/DocumentType", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin })] DocumentType = 1610,
        [MenuComponent("Employee Dump", enmMenuMaster.None, "fa fa-user", "Employee/EmployeeDump", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] EmployeeDump = 1611,
        [MenuComponent("Leave Adjust Report", enmMenuMaster.None, "fa fa-user", "Report/LeaveAdjustReport", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] EmployeeLeaveAdjustReport = 1612,
        [MenuComponent("Monthly Summary Report", enmMenuMaster.None, "fa fa-user", "Report/MonthlySummaryReport", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] EmployeeMonthlySummaryReport = 1613,
        [MenuComponent("Attendance Monthly Report", enmMenuMaster.None, "fa fa-user", "Report/AttendanceMonthlyReport", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] EmployeeAttendanceMonthlyReport = 1614,
        [MenuComponent("Attendence Summary Report", enmMenuMaster.None, "fa fa-user", "Report/AttendenceSummaryReport", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] EmployeeAttendenceSummaryReport = 1615,
        [MenuComponent("Attendance In Out Report", enmMenuMaster.None, "fa fa-user", "Report/AttendanceInOutReport", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, })] EmployeeAttendanceInOutReport = 1616,
        [MenuComponent("KT Task", enmMenuMaster.None, "fa fa-user", "Employee/Save_KT_Task", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] KT_Task = 1641,
        [MenuComponent("KT Task Status", enmMenuMaster.None, "fa fa-user", "Employee/Save_KT_Task", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] KT_Task_Status = 1642,
        [MenuComponent("KT Task Report", enmMenuMaster.None, "fa fa-user", "Employee/Save_KT_Task", 1, 1, new enmRoleMaster[] { enmRoleMaster.SuperAdmin, enmRoleMaster.HRAdmin, enmRoleMaster.MastersAdmin, enmRoleMaster.EmployeeMasterAdmin, enmRoleMaster.Manager, enmRoleMaster.Employee, enmRoleMaster.TeamLeader, })] KT_Task_Report = 1644,
    }

    public enum enmRoleMaster
    {
        SuperAdmin = 1,
        Manager = 11,
        SectionHead = 12,
        TeamLeader = 13,
        Management = 14,
        Consultant = 15,
        Employee = 21,
        Intern = 22,
        Trainee = 23,
        HRAdmin = 101,
        FinanceAdmin = 102,
        MastersAdmin = 103,
        EmployeeMasterAdmin = 104,
        AttendanceAdmin = 105,
        PayrollAdmin = 106,
        NoDuesAdmin = 107
    }

    public enum enmLeaveTransactionType
    {
        Add_by_System = 1,
        Consumed = 2,
        Expired = 3,
        In_Cash = 4,
        Manual_Add = 5,
        Manual_Delete = 6,
        Deleted_By_System = 7,
        Previous_Leave_Credit_by_System = 100
    }



    public enum enmPaymentMode
    {
        [Description("Bank Transfer")]
        BankTransfer = 1,
        [Description("Cheque")]
        Cheque = 2,
        [Description("Cash")]
        Cash = 3,
        [Description("Demand Draft")]
        DemandDraft = 4
    }

    public enum enmPayrollReportProperty
    {
        NormalValue = 1,
        Rate = 2,
        CurrentMonthValue = 3,
        ArrearValue = 4,
        TotalValue = 5
    }

    public enum enmPFGroup
    {
        [Description("Min Slab")]
        PercentageOnMinBasicSlab = 1,
        [Description("12% of Basic")]
        PercentageOnBasic = 2
    }
    public enum enmVPFGroup
    {
        [Description("Fixed Amount")]
        FixedAmount = 1,
        [Description("Basic Percentage")]
        BasicPercentage = 2
    }

    public enum enmComponentType
    {
        Income = 1,
        Deduction = 2,
        Other = 3,
        OtherIncome = 4,
        OtherDeduction = 5,
        EmployerContribution = 6
    }
    public enum enmComponentDataType
    {
        Integer = 1,
        Decimal = 2,
        String = 3,
        DateTime = 4
    }

    public class PayrollComponent : Attribute
    {
        public string component_name { get; set; }
        public byte datatype { get; set; }
        public string defaultvalue { get; set; }
        public int parentid { get; set; }
        public byte is_system_key { get; set; }
        public string System_function { get; set; }
        public enmComponentType ComponentType { get; set; }
        public byte is_salary_comp { get; set; }
        public byte is_data_entry_comp { get; set; }
        public int is_user_interface { get; set; }
        public byte is_payslip { get; set; }
        public string formula { get; set; }
        public int function_calling_order { get; set; }
    }

        public enum enmJourneyType
        {
            OneWay = 1, Return = 2, MultiStop = 3, AdvanceSearch = 4, SpecialReturn = 5
        }

        public enum enmCabinClass
        {
            //ALL=1,
            ECONOMY = 2,
            PREMIUM_ECONOMY = 3,
            BUSINESS = 4,
            //PremiumBusiness=5,
            FIRST = 6
        }

        public enum enmMarkupApplicability
        {
            OnTicket = 1,
            OnPassenger = 2,
            OnBaggageServices = 4,
            OnMealServices = 8,
            OnSeatServices = 16,
            OnExtraService = 32
        }

        public enum enmPreferredDepartureTime
        {
            AnyTime = 1,
            Morning = 2,
            AfterNoon = 3,
            Evening = 4,
            Night = 5
        }

        public enum enmBookingStatus
        {
            Pending = 0,
            Booked = 1,
            Refund = 2,
            PartialBooked = 3,
            Failed = 4,
            All = 100,
        }


        public enum enmBankTransactionType
        {
            None = 0,
            UPI = 1,
            NEFT = 2,
            RTGS = 3,
            CHEQUE = 4
        }
        public enum enmCustomerType
        {
            Admin = 1,
            MLM = 2,
            B2B = 3,
            B2C = 4,
            InHouse = 5
        }


        public enum enmServiceProvider
        {
            None = 0,
            TBO = 1,
            TripJack = 2,
            Kafila = 3
        }

        public enum enmPassengerType
        {
            Adult = 1,
            Child = 2,
            Infant = 3,
        }
        public enum enmGender
        {
            Male = 1,
            Female = 2,
            Trans = 4,
        }

        public enum enmPackageCustomerType
        {
            Solo = 1,
            Couple = 2,
            Family = 4,
            Friends = 8,
            Cooperate = 16,
        }
        public enum enmStatus
        {
            Active = 0,
            Deactive = 1,
        }

        public enum enmTCStatus
        {
            Active = 1,
            Block = 2,
            Terminate = 3,

        }
        public enum enmTCRanks
        {
            Level1 = 1,
            Level2 = 2,
            Level3 = 3,
            Level4 = 4,
            Level5 = 5,
            Level6 = 6,
            Level7 = 7,
            Level8 = 8,
        }

        public enum enmAddressType
        {
            Permanent = 1,
            Contact = 2
        }

        public enum enmBookingType
        {
            Flight = 1,
            Bus = 2,
            Train = 3,
            Hotel = 4,
            HolidayPackage = 5
        }

        public enum enmUserType
        {
            Consolidator = 1,
            Employee = 2,
            B2B = 4,
            B2C = 8
        }

        public enum enmMessageType
        {
            None= 0,
            Success = 1,
            Error = 2,
            Warning = 3,
            Info = 4,
        }
        public enum enmApprovalType : byte
        {
            Pending = 0,
            Approved = 1,
            Rejected = 2,
            InProcessing = 3,
            Initiated = 4
        }
        public enum enmProcessStatus : byte
        {
            Pending = 0,
            Completed = 1,
            InProcessing = 2,
            Hold = 3,
            Initiated = 4,
        }

        public enum enmIsKycUpdated
        {
            No = 0,
            Yes = 1,
            Partial = 2
        }

        public enum enmIdentityProof
        {
            Aadhar = 1,
            Passport = 2,
            PANcard = 3,
            DrivingLicense = 4,
            VoterID = 5,
        }

        public enum enmNomineeRelation
        {
            Father = 1,
            Mother = 2,
            Husband = 3,
            Wife = 4,
            Son = 5,
            Daughter = 6,
        }


        public enum enmMemberRank
        {
            Diamond = 1,
            Turquoise = 2,
            Amber = 3,
            Zircon = 4,
            Onyx = 5,
            lolite = 6,
            Ivory = 7,
            Amethyst = 8,
        }

        public enum enmIncentiveStatus
        {
            Pending = 1,
            Processing = 2,
            Hold = 3,
            Release = 4,
            Cancelled = 5,
            Adjusted = 6,
        }

        public enum enmAddressProof
        {
            Aadhar = 1,
            Passport = 2,
            Passbook = 3,
            RationCard = 4,
            Waterbill = 5,
            ElectricityBill = 6,
        }
        public enum enmWalletTransactiontype
        {
            Credit = 1,
            Debit = 2,
        }


        public enum enmLoadData
        {
            ByID = 1,
            ByNid = 2,
            ByDateFilter = 3,
            ByApproved = 4,
            ByReject = 5
        }

        public enum enmIncentiveTransactionType
        {
            PushIncentive = 1,
            DipatchIncentive = 2,
            HoldIncenitve = 3,
            UnHoldIncentive = 4,
            AdjustIncentive = 5,
            TransferToWallet = 6,
            ReturnFromWallet = 7,
            ReturnIncentive = 8

        }

        public enum enmPaymentRequestType
        {
            WalletRecharge = 1,
            CreditRequest = 2,
            IncentiveTransferToWallet = 3,
            IncentiveReturnFromWallet = 4,
        }
        public enum enmTransactionType
        {
            FlightTicketBook = 1,
            HotelTicketBook = 2,
            BusesBook = 3,
            TaxiBook = 4,
            TrainTicketBook = 5,
            PackageBook = 10,
            IncentiveTransferToWallet = 11,
            IncentiveReturnFromWallet = 12,
            WalletAmountUpdate = 100,
            PaymentGatewayAmountUpdate = 101,
            OnCreditUpdate = 102,
        }

        public enum enmNotificationType
        {
            Flight = 1,
            Bus = 2,
            Train = 3,
            Hotel = 4,
            HolidayPackage = 5,

            PanStatusChange = 101,
            BankStatusChange = 102,
            NomineeStatusChange = 103,
            IdentityStatusChange = 104,

            WalletMinBalenceAlert = 111,
            PaymentRequestStatusChange = 112
        }
        public enum enmNotificationMode
        {
            Application = 1,
            SMS = 2,
            Email = 3,
            MobilePush = 4,
            DesktopPush = 5,

        }
    




    public enum enmOtherComponent
    {

        [PayrollComponent(component_name = "Company Id", ComponentType = enmComponentType.Other,
            datatype = 3, defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0,
            is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncCompanyId_sys")]
        CompanyId = 100,
        [PayrollComponent(component_name = "Company Name", ComponentType = enmComponentType.Other,
            datatype = 3, defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0,
            is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1,
            System_function = "fncCompanyName_sys")]
        CompanyName = 101,
        [PayrollComponent(component_name = "Company Logo", ComponentType = enmComponentType.Other,
            datatype = 3, defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0,
            is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1,
            System_function = "fncCompanyLogoPath_sys")]
        CompanyLogoPath = 102,
        [PayrollComponent(component_name = "Emp Code", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEMP_Code_sys")]
        EmpCode = 103,
        [PayrollComponent(component_name = "Emp Name", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0,
            is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1,
            System_function = "fncEmpName_sys")]
        EmpName = 104,
        [PayrollComponent(component_name = "Father/Husband Name", ComponentType = enmComponentType.Other,
            datatype = 3, defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0,
            is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1,
            System_function = "fncEmpFatherHusbandName_sys")]
        EmpFatherHusbandName = 105,

        [PayrollComponent(component_name = "Joining Dt", ComponentType = enmComponentType.Other,
            datatype = 4, defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0,
            is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1,
            System_function = "fncEmpJoiningDt_sys")]
        EmpJoiningDt = 106,
        [PayrollComponent(component_name = "Emp Status", ComponentType = enmComponentType.Other,
            datatype = 3, defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0,
            is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEmpStatus_sys")]
        EmpStatus = 107,
        [PayrollComponent(component_name = "Gender", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0,
            is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1,
            System_function = "fncGender_sys")]
        Gender = 108,
        [PayrollComponent(component_name = "DOB", ComponentType = enmComponentType.Other, datatype = 4,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0,
            is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1,
            System_function = "fncEmpDOB_sys")]
        EmpDOB = 109,
        [PayrollComponent(component_name = "Email", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEmpEmailId_sys")]
        EmpEmail = 110,
        [PayrollComponent(component_name = "Nationality", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncNationality_sys")]
        EmpNationality = 111,
        [PayrollComponent(component_name = "Education Level", ComponentType = enmComponentType.Other,
            datatype = 3, defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0,
            is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEducationLevel_sys")]
        EducationLevel = 112,
        [PayrollComponent(component_name = "ContactNo", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEmpContact_sys")]
        EmpContact = 113,



        [PayrollComponent(component_name = "Grade", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1,
            System_function = "fncEMPGradeName_sys")]
        EmpGrade = 114,
        [PayrollComponent(component_name = "Designation", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEmpDesignation_sys")]
        EmpDesignation = 115,
        [PayrollComponent(component_name = "Department", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEmpDepartment_sys")]
        EmpDepartment = 116,
        [PayrollComponent(component_name = "Location", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEmpLocation_sys")]
        EmpLocation = 117,
        [PayrollComponent(component_name = "WorkingState", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEmpWorkingState_sys")]
        EmpWorkingState = 118,

        [PayrollComponent(component_name = "Pan", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncPanNo_sys")]
        PanNo = 119,
        [PayrollComponent(component_name = "Pan Name", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncPanName_sys")]
        PanName = 120,
        [PayrollComponent(component_name = "Adhar", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncAdharNo_sys")]
        AdharNo = 121,
        [PayrollComponent(component_name = "Adhar Name", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncAdharName_sys")]
        AdharName = 122,


        [PayrollComponent(component_name = "Account No", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncBankAccountNo_sys")]
        BankAccountNo = 201,
        [PayrollComponent(component_name = "IFSC", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncIFSCCode_sys")]
        IFSCCode = 202,
        [PayrollComponent(component_name = "Bank", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncBankName_sys")]
        BankName = 203,

        [PayrollComponent(component_name = "ESIC flag", ComponentType = enmComponentType.Other, datatype = 1,
            defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEsicAppliable_sys")]
        EsicApplicable = 204,
        [PayrollComponent(component_name = "ESIC No", ComponentType = enmComponentType.Other, datatype = 1,
            defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEsicNo_sys")]
        ESICNo = 205,
        [PayrollComponent(component_name = "ESIC", ComponentType = enmComponentType.Other, datatype = 3,
            defaultvalue = "1", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEsicAppliableName_sys")]
        EsicApplicableYesNo = 206,
        [PayrollComponent(component_name = "ESIC Per", ComponentType = enmComponentType.Other, datatype = 2,
            defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEsicPercentage_sys")]
        EsicPercentage = 207,

        [PayrollComponent(component_name = "ESIC Employer Per", ComponentType = enmComponentType.Other, datatype = 2,
            defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEmployerEsicPercentage_sys")]
        EmployerEsicPercentage = 208,






        [PayrollComponent(component_name = "Total Payroll Day", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "30", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncToalPayrollDay_sys")]
        ToalPayrollDay = 501,
        [PayrollComponent(component_name = "LOD Sys", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncLOD_sys")]
        LodSys = 502,
        [PayrollComponent(component_name = "LOP", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "@LodSys", function_calling_order = 2, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 1, System_function = "")]
        LopDays = 503,
        [PayrollComponent(component_name = "Paid Day", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0",
            formula = "@ToalPayrollDay-@LopDays", function_calling_order = 3, is_data_entry_comp = 0, is_payslip = 1, is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 1, System_function = "")]
        PaidDays = 504,
        [PayrollComponent(component_name = "Arrear Day", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "0", function_calling_order = 3, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 1, System_function = "")]
        ArrearDays = 505,
        [PayrollComponent(component_name = "Arrear YearMonth", ComponentType = enmComponentType.Other, datatype = 1, defaultvalue = "0", formula = "0", function_calling_order = 3, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 1, System_function = "")]
        ArrearMonthYear = 506,



        [PayrollComponent(component_name = "UAN", ComponentType = enmComponentType.Other, datatype = 3, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncUanNo_sys")]
        Uan = 507,
        [PayrollComponent(component_name = "PF Applicable Flag", ComponentType = enmComponentType.Other, datatype = 1, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncPFApplicable_sys")]
        PfApplicable = 508,
        [PayrollComponent(component_name = "EPS Applicable Flag", ComponentType = enmComponentType.Other, datatype = 1, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEPSApplicable_sys")]
        EpsApplicable = 509,
        [PayrollComponent(component_name = "PF Applicable", ComponentType = enmComponentType.Other, datatype = 3, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncPFApplicableyesNo_sys")]
        PfApplicableYesNo = 510,
        [PayrollComponent(component_name = "EPS Applicable", ComponentType = enmComponentType.Other, datatype = 3, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEPSApplicableyesNo_sys")]
        EpsApplicableYesNo = 511,
        [PayrollComponent(component_name = "PF No", ComponentType = enmComponentType.Other, datatype = 3, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncPfNo_sys")]
        PfNo = 512,
        [PayrollComponent(component_name = "PF Group", ComponentType = enmComponentType.Other, datatype = 3, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncPFGroupid_sys")]
        PfGroup = 513,
        [PayrollComponent(component_name = "VPF Group", ComponentType = enmComponentType.Other, datatype = 3, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncVPFGroup_sys")]
        VPfGroup = 514,
        [PayrollComponent(component_name = "PF ceiling", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncPfSalarySlab_sys")]
        PfSalarySlab = 515,
        [PayrollComponent(component_name = "PF Per", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncPfPercentage_sys")]
        PfPercentage = 516,
        [PayrollComponent(component_name = "VPF Applicable Flag", ComponentType = enmComponentType.Other, datatype = 1, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncVPFApplicable_sys")]
        Vpf_applicable = 517,
        [PayrollComponent(component_name = "VPF Applicable", ComponentType = enmComponentType.Other, datatype = 3, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncVPFApplicableyesNo_sys")]
        Vpf_applicableYesNo = 518,
        [PayrollComponent(component_name = "VPF Per", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncVpfPercentage_sys")]

        vpf_percentage = 519,
        [PayrollComponent(component_name = "PF ceiling", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncPFCeling_sys")]
        pf_celling = 520,

        [PayrollComponent(component_name = "Employer Pf Per", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEmployer_pf_percentage_sys")]
        Employer_pf_percentage = 521,

        [PayrollComponent(component_name = "EPS Per", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEmployer_Pension_Scheme_percentage_sys")]
        Employer_Pension_Scheme_percentage = 522,

        [PayrollComponent(component_name = "EDLIS Per", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEmployer_EDLIS_percetage_sys")]
        Employer_EDLIS_percetage = 523,

        [PayrollComponent(component_name = "Admin Epf per", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEPF_Administration_charges_percentage_sys")]
        EPF_Administration_charges_percentage = 524,

        [PayrollComponent(component_name = "Admin EDLIS per", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEDLIS_Administration_charges_percentage_sys")]
        EDLIS_Administration_charges_percentage = 525,


        [PayrollComponent(component_name = "Salary Group Id", ComponentType = enmComponentType.Other, datatype = 1, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncSalaryGroupId_sys")]
        EmpSalaryGroupId = 526,
        [PayrollComponent(component_name = "Salary Group", ComponentType = enmComponentType.Other, datatype = 3, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEmpSalaryGroupName_sys")]
        EmpSalaryGroupName = 527,

        [PayrollComponent(component_name = "Gross Salary Sys", ComponentType = enmComponentType.Other, datatype = 2,
            defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0,
            is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncSalary_sys")]
        GrossSalary_sys = 528,
        [PayrollComponent(component_name = "Gross Salary", ComponentType = enmComponentType.Other, datatype = 2,
            defaultvalue = "0", formula = "@GrossSalary_sys", function_calling_order = 2, is_data_entry_comp = 1,
            is_payslip = 1, is_salary_comp = 1, is_system_key = 0, is_user_interface = 1, parentid = 1, System_function = "")]
        GrossSalary = 529,

        [PayrollComponent(component_name = "OT Rate System", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncOTRate_sys")]
        OTRate_sys = 530,
        [PayrollComponent(component_name = "OT Hour System", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncOTHour_sys")]
        OTHour_sys = 531,
        [PayrollComponent(component_name = "OT Rate", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "@OTRate_sys", function_calling_order = 2, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 1, System_function = "")]
        OTRate = 532,
        [PayrollComponent(component_name = "OT Hour", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "@OTHour_sys", function_calling_order = 2, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 1, System_function = "")]
        OTHour = 533,

        [PayrollComponent(component_name = "Tax Sys", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncIncomeTax_sys")]
        Tax_sys = 534,
        [PayrollComponent(component_name = "Advance Sys", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "", function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 1, is_user_interface = 0, parentid = 1, System_function = "fncEMI_sys")]
        Advance_Loan_sys = 535,

        [PayrollComponent(component_name = "CTC", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "@GrossSalary+@Employer_pf_amount",
            function_calling_order = 201, is_data_entry_comp = 0, is_payslip = 1, is_salary_comp = 1, is_system_key = 0,
            is_user_interface = 1, parentid = 1, System_function = "")]
        CTC = 536,

        [PayrollComponent(component_name = "Total Gross", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "Round(IFNULL(@BasicSalary,0)+IFNULL(@HRA,0)+IFNULL(@Conveyance,0)+IFNULL(@SPL,0)+IFNULL(@DA,0),0)",
            function_calling_order = 100, is_data_entry_comp = 0, is_payslip = 1, is_salary_comp = 0, is_system_key = 0,
            is_user_interface = 1, parentid = 1, System_function = "")]
        TotalGross = 537,


        [PayrollComponent(component_name = "ESIC flag manual", ComponentType = enmComponentType.Other,
            datatype = 1, defaultvalue = "1", formula = "if(@GrossSalary>=21000,0,1)", function_calling_order = 10,
            is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 0,
            is_user_interface = 0, parentid = 1, System_function = "")]
        EsicApplicableManual = 210,

        [PayrollComponent(component_name = "PF Applicable Flag manual", ComponentType = enmComponentType.Other,
            datatype = 1, defaultvalue = "1", formula = "1", function_calling_order = 10,
            is_data_entry_comp = 0, is_payslip = 0, is_salary_comp = 0, is_system_key = 0, is_user_interface = 0,
            parentid = 1, System_function = "")]
        PfApplicableManual = 540,


        [PayrollComponent(component_name = "EL Encashment Day", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "0",
            function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 1, is_salary_comp = 0, is_system_key = 1,
            is_user_interface = 1, parentid = 1000, System_function = "fncELEncasment_sys")]
        ELEncashmentDay = 541,
        [PayrollComponent(component_name = "CompOff Encashment Day", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "0",
            function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 1, is_salary_comp = 0, is_system_key = 1,
            is_user_interface = 1, parentid = 1000, System_function = "fncComp_off_Encasment_sys")]
        CompOffEncashmentDay = 542,

        [PayrollComponent(component_name = "Addtitional Paid Day", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "0",
            function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 1, is_salary_comp = 0, is_system_key = 1,
            is_user_interface = 1, parentid = 1000, System_function = "fncAddtitionalPaidDay_sys")]
        AddtitionalPaidDay = 543,


        [PayrollComponent(component_name = "Net Paid Days", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0",
            formula = "ifnull(@PaidDays,0)+ifnull(@AddtitionalPaidDay,0) + ifnull( @CompOffEncashmentDay ,0)+IfNull( @ELEncashmentDay,0) + IfNull( @NoticePaymentDay,0) - IfNull( @NoticeRecoveryDay,0)",
            function_calling_order = 4, is_data_entry_comp = 0, is_payslip = 1, is_salary_comp = 0, is_system_key = 0,
            is_user_interface = 1, parentid = 1000, System_function = "")]
        NetPaidDays = 544,

        [PayrollComponent(component_name = "NoticePaymentDay", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "0",
            function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 1, is_salary_comp = 0, is_system_key = 1,
            is_user_interface = 1, parentid = 1000, System_function = "fncNoticePaymentDay_sys")]
        NoticePaymentDay = 545,
        [PayrollComponent(component_name = "NoticeRecoveryDay", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "0",
            function_calling_order = 1, is_data_entry_comp = 0, is_payslip = 1, is_salary_comp = 0, is_system_key = 1,
            is_user_interface = 1, parentid = 1000, System_function = "fncNoticeRecoveryDay_sys")]
        NoticeRecoveryDay = 546,



        [PayrollComponent(component_name = "Basic", ComponentType = enmComponentType.Income, datatype = 2, defaultvalue = "0",
            formula = "ROUND(if(@GrossSalary>25000,@GrossSalary*0.6,@GrossSalary*0.67),0)",
            function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 1, is_system_key = 0,
            is_user_interface = 1, parentid = 2000, System_function = "")]
        BasicSalary = 2001,
        [PayrollComponent(component_name = "HRA", ComponentType = enmComponentType.Income, datatype = 2, defaultvalue = "0",
            formula = "ROUND(if( @GrossSalary>25000,@BasicSalary*0.5,@GrossSalary*0.33),0)",
            function_calling_order = 40, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 1, is_system_key = 0,
            is_user_interface = 1, parentid = 2000, System_function = "")]
        HRA = 2002,
        [PayrollComponent(component_name = "Conveyance", ComponentType = enmComponentType.Income, datatype = 2, defaultvalue = "0",
            formula = "GREATEST(IF((@GrossSalary-@BasicSalary-@HRA)>=1600,1600,(@GrossSalary-@BasicSalary-@HRA)),0)",
            function_calling_order = 50, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 1, is_system_key = 0,
            is_user_interface = 1, parentid = 2000, System_function = "")]
        Conveyance = 2003,
        [PayrollComponent(component_name = "Special Allowance", ComponentType = enmComponentType.Income, datatype = 2, defaultvalue = "0",
            formula = "GREATEST(ROUND(@GrossSalary-@BasicSalary-@HRA-@Conveyance,0),0)",
            function_calling_order = 60, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 1, is_system_key = 0,
            is_user_interface = 1, parentid = 2000, System_function = "")]
        SPL = 2004,
        [PayrollComponent(component_name = "Bonus", ComponentType = enmComponentType.OtherIncome, datatype = 2,
            defaultvalue = "0", formula = "0", function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1,
            is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 2000, System_function = "")]
        Bonus = 2005,

        [PayrollComponent(component_name = "OT", ComponentType = enmComponentType.OtherIncome, datatype = 2, defaultvalue = "0",
            formula = "IFNULL(@OTHour,0)* IFNULL(@OTRate,0)", function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 2000, System_function = "")]
        OT = 2006,
        [PayrollComponent(component_name = "Medical Allowance", ComponentType = enmComponentType.OtherIncome,
            datatype = 2, defaultvalue = "0", formula = "0", function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 2000, System_function = "")]
        Medical_Allowance = 2007,
        [PayrollComponent(component_name = "Children Education Allowance", ComponentType = enmComponentType.OtherIncome,
            datatype = 2, defaultvalue = "0", formula = "0", function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 2000, System_function = "")]
        ChildrenEducationAllowance = 2008,
        [PayrollComponent(component_name = "CityCompensatoryAllowances", ComponentType = enmComponentType.OtherIncome,
            datatype = 2, defaultvalue = "0", formula = "0", function_calling_order = 30, is_data_entry_comp = 1,
            is_payslip = 1, is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 2000, System_function = "")]
        CityCompensatoryAllowances = 2009,
        [PayrollComponent(component_name = "Compoff Encashment", ComponentType = enmComponentType.OtherIncome,
            datatype = 2, defaultvalue = "0", formula = "0", function_calling_order = 30, is_data_entry_comp = 1,
            is_payslip = 1, is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 2000, System_function = "")]
        CompoffEncashment = 2010,
        [PayrollComponent(component_name = "Imprest Payable", ComponentType = enmComponentType.OtherIncome, datatype = 2,
            defaultvalue = "0", formula = "0", function_calling_order = 30, is_data_entry_comp = 1,
            is_payslip = 1, is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 2000, System_function = "")]
        ImprestPayable = 2011,
        [PayrollComponent(component_name = "Gratuity", ComponentType = enmComponentType.OtherIncome, datatype = 2,
            defaultvalue = "0", formula = "0", function_calling_order = 30, is_data_entry_comp = 1,
            is_payslip = 1, is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 2000, System_function = "")]
        Gratuity = 2012,
        [PayrollComponent(component_name = "Leen", ComponentType = enmComponentType.OtherIncome, datatype = 2,
            defaultvalue = "0", formula = "0", function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1,
            is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 2000, System_function = "")]
        Leen = 2013,
        [PayrollComponent(component_name = "DA", ComponentType = enmComponentType.Income, datatype = 30, defaultvalue = "0",
            formula = "0", function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 1,
            is_system_key = 0, is_user_interface = 1, parentid = 2000, System_function = "")]
        DA = 2014,


        [PayrollComponent(component_name = "Tax", ComponentType = enmComponentType.OtherDeduction, datatype = 2,
            defaultvalue = "0", formula = "@Tax_sys", function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1,
            is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 3000, System_function = "")]
        Tax = 3001,
        [PayrollComponent(component_name = "Advance", ComponentType = enmComponentType.OtherDeduction, datatype = 2,
            defaultvalue = "0", formula = "@Advance_Loan_sys", function_calling_order = 30, is_data_entry_comp = 1,
            is_payslip = 1, is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 3000, System_function = "")]
        Advance_Loan = 3002,
        [PayrollComponent(component_name = "ESIC", ComponentType = enmComponentType.OtherDeduction, datatype = 2,
            defaultvalue = "0", formula = "If ( @EsicApplicableManual=1, ceiling(@GrossSalary* @EsicPercentage/100.0),0)",
            function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 1, is_system_key = 0,
            is_user_interface = 1, parentid = 3000, System_function = "")]
        EsicAmount = 3003,
        [PayrollComponent(component_name = "ER ESIC", ComponentType = enmComponentType.OtherDeduction, datatype = 2, defaultvalue = "0",
            formula = "If ( @EsicApplicableManual=1, round((@GrossSalary* @EmployerEsicPercentage/100.0),0),0)",
            function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0,
            is_user_interface = 1, parentid = 3000, System_function = "")]
        EmployerEsicAmount = 3004,

        [PayrollComponent(component_name = "PF", ComponentType = enmComponentType.OtherDeduction, datatype = 2, defaultvalue = "0",
            formula = "If ( (@BasicSalary)<=@PfSalarySlab, If ((@BasicSalary)*(@PfPercentage/100.0) >=@pf_celling,@pf_celling,ROUND((@BasicSalary)*(@PfPercentage/100.0),0)),If(@PfApplicable=1, 	If ( if(@PfGroup=1,@PfSalarySlab,@BasicSalary) *(@PfPercentage/100.0) >=@pf_celling    ,@pf_celling,ROUND(if(@PfGroup=1,@PfSalarySlab,@BasicSalary)*(@PfPercentage/100.0),0)),0))",
            function_calling_order = 70, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 1, is_system_key = 0,
            is_user_interface = 1, parentid = 3000, System_function = "")]
        Pf_amount = 3005,
        [PayrollComponent(component_name = "VPF", ComponentType = enmComponentType.OtherDeduction, datatype = 2, defaultvalue = "0",
            formula = "If(@Vpf_applicable=1,If(@VPfGroup=1,@vpf_percentage, ROUND( @BasicSalary * @vpf_percentage/100.0 ,0)) ,0)",
            function_calling_order = 70, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 1, is_system_key = 0,
            is_user_interface = 1, parentid = 3000, System_function = "")]
        Vpf_amount = 3006,

        [PayrollComponent(component_name = "Product Purchase", ComponentType = enmComponentType.OtherDeduction, datatype = 2,
            defaultvalue = "0", formula = "0", function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1,
            is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 3000, System_function = "")]
        ProductPurchase = 3007,
        [PayrollComponent(component_name = "Recovery", ComponentType = enmComponentType.OtherDeduction, datatype = 2,
            defaultvalue = "0", formula = "0", function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1,
            is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 3000, System_function = "")]
        Recovery = 3008,
        [PayrollComponent(component_name = "Other Deduction", ComponentType = enmComponentType.OtherDeduction, datatype = 2,
            defaultvalue = "0", formula = "0", function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1,
            is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 3000, System_function = "")]
        OtherDeduction = 3009,
        [PayrollComponent(component_name = "PT", ComponentType = enmComponentType.OtherDeduction, datatype = 2,
            defaultvalue = "0", formula = "0", function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1,
            is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 3000, System_function = "")]
        PT = 3010,
        [PayrollComponent(component_name = "Surcharge", ComponentType = enmComponentType.OtherDeduction,
            datatype = 2, defaultvalue = "0", formula = "0", function_calling_order = 30,
            is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0,
            is_user_interface = 1, parentid = 3000, System_function = "")]
        Surcharge = 3011,
        [PayrollComponent(component_name = "Cess", ComponentType = enmComponentType.OtherDeduction, datatype = 2,
            defaultvalue = "0", formula = "0", function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1,
            is_salary_comp = 0, is_system_key = 0, is_user_interface = 1, parentid = 3000, System_function = "")]
        Cess = 3012,


        [PayrollComponent(component_name = "ER PF", ComponentType = enmComponentType.EmployerContribution, datatype = 2,
            defaultvalue = "0", formula = "@Pf_amount", function_calling_order = 71, is_data_entry_comp = 1,
            is_payslip = 1, is_salary_comp = 1, is_system_key = 0,
            is_user_interface = 1, parentid = 4000, System_function = "")]
        Employer_pf_amount = 4001,
        [PayrollComponent(component_name = "ER EPS", ComponentType = enmComponentType.EmployerContribution, datatype = 2, defaultvalue = "0", formula = "0",
            function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0,
            is_user_interface = 1, parentid = 4000, System_function = "")]
        Employer_Pension_Scheme_amount = 4002,
        [PayrollComponent(component_name = "ER EDLIS", ComponentType = enmComponentType.EmployerContribution, datatype = 2, defaultvalue = "0", formula = "0",
            function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0,
            is_user_interface = 1, parentid = 4000, System_function = "")]
        Employer_EDLIS_amount = 4003,
        [PayrollComponent(component_name = "ER AdminCharge EPF", ComponentType = enmComponentType.EmployerContribution, datatype = 2, defaultvalue = "0", formula = "0",
            function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0,
            is_user_interface = 1, parentid = 4000, System_function = "")]
        EPF_Administration_charges_amount = 4004,
        [PayrollComponent(component_name = "ER AdminCharge EDLIS", ComponentType = enmComponentType.EmployerContribution, datatype = 2, defaultvalue = "0", formula = "0",
            function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0,
            is_user_interface = 1, parentid = 4000, System_function = "")]
        EDLIS_Administration_charges_amount = 4005,


        [PayrollComponent(component_name = "LWF", ComponentType = enmComponentType.Deduction, datatype = 2, defaultvalue = "0", formula = "0",
            function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0,
            is_user_interface = 1, parentid = 1, System_function = "")]
        LWF = 5000,
        [PayrollComponent(component_name = "TDS", ComponentType = enmComponentType.Deduction, datatype = 2, defaultvalue = "0", formula = "0",
            function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0,
            is_user_interface = 1, parentid = 1, System_function = "")]
        TDS = 5001,
        [PayrollComponent(component_name = "Salary Arrear", ComponentType = enmComponentType.Income, datatype = 2, defaultvalue = "0", formula = "0",
            function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0,
            is_user_interface = 1, parentid = 1, System_function = "")]
        Salary_Arrear = 5002,
        [PayrollComponent(component_name = "Overtime", ComponentType = enmComponentType.Income, datatype = 2, defaultvalue = "0", formula = "0",
            function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0,
            is_user_interface = 1, parentid = 1, System_function = "")]
        Overtime = 5003,
        [PayrollComponent(component_name = "Other Allowance", ComponentType = enmComponentType.Income, datatype = 2, defaultvalue = "0", formula = "0",
            function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0,
            is_user_interface = 1, parentid = 1, System_function = "")]
        Other_Allowance = 5004,
        [PayrollComponent(component_name = "Imprest Deductions", ComponentType = enmComponentType.OtherDeduction, datatype = 2, defaultvalue = "0", formula = "0",
            function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0,
            is_user_interface = 1, parentid = 1, System_function = "")]
        Imprest_Deductions = 5005,
        [PayrollComponent(component_name = "Mobile Phone Recovery", ComponentType = enmComponentType.OtherDeduction, datatype = 2, defaultvalue = "0", formula = "0",
            function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0,
            is_user_interface = 1, parentid = 1, System_function = "")]
        Mobile_Phone_Recovery = 5006,
        [PayrollComponent(component_name = "Notice Period Recovery", ComponentType = enmComponentType.OtherDeduction, datatype = 2, defaultvalue = "0", formula = "0",
            function_calling_order = 30, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0,
            is_user_interface = 1, parentid = 1, System_function = "")]
        Notice_Period_Recovery = 5007,

        //[PayrollComponent(component_name = "EL Encashment", ComponentType = enmComponentType.Income, datatype = 2, defaultvalue = "0", formula = "0",
        //    function_calling_order = 1, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0,
        //    is_user_interface = 1, parentid = 1, System_function = "fncELEncasment_sys")]
        //EL_Encashment = 5008,
        //[PayrollComponent(component_name = "Comp Off Encashment", ComponentType = enmComponentType.Income, datatype = 2, defaultvalue = "0", formula = "0",
        //    function_calling_order = 1, is_data_entry_comp = 1, is_payslip = 1, is_salary_comp = 0, is_system_key = 0,
        //    is_user_interface = 1, parentid = 1, System_function = "fncComp_off_Encasment_sys")]
        //Comp_Off_Encashment = 5009,



        [PayrollComponent(component_name = "Total Deduction", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "IFNULL(@Tax,0)+IFNULL(@Advance_Loan,0)+IFNULL(@EsicAmount,0)+IFNULL(@Pf_amount,0)+IFNULL(@OtherDeduction,0)",
            function_calling_order = 100, is_data_entry_comp = 0, is_payslip = 1, is_salary_comp = 1, is_system_key = 0,
            is_user_interface = 1, parentid = 1, System_function = "")]
        Deduction = 3999,

        [PayrollComponent(component_name = "Net", ComponentType = enmComponentType.Other, datatype = 2, defaultvalue = "0", formula = "IFNULL(@TotalGross,0)-IFNULL(@Deduction,0)",
            function_calling_order = 200, is_data_entry_comp = 0, is_payslip = 1, is_salary_comp = 1, is_system_key = 0,
            is_user_interface = 1, parentid = 1, System_function = "")]
        Net = 2999,

    }
}
