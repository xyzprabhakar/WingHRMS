

select * from tbl_employee

select * from tbl_department

select * from tbl_status

select top 10 * from tbl_ticket where ticket_id=1617  order by created_date desc

select top 10 * from tbl_sub_ticket where sbticket_id=1617

select   * from tbl_workflow where ticket_id=1617 and  sub_ticket_id<>0 and ticket_id=110646




select * from [dbo].[tbl_epa_kpi_objective_setting] where kpi_objective_id=3976

select * from [dbo].[tbl_epa_kpi_objective_detail] where kpi_objective_id=3976

select * from [dbo].[tbl_epa_kra_detail]  where kpi_objective_id=3976


select * from [dbo].[tbl_cc_audit]

select * from [dbo].[tbl_cc_audit_detail]


select * from tbl_request_type

select * from tbl_request_sub_type

select * from tbl_request_subject



select * from [data_security].[tbl_application_login_otp]

select * from [data_security].[tbl_password_management]

