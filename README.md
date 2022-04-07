
# Infinite Core

毕业设计

5v5玩家对战，每个玩家的可以选择不同定位的角色，不同定位的角色有不同的数个技能，玩家可以根据需求提升技能等级。

# DevNote

## To Do

### 高优先级

CharManager,CharBase补完注册订阅事件

开始实际运用事件系统

---

监听CharBase值变化

比赛内逻辑重做

异步加载地图 全部玩家加载完后再跳转

#### bug

离开房间时会回到连接界面而不是主界面

### 优先级低

玩家退出时用PUN销毁物体

总控制器添加开局计时器 复活时间改为受时长影响

装备血量主词条只影响玩家最大生命值

DebugConsole 改版

动画控制器记得加入photonView.IsMine的判断

## Tips

该事件系统中 RootGroup必须enable为true，且只能在GameEventManager中使用enable修改， 无法使用EnableAllEvents或EnableEvent对EventName.rootGroup进行操作，必须从RootGroup下的第一级事件组当中进行操作 如EventName.playerGroup  

技能碰撞限制

项目命名空间无法识别时直接到Unity 的Edit-> Perferences-> External Tools-> Regenarate project files

以后我再也不要在代码里除字符串和注释以外的任何地方写中文了

如果玩家阵营信息逻辑太复杂可以让玩家条目挂载CharBase 将信息全部赋值到CharBase上 再直接取值作判断

进房间时通过PlayerList查找每一个玩家的阵营 根据阵营填充背景颜色 

完善房间进出时回调的字典控制

房间内玩家需要准备才可开始

红方玩家材质都为红色 蓝方玩家材质都为蓝色

玩家在房间内选择好自己的职业 每个职业的技能是固定的那3个

如果主机切换 则需要新的主机负责生成野怪(AsteroidsGameManager.cs 131)

倒计时显示XX.00位方法(AsteroidsGameManager.cs 100)

当玩家属性更新时(AsteroidsGameManager.cs 148)

检测玩家是否都加载完关卡(AsteroidsGameManager.cs 223)

玩家在房间选择好属性 设置哈希表 全部准备完毕后才能开始 (PlayerListEntry.cs 39)

快捷方便的开关UI面板方法(LobbyMainPanel.cs 336)

加分改属性示例(PunPlayerScores.cs 45)

如果PhotonNetwork的引用出毛病了就备份一下重要文件 然后Package Manager里重装即可

## Done

### 2022.4.7

重写CharBase(1.8/2) 肢解CharStateController

重写CharManager(1.5/2)

重写事件系统

---

重写了玩家基本逻辑

### 2022.4.6

重写了玩家生成逻辑

添加了一些事件但还未使用

---

重写CharManager、CharSpawnController

### 2022.4.5

好！能同步！(通过DebugConsole测试是否数据同步)

---

初步实现同步玩家的CharBase

### 2022.4.4

重写玩家开局部分逻辑

---

偷学了一个事件系统 开始消化

### 2022.4.3

玩家全部准备并且全部选择阵营时房主可以开始游戏

### 2022.4.2

修复阵营数检测bug

---

修复玩家加入房间时房主的bug

新增连接主界面 只有标题和连接功能 PlayButtom分离出单独的加入房间

新增玩家在房间选择生成设置(2/2)

### 2022.4.1

小优化UI

技能系统(1/10)

重做枚举(1.8/2)

### 2022.3.30

新增玩家在房间选择生成设置(1/2)

### 2022.3.29

重做枚举(1/2)

UI改善以及新UI

### 2022.3.28

修复选择阵营后 红蓝阵营计数器赋值错误

修复选择阵营后 脚本上的值没变

修复退出房间后再进入 自己的isReady没变

修复退出房间后再进入 自己的背景颜色没变

修复有玩家退出之后再进入新玩家时 之前玩家的阵营颜色不会改变

修复其他玩家改变准备状态后 本地玩家看过去的isReady状态不会改变

---

重写房间逻辑(1.5/2)

### 2022.3.27

并且为区分红蓝方做颜色背景 Tips[快捷方便的开关UI面板方法]

---

改善房间UI控制器

别人准备时也将同步别人的已准备图标

### 2022.3.26

学习其Demo(3/3)

### 2022.3.25

学习其Demo(2.8/3)

### 2022.3.23

为了更好的学习PUN插件 正在学习其Demo(1/3)

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