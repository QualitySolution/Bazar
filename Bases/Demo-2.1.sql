CREATE DATABASE  IF NOT EXISTS `bazar` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `bazar`;
-- MySQL dump 10.13  Distrib 5.5.16, for Win32 (x86)
--
-- Host: demo.qsolution.ru    Database: bazar
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
  `contract_id` int(10) unsigned NOT NULL,
  `month` tinyint(3) unsigned NOT NULL,
  `year` int(10) unsigned NOT NULL,
  `paid` tinyint(1) NOT NULL DEFAULT '0',
  `no_complete` tinyint(1) NOT NULL DEFAULT '1',
  `comments` text,
  PRIMARY KEY (`id`),
  UNIQUE KEY `Contract_per_month` (`contract_id`,`year`,`month`) USING BTREE,
  KEY `fk_accrual_1_idx` (`user_id`),
  KEY `fk_accrual_contract_id_idx` (`contract_id`),
  CONSTRAINT `fk_accrual_contract_id` FOREIGN KEY (`contract_id`) REFERENCES `contracts` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_accrual_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=56 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accrual`
--

LOCK TABLES `accrual` WRITE;
/*!40000 ALTER TABLE `accrual` DISABLE KEYS */;
INSERT INTO `accrual` VALUES (1,1,3,1,2013,1,0,NULL),(2,1,4,1,2013,1,0,NULL),(3,1,7,1,2013,1,0,NULL),(4,1,8,1,2013,1,0,NULL),(5,1,10,1,2013,1,0,NULL),(6,1,13,1,2013,0,0,NULL),(7,1,3,2,2013,1,0,NULL),(8,1,4,2,2013,1,0,NULL),(9,1,7,2,2013,1,0,NULL),(10,1,8,2,2013,0,0,NULL),(11,1,10,2,2013,1,0,NULL),(12,1,16,2,2013,1,0,NULL),(26,1,1,10,2013,1,0,NULL),(27,1,3,10,2013,1,0,NULL),(28,1,6,10,2013,1,0,NULL),(29,1,7,10,2013,1,0,NULL),(30,1,8,10,2013,1,0,NULL),(31,1,9,10,2013,1,0,NULL),(32,1,10,10,2013,1,0,NULL),(33,1,12,10,2013,1,0,NULL),(34,1,14,10,2013,1,0,NULL),(35,1,15,10,2013,1,0,NULL),(36,1,16,10,2013,1,0,NULL),(37,1,17,10,2013,1,0,NULL),(38,1,18,10,2013,1,0,NULL),(39,1,3,11,2013,0,0,NULL),(40,1,1,11,2013,1,0,NULL),(41,1,6,11,2013,1,0,NULL),(42,1,7,11,2013,1,0,NULL),(43,1,8,11,2013,0,0,NULL),(44,1,9,11,2013,1,0,NULL),(45,1,10,11,2013,1,0,NULL),(46,1,12,11,2013,0,0,NULL),(47,1,14,11,2013,1,0,NULL),(48,1,15,11,2013,0,0,NULL),(49,1,17,11,2013,1,0,NULL),(50,1,18,11,2013,1,0,NULL),(51,1,20,10,2013,1,0,NULL),(52,1,21,10,2013,1,0,NULL),(53,1,20,11,2013,0,1,NULL),(54,1,21,11,2013,0,0,NULL),(55,1,9,12,2013,1,0,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=191 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accrual_pays`
--

LOCK TABLES `accrual_pays` WRITE;
/*!40000 ALTER TABLE `accrual_pays` DISABLE KEYS */;
INSERT INTO `accrual_pays` VALUES (2,2,1,1,50,580.00),(3,3,1,1,50,150.00),(4,4,1,1,170,250.00),(5,4,2,1,350,10.00),(6,4,5,1,1,1500.00),(7,5,1,1,42,1090.00),(8,5,2,1,200,10.00),(9,5,5,1,1,2000.00),(10,5,6,1,1,1000.00),(11,5,3,1,1,350.00),(12,5,4,1,1,1500.00),(14,6,1,1,24,1900.00),(15,6,4,1,1,450.00),(16,6,3,1,1,1000.00),(17,6,5,1,1,2000.00),(18,6,6,1,1,3505.00),(21,1,1,1,140,250.00),(23,7,1,1,140,250.00),(24,8,1,1,50,580.00),(25,9,1,1,50,150.00),(26,10,1,1,170,250.00),(27,10,2,1,350,10.00),(28,10,5,1,1,1500.00),(29,11,1,1,42,1090.00),(30,11,2,1,200,10.00),(31,11,5,1,1,2000.00),(32,11,6,1,1,1000.00),(33,11,3,1,1,350.00),(34,11,4,1,1,1500.00),(36,1,2,1,150,5.00),(37,12,1,1,24,450.00),(38,12,2,1,2000,15.00),(39,12,5,1,1,3000.00),(40,12,6,1,1,2000.00),(41,12,3,1,1,1500.00),(42,12,4,1,1,2500.00),(77,26,1,1,400,1000.00),(78,26,7,1,280,1.00),(79,26,2,1,10,1.50),(80,27,1,1,140,250.00),(81,27,7,1,2163,2.00),(82,27,2,1,1333,3.00),(83,28,1,1,38,700.00),(84,28,7,1,798,2.00),(85,28,2,1,83,5.00),(86,29,1,1,50,150.00),(87,29,7,1,840,3.00),(88,29,7,1,956,3.00),(89,30,1,1,170,250.00),(90,30,2,1,350,10.00),(91,30,5,1,1,1500.00),(92,31,2,1,3720,3.00),(93,31,1,1,19,140.00),(94,31,7,1,2592,1.00),(95,32,1,1,42,1090.00),(96,32,2,1,200,10.00),(97,32,5,1,1,2000.00),(98,32,6,1,1,1000.00),(99,32,3,1,1,350.00),(100,32,4,1,1,1500.00),(101,32,7,1,1222,3.00),(102,33,1,1,18,350.00),(103,33,6,1,1,3000.00),(104,33,5,1,1,3000.00),(105,33,3,1,1,1500.00),(106,33,7,1,696,4.00),(107,33,2,1,678,6.00),(109,34,1,1,80,250.00),(110,34,2,1,500,3.00),(112,35,1,1,130,250.00),(113,35,7,1,188,5.00),(114,35,2,1,146,1.00),(115,36,1,1,24,450.00),(116,36,2,1,2000,15.00),(117,36,5,1,1,3000.00),(118,36,6,1,1,2000.00),(119,36,3,1,1,1500.00),(120,36,4,1,1,2500.00),(122,37,1,1,107,200.00),(123,37,7,1,1007,3.00),(124,37,2,1,130,4.00),(125,38,1,1,60,450.00),(126,38,2,1,500,3.00),(127,38,7,1,195,5.00),(128,39,1,1,140,250.00),(129,39,7,1,647,2.00),(130,39,2,1,79955,3.00),(131,40,1,1,400,1000.00),(132,40,7,1,660,1.00),(133,40,2,1,68,1.50),(134,40,2,1,345,0.69),(138,41,1,1,38,700.00),(139,41,7,1,188,2.00),(140,41,2,1,450,5.00),(141,42,1,1,50,150.00),(142,42,7,1,667,3.00),(143,42,7,1,4220,3.00),(144,43,1,1,170,250.00),(145,43,2,1,12,10.00),(146,43,5,1,1,1500.00),(147,44,2,1,456,3.00),(148,44,1,1,19,140.00),(149,44,7,1,6278,1.00),(150,45,1,1,42,1090.00),(151,45,2,1,278,10.00),(152,45,5,1,1,2000.00),(153,45,6,1,1,1000.00),(154,45,3,1,1,350.00),(155,45,4,1,1,1500.00),(156,45,7,1,196,3.00),(157,46,1,1,18,350.00),(158,46,6,1,1,3000.00),(159,46,5,1,1,3000.00),(160,46,3,1,1,1500.00),(161,46,7,1,214,4.00),(162,46,2,1,67,6.00),(164,47,1,1,80,250.00),(165,47,2,1,600,3.00),(167,48,1,1,130,250.00),(168,48,7,1,174,5.00),(169,48,2,1,14,1.00),(170,49,1,1,107,200.00),(171,49,7,1,321,3.00),(172,49,2,1,567,4.00),(173,50,1,1,60,450.00),(174,50,2,1,890,3.00),(175,50,7,1,78,5.00),(176,51,1,1,24,567.00),(177,51,2,1,22,4.00),(178,51,7,1,278,1.70),(179,52,1,1,90,789.00),(180,52,2,1,678,5.00),(181,52,7,1,789,3.00),(182,53,1,1,24,567.00),(183,53,2,1,0,4.00),(184,53,7,1,0,1.70),(185,54,1,1,90,789.00),(186,54,2,1,456,5.00),(187,54,7,1,179,3.00),(188,55,2,1,70,3.00),(189,55,1,1,19,140.00),(190,55,7,1,5788,1.00);
/*!40000 ALTER TABLE `accrual_pays` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary table structure for view `active_contracts`
--

DROP TABLE IF EXISTS `active_contracts`;
/*!50001 DROP VIEW IF EXISTS `active_contracts`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE TABLE `active_contracts` (
  `id` int(10) unsigned,
  `number` varchar(15),
  `lessee_id` int(10) unsigned,
  `org_id` int(10) unsigned,
  `place_type_id` int(10) unsigned,
  `place_no` varchar(10),
  `sign_date` date,
  `start_date` date,
  `end_date` date,
  `pay_day` int(11),
  `cancel_date` date,
  `comments` text
) ENGINE=MyISAM */;
SET character_set_client = @saved_cs_client;

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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `advance`
--

