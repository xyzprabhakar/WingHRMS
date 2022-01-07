using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace projContext
{
    public enum enmApplication 
    {
        [Application(IsArea: false, DisplayOrder: 0, Name: "Gateway", Description: "Basic", Icon: "nav-icon fas fa-tree", AreaName: "")]
        Gateway = 1,
        [Application(IsArea: false, DisplayOrder: 0, Name: "Gateway", Description: "Basic", Icon: "nav-icon fas fa-paper-plane", AreaName: "")]
        B2CGateway = 2,
        [Application(IsArea: false, DisplayOrder: 0, Name: "Gateway", Description: "Basic", Icon: "nav-icon fas fa-briefcase", AreaName: "")]
        B2BGateway = 3,
        [Application(IsArea: false, DisplayOrder: 1, Name: "CRM", Description: "", Icon: "nav-icon fas fa-bar-chart", AreaName: "")]
        CRM = 10,
        [Application(IsArea: false, DisplayOrder: 3, Name: "HRMS", Description: "", Icon: "nav-icon fas fa-calculator", AreaName: "")]
        HRMS= 11,
        [Application(IsArea: false, DisplayOrder: 4, Name: "Asset Management", Description: "", Icon: "nav-icon fas fa-mobile", AreaName: "/Asset/Dashboard")]
        AssetManagement = 12,
        [Application(IsArea: false, DisplayOrder: 5, Name: "Time Sheeting", Description: "", Icon: "nav-icon fas fa-calendar", AreaName: "/Task/Dashboard")]
        ETS= 13,
        [Application(IsArea: false, DisplayOrder: 6, Name: "Warehouse Management", Description: "", Icon: "nav-icon fas fa-industry", AreaName: "/WMS/Dashboard")]
        WMS = 14,
    }

    public enum enmModule : int
    {

        [Module( false, DisplayOrder: 0, Name: "Organisation", Description: "", Icon: "nav-icon far fa-plus-square", AreaName: "", CntrlName: "Profile")]
        Organisation = 1,
        [Module(false, DisplayOrder: 0, Name: "Authentication ", Description: "", Icon: "nav-icon far fa-plus-square", AreaName: "", CntrlName: "Profile")]
        Authentication = 2,


        //[Module( enmApplication.Gateway, false, DisplayOrder: 0, Name: "Profile", Description: "", Icon: "nav-icon far fa-plus-square", AreaName: "", CntrlName: "Profile")]
        //Gateway_Profile = 1,
        //[Module(EnmApplication: enmApplication.Gateway, IsArea: false, DisplayOrder: 0, Name: "Booking", Description: "", Icon: "nav-icon far fa-plus-square", AreaName: "", CntrlName: "Booking")]
        //Gateway_Booking = 2,
        //[Module(EnmApplication: enmApplication.Gateway, IsArea: false, DisplayOrder: 0, Name: "Incentive", Description: "", Icon: "nav-icon far fa-plus-square", AreaName: "", CntrlName: "Incentive")]
        //Gateway_Incentive = 3,
        //[Module(EnmApplication: enmApplication.Gateway, IsArea: false, DisplayOrder: 0, Name: "Promotion", Description: "", Icon: "nav-icon far fa-plus-square", AreaName: "", CntrlName: "Promotion")]
        //Gateway_Promotion = 4,
        //[Module(EnmApplication: enmApplication.Gateway, IsArea: false, DisplayOrder: 0, Name: "Team", Description: "", Icon: "nav-icon far fa-plus-square", AreaName: "", CntrlName: "Team")]
        //Gateway_Team = 5,
        //[Module(EnmApplication: enmApplication.Gateway, IsArea: false, DisplayOrder: 0, Name: "Setting", Description: "", Icon: "nav-icon far fa-plus-square", AreaName: "", CntrlName: "Setting")]
        //Gateway_Setting = 6,


        //[Module(EnmApplication: enmApplication.CRM, IsArea: false, DisplayOrder: 0, Name: "Profile", Description: "TC, bank, Pan, address contact etc", Icon: "nav-icon far fa-plus-square", AreaName: "", CntrlName: "Wing")]
        //CRM_Tc_Profile = 101,



        //[Module(EnmApplication: enmApplication.HRMS, IsArea: false, DisplayOrder: 0, Name: "Profile", Description: "Employee Basic Details", Icon: "nav-icon far fa-plus-square", AreaName: "", CntrlName: "Employee")]
        //HRMS_Employee_Profile = 301,
        //[Module(EnmApplication: enmApplication.HRMS, IsArea: false, DisplayOrder: 0, Name: "Attendance", Description: "Attendance Management", Icon: "nav-icon far fa-plus-square", AreaName: "", CntrlName: "Attendance")]
        //HRMS_AttendanceManagement = 302,
        //[Module(EnmApplication: enmApplication.HRMS, IsArea: false, DisplayOrder: 0, Name: "Payroll", Description: "Attendance Management", Icon: "nav-icon far fa-plus-square", AreaName: "", CntrlName: "Payroll")]
        //HRMS_PayrollManagement = 303,
        //[Module(EnmApplication: enmApplication.HRMS, IsArea: false, DisplayOrder: 0, Name: "ExitManagement", Description: "Employee Basic Details", Icon: "nav-icon far fa-plus-square", AreaName: "", CntrlName: "ExitManagement")]
        //HRMS_ExitManagement = 304,
    }

    public enum enmSubModule : int
    {
        [SubModule(EnmModule: enmModule.Authentication, DisplayOrder: 1, Name: "Org Auth", Description: "Address,Email, Contact", Icon: "nav-icon fas fa-file", CntrlName: "Profile")]
        Organisation_Authentication = 1,

        //[SubModule(EnmModule: enmModule.Gateway_Profile, DisplayOrder: 1, Name: "Personal", Description: "Address,Email, Contact", Icon: "nav-icon fas fa-file", CntrlName: "Profile")]
        //Gateway_Personal_Profile = 1,

        //[SubModule(EnmModule: enmModule.Gateway_Incentive, DisplayOrder: 2, Name: "Wallet", Description: "Add Wallet Money,See Ledger", Icon: "nav-icon fas fa-file", CntrlName: "Wallet")]
        //Gateway_Incentive_wallet = 2,

        //[SubModule(EnmModule: enmModule.Gateway_Incentive, DisplayOrder: 2, Name: "KYC", Description: "", Icon: "nav-icon fas fa-file", CntrlName: "Wing")]
        //CRM_TcProfile_kyc = 11,
        //[SubModule(EnmModule: enmModule.Gateway_Incentive, DisplayOrder: 2, Name: "Bank", Description: "Add Wallet Money,See Ledger", Icon: "nav-icon fas fa-landmark", CntrlName: "Wing")]
        //CRM_TcProfile_Bank = 12,
        //[SubModule(EnmModule: enmModule.Gateway_Incentive, DisplayOrder: 2, Name: "Pan", Description: "Add Wallet Money,See Ledger", Icon: "nav-icon fas fa-file-signature", CntrlName: "Wing")]
        //CRM_TcProfile_Pan = 13,

        //[SubModule(EnmModule: enmModule.Gateway_Incentive, DisplayOrder: 2, Name: "TC", Description: "TC ", Icon: "nav-icon fas fa-file-signature", CntrlName: "Wing")]
        //CRM_TcProfile_TC = 17,

        //[SubModule(EnmModule: enmModule.Gateway_Incentive, DisplayOrder: 2, Name: "Address", Description: "Add Wallet Money,See Ledger", Icon: "nav-icon fas fa-id-card", CntrlName: "Wing")]
        //CRM_TcProfile_Address = 14,
        //[SubModule(EnmModule: enmModule.Gateway_Incentive, DisplayOrder: 2, Name: "Contact", Description: "Add Wallet Money,See Ledger", Icon: "nav-icon fa fa-phone", CntrlName: "Wing")]
        //CRM_TcProfile_contact = 15,
        //[SubModule(EnmModule: enmModule.Gateway_Incentive, DisplayOrder: 2, Name: "Email", Description: "Add Wallet Money,See Ledger", Icon: "nav-icon fa fa-phone", CntrlName: "Wing")]
        //CRM_TcProfile_Email = 16,



    }

    public enum enmDocumentMaster : int
    {

        [Document( enmDocumentType.Report|enmDocumentType.DisplayMenu , 0, "Dashboard", "Dashboard", "far fa-circle nav-icon", "/Index")]
        Dashboard = 1,
        [Document(EnmModule: enmModule.Organisation,DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.Report | enmDocumentType.DisplayMenu, 
            DisplayOrder: 1,Name: "Organisation",Description: "Dashboard the Orgasnisation Details",Icon: "far fa-circle nav-icon",ActionName:"/Masters/Org/Organisation")]
        Organisation= 2,
        [Document(EnmModule: enmModule.Organisation, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.Report | enmDocumentType.DisplayMenu,
            DisplayOrder: 2, Name: "Company", Description: "Company", Icon: "far fa-circle nav-icon",ActionName: "/Masters/Org/Company")]        
        Company= 3,
        [Document(EnmModule: enmModule.Organisation, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.Report | enmDocumentType.DisplayMenu,
            DisplayOrder: 3, Name: "Zone", Description: "Zone", Icon: "far fa-circle nav-icon", ActionName: "/Masters/Org/Zone")]        
        Zone = 4,
        [Document(EnmModule: enmModule.Organisation, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.Report | enmDocumentType.DisplayMenu,
            DisplayOrder: 4, Name: "Location", Description: "Location", Icon: "far fa-circle nav-icon", ActionName: "/Masters/Org/Location")]
        Location = 5,

        [Document(EnmModule: enmModule.Organisation, EnmSubModule:enmSubModule.Organisation_Authentication ,DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.Report | enmDocumentType.DisplayMenu,
            DisplayOrder: 1, Name: "Org Permission", Description: "Organisation Permission", Icon: "far fa-circle nav-icon", ActionName: "/Base/Location")]
        Org_Permission= 6,
        [Document(EnmModule: enmModule.Organisation, EnmSubModule: enmSubModule.Organisation_Authentication, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.Report | enmDocumentType.DisplayMenu,
            DisplayOrder: 1, Name: "User Permission", Description: "User Permission", Icon: "far fa-circle nav-icon", ActionName: "/Base/Location")]
        User_Permission = 7,

        //[Document(enmDocumentType.Report, 1, "Dashboard", "Dashboard", "far fa-circle nav-icon", "/Home/Index")]
        //Gateway_Dashboard = 1,
        //[Document(enmDocumentType.Report, 1, "Notification", "Notifications", "far fa-circle nav-icon", "Notifications")]
        //Gateway_Notifications = 2,
        //[Document(EnmSubModule: enmSubModule.Gateway_Personal_Profile, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.DisplayMenu, DisplayOrder: 1, Name: "Kyc", Description: "Kyc", Icon: "far fa-circle nav-icon", ActionName: "/Home/UploadKyc")]
        //Gateway_UploadKyc = 3,
        //[Document(EnmSubModule: enmSubModule.Gateway_Personal_Profile, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.DisplayMenu, DisplayOrder: 1, Name: "Address", Description: "Address", Icon: "far fa-circle nav-icon", ActionName: "/Home/Address")]
        //Gateway_Address = 4,
        //[Document(EnmSubModule: enmSubModule.Gateway_Personal_Profile, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.DisplayMenu, DisplayOrder: 1, Name: "Contact", Description: "Contact", Icon: "far fa-circle nav-icon", ActionName: "/Home/Contact")]
        //Gateway_Contact = 5,

        //[Document(EnmSubModule: enmSubModule.Gateway_Personal_Profile, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.DisplayMenu, DisplayOrder: 1, Name: "Email", Description: "Email", Icon: "far fa-circle nav-icon", ActionName: "/Home/Email")]
        //Gateway_Email = 6,

        //[Document(EnmModule: enmModule.Gateway_Profile, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Pan", Description: "Pan", Icon: "far fa-circle nav-icon", ActionName: "/Home/PAN")]
        //Gateway_PAN = 7,
        //[Document(EnmModule: enmModule.Gateway_Profile, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Bank", Description: "Bank", Icon: "far fa-circle nav-icon", ActionName: "/Home/Bank")]
        //Gateway_Bank = 8,
        //[Document(EnmModule: enmModule.Gateway_Profile, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Nominee", Description: "Nominee", Icon: "far fa-circle nav-icon", ActionName: "/Home/Nominee")]
        //Gateway_Nominee = 9,
        //[Document(EnmModule: enmModule.Gateway_Booking, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Flight", Description: "Flight", Icon: "fa fa-plane nav-icon", ActionName: "Flight")]
        //Gateway_Flight = 10,
        //[Document(EnmModule: enmModule.Gateway_Booking, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Hotel", Description: "Hotel", Icon: "fa fa-building nav-icon", ActionName: "Hotel")]
        //Gateway_Hotel = 11,
        //[Document(EnmModule: enmModule.Gateway_Booking, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Buses", Description: "Buses", Icon: "fa fa-bus nav-icon", ActionName: "Buses")]
        //Gateway_Buses = 12,
        //[Document(EnmModule: enmModule.Gateway_Booking, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Train", Description: "Train", Icon: "fa fa-train nav-icon", ActionName: "Train")]
        //Gateway_Train = 13,
        //[Document(EnmModule: enmModule.Gateway_Booking, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Holiday Package", Description: "Holiday Package", Icon: "fa fa-binoculars nav-icon", ActionName: "/Wing/HolidayPackageNew")]
        //Gateway_Holiday_Package = 14,

        //[Document(EnmModule: enmModule.Gateway_Booking, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Holiday Package Report", Description: "Holiday Package", Icon: "fa fa-binoculars nav-icon", ActionName: "/Wing/HolidayPackageReport")]
        //Gateway_Holiday_Package_Report = 15,

        //[Document(EnmModule: enmModule.Gateway_Booking, DocumentType: enmDocumentType.Report | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Booking Report", Description: "Booking Report", Icon: "far fa-circle nav-icon", ActionName: "BookingReport")]
        //Gateway_Booking_Report = 19,

        //[Document(EnmModule: enmModule.Gateway_Incentive, DocumentType: enmDocumentType.Report | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Statement", Description: "Statement", Icon: "far fa-circle nav-icon", ActionName: "Statement")]
        //Gateway_Statement = 21,
        //[Document(EnmSubModule: enmSubModule.Gateway_Incentive_wallet, DocumentType: enmDocumentType.Report | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Wallet Statement", Description: "Wallet Statement", Icon: "far fa-circle nav-icon", ActionName: "/Wing/WalletStatement")]
        //Gateway_Wallet_Statement = 22,
        //[Document(EnmSubModule: enmSubModule.Gateway_Incentive_wallet, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Add Wallet", Description: "Add Wallet", Icon: "far fa-circle nav-icon", ActionName: "/Wing/AddWallet")]
        //Gateway_Add_Wallet = 23,

        //[Document(EnmModule: enmModule.Gateway_Promotion, DocumentType: enmDocumentType.Create | enmDocumentType.Update,
        //    DisplayOrder: 1, Name: "Promotion Details", Description: "Promotion Details", Icon: "far fa-circle nav-icon", ActionName: "PromotionDetails")]
        //Gateway_Promotion_Details = 31,

        //[Document(EnmModule: enmModule.Gateway_Team, DocumentType: enmDocumentType.Report | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Tree", Description: "Tree", Icon: "far fa-circle nav-icon", ActionName: "/Home/Tree")]
        //Gateway_Tree = 41,
        //[Document(EnmModule: enmModule.Gateway_Team, DocumentType: enmDocumentType.Report | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Group Billing", Description: "Group Billing", Icon: "far fa-circle nav-icon", ActionName: "GroupBilling")]
        //Gateway_Group_Billing = 42,

        //[Document(EnmModule: enmModule.Gateway_Setting, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.Report | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "MarkUp", Description: "MarkUp", Icon: "far fa-circle nav-icon", ActionName: "/Home/MarkUp")]
        //Gateway_Markup = 51,
        //[Document(EnmModule: enmModule.Gateway_Setting, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Convenience", Description: "Convenience", Icon: "far fa-circle nav-icon", ActionName: "Convenience")]
        //Gateway_convenience_fee = 52,

        //[Document(EnmModule: enmModule.Gateway_Setting, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Change Password", Description: "Change Password", Icon: "far fa-circle nav-icon", ActionName: "/Home/ChangePassword")]
        //Gateway_ChangePassword = 53,

        //[Document(EnmModule: enmModule.Gateway_Setting, DocumentType: enmDocumentType.Create | enmDocumentType.Update | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Dashboard", Description: "Employee Dash board", Icon: "far fa-circle nav-icon", ActionName: "/Wing/Index")]
        //Emp_Dashboard = 101,

        //[Document(EnmSubModule: enmSubModule.CRM_TcProfile_Bank, DocumentType: enmDocumentType.Create | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Bank Details", Description: "Bank Details", Icon: "far fa-circle nav-icon", ActionName: "/Wing/BankDetails")]
        //Emp_Tc_BankDetails = 110,
        //[Document(EnmSubModule: enmSubModule.CRM_TcProfile_Bank, DocumentType: enmDocumentType.Create | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Bank Update", Description: "Bank Update", Icon: "far fa-circle nav-icon", ActionName: "/Wing/BankUpdate")]
        //Emp_Tc_BankUpdate = 111,
        //[Document(EnmSubModule: enmSubModule.CRM_TcProfile_Bank, DocumentType: enmDocumentType.Create | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "Bank Approval", Description: "Bank Approval", Icon: "far fa-circle nav-icon", ActionName: "/Wing/BankApproval")]
        //Emp_Tc_BankApproval = 112,


        //[Document(EnmSubModule: enmSubModule.CRM_TcProfile_Pan, DocumentType: enmDocumentType.Create | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "PAN Details", Description: "PAN Details", Icon: "far fa-circle nav-icon", ActionName: "/Wing/PANDetails")]
        //Emp_Tc_PANDetails = 113,
        //[Document(EnmSubModule: enmSubModule.CRM_TcProfile_Pan, DocumentType: enmDocumentType.Create | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "PAN Update", Description: "PAN Update", Icon: "far fa-circle nav-icon", ActionName: "/Wing/PANUpdate")]
        //Emp_Tc_PANUpdate = 114,
        //[Document(EnmSubModule: enmSubModule.CRM_TcProfile_Pan, DocumentType: enmDocumentType.Create | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "PAN Approval", Description: "PAN Approval", Icon: "far fa-circle nav-icon", ActionName: "/Wing/PANApproval")]
        //Emp_Tc_PANApproval = 115,


        //[Document(EnmSubModule: enmSubModule.CRM_TcProfile_TC, DocumentType: enmDocumentType.Create | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "TC Details", Description: "TC Details", Icon: "far fa-circle nav-icon", ActionName: "/Wing/TCDetails")]
        //Emp_Tc_Details = 116,
        //[Document(EnmSubModule: enmSubModule.CRM_TcProfile_TC, DocumentType: enmDocumentType.Create | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "TC Update", Description: "TC Update", Icon: "far fa-circle nav-icon", ActionName: "/Wing/TCUpdate")]
        //Emp_Tc_Update = 117,
        //[Document(EnmSubModule: enmSubModule.CRM_TcProfile_TC, DocumentType: enmDocumentType.Create | enmDocumentType.DisplayMenu,
        //    DisplayOrder: 1, Name: "TC Approval", Description: "TC Approval", Icon: "far fa-circle nav-icon", ActionName: "/Wing/TCApproval")]
        //Emp_Tc_Approval = 118,

    }

    public interface IDocuments
    {
        int DisplayOrder { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string Icon { get; set; }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class EnumGrouping : Attribute
    {
        public EnumGrouping(string Name, string Description, string GroupName, string DefaultValue)
        {
            this.Name = Name;
            this.Description = Description;
            this.GroupName = GroupName;
            this.DefaultValue = DefaultValue;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GroupName { get; set; }
        public string DefaultValue { get; set; }
    }
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class Application : Attribute, IDocuments
    {
        public Application(bool IsArea, int DisplayOrder, string Name, string Description, string Icon, string AreaName)
        {

            this.IsArea = IsArea;
            this.DisplayOrder = DisplayOrder;
            this.Name = Name;
            this.Description = Description;
            this.Icon = Icon;
            this.AreaName = AreaName;
        }
        public virtual int Id { get; set; }
        public bool IsArea { get; set; }
        public int DisplayOrder { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string AreaName { get; set; }
        public virtual List<Module> Modules { get; set; }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class Module : Attribute, IDocuments
    {
        public Module(bool IsArea, int DisplayOrder, string Name, string Description, string Icon, string AreaName, string CntrlName)
        {   
            this.IsArea = IsArea;
            this.DisplayOrder = DisplayOrder;
            this.Description = Description;
            this.Name = Name;
            this.Icon = Icon;
            this.AreaName = AreaName;
            this.CntrlName = CntrlName;
        }

        public Module(enmApplication EnmApplication, bool IsArea, int DisplayOrder, string Name, string Description, string Icon, string AreaName, string CntrlName)
        {
            this.EnmApplication = EnmApplication;
            this.IsArea = IsArea;
            this.DisplayOrder = DisplayOrder;
            this.Description = Description;
            this.Name = Name;
            this.Icon = Icon;
            this.AreaName = AreaName;
            this.CntrlName = CntrlName;
        }
        public virtual int Id { get; set; }
        public enmApplication? EnmApplication { get; set; }
        public bool IsArea { get; set; }
        public int DisplayOrder { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string AreaName { get; set; }
        public string CntrlName { get; set; }
        public virtual List<SubModule> SubModules { get; set; }
        public virtual List<Document> Documents { get; set; }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class SubModule : Attribute, IDocuments
    {
        public SubModule(enmModule EnmModule, int DisplayOrder, string Name, string Description, string Icon, string CntrlName)
        {
            this.EnmModule = EnmModule;
            this.DisplayOrder = DisplayOrder;
            this.Description = Description;
            this.Name = Name;
            this.CntrlName = CntrlName;
            this.Icon = Icon;
        }
        public virtual int Id { get; set; }
        public enmModule EnmModule { get; set; }
        public int DisplayOrder { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string CntrlName { get; set; }
        public virtual List<Document> Documents { get; set; }

    }

    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class Document : Attribute, IDocuments
    {

        public Document(enmDocumentType DocumentType, int DisplayOrder, string Name, string Description,
    string Icon, string ActionName)
        {   
            this.DocumentType = DocumentType;
            this.DisplayOrder = DisplayOrder;
            this.Description = Description;
            this.Name = Name;
            this.Icon = Icon;
            this.ActionName = ActionName;
        }

        public Document(enmModule EnmModule, enmDocumentType DocumentType, int DisplayOrder, string Name, string Description,
            string Icon, string ActionName)
        {
            
            this.DocumentType = DocumentType;
            this.DisplayOrder = DisplayOrder;
            this.Description = Description;
            this.Name = Name;
            this.Icon = Icon;
            this.ActionName = ActionName;
            this.EnmModule = EnmModule;
        }

        public Document(enmModule EnmModule, enmSubModule EnmSubModule, enmDocumentType DocumentType, int DisplayOrder, string Name, string Description,
            string Icon, string ActionName)
        {
            
            this.DocumentType = DocumentType;
            this.DisplayOrder = DisplayOrder;
            this.Description = Description;
            this.Name = Name;
            this.Icon = Icon;
            this.ActionName = ActionName;
            this.EnmSubModule = EnmSubModule;
            this.EnmModule = EnmModule;
        }
        public Document( enmSubModule EnmSubModule, enmDocumentType DocumentType, int DisplayOrder, string Name, string Description,
            string Icon, string ActionName)
        {   
            this.DocumentType = DocumentType;
            this.DisplayOrder = DisplayOrder;
            this.Description = Description;
            this.Name = Name;
            this.Icon = Icon;
            this.ActionName = ActionName;
            this.EnmSubModule = EnmSubModule;
        }

        public Document(enmApplication EnmApplication, enmDocumentType DocumentType, int DisplayOrder, string Name, string Description,
            string Icon, string ActionName)
        {
            this.EnmApplication = EnmApplication;
            this.DocumentType = DocumentType;
            this.DisplayOrder = DisplayOrder;
            this.Description = Description;
            this.Name = Name;
            this.Icon = Icon;
            this.ActionName = ActionName;
        }

        public Document(enmApplication EnmApplication, enmModule EnmModule, enmDocumentType DocumentType, int DisplayOrder, string Name, string Description,
            string Icon, string ActionName)
        {
            this.EnmApplication = EnmApplication;
            this.DocumentType = DocumentType;
            this.DisplayOrder = DisplayOrder;
            this.Description = Description;
            this.Name = Name;
            this.Icon = Icon;
            this.ActionName = ActionName;
            this.EnmModule = EnmModule;
        }

        public Document(enmApplication EnmApplication, enmModule EnmModule, enmSubModule EnmSubModule, enmDocumentType DocumentType, int DisplayOrder, string Name, string Description,
            string Icon, string ActionName)
        {
            this.EnmApplication = EnmApplication;
            this.DocumentType = DocumentType;
            this.DisplayOrder = DisplayOrder;
            this.Description = Description;
            this.Name = Name;
            this.Icon = Icon;
            this.ActionName = ActionName;
            this.EnmSubModule = EnmSubModule;
            this.EnmModule = EnmModule;
        }
        public Document(enmApplication EnmApplication, enmSubModule EnmSubModule, enmDocumentType DocumentType, int DisplayOrder, string Name, string Description,
            string Icon, string ActionName)
        {
            this.EnmApplication = EnmApplication;
            this.DocumentType = DocumentType;
            this.DisplayOrder = DisplayOrder;
            this.Description = Description;
            this.Name = Name;
            this.Icon = Icon;
            this.ActionName = ActionName;
            this.EnmSubModule = EnmSubModule;
        }


        public enmApplication? EnmApplication { get; set; }
        public virtual int Id { get; set; }
        public enmSubModule? EnmSubModule { get; set; }
        public enmDocumentType DocumentType { get; set; }
        public string Description { get; set; }
        public enmModule? EnmModule { get; set; }
        public int DisplayOrder { get; set; }
        public string Name { get; set; }
        public string ActionName { get; set; }
        public string Icon { get; set; }
        public bool HaveCreate { get { return DocumentType.HasFlag(enmDocumentType.Create); } }
        public bool HaveUpdate { get { return DocumentType.HasFlag(enmDocumentType.Update); } }
        public bool HaveApproval { get { return DocumentType.HasFlag(enmDocumentType.Approval); } }
        public bool HaveDelete { get { return DocumentType.HasFlag(enmDocumentType.Delete); } }
        public bool HaveReport { get { return DocumentType.HasFlag(enmDocumentType.Report); } }
        public bool HaveDisplayMenu { get { return DocumentType.HasFlag(enmDocumentType.DisplayMenu); } }
    }


    public enum enmDocumentType : byte
    {
        None = 0,
        Create = 1,
        Update = 2,
        Approval = 4,
        Delete = 8,
        Report = 16,
        DisplayMenu = 32
    }
    public enum enmDocumentPartitionType : byte
    {
        None,
        FiscalYear,
        Yearly,
        Quaterly,
        Monthly,
    }

    public enum enmSaveStatus : byte
    {
        none = 0,
        success = 1,
        danger = 2,
        warning = 3,
        info = 4,
        primary = 5,
        secondary = 6,
    }
    public enum enmMessage
    {
        [Description("Access Denied!!!")]
        AccessDenied,
        [Description("Pending for Approval!!!")]
        PendingApproval,
        [Description("Save Successfully!!!")]
        SaveSucessfully,
        [Description("Update Successfully!!!")]
        UpdateSucessfully,
        [Description("Approved Successfully!!!")]
        ApprovedSucessfully,
        [Description("Rejected Successfully!!!")]
        RejectSucessfully,
        [Description("Enter all Required inputs!!!")]
        RequiredField,
        [Description("Invalid Id !!!")]
        InvalidID,
        [Description("Invalid Organization !!!")]
        InvalidOrganization,
        [Description("Invalid UserId !!!")]
        InvalidUserID,
        [Description("Invalid Applicable date !!!")]
        InvalidApplicableDt,
        [Description("Invalid Data !!!")]
        InvalidData,
        [Description("Invalid User or Password !!!")]
        InvalidUserOrPassword,
        [Description("Invalid Operation !!!")]
        InvalidOperation,
        [Description("User Blocked !!!")]
        UserLocked,
        [Description("Data already exists !!!")]
        AlreadyExists,
        [Description("Request already in Processing !!!")]
        RequestAlreadyInProcessing,
        [Description("Request already Processed !!!")]
        RequestAlreadyProcessed,
        [Description("Data not exists !!!")]
        DataNotExists,
        [Description("Data not Found !!!")]
        DataNotFound,
        [Description("Concurrency Error !!!")]
        ConcurrencyError,
        [Description("Database Error !!!")]
        DatabaseError,
        [Description("Undefined Exception!!!")]
        UndefinedException,
        [Description("Only one head office can be created!!!")]
        SingleHeadOfficeException,
        [Description("Invalid Old Password !!!")]
        InvalidOldPassword,
        [Description("Invalid Service Provider !!!")]
        InvalidServiceProvider,
        [Description("Provider Not Implemeneted !!!")]
        ProviderNotImplemented,
        [Description("Insufficient Balance !!!")]
        InsufficientBalance

    }

    public static class EnmDescription
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }
            return null; // could also return string.Empty
        }

        public static Document GetDocumentDetails(this enmDocumentMaster e)
        {
            var type = e.GetType();
            var name = Enum.GetName(type, e);
            var returnData = (Document)type.GetField(name).GetCustomAttributes(typeof(Document), false).FirstOrDefault();
            returnData.Id = (int)e;
            return returnData;
        }

        public static Module GetModuleDetails(this enmModule e)
        {
            var type = e.GetType();
            var name = Enum.GetName(type, e);
            var returnData = (Module)type.GetField(name).GetCustomAttributes(typeof(Module), false).FirstOrDefault();
            returnData.Id = (int)e;
            return returnData;
        }

        public static SubModule GetSubModuleDetails(this enmSubModule e)
        {
            var type = e.GetType();
            var name = Enum.GetName(type, e);
            var returnData = (SubModule)type.GetField(name).GetCustomAttributes(typeof(SubModule), false).FirstOrDefault();
            returnData.Id = (int)e;
            return returnData;
        }
        public static Application GetApplicationDetails(this enmApplication e)
        {
            var type = e.GetType();
            var name = Enum.GetName(type, e);
            var returnData = (Application)type.GetField(name).GetCustomAttributes(typeof(Application), false).FirstOrDefault();
            returnData.Id = (int)e;
            return returnData;
        }
    }
}
