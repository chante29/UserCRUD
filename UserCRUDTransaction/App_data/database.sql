-- DELIMITER ;

CREATE SCHEMA UserCRUD;

Use UserCRUD;

CREATE TABLE IF NOT EXISTS `User` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(128) NOT NULL,
  `Birthday` varchar(10) NOT NULL COMMENT 'The birthday in yyyy/MM/dd format',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB;


