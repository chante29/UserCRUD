-- DELIMITER ;

CREATE SCHEMA UserCRUD;

Use UserCRUD;

CREATE TABLE IF NOT EXISTS `User` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(128) NOT NULL,
  `Birthday` varchar(9) NOT NULL COMMENT 'The lowered e-mail,USE THIS For searchs',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB;


