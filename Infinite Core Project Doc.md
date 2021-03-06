
# Infinity Core

**介绍**

5v5玩家对战，每个玩家的可以选择不同定位的角色，不同定位的角色有不同的数个技能，玩家可以根据需求提升技能等级。

## 规则

玩家需要在本阵营区域内击杀野怪，掉落金币和经验值或装备，搜刮物资，以此为角色提升等级和属性，并升级技能

- START - 4Min

巨大的城墙[墙名：空集EmptySet]尚未开启，将两方玩家的区域分隔开，两方玩家无法互相接触

- 4Min - 8Min

比赛开始4分钟后，EmptySet开放，高等级区域解锁，高等级区域的城墙留下红蓝两种颜色，只有对应颜色阵营的玩家可以进入到新的区域，蓝方的高等级地区在右上方，红方的高等级区域在左下方

- 8Min - 10Min

无限核心区域开放，无限核心显现并附带高额生命值的护盾，但无限核心毁灭倒计时尚未开始，2分钟内将护盾击破的阵营将获得3分钟的最终伤害+10%加成BUFF

无限核心在战斗期间都会间隔释放AOE攻击范围内的所有玩家，脱战后护盾将会自动回血

无限核心护盾被击破后，将直接进入下一阶段

- 8~10Min[护盾被击破] - 15Min / 10Min[护盾未击破] - 15Min

无限核心暴露，将会在区域内5个固定位置随机转移，30s转移一次，直至比赛进行到15分钟整之前，向无限核心倾注所有火力，15分钟整时，会计算红方与蓝方所造成的伤害，向造成伤害最高的阵营给予BUFF，持续时间无限。

1. 造成伤害最高的一方将获得20%的最终伤害增加

2. 造成击杀无限核心的一方将额外获得10%的最终伤害增加，10%的移动速度增加

- 15Min - 16Min

无限核心被毁灭，此时玩家死亡后无法复活，在复活倒计时中的玩家直接死亡

- 16Min - 19.5Min

一个圆形侵蚀立场显现，以地图中心为圆心，半径逐渐缩小，直至将核心区域之外全部覆盖，处于侵蚀区域内的玩家将每秒损失最大生命值的5%

- 19.5Min - 20Min[TheEnd]

全场玩家将每秒损失最大生命值的5%，此时玩家生命回复能力为0

## 地图

正方形地图，左上至右下的对角线城墙，以正方形中心为原点，有两个半径不同的圆形城墙，大圆外为低等级区域，大圆内以及小圆外为高等级区域，小圆内为无限核心区域

蓝方在左下，红方在右上，复活点附近可以迅速回复生命值

高、低等级区域又分为红方、蓝方通行

## 野怪

击杀野怪将有概率随机掉落**一件**不同品质的装备，品质越高概率越低

无限核心毁灭后，野怪也无法复活，野怪同样将受到侵蚀效果的伤害且为每秒20%的最大血量

### 低等级区域

#### 低等级区域怪物信息

野怪类型|普通|神器|史诗|奇特|金币|复活时间|经验值
-|-|-|-|-|-|-|-
巡逻野怪|50%|25%|5%|0.5%|15~25|20s|300~500
宝箱守卫杂兵|30%|10%|3%|0.3%|10~30|首领死亡后60s|
宝箱守卫首领|10%|50%|20%|1%|50~130|60s|
光环附体怪物|0%|0%|100%|0%|100|60s间隔进行附加|

#### 机制

- ##### 史诗光环

每隔1分钟随机为两只区域内的野怪附加史诗光环，击杀附带史诗光环的野怪时必定掉落史诗品质装备，区域内上限4个史诗光环野怪

- ##### 宝箱守卫

有一群怪物看守的宝箱区域，守卫怪物未消灭完全时宝箱无法打开，宝箱必定产出神器或史诗品质装备

### 高等级区域

#### 高等级区域怪物信息

野怪类型|普通|神器|史诗|奇特|金币|复活时间|经验值
-|-|-|-|-|-|-|-
强力野怪|0%|30%|60%|5%|100~200|60s|??
世界BOSS|0%|0%|0%|100%|200~250|120s|??

#### 机制

- ##### 世界BOSS

地图中有4个世界BOSS区域，圆形石壁分割两边，分别位于高等级区域的左上、右上、左下、右下，其中左上和右下的开口朝向无限核心方向，且开口前方一段距离后便是连接着无限核心区域的延伸墙壁。左下和右上的开口朝向阵营复活点方向

击杀世界BOSS必定掉落奇特品质装备

- ##### 强力野怪

该区域内经常出现的单独个体，击杀后大概率掉落史诗装备

## 无限核心

位于地图正中心，HP不会回复，交战状态下每隔一段时间向周围释放一段AOE攻击，会优先攻击2秒内对核心造成最高单次伤害的玩家

AOE伤害为2000，间隔8秒，单体攻击一次伤害为500，2秒间隔，发射后必中

---

## 角色

### 定位

- 进攻型
    - 近距离攻击型角色
    - 远距离攻击型角色

- 辅助型
    - 回复型辅助角色

- 防御型
    - 护盾型角色

### 角色升级

每升一级将获得一次技能点数，技能点数可以给Q,E,R技能升一次级

---

### 击杀玩家

击杀玩家时，击杀者获得击杀奖励，即金币和大量经验值，其他同阵营玩家都将奖励50金币和少量经验值

本游戏没有助攻机制，只有击杀者能获取击杀奖励

击杀奖励获得的金币由每个玩家的价值计算器决定，经验值则根据被击杀玩家的等级按特定算法计算

#### 价值计算器[取消]