LOCK TABLES `advance` WRITE;
/*!40000 ALTER TABLE `advance` DISABLE KEYS */;
INSERT INTO `advance` VALUES (1,1,1,'2013-01-24',1,2,1,3,2500.00,NULL),(2,1,1,'2013-01-24',3,NULL,1,4,20000.00,NULL),(3,1,1,'2013-01-29',2,1,1,2,25000.00,NULL),(4,1,1,'2013-10-08',1,3,1,1,300.00,NULL),(5,1,1,'2013-10-08',3,1,1,2,89000.00,NULL),(6,1,1,'2013-10-08',2,5,1,4,15000.00,NULL);
/*!40000 ALTER TABLE `advance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `base_parameters`
--

DROP TABLE IF EXISTS `base_parameters`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `base_parameters` (
  `name` varchar(20) NOT NULL,
  `str_value` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `base_parameters`
--

LOCK TABLES `base_parameters` WRITE;
/*!40000 ALTER TABLE `base_parameters` DISABLE KEYS */;
INSERT INTO `base_parameters` VALUES ('edition','gpl'),('product_name','BazAr'),('version','2.1');
/*!40000 ALTER TABLE `base_parameters` ENABLE KEYS */;
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
  `contract_id` int(10) unsigned NOT NULL,
  `service_id` int(10) unsigned NOT NULL,
  `cash_id` int(10) unsigned NOT NULL,
  `count` int(10) unsigned NOT NULL DEFAULT '1',
  `price` decimal(10,2) unsigned NOT NULL,
  `min_sum` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_contract_pays_service_id_idx` (`service_id`),
  KEY `fk_contract_pays_1_idx` (`cash_id`),
  KEY `fk_contract_pays_contract_id_idx` (`contract_id`),
  CONSTRAINT `fk_contract_pays_cash_id` FOREIGN KEY (`cash_id`) REFERENCES `cash` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_contract_pays_contract_id` FOREIGN KEY (`contract_id`) REFERENCES `contracts` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_contract_pays_service_id` FOREIGN KEY (`service_id`) REFERENCES `services` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=74 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contract_pays`
--

