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
-- Table structure for tb_charging_pile
-- ----------------------------
DROP TABLE IF EXISTS `tb_charging_pile`;
CREATE TABLE `tb_charging_pile`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'id',
  `sn` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '充电桩序列号',
  `type` tinyint(4) NOT NULL COMMENT '充电桩类型，0：慢充，1：快充',
  `terminal_type` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '终端类型（交流单相7KW……）',
  `charge_interface` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '充电接口',
  `station_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '充电桩所属充电站',
  `state` int(4) NOT NULL DEFAULT 0 COMMENT '充电桩状态（0：可用；1：在充电；2：离线）',
  `order_type` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '充电桩指令',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `state_id`(`state`) USING BTREE,
  INDEX `station_id`(`station_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_charging_pile
-- ----------------------------
INSERT INTO `tb_charging_pile` VALUES ('11', '11111', 0, NULL, '1', '11112', 0, '开启');
INSERT INTO `tb_charging_pile` VALUES ('12', 'XNG6316177247', 0, NULL, '2', '11113', 0, '开启');
INSERT INTO `tb_charging_pile` VALUES ('13', '3', 1, NULL, '3', '11114', 1, '3');
INSERT INTO `tb_charging_pile` VALUES ('14', '4', 1, NULL, '4', '11112', 0, '4');
INSERT INTO `tb_charging_pile` VALUES ('15', '5', 1, NULL, '5', '11112', 0, '5');

-- ----------------------------
-- Table structure for tb_charging_price
-- ----------------------------
DROP TABLE IF EXISTS `tb_charging_price`;
CREATE TABLE `tb_charging_price`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'id',
  `type` int(4) NOT NULL COMMENT '参数类型',
  `name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '名称',
  `electric_price` float(64, 4) NOT NULL COMMENT '电价',
  `service_price` float(64, 4) NULL DEFAULT NULL COMMENT '服务价格',
  `remark` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_charging_price
-- ----------------------------
INSERT INTO `tb_charging_price` VALUES ('1', 1, '1', 1.0000, 1.0000, '1');

-- ----------------------------
-- Table structure for tb_charging_station
-- ----------------------------
DROP TABLE IF EXISTS `tb_charging_station`;
CREATE TABLE `tb_charging_station`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'id',
  `name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '充电站名',
  `longitude` double(32, 10) NOT NULL COMMENT '充电站经度',
  `latitude` double(32, 10) NOT NULL COMMENT '充电站纬度',
  `address` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '充电站地址',
  `price_type_id` int(64) UNSIGNED NOT NULL COMMENT '当前充电价格类型',
  `price_unit` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '慢充单价',
  `power_unit` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '电量单位',
  `owner` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '管理员id',
  `worktime` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '营业时间',
  `parking_fee` double(10, 0) NOT NULL COMMENT '停车费',
  `photo` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '充电站图片',
  `state` int(4) NOT NULL DEFAULT 0 COMMENT '充电站状态（0：在线；1：离线）',
  `desc` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '站点描述',
  `create_date` datetime(0) NOT NULL COMMENT '创建时间',
  `flag` int(4) NOT NULL DEFAULT 0 COMMENT '标志，默认为0',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_charging_station
