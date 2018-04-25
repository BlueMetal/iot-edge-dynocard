/******************************************************
 *
 * Name:         create-dynocard-db.sql
 *     
 * Design Phase:
 *     Author:   Mike Shir
 *     Date:     04-23-2018
 *     Purpose:  Create the [db4cards] database on SQL Edge
 * 
 ******************************************************/

--
-- Create the edge database (run from master)
--

-- Delete existing database if it exists
USE [master]
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'db4cards')
	DROP DATABASE [db4cards]
GO

PRINT N'Creating Edge Database'
CREATE DATABASE [db4cards]

-- Configure
PRINT N'Configure Contained Database'
EXEC sp_configure 'contained database authentication', 1
RECONFIGURE

ALTER DATABASE [db4cards] SET CONTAINMENT = PARTIAL