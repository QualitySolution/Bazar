CREATE DATABASE  IF NOT EXISTS `bazar` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `bazar`;
-- MySQL dump 10.13  Distrib 5.5.28, for Linux (x86_64)
--
-- Host: 89.223.63.130    Database: bazar
-- ------------------------------------------------------
-- Server version	5.5.25-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `accrual`
--

DROP TABLE IF EXISTS `accrual`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `accrual` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user_id` int(10) unsigned DEFAULT NULL,
  `contract_no` varchar(15) NOT NULL,
  `month` tinyint(3) unsigned NOT NULL,
  `year` int(10) unsigned NOT NULL,
  `paid` tinyint(1) NOT NULL DEFAULT '0',
  `no_complete` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`),
  UNIQUE KEY `Contract_per_month` (`contract_no`,`year`,`month`),
  KEY `fk_accrual_1_idx` (`user_id`),
  KEY `fk_accrual_1_idx1` (`contract_no`),
  CONSTRAINT `fk_accrual_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_accrual_contract_no` FOREIGN KEY (`contract_no`) REFERENCES `contracts` (`number`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accrual`
--

LOCK TABLES `accrual` WRITE;
/*!40000 ALTER TABLE `accrual` DISABLE KEYS */;
/*!40000 ALTER TABLE `accrual` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `accrual_pays`
--

DROP TABLE IF EXISTS `accrual_pays`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `accrual_pays` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `accrual_id` int(10) unsigned NOT NULL,
  `service_id` int(10) unsigned NOT NULL,
  `cash_id` int(10) unsigned NOT NULL,
  `count` int(10) unsigned DEFAULT '0',
  `price` decimal(10,2) unsigned DEFAULT '0.00',
  PRIMARY KEY (`id`),
  KEY `fk_accrual_pays_accrual_id_idx` (`accrual_id`),
  KEY `fk_accrual_pays_service_id_idx` (`service_id`),
  KEY `fk_accrual_pays_cash_idx` (`cash_id`),
  CONSTRAINT `fk_accrual_pays_accrual_id` FOREIGN KEY (`accrual_id`) REFERENCES `accrual` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_accrual_pays_service_id` FOREIGN KEY (`service_id`) REFERENCES `services` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_accrual_pays_cash` FOREIGN KEY (`cash_id`) REFERENCES `cash` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accrual_pays`
--

LOCK TABLES `accrual_pays` WRITE;
/*!40000 ALTER TABLE `accrual_pays` DISABLE KEYS */;
/*!40000 ALTER TABLE `accrual_pays` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `advance`
--

DROP TABLE IF EXISTS `advance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `advance` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `org_id` int(10) unsigned NOT NULL,
  `cash_id` int(10) unsigned NOT NULL,
  `date` date NOT NULL,
  `employee_id` int(10) unsigned NOT NULL,
  `contractor_id` int(10) unsigned DEFAULT NULL,
  `user_id` int(10) unsigned DEFAULT NULL,
  `expense_id` int(10) unsigned NOT NULL,
  `sum` decimal(10,2) unsigned NOT NULL DEFAULT '0.00',
  `details` text,
  PRIMARY KEY (`id`),
  KEY `fk_advance_org_idx` (`org_id`),
  KEY `fk_advance_cash_idx` (`cash_id`),
  KEY `fk_advance_employee_idx` (`employee_id`),
  KEY `fk_advance_contractor_idx` (`contractor_id`),
  KEY `fk_advance_user_idx` (`user_id`),
  KEY `fk_advance_expense_idx` (`expense_id`),
  CONSTRAINT `fk_advance_org` FOREIGN KEY (`org_id`) REFERENCES `organizations` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_cash` FOREIGN KEY (`cash_id`) REFERENCES `cash` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_employee` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_contractor` FOREIGN KEY (`contractor_id`) REFERENCES `contractors` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_expense` FOREIGN KEY (`expense_id`) REFERENCES `expense_items` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `advance`
--

LOCK TABLES `advance` WRITE;
/*!40000 ALTER TABLE `advance` DISABLE KEYS */;
/*!40000 ALTER TABLE `advance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cash`
--

DROP TABLE IF EXISTS `cash`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cash` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cash`
--

LOCK TABLES `cash` WRITE;
/*!40000 ALTER TABLE `cash` DISABLE KEYS */;
INSERT INTO `cash` VALUES (1,'Наличная касса');
/*!40000 ALTER TABLE `cash` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `classes`
--

DROP TABLE IF EXISTS `classes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `classes` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `classes`
--

LOCK TABLES `classes` WRITE;
/*!40000 ALTER TABLE `classes` DISABLE KEYS */;
/*!40000 ALTER TABLE `classes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contact_persons`
--

DROP TABLE IF EXISTS `contact_persons`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `contact_persons` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `telephones` varchar(100) DEFAULT NULL,
  `comments` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contact_persons`
--

LOCK TABLES `contact_persons` WRITE;
/*!40000 ALTER TABLE `contact_persons` DISABLE KEYS */;
/*!40000 ALTER TABLE `contact_persons` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contract_pays`
--

DROP TABLE IF EXISTS `contract_pays`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `contract_pays` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `contract_no` varchar(15) NOT NULL,
  `service_id` int(10) unsigned NOT NULL,
  `cash_id` int(10) unsigned NOT NULL,
  `count` int(10) unsigned NOT NULL DEFAULT '1',
  `price` decimal(10,2) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `contract_no_idx` (`contract_no`),
  KEY `fk_contract_pays_service_id_idx` (`service_id`),
  KEY `fk_contract_pays_1_idx` (`cash_id`),
  CONSTRAINT `contract_no` FOREIGN KEY (`contract_no`) REFERENCES `contracts` (`number`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_contract_pays_service_id` FOREIGN KEY (`service_id`) REFERENCES `services` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_contract_pays_cash_id` FOREIGN KEY (`cash_id`) REFERENCES `cash` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contract_pays`
--

LOCK TABLES `contract_pays` WRITE;
/*!40000 ALTER TABLE `contract_pays` DISABLE KEYS */;
/*!40000 ALTER TABLE `contract_pays` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contractors`
--

DROP TABLE IF EXISTS `contractors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `contractors` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contractors`
--

LOCK TABLES `contractors` WRITE;
/*!40000 ALTER TABLE `contractors` DISABLE KEYS */;
/*!40000 ALTER TABLE `contractors` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contracts`
--

DROP TABLE IF EXISTS `contracts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `contracts` (
  `number` varchar(15) NOT NULL,
  `lessee_id` int(10) unsigned NOT NULL,
  `org_id` int(10) unsigned NOT NULL,
  `place_type_id` int(10) unsigned NOT NULL,
  `place_no` varchar(10) NOT NULL,
  `sign_date` date DEFAULT NULL,
  `start_date` date NOT NULL,
  `end_date` date NOT NULL,
  `pay_day` int(11) DEFAULT NULL,
  `cancel_date` date DEFAULT NULL,
  `comments` text,
  PRIMARY KEY (`number`),
  UNIQUE KEY `Number_UNIQUE` (`number`),
  KEY `fk_lessee_id_idx` (`lessee_id`),
  KEY `fk_org_id_idx` (`org_id`),
  KEY `fk_place_id_idx` (`place_type_id`,`place_no`),
  CONSTRAINT `fk_lessee_id` FOREIGN KEY (`lessee_id`) REFERENCES `lessees` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_org_id` FOREIGN KEY (`org_id`) REFERENCES `organizations` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_place_id` FOREIGN KEY (`place_type_id`, `place_no`) REFERENCES `places` (`type_id`, `place_no`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contracts`
--

LOCK TABLES `contracts` WRITE;
/*!40000 ALTER TABLE `contracts` DISABLE KEYS */;
/*!40000 ALTER TABLE `contracts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `credit_slips`
--

DROP TABLE IF EXISTS `credit_slips`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `credit_slips` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `org_id` int(10) unsigned NOT NULL,
  `cash_id` int(10) unsigned NOT NULL,
  `lessee_id` int(10) unsigned DEFAULT NULL,
  `user_id` int(10) unsigned DEFAULT NULL,
  `date` date NOT NULL,
  `sum` decimal(10,2) unsigned NOT NULL,
  `contract_no` varchar(15) DEFAULT NULL,
  `income_id` int(10) unsigned DEFAULT NULL,
  `operation` enum('common','advance') NOT NULL DEFAULT 'common',
  `accrual_id` int(10) unsigned DEFAULT NULL,
  `employee_id` int(10) unsigned DEFAULT NULL,
  `details` text,
  PRIMARY KEY (`id`),
  KEY `fk_credit_org_id_idx` (`org_id`),
  KEY `fk_credit_cash_id_idx` (`cash_id`),
  KEY `fk_credit_lessee_id_idx` (`lessee_id`),
  KEY `fk_credit_contract_no_idx` (`contract_no`),
  KEY `fk_credit_income_id_idx` (`income_id`),
  KEY `user_id_idx` (`user_id`),
  KEY `fk_credit_employee_id_idx` (`employee_id`),
  KEY `credit_operation_idx` (`operation`),
  KEY `fk_credit_slips_accural_idx` (`accrual_id`),
  CONSTRAINT `fk_credit_org_id` FOREIGN KEY (`org_id`) REFERENCES `organizations` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_cash_id` FOREIGN KEY (`cash_id`) REFERENCES `cash` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_lessee_id` FOREIGN KEY (`lessee_id`) REFERENCES `lessees` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_contract_no` FOREIGN KEY (`contract_no`) REFERENCES `contracts` (`number`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_income_id` FOREIGN KEY (`income_id`) REFERENCES `income_items` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_employee_id` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_slips_accural` FOREIGN KEY (`accrual_id`) REFERENCES `accrual` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `credit_slips`
--

LOCK TABLES `credit_slips` WRITE;
/*!40000 ALTER TABLE `credit_slips` DISABLE KEYS */;
/*!40000 ALTER TABLE `credit_slips` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `debit_slips`
--

DROP TABLE IF EXISTS `debit_slips`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `debit_slips` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `org_id` int(10) unsigned NOT NULL,
  `cash_id` int(10) unsigned NOT NULL,
  `contractor_id` int(10) unsigned DEFAULT NULL,
  `user_id` int(10) unsigned DEFAULT NULL,
  `date` date NOT NULL,
  `sum` decimal(10,2) unsigned NOT NULL DEFAULT '0.00',
  `expense_id` int(10) unsigned DEFAULT NULL,
  `operation` enum('common','advance') NOT NULL DEFAULT 'common',
  `employee_id` int(10) unsigned DEFAULT NULL,
  `details` text,
  PRIMARY KEY (`id`),
  KEY `fk_debit_org_idx` (`org_id`),
  KEY `fk_debit_cash_idx` (`cash_id`),
  KEY `fk_debit_contractor_idx` (`contractor_id`),
  KEY `fk_debit_user_idx` (`user_id`),
  KEY `fk_debit_expense_idx` (`expense_id`),
  KEY `fk_debit_employee_idx` (`employee_id`),
  KEY `debit_opration_idx` (`operation`),
  CONSTRAINT `fk_debit_org` FOREIGN KEY (`org_id`) REFERENCES `organizations` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_debit_cash` FOREIGN KEY (`cash_id`) REFERENCES `cash` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_debit_contractor` FOREIGN KEY (`contractor_id`) REFERENCES `contractors` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_debit_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_debit_expense` FOREIGN KEY (`expense_id`) REFERENCES `expense_items` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_debit_employee` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `debit_slips`
--

LOCK TABLES `debit_slips` WRITE;
/*!40000 ALTER TABLE `debit_slips` DISABLE KEYS */;
/*!40000 ALTER TABLE `debit_slips` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employees`
--

DROP TABLE IF EXISTS `employees`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `employees` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employees`
--

LOCK TABLES `employees` WRITE;
/*!40000 ALTER TABLE `employees` DISABLE KEYS */;
/*!40000 ALTER TABLE `employees` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `events`
--

DROP TABLE IF EXISTS `events`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `events` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `date` datetime NOT NULL,
  `class_id` int(10) unsigned DEFAULT NULL,
  `user_id` int(10) unsigned DEFAULT NULL,
  `user` varchar(20) DEFAULT NULL,
  `cause` text,
  `activity` text,
  `lessee_id` int(10) unsigned DEFAULT NULL,
  `place_type_id` int(10) unsigned DEFAULT NULL,
  `place_no` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `places_id` (`place_type_id`,`place_no`),
  KEY `classes_id` (`class_id`),
  KEY `lessees` (`lessee_id`),
  KEY `class_key` (`class_id`),
  KEY `lessees_key` (`lessee_id`),
  KEY `place_key` (`place_type_id`,`place_no`),
  FULLTEXT KEY `cause_idx` (`cause`),
  FULLTEXT KEY `activity_idx` (`activity`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `events`
--

LOCK TABLES `events` WRITE;
/*!40000 ALTER TABLE `events` DISABLE KEYS */;
/*!40000 ALTER TABLE `events` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `expense_items`
--

DROP TABLE IF EXISTS `expense_items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `expense_items` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `expense_items`
--

LOCK TABLES `expense_items` WRITE;
/*!40000 ALTER TABLE `expense_items` DISABLE KEYS */;
/*!40000 ALTER TABLE `expense_items` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `goods`
--

DROP TABLE IF EXISTS `goods`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `goods` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `goods`
--

LOCK TABLES `goods` WRITE;
/*!40000 ALTER TABLE `goods` DISABLE KEYS */;
/*!40000 ALTER TABLE `goods` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `income_items`
--

DROP TABLE IF EXISTS `income_items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `income_items` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `income_items`
--

LOCK TABLES `income_items` WRITE;
/*!40000 ALTER TABLE `income_items` DISABLE KEYS */;
/*!40000 ALTER TABLE `income_items` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `lessees`
--

DROP TABLE IF EXISTS `lessees`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `lessees` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `FIO_dir` varchar(45) DEFAULT NULL,
  `passport_ser` varchar(5) DEFAULT NULL,
  `passport_no` varchar(6) DEFAULT NULL,
  `passport_exit` varchar(100) DEFAULT NULL,
  `address` varchar(100) DEFAULT NULL,
  `INN` varchar(12) DEFAULT NULL,
  `OGRN` varchar(15) DEFAULT NULL,
  `wholesaler` tinyint(1) DEFAULT '0',
  `retail` tinyint(1) DEFAULT '0',
  `goods_id` int(10) unsigned DEFAULT NULL,
  `comments` text,
  PRIMARY KEY (`id`),
  KEY `goods_id_idx` (`goods_id`),
  CONSTRAINT `goods_id` FOREIGN KEY (`goods_id`) REFERENCES `goods` (`id`) ON DELETE SET NULL ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `lessees`
--

LOCK TABLES `lessees` WRITE;
/*!40000 ALTER TABLE `lessees` DISABLE KEYS */;
/*!40000 ALTER TABLE `lessees` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `organizations`
--

DROP TABLE IF EXISTS `organizations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `organizations` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `organizations`
--

LOCK TABLES `organizations` WRITE;
/*!40000 ALTER TABLE `organizations` DISABLE KEYS */;
INSERT INTO `organizations` VALUES (1,'Моя организация');
/*!40000 ALTER TABLE `organizations` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `place_types`
--

DROP TABLE IF EXISTS `place_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `place_types` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(10) DEFAULT NULL,
  `description` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `place_types`
--

LOCK TABLES `place_types` WRITE;
/*!40000 ALTER TABLE `place_types` DISABLE KEYS */;
/*!40000 ALTER TABLE `place_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `places`
--

DROP TABLE IF EXISTS `places`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `places` (
  `type_id` int(10) unsigned NOT NULL,
  `place_no` varchar(10) NOT NULL,
  `area` float(10,2) DEFAULT NULL,
  `contact_person_id` int(10) unsigned DEFAULT NULL,
  `org_id` int(10) unsigned DEFAULT NULL,
  `comments` text,
  PRIMARY KEY (`type_id`,`place_no`),
  KEY `types_id_idx` (`type_id`),
  KEY `contacts_idx` (`contact_person_id`),
  KEY `org_id_idx` (`org_id`),
  CONSTRAINT `contacts` FOREIGN KEY (`contact_person_id`) REFERENCES `contact_persons` (`id`) ON DELETE SET NULL ON UPDATE NO ACTION,
  CONSTRAINT `types_id` FOREIGN KEY (`type_id`) REFERENCES `place_types` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `org_id` FOREIGN KEY (`org_id`) REFERENCES `organizations` (`id`) ON DELETE SET NULL ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `places`
--

LOCK TABLES `places` WRITE;
/*!40000 ALTER TABLE `places` DISABLE KEYS */;
/*!40000 ALTER TABLE `places` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `services`
--

DROP TABLE IF EXISTS `services`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `services` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `units_id` int(10) unsigned NOT NULL,
  `income_id` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`),
  KEY `units_id_idx` (`units_id`),
  KEY `fk_services_income_id_idx` (`income_id`),
  CONSTRAINT `units_id` FOREIGN KEY (`units_id`) REFERENCES `units` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `fk_services_income_id` FOREIGN KEY (`income_id`) REFERENCES `income_items` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `services`
--

LOCK TABLES `services` WRITE;
/*!40000 ALTER TABLE `services` DISABLE KEYS */;
/*!40000 ALTER TABLE `services` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `units`
--

DROP TABLE IF EXISTS `units`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `units` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `units`
--

LOCK TABLES `units` WRITE;
/*!40000 ALTER TABLE `units` DISABLE KEYS */;
INSERT INTO `units` VALUES (1,'шт.'),(2,'усл.'),(3,'кв. м.');
/*!40000 ALTER TABLE `units` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `login` varchar(45) NOT NULL,
  `description` text,
  `admin` tinyint(1) NOT NULL DEFAULT '0',
  `edit_slips` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'demo','demo',NULL,1,0);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2013-01-23 10:59:00
