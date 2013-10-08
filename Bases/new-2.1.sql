SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

DROP SCHEMA IF EXISTS `bazar` ;
CREATE SCHEMA IF NOT EXISTS `bazar` DEFAULT CHARACTER SET utf8 ;
USE `bazar` ;

-- -----------------------------------------------------
-- Table `bazar`.`classes`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`classes` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`contact_persons`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`contact_persons` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `telephones` VARCHAR(100) NULL DEFAULT NULL,
  `comments` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`events`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`events` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `date` DATETIME NOT NULL,
  `class_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `user_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `user` VARCHAR(20) NULL DEFAULT NULL,
  `cause` TEXT NULL DEFAULT NULL,
  `activity` TEXT NULL DEFAULT NULL,
  `lessee_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `place_type_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `place_no` VARCHAR(10) NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `places_id` (`place_type_id` ASC, `place_no` ASC),
  INDEX `classes_id` (`class_id` ASC),
  INDEX `lessees` (`lessee_id` ASC),
  INDEX `class_key` (`class_id` ASC),
  INDEX `lessees_key` (`lessee_id` ASC),
  INDEX `place_key` (`place_type_id` ASC, `place_no` ASC),
  FULLTEXT INDEX `cause_idx` (`cause` ASC),
  FULLTEXT INDEX `activity_idx` (`activity` ASC))