-- ----------------------------
INSERT INTO `tb_charging_station` VALUES ('00acce675a97418f8d2759d425034da9', '222222222222222222222222', 117.1115112305, 31.8611971737, '2', 0, '元', 'kwh', '', '2', 2, 'save and record path', 0, '2', '2019-04-01 21:43:19', 0);
INSERT INTO `tb_charging_station` VALUES ('0630cdf8564847b3be7116cdf0a66964', '55', 117.1287202835, 31.8584269881, '555', 0, '元', 'kwh', '', '55', 55, 'save and record path', 0, '555', '2019-04-01 20:48:45', 0);
INSERT INTO `tb_charging_station` VALUES ('092abdfedcd3486b9185edfd257f799f', '添加区域', 117.1040868759, 31.8582568861, '爱对方答复', 0, '元', 'kwh', '', '阿道夫', 22, 'save and record path', 0, '发大水', '2019-04-02 15:12:27', 0);
INSERT INTO `tb_charging_station` VALUES ('0931a4dda6e4458cbd43d8f7a5d486b7', '111', 117.1091938019, 31.8576979781, '11', 0, '元', 'kwh', '', '1', 1, 'save and record path', 0, '1', '2019-04-01 21:18:18', 0);
INSERT INTO `tb_charging_station` VALUES ('11111', '测试充电站', 117.1338380000, 31.8503615720, '合肥市', 1, '1', '1', 'u1', '1', 1, NULL, 1, NULL, '2019-03-21 19:49:51', 0);
INSERT INTO `tb_charging_station` VALUES ('11112', '测试充电站2', 117.1353401000, 31.8470077800, '合肥市2', 1, '1', '1', 'u2', '1', 1, NULL, 1, NULL, '2019-03-21 19:49:51', 0);
INSERT INTO `tb_charging_station` VALUES ('11113', '测试充电站3', 117.1280445016, 31.8450392010, '合肥市3', 1, '1', '1', 'u1', '1', 1, NULL, 1, NULL, '2019-03-21 19:49:51', 0);
INSERT INTO `tb_charging_station` VALUES ('11114', '测试充电站4', 117.2568763600, 31.8566678200, '合肥市3', 1, '1', '1', 'u1', '1', 1, NULL, 1, NULL, '2019-03-21 19:49:51', 0);
INSERT INTO `tb_charging_station` VALUES ('12f7b3c870d046f09a2b4b6ba397f144', '阿什顿发放', 117.1233987808, 31.8614644677, '士大夫', 0, '元', 'kwh', '', '爱上对方', 123, 'save and record path', 0, '士大夫三大发到 ', '2019-04-01 16:45:56', 0);
INSERT INTO `tb_charging_station` VALUES ('14a5781f07704098a321f8b49e69b3ba', '是的发的是', 117.1314668655, 31.8600429396, '大', 0, '元', 'kwh', '', '大 ', 152, 'save and record path', 0, '士大夫爱上对方', '2019-04-01 16:44:37', 0);
INSERT INTO `tb_charging_station` VALUES ('283e855231a243fa86f635192cfc7bf5', '1111', 117.1166181564, 31.8591924424, '11111', 0, '元', 'kwh', '', '111', 111, 'save and record path', 0, '11', '2019-04-02 19:53:10', 0);
INSERT INTO `tb_charging_station` VALUES ('3Sc2xHEm97eh7ofiBaDWMBV4kcyLq9Uh', '测试用（勿删）', 120.0000000000, 32.0000000000, '1', 1, '1', '1', 'wwd', '1', 1, NULL, 1, NULL, '2019-03-27 15:10:50', 0);
INSERT INTO `tb_charging_station` VALUES ('44f80145a1494d2e8f21bcd2cd4ba0fd', '11', 117.1370458603, 31.8614887673, '11', 0, '元', 'kwh', '', '1', 1, 'save and record path', 0, '1', '2019-04-01 21:43:10', 0);
INSERT INTO `tb_charging_station` VALUES ('4c57fc086f434c6ca453d18d5d3b744f', '444', 117.1250295639, 31.8584634385, '444', 0, '元', 'kwh', '', '444', 44, 'save and record path', 0, '4444', '2019-04-01 20:48:37', 0);
INSERT INTO `tb_charging_station` VALUES ('59d7c79204ab452c87f370446e7145b0', '33', 117.1366381645, 31.8548119718, '3', 0, '元', 'kwh', '', '3', 3, 'save and record path', 0, '3', '2019-04-01 16:48:22', 0);
INSERT INTO `tb_charging_station` VALUES ('5fe3e4b176cb449a9b7ed7478c78124b', '77', 117.1291065216, 31.8556688696, '7', 0, '元', 'kwh', '', '7', 7, 'save and record path', 0, '7', '2019-04-01 17:08:25', 0);
INSERT INTO `tb_charging_station` VALUES ('64fa35e8f52c4c5291d5209ce29a5b4c', '888', 117.1212959290, 31.8598485411, '88', 0, '元', 'kwh', '', '88', 88, 'save and record path', 0, '88', '2019-04-01 20:55:47', 0);
INSERT INTO `tb_charging_station` VALUES ('6a6e5002350e4ff7982b77387ae1134c', '444', 117.1265316010, 31.8664457180, '44', 0, '元', 'kwh', '', '44', 44, 'save and record path', 0, '44', '2019-04-02 21:28:28', 0);
INSERT INTO `tb_charging_station` VALUES ('81d3c4d7c9e24feba3751bdd200bcac9', '66', 117.1363162994, 31.8372750019, '66', 0, '元', 'kwh', '', '6666', 6, 'save and record path', 0, '25555', '2019-04-01 16:52:03', 0);
INSERT INTO `tb_charging_station` VALUES ('82f3b403b0c74c29bb6b6b42a763d1bd', '88', 117.1265316010, 31.8607233320, '8', 0, '元', 'kwh', '', '8', 88, 'save and record path', 0, '8', '2019-04-01 20:39:45', 0);
INSERT INTO `tb_charging_station` VALUES ('85db0def928d4e529b002a29dd5ba4d6', '999', 117.1312093735, 31.8625822352, '999', 0, '元', 'kwh', '', '99', 99, 'save and record path', 0, '99999', '2019-04-01 21:10:45', 0);
INSERT INTO `tb_charging_station` VALUES ('a5632699cac246feaa2d026577608cac', '000', 117.0952463150, 31.8609017821, '000', 0, '元', 'kwh', '', '00', 11, 'save and record path', 0, '110000', '2019-04-04 16:32:58', 0);
INSERT INTO `tb_charging_station` VALUES ('a8bcb59cebe44d05977f7babbbd35e04', '9', 117.1199226379, 31.8563492943, '9', 0, '元', 'kwh', '', '9', 99, 'save and record path', 0, '999', '2019-04-01 20:42:25', 0);
INSERT INTO `tb_charging_station` VALUES ('ababdc72540547eabe63b3f65445a818', '11', 117.1111249924, 31.8585727894, '11', 0, '元', 'kwh', '', '11', 11, 'save and record path', 0, '11', '2019-04-01 21:41:22', 0);
INSERT INTO `tb_charging_station` VALUES ('c6f93ec95a724a1e8730ed41c7022021', '11', 117.1155881882, 31.8586456900, '11', 0, '元', 'kwh', '', '11', 11, 'save and record path', 0, '11', '2019-04-01 21:13:57', 0);
INSERT INTO `tb_charging_station` VALUES ('d14031cb19ce4f73b117e3bf79c08343', '111', 117.1388053894, 31.8309091281, '11', 0, '元', 'kwh', '', '11', 11, 'save and record path', 0, '11', '2019-04-02 19:56:23', 0);
INSERT INTO `tb_charging_station` VALUES ('d71b1dda05154a8ca4ddf8aacbb7ff9c', '33', 117.1145582199, 31.8601765892, '3', 0, '元', 'kwh', '', '3', 3, 'save and record path', 0, '3', '2019-04-01 21:43:42', 0);
INSERT INTO `tb_charging_station` VALUES ('dc93bd7692e54e5783e5a611655aaa32', '111', 117.1554458141, 31.8354393252, '111', 0, '元', 'kwh', '', '111', 11, 'save and record path', 0, '11', '2019-04-02 19:56:09', 0);
INSERT INTO `tb_charging_station` VALUES ('ebb688c193264a47bbbf963ae36d102c', '222', 117.1336984634, 31.8586092403, '222', 0, '元', 'kwh', '', '222', 222, 'save and record path', 0, '222', '2019-04-01 21:53:37', 0);
INSERT INTO `tb_charging_station` VALUES ('f3339a003e2b4c66bd1d8178b86f8cd7', '22222222222222', 117.1185493469, 31.8632383103, '2', 0, '元', 'kwh', '', '2', 2, 'save and record path', 0, '2', '2019-04-01 21:44:21', 0);
INSERT INTO `tb_charging_station` VALUES ('f4233cf47eff40xxxxxxx25', 'sfsdafdf ', 7.1444400000, 31.8573310000, 'fafdsf ', 13, '', '', '', 'df ', 1255, 'save and record path', 0, 'adsfdsfasfdagdsf方法第三方发文飞飞啊是非得失', '2019-04-01 15:46:54', 0);
INSERT INTO `tb_charging_station` VALUES ('f445cf5ec49249d9aa01c4b5c9caf0b0', '5', 117.1235275269, 31.8349621230, '5', 0, '元', 'kwh', '', '5', 5, 'save and record path', 0, '5', '2019-04-01 16:48:43', 0);
INSERT INTO `tb_charging_station` VALUES ('f7e0d3ac79044ef9971d3abc985e7b31', '111', 117.1163606644, 31.8630196188, '111', 0, '元', 'kwh', '', '11', 11, 'save and record path', 0, '11', '2019-04-01 21:12:54', 0);
INSERT INTO `tb_charging_station` VALUES ('f804ec7676844512905ca0df46c2bfaf', '0000', 117.0982933044, 31.8653887435, '000', 0, '元', 'kwh', '', '00', 0, 'save and record path', 0, '00', '2019-04-02 22:35:43', 0);
INSERT INTO `tb_charging_station` VALUES ('fa953d6d96e540a492815b86a3bb5986', '999', 117.1293210983, 31.8595933922, '999', 0, '元', 'kwh', '', '99', 99, 'save and record path', 0, '999', '2019-04-01 20:46:54', 0);

