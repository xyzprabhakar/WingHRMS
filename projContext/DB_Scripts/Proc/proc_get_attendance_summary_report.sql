-----------------------------------------------------------------------------------------------------------------
-------------------------modified by navneet on 18 March 2021----------------------------------------------------
DELIMITER $$
CREATE DEFINER=`sa`@`%` PROCEDURE `proc_get_attendance_summary_report`(in proc_type int, in employee_ids MEDIUMTEXT, v_from_date datetime,v_to_date datetime)
BEGIN

Select distinct
c.company_name as 'Company Name',
a.employee_id as E_id,
a.emp_code as'Employee Code',
-- case when a.is_active=0 then 'Inactive' when a.is_active=1 then 'Active' end 'Status',
CONCAT(d.employee_first_name , " " , d.employee_middle_name ," ", d.employee_last_name) as 'Employee Name',
e.location_name as 'Location Name',
g.department_name as 'Department Name',
de.designation_name as 'Designation Name',
case when m.final_approval=1 then
(SELECT emp_code FROM  tbl_employee_master d where d.employee_id = m.m_one_id and d.is_active=1 )
when m.final_approval=2 then
 (SELECT emp_code FROM  tbl_employee_master d where d.employee_id = m.m_two_id and d.is_active=1 )
when m.final_approval=3 then
(SELECT emp_code FROM  tbl_employee_master d where d.employee_id = m.m_three_id and d.is_active=1 )
else '' end as 'Manager Code',
case when m.final_approval=1 then
(select distinct CONCAT(d1.employee_first_name , ' ' , d1.employee_middle_name ,' ', d1.employee_last_name) from tbl_emp_officaial_sec d1 where d1.employee_id=m.m_one_id and d1.is_deleted=0   ) 
when m.final_approval=2 then
 (select distinct CONCAT(d1.employee_first_name , ' ' , d1.employee_middle_name ,' ', d1.employee_last_name) from tbl_emp_officaial_sec d1 where d1.employee_id=m.m_two_id and d1.is_deleted=0  )
when m.final_approval=3 then
 (select distinct CONCAT(d1.employee_first_name , ' ' , d1.employee_middle_name ,' ', d1.employee_last_name) from tbl_emp_officaial_sec d1 where d1.employee_id=m.m_three_id and d1.is_deleted=0 ) 
else '' end as 'Manager Name',
cast(d.date_of_joining as Date) as 'Date of joining',

-- No_of_working_Days
datediff(v_to_date,v_from_date) + 1  as 'No_of_working_Days',

-- No_of_Days_worked
((datediff(v_to_date,v_from_date)+1) -
((Select count(*) from (Select * from tbl_daily_attendance ta1 where 
cast(ta1.attendance_dt as Date) between Cast(v_from_date as Date ) and  Cast(v_to_date as Date)
 and ta1.emp_id=a.employee_id and ta1.day_status=2 ) a) +
 ((Select count(*) from (Select * from tbl_daily_attendance ta2 where 
cast(ta2.attendance_dt  as Date) between Cast(v_from_date as Date ) and Cast(v_to_date as Date) 
and ta2.emp_id=a.employee_id and ta2.day_status in (4,6)) b)*0.5)
 )) as 'No_of_days_worked',

-- No_of_Days_Present

((Select count(*) from (Select * from tbl_daily_attendance ta1 where 
cast(ta1.attendance_dt as Date) between Cast(v_from_date as Date ) and  Cast(v_to_date as Date)
and ta1.emp_id=a.employee_id and ta1.day_status=1 ) a) +
 ((Select count(*) from (Select * from tbl_daily_attendance ta2 where 
cast(ta2.attendance_dt  as Date) between Cast(v_from_date as Date ) and Cast(v_to_date as Date) 
and ta2.emp_id=a.employee_id and ta2.day_status in (4,5)) b)*0.5)
 ) as 'No_of_days_Present',


-- No_of_Week_off
(Select count(*) from (Select * from tbl_daily_attendance ta1 where 
cast(ta1.attendance_dt as Date) between Cast(v_from_date as Date ) and  Cast(v_to_date as Date)
 and ta1.emp_id=a.employee_id and  is_weekly_off=1 ) a) as 'No_of_Week_off',
 
 -- No_of_Holidays
(Select count(*) from (Select * from tbl_daily_attendance ta1 where 
cast(ta1.attendance_dt as Date) between Cast(v_from_date as Date ) and  Cast(v_to_date as Date)
and ta1.emp_id=a.employee_id and  is_holiday=1 ) a) as 'No_of_Holidays',
 
 -- No_of_leaves_taken
((Select count(*) from (Select * from tbl_daily_attendance ta1 where 
cast(ta1.attendance_dt as Date) between Cast(v_from_date as Date ) and  Cast(v_to_date as Date)
and ta1.emp_id=a.employee_id and ta1.day_status=3 ) a) +
 ( (Select count(*) from (Select * from tbl_daily_attendance ta2 where 
cast(ta2.attendance_dt  as Date) between Cast(v_from_date as Date ) and Cast(v_to_date as Date) 
 and ta2.emp_id=a.employee_id and ta2.day_status in (5,6)) b)*0.5)
 ) as 'No_of_leaves_taken',
 
 -- 'No_of_Half_days_leave_Applied'
