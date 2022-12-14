# CardEssential
A mod for Card Survival Tropical Island. You can view and modify game stats by this mod.

卡牌本质 - CardEssential v0.0.1 by MyPure

- 简要
卡牌本质是一个基础工具类Mod，提供调整游戏内容的基础功能。目前仅有查看游戏数值（Stat）的功能。
你可以通过本Mod查看游戏内查看与修改所有内在数值，例如人物状态、种群密度、植物数量等。后面会持续更新与添加功能！
注：Mod会影响游戏平衡，请慎重使用！

- 依赖
BepInEx 5
https://github.com/BepInEx/BepInEx
UniverseLib 1.5.1
https://github.com/sinai-dev/UniverseLib

- 安装方法
把CardEssential.dll与UniverseLib.Mono.dll放入游戏根目录\BepInEx\plugins下。如果你没有这个目录，你需要先安装BepInEx 5。
https://docs.qq.com/doc/DRk9QcHltUW1WY1lv?&u=adcc1c3efcc7402e8f011588a1460648

- 使用方法
进入游戏后，按键盘快捷键Ctrl + F开启与关闭页面。

- 详细说明
工具列出了游戏内数值构成，分为Value和Rate两种类型。
Value即当前状态的数值。
Rate指的是当前状态在下一个Tick会增减的数值。在这个游戏中，一个Tick对应游戏中15分钟。
每种类型又有两种数据，Base和Modifiers。
Base指的是当前状态的基础数值。
Modifiers指的是当前数值的影响量，这个影响会附加在Base上。
	（还有一个AtBase，目前不知道干啥用的，如果非0也会显示出来）