-- ----------------------------
-- Table structure for tb_collection
-- ----------------------------
DROP TABLE IF EXISTS `tb_collection`;
CREATE TABLE `tb_collection`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'id',
  `logo` blob NULL COMMENT '收藏点图片标识',
  `station_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '收藏充电站ID',
  `user_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '用户ID',
  `create_date` datetime(0) NOT NULL ON UPDATE CURRENT_TIMESTAMP(0) COMMENT '创建时间',
  `remark` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `station`(`station_id`) USING BTREE,
  INDEX `user2`(`user_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_collection
-- ----------------------------
INSERT INTO `tb_collection` VALUES ('e2a716b06cbd470cb01006e988671821', NULL, '11114', '912e1d94e54e4bb9b4557803ab63e970', '2019-04-10 08:43:25', NULL);

-- ----------------------------
-- Table structure for tb_evaluation
-- ----------------------------
DROP TABLE IF EXISTS `tb_evaluation`;
CREATE TABLE `tb_evaluation`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'id',
  `user_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '用户id',
  `order_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '订单id',
  `station_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '充电站id',
  `evaluate_lever` int(4) NOT NULL COMMENT '评价星级',
  `evaluate_content` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '评价内容',
  `create_time` datetime(0) NOT NULL ON UPDATE CURRENT_TIMESTAMP(0) COMMENT '创建时间',
  `remark` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `usereva`(`user_id`) USING BTREE,
  INDEX `stationeva`(`station_id`) USING BTREE,
  INDEX `ordereva`(`order_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_evaluation
-- ----------------------------
INSERT INTO `tb_evaluation` VALUES ('1', '5865a365bc544908b4ec14c9c067c32d', '1', '11114', 4, '非常好', '2019-03-28 11:05:44', '');

-- ----------------------------
-- Table structure for tb_feedback
-- ----------------------------
DROP TABLE IF EXISTS `tb_feedback`;
CREATE TABLE `tb_feedback`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'id',
  `title` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '反馈标题',
  `text` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '反馈内容',
  `url` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '图片',
  `user_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '反馈人',
  `create_time` datetime(0) NOT NULL COMMENT '反馈时间',
  `opinion` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '处理意见',
  `opinion_state` tinyint(4) NULL DEFAULT NULL COMMENT '处理状态',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tb_invoice
-- ----------------------------
DROP TABLE IF EXISTS `tb_invoice`;
CREATE TABLE `tb_invoice`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'id',
  `user_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '用户id',
  `invoice_type` int(11) NOT NULL COMMENT '发票类型（0：电子发票；1：纸质发票）',
  `create_time` datetime(0) NOT NULL ON UPDATE CURRENT_TIMESTAMP(0) COMMENT '创建时间',
  `title_type` tinyint(2) NOT NULL COMMENT '抬头类型（0：企业单位；1：个人）',
  `title` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '发票抬头',
  `tax_num` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '税号(个人无税号)',
  `invoice_num` int(64) NULL DEFAULT NULL COMMENT '发票金额（单位：分）',
  `invoice_content` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '发票内容',
  `receiver_name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '收件人',
  `receiver_phone` int(64) NULL DEFAULT NULL COMMENT '收件人电话',
  `receiver_mail` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '电子邮箱',
  `state` int(4) NOT NULL DEFAULT 1 COMMENT '订单状态（0：未完成；1：已完成；2：删除）',
  `remarks` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `inuser`(`user_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tb_oplist
-- ----------------------------
DROP TABLE IF EXISTS `tb_oplist`;
CREATE TABLE `tb_oplist`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `op_code` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `op_name` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `flag` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tb_order
-- ----------------------------
DROP TABLE IF EXISTS `tb_order`;
CREATE TABLE `tb_order`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'id',
  `order_no` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '订单流水号',
  `station_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '充电站ID',
  `pile_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '充电桩ID',
  `user_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '用户',
  `create_time` datetime(0) NOT NULL COMMENT '创建时间',
  `withdrawal_monetary` int(64) NOT NULL COMMENT '可提现账号消费总金额（单位：分）',
  `discount_monetary` int(64) NOT NULL DEFAULT 0 COMMENT '优惠账号消费总金额（单位：分）',
  `power` double(64, 4) NULL DEFAULT NULL COMMENT '消费总电量',
  `actual_service_charge` int(255) NULL DEFAULT 0 COMMENT '充电桩传入的实际服务费金额(单位：分)',
  `actual_electric_charge` int(255) NULL DEFAULT 0 COMMENT '充电桩账单的实际电费金额(单位：分)',
  `duration` double(8, 4) NULL DEFAULT NULL COMMENT '使用时长/分',
  `datail` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '使用详情（用于统计每个时间段的使用情况）',
  `state` int(11) NOT NULL DEFAULT 0 COMMENT '订单状态（0：进行中；1：可结算；2：已结算；4：异常订单。默认0）',
  `invoice_state` int(4) NOT NULL DEFAULT 0 COMMENT '发票状态（0：未开发票;1:已开发票）',
  `remark` varchar(225) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `orstation`(`station_id`) USING BTREE,
  INDEX `orpile`(`pile_id`) USING BTREE,
  INDEX `oruser`(`user_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_order
-- ----------------------------
INSERT INTO `tb_order` VALUES ('1', '1', '11111', '11', '1eb9f15109d83dc397b05cfe8a8bba47', '2019-04-08 16:23:04', 1, 1, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL);
INSERT INTO `tb_order` VALUES ('2', '2017031512322400000001', '11112', '12', '1eb9f15109d83dc397b05cfe8a8bba47', '2019-04-10 11:05:30', 1, 0, 0.0000, 0, 0, 1.0000, '1', 2, 0, NULL);
INSERT INTO `tb_order` VALUES ('3', '222', '11113', '13', '1eb9f15109d83dc397b05cfe8a8bba47', '2019-04-10 15:16:31', 1, 1, 1.0000, 1, 0, NULL, NULL, 1, 0, NULL);

-- ----------------------------
-- Table structure for tb_org
-- ----------------------------
DROP TABLE IF EXISTS `tb_org`;
CREATE TABLE `tb_org`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `org_name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `parent_id` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `org_desc` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `flag` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `state` int(4) NOT NULL COMMENT 'int型，0组织，1站点',
  `create_time` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_org
-- ----------------------------
INSERT INTO `tb_org` VALUES ('0811b6966a4e43d6bc133c5e29ef14ae', 'tsfdsfaew32q2', '215ea2b1fb5f44b6a4dd5ac7231d7192', '发的方法爱上对方', '0', 1, NULL);
INSERT INTO `tb_org` VALUES ('1ce92dceb250437bb4eb3479d8a209fe', 'tsfdsfaew32q2撒发的身份', '215ea2b1fb5f44b6a4dd5ac7231d7192', '发的方法爱上对方算法是打发发到深V', '0', 1, NULL);
INSERT INTO `tb_org` VALUES ('215ea2b1fb5f44b6a4dd5ac7231d7192', '大事发生地方', NULL, '发生地方', '0', 0, NULL);
INSERT INTO `tb_org` VALUES ('240478f5a3fb43d086c634402f9fa587', '44444', NULL, '444', NULL, 0, NULL);
INSERT INTO `tb_org` VALUES ('3Sc2xHEm97eh7ofiBaDWMBV4kcyLq9U4', '测试站点（勿删2）', '215ea2b1fb5f44b6a4dd5ac7231d7192', '21', '0', 1, NULL);
INSERT INTO `tb_org` VALUES ('3Sc2xHEm97eh7ofiBaDWMBV4kcyLq9Uh', '测试站点（勿删）', 'IL1GNVM9EokEd8v1yFsh5RAqVSjkWsUO', NULL, '0', 1, NULL);
INSERT INTO `tb_org` VALUES ('437f2870816b4933baf604d2a2789b3f', '新区域1', NULL, 'MIAHSHU', NULL, 0, NULL);
INSERT INTO `tb_org` VALUES ('99db001dd74a46c184c992ecac2f7a2d', '11', NULL, '11', '0', 0, '2019-04-02 22:34:51');
INSERT INTO `tb_org` VALUES ('a5632699cac246feaa2d026577608cac', '000', NULL, '110000', '0', 1, '2019-04-04 16:32:58');
INSERT INTO `tb_org` VALUES ('b207599ae3254e8594e80aea2f911ee8', '12', NULL, '12', '0', 0, '2019-04-02 22:32:59');
INSERT INTO `tb_org` VALUES ('e33c1d91a9c34c0a8461ce7b39c5fc97', '1', NULL, '1', '0', 0, '2019-04-02 22:30:40');
INSERT INTO `tb_org` VALUES ('f804ec7676844512905ca0df46c2bfaf', '0000', NULL, '00', '0', 1, '2019-04-02 22:35:44');
INSERT INTO `tb_org` VALUES ('IL1GNVM9EokEd8v1yFsh5RAqVSjkWsU1', '测试代理区域（勿删）1', NULL, NULL, '0', 0, NULL);
INSERT INTO `tb_org` VALUES ('IL1GNVM9EokEd8v1yFsh5RAqVSjkWsUO', '测试代理区域（勿删）', NULL, NULL, '0', 0, NULL);
INSERT INTO `tb_org` VALUES ('站点id或组织id', '组织名或站点名', '站点的组织id', '描述', '1', 0, NULL);

-- ----------------------------
-- Table structure for tb_pile_bill
-- ----------------------------
DROP TABLE IF EXISTS `tb_pile_bill`;
CREATE TABLE `tb_pile_bill`  (
  `id` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '以账单流水号作为id',
  `user_card` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '用户卡号，以用户手机号作为卡号',
  `power` int(64) NULL DEFAULT NULL COMMENT '充电电量',
  `electric_charge` int(64) NULL DEFAULT NULL COMMENT '电费/分',
  `service_charge` int(64) NULL DEFAULT NULL COMMENT '服务费/分',
  `reason` int(4) NULL DEFAULT NULL COMMENT '停止充电原因',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_pile_bill
-- ----------------------------
INSERT INTO `tb_pile_bill` VALUES ('2017031512322400000001', '1234567890000001', 0, 0, 0, 6);

-- ----------------------------
-- Table structure for tb_pile_session
-- ----------------------------
DROP TABLE IF EXISTS `tb_pile_session`;
CREATE TABLE `tb_pile_session`  (
  `id` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'id',
  `code` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '桩编码ASCII',
  `info` int(4) NULL DEFAULT NULL COMMENT '枪的编码！1234',
  `sessionid` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT 'sessionid',
  `power` int(255) NULL DEFAULT 0 COMMENT '总充电量',
  `pilestate` int(5) NULL DEFAULT 0 COMMENT '桩状态',
  `gunstate` int(5) NULL DEFAULT 0 COMMENT '充电枪状态',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_pile_session
-- ----------------------------
INSERT INTO `tb_pile_session` VALUES ('2ebd8558bbfd4e1faed4ace646398513', '1234567890000001', 0, '2c4c2f04-e9a6-4865-a4c3-8bca33da0f0f', 0, 0, 0);

-- ----------------------------
-- Table structure for tb_price_model
-- ----------------------------
DROP TABLE IF EXISTS `tb_price_model`;
CREATE TABLE `tb_price_model`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '计费类型ID',
  `name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '计费类型名称',
  `electric_price` int(64) NOT NULL COMMENT '电价（单位：分）',
  `service_price` int(64) NOT NULL DEFAULT 0 COMMENT '服务价格（单位：分）',
  `remark` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  `model_sign` int(1) NULL DEFAULT NULL COMMENT '模型代码 1234',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_price_model
-- ----------------------------
INSERT INTO `tb_price_model` VALUES ('2a633ebf87684421b1aa75633c611210', '尖时段', 60, 60, '1', 1);
INSERT INTO `tb_price_model` VALUES ('2a633ebf87684421b1aa75633c611220', '峰时段', 50, 60, '2', 2);
INSERT INTO `tb_price_model` VALUES ('2a633ebf87684421b1aa75633c611230', '平时段', 45, 50, '3', 3);
INSERT INTO `tb_price_model` VALUES ('2a633ebf87684421b1aa75633c614230', '股时段', 40, 40, '4', 4);

-- ----------------------------
-- Table structure for tb_recharge
-- ----------------------------
DROP TABLE IF EXISTS `tb_recharge`;
CREATE TABLE `tb_recharge`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'id',
  `user_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '充值账号',
  `amount` int(64) NOT NULL COMMENT '充值金额（单位：分）',
  `create_time` datetime(0) NOT NULL COMMENT '充值时间',
  `method` tinyint(4) NOT NULL DEFAULT 0 COMMENT '0:现金充值；1：优惠券充值',
  `remark` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  `state` int(11) NOT NULL DEFAULT 1 COMMENT '状态（0：未完成；1：已完成）',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `reuser`(`user_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_recharge
-- ----------------------------
INSERT INTO `tb_recharge` VALUES ('2ed45513e41445fc9b83a7d750930224', '2a633ebf87684421b1aa75633c611210', 500, '2019-04-04 14:52:22', 0, NULL, 1);

-- ----------------------------
-- Table structure for tb_refund
-- ----------------------------
DROP TABLE IF EXISTS `tb_refund`;
CREATE TABLE `tb_refund`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'id',
  `create_time` datetime(0) NOT NULL COMMENT '退款时间',
  `order_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '订单号',
  `refund_amount` int(64) NOT NULL COMMENT '退款金额（单位：分）',
  `user_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '提现账户',
  `reason` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '退款原因',
  `state` int(11) NOT NULL DEFAULT 0 COMMENT '状态（0：未完成，仅提交申请；1：已完成）2 已取消',
  `photo` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '照片（存放url）',
  `remarks` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `refunduser`(`user_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_refund
-- ----------------------------
INSERT INTO `tb_refund` VALUES ('1', '2019-04-17 11:07:52', '1212', 12, '12', '112', 0, NULL, '222');
INSERT INTO `tb_refund` VALUES ('2', '2019-04-01 11:08:20', '2', 222, '22', '222', 1, NULL, '22');

-- ----------------------------
-- Table structure for tb_repair
-- ----------------------------
DROP TABLE IF EXISTS `tb_repair`;
CREATE TABLE `tb_repair`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0' COMMENT 'id',
  `station_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '充电站',
  `sn` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '充电桩序列号',
  `repair_type_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '报修原因',
  `datail` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '报修详情',
  `create_date` datetime(0) NOT NULL COMMENT '申报日期',
  `user_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '申报人id',
  `result` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '报修结果',
  `state` int(4) NOT NULL DEFAULT 0 COMMENT '处理状态（0：未处理；1：已处理）',
  `remark` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `repstation`(`station_id`) USING BTREE,
  INDEX `repuser`(`user_id`) USING BTREE,
  INDEX `reptype`(`repair_type_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_repair
-- ----------------------------
INSERT INTO `tb_repair` VALUES ('27281c02671c4d6cacca90cce7da37c2', NULL, '436161', '1', NULL, '0001-01-01 00:00:00', '912e1d94e54e4bb9b4557803ab63e970', NULL, 0, NULL);
INSERT INTO `tb_repair` VALUES ('2f9a71a849ba4d9e9d6b3939b0e96ffb', NULL, '43616171', '1', NULL, '0001-01-01 00:00:00', '912e1d94e54e4bb9b4557803ab63e970', NULL, 0, NULL);
INSERT INTO `tb_repair` VALUES ('3b5bd884a9ab4e37be7e5b2d305d901b', NULL, 'XNG6316177247', '1', NULL, '0001-01-01 00:00:00', '912e1d94e54e4bb9b4557803ab63e970', NULL, 0, NULL);
INSERT INTO `tb_repair` VALUES ('5318d1a7c70f4918895701081de49431', NULL, '316171', '7', NULL, '0001-01-01 00:00:00', '912e1d94e54e4bb9b4557803ab63e970', NULL, 0, NULL);
INSERT INTO `tb_repair` VALUES ('815f219a9d494ef9bb0ea0a94bbe1f9b', NULL, '0', '1', NULL, '0001-01-01 00:00:00', '912e1d94e54e4bb9b4557803ab63e970', NULL, 0, NULL);
INSERT INTO `tb_repair` VALUES ('84204417642640158e0683290c1944b3', NULL, '0', '1', NULL, '0001-01-01 00:00:00', '912e1d94e54e4bb9b4557803ab63e970', NULL, 0, NULL);
INSERT INTO `tb_repair` VALUES ('89e5f65531c94f03ba76aa2e98b62b31', NULL, 'XNG6316177247', '1', NULL, '0001-01-01 00:00:00', '912e1d94e54e4bb9b4557803ab63e970', NULL, 0, NULL);
INSERT INTO `tb_repair` VALUES ('8b38ec4de5854c36b70606ac9573a51b', NULL, '4616136161', '1', NULL, '0001-01-01 00:00:00', '912e1d94e54e4bb9b4557803ab63e970', NULL, 0, NULL);
INSERT INTO `tb_repair` VALUES ('aff44d03dd5444c7b602a85d515dbacd', NULL, '11', '1', NULL, '0001-01-01 00:00:00', '912e1d94e54e4bb9b4557803ab63e970', NULL, 0, NULL);
INSERT INTO `tb_repair` VALUES ('bf3ec85011bb474082d831693daff824', NULL, '4613617', '4', NULL, '0001-01-01 00:00:00', '912e1d94e54e4bb9b4557803ab63e970', NULL, 0, NULL);
INSERT INTO `tb_repair` VALUES ('de2d531552b647f1bfd45ec0fae43641', NULL, 'XNG6316177247', '1', NULL, '0001-01-01 00:00:00', '912e1d94e54e4bb9b4557803ab63e970', NULL, 0, NULL);
INSERT INTO `tb_repair` VALUES ('e9b1e2a64c454abca1f9b27bf309123a', NULL, '0', '1', NULL, '0001-01-01 00:00:00', '912e1d94e54e4bb9b4557803ab63e970', NULL, 0, NULL);
INSERT INTO `tb_repair` VALUES ('f8539f17a8c24fcfb149c67a3a4cfd4f', NULL, '0', '1', NULL, '0001-01-01 00:00:00', '912e1d94e54e4bb9b4557803ab63e970', NULL, 0, NULL);
INSERT INTO `tb_repair` VALUES ('fa30f749453841c48840968a72609ae1', NULL, '3516', '1', NULL, '0001-01-01 00:00:00', '912e1d94e54e4bb9b4557803ab63e970', NULL, 0, NULL);

-- ----------------------------
-- Table structure for tb_repair_type
-- ----------------------------
DROP TABLE IF EXISTS `tb_repair_type`;
CREATE TABLE `tb_repair_type`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'id',
  `name` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '保修原因',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_repair_type
-- ----------------------------
INSERT INTO `tb_repair_type` VALUES ('1', '二维码');
INSERT INTO `tb_repair_type` VALUES ('2', '充电枪');
INSERT INTO `tb_repair_type` VALUES ('4', '收费异常');
INSERT INTO `tb_repair_type` VALUES ('5', '桩体受损');
INSERT INTO `tb_repair_type` VALUES ('6', '充电枪故障');
INSERT INTO `tb_repair_type` VALUES ('7', '充电桩显示屏');

-- ----------------------------
-- Table structure for tb_role
-- ----------------------------
DROP TABLE IF EXISTS `tb_role`;
CREATE TABLE `tb_role`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `role_name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `role_desc` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `role_type` int(11) NULL DEFAULT NULL,
  `createdate` datetime(0) NOT NULL ON UPDATE CURRENT_TIMESTAMP(0),
  `flag` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_role
-- ----------------------------
INSERT INTO `tb_role` VALUES ('33bca7898b54332f93faf42fddfdc97a', 'stationAdmin', '站点管理员', NULL, '2019-03-25 15:13:19', 0);
INSERT INTO `tb_role` VALUES ('4ec2530f0dff394b9eba908b0b4bccfd', 'superAdmin', '超级管理员', NULL, '2019-03-25 15:13:24', 0);
INSERT INTO `tb_role` VALUES ('e6310bcde934335d94e516d362694385', 'agentAdmin', '代理商', NULL, '2019-03-25 15:13:28', 0);

-- ----------------------------
-- Table structure for tb_role_op
-- ----------------------------
DROP TABLE IF EXISTS `tb_role_op`;
CREATE TABLE `tb_role_op`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `op_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `op_code` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `role_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `flag` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tb_synusers
-- ----------------------------
DROP TABLE IF EXISTS `tb_synusers`;
CREATE TABLE `tb_synusers`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `login_name` varchar(120) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `password` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `role_name` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `createdate` datetime(0) NOT NULL ON UPDATE CURRENT_TIMESTAMP(0),
  `flag` int(11) NOT NULL DEFAULT 0,
  `nature_name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `phone` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `gender` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_synusers
-- ----------------------------
INSERT INTO `tb_synusers` VALUES ('1eb9f15109d83dc397b05cfe8a8bba47', 'admin', '123456', 'superAdmin', '2019-03-28 17:27:07', 0, '超级管理员', '13345678909', 0);
INSERT INTO `tb_synusers` VALUES ('2f0919030c5e47d1ab603489da86c5b2', 'wwww', '1', 'agentAdmin', '2019-03-29 09:13:37', 0, '1', '1', 0);
INSERT INTO `tb_synusers` VALUES ('5a28622eafc84c64b6f85506c138e080', 'w', '1', 'agentAdmin', '2019-04-02 20:54:05', 0, '', '1', 1);
INSERT INTO `tb_synusers` VALUES ('797f066b7636458db0318a4f7b747216', '  ', '  ', 'stationAdmin', '2019-04-04 16:29:37', 1, '  ', '  ', 3);
INSERT INTO `tb_synusers` VALUES ('801c1d44402746f18b74322a893636d5', 'zd', '1', 'stationAdmin', '2019-04-04 14:10:07', 0, 'zd', '1122223', 1);
INSERT INTO `tb_synusers` VALUES ('b72ee09122ff4117939a530440e99de4', '1', '2', 'stationAdmin', '2019-03-29 10:03:12', 1, '2', '2', 3);
INSERT INTO `tb_synusers` VALUES ('c3987bb1e0954e33b57f28aa813bfe6b', '  ', '  ', 'agentAdmin', '2019-04-04 16:28:55', 0, '  ', '  ', 3);
INSERT INTO `tb_synusers` VALUES ('e55a3680a52649f2b0211f4ceb107208', 'wwd', '1', 'agentAdmin', '2019-03-29 09:04:53', 1, '', '1', 1);
INSERT INTO `tb_synusers` VALUES ('ed6723dd4fc1434a8fbfabcd7d188aa4', 'www', '123', 'stationAdmin', '2019-04-02 15:21:27', 0, '12', '2', 1);
INSERT INTO `tb_synusers` VALUES ('u1', 'u1', 'u1', 'agentAdmin', '2019-03-28 17:01:01', 0, 'uu1', '13345678909', 1);
INSERT INTO `tb_synusers` VALUES ('u2', 'u2', 'u2', 'stationAdmin', '2019-03-28 17:01:04', 0, 'uu2', '13345678909', 2);

-- ----------------------------
-- Table structure for tb_time_model
-- ----------------------------
DROP TABLE IF EXISTS `tb_time_model`;
CREATE TABLE `tb_time_model`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'ID',
  `price_model_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '价格模型ID（尖、峰、平、谷）',
  `duration` int(64) NULL DEFAULT NULL COMMENT '持续时间（单位分，最大1440）',
  `model_sign` int(4) NULL DEFAULT NULL COMMENT '模型代码1234',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_time_model
-- ----------------------------
INSERT INTO `tb_time_model` VALUES ('912e1d94e54e4bb9b4557803ab63e970', '2a633ebf87684421b1aa75633c611230', 1440, 3);

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

-- ----------------------------
-- Table structure for tb_user_org
-- ----------------------------
DROP TABLE IF EXISTS `tb_user_org`;
CREATE TABLE `tb_user_org`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `user_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `org_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `flag` int(11) NULL DEFAULT 0,
  `org_name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_user_org
-- ----------------------------
INSERT INTO `tb_user_org` VALUES ('04e121ae51ad4926aafa6e7731b92cbf', '5a28622eafc84c64b6f85506c138e080', 'IL1GNVM9EokEd8v1yFsh5RAqVSjkWsUO', NULL, '测试代理区域（勿删）');
INSERT INTO `tb_user_org` VALUES ('0fd5c01dd611422f8f989c0dfc1a5d89', 'b72ee09122ff4117939a530440e99de4', '3Sc2xHEm97eh7ofiBaDWMBV4kcyLq9U4', NULL, '测试站点（勿删2）');
INSERT INTO `tb_user_org` VALUES ('1', 'u1', 'IL1GNVM9EokEd8v1yFsh5RAqVSjkWsUO', 0, '测试代理区域（勿删）');
INSERT INTO `tb_user_org` VALUES ('2', 'u1', 'IL1GNVM9EokEd8v1yFsh5RAqVSjkWsU1', 0, '测试代理区域（勿删）1');
INSERT INTO `tb_user_org` VALUES ('237a55108d1f484ea78a8827550c7270', '5a28622eafc84c64b6f85506c138e080', '215ea2b1fb5f44b6a4dd5ac7231d7192', NULL, '大事发生地方');
INSERT INTO `tb_user_org` VALUES ('4570ebdef4fc4b309202bc323cdfddad', '801c1d44402746f18b74322a893636d5', '3Sc2xHEm97eh7ofiBaDWMBV4kcyLq9Uh', NULL, '测试站点（勿删）');
INSERT INTO `tb_user_org` VALUES ('509f776733634ce0a3b8b8f926ff0c7f', '5a28622eafc84c64b6f85506c138e080', '1ce92dceb250437bb4eb3479d8a209fe', NULL, 'tsfdsfaew32q2撒发的身份');
INSERT INTO `tb_user_org` VALUES ('53e1ef8e0d3a4502b802231bb37c1fdc', 'u2', '3Sc2xHEm97eh7ofiBaDWMBV4kcyLq9U4', NULL, '测试站点（勿删2）');
INSERT INTO `tb_user_org` VALUES ('681784f983164aa482748fe894ca1bb1', 'u2', '3Sc2xHEm97eh7ofiBaDWMBV4kcyLq9Uh', NULL, '测试站点（勿删）');
INSERT INTO `tb_user_org` VALUES ('69b42a8e575b45899eda3cdee2eb626a', '2f0919030c5e47d1ab603489da86c5b2', 'IL1GNVM9EokEd8v1yFsh5RAqVSjkWsUO', NULL, '测试代理区域（勿删）');
INSERT INTO `tb_user_org` VALUES ('6e1851ce318347b39a79b3c14926cfeb', 'ed6723dd4fc1434a8fbfabcd7d188aa4', '3Sc2xHEm97eh7ofiBaDWMBV4kcyLq9U4', NULL, '测试站点（勿删2）');
INSERT INTO `tb_user_org` VALUES ('cfffb36bed984f489534eec3acbbf4b4', 'ed6723dd4fc1434a8fbfabcd7d188aa4', '3Sc2xHEm97eh7ofiBaDWMBV4kcyLq9Uh', NULL, '测试站点（勿删）');
INSERT INTO `tb_user_org` VALUES ('d7c14b8512b94b1e84b705dfa97d9810', 'e55a3680a52649f2b0211f4ceb107208', 'IL1GNVM9EokEd8v1yFsh5RAqVSjkWsU1', NULL, '测试代理区域（勿删）1');
INSERT INTO `tb_user_org` VALUES ('eb1d4e951e8e49bea141a9141d05cf54', '5a28622eafc84c64b6f85506c138e080', 'IL1GNVM9EokEd8v1yFsh5RAqVSjkWsU1', NULL, '测试代理区域（勿删）1');

-- ----------------------------
-- Table structure for tb_user_role
-- ----------------------------
DROP TABLE IF EXISTS `tb_user_role`;
CREATE TABLE `tb_user_role`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `user_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `role_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `flag` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_user_role
-- ----------------------------
INSERT INTO `tb_user_role` VALUES ('1f09ed7334c93f818f5f02c90181bcbf', '1eb9f15109d83dc397b05cfe8a8bba47', '4ec2530f0dff394b9eba908b0b4bccfd', 0);

-- ----------------------------
-- Table structure for tb_way
-- ----------------------------
DROP TABLE IF EXISTS `tb_way`;
CREATE TABLE `tb_way`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'id',
  `name` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '名称',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tb_withdraw
-- ----------------------------
DROP TABLE IF EXISTS `tb_withdraw`;
CREATE TABLE `tb_withdraw`  (
  `id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'id',
  `withdraw_time` datetime(0) NOT NULL COMMENT '提现时间',
  `withdraw_amount` int(64) NOT NULL COMMENT '提现金额（单位：分）',
  `user_id` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '提现账户',
  `remarks` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  `state` int(11) NOT NULL COMMENT '状态（0：未完成，仅提交申请；1：已完成提现；2：取消）',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `withuser`(`user_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tb_withdraw
-- ----------------------------
INSERT INTO `tb_withdraw` VALUES ('050ae404a1ca4181878399972d39bd57', '0001-01-01 00:00:00', 1, '912e1d94e54e4bb9b4557803ab63e970', NULL, 0);
INSERT INTO `tb_withdraw` VALUES ('0626f15a01264d199ea009e9bfea8970', '2019-04-04 11:06:20', 1, '912e1d94e54e4bb9b4557803ab63e970', NULL, 0);
INSERT INTO `tb_withdraw` VALUES ('0f20096762b84deb915d97b4b343a9f8', '0001-01-01 00:00:00', 1, '912e1d94e54e4bb9b4557803ab63e970', NULL, 0);
INSERT INTO `tb_withdraw` VALUES ('1267efdd1fbc4aada5bc3e29b1046d47', '0001-01-01 00:00:00', 5000, '912e1d94e54e4bb9b4557803ab63e970', NULL, 0);
INSERT INTO `tb_withdraw` VALUES ('21edc02bd9a34b75ae97c079edc54ca6', '2019-04-04 11:06:23', 1, '912e1d94e54e4bb9b4557803ab63e970', NULL, 0);
INSERT INTO `tb_withdraw` VALUES ('24ab8b2ed93a4ec7b12db144808f442b', '2019-04-04 11:06:26', 1, '912e1d94e54e4bb9b4557803ab63e970', NULL, 0);
INSERT INTO `tb_withdraw` VALUES ('27ba6fcd108d437087cb622c56293e4a', '2019-04-04 11:06:29', 0, '912e1d94e54e4bb9b4557803ab63e970', NULL, 0);
INSERT INTO `tb_withdraw` VALUES ('28e87621b2744d07840943592d7f0c76', '0001-01-01 00:00:00', 1, '912e1d94e54e4bb9b4557803ab63e970', NULL, 0);
INSERT INTO `tb_withdraw` VALUES ('3b7376894dbc48058b74f481ebfd95b9', '2019-04-04 11:06:34', 8, '912e1d94e54e4bb9b4557803ab63e970', NULL, 0);
INSERT INTO `tb_withdraw` VALUES ('8ca6fec717134505b2bea722c9d03dd2', '0001-01-01 00:00:00', 1, '912e1d94e54e4bb9b4557803ab63e970', NULL, 0);
INSERT INTO `tb_withdraw` VALUES ('9021a48bd2054cac9c014449ba365d03', '2019-04-04 11:06:38', 0, '912e1d94e54e4bb9b4557803ab63e970', NULL, 0);
INSERT INTO `tb_withdraw` VALUES ('a2eee544e6a24226b0f0f854aca47885', '2019-04-04 11:06:40', 1, '912e1d94e54e4bb9b4557803ab63e970', NULL, 1);
INSERT INTO `tb_withdraw` VALUES ('c66094bdf81c4a518bf4379919b02b9d', '0001-01-01 00:00:00', 1, '912e1d94e54e4bb9b4557803ab63e970', NULL, 0);
INSERT INTO `tb_withdraw` VALUES ('e7af998db3a74e4d83c77a25c8312a2b', '0001-01-01 00:00:00', 1, '912e1d94e54e4bb9b4557803ab63e970', NULL, 0);
INSERT INTO `tb_withdraw` VALUES ('f0fde2f820ba4fa7b3bf4dc44ceb1c3d', '2019-04-04 11:06:44', 100, '912e1d94e54e4bb9b4557803ab63e970', NULL, 1);

-- ----------------------------
-- Table structure for test
-- ----------------------------
DROP TABLE IF EXISTS `test`;
CREATE TABLE `test`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `birthday` datetime(0) NULL DEFAULT NULL,
  `income` double NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 72 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of test
-- ----------------------------
INSERT INTO `test` VALUES (1, 'zzx', NULL, 10001);
INSERT INTO `test` VALUES (2, 'zzx', NULL, 10001);
INSERT INTO `test` VALUES (3, 'zzx', NULL, 10001);
INSERT INTO `test` VALUES (4, 'zzx', NULL, 10001);
INSERT INTO `test` VALUES (5, 'zzx', NULL, 10001);
INSERT INTO `test` VALUES (6, 'zzx', NULL, 10001);
INSERT INTO `test` VALUES (7, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (8, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (9, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (10, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (11, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (12, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (13, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (14, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (15, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (16, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (17, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (18, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (19, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (20, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (21, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (22, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (23, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (24, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (25, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (26, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (27, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (28, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (29, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (30, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (31, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (32, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (33, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (34, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (35, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (36, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (37, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (38, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (39, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (40, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (41, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (42, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (43, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (44, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (45, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (46, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (47, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (48, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (49, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (50, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (51, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (52, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (53, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (54, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (55, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (56, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (57, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (58, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (59, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (60, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (61, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (62, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (63, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (64, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (65, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (66, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (67, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (68, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (69, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (70, 'zzx', NULL, 10000.88);
INSERT INTO `test` VALUES (71, 'zzx', NULL, 10000.88);

SET FOREIGN_KEY_CHECKS = 1;
