<!--
 * @Author: your name
 * @Date: 2022-03-11 00:39:53
 * @LastEditTime: 2022-03-11 14:58:38
 * @LastEditors: your name
 * @Description: 打开koroFileHeader查看配置 进行设置: https://github.com/OBKoro1/koro1FileHeader/wiki/%E9%85%8D%E7%BD%AE
 * @FilePath: \undefinedf:\_HomeWorks\Infinite Core\README.md
-->
# Infinite Core

毕业设计

# DevNote

## To Do



玩家模型默认没有挂载CharBase,而是挂载于该玩家的摄像机上,生成玩家时只生成模型为摄像机的子物体,玩家摄像机身上的CharBase每秒或某些事件时会自动更新到Manager的List中


玩家State控制器
玩家自动回血
玩家第一次生成和复活流程

---
### 优先级低

摄像机控制
玩家移动控制
DebugConsole完善

## Done

### 2022.3.11

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