ENGINE = MyISAM
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`goods`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`goods` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`lessees`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`lessees` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(100) NOT NULL,
  `FIO_dir` VARCHAR(45) NULL DEFAULT NULL,
  `passport_ser` VARCHAR(5) NULL DEFAULT NULL,
  `passport_no` VARCHAR(6) NULL DEFAULT NULL,
  `passport_exit` VARCHAR(100) NULL DEFAULT NULL,
  `address` VARCHAR(100) NULL DEFAULT NULL,
  `INN` VARCHAR(12) NULL DEFAULT NULL,
  `OGRN` VARCHAR(15) NULL DEFAULT NULL,
  `wholesaler` TINYINT(1) NULL DEFAULT '0',
  `retail` TINYINT(1) NULL DEFAULT '0',
  `goods_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `comments` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `goods_id_idx` (`goods_id` ASC),
  CONSTRAINT `goods_id`
    FOREIGN KEY (`goods_id`)
    REFERENCES `bazar`.`goods` (`id`)
    ON DELETE SET NULL
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`users` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `login` VARCHAR(45) NOT NULL,
  `description` TEXT NULL DEFAULT NULL,
  `admin` TINYINT(1) NOT NULL DEFAULT FALSE,
  `edit_slips` TINYINT(1) NOT NULL DEFAULT FALSE,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`organizations`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`organizations` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(100) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`place_types`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`place_types` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(10) NULL DEFAULT NULL,
  `description` VARCHAR(100) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`places`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`places` (
  `type_id` INT(10) UNSIGNED NOT NULL,
  `place_no` VARCHAR(10) NOT NULL,
  `area` FLOAT(10,2) NULL DEFAULT NULL,
  `contact_person_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `org_id` INT UNSIGNED NULL DEFAULT NULL,
  `comments` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`type_id`, `place_no`),
  INDEX `types_id_idx` (`type_id` ASC),
  INDEX `contacts_idx` (`contact_person_id` ASC),
  INDEX `org_id_idx` (`org_id` ASC),
  CONSTRAINT `contacts`
    FOREIGN KEY (`contact_person_id`)
    REFERENCES `bazar`.`contact_persons` (`id`)
    ON DELETE SET NULL
    ON UPDATE NO ACTION,
  CONSTRAINT `types_id`
    FOREIGN KEY (`type_id`)
    REFERENCES `bazar`.`place_types` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT `org_id`
    FOREIGN KEY (`org_id`)
    REFERENCES `bazar`.`organizations` (`id`)
    ON DELETE SET NULL
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`contracts`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`contracts` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `number` VARCHAR(15) NOT NULL,
  `lessee_id` INT UNSIGNED NOT NULL,
  `org_id` INT UNSIGNED NOT NULL,
  `place_type_id` INT(10) UNSIGNED NOT NULL,
  `place_no` VARCHAR(10) NOT NULL,
  `sign_date` DATE NULL,
  `start_date` DATE NOT NULL,
  `end_date` DATE NOT NULL,
  `pay_day` INT NULL DEFAULT NULL,
  `cancel_date` DATE NULL DEFAULT NULL,
  `comments` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_lessee_id_idx` (`lessee_id` ASC),
  INDEX `fk_org_id_idx` (`org_id` ASC),
  INDEX `fk_place_id_idx` (`place_type_id` ASC, `place_no` ASC),
  UNIQUE INDEX `unique_number_date` (`number` ASC, `sign_date` ASC),
  CONSTRAINT `fk_lessee_id`
    FOREIGN KEY (`lessee_id`)
    REFERENCES `bazar`.`lessees` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_org_id`
    FOREIGN KEY (`org_id`)
    REFERENCES `bazar`.`organizations` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_place_id`
    FOREIGN KEY (`place_type_id` , `place_no`)
    REFERENCES `bazar`.`places` (`type_id` , `place_no`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`accrual`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`accrual` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `user_id` INT UNSIGNED NULL DEFAULT NULL,
  `contract_id` INT UNSIGNED NOT NULL,
  `month` TINYINT UNSIGNED NOT NULL,
  `year` INT UNSIGNED NOT NULL,
  `paid` TINYINT(1) NOT NULL DEFAULT FALSE,
  `no_complete` TINYINT(1) NOT NULL DEFAULT TRUE,
  `comments` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `Contract_per_month` USING BTREE (`contract_id` ASC, `year` ASC, `month` ASC),
  INDEX `fk_accrual_1_idx` (`user_id` ASC),
  INDEX `fk_accrual_contract_id_idx` (`contract_id` ASC),
  CONSTRAINT `fk_accrual_user_id`
    FOREIGN KEY (`user_id`)
    REFERENCES `bazar`.`users` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_accrual_contract_id`
    FOREIGN KEY (`contract_id`)
    REFERENCES `bazar`.`contracts` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`cash`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`cash` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`employees`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`employees` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`income_items`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`income_items` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`credit_slips`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`credit_slips` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `org_id` INT UNSIGNED NOT NULL,
  `cash_id` INT UNSIGNED NOT NULL,
  `lessee_id` INT UNSIGNED NULL DEFAULT NULL,
  `user_id` INT UNSIGNED NULL,
  `date` DATE NOT NULL,
  `sum` DECIMAL(10,2) UNSIGNED NOT NULL,
  `contract_id` INT UNSIGNED NULL DEFAULT NULL,
  `income_id` INT UNSIGNED NULL DEFAULT NULL,
  `operation` ENUM('common','advance','payment') NOT NULL DEFAULT 'common',
  `accrual_id` INT UNSIGNED NULL DEFAULT NULL,
  `employee_id` INT UNSIGNED NULL DEFAULT NULL,
  `details` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_credit_org_id_idx` (`org_id` ASC),
  INDEX `fk_credit_cash_id_idx` (`cash_id` ASC),
  INDEX `fk_credit_lessee_id_idx` (`lessee_id` ASC),
  INDEX `fk_credit_income_id_idx` (`income_id` ASC),
  INDEX `user_id_idx` (`user_id` ASC),
  INDEX `fk_credit_employee_id_idx` (`employee_id` ASC),
  INDEX `credit_operation_idx` (`operation` ASC),
  INDEX `fk_credit_slips_accural_idx` (`accrual_id` ASC),
  INDEX `fk_credit_slips_contract_id_idx` (`contract_id` ASC),
  CONSTRAINT `fk_credit_org_id`
    FOREIGN KEY (`org_id`)
    REFERENCES `bazar`.`organizations` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_cash_id`
    FOREIGN KEY (`cash_id`)
    REFERENCES `bazar`.`cash` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_lessee_id`
    FOREIGN KEY (`lessee_id`)
    REFERENCES `bazar`.`lessees` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `user_id`
    FOREIGN KEY (`user_id`)
    REFERENCES `bazar`.`users` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_employee_id`
    FOREIGN KEY (`employee_id`)
    REFERENCES `bazar`.`employees` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_slips_accural`
    FOREIGN KEY (`accrual_id`)
    REFERENCES `bazar`.`accrual` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_slips_contract_id`
    FOREIGN KEY (`contract_id`)
    REFERENCES `bazar`.`contracts` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_income_id`
    FOREIGN KEY (`income_id`)
    REFERENCES `bazar`.`income_items` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`payments`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`payments` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `createdate` DATETIME NULL DEFAULT NULL,
  `credit_slip_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `accrual_id` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_pays_credit_slips_idx` (`credit_slip_id` ASC),
  INDEX `fk_payments_accrual_idx` (`accrual_id` ASC),
  CONSTRAINT `fk_payments_accrual`
    FOREIGN KEY (`accrual_id`)
    REFERENCES `bazar`.`accrual` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_pays_credit_slips`
    FOREIGN KEY (`credit_slip_id`)
    REFERENCES `bazar`.`credit_slips` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 0
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;


