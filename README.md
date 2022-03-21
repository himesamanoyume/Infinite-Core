<!--
 * @Author: your name
 * @Date: 2022-03-11 00:39:53
 * @LastEditTime: 2022-03-21 14:09:40
 * @LastEditors: Please set LastEditors
 * @Description: 打开koroFileHeader查看配置 进行设置: https://github.com/OBKoro1/koro1FileHeader/wiki/%E9%85%8D%E7%BD%AE
 * @FilePath: \undefinedf:\_HomeWorks\Infinite Core\README.md
-->
# Infinite Core

毕业设计

5v5玩家对战，每个玩家的可以选择不同定位的角色，不同定位的角色有不同的数个技能，玩家可以根据需求提升技能等级。

# DevNote

## To Do

### 高优先级

新增玩家在房间选择生成设置(PUN的实例化只写一次调用似乎会自动多次调用，因此之前的生成逻辑必须更改)

修复玩家第一次生成位置

修复其他玩家无法绑定父物体

在房间处就生成玩家模型/生成不被破坏的空物体用于记录玩家的职业技能选择

---

总控制器添加开局计时器 复活时间改为受时长影响

装备血量主词条只影响玩家最大生命值

### 优先级低

DebugConsole完善

动画控制器记得加入photonView.IsMine的判断

## Done

### 2022.3.21

重写房间逻辑(1/2)

### 2022.3.20

玩家复活逻辑修复

Launcher修复

CharManager的List已修复

玩家位置并未同步 *

摄像机错乱绑定玩家 *

只有某个玩家能够移动 *

*：将photonView的followOnStart取消勾选即可,只在单机测试时勾选,打包发布时取消

### 2022.3.19

联机改造实现进度(1/3)

### 2022.3.16

摄像机控制

添加玩家控制

### 2022.3.13

添加了好几种函数 基本完成了玩家属性的基本函数 为之后的装备系统作好准备

给予玩家护盾函数

给予玩家health函数

### 2022.3.12

调整restore数值

给予玩家money函数

给予玩家经验函数

DebugConsole新增money经验的对应函数

### 2022.3.11

玩家State控制器

玩家复活流程

玩家自动回血

玩家第一次生成

重写CharManager

重写SpawnPlayer

重写CharSpawnController

玩家生成

快速方便赋值

### 2022.3.10

重写了CharBase类

### 2022.3.9

控制台减少玩家血量函数

玩家等级修改

玩家升级后属性发生变化

DebugConsole Text大小

DebugConsole全分辨率适配