#Создаем id
ALTER TABLE `bazar`.`contracts` ADD COLUMN `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT  FIRST 
, DROP PRIMARY KEY 
, ADD PRIMARY KEY (`id`) ;
ALTER TABLE `bazar`.`contracts` 
ADD UNIQUE INDEX `unique_number_date` (`number` ASC, `sign_date` ASC) 
, DROP INDEX `Number_UNIQUE`;

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