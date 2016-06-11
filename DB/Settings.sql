/*
Navicat SQLite Data Transfer

Source Server         : SQLite
Source Server Version : 30808
Source Host           : :0

Target Server Type    : SQLite
Target Server Version : 30808
File Encoding         : 65001

Date: 2016-06-11 13:21:15
*/

PRAGMA foreign_keys = OFF;

-- ----------------------------
-- Table structure for Settings
-- ----------------------------
DROP TABLE IF EXISTS "main"."Settings";
CREATE TABLE "Settings" (
"Id"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"Key"  TEXT NOT NULL,
"Value"  TEXT
);
