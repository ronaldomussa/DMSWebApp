-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema dms
-- -----------------------------------------------------
-- display management system

-- -----------------------------------------------------
-- Schema dms
--
-- display management system
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `dms` DEFAULT CHARACTER SET utf8 COLLATE utf8_bin ;
USE `dms` ;

-- -----------------------------------------------------
-- Table `dms`.`display`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `dms`.`display` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(30) NULL,
  `token` VARCHAR(30) NULL,
  `location` VARCHAR(60) NULL,
  `orientation` INT NULL COMMENT '1 = paisagem\n2 = retrato\n3 = livre',
  `display_size` VARCHAR(15) NULL,
  `timetable` TEXT NULL COMMENT 'Json modelo = \n{0:{\"start\":\"08:00\", \"end\":\"16:00\"},1:{\"start\":\"08:00\", \"end\":\"16:00\"} }\n0 = domingo\n1 = segunda\netc.',
  `is_active` BIT NOT NULL DEFAULT 1,
  `last_activity_date` DATETIME NULL,
  `created_by` VARCHAR(45) NULL,
  `created_date` DATETIME NULL,
  `last_modified_date` DATETIME NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `dms`.`media`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `dms`.`media` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(30) NULL,
  `media_type` INT NULL DEFAULT 1 COMMENT '1 = video\n2 = image\n3 = rss\n4 = interactive',
  `timing` INT NULL COMMENT 'Tempo em segundos',
  `media_url` VARCHAR(120) NULL,
  `start_date` DATETIME NULL,
  `end_date` DATETIME NULL,
  `insertions_limit` INT NULL,
  `expose_at_all` BIT NULL,
  `expose_in_groups` VARCHAR(150) NULL,
  `expose_in` VARCHAR(150) NULL,
  `is_active` BIT NOT NULL DEFAULT 1,
  `created_by` VARCHAR(45) NULL,
  `created_date` DATETIME NULL,
  `last_modified_date` DATETIME NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `dms`.`report`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `dms`.`report` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `display_id` INT NULL,
  `media_id` INT NULL,
  `insetions_counted` INT NULL,
  `date_start` DATETIME NULL,
  `date_end` DATETIME NULL,
  `date_created` DATETIME NULL,
  PRIMARY KEY (`id`),
  INDEX `report_media_idx` (`media_id` ASC),
  INDEX `report_display_idx` (`display_id` ASC),
  CONSTRAINT `report_media`
    FOREIGN KEY (`media_id`)
    REFERENCES `dms`.`media` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `report_display`
    FOREIGN KEY (`display_id`)
    REFERENCES `dms`.`display` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `dms`.`user_role`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `dms`.`user_role` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(20) NULL,
  `is_active` BIT NULL,
  `created_by` VARCHAR(45) NULL,
  `created_date` DATETIME NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `dms`.`user`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `dms`.`user` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `user_role_id` INT NULL,
  `name` VARCHAR(30) NULL,
  `user_picture_url` VARCHAR(120) NULL,
  `email` VARCHAR(150) NULL,
  `password` VARCHAR(45) NULL,
  `phone` VARCHAR(20) NULL,
  `token` VARCHAR(45) NULL,
  `token_expiration_date` DATETIME NULL,
  `created_by` VARCHAR(30) NULL,
  `created_date` DATETIME NULL,
  `last_modified_date` DATETIME NULL,
  `is_active` BIT NULL,
  `is_super_user` BIT NULL,
  PRIMARY KEY (`id`),
  INDEX `user_role_idx` (`user_role_id` ASC),
  CONSTRAINT `user_role`
    FOREIGN KEY (`user_role_id`)
    REFERENCES `dms`.`user_role` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `dms`.`modules`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `dms`.`modules` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NULL,
  `description` TINYTEXT NULL,
  `is_active` BIT NULL DEFAULT 1,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `dms`.`user_role_permission`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `dms`.`user_role_permission` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `user_role_id` INT NOT NULL,
  `module_id` INT NOT NULL,
  `can_read` BIT NULL,
  `can_write` BIT NULL,
  `can_delete` BIT NULL,
  `is_active` BIT NULL DEFAULT 1,
  `created_by` VARCHAR(45) NULL,
  `created_date` DATETIME NULL,
  PRIMARY KEY (`id`),
  INDEX `role_permission_idx` (`user_role_id` ASC),
  INDEX `permission_module_idx` (`module_id` ASC),
  CONSTRAINT `role_permission`
    FOREIGN KEY (`user_role_id`)
    REFERENCES `dms`.`user_role` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `permission_module`
    FOREIGN KEY (`module_id`)
    REFERENCES `dms`.`modules` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

-- -----------------------------------------------------
-- Data for table `dms`.`user_role`
-- -----------------------------------------------------
START TRANSACTION;
USE `dms`;
INSERT INTO `dms`.`user_role` (`id`, `name`, `is_active`, `created_by`, `created_date`) VALUES (1, 'Padrão', 1, 'system', NULL);
INSERT INTO `dms`.`user_role` (`id`, `name`, `is_active`, `created_by`, `created_date`) VALUES (2, 'Admin', 1, 'system', NULL);

COMMIT;


-- -----------------------------------------------------
-- Data for table `dms`.`user`
-- -----------------------------------------------------
START TRANSACTION;
USE `dms`;
INSERT INTO `dms`.`user` (`id`, `user_role_id`, `name`, `user_picture_url`, `email`, `password`, `phone`, `token`, `token_expiration_date`, `created_by`, `created_date`, `last_modified_date`, `is_active`, `is_super_user`) VALUES (1, NULL, 'Ronaldo Mussa', NULL, 'ronaldo.mussa@gmail.com', 'bde0425c3b1e884ea2b9f6f4a7ca5811', '5592982438054', NULL, NULL, 'system', NULL, NULL, 1, 1);

COMMIT;


-- -----------------------------------------------------
-- Data for table `dms`.`modules`
-- -----------------------------------------------------
START TRANSACTION;
USE `dms`;
INSERT INTO `dms`.`modules` (`id`, `name`, `description`, `is_active`) VALUES (1, 'Usuarios', 'Gestão dos usuários do sistema', 1);

COMMIT;


-- -----------------------------------------------------
-- Data for table `dms`.`user_role_permission`
-- -----------------------------------------------------
START TRANSACTION;
USE `dms`;
INSERT INTO `dms`.`user_role_permission` (`id`, `user_role_id`, `module_id`, `can_read`, `can_write`, `can_delete`, `is_active`, `created_by`, `created_date`) VALUES (1, 1, 1, 1, 0, 0, 1, 'system', NULL);
INSERT INTO `dms`.`user_role_permission` (`id`, `user_role_id`, `module_id`, `can_read`, `can_write`, `can_delete`, `is_active`, `created_by`, `created_date`) VALUES (2, 2, 1, 1, 1, 1, 1, 'system', NULL);

COMMIT;

