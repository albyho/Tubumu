CREATE TABLE `tubumu`.`group` (
  `group_id` char(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '分组 Id',
  `parent_id` char(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL COMMENT '父 Id',
  `name` NVARCHAR(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '名称',
  `level` INT NOT NULL COMMENT '层级',
  `display_order` INT NOT NULL COMMENT '显示顺序',
  `is_contains_user` TINYINT(1) NOT NULL COMMENT '是否包含用户',
  `is_disabled` TINYINT(1) NOT NULL COMMENT '是否停用',
  `is_system` TINYINT(1) NOT NULL COMMENT '是否是系统分组',
  PRIMARY KEY (`group_id`)),
  INDEX `idx_display_order` (`display_order` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COMMENT = '分组';

ALTER TABLE `tubumu`.`group` 
ADD FOREIGN KEY (`parent_id`) REFERENCES `tubumu`.`group` (`group_id`),
ADD INDEX `display_order`(`display_order`) USING BTREE;

