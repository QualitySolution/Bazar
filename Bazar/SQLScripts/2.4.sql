﻿-- Обновление с 2.3 на 2.4

-- Удаляем старые связи

ALTER TABLE `contracts` 
DROP FOREIGN KEY `fk_place_id`;

ALTER TABLE `meters` 
DROP FOREIGN KEY `fk_meters_place`;

-- Создаем новые таблицы

CREATE TABLE IF NOT EXISTS `document_last_numbers` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `doc_type` ENUM('Invoice') NULL DEFAULT NULL,
  `year` INT(10) UNSIGNED NOT NULL,
  `number` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `index2` (`doc_type` ASC, `year` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

-- Создаем новые поля

ALTER TABLE `organizations` 
ADD COLUMN `print_name` VARCHAR(200) NULL DEFAULT NULL AFTER `name`,
ADD COLUMN `inn` VARCHAR(12) NULL DEFAULT NULL AFTER `print_name`,
ADD COLUMN `kpp` VARCHAR(10) NULL DEFAULT NULL AFTER `inn`,
ADD COLUMN `jur_address` VARCHAR(200) NULL DEFAULT NULL AFTER `kpp`,
ADD COLUMN `phone` VARCHAR(16) NULL DEFAULT NULL AFTER `jur_address`,
ADD COLUMN `bank_bik` VARCHAR(9) NULL DEFAULT NULL AFTER `phone`,
ADD COLUMN `bank_account` VARCHAR(25) NULL DEFAULT NULL AFTER `bank_bik`,
ADD COLUMN `bank_cor_account` VARCHAR(25) NULL DEFAULT NULL AFTER `bank_account`,
ADD COLUMN `bank_name` VARCHAR(200) NULL DEFAULT NULL AFTER `bank_cor_account`,
ADD COLUMN `leader_sign` VARCHAR(60) NULL DEFAULT NULL AFTER `bank_name`,
ADD COLUMN `buhgalter_sign` VARCHAR(60) NULL DEFAULT NULL AFTER `leader_sign`;

ALTER TABLE `services` 
ADD COLUMN `place_set` ENUM('Required', 'Allowed', 'Prohibited') NOT NULL DEFAULT 'Allowed' AFTER `service_provider_id`,
ADD COLUMN `place_occupy` TINYINT(1) NOT NULL DEFAULT 1 AFTER `place_set`;

ALTER TABLE `places` 
ADD COLUMN `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT FIRST,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`id`);

ALTER TABLE `contract_pays` 
ADD COLUMN `place_id` INT(10) UNSIGNED NULL DEFAULT NULL AFTER `service_id`,
ADD INDEX `fk_contract_pays_1_idx1` (`place_id` ASC);

ALTER TABLE `accrual_pays` 
ADD COLUMN `place_id` INT(10) UNSIGNED NULL DEFAULT NULL AFTER `service_id`,
ADD INDEX `fk_accrual_pays_1_idx` (`place_id` ASC);

ALTER TABLE `meters` 
ADD COLUMN `place_id` INT(10) UNSIGNED NOT NULL AFTER `name`,
ADD INDEX `fk_meters_1_idx` (`place_id` ASC);

ALTER TABLE `units` 
ADD COLUMN `digits` TINYINT(3) UNSIGNED NOT NULL DEFAULT 0 AFTER `name`,
ADD COLUMN `okei` VARCHAR(3) NULL DEFAULT NULL AFTER `digits`;

ALTER TABLE `accrual` 
ADD COLUMN `date` DATE NULL DEFAULT NULL AFTER `contract_id`,
ADD COLUMN `invoice_number` INT(10) UNSIGNED NULL DEFAULT NULL AFTER `date`,
CHANGE COLUMN `paid` `paid` TINYINT(1) NOT NULL DEFAULT FALSE ,
ADD UNIQUE INDEX `index4` (`contract_id` ASC, `date` ASC),
ADD UNIQUE INDEX `index5` (`year` ASC, `invoice_number` ASC),
DROP INDEX `Contract_per_month` ;

-- Миграция из старых данных в новые

UPDATE contract_pays, contracts, places
SET contract_pays.place_id = places.id
WHERE contract_pays.contract_id = contracts.id AND contracts.place_type_id = places.type_id AND contracts.place_no = places.place_no;

UPDATE accrual_pays, accrual, contracts, places
SET accrual_pays.place_id = places.id
WHERE accrual_pays.accrual_id = accrual.id AND accrual.contract_id = contracts.id AND contracts.place_type_id = places.type_id AND contracts.place_no = places.place_no;

UPDATE meters, places
SET meters.place_id = places.id
WHERE meters.place_type_id = places.type_id AND meters.place_no = places.place_no;

-- Удаляем старые поля

ALTER TABLE `contracts` 
DROP COLUMN `place_no`,
DROP COLUMN `place_type_id`,
DROP INDEX `fk_place_id_idx` ;

ALTER TABLE `meters` 
DROP COLUMN `place_no`,
DROP COLUMN `place_type_id`,
DROP INDEX `fk_meters_place_idx` ;

-- Создаем индексы

ALTER TABLE `contract_pays` 
ADD CONSTRAINT `fk_contract_pays_1`
  FOREIGN KEY (`place_id`)
  REFERENCES `places` (`id`)
  ON DELETE RESTRICT
  ON UPDATE CASCADE;

ALTER TABLE `accrual_pays` 
ADD CONSTRAINT `fk_accrual_pays_1`
  FOREIGN KEY (`place_id`)
  REFERENCES `places` (`id`)
  ON DELETE RESTRICT
  ON UPDATE CASCADE;
  
ALTER TABLE `meters` 
ADD CONSTRAINT `fk_meters_1`
  FOREIGN KEY (`place_id`)
  REFERENCES `places` (`id`)
  ON DELETE RESTRICT
  ON UPDATE CASCADE;
  
-- Добавляем новое представление

CREATE OR REPLACE VIEW `current_place_leases` AS
    SELECT 
        contracts.*,
        contract_pays.place_id,
        IFNULL(contracts.cancel_date, contracts.end_date) AS finish_date
    FROM
        contract_pays,
        contracts
    WHERE
        contract_pays.place_id IS NOT NULL
            AND contract_pays.contract_id = contracts.id
            AND CURDATE() BETWEEN contracts.start_date AND IFNULL(contracts.cancel_date, contracts.end_date)
    GROUP BY place_id;

-- Версия базы
DELETE FROM base_parameters WHERE name = 'micro_updates';
UPDATE base_parameters SET str_value = '2.4' WHERE name = 'version';
