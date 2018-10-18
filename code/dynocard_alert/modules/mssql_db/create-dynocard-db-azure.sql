/******************************************************
 *
 * Name:         create-dynocard-db-azure.sql
 *     
 * Design Phase:
 *     Author:   Mike Shir
 *     Date:     04-25-2018
 *     Purpose:  Create the [db4cards] database on Azure SQL
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

PRINT N'Creating Azure SQL Database'
CREATE DATABASE [db4cards]	( MAXSIZE = 500 MB, EDITION = 'basic',	SERVICE_OBJECTIVE = 'basic')
GO