/******************************************************
 *
 * Name:         create-dynocard-schema.sql
 *     
 * Design Phase:
 *     Author:   John Miner
 *     Date:     04-01-2018
 *     Purpose:  Create the schema for the [db4cards] database.
 * 
 ******************************************************/

--
-- Create the database (run from master)
--

-- Delete existing database


--IF  NOT CAST(ServerProperty('Edition') AS varchar) = 'SQL Azure' print 'sql azure';
USE [master]
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'db4cards')
	DROP DATABASE [db4cards]
GO

DECLARE @CreateStatement VARCHAR(250);

-- Step #1. Create new database
IF CAST(ServerProperty('Edition') AS varchar) = 'SQL Azure'
	SET @CreateStatement = 'CREATE DATABASE [db4cards]	( MAXSIZE = 500 MB, EDITION = ''basic'',	SERVICE_OBJECTIVE = ''basic'')'
ELSE
	SET @CreateStatement = 'CREATE DATABASE [db4cards]'

EXEC (@CreateStatement)
GO



ALTER DATABASE [db4cards] SET COMPATIBILITY_LEVEL = 140
GO

USE [db4cards]
EXEC sp_configure 'contained database authentication', 1
GO
RECONFIGURE
GO

ALTER DATABASE [db4cards]
SET CONTAINMENT = PARTIAL
GO

-- Step #2. Change to [db4cards] database

-- Step #3. Change to [db4cards] database

--
-- Create Contained database user
--

-- Service Account
CREATE USER [dyno_user] WITH PASSWORD = 'GMZNAQ]Q6R6Ljz9>';

-- Give rights
EXEC sp_addrolemember 'db_owner', 'dyno_user'  

--
-- Create ACTIVE schema
--

-- Delete existing schema.
DROP SCHEMA IF EXISTS [ACTIVE]
GO
 
-- Add new schema.
CREATE SCHEMA [ACTIVE] AUTHORIZATION [dbo]
GO



--
-- Create STAGE schema
--

-- Delete existing schema.
DROP SCHEMA IF EXISTS [STAGE]
GO
 
-- Add new schema.
CREATE SCHEMA [STAGE] AUTHORIZATION [dbo]
GO



--
-- Create PUMP table
--
 
-- Delete existing table
DROP TABLE IF EXISTS [ACTIVE].[PUMP]
GO
 
-- Create new table
CREATE TABLE [ACTIVE].[PUMP]
(
  PU_ID INT IDENTITY(1, 1) NOT NULL,
  PU_TAG VARCHAR(32) NOT NULL,
  PU_INSTALLED DATE NOT NULL,
  PU_LONGITUDE REAL NULL,
  PU_LATITUDE REAL NULL,
  PU_DESCRIPTION VARCHAR(128) NULL,
  PU_UPDATE_DATE DATETIME CONSTRAINT [DF_PUMP_UPD_DATE] DEFAULT (getdate()) ,
  PU_UPDATE_BY VARCHAR(128) CONSTRAINT [DF_PUMP_UPD_BY] DEFAULT (coalesce(suser_sname(),'?')),
  CONSTRAINT [PK_PUMP_ID] PRIMARY KEY CLUSTERED (PU_ID ASC)
)
GO

INSERT INTO [ACTIVE].[PUMP]
           ([PU_TAG]
           ,[PU_INSTALLED])
VALUES ('simPump', GetDate())
GO

--
-- Create DYNO CARD table
--

-- Delete existing table
DROP TABLE IF EXISTS [ACTIVE].[DYNO_CARD]
GO
 
-- Create new table
CREATE TABLE [ACTIVE].[DYNO_CARD]
(
  DC_ID INT IDENTITY(1, 1) NOT NULL,
  PU_ID INT NOT NULL,
  DC_UPDATE_DATE DATETIME CONSTRAINT [DF_DYNO_CARD_DATE] DEFAULT (getdate()) ,
  DC_UPDATE_BY VARCHAR(128) CONSTRAINT [DF_DYNO_CARD_BY] DEFAULT (coalesce(suser_sname(),'?')),
  CONSTRAINT [PK_DYNO_CARD_ID] PRIMARY KEY CLUSTERED (DC_ID ASC)
)
GO

