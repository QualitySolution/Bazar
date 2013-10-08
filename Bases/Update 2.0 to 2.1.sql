SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

#Создаем id
ALTER TABLE `bazar`.`contracts` ADD COLUMN `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT  FIRST 
, DROP PRIMARY KEY 
, ADD PRIMARY KEY (`id`) ;

#Создаем колонки в других таблицах.
ALTER TABLE `bazar`.`contract_pays` ADD COLUMN `contract_id` INT(10) UNSIGNED NOT NULL  AFTER `id`;
ALTER TABLE `bazar`.`credit_slips` ADD COLUMN `contract_id` INT(10) UNSIGNED NULL DEFAULT NULL  AFTER `sum`;
ALTER TABLE `bazar`.`accrual` ADD COLUMN `contract_id` INT(10) UNSIGNED NOT NULL  AFTER `user_id`;

#Присваиваем значения id договоров.
UPDATE accrual SET contract_id = 
(SELECT contracts.id FROM contracts WHERE accrual.contract_no = contracts.number);
UPDATE credit_slips SET contract_id = 
(SELECT contracts.id FROM contracts WHERE credit_slips.contract_no = contracts.number);
UPDATE contract_pays SET contract_id = 
(SELECT contracts.id FROM contracts WHERE contract_pays.contract_no = contracts.number);

#Создаем новые индексы удаляем старую колонку.
ALTER TABLE `bazar`.`contract_pays` DROP FOREIGN KEY `contract_no`; 
ALTER TABLE `bazar`.`contract_pays` DROP INDEX `contract_no_idx`, DROP COLUMN `contract_no`;
ALTER TABLE `bazar`.`contract_pays` 
  ADD CONSTRAINT `fk_contract_pays_contract_id`
  FOREIGN KEY (`contract_id` )
  REFERENCES `bazar`.`contracts` (`id` )
  ON DELETE CASCADE
  ON UPDATE CASCADE
, ADD INDEX `fk_contract_pays_contract_id_idx` (`contract_id` ASC) ;

ALTER TABLE `bazar`.`credit_slips` DROP FOREIGN KEY `fk_credit_contract_no` ;
ALTER TABLE `bazar`.`credit_slips` DROP INDEX `fk_credit_contract_no_idx`, DROP COLUMN `contract_no` ; 
ALTER TABLE `bazar`.`credit_slips` 
  ADD CONSTRAINT `fk_credit_slips_contract_id`
  FOREIGN KEY (`contract_id` )
  REFERENCES `bazar`.`contracts` (`id` )
  ON DELETE SET NULL
  ON UPDATE CASCADE
, ADD INDEX `fk_credit_slips_contract_id_idx` (`contract_id` ASC) ;

ALTER TABLE `bazar`.`accrual` DROP FOREIGN KEY `fk_accrual_contract_no` ;
ALTER TABLE `bazar`.`accrual` DROP INDEX `Contract_per_month`, DROP COLUMN `contract_no` ,
  ADD CONSTRAINT `fk_accrual_contract_id`
  FOREIGN KEY (`contract_id` )
  REFERENCES `bazar`.`contracts` (`id` )
  ON DELETE CASCADE
  ON UPDATE CASCADE
, ADD INDEX `fk_accrual_contract_id_idx` (`contract_id` ASC)
, ADD UNIQUE INDEX `Contract_per_month` (`contract_id` ASC, `year` ASC, `month` ASC)  ;

#Заменяем основной индекс.
ALTER TABLE `bazar`.`contracts` 
ADD UNIQUE INDEX `unique_number_date` (`number` ASC, `sign_date` ASC) 
, DROP INDEX `Number_UNIQUE`;

ALTER TABLE `bazar`.`credit_slips` 
DROP FOREIGN KEY `fk_credit_income_id`;

ALTER TABLE `bazar`.`credit_slips` 
CHANGE COLUMN `id` `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT ,
CHANGE COLUMN `operation` `operation` ENUM('common','advance','payment') NOT NULL DEFAULT 'common' ;

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

ALTER TABLE `bazar`.`services` 
ADD COLUMN `by_area` TINYINT(1) NOT NULL DEFAULT FALSE AFTER `income_id`;

ALTER TABLE `bazar`.`contract_pays` 
ADD COLUMN `min_sum` DECIMAL(10,2) NULL DEFAULT NULL AFTER `price`;

ALTER TABLE `bazar`.`users` 
CHANGE COLUMN `admin` `admin` TINYINT(1) NOT NULL DEFAULT FALSE ,
CHANGE COLUMN `edit_slips` `edit_slips` TINYINT(1) NOT NULL DEFAULT FALSE ;

ALTER TABLE `bazar`.`accrual` 
CHANGE COLUMN `paid` `paid` TINYINT(1) NOT NULL DEFAULT FALSE ,
ADD COLUMN `comments` TEXT NULL DEFAULT NULL AFTER `no_complete`,
DROP INDEX `Contract_per_month` ,
ADD UNIQUE INDEX `Contract_per_month` USING BTREE (`contract_id` ASC, `year` ASC, `month` ASC);

CREATE TABLE IF NOT EXISTS `bazar`.`base_parameters` (
  `name` VARCHAR(20) NOT NULL,
  `str_value` VARCHAR(100) NULL DEFAULT NULL,
  PRIMARY KEY (`name`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

CREATE TABLE IF NOT EXISTS `bazar`.`meters` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `meter_type_id` INT(10) UNSIGNED NOT NULL,
  `place_type_id` INT(10) UNSIGNED NOT NULL,
  `place_no` VARCHAR(10) NOT NULL,
  `parent_meter_id` INT(10) UNSIGNED NULL DEFAULT NULL,
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
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

CREATE TABLE IF NOT EXISTS `bazar`.`meter_types` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

CREATE TABLE IF NOT EXISTS `bazar`.`meter_tariffs` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `meter_type_id` INT(10) UNSIGNED NOT NULL,
  `service_id` INT(10) UNSIGNED NULL DEFAULT NULL,
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
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

CREATE TABLE IF NOT EXISTS `bazar`.`meter_reading` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `date` DATE NOT NULL,
  `meter_id` INT(10) UNSIGNED NOT NULL,
  `meter_tariff_id` INT(10) UNSIGNED NOT NULL,
  `value` INT(10) UNSIGNED NOT NULL,
  `accrual_pay_id` INT(10) UNSIGNED NULL DEFAULT NULL,
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
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

ALTER TABLE `bazar`.`credit_slips` 
ADD CONSTRAINT `fk_credit_income_id`
  FOREIGN KEY (`income_id`)
  REFERENCES `bazar`.`income_items` (`id`)
  ON DELETE CASCADE
  ON UPDATE CASCADE;


-- -----------------------------------------------------
-- Placeholder table for view `bazar`.`active_contracts`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bazar`.`active_contracts` (`id` INT, `number` INT, `lessee_id` INT, `org_id` INT, `place_type_id` INT, `place_no` INT, `sign_date` INT, `start_date` INT, `end_date` INT, `pay_day` INT, `cancel_date` INT, `comments` INT);


USE `bazar`;

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

INSERT INTO `base_parameters` (`name`,`str_value`) VALUES ('product_name','BazAr');
INSERT INTO `base_parameters` (`name`,`str_value`) VALUES ('version','2.1');
INSERT INTO `base_parameters` (`name`,`str_value`) VALUES ('edition','gpl');

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
