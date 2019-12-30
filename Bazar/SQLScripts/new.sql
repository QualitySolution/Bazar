-- MySQL Workbench Forward Engineering

-- -----------------------------------------------------
-- Table `classes`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `classes` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `contact_persons`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `contact_persons` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `telephones` VARCHAR(100) NULL DEFAULT NULL,
  `comments` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `events`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `events` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `date` DATETIME NOT NULL,
  `class_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `user_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `user` VARCHAR(20) NULL DEFAULT NULL,
  `cause` TEXT NULL DEFAULT NULL,
  `activity` TEXT NULL DEFAULT NULL,
  `lessee_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `place_type_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `place_no` VARCHAR(20) NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `places_id` (`place_type_id` ASC, `place_no` ASC),
  INDEX `classes_id` (`class_id` ASC),
  INDEX `lessees` (`lessee_id` ASC),
  INDEX `class_key` (`class_id` ASC),
  INDEX `lessees_key` (`lessee_id` ASC),
  INDEX `place_key` (`place_type_id` ASC, `place_no` ASC),
  FULLTEXT INDEX `cause_idx` (`cause`),
  FULLTEXT INDEX `activity_idx` (`activity`))
ENGINE = MyISAM
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `goods`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `goods` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `lessees`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `lessees` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(100) NOT NULL,
  `FIO_dir` VARCHAR(45) NULL DEFAULT NULL,
  `passport_ser` VARCHAR(5) NULL DEFAULT NULL,
  `passport_no` VARCHAR(6) NULL DEFAULT NULL,
  `passport_exit` VARCHAR(100) NULL DEFAULT NULL,
  `address` VARCHAR(100) NULL DEFAULT NULL,
  `INN` VARCHAR(12) NULL DEFAULT NULL,
  `KPP` VARCHAR(10) NULL DEFAULT NULL,
  `OGRN` VARCHAR(15) NULL DEFAULT NULL,
  `wholesaler` TINYINT(1) NULL DEFAULT '0',
  `retail` TINYINT(1) NULL DEFAULT '0',
  `goods_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `comments` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `goods_id_idx` (`goods_id` ASC),
  CONSTRAINT `goods_id`
    FOREIGN KEY (`goods_id`)
    REFERENCES `goods` (`id`)
    ON DELETE SET NULL
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `users` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `login` VARCHAR(45) NOT NULL,
  `deactivated` TINYINT(1) NOT NULL DEFAULT 0,
  `email` VARCHAR(60) NULL,
  `description` TEXT NULL DEFAULT NULL,
  `admin` TINYINT(1) NOT NULL DEFAULT FALSE,
  `edit_slips` TINYINT(1) NOT NULL DEFAULT FALSE,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `organizations`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `organizations` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(100) NULL DEFAULT NULL,
  `print_name` VARCHAR(200) NULL DEFAULT NULL,
  `inn` VARCHAR(12) NULL DEFAULT NULL,
  `kpp` VARCHAR(10) NULL DEFAULT NULL,
  `jur_address` VARCHAR(200) NULL DEFAULT NULL,
  `phone` VARCHAR(16) NULL DEFAULT NULL,
  `bank_bik` VARCHAR(9) NULL DEFAULT NULL,
  `bank_account` VARCHAR(25) NULL DEFAULT NULL,
  `bank_cor_account` VARCHAR(25) NULL DEFAULT NULL,
  `bank_name` VARCHAR(200) NULL DEFAULT NULL,
  `leader_sign` VARCHAR(60) NULL DEFAULT NULL,
  `buhgalter_sign` VARCHAR(60) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `contracts`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `contracts` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `number` VARCHAR(15) NOT NULL,
  `lessee_id` INT UNSIGNED NOT NULL,
  `org_id` INT UNSIGNED NOT NULL,
  `sign_date` DATE NULL,
  `start_date` DATE NOT NULL,
  `end_date` DATE NOT NULL,
  `pay_day` INT NULL DEFAULT NULL,
  `cancel_date` DATE NULL DEFAULT NULL,
  `comments` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_lessee_id_idx` (`lessee_id` ASC),
  INDEX `fk_org_id_idx` (`org_id` ASC),
  UNIQUE INDEX `unique_number_date` (`number` ASC, `sign_date` ASC, `org_id` ASC),
  CONSTRAINT `fk_lessee_id`
    FOREIGN KEY (`lessee_id`)
    REFERENCES `lessees` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_org_id`
    FOREIGN KEY (`org_id`)
    REFERENCES `organizations` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `accrual`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `accrual` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `user_id` INT UNSIGNED NULL DEFAULT NULL,
  `contract_id` INT UNSIGNED NOT NULL,
  `date` DATE NULL DEFAULT NULL,
  `invoice_number` INT UNSIGNED NULL DEFAULT NULL,
  `month` TINYINT UNSIGNED NOT NULL,
  `year` INT UNSIGNED NOT NULL,
  `paid` TINYINT(1) NOT NULL DEFAULT FALSE,
  `no_complete` TINYINT(1) NOT NULL DEFAULT TRUE,
  `comments` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_accrual_1_idx` (`user_id` ASC),
  INDEX `fk_accrual_contract_id_idx` (`contract_id` ASC),
  UNIQUE INDEX `index4` (`contract_id` ASC, `date` ASC),
  UNIQUE INDEX `index5` (`year` ASC, `invoice_number` ASC),
  CONSTRAINT `fk_accrual_user_id`
    FOREIGN KEY (`user_id`)
    REFERENCES `users` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_accrual_contract_id`
    FOREIGN KEY (`contract_id`)
    REFERENCES `contracts` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `cash`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `cash` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `color` VARCHAR(15) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `employees`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `employees` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `income_items`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `income_items` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `credit_slips`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `credit_slips` (
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
    REFERENCES `organizations` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_cash_id`
    FOREIGN KEY (`cash_id`)
    REFERENCES `cash` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_lessee_id`
    FOREIGN KEY (`lessee_id`)
    REFERENCES `lessees` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `user_id`
    FOREIGN KEY (`user_id`)
    REFERENCES `users` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_employee_id`
    FOREIGN KEY (`employee_id`)
    REFERENCES `employees` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_slips_accural`
    FOREIGN KEY (`accrual_id`)
    REFERENCES `accrual` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_slips_contract_id`
    FOREIGN KEY (`contract_id`)
    REFERENCES `contracts` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_income_id`
    FOREIGN KEY (`income_id`)
    REFERENCES `income_items` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `payments`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `payments` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `createdate` DATETIME NULL DEFAULT NULL,
  `credit_slip_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `accrual_id` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_pays_credit_slips_idx` (`credit_slip_id` ASC),
  INDEX `fk_payments_accrual_idx` (`accrual_id` ASC),
  CONSTRAINT `fk_payments_accrual`
    FOREIGN KEY (`accrual_id`)
    REFERENCES `accrual` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_pays_credit_slips`
    FOREIGN KEY (`credit_slip_id`)
    REFERENCES `credit_slips` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 0
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `units`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `units` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(10) NOT NULL,
  `digits` TINYINT(3) UNSIGNED NOT NULL DEFAULT 0,
  `okei` VARCHAR(3) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `service_providers`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `service_providers` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 11
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `services`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `services` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `units_id` INT UNSIGNED NOT NULL,
  `income_id` INT UNSIGNED NULL DEFAULT NULL,
  `by_area` TINYINT(1) NOT NULL DEFAULT FALSE,
  `incomplete_month` TINYINT(1) NOT NULL DEFAULT 0,
  `service_provider_id` INT UNSIGNED NULL DEFAULT NULL,
  `place_set` ENUM('Required', 'Allowed', 'Prohibited') NOT NULL DEFAULT 'Allowed',
  `place_occupy` TINYINT(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`id`),
  INDEX `units_id_idx` (`units_id` ASC),
  UNIQUE INDEX `name_UNIQUE` (`name` ASC),
  INDEX `fk_services_income_id_idx` (`income_id` ASC),
  INDEX `fk_services_service_provider_id_idx` (`service_provider_id` ASC),
  CONSTRAINT `units_id`
    FOREIGN KEY (`units_id`)
    REFERENCES `units` (`id`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_services_income_id`
    FOREIGN KEY (`income_id`)
    REFERENCES `income_items` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_services_service_provider_id`
    FOREIGN KEY (`service_provider_id`)
    REFERENCES `service_providers` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `place_types`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `place_types` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(10) NULL DEFAULT NULL,
  `description` VARCHAR(100) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `places`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `places` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `type_id` INT(10) UNSIGNED NOT NULL,
  `place_no` VARCHAR(20) NOT NULL,
  `area` FLOAT(10,2) NULL DEFAULT NULL,
  `contact_person_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `org_id` INT UNSIGNED NULL DEFAULT NULL,
  `comments` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `types_id_idx` (`type_id` ASC),
  INDEX `contacts_idx` (`contact_person_id` ASC),
  INDEX `org_id_idx` (`org_id` ASC),
  CONSTRAINT `contacts`
    FOREIGN KEY (`contact_person_id`)
    REFERENCES `contact_persons` (`id`)
    ON DELETE SET NULL
    ON UPDATE NO ACTION,
  CONSTRAINT `types_id`
    FOREIGN KEY (`type_id`)
    REFERENCES `place_types` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT `org_id`
    FOREIGN KEY (`org_id`)
    REFERENCES `organizations` (`id`)
    ON DELETE SET NULL
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `accrual_pays`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `accrual_pays` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `accrual_id` INT UNSIGNED NOT NULL,
  `service_id` INT UNSIGNED NOT NULL,
  `place_id` INT UNSIGNED NULL DEFAULT NULL,
  `cash_id` INT UNSIGNED NOT NULL,
  `count` DECIMAL(10,2) UNSIGNED NOT NULL DEFAULT 1,
  `price` DECIMAL(10,2) UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  INDEX `fk_accrual_pays_accrual_id_idx` (`accrual_id` ASC),
  INDEX `fk_accrual_pays_service_id_idx` (`service_id` ASC),
  INDEX `fk_accrual_pays_cash_idx` (`cash_id` ASC),
  INDEX `fk_accrual_pays_1_idx` (`place_id` ASC),
  CONSTRAINT `fk_accrual_pays_accrual_id`
    FOREIGN KEY (`accrual_id`)
    REFERENCES `accrual` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_accrual_pays_service_id`
    FOREIGN KEY (`service_id`)
    REFERENCES `services` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_accrual_pays_cash`
    FOREIGN KEY (`cash_id`)
    REFERENCES `cash` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_accrual_pays_1`
    FOREIGN KEY (`place_id`)
    REFERENCES `places` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `payment_details`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `payment_details` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `payment_id` INT(10) UNSIGNED NOT NULL,
  `accrual_pay_id` INT(10) UNSIGNED NOT NULL,
  `sum` DECIMAL(10,2) NOT NULL DEFAULT '0',
  `income_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_pay_details_1_idx` (`accrual_pay_id` ASC),
  INDEX `fk_pay_details_income_idx` (`income_id` ASC),
  INDEX `fk_payment_details_parent_idx` (`payment_id` ASC),
  CONSTRAINT `fk_payment_details_parent`
    FOREIGN KEY (`payment_id`)
    REFERENCES `payments` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_pay_details_accrual`
    FOREIGN KEY (`accrual_pay_id`)
    REFERENCES `accrual_pays` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_pay_details_income`
    FOREIGN KEY (`income_id`)
    REFERENCES `income_items` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 0
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `read_news`
-- -----------------------------------------------------
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
AUTO_INCREMENT = 17
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `contract_pays`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `contract_pays` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `contract_id` INT UNSIGNED NOT NULL,
  `service_id` INT UNSIGNED NOT NULL,
  `place_id` INT UNSIGNED NULL DEFAULT NULL,
  `cash_id` INT UNSIGNED NOT NULL,
  `count` DECIMAL(10,2) UNSIGNED NOT NULL DEFAULT 1,
  `price` DECIMAL(10,2) UNSIGNED NOT NULL DEFAULT 0,
  `min_sum` DECIMAL(10,2) NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_contract_pays_1_idx` (`cash_id` ASC),
  INDEX `fk_contract_pays_contract_id_idx` (`contract_id` ASC),
  INDEX `fk_contract_pays_1_idx1` (`place_id` ASC),
  INDEX `fk_contract_pays_service_id_idx` (`service_id` ASC),
  CONSTRAINT `fk_contract_pays_service_id`
    FOREIGN KEY (`service_id`)
    REFERENCES `services` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT `fk_contract_pays_cash_id`
    FOREIGN KEY (`cash_id`)
    REFERENCES `cash` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_contract_pays_contract_id`
    FOREIGN KEY (`contract_id`)
    REFERENCES `contracts` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_contract_pays_1`
    FOREIGN KEY (`place_id`)
    REFERENCES `places` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `expense_items`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `expense_items` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `contractors`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `contractors` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `debit_slips`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `debit_slips` (
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
    REFERENCES `organizations` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_debit_cash`
    FOREIGN KEY (`cash_id`)
    REFERENCES `cash` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_debit_contractor`
    FOREIGN KEY (`contractor_id`)
    REFERENCES `contractors` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_debit_user`
    FOREIGN KEY (`user_id`)
    REFERENCES `users` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_debit_expense`
    FOREIGN KEY (`expense_id`)
    REFERENCES `expense_items` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_debit_employee`
    FOREIGN KEY (`employee_id`)
    REFERENCES `employees` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8
PACK_KEYS = DEFAULT;


-- -----------------------------------------------------
-- Table `advance`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `advance` (
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
    REFERENCES `organizations` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_cash`
    FOREIGN KEY (`cash_id`)
    REFERENCES `cash` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_employee`
    FOREIGN KEY (`employee_id`)
    REFERENCES `employees` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_contractor`
    FOREIGN KEY (`contractor_id`)
    REFERENCES `contractors` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_user`
    FOREIGN KEY (`user_id`)
    REFERENCES `users` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_expense`
    FOREIGN KEY (`expense_id`)
    REFERENCES `expense_items` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `base_parameters`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `base_parameters` (
  `name` VARCHAR(20) NOT NULL,
  `str_value` VARCHAR(100) NULL DEFAULT NULL,
  PRIMARY KEY (`name`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `meter_types`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `meter_types` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `reading_ratio` DOUBLE UNSIGNED NOT NULL DEFAULT 1,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `meters`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `meters` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `place_id` INT UNSIGNED NOT NULL,
  `meter_type_id` INT UNSIGNED NOT NULL,
  `parent_meter_id` INT UNSIGNED NULL DEFAULT NULL,
  `disabled` TINYINT(1) NOT NULL DEFAULT FALSE,
  PRIMARY KEY (`id`),
  INDEX `fk_meters_type_idx` (`meter_type_id` ASC),
  INDEX `fk_meters_parent_idx` (`parent_meter_id` ASC),
  INDEX `fk_meters_1_idx` (`place_id` ASC),
  CONSTRAINT `fk_meters_type`
    FOREIGN KEY (`meter_type_id`)
    REFERENCES `meter_types` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_meters_parent`
    FOREIGN KEY (`parent_meter_id`)
    REFERENCES `meters` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_meters_1`
    FOREIGN KEY (`place_id`)
    REFERENCES `places` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1;


-- -----------------------------------------------------
-- Table `meter_tariffs`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `meter_tariffs` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `meter_type_id` INT UNSIGNED NOT NULL,
  `service_id` INT UNSIGNED NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_meter_tariffs_meter_type_idx` (`meter_type_id` ASC),
  INDEX `fk_meter_tariffs_service_idx` (`service_id` ASC),
  CONSTRAINT `fk_meter_tariffs_meter_type`
    FOREIGN KEY (`meter_type_id`)
    REFERENCES `meter_types` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_meter_tariffs_service`
    FOREIGN KEY (`service_id`)
    REFERENCES `services` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1;


-- -----------------------------------------------------
-- Table `meter_reading`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `meter_reading` (
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
    REFERENCES `meters` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_meter_reading_tariff`
    FOREIGN KEY (`meter_tariff_id`)
    REFERENCES `meter_tariffs` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_meter_reading_accrual_pay`
    FOREIGN KEY (`accrual_pay_id`)
    REFERENCES `accrual_pays` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `document_last_numbers`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `document_last_numbers` (
  `id` INT UNSIGNED ZEROFILL NOT NULL AUTO_INCREMENT,
  `doc_type` ENUM('Invoice') NULL,
  `year` INT UNSIGNED NOT NULL,
  `number` INT UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `index2` (`doc_type` ASC, `year` ASC))
ENGINE = InnoDB;

-- -----------------------------------------------------
-- Placeholder table for view `active_contracts`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `active_contracts` (`id` INT, `number` INT, `lessee_id` INT, `org_id` INT, `sign_date` INT, `start_date` INT, `end_date` INT, `pay_day` INT, `cancel_date` INT, `comments` INT);

-- -----------------------------------------------------
-- Placeholder table for view `current_place_leases`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `current_place_leases` (`id` INT, `number` INT, `lessee_id` INT, `org_id` INT, `sign_date` INT, `start_date` INT, `end_date` INT, `pay_day` INT, `cancel_date` INT, `comments` INT, `place_id` INT, `finish_date` INT);

-- -----------------------------------------------------
-- View `active_contracts`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `active_contracts`;
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

-- -----------------------------------------------------
-- View `current_place_leases`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `current_place_leases`;
CREATE  OR REPLACE VIEW `current_place_leases` AS
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
    
-- -----------------------------------------------------
-- Data for table `organizations`
-- -----------------------------------------------------
START TRANSACTION;
INSERT INTO `organizations` (`id`, `name`, `print_name`, `inn`, `kpp`, `jur_address`, `phone`, `bank_bik`, `bank_account`, `bank_cor_account`, `bank_name`, `leader_sign`, `buhgalter_sign`) VALUES (DEFAULT, 'Моя организация', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

COMMIT;


-- -----------------------------------------------------
-- Data for table `cash`
-- -----------------------------------------------------
START TRANSACTION;
INSERT INTO `cash` (`id`, `name`, `color`) VALUES (DEFAULT, 'Наличная касса', NULL);

COMMIT;


-- -----------------------------------------------------
-- Data for table `income_items`
-- -----------------------------------------------------
START TRANSACTION;
INSERT INTO `income_items` (`id`, `name`) VALUES (DEFAULT, 'Доход от аренды');
INSERT INTO `income_items` (`id`, `name`) VALUES (DEFAULT, 'Комунальные платежы');

COMMIT;


-- -----------------------------------------------------
-- Data for table `units`
-- -----------------------------------------------------
START TRANSACTION;
INSERT INTO `units` (`id`, `name`, `digits`, `okei`) VALUES (DEFAULT, 'усл.', DEFAULT, NULL);
INSERT INTO `units` (`id`, `name`, `digits`, `okei`) VALUES (DEFAULT, 'шт.', DEFAULT, NULL);
INSERT INTO `units` (`id`, `name`, `digits`, `okei`) VALUES (DEFAULT, 'кв. м.', DEFAULT, NULL);
INSERT INTO `units` (`id`, `name`, `digits`, `okei`) VALUES (DEFAULT, 'кВт', DEFAULT, NULL);

COMMIT;


-- -----------------------------------------------------
-- Data for table `services`
-- -----------------------------------------------------
START TRANSACTION;
INSERT INTO `services` (`id`, `name`, `units_id`, `income_id`, `by_area`, `incomplete_month`, `service_provider_id`, `place_set`, `place_occupy`) VALUES (DEFAULT, 'Аренда', 3, 1, TRUE, FALSE, NULL, 'Required', 1);
INSERT INTO `services` (`id`, `name`, `units_id`, `income_id`, `by_area`, `incomplete_month`, `service_provider_id`, `place_set`, `place_occupy`) VALUES (DEFAULT, 'Электричество', 4, 2, FALSE, FALSE, NULL, 'Allowed', 0);

COMMIT;


-- -----------------------------------------------------
-- Data for table `base_parameters`
-- -----------------------------------------------------
START TRANSACTION;
INSERT INTO `base_parameters` (`name`, `str_value`) VALUES ('product_name', 'BazAr');
INSERT INTO `base_parameters` (`name`, `str_value`) VALUES ('version', '2.4');
INSERT INTO `base_parameters` (`name`, `str_value`) VALUES ('edition', 'gpl');

COMMIT;


-- -----------------------------------------------------
-- Data for table `meter_types`
-- -----------------------------------------------------
START TRANSACTION;
INSERT INTO `meter_types` (`id`, `name`, `reading_ratio`) VALUES (DEFAULT, 'Эл. однотарифный', DEFAULT);

COMMIT;


-- -----------------------------------------------------
-- Data for table `meter_tariffs`
-- -----------------------------------------------------
START TRANSACTION;
INSERT INTO `meter_tariffs` (`id`, `name`, `meter_type_id`, `service_id`) VALUES (DEFAULT, 'Основной', 1, 2);

COMMIT;

