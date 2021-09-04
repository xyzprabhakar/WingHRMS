
Drop TABLE `tbl_emp_separation` ;
CREATE TABLE `tbl_emp_separation` (
  `sepration_id` int NOT NULL AUTO_INCREMENT,
  `emp_id` int NOT NULL,
  `company_id` int NOT NULL,
  `resignation_dt` datetime NOT NULL,
  `req_relieving_date` datetime DEFAULT NULL,
  `req_notice_days` int DEFAULT NULL,
  `diff_notice_days` int DEFAULT NULL,
  `policy_relieving_dt` datetime DEFAULT NULL,
  `is_relieving_dt_change` int DEFAULT NULL,
  `last_wrking_dt` datetime DEFAULT NULL,
  `req_reason` text,
  `req_remarks` text,
  `approver1_id` int DEFAULT NULL,
  `is_approved1` int NOT NULL,
  `app1_remarks` text,
  `app1_dt` datetime NOT NULL,
  `approver2_id` int DEFAULT NULL,
  `is_approved2` int NOT NULL,
  `app2_remarks` text,
  `app2_dt` datetime NOT NULL,
  `apprver3_id` int DEFAULT NULL,
  `is_approved3` int NOT NULL,
  `app3_remarks` text,
  `app3_dt` datetime NOT NULL,
  `admin_id` int DEFAULT NULL,
  `is_admin_approved` int NOT NULL,
  `admin_remarks` text,
  `admin_dt` datetime NOT NULL,
  `is_final_approve` int NOT NULL,
  `final_relieve_dt` datetime NOT NULL,
  `is_cancel` int NOT NULL,
  `cancel_remarks` text,
  `cancelation_dt` datetime NOT NULL,
  `is_deleted` int NOT NULL,
  `created_by` int NOT NULL,
  `created_dt` datetime NOT NULL,
  `modified_by` int NOT NULL,
  `modified_dt` datetime NOT NULL,
  `notice_day` int DEFAULT NULL,
  `withdrawal_type` varchar(250) DEFAULT NULL,
  `salary_process_type` int DEFAULT NULL,
  `gratuity` int DEFAULT NULL,
  `is_withdrawal` int NOT NULL DEFAULT '0',
  `is_no_due_cleared` int NOT NULL DEFAULT '0',
  `is_kt_transfered` int NOT NULL DEFAULT '0',
  `ref_doc_path` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`sepration_id`),
  KEY `FK_tbl_emp_separation_tbl_employee_master_admin_id` (`admin_id`),
  KEY `FK_tbl_emp_separation_tbl_employee_master_approver1_id` (`approver1_id`),
  KEY `FK_tbl_emp_separation_tbl_employee_master_approver2_id` (`approver2_id`),
  KEY `FK_tbl_emp_separation_tbl_employee_master_apprver3_id` (`apprver3_id`),
  KEY `FK_tbl_emp_separation_tbl_company_master_company_id` (`company_id`),
  KEY `FK_tbl_emp_separation_tbl_employee_master_emp_id` (`emp_id`),
  CONSTRAINT `FK_tbl_emp_separation_tbl_company_master_company_id` FOREIGN KEY (`company_id`) REFERENCES `tbl_company_master` (`company_id`) ON DELETE CASCADE,
  CONSTRAINT `FK_tbl_emp_separation_tbl_employee_master_admin_id` FOREIGN KEY (`admin_id`) REFERENCES `tbl_employee_master` (`employee_id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_tbl_emp_separation_tbl_employee_master_approver1_id` FOREIGN KEY (`approver1_id`) REFERENCES `tbl_employee_master` (`employee_id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_tbl_emp_separation_tbl_employee_master_approver2_id` FOREIGN KEY (`approver2_id`) REFERENCES `tbl_employee_master` (`employee_id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_tbl_emp_separation_tbl_employee_master_apprver3_id` FOREIGN KEY (`apprver3_id`) REFERENCES `tbl_employee_master` (`employee_id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_tbl_emp_separation_tbl_employee_master_emp_id` FOREIGN KEY (`emp_id`) REFERENCES `tbl_employee_master` (`employee_id`)
) ;


Drop TABLE `tbl_approved_emp_separation_cancellation` ;
CREATE TABLE `tbl_approved_emp_separation_cancellation` (
  `pkid_AppEmpSepCancel` int NOT NULL AUTO_INCREMENT,
  `fkid_empSepration` int NOT NULL,
  `cancellation_dt` datetime NOT NULL,
  `cancel_remarks` text,
  `approver1_id` int DEFAULT NULL,
  `is_approved1` int NOT NULL,
  `app1_remarks` text,
  `app1_dt` datetime NOT NULL,
  `approver2_id` int DEFAULT NULL,
  `is_approved2` int NOT NULL,
  `app2_remarks` text,
  `app2_dt` datetime NOT NULL,
  `apprver3_id` int DEFAULT NULL,
  `is_approved3` int NOT NULL,
  `app3_remarks` text,
  `app3_dt` datetime NOT NULL,
  `admin_id` int DEFAULT NULL,
  `is_admin_approved` int NOT NULL,
  `admin_remarks` text,
  `admin_dt` datetime NOT NULL,
  `is_final_approve` int NOT NULL,
  `is_deleted` int NOT NULL,
  `created_by` int NOT NULL,
  `created_dt` datetime NOT NULL,
  `modified_by` int NOT NULL,
  `modified_dt` datetime NOT NULL,
  PRIMARY KEY (`pkid_AppEmpSepCancel`),
  KEY `FK_admin_id` (`admin_id`),
  KEY `FK_approver1_id` (`approver1_id`),
  KEY `FK_approver2_id` (`approver2_id`),
  KEY `FK_apprver3_id` (`apprver3_id`),
  KEY `FK_empSepration` (`fkid_empSepration`),
  CONSTRAINT `FK_admin_id` FOREIGN KEY (`admin_id`) REFERENCES `tbl_employee_master` (`employee_id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_approver1_id` FOREIGN KEY (`approver1_id`) REFERENCES `tbl_employee_master` (`employee_id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_approver2_id` FOREIGN KEY (`approver2_id`) REFERENCES `tbl_employee_master` (`employee_id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_apprver3_id` FOREIGN KEY (`apprver3_id`) REFERENCES `tbl_employee_master` (`employee_id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_empSepration` FOREIGN KEY (`fkid_empSepration`) REFERENCES `tbl_emp_separation` (`sepration_id`)
) ;

 Select * from tbl_No_dues_particular_master;
CREATE TABLE `tbl_No_dues_particular_master` (
  `pkid_ParticularMaster` int NOT NULL AUTO_INCREMENT primary key,
  `company_id` int NOT NULL,
  `department_id` int NOT NULL,
  `particular_name` varchar(150) NOT NULL,
  `remarks` varchar(500) DEFAULT NULL,
  `is_deleted` int NOT NULL DEFAULT '0',
  `created_by` int NOT NULL,
  `created_date` datetime NOT NULL,
  `modified_by` int DEFAULT NULL,
  `modified_date` datetime NOT NULL
 ) ;
 
 Select * from tbl_No_dues_particular_responsible;
 CREATE TABLE `tbl_No_dues_particular_responsible` (
  `pkid_ParticularResponsible` int NOT NULL AUTO_INCREMENT primary key,
  `emp_id` int NOT NULL,
  `company_id` int NOT NULL,
  `department_id` int NOT NULL,
  `is_deleted` int NOT NULL DEFAULT '0',
  `created_by` int NOT NULL,
  `created_date` datetime NOT NULL,
  `modified_by` int DEFAULT NULL,
  `modified_date` datetime NOT NULL
 ) ;
 
 CREATE TABLE `tbl_No_dues_clearance_Department` (
  `pkid_ClearanceDepartment` int NOT NULL AUTO_INCREMENT primary key,
  `fkid_EmpSaperationId` int NOT NULL,
  `department_id` int NOT NULL,
  `is_Cleared` int NOT NULL DEFAULT '0',
  `is_deleted` int NOT NULL DEFAULT '0',
  `created_by` int NOT NULL,
  `created_date` datetime NOT NULL,
  `modified_by` int DEFAULT NULL,
  `modified_date` datetime NOT NULL
 ) ;
 
 CREATE TABLE `tbl_No_dues_emp_particular_Clearence_detail` (
  `pkid_EmpParticularClearance` int NOT NULL AUTO_INCREMENT primary key,
  `fkid_EmpSaperationId` int NOT NULL,
  `fkid_department_id` int NOT NULL,
  `fkid_ParticularId` int NOT NULL,
  `is_Outstanding` int NOT NULL DEFAULT '1',
  `is_Deleted` int NOT NULL DEFAULT '0',
  `remarks` varchar(500) DEFAULT NULL,
  `created_by` int NOT NULL,
  `created_date` datetime NOT NULL,
  `modified_by` int DEFAULT NULL,
  `modified_date` datetime NOT NULL
 ) ;
 
 drop table tbl_fnf_attendance_dtl;
 CREATE TABLE `tbl_fnf_attendance_dtl` (
  `fnf_attend_id` int NOT NULL AUTO_INCREMENT,
  `monthyear` int NOT NULL,
  `totaldays` int DEFAULT NULL,
  `acutual_lop_days` decimal(18,2) DEFAULT NULL,
  `final_lop_days` decimal(18,2) DEFAULT NULL,
  `Week_off_days` decimal(18,2) NOT NULL DEFAULT '0.00',
  `Holiday_days` decimal(18,2) NOT NULL DEFAULT '0.00',
  `Present_days` decimal(18,2) NOT NULL DEFAULT '0.00',
  `Absent_days` decimal(18,2) NOT NULL DEFAULT '0.00',
  `Leave_days` decimal(18,2) NOT NULL DEFAULT '0.00',
  `Total_Paid_days` decimal(18,2) NOT NULL DEFAULT '0.00',
  `paid_amount` double NOT NULL,
  `is_process` int NOT NULL DEFAULT '0',
  `is_deleted` int NOT NULL DEFAULT '0',
  `created_by` int NOT NULL,
  `created_dt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int NOT NULL,
  `modified_dt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `fnf_id` int DEFAULT NULL,
  `x_id` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`fnf_attend_id`),
  KEY `FK_tbl_fnf_attendance_dtl_tbl_fnf_master_fnf_id_idx` (`fnf_id`),
  CONSTRAINT `FK_tbl_fnf_attendance_dtl_tbl_fnf_master_fnf_id` FOREIGN KEY (`fnf_id`) REFERENCES `tbl_fnf_master` (`pkid_fnfMaster`)
) ;

Drop table tbl_fnf_component;
CREATE TABLE `tbl_fnf_component` (
  `variable_id` int NOT NULL AUTO_INCREMENT,
  `component_id` int DEFAULT NULL,
  `component_type` int NOT NULL DEFAULT '0',
  `monthyear` int NOT NULL,
  `variable_amt` double NOT NULL,
  `remarks` varchar(200) DEFAULT NULL,
  `is_deleted` int NOT NULL DEFAULT '0',
  `is_process` int NOT NULL DEFAULT '0',
  `created_by` int NOT NULL,
  `created_dt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int NOT NULL,
  `modified_dt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `fnf_id` int DEFAULT NULL,
  `x_id` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`variable_id`),
  KEY `FK_tbl_fnf_component_tbl_component_master_component_id` (`component_id`),
  KEY `FK_tbl_fnf_component_tbl_employee_master_fnf_id_idx` (`fnf_id`),
  CONSTRAINT `FK_tbl_fnf_component_tbl_component_master_component_id` FOREIGN KEY (`component_id`) REFERENCES `tbl_component_master` (`component_id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_tbl_fnf_component_tbl_employee_master_fnf_id` FOREIGN KEY (`fnf_id`) REFERENCES `tbl_fnf_master` (`pkid_fnfMaster`)
) ;

drop TABLE `tbl_fnf_leave_encash`;
CREATE TABLE `tbl_fnf_leave_encash` (
  `leave_encash_id` int NOT NULL AUTO_INCREMENT,
  `leave_type_id` int DEFAULT NULL,
  `leave_balance` double NOT NULL,
  `leave_encash_day` double NOT NULL DEFAULT '0',
  `leave_encash_cal` double NOT NULL DEFAULT '0',
  `leave_encash_final` double NOT NULL DEFAULT '0',
  `is_deleted` int NOT NULL DEFAULT '0',
  `created_by` int NOT NULL,
  `created_dt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `modified_by` int NOT NULL DEFAULT '0',
  `modified_dt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `is_process` int NOT NULL DEFAULT '0',
  `fnf_id` int DEFAULT NULL,
  `x_id` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`leave_encash_id`),
  KEY `FK_tbl_fnf_leave_encash_tbl_leave_type_leave_type_id` (`leave_type_id`),
  KEY `FK_tbl_fnf_leave_encash_tbl_leave_type_fdnf_id_idx` (`fnf_id`),
  CONSTRAINT `FK_tbl_fnf_leave_encash_tbl_leave_type_fdnf_id` FOREIGN KEY (`fnf_id`) REFERENCES `tbl_fnf_master` (`pkid_fnfMaster`),
  CONSTRAINT `FK_tbl_fnf_leave_encash_tbl_leave_type_leave_type_id` FOREIGN KEY (`leave_type_id`) REFERENCES `tbl_leave_type` (`leave_type_id`) ON DELETE RESTRICT
) ;


drop TABLE `tbl_fnf_loan_recover`;
CREATE TABLE `tbl_fnf_loan_recover` (
  `loan_recovery_id` int NOT NULL AUTO_INCREMENT,
  `loan_req_id` int DEFAULT NULL,
  `loan_recover_amt` decimal(18,2) NOT NULL,
  `is_process` tinyint NOT NULL,
  `is_deleted` tinyint NOT NULL,
  `created_by` int NOT NULL,
  `created_dt` datetime NOT NULL,
  `modified_by` int NOT NULL,
  `modified_dt` datetime NOT NULL,
  `x_id` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`loan_recovery_id`),
  KEY `FK_tbl_fnf_loan_recover_tbl_loan_request_loan_req_id` (`loan_req_id`),
  CONSTRAINT `FK_tbl_fnf_loan_recover_tbl_loan_request_loan_req_id` FOREIGN KEY (`loan_req_id`) REFERENCES `tbl_loan_request` (`loan_req_id`) ON DELETE RESTRICT
) ;

Drop TABLE `tbl_fnf_master` ;
CREATE TABLE `tbl_fnf_master` (
  `pkid_fnfMaster` int NOT NULL AUTO_INCREMENT,
  `emp_id` int DEFAULT NULL,
  `fkid_empSepration` int DEFAULT NULL,
  `resign_dt` datetime(6) NOT NULL,
  `notice_recovery_days` int NOT NULL DEFAULT '0',
  `notice_payment_days` int NOT NULL DEFAULT '0',
  `net_amt` decimal(10,2) NOT NULL,
  `settlment_amt` decimal(10,2) NOT NULL,
  `settlment_dt` datetime(6) NOT NULL,
  `settlement_type` int NOT NULL,
  `is_freezed` int unsigned NOT NULL,
  `is_gratuity` int NOT NULL DEFAULT '0',
  `last_working_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `is_deleted` int NOT NULL,
  `created_by` int NOT NULL,
  `created_dt` datetime(6) NOT NULL,
  `last_modified_by` int NOT NULL,
  `last_modified_date` datetime(6) NOT NULL,
  `remarks` varchar(300) DEFAULT NULL,
  `monthYear` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`pkid_fnfMaster`),
  KEY `IX_tbl_employee_master_emp_id` (`emp_id`),
  KEY `Fk_tbl_emp_sepration_idx` (`fkid_empSepration`),
  CONSTRAINT `Fk_tbl_emp_sepration` FOREIGN KEY (`fkid_empSepration`) REFERENCES `tbl_emp_separation` (`sepration_id`),
  CONSTRAINT `FK_tbl_employee_master_emp_id` FOREIGN KEY (`emp_id`) REFERENCES `tbl_employee_master` (`employee_id`) ON DELETE RESTRICT
) ;

drop TABLE `tbl_fnf_reimburesment` ;
CREATE TABLE `tbl_fnf_reimburesment` (
  `fnf_reim_id` int NOT NULL AUTO_INCREMENT,
  `company_id` int NOT NULL,
  `emp_id` int DEFAULT NULL,
  `reim_name` varchar(200) DEFAULT NULL,
  `reim_amt` double NOT NULL,
  `is_deleted` int NOT NULL,
  `created_by` int NOT NULL,
  `created_dt` datetime NOT NULL,
  `modified_by` int NOT NULL,
  `modified_dt` datetime NOT NULL,
  `is_process` int NOT NULL,
  `fnf_id` int DEFAULT NULL,
  `reim_bal` double NOT NULL DEFAULT '0',
  `x_id` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`fnf_reim_id`),
  KEY `FK_tbl_fnf_reimburesment_tbl_company_master_company_id` (`company_id`),
  KEY `FK_tbl_fnf_reimburesment_tbl_employee_master_emp_id` (`emp_id`),
  KEY `FK_tbl_fnf_reimburesment_tbl_employee_master_fnf_id_idx` (`fnf_id`),
  CONSTRAINT `FK_tbl_fnf_reimburesment_tbl_company_master_company_id` FOREIGN KEY (`company_id`) REFERENCES `tbl_company_master` (`company_id`) ON DELETE CASCADE,
  CONSTRAINT `FK_tbl_fnf_reimburesment_tbl_employee_master_emp_id` FOREIGN KEY (`emp_id`) REFERENCES `tbl_employee_master` (`employee_id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_tbl_fnf_reimburesment_tbl_employee_master_fnf_id` FOREIGN KEY (`fnf_id`) REFERENCES `tbl_fnf_master` (`pkid_fnfMaster`)
) ;

drop TABLE `tbl_emp_fnf_asset` ;
CREATE TABLE `tbl_emp_fnf_asset` (
  `fnf_asset_id` int NOT NULL AUTO_INCREMENT,
  `asset_req_id` int DEFAULT NULL,
  `recovery_amt` double NOT NULL,
  `is_deleted` int NOT NULL,
  `created_by` int NOT NULL,
  `created_dt` datetime NOT NULL,
  `modified_by` int NOT NULL,
  `modified_dt` datetime NOT NULL,
  `is_process` int NOT NULL,
  `x_id` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`fnf_asset_id`),
  KEY `FK_tbl_emp_fnf_asset_tbl_assets_request_master_asset_req_id` (`asset_req_id`),
  CONSTRAINT `FK_tbl_emp_fnf_asset_tbl_assets_request_master_asset_req_id` FOREIGN KEY (`asset_req_id`) REFERENCES `tbl_assets_request_master` (`asset_req_id`) ON DELETE RESTRICT
) ;





 -----------------------------------------------------------------------------------------------
 -- To Add Component

 ALTER TABLE `db_hrms_glaze_02_02_21`.`tbl_component_master` 
ADD COLUMN `is_fnf_component` INT NOT NULL DEFAULT 0 AFTER `is_payslip`;

 -- Adding Component------------------
 INSERT INTO `tbl_component_master` (`component_id`, `component_name`, `datatype`, `defaultvalue`, `parentid`, `is_system_key`, `property_details`, `System_function`, `component_type`, `is_salary_comp`, `is_tds_comp`, `is_data_entry_comp`, `payment_type`, `is_user_interface`, `is_payslip`, `is_fnf_component`, `created_by`, `created_dt`, `modified_by`, `is_active`, `modified_dt`) VALUES ('5000', '@LWF', '2', '0', '1', '0', 'LWF', '', '2', '0', '0', '0', '0', '1', '0', '1', '1', '2020-01-01 00:00:00', '1', '1', '2020-01-01 00:00:00');
 INSERT INTO `tbl_component_master` (`component_id`, `component_name`, `datatype`, `defaultvalue`, `parentid`, `is_system_key`, `property_details`, `System_function`, `component_type`, `is_salary_comp`, `is_tds_comp`, `is_data_entry_comp`, `payment_type`, `is_user_interface`, `is_payslip`, `is_fnf_component`, `created_by`, `created_dt`, `modified_by`, `is_active`, `modified_dt`) VALUES ('5001', '@TDS', '2', '0', '1', '0', 'TDS', '', '2', '0', '0', '0', '0', '1', '0', '1', '1', '2020-01-01 00:00:00', '1', '1', '2020-01-01 00:00:00');
 INSERT INTO `tbl_component_master` (`component_id`, `component_name`, `datatype`, `defaultvalue`, `parentid`, `is_system_key`, `property_details`, `System_function`, `component_type`, `is_salary_comp`, `is_tds_comp`, `is_data_entry_comp`, `payment_type`, `is_user_interface`, `is_payslip`, `is_fnf_component`, `created_by`, `created_dt`, `modified_by`, `is_active`, `modified_dt`) VALUES ('5002', '@Salary_Arrear', '2', '0', '1', '0', 'Salary Arrear', '', '2', '0', '0', '0', '0', '1', '0', '1', '1', '2020-01-01 00:00:00', '1', '1', '2020-01-01 00:00:00');
 INSERT INTO `tbl_component_master` (`component_id`, `component_name`, `datatype`, `defaultvalue`, `parentid`, `is_system_key`, `property_details`, `System_function`, `component_type`, `is_salary_comp`, `is_tds_comp`, `is_data_entry_comp`, `payment_type`, `is_user_interface`, `is_payslip`, `is_fnf_component`, `created_by`, `created_dt`, `modified_by`, `is_active`, `modified_dt`) VALUES ('5003', '@Overtime', '2', '0', '1', '0', 'Overtime', '', '2', '0', '0', '0', '0', '1', '0', '1', '1', '2020-01-01 00:00:00', '1', '1', '2020-01-01 00:00:00');
 INSERT INTO `tbl_component_master` (`component_id`, `component_name`, `datatype`, `defaultvalue`, `parentid`, `is_system_key`, `property_details`, `System_function`, `component_type`, `is_salary_comp`, `is_tds_comp`, `is_data_entry_comp`, `payment_type`, `is_user_interface`, `is_payslip`, `is_fnf_component`, `created_by`, `created_dt`, `modified_by`, `is_active`, `modified_dt`) VALUES ('5004', '@Other_Allowance', '2', '0', '1', '0', 'Other Allowance', '', '1', '0', '0', '0', '0', '1', '0', '1', '1', '2020-01-01 00:00:00', '1', '1', '2020-01-01 00:00:00');
 INSERT INTO `tbl_component_master` (`component_id`, `component_name`, `datatype`, `defaultvalue`, `parentid`, `is_system_key`, `property_details`, `System_function`, `component_type`, `is_salary_comp`, `is_tds_comp`, `is_data_entry_comp`, `payment_type`, `is_user_interface`, `is_payslip`, `is_fnf_component`, `created_by`, `created_dt`, `modified_by`, `is_active`, `modified_dt`) VALUES ('5005', '@Imprest_Deductions ', '2', '0', '1', '0', 'Imprest Deductions', '', '2', '0', '0', '0', '0', '1', '0', '1', '1', '2020-01-01 00:00:00', '1', '1', '2020-01-01 00:00:00');
 UPDATE `tbl_component_master` SET `component_type` = '1' WHERE (`component_id` = '5002');
UPDATE `tbl_component_master` SET `component_type` = '1' WHERE (`component_id` = '5003');

UPDATE `tbl_component_master` SET `is_fnf_component` = '1' WHERE (`component_id` = '3005');
UPDATE `tbl_component_master` SET `is_fnf_component` = '1' WHERE (`component_id` = '3001');
UPDATE `tbl_component_master` SET `is_fnf_component` = '1' WHERE (`component_id` = '3006');
UPDATE `tbl_component_master` SET `is_fnf_component` = '1' WHERE (`component_id` = '2012');
UPDATE `tbl_component_master` SET `is_fnf_component` = '1' WHERE (`component_id` = '2007');
UPDATE `tbl_component_master` SET `is_fnf_component` = '1' WHERE (`component_id` = '2004');
UPDATE `tbl_component_master` SET `is_fnf_component` = '1' WHERE (`component_id` = '3003');
UPDATE `tbl_component_master` SET `is_fnf_component` = '1' WHERE (`component_id` = '3009');
UPDATE `tbl_component_master` SET `is_fnf_component` = '1' WHERE (`component_id` = '3002');
UPDATE `tbl_component_master` SET `is_fnf_component` = '1' WHERE (`component_id` = '2009');
UPDATE `tbl_component_master` SET `is_fnf_component` = '1' WHERE (`component_id` = '3007');
UPDATE `tbl_component_master` SET `is_fnf_component` = '1' WHERE (`component_id` = '2011');
UPDATE `tbl_component_master` SET `is_fnf_component` = '1' WHERE (`component_id` = '2005');
UPDATE `tbl_component_master` SET `is_fnf_component` = '1' WHERE (`component_id` = '3008');

------------------------------------------------------------------------------------------------------------------------------------------
-- Adding Column in loss_of_pay_master

ALTER TABLE `db_hrms_glaze_26_03_21`.`tbl_lossofpay_master` 
ADD COLUMN `Week_off_days` DECIMAL(18,2) NOT NULL DEFAULT 0 AFTER `monthyear`,
ADD COLUMN `Holiday_days` DECIMAL(18,2) NOT NULL DEFAULT 0 AFTER `Week_off_days`,
ADD COLUMN `Present_days` DECIMAL(18,2) NOT NULL DEFAULT 0 AFTER `Holiday_days`,
ADD COLUMN `Absent_days` DECIMAL(18,2) NOT NULL DEFAULT 0 AFTER `Present_days`,
ADD COLUMN `Leave_days` DECIMAL(18,2) NOT NULL DEFAULT 0 AFTER `Absent_days`,
ADD COLUMN `Total_Paid_days` DECIMAL(18,2) NOT NULL DEFAULT 0 AFTER `Leave_days`,
ADD COLUMN `is_freezed` INT NOT NULL DEFAULT 0 AFTER `Total_Paid_days`,
ADD COLUMN `remarks` VARCHAR(45) NULL AFTER `is_freezed`;

---------------------------------------------------- Added by navneet 13 April 2021--------------------------------------------
ALTER TABLE `tbl_fnf_master` 
DROP FOREIGN KEY `FK_tbl_employee_master_emp_id`;
ALTER TABLE `tbl_fnf_master` 
ADD COLUMN `notice_recovery_days` INT NOT NULL DEFAULT 0 AFTER `resign_dt`,
ADD COLUMN `notice_payment_days` INT NOT NULL DEFAULT 0 AFTER `notice_recovery_days`,
CHANGE COLUMN `fnf_id` `pkid_fnfMaster` INT NOT NULL AUTO_INCREMENT ,
CHANGE COLUMN `emp_id` `fkid_empSepraation` INT NULL DEFAULT NULL ;
ALTER TABLE `tbl_fnf_master` 
ADD CONSTRAINT `FK_tbl_employee_master_emp_id`
  FOREIGN KEY (`fkid_empSepraation`)
  REFERENCES `tbl_employee_master` (`employee_id`)
  ON DELETE RESTRICT;


ALTER TABLE `tbl_fnf_master` 
DROP FOREIGN KEY `FK_tbl_employee_master_emp_id`;
ALTER TABLE `tbl_fnf_master` 
CHANGE COLUMN `fkid_empSepraation` `emp_id` INT NULL DEFAULT NULL ;
ALTER TABLE `tbl_fnf_master` 
ADD CONSTRAINT `FK_tbl_employee_master_emp_id`
  FOREIGN KEY (`emp_id`)
  REFERENCES `tbl_employee_master` (`employee_id`)
  ON DELETE RESTRICT;
ALTER TABLE `tbl_fnf_master` 
ADD COLUMN `fkid_empSepration` INT NULL AFTER `remarks`,
ADD INDEX `Fk_tbl_emp_sepration_idx` (`fkid_empSepration` ASC) VISIBLE;
;
--------------------------------------------------------------- 15 April 2021------------------------------------------------------------------

ALTER TABLE `tbl_fnf_master` 
ADD CONSTRAINT `Fk_tbl_emp_sepration`
  FOREIGN KEY (`fkid_empSepration`)
  REFERENCES `tbl_emp_separation` (`sepration_id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;
  
  ALTER TABLE `tbl_fnf_master` 
CHANGE COLUMN `fkid_empSepration` `fkid_empSepration` INT NULL DEFAULT NULL AFTER `emp_id`;

--------------------------------------------------------------- Navneet 12 May 2021------------------------------------------------------------------
ALTER TABLE `tbl_fnf_master` 
ADD COLUMN `is_gratuity` INT NOT NULL DEFAULT 0 AFTER `is_freezed`,
ADD COLUMN `last_working_date` DATETIME NULL DEFAULT CURRENT_TIMESTAMP AFTER `is_gratuity`,
CHANGE COLUMN `is_freezed` `is_freezed` INT UNSIGNED NOT NULL ;
ALTER TABLE `db_hrms_glaze_02_02_21`.`tbl_fnf_master` 
CHANGE COLUMN `created_date` `created_dt` DATETIME(6) NOT NULL ;

ALTER TABLE `tbl_fnf_leave_encash` 
DROP FOREIGN KEY `FK_tbl_fnf_leave_encash_tbl_employee_master_emp_id`,
DROP FOREIGN KEY `FK_tbl_fnf_leave_encash_tbl_company_master_company_id`;
ALTER TABLE `tbl_fnf_leave_encash` 
DROP COLUMN `emp_id`,
DROP COLUMN `company_id`,
DROP INDEX `FK_tbl_fnf_leave_encash_tbl_employee_master_emp_id` ,
DROP INDEX `FK_tbl_fnf_leave_encash_tbl_company_master_company_id` ;
;


----------------------------------------------------------------------------------------------------------------------------------------------------
ALTER TABLE `tbl_fnf_leave_encash` 
DROP COLUMN `leave_encash_amt`,
DROP COLUMN `leave_encash`,
CHANGE COLUMN `leave_encash_day` `leave_encash_day` DOUBLE NOT NULL DEFAULT '0' AFTER `leave_balance`,
CHANGE COLUMN `leave_encash_cal` `leave_encash_cal` DOUBLE NOT NULL DEFAULT '0' AFTER `leave_encash_day`,
CHANGE COLUMN `leave_encash_final` `leave_encash_final` DOUBLE NOT NULL DEFAULT '0' AFTER `leave_encash_cal`,
CHANGE COLUMN `is_deleted` `is_deleted` INT NOT NULL DEFAULT 0 ,
CHANGE COLUMN `created_dt` `created_dt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ,
CHANGE COLUMN `modified_dt` `modified_dt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ,
CHANGE COLUMN `is_process` `is_process` INT NOT NULL DEFAULT 0 ,
CHANGE COLUMN `modified_by` `modified_by` INT NOT NULL DEFAULT 0 ;

----------------------------------------------------------------------------------------------------------------------------------------------------
ALTER TABLE `tbl_fnf_attendance_dtl` 
DROP FOREIGN KEY `FK_tbl_fnf_attendance_dtl_tbl_employee_master_emp_id`,
DROP FOREIGN KEY `FK_tbl_fnf_attendance_dtl_tbl_company_master_company_id`;
ALTER TABLE `tbl_fnf_attendance_dtl` 
DROP COLUMN `emp_id`,
DROP COLUMN `company_id`,
DROP INDEX `FK_tbl_fnf_attendance_dtl_tbl_employee_master_emp_id` ,
DROP INDEX `FK_tbl_fnf_attendance_dtl_tbl_company_master_company_id` ;
;

ALTER TABLE `tbl_fnf_attendance_dtl` 
DROP COLUMN `paid_days`,
DROP COLUMN `actual_paid_days`,
DROP COLUMN `total_days`;
ALTER TABLE `tbl_fnf_attendance_dtl` 
ADD COLUMN `totaldays` INT NULL DEFAULT NULL AFTER `monthyear`,
ADD COLUMN `acutual_lop_days` DECIMAL(18,2) NULL DEFAULT NULL AFTER `totaldays`,
ADD COLUMN `final_lop_days` DECIMAL(18,2) NULL DEFAULT NULL AFTER `acutual_lop_days`,
ADD COLUMN `Week_off_days` DECIMAL(18,2) NOT NULL DEFAULT '0.00' AFTER `final_lop_days`,
ADD COLUMN `Holiday_days` DECIMAL(18,2) NOT NULL DEFAULT '0.00' AFTER `Week_off_days`,
ADD COLUMN `Present_days` DECIMAL(18,2) NOT NULL DEFAULT '0.00' AFTER `Holiday_days`,
ADD COLUMN `Absent_days` DECIMAL(18,2) NOT NULL DEFAULT '0.00' AFTER `Present_days`,
ADD COLUMN `Leave_days` DECIMAL(18,2) NOT NULL DEFAULT '0.00' AFTER `Absent_days`,
ADD COLUMN `Total_Paid_days` DECIMAL(18,2) NOT NULL DEFAULT '0.00' AFTER `Leave_days`,
CHANGE COLUMN `is_process` `is_process` INT NOT NULL DEFAULT '0' ,
CHANGE COLUMN `is_deleted` `is_deleted` INT NOT NULL ;

ALTER TABLE `tbl_fnf_attendance_dtl` 
CHANGE COLUMN `is_deleted` `is_deleted` INT NOT NULL DEFAULT 0 ,
CHANGE COLUMN `created_dt` `created_dt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ,
CHANGE COLUMN `modified_dt` `modified_dt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ;

ALTER TABLE `tbl_fnf_component` 
ADD COLUMN `component_type` INT NOT NULL DEFAULT 0 AFTER `component_id`;

ALTER TABLE `tbl_fnf_component` 
DROP FOREIGN KEY `FK_tbl_fnf_component_tbl_employee_master_emp_id`,
DROP FOREIGN KEY `FK_tbl_fnf_component_tbl_company_master_company_id`;
ALTER TABLE `tbl_fnf_component` 
DROP COLUMN `emp_id`,
DROP COLUMN `company_id`,
DROP INDEX `FK_tbl_fnf_component_tbl_employee_master_emp_id` ,
DROP INDEX `FK_tbl_fnf_component_tbl_company_master_company_id` ;
;
ALTER TABLE `tbl_fnf_component` 
CHANGE COLUMN `is_deleted` `is_deleted` INT NOT NULL DEFAULT 0 ,
CHANGE COLUMN `is_process` `is_process` INT NOT NULL DEFAULT 0 ,
CHANGE COLUMN `created_dt` `created_dt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ,
CHANGE COLUMN `modified_dt` `modified_dt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ;

UPDATE `tbl_component_master` SET `is_fnf_component` = '1' WHERE (`component_id` = '505');
INSERT INTO `tbl_component_master` (`component_id`, `component_name`, `datatype`, `defaultvalue`, `parentid`, `is_system_key`, `property_details`, `System_function`, `component_type`, `is_salary_comp`, `is_tds_comp`, `is_data_entry_comp`, `payment_type`, `is_user_interface`, `is_payslip`, `is_fnf_component`, `created_by`, `created_dt`, `modified_by`, `is_active`, `modified_dt`) VALUES ('5006', '@Mobile_Phone_Recovery ', '2', '0', '1', '0', 'Mobile Phone Recovery', '', '2', '0', '0', '0', '0', '1', '0', '1', '1', '2020-01-01 00:00:00', '1', '1', '2020-01-01 00:00:00');
INSERT INTO `tbl_component_master` (`component_id`, `component_name`, `datatype`, `defaultvalue`, `parentid`, `is_system_key`, `property_details`, `System_function`, `component_type`, `is_salary_comp`, `is_tds_comp`, `is_data_entry_comp`, `payment_type`, `is_user_interface`, `is_payslip`, `is_fnf_component`, `created_by`, `created_dt`, `modified_by`, `is_active`, `modified_dt`) VALUES ('5007', '@Notice_Period_Recovery', '2', '0', '1', '0', 'Notice Period Recovery', '', '2', '0', '0', '0', '0', '1', '0', '1', '1', '2020-01-01 00:00:00', '1', '1', '2020-01-01 00:00:00');

ALTER TABLE `tbl_salary_input` 
ADD COLUMN `is_fnf_comp` INT NOT NULL DEFAULT 0 AFTER `company_id`;

ALTER TABLE `tbl_fnf_master` 
ADD COLUMN `monthYear` INT NOT NULL DEFAULT 0 AFTER `remarks`;

INSERT INTO `tbl_menu_master` (`menu_id`, `menu_name`, `parent_menu_id`, `IconUrl`, `is_active`, `created_by`, `created_date`, `modified_by`, `modified_date`, `urll`, `SortingOrder`, `type`) VALUES ('1640', 'KT Task Master', '1639', 'fa fa-cog', '1', '1', '2020-01-01 00:00:00', '1', '2021-03-16 13:23:42', 'Employee/KT_TaskMaster', '0', '1');
INSERT INTO `tbl_menu_master` (`menu_id`, `menu_name`, `parent_menu_id`, `IconUrl`, `is_active`, `created_by`, `created_date`, `modified_by`, `modified_date`, `urll`, `SortingOrder`, `type`) VALUES ('1641', 'KT Task', '1639', 'fa fa-cog', '1', '1', '2020-01-01 00:00:00', '1', '2021-03-16 13:23:42', 'Employee/KTTask', '0', '1');
INSERT INTO `tbl_menu_master` (`menu_id`, `menu_name`, `parent_menu_id`, `IconUrl`, `is_active`, `created_by`, `created_date`, `modified_by`, `modified_date`, `urll`, `SortingOrder`, `type`) VALUES ('1642', 'KT Task Status', '1639', 'fa fa-cog', '1', '1', '2020-01-01 00:00:00', '1', '2021-03-16 13:23:42', 'Employee/KT_TaskStatus', '0', '1');
INSERT INTO `tbl_menu_master` (`menu_id`, `menu_name`, `parent_menu_id`, `IconUrl`, `is_active`, `created_by`, `created_date`, `modified_by`, `modified_date`, `urll`, `SortingOrder`, `type`) VALUES ('1643', 'KT Task Upload', '1639', 'fa fa-cog', '1', '1', '2020-01-01 00:00:00', '1', '2021-03-16 13:23:42', 'Employee/KT_TaskUpload', '0', '1');
INSERT INTO `tbl_menu_master` (`menu_id`, `menu_name`, `parent_menu_id`, `IconUrl`, `is_active`, `created_by`, `created_date`, `modified_by`, `modified_date`, `urll`, `SortingOrder`, `type`) VALUES ('1644', 'KT Task Report', '1639', 'fa fa-cog', '1', '1', '2020-01-01 00:00:00', '1', '2021-03-16 13:23:42', 'view/KTTaskReport', '0', '1');

INSERT INTO `tbl_role_menu_master` (`role_menu_id`, `role_id`, `menu_id`, `created_by`, `created_date`, `modified_by`, `modified_date`) VALUES ('1609012470', '1', '1640', '1', '2020-01-01 00:00:00', '1', '2020-01-01 00:00:00');
INSERT INTO `tbl_role_menu_master` (`role_menu_id`, `role_id`, `menu_id`, `created_by`, `created_date`, `modified_by`, `modified_date`) VALUES ('1609012471', '101', '1640', '1', '2020-01-01 00:00:00', '1', '2020-01-01 00:00:00');
INSERT INTO `tbl_role_menu_master` (`role_menu_id`, `role_id`, `menu_id`, `created_by`, `created_date`, `modified_by`, `modified_date`) VALUES ('1609012472', '1', '1641', '1', '2020-01-01 00:00:00', '1', '2020-01-01 00:00:00');
INSERT INTO `tbl_role_menu_master` (`role_menu_id`, `role_id`, `menu_id`, `created_by`, `created_date`, `modified_by`, `modified_date`) VALUES ('1609012473', '101', '1641', '1', '2020-01-01 00:00:00', '1', '2020-01-01 00:00:00');
INSERT INTO `tbl_role_menu_master` (`role_menu_id`, `role_id`, `menu_id`, `created_by`, `created_date`, `modified_by`, `modified_date`) VALUES ('1609012474', '1', '1642', '1', '2020-01-01 00:00:00', '1', '2020-01-01 00:00:00');
INSERT INTO `tbl_role_menu_master` (`role_menu_id`, `role_id`, `menu_id`, `created_by`, `created_date`, `modified_by`, `modified_date`) VALUES ('1609012475', '101', '1642', '1', '2020-01-01 00:00:00', '1', '2020-01-01 00:00:00');
INSERT INTO `tbl_role_menu_master` (`role_menu_id`, `role_id`, `menu_id`, `created_by`, `created_date`, `modified_by`, `modified_date`) VALUES ('1609012476', '1', '1643', '1', '2020-01-01 00:00:00', '1', '2020-01-01 00:00:00');
INSERT INTO `tbl_role_menu_master` (`role_menu_id`, `role_id`, `menu_id`, `created_by`, `created_date`, `modified_by`, `modified_date`) VALUES ('1609012477', '101', '1643', '1', '2020-01-01 00:00:00', '1', '2020-01-01 00:00:00');
INSERT INTO `tbl_role_menu_master` (`role_menu_id`, `role_id`, `menu_id`, `created_by`, `created_date`, `modified_by`, `modified_date`) VALUES ('1609012478', '1', '1644', '1', '2020-01-01 00:00:00', '1', '2020-01-01 00:00:00');
INSERT INTO `tbl_role_menu_master` (`role_menu_id`, `role_id`, `menu_id`, `created_by`, `created_date`, `modified_by`, `modified_date`) VALUES ('1609012479', '101', '1644', '1', '2020-01-01 00:00:00', '1', '2020-01-01 00:00:00');


----------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------- 
ALTER TABLE `tbl_lossofpay_master` 
ADD COLUMN `fkid_sepration` INT NOT NULL DEFAULT 0 AFTER `remarks`;

INSERT INTO `tbl_component_master` (`component_id`, `component_name`, `datatype`, `defaultvalue`, `parentid`, `is_system_key`, `property_details`, `System_function`, `component_type`, `is_salary_comp`, `is_tds_comp`, `is_data_entry_comp`, `payment_type`, `is_user_interface`, `is_payslip`, `is_fnf_component`, `created_by`, `created_dt`, `modified_by`, `is_active`, `modified_dt`) VALUES ('541', '@EL_Encashment', '2', '0', '1', '0', 'EL Encashment', '', '1', '0', '0', '0', '0', '1', '0', '1', '1', '2020-01-01 00:00:00', '1', '1', '2020-01-01 00:00:00');
INSERT INTO `tbl_component_master` (`component_id`, `component_name`, `datatype`, `defaultvalue`, `parentid`, `is_system_key`, `property_details`, `System_function`, `component_type`, `is_salary_comp`, `is_tds_comp`, `is_data_entry_comp`, `payment_type`, `is_user_interface`, `is_payslip`, `is_fnf_component`, `created_by`, `created_dt`, `modified_by`, `is_active`, `modified_dt`) VALUES ('542', '@Comp_Off_Encashment', '2', '0', '1', '0', 'Comp Off Encashment', '', '1', '0', '0', '0', '0', '1', '0', '1', '1', '2020-01-01 00:00:00', '1', '1', '2020-01-01 00:00:00');

ALTER TABLE `tbl_salary_input_change` 
ADD COLUMN `is_fnf_comp` INT NOT NULL DEFAULT 0 AFTER `company_id`;

----------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------- 
ALTER TABLE `db_hrms_glaze_02_02_21`.`tbl_lossofpay_master` 
ADD COLUMN `Actual_Paid_days` DECIMAL(18,2) NULL AFTER `Total_Paid_days`,
ADD COLUMN `Additional_Paid_days` DECIMAL(18,2) NULL AFTER `Actual_Paid_days`,
CHANGE COLUMN `monthyear` `monthyear` INT NULL DEFAULT NULL AFTER `company_id`,
CHANGE COLUMN `is_active` `is_active` INT NOT NULL AFTER `fkid_sepration`,
CHANGE COLUMN `created_by` `created_by` INT NOT NULL AFTER `is_active`,
CHANGE COLUMN `created_date` `created_date` DATETIME NOT NULL AFTER `created_by`,
CHANGE COLUMN `modified_by` `modified_by` INT NOT NULL AFTER `created_date`,
CHANGE COLUMN `modified_date` `modified_date` DATETIME NOT NULL AFTER `modified_by`;
ALTER TABLE `db_hrms_glaze_02_02_21`.`tbl_lossofpay_master` 
CHANGE COLUMN `Actual_Paid_days` `Actual_Paid_days` DECIMAL(18,2) NULL DEFAULT 0 ,
CHANGE COLUMN `Additional_Paid_days` `Additional_Paid_days` DECIMAL(18,2) NULL DEFAULT 0 ;


----------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------
INSERT INTO `db_hrms_glaze_02_02_21`.`tbl_component_formula_details` (`sno`, `component_id`, `company_id`, `salary_group_id`, `formula`, `function_calling_order`, `created_by`, `created_dt`, `deleted_by`, `deleted_dt`, `is_deleted`) VALUES ('541', '541', '1', '1', '0', '1', '1', '2020-01-01 00:00:00', '1', '2020-01-01 00:00:00', '0');
INSERT INTO `db_hrms_glaze_02_02_21`.`tbl_component_formula_details` (`sno`, `component_id`, `company_id`, `salary_group_id`, `formula`, `function_calling_order`, `created_by`, `created_dt`, `deleted_by`, `deleted_dt`, `is_deleted`) VALUES ('542', '542', '1', '1', '0', '1', '1', '2020-01-01 00:00:00', '1', '2020-01-01 00:00:00', '0');
INSERT INTO `db_hrms_glaze_02_02_21`.`tbl_component_formula_details` (`sno`, `component_id`, `company_id`, `salary_group_id`, `formula`, `function_calling_order`, `created_by`, `created_dt`, `deleted_by`, `deleted_dt`, `is_deleted`) VALUES ('543', '543', '1', '1', '0', '1', '1', '2020-01-01 00:00:00', '1', '2020-01-01 00:00:00', '0');
INSERT INTO `db_hrms_glaze_02_02_21`.`tbl_component_formula_details` (`sno`, `component_id`, `company_id`, `salary_group_id`, `formula`, `function_calling_order`, `created_by`, `created_dt`, `deleted_by`, `deleted_dt`, `is_deleted`) VALUES ('544', '544', '1', '1', 'ifnull(@PaidDays,0)+ifnull(@AddtitionalPaidDay,0) + ifnull( @CompOffEncashmentDay ,0)+IfNull( @ELEncashmentDay,0)', '4', '1', '2020-01-01 00:00:00', '1', '2020-01-01 00:00:00', '0');
INSERT INTO `db_hrms_glaze_02_02_21`.`tbl_component_formula_details` (`sno`, `component_id`, `company_id`, `salary_group_id`, `formula`, `function_calling_order`, `created_by`, `created_dt`, `deleted_by`, `deleted_dt`, `is_deleted`) VALUES ('545', '545', '1', '1', '0', '1', '1', '2020-01-01 00:00:00', '1', '2020-01-01 00:00:00', '0');
INSERT INTO `db_hrms_glaze_02_02_21`.`tbl_component_formula_details` (`sno`, `component_id`, `company_id`, `salary_group_id`, `formula`, `function_calling_order`, `created_by`, `created_dt`, `deleted_by`, `deleted_dt`, `is_deleted`) VALUES ('546', '546', '1', '1', '0', '1', '1', '2020-01-01 00:00:00', '1', '2020-01-01 00:00:00', '0');

----------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------- 


SET SQL_SAFE_UPDATES = 0;
Update tbl_emp_officaial_sec set state_id=0 where state_id is null

ALTER TABLE `db_hrms_glaze_02_02_21`.`tbl_no_dues_emp_particular_clearence_detail` 
CHANGE COLUMN `is_Outstanding` `is_Outstanding` INT NOT NULL DEFAULT '2' ;
