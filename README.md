# Dynamic Configuration

This project is a dynamic configuration library that can pull application settings from database dynamically with a given time interval. 
It aims to avoid adding application settings to files (appSettings, web.config etc.)
You can add Config.Lib (.net standard 2.0) into a project and get your settings by calling;

ConfigManager.Instance.Init(applicationName, connectionStr, refreshTime);

# Installation

Get solution file. You can call library from Config.API or Config.Test (Console application).
Dump20190309.sql must be executed in MySql.

# Tools

 - .NET Reactive Extension is used for getting data from database based on a given refresh time in milliseconds.
 - Dapper
 - MySQL

# Config.Lib Usage

Add Config.Lib reference into your project.
Projectcan be init just once in application by calling;
  
  ConfigManager.Instance.Init("demo", connStr, 20000);
  
If settings from database involved in cache, results are given from cache. If there are no data in cache, there'll be a query to DB.

# Test

Config.Test console project is written for test.
You can initialize dynamic configuration simply adding;

manager.Init("demo", connStr, 20000);

CRUD operations are also available for reading all/single configuration, update or delete configurations using Config.API project.






