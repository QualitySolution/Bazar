ALTER TABLE `places` 
CHANGE COLUMN `place_no` `place_no` VARCHAR(20) NOT NULL ;

CREATE TABLE IF NOT EXISTS `read_news` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `user_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `feed_id` VARCHAR(64) NOT NULL,
  `items` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_read_news_user_id_idx` (`user_id` ASC),
  CONSTRAINT `fk_read_news_user_id`
    FOREIGN KEY (`user_id`)
    REFERENCES `users` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

CREATE TABLE IF NOT EXISTS `service_providers` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

SELECT count(*)
INTO @exist
FROM information_schema.columns 
WHERE table_schema = database()
and COLUMN_NAME = 'deactivated'
AND table_name = 'users';

set @query = IF(@exist <= 0, 'ALTER TABLE `users` 
ADD COLUMN `deactivated` TINYINT(1) NOT NULL DEFAULT 0 AFTER `login`,
ADD COLUMN `email` VARCHAR(60) NULL DEFAULT NULL AFTER `deactivated`;','');

prepare stmt from @query;

EXECUTE stmt;

ALTER TABLE `services` 
ADD COLUMN `service_provider_id` INT(10) UNSIGNED NULL DEFAULT NULL AFTER `incomplete_month`,
ADD INDEX `fk_services_service_provider_id_idx` (`service_provider_id` ASC);

ALTER TABLE `services` 
ADD CONSTRAINT `fk_services_service_provider_id`
  FOREIGN KEY (`service_provider_id`)
  REFERENCES `service_providers` (`id`)
  ON DELETE SET NULL
  ON UPDATE CASCADE;


-- -----------------------------------------------------
-- Placeholder table for view `meter_reading_with_prev`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `meter_reading_with_prev` (`id` INT, `date` INT, `meter_id` INT, `meter_tariff_id` INT, `accrual_pay_id` INT, `value` INT, `prev_value` INT);

-- -----------------------------------------------------
-- Placeholder table for view `reading_with_prev_date`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `reading_with_prev_date` (`id` INT, `date` INT, `meter_id` INT, `meter_tariff_id` INT, `value` INT, `accrual_pay_id` INT, `prev_date` INT);

-- -----------------------------------------------------
-- Placeholder table for view `reading_with_prev_date_subquery`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `reading_with_prev_date_subquery` (`id` INT, `date` INT, `meter_id` INT, `meter_tariff_id` INT, `value` INT, `accrual_pay_id` INT, `last_reading_id` INT, `lesser_date` INT, `lesser_value` INT);

-- -----------------------------------------------------
-- View `meter_reading_with_prev`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `meter_reading_with_prev`;

CREATE 
 OR REPLACE VIEW `meter_reading_with_prev` AS
    select 
        `reading_with_prev_date`.`id` AS `id`,
        `reading_with_prev_date`.`date` AS `date`,
        `reading_with_prev_date`.`meter_id` AS `meter_id`,
        `reading_with_prev_date`.`meter_tariff_id` AS `meter_tariff_id`,
        `reading_with_prev_date`.`accrual_pay_id` AS `accrual_pay_id`,
        `reading_with_prev_date`.`value` AS `value`,
        `meter_reading`.`value` AS `prev_value`
    from
        (`reading_with_prev_date`
        left join `meter_reading` ON (((`meter_reading`.`meter_id` = `reading_with_prev_date`.`meter_id`)
            and (`meter_reading`.`meter_tariff_id` = `reading_with_prev_date`.`meter_tariff_id`)
            and (`meter_reading`.`date` = `reading_with_prev_date`.`prev_date`))));

-- -----------------------------------------------------
-- View `reading_with_prev_date`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `reading_with_prev_date`;

CREATE  OR REPLACE VIEW `reading_with_prev_date` AS
    select 
        `reading_with_prev_date_subquery`.`id` AS `id`,
        `reading_with_prev_date_subquery`.`date` AS `date`,
        `reading_with_prev_date_subquery`.`meter_id` AS `meter_id`,
        `reading_with_prev_date_subquery`.`meter_tariff_id` AS `meter_tariff_id`,
        `reading_with_prev_date_subquery`.`value` AS `value`,
        `reading_with_prev_date_subquery`.`accrual_pay_id` AS `accrual_pay_id`,
        max(`reading_with_prev_date_subquery`.`lesser_date`) AS `prev_date`
    from
        `reading_with_prev_date_subquery`
    group by `reading_with_prev_date_subquery`.`meter_id` , `reading_with_prev_date_subquery`.`meter_tariff_id` , `reading_with_prev_date_subquery`.`date`;

-- -----------------------------------------------------
-- View `reading_with_prev_date_subquery`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `reading_with_prev_date_subquery`;

CREATE  OR REPLACE VIEW `reading_with_prev_date_subquery` AS
    select 
        `a`.`id` AS `id`,
        `a`.`date` AS `date`,
        `a`.`meter_id` AS `meter_id`,
        `a`.`meter_tariff_id` AS `meter_tariff_id`,
        `a`.`value` AS `value`,
        `a`.`accrual_pay_id` AS `accrual_pay_id`,
        `b`.`date` AS `lesser_date`,
        `b`.`value` AS `lesser_value`
    from
        (`meter_reading` as `a`
        left join `meter_reading` as `b` ON (((`a`.`meter_id` = `b`.`meter_id`)
            and (`a`.`meter_tariff_id` = `b`.`meter_tariff_id`)
            and (`a`.`date` >= `b`.`date`)
            and (`a`.`id` <> `b`.`id`))));

-- Исправляем договора, проставляем даты подписания. Без этого есть проблемы с пролонгацией.

UPDATE contracts SET sign_date = start_date WHERE sign_date IS NULL;

-- Обновляем версию базы.

DELETE FROM base_parameters WHERE name = 'micro_updates';
UPDATE base_parameters SET str_value = '2.3' WHERE name = 'version';
