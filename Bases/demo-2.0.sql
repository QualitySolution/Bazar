CREATE DATABASE  IF NOT EXISTS `bazar` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `bazar`;
-- MySQL dump 10.13  Distrib 5.5.29, for Linux (x86_64)
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
  CONSTRAINT `fk_accrual_contract_no` FOREIGN KEY (`contract_no`) REFERENCES `contracts` (`number`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_accrual_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accrual`
--

LOCK TABLES `accrual` WRITE;
/*!40000 ALTER TABLE `accrual` DISABLE KEYS */;
INSERT INTO `accrual` VALUES (1,1,'11',1,2013,1,0),(2,1,'12',1,2013,1,0),(3,1,'33',1,2013,1,0),(4,1,'34',1,2013,1,0),(5,1,'45',1,2013,1,0),(6,1,'65',1,2013,0,0),(7,1,'11',2,2013,1,0),(8,1,'12',2,2013,1,0),(9,1,'33',2,2013,1,0),(10,1,'34',2,2013,0,0),(11,1,'45',2,2013,1,0),(12,1,'77/1',2,2013,1,0);
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
  CONSTRAINT `fk_accrual_pays_cash` FOREIGN KEY (`cash_id`) REFERENCES `cash` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_accrual_pays_service_id` FOREIGN KEY (`service_id`) REFERENCES `services` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=43 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accrual_pays`
--

LOCK TABLES `accrual_pays` WRITE;
/*!40000 ALTER TABLE `accrual_pays` DISABLE KEYS */;
INSERT INTO `accrual_pays` VALUES (2,2,1,1,50,580.00),(3,3,1,1,50,150.00),(4,4,1,1,170,250.00),(5,4,2,1,350,10.00),(6,4,5,1,1,1500.00),(7,5,1,1,42,1090.00),(8,5,2,1,200,10.00),(9,5,5,1,1,2000.00),(10,5,6,1,1,1000.00),(11,5,3,1,1,350.00),(12,5,4,1,1,1500.00),(14,6,1,1,24,1900.00),(15,6,4,1,1,450.00),(16,6,3,1,1,1000.00),(17,6,5,1,1,2000.00),(18,6,6,1,1,3505.00),(21,1,1,1,140,250.00),(23,7,1,1,140,250.00),(24,8,1,1,50,580.00),(25,9,1,1,50,150.00),(26,10,1,1,170,250.00),(27,10,2,1,350,10.00),(28,10,5,1,1,1500.00),(29,11,1,1,42,1090.00),(30,11,2,1,200,10.00),(31,11,5,1,1,2000.00),(32,11,6,1,1,1000.00),(33,11,3,1,1,350.00),(34,11,4,1,1,1500.00),(36,1,2,1,150,5.00),(37,12,1,1,24,450.00),(38,12,2,1,2000,15.00),(39,12,5,1,1,3000.00),(40,12,6,1,1,2000.00),(41,12,3,1,1,1500.00),(42,12,4,1,1,2500.00);
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
  CONSTRAINT `fk_advance_cash` FOREIGN KEY (`cash_id`) REFERENCES `cash` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_contractor` FOREIGN KEY (`contractor_id`) REFERENCES `contractors` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_employee` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_expense` FOREIGN KEY (`expense_id`) REFERENCES `expense_items` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_org` FOREIGN KEY (`org_id`) REFERENCES `organizations` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_advance_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `advance`
--

LOCK TABLES `advance` WRITE;
/*!40000 ALTER TABLE `advance` DISABLE KEYS */;
INSERT INTO `advance` VALUES (1,1,1,'2013-01-24',1,2,1,3,2500.00,NULL),(2,1,1,'2013-01-24',3,NULL,1,4,20000.00,NULL),(3,1,1,'2013-01-29',2,1,1,2,25000.00,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `classes`
--

LOCK TABLES `classes` WRITE;
/*!40000 ALTER TABLE `classes` DISABLE KEYS */;
INSERT INTO `classes` VALUES (1,'авария'),(2,'пожар');
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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contact_persons`
--

LOCK TABLES `contact_persons` WRITE;
/*!40000 ALTER TABLE `contact_persons` DISABLE KEYS */;
INSERT INTO `contact_persons` VALUES (1,'Манькин Денис Валерьевич','+7911-456-45-78',NULL),(2,'Карташов Максим Григорьевич','+7921-876-34-09',NULL),(3,'Михайлова Эльвира Андреевна','245-09-87',NULL),(4,'Потапова Анна Петровна','987-65-43',NULL),(5,'Завьялов Николай Алексеевич','234-76-09',NULL);
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
  CONSTRAINT `fk_contract_pays_cash_id` FOREIGN KEY (`cash_id`) REFERENCES `cash` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_contract_pays_service_id` FOREIGN KEY (`service_id`) REFERENCES `services` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contract_pays`
--

LOCK TABLES `contract_pays` WRITE;
/*!40000 ALTER TABLE `contract_pays` DISABLE KEYS */;
INSERT INTO `contract_pays` VALUES (9,'34',1,1,170,250.00),(10,'2',1,1,42,1250.00),(11,'2',5,1,1,3000.00),(12,'45',1,1,42,1090.00),(13,'45',2,1,200,10.00),(14,'45',5,1,1,2000.00),(15,'45',6,1,1,1000.00),(16,'45',3,1,1,350.00),(17,'45',4,1,1,1500.00),(18,'34',2,1,350,10.00),(19,'34',5,1,1,1500.00),(20,'65',1,1,24,1900.00),(21,'65',4,1,1,450.00),(22,'65',3,1,1,1000.00),(23,'65',5,1,1,2000.00),(24,'65',6,1,1,3505.00),(25,'12',1,1,50,580.00),(26,'11',1,1,140,250.00),(27,'33',1,1,50,150.00),(28,'77/1',1,1,24,450.00),(29,'77/1',2,1,2000,15.00),(30,'77/1',5,1,1,3000.00),(31,'77/1',6,1,1,2000.00),(32,'77/1',3,1,1,1500.00),(33,'77/1',4,1,1,2500.00),(34,'098',1,1,400,1000.00),(35,'90',1,1,50,750.00),(36,'678',1,1,80,250.00),(37,'678',2,1,600,3.00),(38,'3',1,1,38,700.00),(39,'6',1,1,18,350.00),(40,'6',6,1,1,3000.00),(41,'6',5,1,1,3000.00),(42,'6',3,1,1,1500.00),(43,'7',1,1,130,250.00),(44,'8',1,1,107,200.00),(45,'9',1,1,60,450.00);
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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contractors`
--

LOCK TABLES `contractors` WRITE;
/*!40000 ALTER TABLE `contractors` DISABLE KEYS */;
INSERT INTO `contractors` VALUES (1,'ЗАО \"Водоканал\"'),(2,'ОАО \"МТС\"'),(3,'ОАО \"ЛенЭнерго\"'),(4,'ОАО \"Ростелеком\"'),(5,'ООО \"1 Клининговая\"'),(6,'Магазин на углу');
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
INSERT INTO `contracts` VALUES ('098',6,1,1,'450','2013-01-29','2013-01-29','2014-01-29',2,NULL,NULL),('10',3,1,1,'3',NULL,'2011-12-01','2012-06-30',NULL,NULL,NULL),('11',5,1,4,'1','2012-12-01','2012-12-01','2015-01-01',10,NULL,NULL),('12',1,1,3,'1','2013-01-01','2013-01-01','2014-01-01',NULL,'2013-02-01',NULL),('2',5,1,1,'2','2012-09-01','2012-09-01','2012-12-31',2,'2012-11-29',NULL),('3',6,1,1,'10','2013-01-29','2013-01-29','2014-01-29',8,NULL,NULL),('33',5,1,13,'1','2012-12-01','2012-12-01','2014-01-01',10,NULL,NULL),('34',4,1,2,'7','2013-01-24','2013-01-24','2014-01-24',7,NULL,NULL),('4',2,1,1,'3','2013-01-29','2013-01-29','2015-01-29',NULL,NULL,NULL),('45',2,1,1,'2','2013-01-01','2013-01-01','2014-01-01',15,NULL,NULL),('5',6,1,1,'3','2012-07-29','2012-07-29','2012-12-31',8,NULL,'Переехали в офис 11'),('6',1,1,1,'5','2013-01-01','2013-01-01','2014-01-01',NULL,NULL,NULL),('65',3,1,1,'34','2011-12-01','2011-01-17','2013-01-24',7,NULL,NULL),('678',7,1,13,'34','2013-01-31','2013-01-31','2013-11-01',15,NULL,NULL),('7',7,1,1,'55','2013-01-01','2013-01-01','2015-01-01',3,NULL,NULL),('77/1',5,1,1,'34','2013-02-01','2013-02-01','2013-10-01',10,NULL,NULL),('8',3,1,2,'4','2013-01-01','2013-01-01','2013-12-31',4,NULL,NULL),('9',4,1,3,'2','2012-12-01','2012-12-01','2013-12-31',NULL,NULL,NULL),('90',5,1,3,'1','2013-02-02','2013-02-02','2013-05-03',11,NULL,NULL);
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
  CONSTRAINT `fk_credit_cash_id` FOREIGN KEY (`cash_id`) REFERENCES `cash` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_contract_no` FOREIGN KEY (`contract_no`) REFERENCES `contracts` (`number`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_employee_id` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_income_id` FOREIGN KEY (`income_id`) REFERENCES `income_items` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_lessee_id` FOREIGN KEY (`lessee_id`) REFERENCES `lessees` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_org_id` FOREIGN KEY (`org_id`) REFERENCES `organizations` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_slips_accural` FOREIGN KEY (`accrual_id`) REFERENCES `accrual` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `credit_slips`
--

LOCK TABLES `credit_slips` WRITE;
/*!40000 ALTER TABLE `credit_slips` DISABLE KEYS */;
INSERT INTO `credit_slips` VALUES (1,1,1,5,1,'2013-01-24',35000.00,'11',1,'common',1,NULL,'Оплата по начислению № 1 (Январь 2013) за услуги: Аренда'),(2,1,1,1,1,'2013-01-24',29000.00,'12',1,'common',2,NULL,'Оплата по начислению № 2 (Январь 2013) за услуги: Аренда'),(3,1,1,5,1,'2013-01-24',7500.00,'33',1,'common',3,NULL,'Оплата по начислению № 3 (Январь 2013) за услуги: Аренда'),(4,1,1,4,1,'2013-01-24',42500.00,'34',1,'common',4,NULL,'Оплата по начислению № 4 (Январь 2013) за услуги: Аренда'),(5,1,1,4,1,'2013-01-24',3500.00,'34',3,'common',4,NULL,'Оплата по начислению № 4 (Январь 2013) за услуги: Водоснабжение'),(6,1,1,4,1,'2013-01-24',1500.00,'34',6,'common',4,NULL,'Оплата по начислению № 4 (Январь 2013) за услуги: Интернет'),(7,1,1,2,1,'2013-01-24',45780.00,'45',1,'common',5,NULL,'Оплата по начислению № 5 (Январь 2013) за услуги: Аренда'),(8,1,1,2,1,'2013-01-24',2000.00,'45',3,'common',5,NULL,'Оплата по начислению № 5 (Январь 2013) за услуги: Водоснабжение'),(9,1,1,2,1,'2013-01-24',2000.00,'45',6,'common',5,NULL,'Оплата по начислению № 5 (Январь 2013) за услуги: Интернет'),(10,1,1,2,1,'2013-01-24',1000.00,'45',5,'common',5,NULL,'Оплата по начислению № 5 (Январь 2013) за услуги: Охрана'),(11,1,1,2,1,'2013-01-24',350.00,'45',4,'common',5,NULL,'Оплата по начислению № 5 (Январь 2013) за услуги: Телефон'),(12,1,1,2,1,'2013-01-24',1500.00,'45',7,'common',5,NULL,'Оплата по начислению № 5 (Январь 2013) за услуги: Уборка'),(13,1,1,3,1,'2013-01-24',45600.00,'65',1,'common',6,NULL,'Оплата по начислению № 6 (Январь 2013) за услуги: Аренда'),(14,1,1,3,1,'2013-01-24',3505.00,'65',5,'common',6,NULL,'Оплата по начислению № 6 (Январь 2013) за услуги: Охрана'),(15,1,1,5,1,'2013-01-24',35000.00,'11',1,'common',7,NULL,'Оплата по начислению № 7 (Февраль 2013) за услуги: Аренда'),(16,1,1,1,1,'2013-01-24',29000.00,'12',1,'common',8,NULL,'Оплата по начислению № 8 (Февраль 2013) за услуги: Аренда'),(17,1,1,NULL,1,'2013-01-24',500.00,NULL,NULL,'advance',NULL,1,'Возврат в кассу денежных средств по авансовому отчету №1'),(18,1,1,5,1,'2013-01-24',7500.00,'33',1,'common',9,NULL,'Оплата по начислению № 9 (Февраль 2013) за услуги: Аренда'),(19,1,1,4,1,'2013-01-24',3500.00,'34',3,'common',10,NULL,'Оплата по начислению № 10 (Февраль 2013) за услуги: Водоснабжение'),(20,1,1,4,1,'2013-01-24',1500.00,'34',6,'common',10,NULL,'Оплата по начислению № 10 (Февраль 2013) за услуги: Интернет'),(21,1,1,2,1,'2013-01-24',45780.00,'45',1,'common',11,NULL,'Оплата по начислению № 11 (Февраль 2013) за услуги: Аренда'),(22,1,1,2,1,'2013-01-24',2000.00,'45',3,'common',11,NULL,'Оплата по начислению № 11 (Февраль 2013) за услуги: Водоснабжение'),(23,1,1,2,1,'2013-01-24',2000.00,'45',6,'common',11,NULL,'Оплата по начислению № 11 (Февраль 2013) за услуги: Интернет'),(24,1,1,2,1,'2013-01-24',1500.00,'45',7,'common',11,NULL,'Оплата по начислению № 11 (Февраль 2013) за услуги: Уборка'),(25,1,1,NULL,1,'2013-01-24',1500.00,NULL,NULL,'advance',NULL,3,'Возврат в кассу денежных средств по авансовому отчету №2'),(26,1,1,5,1,'2013-01-24',750.00,'11',3,'common',1,NULL,'Оплата по начислению № 1 (Январь 2013) за услуги: Водоснабжение'),(27,1,1,5,1,'2013-01-29',10800.00,'77/1',1,'common',12,NULL,'Оплата по начислению № 12 (Февраль 2013) за услуги: Аренда'),(28,1,1,5,1,'2013-01-29',30000.00,'77/1',3,'common',12,NULL,'Оплата по начислению № 12 (Февраль 2013) за услуги: Водоснабжение'),(29,1,1,5,1,'2013-01-29',3000.00,'77/1',6,'common',12,NULL,'Оплата по начислению № 12 (Февраль 2013) за услуги: Интернет'),(30,1,1,5,1,'2013-01-29',2000.00,'77/1',5,'common',12,NULL,'Оплата по начислению № 12 (Февраль 2013) за услуги: Охрана'),(31,1,1,5,1,'2013-01-29',1500.00,'77/1',4,'common',12,NULL,'Оплата по начислению № 12 (Февраль 2013) за услуги: Телефон'),(32,1,1,5,1,'2013-01-29',2500.00,'77/1',7,'common',12,NULL,'Оплата по начислению № 12 (Февраль 2013) за услуги: Уборка'),(33,1,1,2,1,'2013-01-29',1000.00,'45',5,'common',11,NULL,'Оплата по начислению № 11 (Февраль 2013) за услуги: Охрана'),(34,1,1,2,1,'2013-01-29',350.00,'45',4,'common',11,NULL,'Оплата по начислению № 11 (Февраль 2013) за услуги: Телефон'),(35,1,1,NULL,1,'2013-01-29',10000.00,NULL,NULL,'advance',NULL,2,'Возврат в кассу денежных средств по авансовому отчету №3');
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
  CONSTRAINT `fk_debit_cash` FOREIGN KEY (`cash_id`) REFERENCES `cash` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_debit_contractor` FOREIGN KEY (`contractor_id`) REFERENCES `contractors` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_debit_employee` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `fk_debit_expense` FOREIGN KEY (`expense_id`) REFERENCES `expense_items` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_debit_org` FOREIGN KEY (`org_id`) REFERENCES `organizations` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_debit_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `debit_slips`
--

LOCK TABLES `debit_slips` WRITE;
/*!40000 ALTER TABLE `debit_slips` DISABLE KEYS */;
INSERT INTO `debit_slips` VALUES (1,1,1,NULL,1,'2013-01-24',3000.00,3,'advance',1,NULL),(2,1,1,1,1,'2013-01-24',200000.00,2,'common',NULL,NULL),(3,1,1,5,1,'2013-01-24',50000.00,4,'common',NULL,NULL),(4,1,1,4,1,'2013-01-24',30000.00,3,'common',NULL,NULL),(5,1,1,NULL,1,'2013-01-24',6500.00,NULL,'advance',3,NULL),(6,1,1,NULL,1,'2013-01-24',15000.00,1,'advance',3,NULL),(7,1,1,NULL,1,'2013-01-24',35000.00,2,'advance',2,NULL),(8,1,1,NULL,1,'2013-01-29',1000.00,NULL,'advance',3,'На сантехнику'),(9,1,1,NULL,1,'2013-01-29',5000.00,NULL,'advance',2,'На кофеварку'),(10,1,1,NULL,1,'2013-01-29',300.00,NULL,'advance',1,'На кофе для новой кофеварки.');
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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employees`
--

LOCK TABLES `employees` WRITE;
/*!40000 ALTER TABLE `employees` DISABLE KEYS */;
INSERT INTO `employees` VALUES (1,'Иванов Иван Иванович'),(2,'Петров Петр Петрович'),(3,'Сидоров Николай Андреевич');
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
) ENGINE=MyISAM AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `events`
--

LOCK TABLES `events` WRITE;
/*!40000 ALTER TABLE `events` DISABLE KEYS */;
INSERT INTO `events` VALUES (1,'2013-01-24 15:48:17',1,NULL,'demo','обрыв кабеля телефонии','вызвали ремонтную службу',3,1,'34'),(2,'2013-01-24 15:49:45',2,NULL,'demo','Возгорание мусорных баков','Потушили своими силами, пожарных не вызывали',5,4,'1'),(3,'2013-01-24 15:54:35',1,NULL,'demo','Отключили холодную воду','Позвонили в водоканал, обещали сделать к вечеру',1,3,'1'),(4,'2013-01-31 13:46:28',1,NULL,'demo','Падение снега с крыши на припаркованную машину арендаторов.','Отправили рабочих убрать снег.',2,1,'2');
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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `expense_items`
--

LOCK TABLES `expense_items` WRITE;
/*!40000 ALTER TABLE `expense_items` DISABLE KEYS */;
INSERT INTO `expense_items` VALUES (1,'Премии'),(2,'Вода'),(3,'Связь'),(4,'Уборка');
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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `goods`
--

LOCK TABLES `goods` WRITE;
/*!40000 ALTER TABLE `goods` DISABLE KEYS */;
INSERT INTO `goods` VALUES (1,'Одежда'),(2,'Бытовая химия'),(3,'Товары для охоты и рыбалки'),(4,'Автозапчасти'),(5,'Детское питание'),(6,'ПО');
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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `income_items`
--

LOCK TABLES `income_items` WRITE;
/*!40000 ALTER TABLE `income_items` DISABLE KEYS */;
INSERT INTO `income_items` VALUES (1,'аренда'),(2,'электричество'),(3,'вода'),(4,'телефон'),(5,'охрана'),(6,'интернет'),(7,'уборка');
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
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `lessees`
--

LOCK TABLES `lessees` WRITE;
/*!40000 ALTER TABLE `lessees` DISABLE KEYS */;
INSERT INTO `lessees` VALUES (1,'ООО \"Манок\"','Шувалов Сергей Анатольевич','41 54','767897','28 ОМ Центрального р-на г.Санкт-Петербург','Санкт-Петербург, ул. Правды, д.12, корп.1, оф. 123','78456900','7823457610',0,1,3,NULL),(2,'ООО \"Чадо\"','Красуцкий Антон максимович','47 87','98753','ОМ г. Никольское Тосненского р-на Лен. обл.','Санкт-Петербург, Невский пр., д.85, оф. 65','7890456352','3244675889',1,0,5,NULL),(3,'ОАО \"SMTP\"','Тутаев Владимир Егорьевич','75 76','937262','54 ОМ г Москва','Москва, Пресненский Вал, д.1','987562525','42362721891',1,1,6,NULL),(4,'ООО \"Краса\"','Шутихин Владимир Юрьевич','65 78','235467','45 ОМ Красногвардейского р-на Санкт-Петербурга','Санкт-Петербург, ул.П. Качалова, д11','78645224','23546487548',1,1,4,NULL),(5,'ЧП Леонов М.В.','Леонов Михаил Валентинович','78 98','416627','32 ОМ Васил. р-на г.Санкт-Петербург','Санк-Петербург, ул.Расстанная, д.7','78635124','0292173256',0,1,1,NULL),(6,'ООО \"Инесса\"','Полежаев В.В.',NULL,NULL,NULL,'Санкт-Петербург, ул. Невский пр. д.9','78456352','2544345856996',0,1,NULL,NULL),(7,'Рога и Копыта','Мишкин Е.Ю.','47 98','743625','14 ОМ УФМС г. Зеленогорск','Санкт-Петербург, ул. Заставская, д.67','7890967050','6348239202599',1,1,4,NULL),(8,'ООО \"Нити\"','Веселов Игнат Валерьевич',NULL,NULL,NULL,'Лен. обл. г. Сестрорецк, ул. Ленина, д. 6','4786662','34261718289',1,1,6,NULL),(9,'ОАО \"БИБИ\"','Климов Анатолий Григорьевич',NULL,NULL,NULL,'Санкт-Петербург, ул. Разъежая, д1, оф.56','78946352','342617839340',1,0,4,NULL);
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
INSERT INTO `organizations` VALUES (1,'ООО \"Водолей\"');
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
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `place_types`
--

LOCK TABLES `place_types` WRITE;
/*!40000 ALTER TABLE `place_types` DISABLE KEYS */;
INSERT INTO `place_types` VALUES (1,'Оф','Офис'),(2,'Ск','Склад'),(3,'ТП','технич. пом.'),(4,'П','подвал'),(13,'Ч','чердак');
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
  CONSTRAINT `org_id` FOREIGN KEY (`org_id`) REFERENCES `organizations` (`id`) ON DELETE SET NULL ON UPDATE NO ACTION,
  CONSTRAINT `types_id` FOREIGN KEY (`type_id`) REFERENCES `place_types` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `places`
--

LOCK TABLES `places` WRITE;
/*!40000 ALTER TABLE `places` DISABLE KEYS */;
INSERT INTO `places` VALUES (1,'10',38.00,4,1,NULL),(1,'2',42.00,2,1,NULL),(1,'3',19.00,5,1,NULL),(1,'34',24.00,3,1,NULL),(1,'450',400.00,3,1,NULL),(1,'5',18.00,1,1,NULL),(1,'55',130.00,4,1,NULL),(2,'4',107.00,2,1,NULL),(2,'6',70.00,5,1,NULL),(2,'7',170.00,3,1,NULL),(3,'1',50.00,1,1,NULL),(3,'2',60.00,5,1,NULL),(4,'1',140.00,2,1,NULL),(4,'3',90.00,4,1,NULL),(13,'1',50.00,3,1,NULL),(13,'34',80.00,3,1,NULL);
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
  CONSTRAINT `fk_services_income_id` FOREIGN KEY (`income_id`) REFERENCES `income_items` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `units_id` FOREIGN KEY (`units_id`) REFERENCES `units` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `services`
--

LOCK TABLES `services` WRITE;
/*!40000 ALTER TABLE `services` DISABLE KEYS */;
INSERT INTO `services` VALUES (1,'Аренда',3,NULL),(2,'Водоснабжение',4,NULL),(3,'Телефон',2,NULL),(4,'Уборка',2,NULL),(5,'Интернет',2,NULL),(6,'Охрана',2,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `units`
--

LOCK TABLES `units` WRITE;
/*!40000 ALTER TABLE `units` DISABLE KEYS */;
INSERT INTO `units` VALUES (1,'шт.'),(2,'усл.'),(3,'кв. м.'),(4,'литр');
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

-- Dump completed on 2013-02-03 16:21:11
