SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

USE `bazar`;

#Всвязи с некорректной работой модуля разнесения оплат в базе можно было сохранять оплаты с нулевыми суммами или без ссылок на строку начисления что приводило к проблемам
#Удаляем из базы все некорректные строки.
DELETE FROM payment_details WHERE sum = 0 OR accrual_pay_id IS NULL;

ALTER TABLE `bazar`.`payment_details` 
DROP FOREIGN KEY `fk_pay_details_accrual`,
DROP FOREIGN KEY `fk_pay_details_income`;

ALTER TABLE `bazar`.`lessees` 
ADD COLUMN `KPP` VARCHAR(10) NULL DEFAULT NULL AFTER `INN`;

ALTER TABLE `bazar`.`payment_details`
CHANGE COLUMN `accrual_pay_id` `accrual_pay_id` INT(10) UNSIGNED NOT NULL ,
CHANGE COLUMN `sum` `sum` DECIMAL(10,2) NOT NULL DEFAULT '0' ;

ALTER TABLE `bazar`.`services` 
CHANGE COLUMN `by_area` `by_area` TINYINT(1) NOT NULL DEFAULT FALSE ,
ADD COLUMN `incomplete_month` TINYINT(1) NOT NULL DEFAULT 0 AFTER `by_area`;

ALTER TABLE `bazar`.`contract_pays` 
CHANGE COLUMN `count` `count` DECIMAL(10,2) UNSIGNED NOT NULL DEFAULT 1 ,
CHANGE COLUMN `price` `price` DECIMAL(10,2) UNSIGNED NOT NULL DEFAULT 0 ;

ALTER TABLE `bazar`.`cash` 
ADD COLUMN `color` VARCHAR(15) NULL DEFAULT NULL AFTER `name`;

ALTER TABLE `bazar`.`users` 
CHANGE COLUMN `admin` `admin` TINYINT(1) NOT NULL DEFAULT FALSE ,
CHANGE COLUMN `edit_slips` `edit_slips` TINYINT(1) NOT NULL DEFAULT FALSE ;

ALTER TABLE `bazar`.`accrual` 
CHANGE COLUMN `paid` `paid` TINYINT(1) NOT NULL DEFAULT FALSE ;

ALTER TABLE `bazar`.`accrual_pays` 
CHANGE COLUMN `count` `count` DECIMAL(10,2) UNSIGNED NOT NULL DEFAULT 1 ,
CHANGE COLUMN `price` `price` DECIMAL(10,2) UNSIGNED NOT NULL DEFAULT 0 ;

ALTER TABLE `bazar`.`meters` 
CHANGE COLUMN `disabled` `disabled` TINYINT(1) NOT NULL DEFAULT FALSE ;

ALTER TABLE `bazar`.`payment_details` 
ADD CONSTRAINT `fk_pay_details_accrual`
  FOREIGN KEY (`accrual_pay_id`)
  REFERENCES `bazar`.`accrual_pays` (`id`)
  ON DELETE CASCADE
  ON UPDATE CASCADE,
ADD CONSTRAINT `fk_pay_details_income`
  FOREIGN KEY (`income_id`)
  REFERENCES `bazar`.`income_items` (`id`)
  ON DELETE CASCADE
  ON UPDATE CASCADE;

-- Обновляем версию базы
UPDATE `bazar`.`base_parameters` SET `str_value` = '2.2' WHERE `name` = 'version';

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