LOCK TABLES `contract_pays` WRITE;
/*!40000 ALTER TABLE `contract_pays` DISABLE KEYS */;
INSERT INTO `contract_pays` VALUES (9,8,1,1,170,250.00,NULL),(10,5,1,1,42,1250.00,NULL),(11,5,5,1,1,3000.00,NULL),(12,10,1,1,42,1090.00,NULL),(13,10,2,1,200,10.00,NULL),(14,10,5,1,1,2000.00,NULL),(15,10,6,1,1,1000.00,NULL),(16,10,3,1,1,350.00,NULL),(17,10,4,1,1,1500.00,NULL),(18,8,2,1,350,10.00,NULL),(19,8,5,1,1,1500.00,NULL),(20,13,1,1,24,1900.00,NULL),(21,13,4,1,1,450.00,NULL),(22,13,3,1,1,1000.00,NULL),(23,13,5,1,1,2000.00,NULL),(24,13,6,1,1,3505.00,NULL),(25,4,1,1,50,580.00,NULL),(26,3,1,1,140,250.00,NULL),(27,7,1,1,50,150.00,NULL),(28,16,1,1,24,450.00,NULL),(29,16,2,1,2000,15.00,NULL),(30,16,5,1,1,3000.00,NULL),(31,16,6,1,1,2000.00,NULL),(32,16,3,1,1,1500.00,NULL),(33,16,4,1,1,2500.00,NULL),(34,1,1,1,400,1000.00,NULL),(35,19,1,1,50,750.00,NULL),(36,14,1,1,80,250.00,NULL),(37,14,2,1,600,3.00,NULL),(38,6,1,1,38,700.00,NULL),(39,12,1,1,18,350.00,NULL),(40,12,6,1,1,3000.00,NULL),(41,12,5,1,1,3000.00,NULL),(42,12,3,1,1,1500.00,NULL),(43,15,1,1,130,250.00,NULL),(44,17,1,1,107,200.00,NULL),(45,18,1,1,60,450.00,NULL),(46,1,7,1,0,1.00,NULL),(47,1,2,1,0,1.50,NULL),(48,3,7,1,0,2.00,NULL),(49,3,2,1,0,3.00,4000.00),(50,6,7,1,0,2.00,NULL),(51,6,2,1,0,5.00,NULL),(52,7,7,1,0,3.00,2000.00),(53,7,7,1,0,3.00,NULL),(54,9,2,1,0,3.00,NULL),(55,9,1,1,19,140.00,NULL),(56,9,7,1,0,1.00,NULL),(57,10,7,1,0,3.00,NULL),(58,18,2,1,0,3.00,1500.00),(59,12,7,1,0,4.00,NULL),(60,12,2,1,0,6.00,7000.00),(61,15,7,1,0,5.00,NULL),(62,15,2,1,0,1.00,NULL),(63,17,7,1,0,3.00,NULL),(64,17,2,1,0,4.00,NULL),(65,19,7,1,0,4.00,NULL),(66,18,7,1,0,5.00,NULL),(67,1,2,1,0,0.69,NULL),(68,20,1,1,24,567.00,NULL),(69,20,2,1,0,4.00,NULL),(70,20,7,1,0,1.70,NULL),(71,21,1,1,90,789.00,NULL),(72,21,2,1,0,5.00,NULL),(73,21,7,1,0,3.00,NULL);
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
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
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
  PRIMARY KEY (`id`),
  UNIQUE KEY `unique_number_date` (`number`,`sign_date`),
  KEY `fk_lessee_id_idx` (`lessee_id`),
  KEY `fk_org_id_idx` (`org_id`),
  KEY `fk_place_id_idx` (`place_type_id`,`place_no`),
  CONSTRAINT `fk_lessee_id` FOREIGN KEY (`lessee_id`) REFERENCES `lessees` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_org_id` FOREIGN KEY (`org_id`) REFERENCES `organizations` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_place_id` FOREIGN KEY (`place_type_id`, `place_no`) REFERENCES `places` (`type_id`, `place_no`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contracts`
--

LOCK TABLES `contracts` WRITE;
/*!40000 ALTER TABLE `contracts` DISABLE KEYS */;
INSERT INTO `contracts` VALUES (1,'098',6,1,1,'450','2013-01-29','2013-01-29','2014-01-29',2,NULL,NULL),(2,'10',3,1,1,'3',NULL,'2011-12-01','2012-06-30',NULL,NULL,NULL),(3,'11',5,1,4,'1','2012-12-01','2012-12-01','2015-01-01',10,NULL,NULL),(4,'12',1,1,3,'1','2013-01-01','2013-01-01','2014-01-01',NULL,'2013-02-01',NULL),(5,'2',5,1,1,'2','2012-09-01','2012-09-01','2012-12-31',2,'2012-11-29',NULL),(6,'3',6,1,1,'10','2013-01-29','2013-01-29','2014-01-29',8,NULL,NULL),(7,'33',5,1,13,'1','2012-12-01','2012-12-01','2014-01-01',10,NULL,NULL),(8,'34',4,1,2,'7','2013-01-24','2013-01-24','2014-01-24',7,NULL,NULL),(9,'4',2,1,1,'3','2013-01-29','2013-01-29','2015-01-29',NULL,NULL,NULL),(10,'45',2,1,1,'2','2013-01-01','2013-01-01','2014-01-01',15,NULL,NULL),(11,'5',6,1,1,'3','2012-07-29','2012-07-29','2012-12-31',8,NULL,'Переехали в офис 11'),(12,'6',1,1,1,'5','2013-01-01','2013-01-01','2014-01-01',NULL,NULL,NULL),(13,'65',3,1,1,'34','2011-12-01','2011-01-17','2013-01-24',7,NULL,NULL),(14,'678',7,1,13,'34','2013-01-31','2013-01-31','2013-11-01',15,NULL,NULL),(15,'7',7,1,1,'55','2013-01-01','2013-01-01','2015-01-01',3,NULL,NULL),(16,'77/1',5,1,1,'34','2013-02-01','2013-02-01','2013-10-01',10,NULL,NULL),(17,'8',3,1,2,'4','2013-01-01','2013-01-01','2013-12-31',4,NULL,NULL),(18,'9',4,1,3,'2','2012-12-01','2012-12-01','2013-12-31',NULL,NULL,NULL),(19,'90',5,1,3,'1','2013-02-02','2013-02-02','2013-05-03',11,NULL,NULL),(20,'98',9,1,1,'34','2013-10-01','2013-10-02','2014-10-01',1,NULL,NULL),(21,'789/34',8,1,4,'3','2013-10-01','2013-10-02','2015-10-02',2,NULL,NULL);
/*!40000 ALTER TABLE `contracts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `credit_slips`
--

DROP TABLE IF EXISTS `credit_slips`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `credit_slips` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `org_id` int(10) unsigned NOT NULL,
  `cash_id` int(10) unsigned NOT NULL,
  `lessee_id` int(10) unsigned DEFAULT NULL,
  `user_id` int(10) unsigned DEFAULT NULL,
  `date` date NOT NULL,
  `sum` decimal(10,2) unsigned NOT NULL,
  `contract_id` int(10) unsigned DEFAULT NULL,
  `income_id` int(10) unsigned DEFAULT NULL,
  `operation` enum('common','advance','payment') NOT NULL DEFAULT 'common',
  `accrual_id` int(10) unsigned DEFAULT NULL,
  `employee_id` int(10) unsigned DEFAULT NULL,
  `details` text,
  PRIMARY KEY (`id`),
  KEY `fk_credit_org_id_idx` (`org_id`),
  KEY `fk_credit_cash_id_idx` (`cash_id`),
  KEY `fk_credit_lessee_id_idx` (`lessee_id`),
  KEY `fk_credit_income_id_idx` (`income_id`),
  KEY `user_id_idx` (`user_id`),
  KEY `fk_credit_employee_id_idx` (`employee_id`),
  KEY `credit_operation_idx` (`operation`),
  KEY `fk_credit_slips_accural_idx` (`accrual_id`),
  KEY `fk_credit_slips_contract_id_idx` (`contract_id`),
  CONSTRAINT `fk_credit_income_id` FOREIGN KEY (`income_id`) REFERENCES `income_items` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_cash_id` FOREIGN KEY (`cash_id`) REFERENCES `cash` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_employee_id` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_lessee_id` FOREIGN KEY (`lessee_id`) REFERENCES `lessees` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_org_id` FOREIGN KEY (`org_id`) REFERENCES `organizations` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_slips_accural` FOREIGN KEY (`accrual_id`) REFERENCES `accrual` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_credit_slips_contract_id` FOREIGN KEY (`contract_id`) REFERENCES `contracts` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=68 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `credit_slips`
--

LOCK TABLES `credit_slips` WRITE;
/*!40000 ALTER TABLE `credit_slips` DISABLE KEYS */;
INSERT INTO `credit_slips` VALUES (1,1,1,5,1,'2013-01-24',35000.00,3,1,'common',1,NULL,'Оплата по начислению № 1 (Январь 2013) за услуги: Аренда'),(2,1,1,1,1,'2013-01-24',29000.00,4,1,'common',2,NULL,'Оплата по начислению № 2 (Январь 2013) за услуги: Аренда'),(3,1,1,5,1,'2013-01-24',7500.00,7,1,'common',3,NULL,'Оплата по начислению № 3 (Январь 2013) за услуги: Аренда'),(4,1,1,4,1,'2013-01-24',42500.00,8,1,'common',4,NULL,'Оплата по начислению № 4 (Январь 2013) за услуги: Аренда'),(5,1,1,4,1,'2013-01-24',3500.00,8,3,'common',4,NULL,'Оплата по начислению № 4 (Январь 2013) за услуги: Водоснабжение'),(6,1,1,4,1,'2013-01-24',1500.00,8,6,'common',4,NULL,'Оплата по начислению № 4 (Январь 2013) за услуги: Интернет'),(7,1,1,2,1,'2013-01-24',45780.00,10,1,'common',5,NULL,'Оплата по начислению № 5 (Январь 2013) за услуги: Аренда'),(8,1,1,2,1,'2013-01-24',2000.00,10,3,'common',5,NULL,'Оплата по начислению № 5 (Январь 2013) за услуги: Водоснабжение'),(9,1,1,2,1,'2013-01-24',2000.00,10,6,'common',5,NULL,'Оплата по начислению № 5 (Январь 2013) за услуги: Интернет'),(10,1,1,2,1,'2013-01-24',1000.00,10,5,'common',5,NULL,'Оплата по начислению № 5 (Январь 2013) за услуги: Охрана'),(11,1,1,2,1,'2013-01-24',350.00,10,4,'common',5,NULL,'Оплата по начислению № 5 (Январь 2013) за услуги: Телефон'),(12,1,1,2,1,'2013-01-24',1500.00,10,7,'common',5,NULL,'Оплата по начислению № 5 (Январь 2013) за услуги: Уборка'),(13,1,1,3,1,'2013-01-24',45600.00,13,1,'common',6,NULL,'Оплата по начислению № 6 (Январь 2013) за услуги: Аренда'),(14,1,1,3,1,'2013-01-24',3505.00,13,5,'common',6,NULL,'Оплата по начислению № 6 (Январь 2013) за услуги: Охрана'),(15,1,1,5,1,'2013-01-24',35000.00,3,1,'common',7,NULL,'Оплата по начислению № 7 (Февраль 2013) за услуги: Аренда'),(16,1,1,1,1,'2013-01-24',29000.00,4,1,'common',8,NULL,'Оплата по начислению № 8 (Февраль 2013) за услуги: Аренда'),(17,1,1,NULL,1,'2013-01-24',500.00,NULL,NULL,'advance',NULL,1,'Возврат в кассу денежных средств по авансовому отчету №1'),(18,1,1,5,1,'2013-01-24',7500.00,7,1,'common',9,NULL,'Оплата по начислению № 9 (Февраль 2013) за услуги: Аренда'),(19,1,1,4,1,'2013-01-24',3500.00,8,3,'common',10,NULL,'Оплата по начислению № 10 (Февраль 2013) за услуги: Водоснабжение'),(20,1,1,4,1,'2013-01-24',1500.00,8,6,'common',10,NULL,'Оплата по начислению № 10 (Февраль 2013) за услуги: Интернет'),(21,1,1,2,1,'2013-01-24',45780.00,10,1,'common',11,NULL,'Оплата по начислению № 11 (Февраль 2013) за услуги: Аренда'),(22,1,1,2,1,'2013-01-24',2000.00,10,3,'common',11,NULL,'Оплата по начислению № 11 (Февраль 2013) за услуги: Водоснабжение'),(23,1,1,2,1,'2013-01-24',2000.00,10,6,'common',11,NULL,'Оплата по начислению № 11 (Февраль 2013) за услуги: Интернет'),(24,1,1,2,1,'2013-01-24',1500.00,10,7,'common',11,NULL,'Оплата по начислению № 11 (Февраль 2013) за услуги: Уборка'),(25,1,1,NULL,1,'2013-01-24',1500.00,NULL,NULL,'advance',NULL,3,'Возврат в кассу денежных средств по авансовому отчету №2'),(26,1,1,5,1,'2013-01-24',750.00,3,3,'common',1,NULL,'Оплата по начислению № 1 (Январь 2013) за услуги: Водоснабжение'),(27,1,1,5,1,'2013-01-29',10800.00,16,1,'common',12,NULL,'Оплата по начислению № 12 (Февраль 2013) за услуги: Аренда'),(28,1,1,5,1,'2013-01-29',30000.00,16,3,'common',12,NULL,'Оплата по начислению № 12 (Февраль 2013) за услуги: Водоснабжение'),(29,1,1,5,1,'2013-01-29',3000.00,16,6,'common',12,NULL,'Оплата по начислению № 12 (Февраль 2013) за услуги: Интернет'),(30,1,1,5,1,'2013-01-29',2000.00,16,5,'common',12,NULL,'Оплата по начислению № 12 (Февраль 2013) за услуги: Охрана'),(31,1,1,5,1,'2013-01-29',1500.00,16,4,'common',12,NULL,'Оплата по начислению № 12 (Февраль 2013) за услуги: Телефон'),(32,1,1,5,1,'2013-01-29',2500.00,16,7,'common',12,NULL,'Оплата по начислению № 12 (Февраль 2013) за услуги: Уборка'),(33,1,1,2,1,'2013-01-29',1000.00,10,5,'common',11,NULL,'Оплата по начислению № 11 (Февраль 2013) за услуги: Охрана'),(34,1,1,2,1,'2013-01-29',350.00,10,4,'common',11,NULL,'Оплата по начислению № 11 (Февраль 2013) за услуги: Телефон'),(35,1,1,NULL,1,'2013-01-29',10000.00,NULL,NULL,'advance',NULL,2,'Возврат в кассу денежных средств по авансовому отчету №3'),(36,1,1,2,1,'2013-10-08',49985.00,10,NULL,'payment',32,NULL,'Оплата по начислению № 32 (Октябрь 2013) за услуги: Аренда, Водоснабжение, Электричество'),(37,1,1,6,1,'2013-10-08',400295.00,1,NULL,'payment',26,NULL,'Оплата по начислению № 26 (Октябрь 2013) за услуги: Аренда, Электричество, Водоснабжение'),(38,1,1,5,1,'2013-10-08',43325.00,3,NULL,'payment',27,NULL,'Оплата по начислению № 27 (Октябрь 2013) за услуги: Аренда, Электричество, Водоснабжение'),(39,1,1,6,1,'2013-10-08',52011.00,6,NULL,'payment',28,NULL,'Оплата по начислению № 28 (Октябрь 2013) за услуги: Аренда, Электричество, Водоснабжение'),(40,1,1,5,1,'2013-10-08',12888.00,7,NULL,'payment',29,NULL,'Оплата по начислению № 29 (Октябрь 2013) за услуги: Аренда, Электричество, Электричество'),(41,1,1,4,1,'2013-10-08',47500.00,8,NULL,'payment',30,NULL,'Оплата по начислению № 30 (Октябрь 2013) за услуги: Аренда, Водоснабжение, Интернет'),(42,1,1,2,1,'2013-10-08',16412.00,9,NULL,'payment',31,NULL,'Оплата по начислению № 31 (Октябрь 2013) за услуги: Водоснабжение, Аренда, Электричество'),(43,1,1,2,1,'2013-10-08',5961.00,10,NULL,'payment',32,NULL,'Оплата по начислению № 32 (Октябрь 2013) за услуги: Интернет, Охрана, Уборка, Электричество'),(44,1,1,1,1,'2013-10-08',17868.00,12,NULL,'payment',33,NULL,'Оплата по начислению № 33 (Октябрь 2013) за услуги: Аренда, Охрана, Интернет, Телефон, Водоснабжение'),(45,1,1,7,1,'2013-10-08',21500.00,14,NULL,'payment',34,NULL,'Оплата по начислению № 34 (Октябрь 2013) за услуги: Аренда, Водоснабжение'),(46,1,1,7,1,'2013-10-08',33586.00,15,NULL,'payment',35,NULL,'Оплата по начислению № 35 (Октябрь 2013) за услуги: Аренда, Электричество, Водоснабжение'),(47,1,1,5,1,'2013-10-08',49800.00,16,NULL,'payment',36,NULL,'Оплата по начислению № 36 (Октябрь 2013) за услуги: Аренда, Водоснабжение, Интернет, Охрана, Телефон, Уборка'),(48,1,1,3,1,'2013-10-08',3541.00,17,NULL,'payment',37,NULL,'Оплата по начислению № 37 (Октябрь 2013) за услуги: Электричество, Водоснабжение'),(49,1,1,4,1,'2013-10-08',29475.00,18,NULL,'payment',38,NULL,'Оплата по начислению № 38 (Октябрь 2013) за услуги: Аренда, Водоснабжение, Электричество'),(50,1,1,6,1,'2013-10-08',401000.05,1,NULL,'payment',40,NULL,'Оплата по начислению № 40 (Ноябрь 2013) за услуги: Аренда, Электричество, Водоснабжение, Водоснабжение'),(51,1,1,6,1,'2013-10-08',29226.00,6,NULL,'payment',41,NULL,'Оплата по начислению № 41 (Ноябрь 2013) за услуги: Аренда, Электричество, Водоснабжение'),(52,1,1,5,1,'2013-10-08',22161.00,7,NULL,'payment',42,NULL,'Оплата по начислению № 42 (Ноябрь 2013) за услуги: Аренда, Электричество, Электричество'),(53,1,1,2,1,'2013-10-08',8938.00,9,NULL,'payment',44,NULL,'Оплата по начислению № 44 (Ноябрь 2013) за услуги: Аренда, Электричество'),(54,1,1,7,1,'2013-10-08',21800.00,14,NULL,'payment',47,NULL,'Оплата по начислению № 47 (Ноябрь 2013) за услуги: Аренда, Водоснабжение'),(55,1,1,3,1,'2013-10-08',23668.00,17,NULL,'payment',49,NULL,'Оплата по начислению № 49 (Ноябрь 2013) за услуги: Аренда, Водоснабжение'),(56,1,1,4,1,'2013-10-08',30060.00,18,NULL,'payment',50,NULL,'Оплата по начислению № 50 (Ноябрь 2013) за услуги: Аренда, Водоснабжение, Электричество'),(57,1,1,NULL,1,'2013-10-08',2000.00,NULL,NULL,'advance',NULL,3,'Возврат в кассу денежных средств по авансовому отчету №5'),(58,1,1,8,1,'2013-10-08',76767.00,21,NULL,'payment',52,NULL,'Оплата по начислению № 52 (Октябрь 2013) за услуги: Аренда, Водоснабжение, Электричество'),(59,1,1,9,1,'2013-10-08',14168.60,20,NULL,'payment',51,NULL,'Оплата по начислению № 51 (Октябрь 2013) за услуги: Аренда, Водоснабжение, Электричество'),(60,1,1,2,1,'2013-10-08',350.00,10,NULL,'payment',32,NULL,'Оплата по начислению № 32 (Октябрь 2013) за услуги: Телефон'),(61,1,1,3,1,'2013-10-08',21400.00,17,NULL,'payment',37,NULL,'Оплата по начислению № 37 (Октябрь 2013) за услуги: Аренда'),(62,1,1,1,1,'2013-10-08',2784.00,12,NULL,'payment',33,NULL,'Оплата по начислению № 33 (Октябрь 2013) за услуги: Электричество'),(63,1,1,3,1,'2013-10-08',963.00,17,NULL,'payment',49,NULL,'Оплата по начислению № 49 (Ноябрь 2013) за услуги: Электричество'),(64,1,1,2,1,'2013-10-08',8900.00,9,NULL,'payment',44,NULL,'Оплата по начислению № 44 (Ноябрь 2013) за услуги: Водоснабжение'),(65,1,1,2,1,'2013-10-08',8658.00,9,NULL,'payment',55,NULL,'Оплата по начислению № 55 (Декабрь 2013) за услуги: Водоснабжение, Аренда, Электричество'),(66,1,1,5,1,'2013-10-08',239865.00,3,NULL,'payment',39,NULL,'Оплата по начислению № 39 (Ноябрь 2013) за услуги: Водоснабжение'),(67,1,1,2,1,'2013-10-08',53998.00,10,NULL,'payment',45,NULL,'Оплата по начислению № 45 (Ноябрь 2013) за услуги: Аренда, Водоснабжение, Интернет, Охрана, Телефон, Уборка, Электричество');
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
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `debit_slips`
--

LOCK TABLES `debit_slips` WRITE;
/*!40000 ALTER TABLE `debit_slips` DISABLE KEYS */;
INSERT INTO `debit_slips` VALUES (1,1,1,NULL,1,'2013-01-24',3000.00,3,'advance',1,NULL),(2,1,1,1,1,'2013-01-24',200000.00,2,'common',NULL,NULL),(3,1,1,5,1,'2013-01-24',50000.00,4,'common',NULL,NULL),(4,1,1,4,1,'2013-01-24',30000.00,3,'common',NULL,NULL),(5,1,1,NULL,1,'2013-01-24',6500.00,NULL,'advance',3,NULL),(6,1,1,NULL,1,'2013-01-24',15000.00,1,'advance',3,NULL),(7,1,1,NULL,1,'2013-01-24',35000.00,2,'advance',2,NULL),(8,1,1,NULL,1,'2013-01-29',1000.00,NULL,'advance',3,'На сантехнику'),(9,1,1,NULL,1,'2013-01-29',5000.00,NULL,'advance',2,'На кофеварку'),(10,1,1,NULL,1,'2013-01-29',300.00,NULL,'advance',1,'На кофе для новой кофеварки.'),(11,1,1,2,1,'2013-10-08',8000.00,3,'common',1,'Оплата мобильной связи'),(12,1,1,NULL,1,'2013-10-08',90000.00,1,'advance',3,NULL),(13,1,1,1,1,'2013-10-08',670000.00,2,'common',NULL,NULL),(14,1,1,NULL,1,'2013-10-08',10000.00,NULL,'advance',2,'Доплата денежных средств сотруднику по авансовому отчету №6'),(15,1,1,NULL,1,'2013-10-08',4847.00,2,'advance',1,NULL),(16,1,1,NULL,1,'2013-10-08',3890.00,4,'advance',2,NULL),(17,1,1,NULL,1,'2013-10-08',15274.00,3,'advance',3,'Оплата связи для арендаторов');
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
-- Table structure for table `meter_reading`
--

DROP TABLE IF EXISTS `meter_reading`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `meter_reading` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `date` date NOT NULL,
  `meter_id` int(10) unsigned NOT NULL,
  `meter_tariff_id` int(10) unsigned NOT NULL,
  `value` int(10) unsigned NOT NULL,
  `accrual_pay_id` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_meter_reading_meter_idx` (`meter_id`),
  KEY `fk_meter_reading_tariff_idx` (`meter_tariff_id`),
  KEY `fk_meter_reading_accrual_pay_idx` (`accrual_pay_id`),
  CONSTRAINT `fk_meter_reading_meter` FOREIGN KEY (`meter_id`) REFERENCES `meters` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_meter_reading_tariff` FOREIGN KEY (`meter_tariff_id`) REFERENCES `meter_tariffs` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_meter_reading_accrual_pay` FOREIGN KEY (`accrual_pay_id`) REFERENCES `accrual_pays` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=138 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `meter_reading`
--

LOCK TABLES `meter_reading` WRITE;
/*!40000 ALTER TABLE `meter_reading` DISABLE KEYS */;
INSERT INTO `meter_reading` VALUES (1,'2013-10-08',24,1,120,81),(2,'2013-10-08',24,2,200,81),(3,'2013-10-08',25,3,543,81),(4,'2013-10-08',3,1,357,101),(5,'2013-10-08',3,2,10,101),(6,'2013-10-08',4,1,134,101),(7,'2013-10-08',4,2,234,101),(8,'2013-10-08',10,1,432,106),(9,'2013-10-08',10,2,67,106),(10,'2013-10-08',34,5,67,NULL),(11,'2013-10-01',2,3,90,NULL),(12,'2013-09-01',1,1,78,NULL),(13,'2013-10-08',3,1,67,NULL),(14,'2013-09-01',35,5,67,NULL),(15,'2013-10-08',5,1,9,NULL),(16,'2013-08-01',7,5,780,NULL),(17,'2013-08-01',30,1,890,NULL),(18,'2013-07-02',29,5,890,NULL),(19,'2013-08-01',12,1,789,NULL),(20,'2013-06-06',33,5,34,NULL),(21,'2013-05-01',36,5,90,NULL),(22,'2013-07-01',13,3,12,NULL),(23,'2013-10-01',16,1,86,NULL),(24,'2013-10-08',32,5,89,NULL),(26,'2013-09-01',38,5,9,NULL),(27,'2013-10-08',1,1,900,84),(28,'2013-10-08',1,2,789,84),(29,'2013-10-08',2,3,456,84),(30,'2013-10-08',29,5,900,79),(31,'2013-10-08',30,2,90,78),(32,'2013-10-08',30,10,80,78),(33,'2013-10-08',30,1,1000,78),(34,'2013-10-08',24,10,700,81),(35,'2013-10-08',25,4,600,81),(36,'2013-10-08',38,5,45,82),(37,'2013-10-08',38,5,80000,130),(38,'2013-10-08',34,5,150,85),(39,'2013-10-08',27,1,100,87),(40,'2013-10-08',27,2,290,87),(41,'2013-10-08',27,10,450,87),(42,'2013-10-08',27,1,560,88),(43,'2013-10-08',27,2,780,88),(44,'2013-10-08',27,10,456,88),(45,'2013-10-08',7,5,4500,92),(46,'2013-10-08',5,1,890,94),(47,'2013-10-08',5,2,34,94),(48,'2013-10-08',5,10,567,94),(49,'2013-10-08',6,3,123,94),(50,'2013-10-08',6,4,987,94),(51,'2013-10-08',3,10,456,101),(52,'2013-10-08',4,10,98,101),(53,'2013-10-08',10,10,76,106),(54,'2013-10-08',11,3,23,106),(55,'2013-10-08',11,4,98,106),(56,'2013-10-08',40,5,500,110),(57,'2013-10-08',12,1,799,113),(58,'2013-10-08',12,2,78,113),(59,'2013-10-08',12,10,23,113),(60,'2013-10-08',13,3,24,113),(61,'2013-10-08',13,4,65,113),(62,'2013-10-08',33,5,80,114),(63,'2013-10-08',36,5,190,114),(64,'2013-10-08',14,1,23,123),(65,'2013-10-08',14,2,65,123),(66,'2013-10-08',14,10,43,123),(67,'2013-10-08',15,1,235,123),(68,'2013-10-08',15,2,543,123),(69,'2013-10-08',15,10,98,123),(70,'2013-10-08',31,5,230,124),(71,'2013-10-08',37,5,65,126),(72,'2013-10-08',22,1,54,127),(73,'2013-10-08',22,2,18,127),(74,'2013-10-08',22,10,44,127),(75,'2013-10-08',23,3,79,127),(76,'2013-10-08',24,1,250,129),(77,'2013-10-08',24,2,400,129),(78,'2013-10-08',24,10,890,129),(79,'2013-10-08',25,3,670,129),(80,'2013-10-08',30,1,1200,132),(81,'2013-10-08',30,2,500,132),(82,'2013-10-08',30,10,130,132),(83,'2013-10-08',1,1,1300,139),(84,'2013-10-08',1,2,890,139),(85,'2013-10-08',1,10,876,139),(86,'2013-10-08',2,3,500,139),(87,'2013-10-08',27,1,678,142),(88,'2013-10-08',27,2,870,142),(89,'2013-10-08',27,10,600,142),(90,'2013-10-08',27,1,789,143),(91,'2013-10-08',27,2,4790,143),(92,'2013-10-08',27,10,789,143),(93,'2013-10-08',5,1,900,149),(94,'2013-10-08',5,2,678,149),(95,'2013-10-08',5,10,987,149),(96,'2013-10-08',6,3,5443,149),(97,'2013-10-08',14,1,57,171),(98,'2013-10-08',14,2,94,171),(99,'2013-10-08',14,10,85,171),(100,'2013-10-08',15,1,340,171),(101,'2013-10-08',15,2,579,171),(102,'2013-10-08',15,10,173,171),(103,'2013-10-08',22,1,64,175),(104,'2013-10-08',22,2,30,175),(105,'2013-10-08',22,10,90,175),(106,'2013-10-08',23,3,89,175),(107,'2013-10-08',26,3,789,181),(108,'2013-10-08',8,1,789,178),(109,'2013-10-08',8,2,15,178),(110,'2013-10-08',8,10,23,178),(111,'2013-10-08',9,3,26,178),(112,'2013-10-08',5,1,980,190),(113,'2013-10-08',5,2,789,190),(114,'2013-10-08',5,10,6540,190),(115,'2013-10-08',6,3,5555,190),(116,'2013-10-08',10,1,567,161),(117,'2013-10-08',10,2,100,161),(118,'2013-10-08',10,10,89,161),(119,'2013-10-08',11,3,45,161),(120,'2013-10-08',11,4,109,161),(121,'2013-10-08',26,3,890,187),(122,'2013-10-08',26,4,78,187),(123,'2013-10-08',45,6,12,145),(124,'2013-10-08',3,1,400,156),(125,'2013-10-08',3,2,12,156),(126,'2013-10-08',3,10,500,156),(127,'2013-10-08',4,1,150,156),(128,'2013-10-08',4,2,300,156),(129,'2013-10-08',4,10,123,156),(130,'2013-10-08',35,5,345,151),(131,'2013-10-08',33,5,99,169),(132,'2013-10-08',36,5,250,169),(133,'2013-10-08',12,1,899,168),(134,'2013-10-08',12,2,107,168),(135,'2013-10-08',12,10,34,168),(136,'2013-10-08',13,3,45,168),(137,'2013-10-08',13,4,78,168);
/*!40000 ALTER TABLE `meter_reading` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `meter_tariffs`
--

DROP TABLE IF EXISTS `meter_tariffs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `meter_tariffs` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `meter_type_id` int(10) unsigned NOT NULL,
  `service_id` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_meter_tariffs_meter_type_idx` (`meter_type_id`),
  KEY `fk_meter_tariffs_service_idx` (`service_id`),
  CONSTRAINT `fk_meter_tariffs_meter_type` FOREIGN KEY (`meter_type_id`) REFERENCES `meter_types` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_meter_tariffs_service` FOREIGN KEY (`service_id`) REFERENCES `services` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `meter_tariffs`
--

LOCK TABLES `meter_tariffs` WRITE;
/*!40000 ALTER TABLE `meter_tariffs` DISABLE KEYS */;
INSERT INTO `meter_tariffs` VALUES (1,'Основной',1,7),(2,'Новый тариф',1,7),(3,'Основной',2,7),(4,'подключенный',2,7),(5,'Основной',3,2),(6,'Основной',4,2),(10,'Разовый',1,7);
/*!40000 ALTER TABLE `meter_tariffs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `meter_types`
--

DROP TABLE IF EXISTS `meter_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `meter_types` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `meter_types`
--

LOCK TABLES `meter_types` WRITE;
/*!40000 ALTER TABLE `meter_types` DISABLE KEYS */;
INSERT INTO `meter_types` VALUES (1,'Многотарифный'),(2,'Однотарифный '),(3,'Водомер2'),(4,'Вода горячая');
/*!40000 ALTER TABLE `meter_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `meters`
--

DROP TABLE IF EXISTS `meters`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `meters` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `meter_type_id` int(10) unsigned NOT NULL,
  `place_type_id` int(10) unsigned NOT NULL,
  `place_no` varchar(10) NOT NULL,
  `parent_meter_id` int(10) unsigned DEFAULT NULL,
  `disabled` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `fk_meters_type_idx` (`meter_type_id`),
  KEY `fk_meters_place_idx` (`place_type_id`,`place_no`),
  KEY `fk_meters_parent_idx` (`parent_meter_id`),
  CONSTRAINT `fk_meters_type` FOREIGN KEY (`meter_type_id`) REFERENCES `meter_types` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_meters_place` FOREIGN KEY (`place_type_id`, `place_no`) REFERENCES `places` (`type_id`, `place_no`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_meters_parent` FOREIGN KEY (`parent_meter_id`) REFERENCES `meters` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=47 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `meters`
--

LOCK TABLES `meters` WRITE;
/*!40000 ALTER TABLE `meters` DISABLE KEYS */;
INSERT INTO `meters` VALUES (1,'Оф-10',1,1,'10',NULL,0),(2,'Оф-10',2,1,'10',1,0),(3,'Оф-2',1,1,'2',1,0),(4,'Оф-2',1,1,'2',NULL,0),(5,'Оф-3',1,1,'3',NULL,0),(6,'Оф-3',2,1,'3',NULL,0),(7,'Оф-3',3,1,'3',NULL,0),(8,'Оф-34',1,1,'34',NULL,0),(9,'Оф-34',2,1,'34',NULL,0),(10,'Оф-5',1,1,'5',9,0),(11,'Оф-5',2,1,'5',NULL,0),(12,'Оф-55',1,1,'55',1,0),(13,'Оф-55',2,1,'55',NULL,0),(14,'Ск-4',1,2,'4',NULL,0),(15,'Ск-4',1,2,'4',NULL,0),(16,'Ск-6',1,2,'6',NULL,0),(17,'Ск-6',2,2,'6',NULL,0),(18,'Ск-7',1,2,'7',1,0),(19,'Ск-7',2,2,'7',NULL,0),(20,'ТП-1',1,3,'1',2,0),(21,'ТП-1',2,3,'1',NULL,0),(22,'ТП-2',1,3,'2',5,0),(23,'ТП-2',2,3,'2',NULL,0),(24,'П-1',1,4,'1',NULL,0),(25,'П-1',2,4,'1',NULL,0),(26,'П-3',2,4,'3',NULL,0),(27,'Ч-1',1,13,'1',NULL,0),(28,'Ч-34',2,13,'34',NULL,0),(29,'Оф-450',3,1,'450',NULL,0),(30,'Оф-450',1,1,'450',NULL,0),(31,'Ск-4',3,2,'4',NULL,0),(32,'Ск-6',3,2,'6',NULL,1),(33,'Оф-55',3,1,'55',NULL,0),(34,'Оф-10',3,1,'10',NULL,0),(35,'Оф-2',3,1,'2',NULL,0),(36,'Оф-55',3,1,'55',31,0),(37,'ТП-2',3,3,'2',36,0),(38,'П-1',3,4,'1',NULL,0),(39,'Ч-1',3,13,'1',NULL,0),(40,'Ч-34',3,13,'34',NULL,0),(41,'П-3',3,4,'3',NULL,0),(42,'П-3',4,4,'3',NULL,1),(43,'П-1',4,4,'1',NULL,1),(44,'ТП-2',4,3,'2',NULL,1),(45,'Ск-7',4,2,'7',29,0),(46,'Ск-6',4,2,'6',NULL,0);
/*!40000 ALTER TABLE `meters` ENABLE KEYS */;
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
-- Table structure for table `payment_details`
--

DROP TABLE IF EXISTS `payment_details`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `payment_details` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `payment_id` int(10) unsigned NOT NULL,
  `accrual_pay_id` int(10) unsigned DEFAULT NULL,
  `sum` decimal(10,0) NOT NULL DEFAULT '0',
  `income_id` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_pay_details_1_idx` (`accrual_pay_id`),
  KEY `fk_pay_details_income_idx` (`income_id`),
  KEY `fk_payment_details_parent_idx` (`payment_id`),
  CONSTRAINT `fk_payment_details_parent` FOREIGN KEY (`payment_id`) REFERENCES `payments` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_pay_details_accrual` FOREIGN KEY (`accrual_pay_id`) REFERENCES `accrual_pays` (`id`) ON DELETE SET NULL ON UPDATE NO ACTION,
  CONSTRAINT `fk_pay_details_income` FOREIGN KEY (`income_id`) REFERENCES `income_items` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=88 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `payment_details`
--

LOCK TABLES `payment_details` WRITE;
/*!40000 ALTER TABLE `payment_details` DISABLE KEYS */;
INSERT INTO `payment_details` VALUES (1,1,95,45780,1),(2,1,96,2000,3),(3,1,101,2205,2),(4,2,77,400000,1),(5,2,78,280,2),(6,2,79,15,3),(7,3,80,35000,1),(8,3,81,4326,2),(9,3,82,3999,3),(10,4,83,50000,1),(11,4,84,1596,2),(12,4,85,415,3),(13,5,86,7500,1),(14,5,87,2520,2),(15,5,88,2868,2),(16,6,89,42500,1),(17,6,90,3500,3),(18,6,91,1500,6),(19,7,92,11160,3),(20,7,93,2660,1),(21,7,94,2592,2),(22,8,97,2000,6),(23,8,98,1000,5),(24,8,100,1500,4),(25,8,101,1461,2),(26,9,102,6300,1),(27,9,103,3000,5),(28,9,104,3000,6),(29,9,105,1500,4),(30,9,107,4068,3),(31,10,109,20000,1),(32,10,110,1500,3),(33,11,112,32500,1),(34,11,113,940,2),(35,11,114,146,3),(36,12,115,10800,1),(37,12,116,30000,3),(38,12,117,3000,6),(39,12,118,2000,5),(40,12,119,1500,4),(41,12,120,2500,4),(42,13,123,3021,2),(43,13,124,520,3),(44,14,125,27000,1),(45,14,126,1500,3),(46,14,127,975,2),(47,15,131,400000,1),(48,15,132,660,2),(49,15,133,102,3),(50,15,134,238,3),(51,16,138,26600,1),(52,16,139,376,2),(53,16,140,2250,3),(54,17,141,7500,1),(55,17,142,2001,2),(56,17,143,12660,2),(57,18,148,2660,1),(58,18,149,6278,2),(59,19,164,20000,1),(60,19,165,1800,3),(61,20,170,21400,1),(62,20,172,2268,3),(63,21,173,27000,1),(64,21,174,2670,3),(65,21,175,390,2),(66,22,179,71010,1),(67,22,180,3390,3),(68,22,181,2367,2),(69,23,176,13608,1),(70,23,177,88,3),(71,23,178,473,2),(72,24,99,350,4),(73,25,122,21400,1),(74,26,106,2784,2),(75,27,171,963,2),(76,28,147,8900,3),(77,29,188,210,3),(78,29,189,2660,1),(79,29,190,5788,2),(80,30,130,239865,3),(81,31,150,45780,1),(82,31,151,2780,3),(83,31,152,2000,6),(84,31,153,1000,5),(85,31,154,350,4),(86,31,155,1500,4),(87,31,156,588,2);
/*!40000 ALTER TABLE `payment_details` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `payments`
--

DROP TABLE IF EXISTS `payments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `payments` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `createdate` datetime DEFAULT NULL,
  `credit_slip_id` int(10) unsigned DEFAULT NULL,
  `accrual_id` int(10) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_pays_credit_slips_idx` (`credit_slip_id`),
  KEY `fk_payments_accrual_idx` (`accrual_id`),
  CONSTRAINT `fk_payments_accrual` FOREIGN KEY (`accrual_id`) REFERENCES `accrual` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_pays_credit_slips` FOREIGN KEY (`credit_slip_id`) REFERENCES `credit_slips` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `payments`
--

LOCK TABLES `payments` WRITE;
/*!40000 ALTER TABLE `payments` DISABLE KEYS */;
INSERT INTO `payments` VALUES (1,'2013-10-08 00:00:00',36,32),(2,'2013-10-08 00:00:00',37,26),(3,'2013-10-08 00:00:00',38,27),(4,'2013-10-08 00:00:00',39,28),(5,'2013-10-08 00:00:00',40,29),(6,'2013-10-08 00:00:00',41,30),(7,'2013-10-08 00:00:00',42,31),(8,'2013-10-08 00:00:00',43,32),(9,'2013-10-08 00:00:00',44,33),(10,'2013-10-08 00:00:00',45,34),(11,'2013-10-08 00:00:00',46,35),(12,'2013-10-08 00:00:00',47,36),(13,'2013-10-08 00:00:00',48,37),(14,'2013-10-08 00:00:00',49,38),(15,'2013-10-08 00:00:00',50,40),(16,'2013-10-08 00:00:00',51,41),(17,'2013-10-08 00:00:00',52,42),(18,'2013-10-08 00:00:00',53,44),(19,'2013-10-08 00:00:00',54,47),(20,'2013-10-08 00:00:00',55,49),(21,'2013-10-08 00:00:00',56,50),(22,'2013-10-08 00:00:00',58,52),(23,'2013-10-08 00:00:00',59,51),(24,'2013-10-08 00:00:00',60,32),(25,'2013-10-08 00:00:00',61,37),(26,'2013-10-08 00:00:00',62,33),(27,'2013-10-08 00:00:00',63,49),(28,'2013-10-08 00:00:00',64,44),(29,'2013-10-08 00:00:00',65,55),(30,'2013-10-08 00:00:00',66,39),(31,'2013-10-08 00:00:00',67,45);
/*!40000 ALTER TABLE `payments` ENABLE KEYS */;
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
  `by_area` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`),
  KEY `units_id_idx` (`units_id`),
  KEY `fk_services_income_id_idx` (`income_id`),
  CONSTRAINT `fk_services_income_id` FOREIGN KEY (`income_id`) REFERENCES `income_items` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `units_id` FOREIGN KEY (`units_id`) REFERENCES `units` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `services`
--

LOCK TABLES `services` WRITE;
/*!40000 ALTER TABLE `services` DISABLE KEYS */;
INSERT INTO `services` VALUES (1,'Аренда',3,1,1),(2,'Водоснабжение',4,3,0),(3,'Телефон',2,4,0),(4,'Уборка',2,4,0),(5,'Интернет',2,6,0),(6,'Охрана',2,5,0),(7,'Электричество',5,2,0);
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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `units`
--

LOCK TABLES `units` WRITE;
/*!40000 ALTER TABLE `units` DISABLE KEYS */;
INSERT INTO `units` VALUES (1,'шт.'),(2,'усл.'),(3,'кв. м.'),(4,'литр'),(5,'Квт');
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

--
-- Final view structure for view `active_contracts`
--

/*!50001 DROP TABLE IF EXISTS `active_contracts`*/;
/*!50001 DROP VIEW IF EXISTS `active_contracts`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `active_contracts` AS select `contracts`.`id` AS `id`,`contracts`.`number` AS `number`,`contracts`.`lessee_id` AS `lessee_id`,`contracts`.`org_id` AS `org_id`,`contracts`.`place_type_id` AS `place_type_id`,`contracts`.`place_no` AS `place_no`,`contracts`.`sign_date` AS `sign_date`,`contracts`.`start_date` AS `start_date`,`contracts`.`end_date` AS `end_date`,`contracts`.`pay_day` AS `pay_day`,`contracts`.`cancel_date` AS `cancel_date`,`contracts`.`comments` AS `comments` from `contracts` where ((isnull(`contracts`.`cancel_date`) and (curdate() between `contracts`.`start_date` and `contracts`.`end_date`)) or ((`contracts`.`cancel_date` is not null) and (curdate() between `contracts`.`start_date` and `contracts`.`cancel_date`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2013-10-08 17:35:01
