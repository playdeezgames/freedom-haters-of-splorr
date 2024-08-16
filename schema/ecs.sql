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

-- Dumping structure for table ecs.entities
CREATE TABLE IF NOT EXISTS `entities` (
  `entity_id` int(11) NOT NULL AUTO_INCREMENT,
  `entity_type` varchar(50) NOT NULL,
  PRIMARY KEY (`entity_id`),
  KEY `entity_type` (`entity_type`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

-- Dumping data for table ecs.entities: ~0 rows (approximately)

-- Dumping structure for table ecs.entity_counters
CREATE TABLE IF NOT EXISTS `entity_counters` (
  `entity_counter_id` int(11) NOT NULL AUTO_INCREMENT,
  `entity_id` int(11) NOT NULL,
  `counter_type` varchar(50) NOT NULL,
  `counter_value` int(11) NOT NULL,
  PRIMARY KEY (`entity_counter_id`) USING BTREE,
  UNIQUE KEY `entity_id_counter_type` (`entity_id`,`counter_type`),
  CONSTRAINT `FK_counters_entities` FOREIGN KEY (`entity_id`) REFERENCES `entities` (`entity_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

-- Dumping data for table ecs.entity_counters: ~0 rows (approximately)

-- Dumping structure for table ecs.entity_flags
CREATE TABLE IF NOT EXISTS `entity_flags` (
  `entity_flag_id` int(11) NOT NULL AUTO_INCREMENT,
  `entity_id` int(11) NOT NULL,
  `flag_type` varchar(50) NOT NULL,
  PRIMARY KEY (`entity_flag_id`) USING BTREE,
  UNIQUE KEY `entity_id_flag_type` (`entity_id`,`flag_type`),
  CONSTRAINT `FK__entities` FOREIGN KEY (`entity_id`) REFERENCES `entities` (`entity_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

-- Dumping data for table ecs.entity_flags: ~0 rows (approximately)

-- Dumping structure for table ecs.entity_metadatas
CREATE TABLE IF NOT EXISTS `entity_metadatas` (
  `entity_metadata_id` int(11) NOT NULL AUTO_INCREMENT,
  `entity_id` int(11) NOT NULL,
  `metadata_type` varchar(50) NOT NULL,
  `metadata_value` varchar(250) NOT NULL,
  PRIMARY KEY (`entity_metadata_id`) USING BTREE,
  UNIQUE KEY `entity_id_metadata_type` (`entity_id`,`metadata_type`),
  CONSTRAINT `FK_metadatas_entities` FOREIGN KEY (`entity_id`) REFERENCES `entities` (`entity_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

-- Dumping data for table ecs.entity_metadatas: ~0 rows (approximately)

-- Dumping structure for table ecs.entity_statistics
CREATE TABLE IF NOT EXISTS `entity_statistics` (
  `entity_statistic_id` int(11) NOT NULL AUTO_INCREMENT,
  `entity_id` int(11) NOT NULL,
  `statistic_type` varchar(50) NOT NULL,
  `statistic_value` double NOT NULL,
  PRIMARY KEY (`entity_statistic_id`) USING BTREE,
  UNIQUE KEY `entity_id_statistic_type` (`entity_id`,`statistic_type`),
  CONSTRAINT `FK_statistics_entities` FOREIGN KEY (`entity_id`) REFERENCES `entities` (`entity_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

-- Dumping data for table ecs.entity_statistics: ~0 rows (approximately)

-- Dumping structure for table ecs.yokes
CREATE TABLE IF NOT EXISTS `yokes` (
  `yoke_id` int(11) NOT NULL AUTO_INCREMENT,
  `from_entity_id` int(11) NOT NULL,
  `to_entity_id` int(11) NOT NULL,
  `yoke_type` varchar(50) NOT NULL,
  PRIMARY KEY (`yoke_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

-- Dumping data for table ecs.yokes: ~0 rows (approximately)

-- Dumping structure for table ecs.yoke_counters
CREATE TABLE IF NOT EXISTS `yoke_counters` (
  `yoke_counter_id` int(11) NOT NULL AUTO_INCREMENT,
  `yoke_id` int(11) NOT NULL,
  `counter_type` varchar(50) NOT NULL,
  `counter_value` int(11) NOT NULL,
  PRIMARY KEY (`yoke_counter_id`),
  UNIQUE KEY `yoke_id_counter_type` (`yoke_id`,`counter_type`),
  CONSTRAINT `FK_yoke_counters_yokes` FOREIGN KEY (`yoke_id`) REFERENCES `yokes` (`yoke_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

-- Dumping data for table ecs.yoke_counters: ~0 rows (approximately)

-- Dumping structure for table ecs.yoke_flags
CREATE TABLE IF NOT EXISTS `yoke_flags` (
  `yoke_flag_id` int(11) NOT NULL AUTO_INCREMENT,
  `yoke_id` int(11) NOT NULL,
  `flag_type` varchar(50) NOT NULL,
  PRIMARY KEY (`yoke_flag_id`),
  UNIQUE KEY `yoke_id_flag_type` (`yoke_id`,`flag_type`),
  CONSTRAINT `FK_yoke_flags_yokes` FOREIGN KEY (`yoke_id`) REFERENCES `yokes` (`yoke_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

-- Dumping data for table ecs.yoke_flags: ~0 rows (approximately)

-- Dumping structure for table ecs.yoke_metadatas
CREATE TABLE IF NOT EXISTS `yoke_metadatas` (
  `yoke_metadata_id` int(11) NOT NULL AUTO_INCREMENT,
  `yoke_id` int(11) NOT NULL,
  `metadata_type` varchar(50) NOT NULL,
  `metadata_value` varchar(250) NOT NULL,
  PRIMARY KEY (`yoke_metadata_id`),
  UNIQUE KEY `yoke_id_metadata_type` (`yoke_id`,`metadata_type`),
  CONSTRAINT `FK_yoke_metadatas_yokes` FOREIGN KEY (`yoke_id`) REFERENCES `yokes` (`yoke_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

-- Dumping data for table ecs.yoke_metadatas: ~0 rows (approximately)

-- Dumping structure for table ecs.yoke_statistics
CREATE TABLE IF NOT EXISTS `yoke_statistics` (
  `yoke_statistic_id` int(11) NOT NULL AUTO_INCREMENT,
  `yoke_id` int(11) NOT NULL,
  `statistic_type` varchar(50) NOT NULL,
  `statistic_value` double NOT NULL,
  PRIMARY KEY (`yoke_statistic_id`),
  UNIQUE KEY `yoke_id_statistic_type` (`yoke_id`,`statistic_type`),
  CONSTRAINT `FK_yoke_statistics_yokes` FOREIGN KEY (`yoke_id`) REFERENCES `yokes` (`yoke_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

-- Dumping data for table ecs.yoke_statistics: ~0 rows (approximately)

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