-- -----------------------------------------------------
-- Table `bazar`.`units`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`units` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(10) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`services`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`services` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `units_id` INT UNSIGNED NOT NULL,
  `income_id` INT UNSIGNED NULL DEFAULT NULL,
  `by_area` TINYINT(1) NOT NULL DEFAULT FALSE,
  PRIMARY KEY (`id`),
  INDEX `units_id_idx` (`units_id` ASC),
  UNIQUE INDEX `name_UNIQUE` (`name` ASC),
  INDEX `fk_services_income_id_idx` (`income_id` ASC),
  CONSTRAINT `units_id`
    FOREIGN KEY (`units_id`)
    REFERENCES `bazar`.`units` (`id`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_services_income_id`
    FOREIGN KEY (`income_id`)
    REFERENCES `bazar`.`income_items` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`accrual_pays`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`accrual_pays` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `accrual_id` INT UNSIGNED NOT NULL,
  `service_id` INT UNSIGNED NOT NULL,
  `cash_id` INT UNSIGNED NOT NULL,
  `count` INT UNSIGNED NULL DEFAULT 0,
  `price` DECIMAL(10,2) UNSIGNED NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  INDEX `fk_accrual_pays_accrual_id_idx` (`accrual_id` ASC),
  INDEX `fk_accrual_pays_service_id_idx` (`service_id` ASC),
  INDEX `fk_accrual_pays_cash_idx` (`cash_id` ASC),
  CONSTRAINT `fk_accrual_pays_accrual_id`
    FOREIGN KEY (`accrual_id`)
    REFERENCES `bazar`.`accrual` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_accrual_pays_service_id`
    FOREIGN KEY (`service_id`)
    REFERENCES `bazar`.`services` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_accrual_pays_cash`
    FOREIGN KEY (`cash_id`)
    REFERENCES `bazar`.`cash` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`payment_details`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`payment_details` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `payment_id` INT(10) UNSIGNED NOT NULL,
  `accrual_pay_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `sum` DECIMAL(10,0) NOT NULL DEFAULT '0',
  `income_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_pay_details_1_idx` (`accrual_pay_id` ASC),
  INDEX `fk_pay_details_income_idx` (`income_id` ASC),
  INDEX `fk_payment_details_parent_idx` (`payment_id` ASC),
  CONSTRAINT `fk_payment_details_parent`
    FOREIGN KEY (`payment_id`)
    REFERENCES `bazar`.`payments` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_pay_details_accrual`
    FOREIGN KEY (`accrual_pay_id`)
    REFERENCES `bazar`.`accrual_pays` (`id`)
    ON DELETE SET NULL
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_pay_details_income`
    FOREIGN KEY (`income_id`)
    REFERENCES `bazar`.`income_items` (`id`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 0
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;


-- -----------------------------------------------------
-- Table `bazar`.`contract_pays`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`contract_pays` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `contract_id` INT UNSIGNED NOT NULL,
  `service_id` INT UNSIGNED NOT NULL,
  `cash_id` INT UNSIGNED NOT NULL,
  `count` INT UNSIGNED NOT NULL DEFAULT 1,
  `price` DECIMAL(10,2) UNSIGNED NOT NULL,
  `min_sum` DECIMAL(10,2) NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_contract_pays_service_id_idx` (`service_id` ASC),
  INDEX `fk_contract_pays_1_idx` (`cash_id` ASC),
  INDEX `fk_contract_pays_contract_id_idx` (`contract_id` ASC),
  CONSTRAINT `fk_contract_pays_service_id`
    FOREIGN KEY (`service_id`)
    REFERENCES `bazar`.`services` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_contract_pays_cash_id`
    FOREIGN KEY (`cash_id`)
    REFERENCES `bazar`.`cash` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_contract_pays_contract_id`
    FOREIGN KEY (`contract_id`)
    REFERENCES `bazar`.`contracts` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`expense_items`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`expense_items` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`contractors`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`contractors` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`debit_slips`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`debit_slips` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `org_id` INT UNSIGNED NOT NULL,
  `cash_id` INT UNSIGNED NOT NULL,
  `contractor_id` INT UNSIGNED NULL DEFAULT NULL,
  `user_id` INT UNSIGNED NULL DEFAULT NULL,
  `date` DATE NOT NULL,
  `sum` DECIMAL(10,2) UNSIGNED NOT NULL DEFAULT 0,
  `expense_id` INT UNSIGNED NULL DEFAULT NULL,
  `operation` ENUM('common','advance') NOT NULL DEFAULT 'common',
  `employee_id` INT UNSIGNED NULL DEFAULT NULL,
  `details` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_debit_org_idx` (`org_id` ASC),
  INDEX `fk_debit_cash_idx` (`cash_id` ASC),
  INDEX `fk_debit_contractor_idx` (`contractor_id` ASC),
  INDEX `fk_debit_user_idx` (`user_id` ASC),
  INDEX `fk_debit_expense_idx` (`expense_id` ASC),
  INDEX `fk_debit_employee_idx` (`employee_id` ASC),
  INDEX `debit_opration_idx` (`operation` ASC),
  CONSTRAINT `fk_debit_org`
    FOREIGN KEY (`org_id`)
    REFERENCES `bazar`.`organizations` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_debit_cash`
    FOREIGN KEY (`cash_id`)
    REFERENCES `bazar`.`cash` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_debit_contractor`
    FOREIGN KEY (`contractor_id`)
    REFERENCES `bazar`.`contractors` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_debit_user`
    FOREIGN KEY (`user_id`)
    REFERENCES `bazar`.`users` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_debit_expense`
    FOREIGN KEY (`expense_id`)
    REFERENCES `bazar`.`expense_items` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_debit_employee`
    FOREIGN KEY (`employee_id`)
    REFERENCES `bazar`.`employees` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8
PACK_KEYS = DEFAULT;


-- -----------------------------------------------------
-- Table `bazar`.`advance`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`advance` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `org_id` INT UNSIGNED NOT NULL,
  `cash_id` INT UNSIGNED NOT NULL,
  `date` DATE NOT NULL,
  `employee_id` INT UNSIGNED NOT NULL,
  `contractor_id` INT UNSIGNED NULL DEFAULT NULL,
  `user_id` INT UNSIGNED NULL DEFAULT NULL,
  `expense_id` INT UNSIGNED NOT NULL,
  `sum` DECIMAL(10,2) UNSIGNED NOT NULL DEFAULT 0,
  `details` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_advance_org_idx` (`org_id` ASC),
  INDEX `fk_advance_cash_idx` (`cash_id` ASC),
  INDEX `fk_advance_employee_idx` (`employee_id` ASC),
  INDEX `fk_advance_contractor_idx` (`contractor_id` ASC),
  INDEX `fk_advance_user_idx` (`user_id` ASC),
  INDEX `fk_advance_expense_idx` (`expense_id` ASC),
  CONSTRAINT `fk_advance_org`
    FOREIGN KEY (`org_id`)
    REFERENCES `bazar`.`organizations` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_cash`
    FOREIGN KEY (`cash_id`)
    REFERENCES `bazar`.`cash` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_employee`
    FOREIGN KEY (`employee_id`)
    REFERENCES `bazar`.`employees` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_contractor`
    FOREIGN KEY (`contractor_id`)
    REFERENCES `bazar`.`contractors` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_user`
    FOREIGN KEY (`user_id`)
    REFERENCES `bazar`.`users` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_expense`
    FOREIGN KEY (`expense_id`)
    REFERENCES `bazar`.`expense_items` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`base_parameters`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`base_parameters` (
  `name` VARCHAR(20) NOT NULL,
  `str_value` VARCHAR(100) NULL DEFAULT NULL,
  PRIMARY KEY (`name`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`meter_types`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`meter_types` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `bazar`.`meters`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`meters` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `meter_type_id` INT UNSIGNED NOT NULL,
  `place_type_id` INT UNSIGNED NOT NULL,
  `place_no` VARCHAR(10) NOT NULL,
  `parent_meter_id` INT UNSIGNED NULL DEFAULT NULL,
  `disabled` TINYINT(1) NOT NULL DEFAULT FALSE,
  PRIMARY KEY (`id`),
  INDEX `fk_meters_type_idx` (`meter_type_id` ASC),
  INDEX `fk_meters_place_idx` (`place_type_id` ASC, `place_no` ASC),
  INDEX `fk_meters_parent_idx` (`parent_meter_id` ASC),
  CONSTRAINT `fk_meters_type`
    FOREIGN KEY (`meter_type_id`)
    REFERENCES `bazar`.`meter_types` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_meters_place`
    FOREIGN KEY (`place_type_id` , `place_no`)
    REFERENCES `bazar`.`places` (`type_id` , `place_no`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_meters_parent`
    FOREIGN KEY (`parent_meter_id`)
    REFERENCES `bazar`.`meters` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1;


-- -----------------------------------------------------
-- Table `bazar`.`meter_tariffs`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`meter_tariffs` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `meter_type_id` INT UNSIGNED NOT NULL,
  `service_id` INT UNSIGNED NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_meter_tariffs_meter_type_idx` (`meter_type_id` ASC),
  INDEX `fk_meter_tariffs_service_idx` (`service_id` ASC),
  CONSTRAINT `fk_meter_tariffs_meter_type`
    FOREIGN KEY (`meter_type_id`)
    REFERENCES `bazar`.`meter_types` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_meter_tariffs_service`
    FOREIGN KEY (`service_id`)
    REFERENCES `bazar`.`services` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1;


-- -----------------------------------------------------
-- Table `bazar`.`meter_reading`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`meter_reading` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `date` DATE NOT NULL,
  `meter_id` INT UNSIGNED NOT NULL,
  `meter_tariff_id` INT UNSIGNED NOT NULL,
  `value` INT UNSIGNED NOT NULL,
  `accrual_pay_id` INT UNSIGNED NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_meter_reading_meter_idx` (`meter_id` ASC),
  INDEX `fk_meter_reading_tariff_idx` (`meter_tariff_id` ASC),
  INDEX `fk_meter_reading_accrual_pay_idx` (`accrual_pay_id` ASC),
  CONSTRAINT `fk_meter_reading_meter`
    FOREIGN KEY (`meter_id`)
    REFERENCES `bazar`.`meters` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_meter_reading_tariff`
    FOREIGN KEY (`meter_tariff_id`)
    REFERENCES `bazar`.`meter_tariffs` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_meter_reading_accrual_pay`
    FOREIGN KEY (`accrual_pay_id`)
    REFERENCES `bazar`.`accrual_pays` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

USE `bazar` ;

-- -----------------------------------------------------
-- Placeholder table for view `bazar`.`active_contracts`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`active_contracts` (`id` INT, `number` INT, `lessee_id` INT, `org_id` INT, `place_type_id` INT, `place_no` INT, `sign_date` INT, `start_date` INT, `end_date` INT, `pay_day` INT, `cancel_date` INT, `comments` INT);

-- -----------------------------------------------------
-- View `bazar`.`active_contracts`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bazar`.`active_contracts`;
USE `bazar`;
CREATE  OR REPLACE VIEW `active_contracts` AS
    SELECT 
        *
    FROM
        contracts
    WHERE
        ((contracts.cancel_date IS NULL
            AND CURDATE() BETWEEN contracts.start_date AND contracts.end_date)
            OR (contracts.cancel_date IS NOT NULL
            AND CURDATE() BETWEEN contracts.start_date AND contracts.cancel_date));

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

-- -----------------------------------------------------
-- Data for table `bazar`.`base_parameters`
-- -----------------------------------------------------
START TRANSACTION;
USE `bazar`;
INSERT INTO `bazar`.`base_parameters` (`name`, `str_value`) VALUES ('product_name', 'BazAr');
INSERT INTO `bazar`.`base_parameters` (`name`, `str_value`) VALUES ('version', '2.1');
INSERT INTO `bazar`.`base_parameters` (`name`, `str_value`) VALUES ('edition', 'gpl');

COMMIT;