-- Add foreign key 
ALTER TABLE [ACTIVE].[DYNO_CARD] WITH CHECK 
  ADD CONSTRAINT [FK_PUMP_ID1] FOREIGN KEY([PU_ID])
  REFERENCES [ACTIVE].[PUMP] ([PU_ID])
GO

--
-- Create EVENT table
--

-- Delete existing table
DROP TABLE IF EXISTS [ACTIVE].[EVENT]
GO
 
-- Create new table
CREATE TABLE [ACTIVE].[EVENT]
(
  EV_ID INT IDENTITY(1, 1) NOT NULL,
  PU_ID INT NOT NULL,
  EV_EPOC_DATE INT NULL,
  EV_UPDATE_DATE DATETIME CONSTRAINT [DF_EVENT_UPD_DATE] DEFAULT (getdate()) ,
  EV_UPDATE_BY VARCHAR(128) CONSTRAINT [DF_EVENT_UPD_BY] DEFAULT (coalesce(suser_sname(),'?')),
  CONSTRAINT [PK_EVENT_ID] PRIMARY KEY CLUSTERED (EV_ID ASC)
)
GO

-- Add foreign key 
ALTER TABLE [ACTIVE].[EVENT] WITH CHECK 
  ADD CONSTRAINT [FK_PUMP_ID2] FOREIGN KEY([PU_ID])
  REFERENCES [ACTIVE].[PUMP] ([PU_ID])
GO

--
-- Create EVENT DETAIL table
--

-- Delete existing table
DROP TABLE IF EXISTS [ACTIVE].[EVENT_DETAIL]
GO
 

CREATE TABLE [ACTIVE].[EVENT_DETAIL]
(
    ED_ID [int] IDENTITY(1, 1) NOT NULL,
    EV_ID [int] NOT NULL,
    DC_ID [int] NOT NULL, 
    ED_TRIGGERED_EVENTS BIT CONSTRAINT [DF_EVENT_DETAIL_TRIGGERED_EVENTS] DEFAULT (0),
    ED_UPDATE_DATE DATETIME CONSTRAINT [DF_EVENT_DETAIL_UPD_DATE] DEFAULT (getdate()) ,
    ED_UPDATE_BY VARCHAR(128) CONSTRAINT [DF_EVENT_DETAIL_UPD_BY] DEFAULT (coalesce(suser_sname(),'?')),
    CONSTRAINT [PK_EVENT_DETAIL_ID] PRIMARY KEY CLUSTERED (ED_ID ASC)
)
GO

-- Add foreign key 
ALTER TABLE [ACTIVE].[EVENT_DETAIL] WITH CHECK 
  ADD CONSTRAINT [FK_EVENT_ID] FOREIGN KEY([EV_ID])
  REFERENCES [ACTIVE].[EVENT] ([EV_ID])
GO

-- Add foreign key 
ALTER TABLE [ACTIVE].[EVENT_DETAIL] WITH CHECK 
  ADD CONSTRAINT [FK_DYNO_CARD_ID2] FOREIGN KEY([DC_ID])
  REFERENCES [ACTIVE].[DYNO_CARD] ([DC_ID])
GO

--
-- Create CARD HEADER table
--

-- Delete existing table
DROP TABLE IF EXISTS [ACTIVE].[CARD_HEADER]
GO
 