( (Select count(*) from (Select * from tbl_daily_attendance ta2 where 
cast(ta2.attendance_dt  as Date) between Cast(v_from_date as Date ) and Cast(v_to_date as Date) 
 and ta2.emp_id=a.employee_id and ta2.day_status in (5,6)) b)) as 'No_of_Half_days_leave_Applied',

-- No_of_days_worked_less_than_8_hours
(Select count(*) from (Select * from tbl_daily_attendance ta1 where 
cast(ta1.attendance_dt as Date) between Cast(v_from_date as Date ) and  Cast(v_to_date as Date)
and ta1.emp_id=a.employee_id and  (TIMESTAMPDIFF(minute, ta1.in_time, ta1.out_time) < 480 and TIMESTAMPDIFF(minute, ta1.in_time, ta1.out_Time) > 0 ) ) a ) as 'No_of_days_worked_less_that_8_hours',

-- No_of_days_worked_for_more_than_8_hours_but_less_than_9_hours

(Select count(*) from (Select * from tbl_daily_attendance ta1 where 
cast(ta1.attendance_dt as Date) between Cast(v_from_date as Date ) and  Cast(v_to_date as Date)
 and ta1.emp_id=a.employee_id and  (TIMESTAMPDIFF(minute, ta1.in_time, ta1.out_Time) >= 480 and TIMESTAMPDIFF(minute, ta1.in_time, ta1.out_Time) < 540) ) a ) as 'No_of_days_worked_for_more_than_8_hours_but_less_than_9_hours',

-- no_of_day_applied_on_Date_Outdoor
(Select count(*) from (Select * from tbl_daily_attendance ta1 where 
cast(ta1.attendance_dt as Date) between Cast(v_from_date as Date ) and  Cast(v_to_date as Date)
and ta1.emp_id=a.employee_id and  is_outdoor=1 ) a) as 'No_of_day_applied_on_Date_Outdoor',

-- No_of_Regularised_Days
(Select count(*) from (Select * from tbl_daily_attendance ta1 where 
cast(ta1.attendance_dt as Date) between Cast(v_from_date as Date ) and  Cast(v_to_date as Date)
 and ta1.emp_id=a.employee_id and  is_regularize=1 ) a) as  'No_of_Regularised_Days',


-- Comp_off_Availed
0 as 'Comp_off_Availed',

-- Optional_Holiday_Availed 
0 as 'Optional_Holiday_Availed',

-- No_Of_absent_day_Unapplied_leaves
((Select count(*) from (Select * from tbl_daily_attendance ta1 where 
cast(ta1.attendance_dt as Date) between Cast(v_from_date as Date ) and  Cast(v_to_date as Date)
and ta1.emp_id=a.employee_id and ta1.day_status=2 ) a) +
 ( (Select count(*) from (Select * from tbl_daily_attendance ta2 where 
cast(ta2.attendance_dt  as Date) between Cast(v_from_date as Date ) and Cast(v_to_date as Date) 
 and ta2.emp_id=a.employee_id and ta2.day_status in (4,6)) b)*0.5)
 ) as 'No_Of_absent_day_Unapplied_leaves',

-- Average_Working_hours
(
(Select (sum(worked_hour)/60) from (Select (TIMESTAMPDIFF(minute, ta1.in_time,ta1.out_time)) as worked_hour from tbl_daily_attendance ta1 where cast(ta1.attendance_dt as Date) between Cast(v_from_date as Date ) and  Cast(v_to_date as Date) and ta1.emp_id=a.employee_id and ta1.day_status in(1,4,5) ) a)
 /
 ((Select count(*) from (Select * from tbl_daily_attendance ta1 where 
cast(ta1.attendance_dt as Date) between Cast(v_from_date as Date ) and  Cast(v_to_date as Date)
and ta1.emp_id=a.employee_id and ta1.day_status=1) a) +
 ((Select count(*) from (Select * from tbl_daily_attendance ta2 where 
cast(ta2.attendance_dt  as Date) between Cast(v_from_date as Date ) and Cast(v_to_date as Date) 
and ta2.emp_id=a.employee_id and ta2.day_status in (4,5)) b)*0.5) )
 ) as 'Average_Working_hours'

from tbl_employee_master a
join tbl_user_master b on a.employee_id = b.employee_id
left join tbl_emp_manager m on a.employee_id = m.employee_id 
INNER join tbl_company_master c on b.default_company_id = c.company_id
left join tbl_emp_officaial_sec d on a.employee_id = d.employee_id
INNER join tbl_emp_desi_allocation da on da.employee_id = d.employee_id
INNER join tbl_designation_master de on da.desig_id = de.designation_id
INNER join tbl_location_master e on d.location_id = e.location_id
INNER join tbl_department_master g on d.department_id = g.department_id
where d.is_deleted = 0 and m.is_deleted=0 and FIND_IN_SET (a.employee_id,employee_ids) ;

END$$
DELIMITER ;

----------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------

----------------------------------------------------------------------------------------
-------------------------modified by --- on [] ---------------------------