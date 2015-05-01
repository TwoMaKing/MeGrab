CREATE TABLE `webapp_users` (
  `UserId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Enabled` tinyint(1) DEFAULT '1',
  `LastActivityDate` datetime DEFAULT NULL,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `Name_UNIQUE` (`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;


CREATE TABLE `webapp_membership` (
  `UserId` int(11) NOT NULL DEFAULT '0',
  `Email` varchar(128) DEFAULT NULL,
  `CellPhoneNo` varchar(12) DEFAULT NULL,
  `Password` varchar(128) NOT NULL,
  `PasswordQuestion` varchar(255) DEFAULT NULL,
  `PasswordAnswer` varchar(255) DEFAULT NULL,
  `ConfirmationToken` varchar(128) DEFAULT NULL,
  `IsApproved` tinyint(1) DEFAULT NULL,
  `LastActivityDate` datetime DEFAULT NULL,
  `LastLoginDate` datetime DEFAULT NULL,
  `LastPasswordChangedDate` datetime DEFAULT NULL,
  `CreationDate` datetime DEFAULT NULL,
  `IsLockedOut` tinyint(1) DEFAULT NULL,
  `LastLockedOutDate` datetime DEFAULT NULL,
  `FailedPasswordAttemptCount` int(10) unsigned DEFAULT NULL,
  `FailedPasswordAttemptWindowStart` datetime DEFAULT NULL,
  `PasswordVerificationToken` varchar(128) DEFAULT NULL,
  `PasswordVerificationTokenExpirationDate` datetime DEFAULT NULL,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `Email_UNIQUE` (`Email`),
  UNIQUE KEY `ConfirmationToken_UNIQUE` (`ConfirmationToken`),
  UNIQUE KEY `CellPhoneNo_UNIQUE` (`CellPhoneNo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `webapp_roles` (
  `RoleId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  PRIMARY KEY (`RoleId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `webapp_usersinroles` (
  `UserId` int(11) NOT NULL DEFAULT '0',
  `RoleId` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`UserId`,`RoleId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `redpacket_grab_activity` (
  `rpga_id` char(36) NOT NULL,
  `rpga_total_amount` decimal(7,2) NOT NULL,
  `rpga_redpacket_count` int(11) NOT NULL,
  `rpga_play_mode` tinyint(1) NOT NULL,
  `rpga_limit_member` int(11) NOT NULL,
  `rpga_start_datetime` datetime NOT NULL,
  `rpga_expire_datetime` datetime NOT NULL,
  `rpga_message` varchar(45) DEFAULT NULL,
  `rpga_dispatcher_id` int(11) NOT NULL,
  `rpga_dispatch_datetime` datetime NOT NULL,
  `rpga_cancelled` tinyint(1) DEFAULT '0',
  `rpga_finished` tinyint(1) DEFAULT '0',
  `rpga_last_modified_datetime` datetime DEFAULT NULL,
  `rpga_last_modified_user_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`rpga_id`),
  KEY `Id_StartDateTime_Index` (`rpga_id`,`rpga_start_datetime`),
  KEY `Id_ExpireDateTime_Index` (`rpga_id`,`rpga_expire_datetime`),
  KEY `Id_DispatchDateTime_Index` (`rpga_id`,`rpga_dispatch_datetime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='redpacket grab activity ';







