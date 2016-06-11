/*
Navicat SQLite Data Transfer

Source Server         : SQLite
Source Server Version : 30808
Source Host           : :0

Target Server Type    : SQLite
Target Server Version : 30808
File Encoding         : 65001

Date: 2016-06-11 12:35:42
*/

PRAGMA foreign_keys = OFF;

-- ----------------------------
-- Table structure for Logins
-- ----------------------------
DROP TABLE IF EXISTS "main"."Logins";
CREATE TABLE "Logins" (
"Id"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"UserName"  TEXT NOT NULL,
"IsAuthorized"  TEXT NOT NULL
);
