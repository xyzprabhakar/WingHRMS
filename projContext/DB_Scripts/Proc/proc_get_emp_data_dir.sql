CREATE  PROCEDURE `proc_get_emp_data_dir`(in _proc_type int)
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
        mobileno varchar(20),
        official_contact_no varchar(20),
        email varchar(200),
        Official_email_id varchar(200),
        desig_name varchar(200),
        emp_img varchar(1000),
        dob datetime,
        doj datetime
		);
       
      	
				  insert _tblEmpdata(EmpId,EmpCode,EmpName,company_id,location_id,dept_id,empstatus
                  ,mobileno,email,desig_name,emp_img,dob,doj,Official_email_id,official_contact_no
                  )
                  select distinct t1.employee_id,t1.emp_code,concat(ifnull( t3.employee_first_name,''),' ',ifnull( t3.employee_middle_name,''),' ',ifnull( t3.employee_last_name,'')) ,
                  t2.company_id,t3.location_id,t3.department_id,ifnull(t3.current_employee_type,'')
                  ,t4.primary_contact_number,t4.primary_email_id,t6.designation_name,
                  t3.employee_photo_path,t3.date_of_birth,t3.date_of_joining, ifnull( t3.Official_email_id,''), ifnull( t3.official_contact_no,'')
                  from tbl_employee_master t1 
                  left join tbl_employee_company_map t2 on t1.employee_id=t2.employee_id 
                  left join tbl_emp_officaial_sec t3 on t1.employee_id=t3.employee_id
                  left join tbl_emp_personal_sec t4 on t1.employee_id=t4.employee_id
                  left join tbl_emp_desi_allocation t5 on t1.employee_id=t5.employee_id
				  left join tbl_designation_master t6 on t5.desig_id=t6.designation_id
                  where                  
                  t1.is_active = 1 and IFNULL(t2.is_deleted,0)=0 and  IFNULL(t3.is_deleted ,0)= 0 and IFNULL( t2.is_default,0)=1 and
                  IFNULL(t4.is_deleted,0)=0;
     
        if (_proc_type =2) then
			select distinct 0 as user_id,t1.EmpId as employee_id,t1.EmpCode as emp_code ,t1.EmpName as emp_name,t1.company_id, IFNULL( t1.location_id,0) AS location_id,IFNULL( t1.dept_id,0) AS dept_id,
            IFNULL(t4.company_name ,'') as company_name,IFNULL( t2.location_name,'') as location_name,IFNULL( t3.department_name,'') as dept_name,
            IFNULL( t2.state_id,0) as state_id, IFNULL( t2.state_name,'') as state_name,
            ifnull( t1.empstatus,3) as Emp_status,t1.mobileno,t1.email,t1.desig_name,t1.emp_img,Official_email_id,official_contact_no
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
            ifnull( t1.empstatus,3) as Emp_status,t1.mobileno,t1.email,t1.desig_name,t1.emp_img,t1.dob,t1.doj,Official_email_id,official_contact_no
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
            ifnull(t1.empstatus, 3) as Emp_status,t1.mobileno,t1.email,t1.desig_name,t1.emp_img,t1.dob,t1.doj,Official_email_id,official_contact_no
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

            END