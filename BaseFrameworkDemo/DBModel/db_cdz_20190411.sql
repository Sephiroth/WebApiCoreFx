/*
 Navicat Premium Data Transfer

 Source Server         : 192.168.0.84
 Source Server Type    : MySQL
 Source Server Version : 80011
 Source Host           : 192.168.0.84:3306
 Source Schema         : db_cdz

 Target Server Type    : MySQL
 Target Server Version : 80011
 File Encoding         : 65001

 Date: 11/04/2019 10:13:50
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for tb_user
-- ----------------------------
DROP TABLE IF EXISTS `tb_user`;
CREATE TABLE `tb_user`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '用户ID',
  `name` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '用户名称',
  `nickname` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '昵称',
  `phone` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '手机号',
  `wechat_id` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '微信号',
  `discount_balance` int(64) NOT NULL DEFAULT 0 COMMENT '优惠余额(单位：分）',
  `withdrawal_balance` int(64) NOT NULL DEFAULT 0 COMMENT '可提现余额(单位：分）',
  `car_num` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '车牌号',
  `openid` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '授权ID',
  `pwd` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT 'MD5密码',
  `photo` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '头像',
  `email` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '邮箱',
  `gender` int(11) NULL DEFAULT 0 COMMENT '性别（0：未知；1：男性；2：女性）',
  `state` int(11) NOT NULL DEFAULT 0 COMMENT '状态(0:正常；1：删除）',
  `create_time` datetime(0) NOT NULL ON UPDATE CURRENT_TIMESTAMP(0) COMMENT '创建日期',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `phone`(`phone`) USING BTREE COMMENT '电话号码唯一'
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_user
-- ----------------------------
INSERT INTO `tb_user` VALUES ('2a633ebf87684421b1aa75633c611210', NULL, 'ALbert', '15705605827', NULL, 0, 500, NULL, 'othGa5fyOT7vOJG56HBtIoyQTWks', NULL, 'https://wx.qlogo.cn/mmopen/vi_32/DYAIOgq83eo3ZvSnreeOJ5QFAiaicIUCfWPF6olzYib8aRugZbVI5icLMibAGnfU7zhiaia6jiajjaicG81dozoKEQxj0UA/132', NULL, 1, 0, '2019-04-04 14:52:28');
INSERT INTO `tb_user` VALUES ('912e1d94e54e4bb9b4557803ab63e970', NULL, 'S', '18860487110', NULL, 10016, 2994, '2', 'othGa5X7kxaw5Xv5yVAi-mLnTPBM', NULL, 'https://wx.qlogo.cn/mmopen/vi_32/xWibXCDknHNPm0eTY2OS8BxDiaibK69kw37V1tUVoIAybsLjkOUtood2MMJFUWrZGyibI2ib7JbXMvmtsKLicJnLibzGw/132', '啊1', 1, 0, '2019-04-09 16:08:30');
INSERT INTO `tb_user` VALUES ('c32ed27ad67c4d6b8fcb7674cb89ae61', NULL, 'St Liu', '15656099694', NULL, 0, 0, NULL, 'othGa5cxZ9_bWbJXLXU1I1I8hf9g', NULL, 'https://wx.qlogo.cn/mmopen/vi_32/DYAIOgq83eoItRWj1r7eYAfyasm3fZicLeypSj6uoG7VPYxEo42FfBrABsbTu7pUxBluLFSM46ABJmh55xIPjcQ/132', NULL, 1, 0, '2019-04-04 14:49:25');
INSERT INTO `tb_user` VALUES ('sssasd', 'ssssasddssss', 'qwwww', 'sssssa', 'ssddsaw1223', 0, 0, 'sdadasw1223213', 'sd32312', 'sd2w312', NULL, NULL, 0, 1, '0001-01-01 00:00:00');

SET FOREIGN_KEY_CHECKS = 1;
