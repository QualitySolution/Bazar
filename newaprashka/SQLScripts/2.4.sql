-- Обновление с 2.3 на 2.4

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

-- Миграция из старых данных в новые

UPDATE contract_pays, contracts, places
SET contract_pays.place_id = places.id
WHERE contract_pays.contract_id = contracts.id AND contracts.place_type_id = places.type_id AND contracts.place_no = places.place_no;

UPDATE accrual_pays, accrual, contracts, places
SET accrual_pays.place_id = places.id
WHERE accrual_pays.accrual_id = accrual.id AND accrual.contract_id = contracts.id AND contracts.place_type_id = places.type_id AND contracts.place_no = places.place_no;

-- Удаляем старые поля
ALTER TABLE `contracts` 
DROP FOREIGN KEY `fk_place_id`;

ALTER TABLE `contracts` 
DROP COLUMN `place_no`,
DROP COLUMN `place_type_id`,
DROP INDEX `fk_place_id_idx` ;
;

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

-- Версия базы
DELETE FROM base_parameters WHERE name = 'micro_updates';
UPDATE base_parameters SET str_value = '2.4' WHERE name = 'version';