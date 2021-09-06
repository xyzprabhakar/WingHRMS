using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations
{
    public partial class _3_sTOREDpROCEDURE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var createProcSql1 = @"CREATE PROCEDURE `proc_get_emp_data`(in _proc_type int,in _currentEmpId int,in _currentUserId int,in _Company_Id int, in toDate datetime,in _isActive int)
BEGIN
drop temporary table if exists _tblEmpdata;
	create TEMPORARY TABLE _tblEmpdata(
		EmpId INT ,
        EmpCode varchar(50),
        EmpName varchar(200),
        company_id int,        
        location_id int,        
        dept_id int  ,
        isActive int  ,
        empstatus int
		);
        set @IsHod=0;  #0 for normal user, 1 for Manager, 2 for Admins
        
        if(select 1=1 from tbl_role_master where role_id in (select role_id from tbl_user_role_map where user_id =_currentUserId and is_deleted = 0 ) and is_active=1 and (role_id between 1 and 10 or role_id>100) )then
			set @IsHod=2;        
		elseif (select 1=1 from tbl_role_master where role_id in (select role_id from tbl_user_role_map where user_id =_currentUserId and is_deleted = 0 ) and is_active=1 and (role_id between 11 and 20 ) ) then
            set @IsHod=1; 
        else
            set @IsHod=0; 
        end if;
        
        IF (SELECT 1 = 1 FROM tbl_employee_company_map WHERE employee_id=_currentEmpId and is_deleted=0 limit 1) THEN                 
        # 2 for Admins
         if(@IsHod=2) then		
				  insert _tblEmpdata(EmpId,EmpCode,EmpName,company_id,location_id,dept_id,isActive,empstatus)
                  select distinct t1.employee_id,t1.emp_code,concat(ifnull( t3.employee_first_name,''),' ',ifnull( t3.employee_middle_name,''),' ',ifnull( t3.employee_last_name,'')) ,
                  t2.company_id,t3.location_id,t3.department_id,t1.is_active ,ifnull(t3.current_employee_type,'')
                  from tbl_employee_master t1 
                  inner join tbl_employee_company_map t2 on t1.employee_id=t2.employee_id 
                  inner join tbl_emp_officaial_sec t3 on t1.employee_id=t3.employee_id
                  where
                  t2.company_id=_Company_Id
                  #and t3.current_employee_type < 100 
                  and  case when _isActive=2 then 1=1 else t1.is_active = _isActive  end
                  and  t2.is_deleted=0 and  t3.is_deleted = 0 and t2.is_default=1 
                and  t3.date_of_joining<=toDate;
             end if;   
          # 1 for Manager
		 if(@IsHod=1) then			
                  insert _tblEmpdata(EmpId,EmpCode,EmpName,company_id,location_id,dept_id,isActive,empstatus)
                  select distinct t1.employee_id,t1.emp_code,concat(ifnull( t3.employee_first_name,''),' ',ifnull( t3.employee_middle_name,''),' ',ifnull( t3.employee_last_name,'')) ,
                  t2.company_id,t3.location_id,t3.department_id,t1.is_active ,ifnull(t3.current_employee_type,'')
                  from tbl_employee_master t1 
                  inner join tbl_employee_company_map t2 on t1.employee_id=t2.employee_id 
                  inner join tbl_emp_officaial_sec t3 on t1.employee_id=t3.employee_id    
                  inner join 
                  (
						select employee_id from tbl_emp_manager where m_one_id =_currentEmpId and is_deleted=0
						union all
						select employee_id from tbl_emp_manager where m_two_id =_currentEmpId and is_deleted=0
						union all
						select employee_id from tbl_emp_manager where m_three_id =_currentEmpId and is_deleted=0
                        union all
                        select _currentEmpId
                  ) t4 on t1.employee_id=t4.employee_id
                  where
                  t2.company_id=_Company_Id 
                  #and t3.current_employee_type < 100 
                  #and t1.is_active = _isActive
                  and  case when _isActive=2 then 1=1 else t1.is_active = _isActive  end
                  and t2.is_deleted=0 and  t3.is_deleted = 0 and t2.is_default=1 
                  and t3.date_of_joining<=toDate ;
		    end if;
        #0 for normal user
		 if(@IsHod=0) then	
         
                   insert _tblEmpdata(EmpId,EmpCode,EmpName,company_id,location_id,dept_id,isActive,empstatus)
                  select distinct t1.employee_id,t1.emp_code,concat(ifnull( t3.employee_first_name,''),' ',ifnull( t3.employee_middle_name,''),' ',ifnull( t3.employee_last_name,'')) ,
                  t2.company_id,t3.location_id,t3.department_id,t1.is_active ,ifnull(t3.current_employee_type,'')
                  from tbl_employee_master t1 
                  inner join tbl_employee_company_map t2 on t1.employee_id=t2.employee_id 
                  inner join tbl_emp_officaial_sec t3 on t1.employee_id=t3.employee_id    
                  inner join 
                  (
                   select _currentEmpId as employee_id
                  ) t4 on t1.employee_id=t4.employee_id
                  where
                  t2.company_id=_Company_Id 
                  #and t3.current_employee_type < 100
                  #and t1.is_active = _isActive 
                  and  case when _isActive=2 then 1=1 else t1.is_active = _isActive  end
                  and t2.is_deleted=0 and  t3.is_deleted = 0 and t2.is_default=1 
                  and t3.date_of_joining<=toDate ;                  
		  end if;
		end if;
        
        if(_proc_type =1) then
     
			select 0 as user_id,t1.EmpId as employee_id,t1.EmpCode as  emp_code,t1.EmpName as  emp_name,t1.company_id, IFNULL( t1.location_id,0) AS location_id,IFNULL( t1.dept_id,0) AS dept_id,
            '' as company_name, '' as location_name, '' as dept_name,
            0 as state_id, '' as state_name, isActive ,ifnull( t1.empstatus,3) as Emp_status
            from _tblEmpdata t1 ;           
            
        elseif (_proc_type =2) then
   
			select 0 as user_id,t1.EmpId as employee_id,t1.EmpCode as emp_code ,t1.EmpName as emp_name,t1.company_id, IFNULL( t1.location_id,0) AS location_id,IFNULL( t1.dept_id,0) AS dept_id,
            IFNULL(t4.company_name ,'') as company_name,IFNULL( t2.location_name,'') as location_name,IFNULL( t3.department_name,'') as dept_name,
            IFNULL( t2.state_id,0) as state_id, IFNULL( t2.state_name,'') as state_name, isActive,ifnull( t1.empstatus,3) as Emp_status
             from _tblEmpdata t1 
				left join 
				(select t1.location_id,t1.location_name, ifnull( t1.state_id,0) as  state_id ,ifnull( t2.name,'') as state_name
				from tbl_location_master t1 
				left join tbl_state t2 on t1.state_id =t2.state_id and t2.is_deleted=0
				where t1.is_active=1 ) t2 on t1.location_id=t2.location_id
				left join tbl_department_master t3 on t1.dept_id =t3.department_id and t3.is_active=1
                left join tbl_company_master t4 on t1.company_id=t4.company_id and t4.is_active=1;
                
        end if;   
        
        drop table _tblEmpdata;
        
END";
            migrationBuilder.Sql(createProcSql1);

            var createProcSql2 = @"CREATE PROCEDURE `proc_payroll_dynamic_repor`(IN _monthyear int,In _reportid int, in _company_id int)
BEGIN
   SET group_concat_max_len := 1000000;
   set @monthid=RIGHT(_monthyear, 2);
   set @monthName = 
   case when @monthid=1 then 'Jan'
       when @monthid=2 then 'Feb'
       when @monthid=3 then 'Mar'
       when @monthid=4 then 'Apr'
       when @monthid=5 then 'May'
       when @monthid=6 then 'Jun'
       when @monthid=7 then 'Jul'
       when @monthid=8 then 'Aug'
       when @monthid=9 then 'Sep'
       when @monthid=10 then 'Oct'
       when @monthid=11 then 'Nov'
       when @monthid=0 then 'Dec'
       END;
   set @_year=cast( (_monthyear /100) as UNSIGNED) ;
   set @fiscalyear='';
   if (@monthid>3) then
		set @fiscalyear=concat(@_year,'-',@_year+1);
   else 
         set @fiscalyear=concat(@_year-1,'-',@_year);
   end if;
   
   
	CREATE TEMPORARY TABLE _tblsalarydata(
		EmpId INT ,
        EmpCode varchar(50),
        EmpName varchar(200),
		component_id INT ,
		component_name varchar(100),
        component_value varchar(500),
        component_type int,
        display_order int
	);

 IF (SELECT 1 = 1 FROM tbl_report_master WHERE rpt_id=_reportid limit 1) THEN  
		insert into _tblsalarydata(EmpId,EmpCode,EmpName,component_id,component_name,component_value,component_type,display_order)
		SELECT t2.emp_id,t2.emp_code,t2.EmpName,ifnull(t1.component_id,0) ,t1.rpt_title,'-',t1.payroll_report_property,display_order  FROM
        tbl_rpt_title_master t1 inner join 
        (
        select t3.emp_id,t1.emp_code,concat( ifnull( t2.employee_first_name,''),' ',ifnull( t2.employee_middle_name,''),' ',ifnull(t2.employee_last_name,'')) as EmpName from tbl_employee_master t1 inner join tbl_emp_officaial_sec t2 on 
		t1.employee_id=t2.employee_id and t2.is_deleted=0 inner join tbl_payroll_process_status t3 on t3.emp_id=t1.employee_id and t3.is_deleted=0
		where t3.company_id=_company_id and t3.payroll_month_year=_monthyear
        ) t2  
        WHERE rpt_id=_reportid order by display_order;
        
        IF not exists(SELECT 1 = 1 FROM _tblsalarydata limit 1) THEN  
        insert into _tblsalarydata(EmpId,EmpCode,EmpName,component_id,component_name,component_value,component_type,display_order)
		SELECT 0,'-','-',ifnull(t1.component_id,0) ,t1.rpt_title,'-',t1.payroll_report_property,display_order  FROM
        tbl_rpt_title_master t1  WHERE rpt_id=_reportid order by display_order;
        end if;
        
        update _tblsalarydata 
        inner join tbl_salary_input t on  t.component_id=_tblsalarydata.component_id
        set _tblsalarydata.component_value = CASE
		WHEN _tblsalarydata.component_type =1 THEN t.values
		WHEN _tblsalarydata.component_type =2  THEN rate
        WHEN _tblsalarydata.component_type =3  THEN current_month_value
        WHEN _tblsalarydata.component_type =4  THEN arrear_value		
        WHEN _tblsalarydata.component_type =5  THEN  (current_month_value+arrear_value)		
		END where t.monthyear=_monthyear and company_id=_company_id and t.emp_id=_tblsalarydata.EmpId; 
        
        #select * from _tblsalarydata ;
        SELECT GROUP_CONCAT( concat(' MAX(IF (component_name=\'',`component_name`,'\' , component_value,null))  as  `',`component_name`,'` ')  SEPARATOR ', ')
		INTO @a FROM  (select  DISTINCT  component_name,display_order from _tblsalarydata  order by display_order) t ;
        
        
        SET @a = CONCAT('SELECT `empcode`,`empname`, \'',@monthName,'\' as `Month` , \'',@fiscalyear,'\' as `FinYear` , ', @a, ' FROM `_tblsalarydata` GROUP BY  `empcode`,`empname` order by `empcode`,`empname`');
        #select @a;
        PREPARE s FROM @a;
		EXECUTE s;
		DEALLOCATE PREPARE s;
        #select t2.emp_id,t2.emp_code,t2.EmpName from _tblsalarydata pivot;
        DROP TEMPORARY TABLE _tblsalarydata;
 END IF;

END";
            migrationBuilder.Sql(createProcSql2);

//            var createProcSql3 = @"CREATE PROCEDURE `proc_leave_adjust_report`(in proc_type int, in employee_ids MEDIUMTEXT, in from_date datetime, in to_date datetime)
//BEGIN


//select
//c.company_name as 'Company Name',
//a.emp_code as 'Employee Code',
//CONCAT(d.employee_first_name , ' ' , d.employee_middle_name ,' ', d.employee_last_name) as 'Employee Name',
//e.location_name as 'Location Name'
//,g.department_name as 'Department Name',
//h.designation_name as 'Designation Name',
//i.leave_type_name as 'Leave Type',
//   case 
//   when j.transaction_type = 1 then 'Add by System'
//   when j.transaction_type = 2 then 'Consumed'
//   when j.transaction_type = 3 then 'Expired'
//   when j.transaction_type = 4 then 'In Cash'
//   when j.transaction_type = 5 then 'Manual Add'
//   when j.transaction_type = 6 then 'Manual Delete'
//   when j.transaction_type = 7 then 'Add by System'
//   when j.transaction_type = 100 then 'Previous Leave Credit by System'
//   else 'NA' end as 'Adjustment Type',
//case when j.credit > 0 then 'Credit'
//   when j.dredit > 0 then 'Debit'
//   else ' ' end as 'Particular',
//   case when j.credit > 0 then j.credit
//     when j.dredit > 0 then j.dredit
//   else ' ' end as 'Value',
//j.created_by as 'created_by',
//j.remarks as 'Remark',
//DATE_FORMAT(j.entry_date, '%c-%b-%Y') as 'Action Date'
//from tbl_employee_master a
//left
//join tbl_user_master b on a.employee_id = b.employee_id
//left
//join tbl_company_master c on b.default_company_id = c.company_id
//left
//join tbl_emp_officaial_sec d on a.employee_id = d.employee_id
//left
//join tbl_location_master e on d.location_id = e.location_id
//left
//join tbl_department_master g on d.department_id = g.department_id
//inner
//join tbl_user_master um on a.employee_id = um.employee_id and um.user_id != 1
//left join tbl_emp_desi_allocation f on a.employee_id = f.employee_id
//left join tbl_designation_master h on f.desig_id = h.designation_id
//left join tbl_emp_personal_sec p on um.employee_id = p.employee_id and p.is_deleted = 0
//left join tbl_leave_ledger j on a.employee_id = j.e_id
//left join tbl_leave_type i on j.leave_type_id = i.leave_type_id
//where d.is_deleted = 0 and j.transaction_type in(5, 6)  and FIND_IN_SET(a.employee_id, employee_ids) and j.entry_date BETWEEN from_date AND to_date;
//            -- group by a.emp_code, i.leave_type_name;

//            END";
//            migrationBuilder.Sql(createProcSql3);

            var createProcSq4 = @"CREATE PROCEDURE `proc_get_emp_dump_data`(in proc_type int, in employee_ids MEDIUMTEXT)
BEGIN

select
c.company_name as 'Company Name',
a.emp_code as 'Employee Code',
case when a.is_active=0 then 'Inactive' when a.is_active=1 then 'Active' end as 'Status',
case
when d.salutation = 1  then 'Mr.'
when d.salutation = 2  then 'Mrs.'
when d.salutation = 3  then 'Miss.'
else 'NA' end as 'Salutation' ,
CONCAT(d.employee_first_name , ' ' , d.employee_middle_name ,' ', d.employee_last_name) as 'Employee Name',
case when em.final_approval=1 then
(SELECT emp_code FROM  tbl_employee_master d where d.employee_id = em.m_one_id and d.is_active=1 )
when em.final_approval=2 then
 (SELECT emp_code FROM  tbl_employee_master d where d.employee_id = em.m_two_id and d.is_active=1 )
when em.final_approval=3 then
(SELECT emp_code FROM  tbl_employee_master d where d.employee_id = em.m_three_id and d.is_active=1 )
else '' end as 'Manager Code',
case when em.final_approval=1 then
(select distinct CONCAT(d1.employee_first_name , ' ' , d1.employee_middle_name ,' ', d1.employee_last_name) from tbl_emp_officaial_sec d1 where d1.employee_id=em.m_one_id and d1.is_deleted=0   ) 
when em.final_approval=2 then
 (select distinct CONCAT(d1.employee_first_name , ' ' , d1.employee_middle_name ,' ', d1.employee_last_name) from tbl_emp_officaial_sec d1 where d1.employee_id=em.m_two_id and d1.is_deleted=0  )
when em.final_approval=3 then
 (select distinct CONCAT(d1.employee_first_name , ' ' , d1.employee_middle_name ,' ', d1.employee_last_name) from tbl_emp_officaial_sec d1 where d1.employee_id=em.m_three_id and d1.is_deleted=0 ) 
else '' end as 'Manager Name',
 d.card_number as 'Card Number',
case
when d.gender = 1  then 'Female'
when d.gender = 2  then 'Male'
when d.gender = 3  then 'Other'
else 'NA' end as 'Gender' ,
DATE_FORMAT(d.group_joining_date,'%d/%m/%Y') as 'Group Joining Date',
DATE_FORMAT(d.date_of_joining,'%d/%m/%Y') as 'Date of joining',
DATE_FORMAT(d.date_of_birth,'%d/%m/%Y') as 'Date of Birth',
DATE_FORMAT(d.department_date_of_joining,'%d/%m/%Y') as 'Department Joining Date',
i.religion_name as 'Religion',
case
when d.marital_status = 1  then 'Married'
when d.marital_status = 2  then 'Single'
when d.marital_status = 3  then 'Divorcy'
else 'NA' end as 'Marital Status' ,
d.official_email_id as 'Official Email Id',
e.location_name as 'Location Name'
,case when f.location_name IS NULL then 'NA' else f.location_name end as 'Sub Location'
,g.department_name as 'Department Name'
,case when h.sub_department_name IS NULL then 'NA' else h.sub_department_name end as 'Sub Department',
d.emp_father_name as 'Father Name',d.nationality as 'Nationality',
(case when d.is_ot_allowed=0 then 'NO' when d.is_ot_allowed=1 then 'Yes'end) as 'OT Allowed',
(case when d.is_comb_off_allowed=1 then 'Yes' when d.is_comb_off_allowed=0 then 'No' end) as 'Compoff Allwoed',
(case when d.punch_type=0 then 'Single Punch, Absent' when d.punch_type=1 then 'Single Punch, Present' when d.punch_type=2 then 'Punch Exempted' end) as 'Punch Type',
r.role_name as 'Role Name',
case when is_fixed_weekly_off=1 then 'Fixed' when is_fixed_weekly_off=2 then 'Dynamic' end as 'WeekOff',
case when current_employee_type=1 then 'Temporary' when current_employee_type=2 then 'Probation' when current_employee_type=3 then 'Confirmed' when current_employee_type=4 then 'Contract' when current_employee_type=10 then 'Notice'
when current_employee_type=99 then 'FNF' when current_employee_type=100 then 'Separated' end  as 'Current Employment Type',
 DATE_FORMAT(d.last_working_date,'%d/%m/%Y') as 'Last working Date',
case when p.blood_group=4 then 'AB+' when p.blood_group=5 then 'A-' when p.blood_group=6 then 'O-'
when p.blood_group=1 then 'A+' when p.blood_group=2 then 'O+' when p.blood_group=3 then 'B+'
when p.blood_group=7 then 'B-' when p.blood_group=8 then 'AB-'
else '' end 'Blood Group',
IFNULL(p.primary_contact_number,'') as 'Primary Contact No.',
IFNULL(p.secondary_contact_number,'') as 'Secondary Contact No.',
IFNULL(p.primary_email_id,'') as 'Primary Email ID',
IFNULL(p.secondary_email_id,'') as 'Secondary Email ID',
IFNULL(p.permanent_Address_line_one,'') as 'Permanent Address Line 1',
IFNULL(p.permanent_Address_line_two,'') as 'Permanent address Line 2',
IFNULL(p.permanent_pin_code,'') as 'Permanent PIN Code',
(select IFNULL(name,'') from tbl_country where country_id=p.permanent_country and is_deleted=0) as 'Permanent Country',
(select IFNULL(name,'') from tbl_state where state_id=p.permanent_state and is_deleted=0) as 'Permanent State',
(select IFNULL(name,'') from tbl_city where city_id=p.permanent_city and is_deleted=0) as 'Permanent City',
IFNULL(p.permanent_document_type,'')as 'Permanent Document Type',
IFNULL(p.corresponding_address_line_one,'') as 'Corresponding Address Line 1',
IFNULL(p.corresponding_address_line_two,'')as 'Corresponding Address Line 2',
IFNULL(p.corresponding_pin_code,'')as 'Corresponding PIN Code',
(select IFNULL(name,'') from tbl_country where country_id=p.corresponding_country and is_deleted=0) as 'Corresponding Country',
(select IFNULL(name,'') from tbl_state where state_id=p.corresponding_state and is_deleted=0) as 'Corresponding State',
(select IFNULL(name,'') from tbl_city where city_id=p.corresponding_city and is_deleted=0) as 'Corresponding City',
IFNULL(p.corresponding_document_type,'')as 'Corresponding Document Type',
IFNULL(p.emergency_contact_name,'')as 'Emergency Contact Name',
IFNULL(p.emergency_contact_relation,'')as 'Emergency Contact Relation',
IFNULL(p.emergency_contact_mobile_number,'')as 'Emergency Contact Mobile No.',
IFNULL(p.emergency_contact_line_one,'')as 'Emergency Address Line 1',
IFNULL(p.emergency_contact_line_two,'')as 'Emergency Address Line 2',
IFNULL(p.emergency_contact_pin_code,'')as 'Emergency PIN Code',
(select IFNULL(name,'') from tbl_country where country_id=p.emergency_contact_country and is_deleted=0)as 'Emergency Country',
(select IFNULL(name,'') from tbl_state where state_id=p.emergency_contact_state and is_deleted=0) as 'Emergency State',
(select IFNULL(name,'') from tbl_city where city_id=p.emergency_contact_city and is_deleted=0) as 'Emergency City',
IFNULL(p.emergency_contact_document_type,'') as 'Emergency Document Type',
IFNULL(pf.uan_number,'') as 'UAN Number',
case when pf.is_pf_applicable=1 then 'Yes' when pf.is_pf_applicable=0 then 'No' else '' end as 'PF Applicable',
IFNULL(pf.pf_number,'') as 'PF Number',
case when pf.pf_group=1 then 'Min Slab' when pf.pf_group=2 then '12% of Basic' else '' end as 'PF Group',
IFNULL(pf_celing,'') as 'PF Ceiling',
case when pf.is_vpf_applicable=1 then 'Yes' when pf.is_vpf_applicable=0 then 'No' else '' end as 'IS VPF Applicable',
case when vpf_Group=1 then 'Fixed Amount' when vpf_Group=2 then 'Basic Percentage' else '' end 'VPF Group',
IFNULL(vpf_amount,'') as 'VPF Amount',
case when pf.is_eps_applicable=1 then 'YES' when pf.is_eps_applicable=0 then 'NO' else '' end as 'EPS Applicable',
case when esi.is_esic_applicable=1 then 'YES' when esi.is_esic_applicable=0 then 'NO' else '' end as 'IS ESIC Applicable',
IFNULL(esic_number,'') as 'ESIC No.',
IFNULL(pan.pan_card_name,'') as 'PAN Card Name',
IFNULL(pan.pan_card_number,'') as 'PAN Card No.',
IFNULL(ad.aadha_card_name,'') as 'Aadhar Card Name',
IFNULL(ad.aadha_card_number,'') as 'Aadhar Card No.',
IFNULL(bm.bank_name,'') as 'Bank Name',
IFNULL(bn.bank_acc,'') as 'Account No.',
IFNULL(bn.ifsc_code,'') as 'IFSC Code',
IFNULL(bn.branch_name,'') as 'Branch Name',
case when payment_mode=1 then 'Bank Transfer' 
when payment_mode=2 then 'Cheque' 
when payment_mode=3 then 'Case' 
when payment_mode=4 then 'Demand Draft' 
else '' end as 'Payment Mode',
case when year(d.confirmation_date) < year(d.date_of_joining) then ''
else DATE_FORMAT(d.confirmation_date,'%d/%m/%Y') end as 'Confirmation Date',
gr.grade_name,de.designation_name,
case when d.notice_period =null then '' else d.notice_period end as notice_period,
case when d.employement_type = 1 then 'Full Time'
when d.employement_type=2 then 'Part Time'
when d.employement_type=3 then 'Contract Time'
end as emp_type

from tbl_employee_master a
inner join tbl_user_master b on a.employee_id=b.employee_id and b.user_id!=1 
left join tbl_company_master c on b.default_company_id = c.company_id and c.is_active=1
left join tbl_emp_officaial_sec d on a.employee_id = d.employee_id and d.is_deleted=0
left join tbl_location_master e on d.location_id = e.location_id and e.is_active=1
left join tbl_sub_location_master f on d.sub_location_id = f.sub_location_id and f.is_active=1
left join tbl_department_master g on d.department_id = g.department_id  and g.is_active=1
left join tbl_sub_department_master h on d.sub_dept_id = h.sub_department_id  and h.is_active=1
left join tbl_religion_master i on d.religion_id = i.religion_id  and i.is_active=1
left join tbl_role_master r on d.user_type=r.role_id and r.is_active=1
left join tbl_emp_personal_sec p on b.employee_id=p.employee_id and p.is_deleted=0
left join tbl_emp_pf_details pf on b.employee_id=pf.employee_id and pf.is_deleted=0
left join tbl_emp_esic_details esi on esi.employee_id=pf.employee_id and esi.is_deleted=0
left join tbl_emp_pan_details pan on b.employee_id=pan.employee_id and pan.is_deleted=0
left join tbl_emp_adhar_details ad on pan.employee_id=ad.employee_id and ad.is_deleted=0
left join tbl_emp_bank_details bn on pan.employee_id=bn.employee_id and bn.is_deleted=0
left join tbl_bank_master bm on bn.bank_id=bm.bank_id and bm.is_deleted=0
left join tbl_emp_manager em on a.employee_id=em.employee_id and em.is_deleted=0
left join tbl_emp_grade_allocation eg on a.employee_id=eg.employee_id  
left join tbl_grade_master gr on eg.grade_id=gr.grade_id and gr.is_active=1
left join tbl_emp_desi_allocation ed on a.employee_id=ed.employee_id  
left join tbl_designation_master de on ed.desig_id=de.designation_id and de.is_active=1;

END";
            migrationBuilder.Sql(createProcSq4);


            var createProcSql5 = @"CREATE PROCEDURE `proc_get_emp_data_dir`(in _proc_type int)
BEGIN
drop temporary table if exists _tblEmpdata;
	create TEMPORARY TABLE _tblEmpdata(
		EmpId INT ,
        EmpCode varchar(50),
        EmpName varchar(200),
        company_id int,        
        location_id int,        
        dept_id int  ,
        empstatus int,
        mobileno varchar(10),
        email varchar(50),
        desig_name varchar(200),
        emp_img varchar(1000),
        dob datetime,
        doj datetime
		);
       
      	
				  insert _tblEmpdata(EmpId,EmpCode,EmpName,company_id,location_id,dept_id,empstatus
                  ,mobileno,email,desig_name,emp_img,dob,doj
                  )
                  select distinct t1.employee_id,t1.emp_code,concat(ifnull( t3.employee_first_name,''),' ',ifnull( t3.employee_middle_name,''),' ',ifnull( t3.employee_last_name,'')) ,
                  t2.company_id,t3.location_id,t3.department_id,ifnull(t3.current_employee_type,'')
                  ,t4.primary_contact_number,t4.primary_email_id,t6.designation_name,
                  t3.employee_photo_path,t3.date_of_birth,t3.date_of_joining
                  from tbl_employee_master t1 
                  inner join tbl_employee_company_map t2 on t1.employee_id=t2.employee_id 
                  inner join tbl_emp_officaial_sec t3 on t1.employee_id=t3.employee_id
                  inner join tbl_emp_personal_sec t4 on t1.employee_id=t4.employee_id
                  inner join tbl_emp_desi_allocation t5 on t1.employee_id=t5.employee_id
				  inner join tbl_designation_master t6 on t5.desig_id=t6.designation_id
                  where
                  t3.current_employee_type < 100 and
                  t1.is_active = 1 and t2.is_deleted=0 and  t3.is_deleted = 0 and t2.is_default=1 and
                  t4.is_deleted=0 and t3.current_employee_type not in (99,100);
     
        if (_proc_type =2) then
			select distinct 0 as user_id,t1.EmpId as employee_id,t1.EmpCode as emp_code ,t1.EmpName as emp_name,t1.company_id, IFNULL( t1.location_id,0) AS location_id,IFNULL( t1.dept_id,0) AS dept_id,
            IFNULL(t4.company_name ,'') as company_name,IFNULL( t2.location_name,'') as location_name,IFNULL( t3.department_name,'') as dept_name,
            IFNULL( t2.state_id,0) as state_id, IFNULL( t2.state_name,'') as state_name,
            ifnull( t1.empstatus,3) as Emp_status,t1.mobileno,t1.email,t1.desig_name,t1.emp_img
             from _tblEmpdata t1 
				left join 
				(select t1.location_id,t1.location_name, ifnull( t1.state_id,0) as  state_id ,ifnull( t2.name,'') as state_name
				from tbl_location_master t1 
				left join tbl_state t2 on t1.state_id =t2.state_id and t2.is_deleted=0
				where t1.is_active=1 ) t2 on t1.location_id=t2.location_id
				left join tbl_department_master t3 on t1.dept_id =t3.department_id and t3.is_active=1
                left join tbl_company_master t4 on t1.company_id=t4.company_id and t4.is_active=1;
              
              
		elseif (_proc_type =3) then #BIRTHDAY LIST
              select distinct 0 as user_id,t1.EmpId as employee_id,t1.EmpCode as emp_code ,t1.EmpName as emp_name,t1.company_id, IFNULL( t1.location_id,0) AS location_id,IFNULL( t1.dept_id,0) AS dept_id,
            IFNULL(t4.company_name ,'') as company_name,IFNULL( t2.location_name,'') as location_name,IFNULL( t3.department_name,'') as dept_name,
            IFNULL( t2.state_id,0) as state_id, IFNULL( t2.state_name,'') as state_name,
            ifnull( t1.empstatus,3) as Emp_status,t1.mobileno,t1.email,t1.desig_name,t1.emp_img,t1.dob,t1.doj
             from _tblEmpdata t1 
				left join 
				(select t1.location_id,t1.location_name, ifnull( t1.state_id,0) as  state_id ,ifnull( t2.name,'') as state_name
				from tbl_location_master t1 
				left join tbl_state t2 on t1.state_id =t2.state_id and t2.is_deleted=0
				where t1.is_active=1 ) t2 on t1.location_id=t2.location_id
				left join tbl_department_master t3 on t1.dept_id =t3.department_id and t3.is_active=1
                left join tbl_company_master t4 on t1.company_id=t4.company_id and t4.is_active=1
                where CONCAT(year(CURDATE()), ' - ' , MONTH(t1.dob), ' - ' ,day(t1.dob))
                between CURDATE() and DATE_ADD(CURDATE(), INTERVAL 30 DAY)
                ORDER by Date_Format(t1.dob, '%m/%d');

            elseif(_proc_type = 4) then #ANNIVERSARY LIST
              select distinct 0 as user_id,t1.EmpId as employee_id,t1.EmpCode as emp_code ,t1.EmpName as emp_name,t1.company_id, IFNULL(t1.location_id, 0) AS location_id, IFNULL( t1.dept_id, 0) AS dept_id,
              IFNULL(t4.company_name, '') as company_name,IFNULL(t2.location_name, '') as location_name,IFNULL(t3.department_name, '') as dept_name,
            IFNULL(t2.state_id, 0) as state_id, IFNULL(t2.state_name, '') as state_name,
            ifnull(t1.empstatus, 3) as Emp_status,t1.mobileno,t1.email,t1.desig_name,t1.emp_img,t1.dob,t1.doj
             from _tblEmpdata t1

                left join
                (select t1.location_id, t1.location_name, ifnull(t1.state_id,0) as state_id ,ifnull(t2.name, '') as state_name

                from tbl_location_master t1

                left
                join tbl_state t2 on t1.state_id = t2.state_id and t2.is_deleted = 0

                where t1.is_active = 1 ) t2 on t1.location_id = t2.location_id

                left join tbl_department_master t3 on t1.dept_id = t3.department_id and t3.is_active = 1
                left join tbl_company_master t4 on t1.company_id = t4.company_id and t4.is_active = 1
                where DATE_ADD(t1.doj, INTERVAL 1 YEAR) between CURDATE() and DATE_ADD(CURDATE(), INTERVAL 30 DAY)
                order by t1.doj;
            end if;

            drop table _tblEmpdata;

            END";
            migrationBuilder.Sql(createProcSql5);


            var createProcSql6 = @"CREATE PROCEDURE `proc_attendance_inout_report`(in employee_ids MEDIUMTEXT, in from_date datetime, in to_date datetime)
BEGIN

DROP TEMPORARY TABLE IF EXISTS temp_emp_data,temp_emp_details,temp_emp_att,temp_emp_attendance;
 
CREATE TEMPORARY TABLE temp_emp_data
select distinct
a.emp_code
,CONCAT(d.employee_first_name , ' ' , d.employee_middle_name ,' ', d.employee_last_name) as emp_name
,c.company_name as company_name
,IFNULL( e.location_name,'') as branch_name
,g.department_name as dept_name
,DATE_FORMAT(d.date_of_joining,'%d/%m/%Y') as doj
,sh.shift_name as shift_type
,CONCAT(DATE_FORMAT(sh.punch_in_time,'%h:%i'), '-', DATE_FORMAT(sh.punch_out_time,'%h:%i')) as shift_timing
,CONCAT(sh.maximum_working_hours,':',sh.maximum_working_minute) as shift_hrs,dat.attendance_dt
,dat.day_status 
,dat.is_weekly_off
,dat.is_holiday
,dat.out_time
,dat.in_time
,
case 
when dat.is_weekly_off=1 then 'WO'
when dat.is_holiday=1 then 'Holiday'
when dat.day_status=1 then 'Present'
when dat.day_status=2 then 'Absent'
when dat.day_status=3 then 'Leave'
when dat.day_status=4 then 'Half Day Present Half Day Absent'
when dat.day_status=5 then 'Half Day Present Half Day Leave'
when dat.day_status=6 then 'Half Day Leave Half Day Absent'
#when dat.day_status in (4,5,6) then 'Half Day Present'
when dat.is_outdoor=1 then 'Outdoor' else 'NA'
end as 'Status' 

from tbl_employee_master a
left join tbl_user_master b on a.employee_id = b.employee_id
left join tbl_company_master c on b.default_company_id = c.company_id
left join tbl_emp_officaial_sec d on a.employee_id = d.employee_id
left join tbl_location_master e on d.location_id = e.location_id
left join tbl_department_master g on d.department_id = g.department_id
left join tbl_daily_attendance dat on a.employee_id=dat.emp_id
left join tbl_shift_details sh on dat.shift_id=sh.shift_id
where d.is_deleted = 0 and sh.is_deleted=0 and a.employee_id NOT IN (476,477,478,481,482,479,480,1,378,454)
and FIND_IN_SET (a.employee_id,employee_ids) and dat.attendance_dt between from_date and to_date
 order by dat.attendance_dt;
 
create temporary table temp_emp_details
 select emp_code,emp_name,company_name,branch_name,dept_name,doj,shift_type,shift_timing,shift_hrs,attendance_dt,
 intime,outtime,intime1,outtime1,Status
 from (
 select emp_code,emp_name,company_name,branch_name,dept_name,doj,shift_type,shift_timing,shift_hrs,attendance_dt,out_time,in_time
 ,case 
when is_weekly_off=1 or is_holiday=1 or day_status in (2,3) then ''
else DATE_FORMAT(in_time,'%h:%i %p') end as intime
,case
when is_weekly_off=1 or is_holiday=1 or day_status in (2,3) then ''
else DATE_FORMAT(out_time,'%h:%i %p') end as outtime
 ,case 
when is_weekly_off=1 or is_holiday=1 or day_status in (2,3) then 0
else case when IFNULL(DATE_FORMAT(in_time,'%T'),'')='' then 0 else DATE_FORMAT(in_time,'%T') end end as intime1
,case 
when is_weekly_off=1 or is_holiday=1 or day_status in (2,3) then 0
else case when IFNULL(DATE_FORMAT(out_time,'%T'),'')='' then 0 else DATE_FORMAT(out_time,'%T') end end as outtime1
,Status
from temp_emp_data) tt order by tt.attendance_dt;
 
create temporary table temp_emp_att
select * from temp_emp_details;
 
create temporary table temp_emp_attendance
select distinct tmp.emp_code,tmp.emp_name,tmp.company_name,tmp.branch_name,tmp.dept_name,tmp.doj,tmp.shift_type,tmp.shift_timing,
tmp.shift_hrs,
(select  concat(DATE_FORMAT(t.attendance_dt, '%Y-%m-%d'),';',t.intime,'|',t.outtime,'|',
#case when outtime1=0 and intime1=0 then '' else timediff(outtime1,intime1) end,
left(replace(outtime1,':','.'),5),'|',left(replace(intime1,':','.'),5),'|', t.Status) 
 from temp_emp_att t where t.emp_code=tmp.emp_code and t.attendance_dt=tmp.attendance_dt 
 group by t.emp_code order by t.attendance_dt) as empdata,tmp.attendance_dt
 from temp_emp_details tmp order by tmp.emp_code ;
 
 SELECT t1.emp_code,t1.emp_name,t1.company_name,t1.branch_name,t1.dept_name,t1.doj,t1.shift_type,t1.shift_timing,
t1.shift_hrs, GROUP_CONCAT(t1.empdata) AS empdata
FROM temp_emp_attendance t1
GROUP BY t1.emp_code order by t1.attendance_dt;
 
 drop table temp_emp_data;
 drop table temp_emp_details;
 drop table temp_emp_att;
 drop table temp_emp_attendance;
    
END";
            migrationBuilder.Sql(createProcSql6);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
