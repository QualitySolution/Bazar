ALTER TABLE `bazar`.`meter_types` 
ADD COLUMN `kilowatt_factor` DOUBLE UNSIGNED NOT NULL DEFAULT 1 AFTER `name`;