玩家0杀敌，0死亡时初始价值300金币，每杀敌一次增加100金币价值，每死亡一次减少50金币价值，最低为50金币价值，当玩家连续击杀7次敌人且途中未死亡时，该玩家将被悬赏，击杀被悬赏的玩家将额外获得500金币，其他同阵营玩家都将获得100金币

---

### 玩家复活

所有玩家初始复活时间为10s，每升级1次增加5s，最大为80s (10 + 5*14 = 80)

### 角色基础属性

类型|基础|每级提升|满级+装备主词条理论最大值
-|-|-|-
攻击|100|100|1500+2000=3500
生命|1000|1000|15000+15000=30000
护盾|0|0|0
暴击|5%|0%|5%+45%=50%
爆伤|50%|0%|50%+100%=150%
防御|1000|0|1000+9000=10000
攻速|100%|0|100%+15%=115%
生命恢复|10|5|80+0=80

### 角色负面状态

- 定身
- 迟缓
- 流血
- 沉默
- 侵蚀[系统BUFF]

### 角色爆发[Space]

- ~~瞬移[类似闪现]~~
- 超速[短时间内增加移动速度]
- 回复[一段时间内回复自身血量]
- 斩杀[一定范围内血量低于其敌人自身最大生命值5%的敌人被立即击杀]

---

### 伤害计算算法

**暴击率写作 50%，实际中数值为0.5**

- 无暴击总伤害=[(攻击 * 0.5 * 技能倍率) * (1 - 防御*0.00001)]*最终伤害
> [3500 * 0.5 * 300% * (1- 10000 * 0.00001)] * 1
750 * 3 * 0.9 
= 2025

1级 无装备 不暴击 100攻击

> [100 * 0.5 * 100% * (1 - 1000 * 0.00001)] * 1
50 * 0.99 = 45

- 暴击总伤害=(攻击 * 0.5 * 技能倍率 * 爆伤) * (1 - 防御*0.00001)*最终伤害
> [3500 * 0.5 * 300% * (1 + 150%) * (1- 10000 * 0.00001)] * 1
750 * 3 * 2.5 * 0.9
= 5062.5

1级 无装备 暴击 100攻击

> [100 * 0.5 * (1 + 50%) * 100% * (1 - 1000 * 0.00001)] * 1
75 * 0.99 = 74.25

伤害显示为整数，但保留完整数值

### 技能[Q、E、R]

。。。

---

## 装备

### 说明

装备分为4个品质等级：普通Normal[蓝]、神器Artifact[紫]、史诗Epic[黄]、奇特Strange[红]

普通、神器、史诗装备可以通过野怪掉落，但奇特装备只能通过击杀世界BOSS获得，并且造成了击杀世界BOSS的阵营中参与战斗的玩家，必定获得一件奇特装备

奇特装备只能穿戴1件，没有套装效果，但会拥有一个其他装备都不具有的限定属性

装备套装效果分为1件、2件、3件，同系列装备穿戴数量影响套装效果的触发，套装效果将影响到：

- 持之以恒[普攻倍率增加]Preserve
- 天赋异禀[技能倍率增加]TheGifted
- 虚晃一枪[技能释放后有一定概率无冷却时间]FeintShot
- 心急如焚[技能冷却时间减少]Worried
- 嗜血狂魔[根据造成的伤害按百分比获取生命]Bloodthirsty
- 狂战士[生命越少，攻速越快，暴击爆伤增加]Berserker
- 固若金汤[根据防御生成护盾，攻击下降，护盾被破坏后60秒后才能生成，造成伤害时回复损失的护盾值，护盾被破坏时对周围造成一次伤害]Impregnable

> 代码：施放方，受击方要能够快速获取

- 涌泉相报[根据受到伤害返还给敌人]YongQuanXiangBao
- 赏金猎人[该角色所持金币越多，攻击力增加，移速增加]BountyHunter
- 生命源泉[生命恢复增加]Lifespring
- 狂暴轰入[攻速增加]CrazyAttack

不同品质的同系列装备套装效果都相同，即普通2件装备与1件史诗装备若为同系列装备，则同样可以触发该系列的套装效果

每个玩家的掉落装备类型根据玩家角色定位决定，即战士角色刷野怪时不会掉落法师使用的装备

每个玩家的可拾取装备无上限，金币用于重置装备词条

### 装备主词条

> 装备自带范围内随机属性，且提供类似于套装效果的词条增益，共6种部位，每种部位都有一个主属性

部位：头盔(暴击) 护甲(防御) 护手(攻击) 护膝(爆伤) 护腿(生命) 鞋子(攻速)

#### 参考范围

普通范围：1000~5000

神器范围：2000~5000

史诗范围：3000~5000

奇特范围：3000~5000

#### 装备主词条类型

类型|普通|神器|史诗&奇特|最大值
-|-|-|-|-
攻击|500|750|1000|2000
生命|5000|6500|8000|15000
暴击|5%|10%|15%|45%
爆伤|15%|30%|45%|100%
防御|2000|3000|4000|9000
攻速|3%|6%|9%|15%

### 装备副词条

普通有1个随机词条，神器2个随机词条，史诗3个随机词条

种类：Q技能Lv+1,E技能Lv+1,R技能Lv+1

---

## 商人

> 所有BUFF，和所有操作都要有开关（生效开关控制器）

### 商人区域

玩家处于其中时受到玩家攻击时无敌，但无法免疫无限核心被破坏后的侵蚀效果

### 商人操作

**洗刷**

花费50金币可以洗刷一次单个词条

**增幅**

花费300金币可以提升10%的单个词条属性数值

**移植**

花费300金币可以将其他装备的一个词条转移到特定装备上，被移植的装备无法移植第二个词条，只能再次移植同一位置的词条