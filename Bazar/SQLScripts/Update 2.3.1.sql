ALTER TABLE `contracts` 
DROP FOREIGN KEY `fk_place_id`;

ALTER TABLE `events` 
CHANGE COLUMN `place_no` `place_no` VARCHAR(20) NULL DEFAULT NULL ;

ALTER TABLE `contracts` 
CHANGE COLUMN `place_no` `place_no` VARCHAR(20) NOT NULL ;

ALTER TABLE `meters` 
CHANGE COLUMN `place_no` `place_no` VARCHAR(20) NOT NULL ,
CHANGE COLUMN `disabled` `disabled` TINYINT(1) NOT NULL DEFAULT FALSE ;

ALTER TABLE `contracts` 
ADD CONSTRAINT `fk_place_id`
  FOREIGN KEY (`place_type_id` , `place_no`)
  REFERENCES `places` (`type_id` , `place_no`)
  ON DELETE CASCADE
  ON UPDATE CASCADE;

ALTER TABLE `meters` 
DROP FOREIGN KEY `fk_meters_place`;

ALTER TABLE `meters` ADD CONSTRAINT `fk_meters_place`
  FOREIGN KEY (`place_type_id` , `place_no`)
  REFERENCES `places` (`type_id` , `place_no`)
  ON DELETE CASCADE
  ON UPDATE CASCADE;