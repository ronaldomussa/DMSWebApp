INSERT INTO `dms`.`report` (`media_id`, `media_name`, `display_id`, `display_name`,`display_token`, `display_location`, `display_orientation`, `display_size`, `created_date`) 
VALUES ('1', 'Midia teste 1', '2', 'Display Escritorio', 'Z9ALHFVL', 'Av. Parintins, 229 - Cachoeirinha 69065-050', 'Retrato', '1080x1920', now());

INSERT INTO `dms`.`report` (`media_id`, `media_name`, `display_id`, `display_name`,`display_token`, `display_location`, `display_orientation`, `display_size`, `created_date`) 
VALUES ('2', 'Midia teste 2', '2', 'Display Escritorio','Z9ALHFVL', 'Av. Parintins, 229 - Cachoeirinha 69065-050', 'Retrato', '1080x1920', now());

INSERT INTO `dms`.`report` (`media_id`, `media_name`, `display_id`, `display_name`,`display_token`, `display_location`, `display_orientation`, `display_size`, `created_date`) 
VALUES ('1', 'Midia teste 1', '11', 'Display Semef','Z3TK09Q5', 'Semef, centro', 'Retrato', '1080x1920', now());

INSERT INTO `dms`.`report` (`media_id`, `media_name`, `display_id`, `display_name`,`display_token`, `display_location`, `display_orientation`, `display_size`, `created_date`) 
VALUES ('3', 'Midia teste 3', '11', 'Display Semef','Z3TK09Q5', 'Semef, centro', 'Retrato', '1080x1920', now());

#----


INSERT INTO `dms`.`report_insertions`  
(`report_id`, `insertions_counted` , `period_start_date` , `period_end_date` , `created_date` )
VALUES ('1', '100', '2022-09-08 00:00:00', '2022-09-09 00:00:00', NOW());

INSERT INTO `dms`.`report_insertions`  
(`report_id`, `insertions_counted` , `period_start_date` , `period_end_date` , `created_date` )
VALUES ('1', '150', '2022-09-09 00:00:00', '2022-09-10 00:00:00', NOW());

INSERT INTO `dms`.`report_insertions`  
(`report_id`, `insertions_counted` , `period_start_date` , `period_end_date` , `created_date` )
VALUES ('1', '200', '2022-09-10 00:00:00', '2022-09-11 00:00:00', NOW());


INSERT INTO `dms`.`report_insertions`  
(`report_id`, `insertions_counted` , `period_start_date` , `period_end_date` , `created_date` )
VALUES ('2', '250', '2022-09-08 00:00:00', '2022-09-09 00:00:00', NOW());

INSERT INTO `dms`.`report_insertions`  
(`report_id`, `insertions_counted` , `period_start_date` , `period_end_date` , `created_date` )
VALUES ('2', '300', '2022-09-09 00:00:00', '2022-09-10 00:00:00', NOW());

INSERT INTO `dms`.`report_insertions`  
(`report_id`, `insertions_counted` , `period_start_date` , `period_end_date` , `created_date` )
VALUES ('2', '350', '2022-09-10 00:00:00', '2022-09-11 00:00:00', NOW());


INSERT INTO `dms`.`report_insertions`  
(`report_id`, `insertions_counted` , `period_start_date` , `period_end_date` , `created_date` )
VALUES ('3', '500', '2022-09-08 00:00:00', '2022-09-09 00:00:00', NOW());

INSERT INTO `dms`.`report_insertions`  
(`report_id`, `insertions_counted` , `period_start_date` , `period_end_date` , `created_date` )
VALUES ('3', '450', '2022-09-09 00:00:00', '2022-09-10 00:00:00', NOW());


select * from report;
select * from report_insertions;


