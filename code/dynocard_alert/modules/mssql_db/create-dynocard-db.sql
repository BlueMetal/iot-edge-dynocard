/******************************************************
 *
 * Name:         create-dynocard-db.sql
 *     
 * Design Phase:
 *     Author:   Mike Shir
 *     Date:     04-23-2018
 *     Purpose:  Create the [db4cards] database
 * 
 ******************************************************/

--
-- Create the database (run from master)
--

-- Delete existing database if it exists
USE [master]
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'db4cards')
	DROP DATABASE [db4cards]
GO

IF CAST(ServerProperty('Edition') AS varchar) = 'SQL Azure'
	BEGIN
		PRINT N'Creating Azure SQL Database'
		CREATE DATABASE [db4cards]	( MAXSIZE = 500 MB, EDITION = 'basic',	SERVICE_OBJECTIVE = 'basic')
	END
ELSE
	BEGIN
		PRINT N'Creating Edge Database'
		CREATE DATABASE [db4cards]

		-- Configure
		PRINT N'Configure Contained Database'
		EXEC sp_configure 'contained database authentication', 1
		RECONFIGURE

		ALTER DATABASE [db4cards] SET CONTAINMENT = PARTIAL
	END
GO

--DECLARE @CreateStatement VARCHAR(250);

-- Step #1. Create new database
/*
IF CAST(ServerProperty('Edition') AS varchar) = 'SQL Azure'
	SET @CreateStatement = 'CREATE DATABASE [db4cards]	( MAXSIZE = 500 MB, EDITION = ''basic'',	SERVICE_OBJECTIVE = ''basic'')'
ELSE
	SET @CreateStatement = 'CREATE DATABASE [db4cards]'

EXEC (@CreateStatement)
GO
*/

/*
ALTER DATABASE [db4cards] SET COMPATIBILITY_LEVEL = 140
GO
*/