-- Create new table
CREATE TABLE [ACTIVE].[CARD_HEADER]
(
  CH_ID INT IDENTITY(1, 1) NOT NULL,
  DC_ID INT NOT NULL,
  CH_EPOC_DATE INT NOT NULL,
  CH_SCALED_MAX_LOAD REAL NULL,
  CH_SHUTDOWN_EVENT_ID INT NULL,
  CH_NUMBER_OF_POINTS INT NULL,
  CH_GROSS_STROKE REAL NULL,
  CH_NET_STROKE REAL NULL,
  CH_PUMP_FILLAGE REAL NULL,
  CH_FLUID_LOAD REAL NULL,
  CH_SCALED_MIN_LOAD REAL NULL,
  CH_STROKE_LENGTH REAL NULL,
  CH_STROKE_PERIOD REAL NULL,
  CH_CARD_TYPE CHAR(1) NULL,
  CH_UPDATE_DATE DATETIME CONSTRAINT [DF_CARD_HDR_UPD_DATE] DEFAULT (getdate()) ,
  CH_UPDATE_BY VARCHAR(128) CONSTRAINT [DF_CARD_HDR_UPD_BY] DEFAULT (coalesce(suser_sname(),'?')),
  CONSTRAINT [PK_CARD_HEADER_ID] PRIMARY KEY CLUSTERED (CH_ID ASC)
)
GO

-- Add foreign key 
ALTER TABLE [ACTIVE].[CARD_HEADER] WITH CHECK 
  ADD CONSTRAINT [FK_DYNO_CARD_ID1] FOREIGN KEY([DC_ID])
  REFERENCES [ACTIVE].[DYNO_CARD] ([DC_ID])
GO

--
-- Create CARD DETAIL table
--

-- Delete existing table
DROP TABLE IF EXISTS [ACTIVE].[CARD_DETAIL]
GO
 
-- Create new table
CREATE TABLE [ACTIVE].[CARD_DETAIL]
(
  CD_ID INT IDENTITY(1, 1) NOT NULL,
  CH_ID INT NOT NULL,
  CD_POSITION REAL NOT NULL,
  CD_LOAD REAL NOT NULL,
  CD_UPDATE_DATE DATETIME CONSTRAINT [DF_CARD_DTL_UPD_DATE] DEFAULT (getdate()) ,
  CD_UPDATE_BY VARCHAR(128) CONSTRAINT [DF_CARD_DTL_UPD_BY] DEFAULT (coalesce(suser_sname(),'?')),
  CONSTRAINT [PK_CARD_DETAIL_ID] PRIMARY KEY CLUSTERED (CD_ID ASC)
);


-- Add foreign key 
ALTER TABLE [ACTIVE].[CARD_DETAIL] WITH CHECK 
  ADD CONSTRAINT [FK_HEADER_ID] FOREIGN KEY([CH_ID])
  REFERENCES [ACTIVE].[CARD_HEADER] ([CH_ID])
GO


--
-- Create SURFACE DATA table
--

-- Delete existing table
-- DROP TABLE IF EXISTS [STAGE].[SURFACE_DATA]
GO
 
-- Create new table
CREATE TABLE [STAGE].[SURFACE_DATA]
(
  SD_ID INT IDENTITY(1, 1) NOT NULL,
  SD_TAG VARCHAR(32) NOT NULL,
  SD_POSITION REAL NOT NULL,
  SD_LOAD REAL NOT NULL,
  CONSTRAINT [PK_SURFACE_DATA_ID] PRIMARY KEY CLUSTERED (SD_ID ASC)
);

--
-- Create PUMP DATA table
--

-- Delete existing table
-- DROP TABLE IF EXISTS [STAGE].[PUMP_DATA]
GO
 
-- Create new table
CREATE TABLE [STAGE].[PUMP_DATA]
(
  PD_ID INT IDENTITY(1, 1) NOT NULL,
  PD_TAG VARCHAR(32) NOT NULL,
  PD_SECS REAL NOT NULL,
  PD_POSITION REAL NOT NULL,
  PD_LOAD REAL NOT NULL,
  CONSTRAINT [PK_PUMP_DATA_ID] PRIMARY KEY CLUSTERED (PD_ID ASC)
);
