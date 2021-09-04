
----------------------------------------Navneet 16 April 2021-------------------------
CREATE DEFINER=`sa`@`%` PROCEDURE `proc_payroll_dynamic_repor`(IN _monthyear int,In _reportid int, in _company_id int)
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
		WHEN _tblsalarydata.component_name in ("PF Employee","Arrear PF Employee","ESIC Employee","Arrear ESIC Employee") THEN 0
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

END

-----------------------------------------------------------------------------------------------------------------------------