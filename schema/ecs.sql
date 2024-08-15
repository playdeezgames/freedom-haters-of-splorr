-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               11.4.2-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win64
-- HeidiSQL Version:             12.6.0.6765
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dumping database structure for ecs
CREATE DATABASE IF NOT EXISTS `ecs` /*!40100 DEFAULT CHARACTER SET latin1 COLLATE latin1_general_ci */;
USE `ecs`;

-- Dumping structure for table ecs.counters
CREATE TABLE IF NOT EXISTS `counters` (
  `counter_id` int(11) NOT NULL AUTO_INCREMENT,
  `entity_id` int(11) NOT NULL,
  `counter_type` varchar(50) NOT NULL,
  `counter_value` int(11) NOT NULL,
  PRIMARY KEY (`counter_id`),
  UNIQUE KEY `entity_id_counter_type` (`entity_id`,`counter_type`),
  CONSTRAINT `FK_counters_entities` FOREIGN KEY (`entity_id`) REFERENCES `entities` (`entity_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

-- Dumping data for table ecs.counters: ~0 rows (approximately)

-- Dumping structure for table ecs.entities
CREATE TABLE IF NOT EXISTS `entities` (
  `entity_id` int(11) NOT NULL AUTO_INCREMENT,
  `entity_type` varchar(50) NOT NULL,
  PRIMARY KEY (`entity_id`),
  KEY `entity_type` (`entity_type`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

-- Dumping data for table ecs.entities: ~0 rows (approximately)

-- Dumping structure for table ecs.flags
CREATE TABLE IF NOT EXISTS `flags` (
  `flag_id` int(11) NOT NULL AUTO_INCREMENT,
  `entity_id` int(11) NOT NULL,
  `flag_type` varchar(50) NOT NULL,
  PRIMARY KEY (`flag_id`),
  UNIQUE KEY `entity_id_flag_type` (`entity_id`,`flag_type`),
  CONSTRAINT `FK__entities` FOREIGN KEY (`entity_id`) REFERENCES `entities` (`entity_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

-- Dumping data for table ecs.flags: ~0 rows (approximately)

-- Dumping structure for table ecs.metadatas
CREATE TABLE IF NOT EXISTS `metadatas` (
  `metadata_id` int(11) NOT NULL AUTO_INCREMENT,
  `entity_id` int(11) NOT NULL,
  `metadata_type` varchar(50) NOT NULL,
  `metadata_value` varchar(250) NOT NULL,
  PRIMARY KEY (`metadata_id`),
  UNIQUE KEY `entity_id_metadata_type` (`entity_id`,`metadata_type`),
  CONSTRAINT `FK_metadatas_entities` FOREIGN KEY (`entity_id`) REFERENCES `entities` (`entity_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

-- Dumping data for table ecs.metadatas: ~0 rows (approximately)

-- Dumping structure for table ecs.statistics
CREATE TABLE IF NOT EXISTS `statistics` (
  `statistic_id` int(11) NOT NULL AUTO_INCREMENT,
  `entity_id` int(11) NOT NULL,
  `statistic_type` varchar(50) NOT NULL,
  `statistic_value` double NOT NULL,
  PRIMARY KEY (`statistic_id`),
  UNIQUE KEY `entity_id_statistic_type` (`entity_id`,`statistic_type`),
  CONSTRAINT `FK_statistics_entities` FOREIGN KEY (`entity_id`) REFERENCES `entities` (`entity_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

-- Dumping data for table ecs.statistics: ~0 rows (approximately)

-- Dumping structure for table ecs.yokes
CREATE TABLE IF NOT EXISTS `yokes` (
  `yoke_id` int(11) NOT NULL AUTO_INCREMENT,
  `from_entity_id` int(11) NOT NULL,
  `to_entity_id` int(11) NOT NULL,
  `yoke_type` varchar(50) NOT NULL,
  PRIMARY KEY (`yoke_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

-- Dumping data for table ecs.yokes: ~0 rows (approximately)

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
