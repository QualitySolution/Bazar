-- Обновление с 2.3 на 2.4

-- Удаляем старые связи

ALTER TABLE `contracts` 
DROP FOREIGN KEY `fk_place_id`;

ALTER TABLE `meters` 
DROP FOREIGN KEY `fk_meters_place`;

-- Модуль банков

ALTER TABLE `organizations` 
ADD COLUMN `print_name` VARCHAR(200) NULL DEFAULT NULL AFTER `name`,
ADD COLUMN `INN` VARCHAR(12) NULL DEFAULT NULL AFTER `print_name`,
ADD COLUMN `KPP` VARCHAR(10) NULL DEFAULT NULL AFTER `INN`,
ADD COLUMN `jur_address` VARCHAR(200) NULL DEFAULT NULL AFTER `KPP`,
ADD COLUMN `phone` VARCHAR(16) NULL DEFAULT NULL AFTER `jur_address`;

CREATE TABLE IF NOT EXISTS `bank_accounts` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NULL DEFAULT NULL,
  `number` VARCHAR(25) NULL DEFAULT NULL,
  `bank_id` INT(10) UNSIGNED NOT NULL,
  `organization_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `inactive` TINYINT(1) NOT NULL DEFAULT 0,
  `is_default` TINYINT(1) NOT NULL DEFAULT 0,
  `bank_cor_account_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_bank_accounts_1_idx` (`bank_id` ASC),
  INDEX `fk_bank_accounts_2_idx` (`organization_id` ASC),
  INDEX `fk_bank_accounts_3_idx` (`bank_cor_account_id` ASC),
  CONSTRAINT `fk_bank_accounts_1`
    FOREIGN KEY (`bank_id`)
    REFERENCES `bazar_dev`.`banks` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT `fk_bank_accounts_2`
    FOREIGN KEY (`organization_id`)
    REFERENCES `bazar_dev`.`organizations` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_bank_accounts_3`
    FOREIGN KEY (`bank_cor_account_id`)
    REFERENCES `bazar_dev`.`bank_cor_accounts` (`id`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE TABLE IF NOT EXISTS `bank_regions` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `region` VARCHAR(50) NULL DEFAULT NULL,
  `city` VARCHAR(45) NULL DEFAULT NULL,
  `region_num` INT(11) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE TABLE IF NOT EXISTS `bank_cor_accounts` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `cor_account_number` VARCHAR(25) NOT NULL,
  `bank_id` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_bank_cor_accounts_1_idx` (`bank_id` ASC),
  CONSTRAINT `fk_bank_cor_accounts_1`
    FOREIGN KEY (`bank_id`)
    REFERENCES `bazar_dev`.`banks` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE TABLE IF NOT EXISTS `banks` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(200) NULL DEFAULT NULL,
  `bik` VARCHAR(9) NULL DEFAULT NULL,
  `cor_account` VARCHAR(25) NULL DEFAULT NULL,
  `city` VARCHAR(45) NULL DEFAULT NULL,
  `deleted` TINYINT(1) NOT NULL DEFAULT 0,
  `region_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `default_cor_account_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_banks_1_idx` (`region_id` ASC),
  INDEX `fk_banks_2_idx` (`default_cor_account_id` ASC),
  CONSTRAINT `fk_banks_1`
    FOREIGN KEY (`region_id`)
    REFERENCES `bazar_dev`.`bank_regions` (`id`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE,
  CONSTRAINT `fk_banks_2`
    FOREIGN KEY (`default_cor_account_id`)
    REFERENCES `bazar_dev`.`bank_cor_accounts` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

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
